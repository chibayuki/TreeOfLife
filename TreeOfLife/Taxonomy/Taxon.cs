/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.323.1000.M4.201128-1700

This file is part of "生命树" (TreeOfLife)

"生命树" (TreeOfLife) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife
{
    // 生物分类单元的类型。
    internal enum TaxonType
    {
        Monophyly, // 单系群。
        Paraphyly, // 并系群。
        Polyphyly // 多系群（复系群）。
    }

    // 生物分类单元（类群）。
    internal class Taxon
    {
        private string _BotanicalName = string.Empty; // 学名。
        private string _ChineseName = string.Empty; // 中文名。
        private List<string> _Synonyms = new List<string>(); // 异名、别名、旧名等。
        private List<string> _Tags = new List<string>(); // 标签。
        private string _Description = string.Empty; // 描述。

        private TaxonomicCategory _Category = TaxonomicCategory.Unranked; // 分类阶元。
        private TaxonType _TaxonType = TaxonType.Monophyly; // 类型。

        private bool _IsExtinct = false; // 已灭绝。
        private bool _Unsure = false; // 存疑。

        private Taxon _Parent = null; // 父类群（祖先）。
        private List<Taxon> _Children = new List<Taxon>(); // 子类群（后代）。

        private int _Level = 0; // 当前类群与顶级类群的距离。
        private int _Index = -1; // 当前类群在姊妹类群中的次序。

        //

        public Taxon()
        {
        }

        //

        public string BotanicalName
        {
            get => _BotanicalName;
            set => _BotanicalName = value;
        }

        public string ChineseName
        {
            get => _ChineseName;
            set => _ChineseName = value;
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

        private TaxonType TaxonType
        {
            get => _TaxonType;
            set => _TaxonType = value;
        }

        public bool IsExtinct
        {
            get => _IsExtinct;
            set => _IsExtinct = value;
        }

        public bool Unsure
        {
            get => _Unsure;
            set => _Unsure = value;
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

                while (!(grandParent is null))
                {
                    parent = grandParent;
                    grandParent = parent.Parent;
                }

                return parent;
            }
        }

        // 判断当前类群是否为顶级类群。
        public bool IsRoot => (_Parent is null);

        // 判断当前类群是否继承自指定类群。
        public bool InheritFrom(Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (this == taxon)
            {
                return true;
            }
            else if (_Parent is null)
            {
                return false;
            }
            else
            {
                if (_Level <= taxon._Level)
                {
                    return false;
                }
                else
                {
                    Taxon t = this;

                    while (!(t is null) && t._Level >= taxon._Level)
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
            if (_Parent is null)
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
            if (!(taxon is null))
            {
                _Parent = taxon;
                _Parent._Children.Add(this);
            }
        }

        // 原子地附属到父类群。
        private void _AtomAttachParent(Taxon taxon, int index)
        {
            if (!(taxon is null))
            {
                _Parent = taxon;
                _Parent._Children.Insert(index, this);
            }
        }

        // 原子地脱离父类群。
        private void _AtomDetachParent()
        {
            if (!(_Parent is null))
            {
                _Parent._Children.Remove(this);
                _Parent = null;
            }
        }

        // 变更父类群。
        public void SetParent(Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            if (taxon.InheritFrom(this))
            {
                throw new InvalidOperationException();
            }

            //

            if (!(_Parent is null))
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
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            if (taxon.InheritFrom(this))
            {
                throw new InvalidOperationException();
            }

            //

            if (!(_Parent is null))
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

            if (!(_Parent is null))
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

        // 删除当前类群（并且删除/保留所有子类群）。
        public void RemoveCurrent(bool removeChildren)
        {
            if (!(_Parent is null))
            {
                Taxon parent = _Parent;

                if (removeChildren || _Children.Count <= 0)
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
                }

                _Children.Clear();

                parent._RepairIndex();
            }
        }
    }
}