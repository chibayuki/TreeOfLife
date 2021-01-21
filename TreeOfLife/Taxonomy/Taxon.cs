/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.900.1000.M9.210112-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

#define BAN_MULTI_IN_DEGREE // 禁止复数入度，即至多被一个并系群排除、至多被一个复系群包含

// 数据结构概述：
// 生物分类单元（类群）的数据结构是加权有向图，每个类群是图的一个节点，每个节点存储双向引用关系，
// 权重是单系群、并系群、复系群的抽象，目前没有具体的数值表示，不同权重、不同方向的引用关系由不同的容器分别存储，
// 单系群入度为 0..1（仅顶级类群为 0），入节点引用由 _Parent 对象表示，出度为 0..N，出节点引用由 _Children 集合表示，
// 并系群、复系群出度均为 0..N，入度取决于上述宏 BAN_MULTI_IN_DEGREE，若禁止复数入度，则入度为 0..1，否则为 0..N，
// 并系群入节点引用由 _ExcludeBy 集合表示，出节点引用由 _Excludes 集合表示，
// 复系群入节点引用由 _IncludeBy 集合表示，出节点引用由 _Includes 集合表示，
// 另，支持并系群、复系群之前的版本的数据结构是N叉树。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife.Taxonomy
{
    // 生物分类单元（类群）。
    public class Taxon
    {
        private string _BotanicalName = string.Empty; // 学名。
        private string _ChineseName = string.Empty; // 中文名。
        private List<string> _Synonyms = new List<string>(); // 异名、别名、旧名等。
        private List<string> _Tags = new List<string>(); // 标签。
        private string _Description = string.Empty; // 描述。

        private TaxonomicCategory _Category = TaxonomicCategory.Unranked; // 分类阶元。

        private bool _IsExtinct = false; // 已灭绝。
        private bool _IsUnsure = false; // 存疑。

        private Taxon _Parent = null; // 父类群。
        private List<Taxon> _Children = new List<Taxon>(); // 子类群。

        private int _Level = 0; // 当前类群与顶级类群的距离。
        private int _Index = -1; // 当前类群在姊妹类群中的次序。

        //

        private List<Taxon> _ExcludeBy = new List<Taxon>(); // 排除当前类群的并系群。
        private List<Taxon> _Excludes = new List<Taxon>(); // 当前类群作为并系群排除的类群。

        private List<Taxon> _IncludeBy = new List<Taxon>(); // 包含当前类群的复系群。
        private List<Taxon> _Includes = new List<Taxon>(); // 当前类群作为复系群包含的类群。

        //

        public Taxon()
        {
        }

        //

        public string BotanicalName
        {
            // 确保不为 null 或 WhiteSpace
            get => (_BotanicalName ?? string.Empty);
            set => _BotanicalName = (value?.Trim() ?? string.Empty);
        }

        public string ChineseName
        {
            // 确保不为 null 或 WhiteSpace
            get => (_ChineseName ?? string.Empty);
            set => _ChineseName = (value?.Trim() ?? string.Empty);
        }

        public List<string> Synonyms => _Synonyms;

        public List<string> Tags => _Tags;

        public string Description
        {
            get => _Description;
            set => _Description = value;
        }

        public TaxonomicCategory Category
        {
            get => _Category;
            set => _Category = value;
        }

        public bool IsExtinct
        {
            get => _IsExtinct;
            set => _IsExtinct = value;
        }

        public bool IsUnsure
        {
            get => _IsUnsure;
            set => _IsUnsure = value;
        }

        public Taxon Parent => _Parent;

        public IReadOnlyList<Taxon> Children => _Children;

        // 获取当前类群与顶级类群的距离。
        public int Level => _Level;

        // 获取当前类群在姊妹类群中的次序。
        public int Index => _Index;

        //

        // 获取当前类群的顶级父类群。
        public Taxon Root
        {
            get
            {
                Taxon parent = this;
                Taxon grandParent = parent.Parent;

                while (grandParent != null)
                {
                    parent = grandParent;
                    grandParent = parent.Parent;
                }

                return parent;
            }
        }

        // 判断当前类群是否为顶级类群。
        public bool IsRoot => (_Parent == null);

        // 判断当前类群是否为末端类群。
        public bool IsFinal => (_Children.Count <= 0);

        // 判断当前类群是否继承自指定类群。
        public bool InheritFrom(Taxon taxon)
        {
            // 任何类群都不继承 null（即使父类群是 null）
            if (_Parent == null || taxon == null)
            {
                return false;
            }
            // 任何类群都继承自身
            else if (this == taxon)
            {
                return true;
            }
            else
            {
                // 不可能继承层次更深的类群
                if (_Level <= taxon._Level)
                {
                    return false;
                }
                else
                {
                    Taxon t = this;

                    // 层次相等之后没有必要继续判断
                    while (t != null && t._Level >= taxon._Level)
                    {
                        if (t == taxon)
                        {
                            return true;
                        }

                        t = t._Parent;
                    }

                    return false;
                }
            }
        }

        //

        public IReadOnlyList<Taxon> ExcludeBy => _ExcludeBy;

        public IReadOnlyList<Taxon> Excludes => _Excludes;

        public IReadOnlyList<Taxon> IncludeBy => _IncludeBy;

        public IReadOnlyList<Taxon> Includes => _Includes;

        // 判断当前类群是否为单系群。
        public bool IsMonophyly => (_Includes.Count <= 0 && _Excludes.Count <= 0);

        // 判断当前类群是否为并系群。
        public bool IsParaphyly => (_Includes.Count <= 0 && _Excludes.Count > 0);

        // 判断当前类群是否为复系群。
        public bool IsPolyphyly => (_Includes.Count > 0);

        //

#if DEBUG

        public override string ToString()
        {
            StringBuilder taxonName = new StringBuilder();

            if (_IsUnsure || _IsExtinct)
            {
                if (_IsUnsure)
                {
                    taxonName.Append('?');
                }

                if (_IsExtinct)
                {
                    taxonName.Append('†');
                }

                taxonName.Append(' ');
            }

            if (!string.IsNullOrEmpty(_ChineseName))
            {
                taxonName.Append(_ChineseName);

                if (!string.IsNullOrEmpty(_BotanicalName))
                {
                    taxonName.Append(' ');
                    taxonName.Append(_BotanicalName);
                }
            }
            else if (!string.IsNullOrEmpty(_BotanicalName))
            {
                taxonName.Append(_BotanicalName);
            }

            return taxonName.ToString();
        }

#endif

        //

        // 递归地修复当前类群、子类群与顶级类群的距离。
        private void _RepairLevel()
        {
            if (_Parent == null)
            {
                _Level = 0;
            }
            else
            {
                _Level = _Parent._Level + 1;
            }

            foreach (Taxon child in _Children)
            {
                child._RepairLevel();
            }
        }

        // 修复子类群的次序。
        private void _RepairIndex()
        {
            for (int i = 0; i < _Children.Count; i++)
            {
                _Children[i]._Index = i;
            }
        }

        // 原子地附属到父类群。
        private void _AtomAttachParent(Taxon taxon)
        {
            if (taxon != null)
            {
                _Parent = taxon;
                _Parent._Children.Add(this);
            }
        }

        // 原子地附属到父类群。
        private void _AtomAttachParent(Taxon taxon, int index)
        {
            if (taxon != null)
            {
                _Parent = taxon;
                _Parent._Children.Insert(index, this);
            }
        }

        // 原子地脱离父类群。
        private void _AtomDetachParent()
        {
            if (_Parent != null)
            {
                _Parent._Children.Remove(this);
                _Parent = null;
            }
        }

        // 判断当前类群是否可以变更父类群为指定的类群。
        public bool CanSetParent(Taxon taxon)
        {
            // （1） 不能继承 null
            // （2） 不能继承父类群（因为已经继承）
            // （3） 不能继承子类群
            if (taxon == null || taxon == _Parent || taxon.InheritFrom(this))
            {
                return false;
            }
            else
            {
                // 变更继承关系后，任何并系群不能出现（参见 CanAddExclude）：
                // （4） 排除不继承自身的类群
                //    -> 意味着：该类群被并系群排除，试图继承一个类群，但试图继承的类群不继承任一并系群
                foreach (var excludeBy in _ExcludeBy)
                {
                    if (!taxon.InheritFrom(excludeBy))
                    {
                        return false;
                    }
                }

                // （5） 排除与已排除的类群具有继承关系的类群
                //    -> 意味着：该类群被并系群排除，试图继承一个类群，但试图继承的类群（或其父类群）也被任一并系群排除
                foreach (var excludeBy in _ExcludeBy)
                {
                    foreach (var exclude in excludeBy._Excludes)
                    {
                        if (taxon.InheritFrom(exclude))
                        {
                            return false;
                        }
                    }
                }

                // 任何复系群不能出现（参见 CanAddInclude）：
                // （6） 包含继承自身的类群
                //    -> 意味着：试图继承一个复系群，但该类群已经被该复系群包含
                if (taxon._Includes.Contains(this))
                {
                    return false;
                }

                // （7） 包含不继承自身父类群的类群
                //    -> 意味着：该类群被复系群包含，试图继承一个类群，但试图继承的类群不继承任一复系群的父类群
                foreach (var includeBy in _IncludeBy)
                {
                    if (!taxon.InheritFrom(includeBy._Parent))
                    {
                        return false;
                    }
                }

                // （8） 包含与已包含的类群具有继承关系的类群
                //    -> 意味着：该类群被复系群包含，试图继承一个类群，但试图继承的类群（或其父类群）也被任一复系群包含
                foreach (var includeBy in _IncludeBy)
                {
                    foreach (var include in includeBy._Includes)
                    {
                        if (taxon.InheritFrom(include))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        // 变更父类群。
        public void SetParent(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            if (!CanSetParent(taxon))
            {
                throw new InvalidOperationException();
            }

            //

            if (_Parent != null)
            {
                Taxon parent = _Parent;

                _AtomDetachParent();
                parent._RepairIndex();
            }

            _AtomAttachParent(taxon);
            taxon._RepairIndex();

            _RepairLevel();
        }

        // 变更父类群。
        public void SetParent(Taxon taxon, int index)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            if (index < 0 || index > taxon._Children.Count)
            {
                throw new IndexOutOfRangeException();
            }

            if (!CanSetParent(taxon))
            {
                throw new InvalidOperationException();
            }

            //

            if (_Parent != null)
            {
                Taxon parent = _Parent;

                _AtomDetachParent();
                parent._RepairIndex();
            }

            _AtomAttachParent(taxon, index);
            taxon._RepairIndex();

            _RepairLevel();
        }

        // 添加一个上层父类群。
        public Taxon AddParentUplevel()
        {
            Taxon taxon = new Taxon();

            if (_Parent != null)
            {
                Taxon parent = _Parent;
                int index = _Index;

                _AtomDetachParent();
                taxon._AtomAttachParent(parent, index);
                parent._RepairIndex();
            }

            _AtomAttachParent(taxon);
            taxon._RepairIndex();
            taxon._RepairLevel();

            return taxon;
        }

        // 添加一个下层父类群。
        public Taxon AddParentDownlevel()
        {
            Taxon taxon = new Taxon();

            foreach (var child in _Children)
            {
                child._AtomAttachParent(taxon);
            }

            _Children.Clear();

            taxon._AtomAttachParent(this);
            _RepairIndex();

            taxon._RepairIndex();
            taxon._RepairLevel();

            return taxon;
        }

        // 移动一个子类群。
        public void MoveChild(int oldIndex, int newIndex)
        {
            if (_Children.Count <= 0 || (oldIndex < 0 || oldIndex >= _Children.Count) || (newIndex < 0 || newIndex >= _Children.Count))
            {
                throw new IndexOutOfRangeException();
            }

            //

            if (oldIndex != newIndex)
            {
                Taxon child = _Children[oldIndex];

                _Children.RemoveAt(oldIndex);
                _Children.Insert(newIndex, child);

                _RepairIndex();
            }
        }

        // 交换两个子类群。
        public void SwapChild(int index1, int index2)
        {
            if (_Children.Count <= 0 || (index1 < 0 || index1 >= _Children.Count) || (index2 < 0 || index2 >= _Children.Count))
            {
                throw new IndexOutOfRangeException();
            }

            //

            if (index1 != index2)
            {
                Taxon child1 = _Children[index1];
                Taxon child2 = _Children[index2];

                _Children[index2] = child1;
                _Children[index1] = child2;

                child1._Index = index2;
                child2._Index = index1;
            }
        }

        // 添加一个子类群。
        public Taxon AddChild()
        {
            Taxon child = new Taxon();

            child._AtomAttachParent(this);

            _RepairIndex();
            child._RepairLevel();

            return child;
        }

        // 添加一个子类群。
        public Taxon AddChild(int index)
        {
            Taxon child = new Taxon();

            child._AtomAttachParent(this, index);

            _RepairIndex();
            child._RepairLevel();

            return child;
        }

        // 递归删除所有子类群。
        private void _RecursiveRemoveChildren()
        {
            for (int i = 0; i < _Children.Count; i++)
            {
                _Children[i]._RecursiveRemoveChildren();
            }

            // 直接将父类群设为 null，然后清空子类群即可，不能调用 _AtomDetachParent，会导致上面的循环失败
            _Parent = null;

            _Children.Clear();
        }

        // 删除当前类群（并且删除/保留所有子类群）。
        public void RemoveCurrent(bool removeChildren)
        {
            if (_Parent != null)
            {
                Taxon parent = _Parent;

                if (removeChildren)
                {
                    // 必须确实递归地删除所有子类群，从而避免例如从"搜索"页面跳转到已删除的类群、或者间接引用大量已删除类群的问题
                    _RecursiveRemoveChildren();

                    // 第一层递归已经将父类群设为 null，但没有从父类群的子类群中删除当前类群
                    parent._Children.Remove(this);
                }
                else if (_Children.Count <= 0)
                {
                    _AtomDetachParent();
                }
                else
                {
                    int index = _Index;

                    _AtomDetachParent();

                    for (int i = 0; i < _Children.Count; i++)
                    {
                        _Children[i]._AtomAttachParent(parent, index + i);
                        _Children[i]._RepairLevel();
                    }

                    _Children.Clear();
                }

                parent._RepairIndex();
            }
        }

        //

        // 判断当前类群是否可以作为并系群添加排除指定的类群。
        public bool CanAddExclude(Taxon taxon)
        {
            // （1） 不能排除 null
            // （2） 不能排除自身
            // （3） 不能排除已被任一并系群排除的类群（禁止复数入度）或不能多次排除同一个类群（允许复数入度）
            // （4） 不能排除不继承自身的类群
            if (taxon == null || taxon == this ||
#if BAN_MULTI_IN_DEGREE
                taxon._ExcludeBy.Count > 0
#else
                _Excludes.Contains(taxon)
#endif
                || !taxon.InheritFrom(this))
            {
                return false;
            }
            else
            {
                // （5） 不能排除与已排除的类群具有继承关系的类群
                foreach (var exclude in _Excludes)
                {
                    if (taxon.InheritFrom(exclude) || exclude.InheritFrom(taxon))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        // 当前类群作为并系群添加排除一个类群。
        public void AddExclude(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            if (!CanAddExclude(taxon))
            {
                throw new InvalidOperationException();
            }

            //

            _Excludes.Add(taxon);
            taxon._ExcludeBy.Add(this);
        }

        // 当前类群作为并系群添加排除一个类群。
        public void AddExclude(Taxon taxon, int index)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            if (index < 0 || index > taxon._Excludes.Count)
            {
                throw new IndexOutOfRangeException();
            }

            if (!CanAddExclude(taxon))
            {
                throw new InvalidOperationException();
            }

            //

            _Excludes.Insert(index, taxon);
            taxon._ExcludeBy.Add(this);
        }

        // 当前类群作为并系群删除排除一个类群。
        public void RemoveExclude(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            if (!_Excludes.Contains(taxon))
            {
                throw new InvalidOperationException();
            }

            //

            _Excludes.Remove(taxon);
            taxon._ExcludeBy.Remove(this);
        }

        // 获取当前类群作为并系群排除指定类群的次序。
        public int GetIndexOfExclude(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            return _Excludes.IndexOf(taxon);
        }

        // 当前类群作为并系群移动一个排除的类群。
        public void MoveExclude(int oldIndex, int newIndex)
        {
            if (_Excludes.Count <= 0 || (oldIndex < 0 || oldIndex >= _Excludes.Count) || (newIndex < 0 || newIndex >= _Excludes.Count))
            {
                throw new IndexOutOfRangeException();
            }

            //

            if (oldIndex != newIndex)
            {
                Taxon exclude = _Excludes[oldIndex];

                _Excludes.RemoveAt(oldIndex);
                _Excludes.Insert(newIndex, exclude);
            }
        }

        // 当前类群作为并系群交换两个排除的类群。
        public void SwapExclude(int index1, int index2)
        {
            if (_Excludes.Count <= 0 || (index1 < 0 || index1 >= _Excludes.Count) || (index2 < 0 || index2 >= _Excludes.Count))
            {
                throw new IndexOutOfRangeException();
            }

            //

            if (index1 != index2)
            {
                Taxon exclude1 = _Excludes[index1];
                Taxon exclude2 = _Excludes[index2];

                _Excludes[index2] = exclude1;
                _Excludes[index1] = exclude2;

                exclude1._Index = index2;
                exclude2._Index = index1;
            }
        }

        // 判断当前类群是否可以作为复系群添加包含指定的类群。
        public bool CanAddInclude(Taxon taxon)
        {
            // （1） 不能包含 null
            // （2） 不能包含自身
            // （3） 不能包含已被任一复系群包含的类群（禁止复数入度）或不能多次包含同一个类群（允许复数入度）
            // （4） 不能包含继承自身的类群
            // （5） 不能包含不继承自身父类群的类群
            if (taxon == null || taxon == this ||
#if BAN_MULTI_IN_DEGREE
                taxon._IncludeBy.Count > 0
#else
                _Includes.Contains(taxon)
#endif
                || taxon.InheritFrom(this) || !taxon.InheritFrom(_Parent))
            {
                return false;
            }
            else
            {
                // （6） 不能包含与已包含的类群具有继承关系的类群
                foreach (var include in _Includes)
                {
                    if (taxon.InheritFrom(include) || include.InheritFrom(taxon))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        // 当前类群作为复系群添加包含一个类群。
        public void AddInclude(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            if (!CanAddInclude(taxon))
            {
                throw new InvalidOperationException();
            }

            //

            _Includes.Add(taxon);
            taxon._IncludeBy.Add(this);
        }

        // 当前类群作为复系群添加包含一个类群。
        public void AddInclude(Taxon taxon, int index)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            if (index < 0 || index > taxon._Includes.Count)
            {
                throw new IndexOutOfRangeException();
            }

            if (!CanAddInclude(taxon))
            {
                throw new InvalidOperationException();
            }

            //

            _Includes.Insert(index, taxon);
            taxon._IncludeBy.Add(this);
        }

        // 当前类群作为复系群删除包含一个类群。
        public void RemoveInclude(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            if (!_Includes.Contains(taxon))
            {
                throw new InvalidOperationException();
            }

            //

            _Includes.Remove(taxon);
            taxon._IncludeBy.Remove(this);
        }

        // 获取当前类群作为复系群包含指定类群的次序。
        public int GetIndexOfInclude(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            return _Includes.IndexOf(taxon);
        }

        // 当前类群作为复系群移动一个包含的类群。
        public void MoveInclude(int oldIndex, int newIndex)
        {
            if (_Includes.Count <= 0 || (oldIndex < 0 || oldIndex >= _Includes.Count) || (newIndex < 0 || newIndex >= _Includes.Count))
            {
                throw new IndexOutOfRangeException();
            }

            //

            if (oldIndex != newIndex)
            {
                Taxon include = _Includes[oldIndex];

                _Includes.RemoveAt(oldIndex);
                _Includes.Insert(newIndex, include);
            }
        }

        // 当前类群作为复系群交换两个包含的类群。
        public void SwapInclude(int index1, int index2)
        {
            if (_Includes.Count <= 0 || (index1 < 0 || index1 >= _Includes.Count) || (index2 < 0 || index2 >= _Includes.Count))
            {
                throw new IndexOutOfRangeException();
            }

            //

            if (index1 != index2)
            {
                Taxon include1 = _Includes[index1];
                Taxon include2 = _Includes[index2];

                _Includes[index2] = include1;
                _Includes[index1] = include2;

                include1._Index = index2;
                include2._Index = index1;
            }
        }
    }
}