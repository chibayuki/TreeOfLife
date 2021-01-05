/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.800.1000.M8.201231-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

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
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            if (this == taxon)
            {
                return true;
            }
            else if (_Parent == null)
            {
                return false;
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

        // 变更父类群。
        public void SetParent(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            if (taxon == _Parent || taxon.InheritFrom(this))
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

            if (taxon == _Parent || taxon.InheritFrom(this))
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

            // 直接将父类群设为 null 即可，调用_AtomDetachParent会导致递归失败
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
    }
}