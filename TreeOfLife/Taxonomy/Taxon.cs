/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.112.1000.M2.201110-2050

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
        private List<string> _Synonym = new List<string>(); // 异名、别名、旧名等。
        private List<string> _Tag = new List<string>(); // 标签。

        private TaxonomicCategory _Category = TaxonomicCategory.Unranked; // 分类阶元。
        private TaxonType _TaxonType = TaxonType.Monophyly; // 类型。

        private bool _IsExtinct = false; // 已灭绝。
        private bool _InDoubt = false; // 分类地位存疑。

        private Taxon _Parent = null; // 父类群（祖先）。
        private List<Taxon> _Children = new List<Taxon>(); // 子类群（后代）。

        private string _Comment = string.Empty; // 注释。
        private List<string> _Url = new List<string>(); // 链接。

        private int _Level = 0; // 当前类群与顶级类群的距离。

        //

        public Taxon()
        {
        }

        //

        public string BotanicalName
        {
            get
            {
                return _BotanicalName;
            }

            set
            {
                _BotanicalName = value;
            }
        }

        public string ChineseName
        {
            get
            {
                return _ChineseName;
            }

            set
            {
                _ChineseName = value;
            }
        }

        public List<string> Synonym => _Synonym;

        public List<string> Tag => _Tag;

        public TaxonomicCategory Category
        {
            get
            {
                return _Category;
            }

            set
            {
                _Category = value;
            }
        }

        public TaxonType TaxonType
        {
            get
            {
                return _TaxonType;
            }

            set
            {
                _TaxonType = value;
            }
        }

        public bool IsExtinct
        {
            get
            {
                return _IsExtinct;
            }

            set
            {
                _IsExtinct = value;
            }
        }

        public bool InDoubt
        {
            get
            {
                return _InDoubt;
            }

            set
            {
                _InDoubt = value;
            }
        }

        public Taxon Parent => _Parent;

        public IReadOnlyList<Taxon> Children => _Children;

        public string Comment
        {
            get
            {
                return _Comment;
            }

            set
            {
                _Comment = value;
            }
        }

        public List<string> Url => _Url;

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

        // 获取当前类群与顶级类群的距离。
        public int Level => _Level;

        // 获取当前类群在父类群的所有子类群中的次序。
        public int Index
        {
            get
            {
                if (_Parent is null)
                {
                    return -1;
                }
                else
                {
                    return _Parent._Children.IndexOf(this);
                }
            }
        }

        //

        public override bool Equals(object obj)
        {
            return object.ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (_InDoubt)
            {
                sb.Append('?');
            }

            if (_IsExtinct)
            {
                sb.Append('†');
            }

            if (!string.IsNullOrWhiteSpace(_ChineseName))
            {
                sb.Append(_ChineseName);
            }
            else
            {
                sb.Append(_Category.Name());
            }

            if (!string.IsNullOrWhiteSpace(_BotanicalName))
            {
                sb.Append(' ');
                sb.Append(_BotanicalName);
            }

            return sb.ToString();
        }

        //

        // 递归地修复当前类群、子类群与顶级类群的距离。
        public void RepairLevel()
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
                child.RepairLevel();
            }
        }

        // 递归地修复子类群的继承关系。
        public void RepairInheritance()
        {
            foreach (Taxon child in _Children)
            {
                child._Parent = this;
                child.RepairInheritance();
            }
        }

        //

        // 变更父类群。
        public void SetParent(Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (!(_Parent is null))
            {
                _Parent._Children.Remove(this);
            }

            _Parent = taxon;
            _Parent._Children.Add(this);

            RepairLevel();
        }

        // 变更父类群。
        public void SetParent(Taxon taxon, int index)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (!(_Parent is null))
            {
                _Parent._Children.Remove(this);
            }

            _Parent = taxon;
            _Parent._Children.Insert(index, this);

            RepairLevel();
        }

        // 添加一个上层父类群。
        public Taxon AddParentUplevel()
        {
            Taxon parent = new Taxon();

            parent._Parent = _Parent;

            _Parent._Children[_Parent._Children.IndexOf(this)] = parent;
            _Parent = parent;

            parent.RepairLevel();

            return parent;
        }

        // 添加一个下层父类群。
        public Taxon AddParentDownlevel()
        {
            Taxon parent = new Taxon();

            parent._Parent = this;

            _Parent._Children = _Children;
            _Children = new List<Taxon>() { parent };

            parent.RepairLevel();

            return parent;
        }

        // 移动一个子类群。
        public void MoveChild(int oldIndex, int newIndex)
        {
            if (oldIndex != newIndex)
            {
                Taxon child = _Children[oldIndex];

                _Children.RemoveAt(oldIndex);
                _Children.Insert(newIndex, child);
            }
        }

        // 交换两个子类群。
        public void SwapChild(int index1, int index2)
        {
            if (index1 != index2)
            {
                Taxon child1 = _Children[index1];
                Taxon child2 = _Children[index2];

                _Children[index2] = child1;
                _Children[index1] = child2;
            }
        }

        // 添加一个子类群。
        public Taxon AddChild()
        {
            Taxon child = new Taxon();

            _Children.Add(child);

            child._Parent = this;
            child._Level = _Level + 1;

            return child;
        }

        // 添加一个子类群。
        public Taxon AddChild(int index)
        {
            Taxon child = new Taxon();

            _Children.Insert(index, child);

            child._Parent = this;
            child._Level = _Level + 1;

            return child;
        }

        // 删除当前类群。
        public void RemoveCurrent()
        {
            if (!(_Parent is null))
            {
                _Parent._Children.Remove(this);
                _Parent = null;
            }
        }
    }
}