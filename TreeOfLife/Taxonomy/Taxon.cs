/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.322.1000.M4.201128-1620

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
        private List<string> _Urls = new List<string>(); // 链接。

        private TaxonomicCategory _Category = TaxonomicCategory.Unranked; // 分类阶元。
        private TaxonType _TaxonType = TaxonType.Monophyly; // 类型。

        private bool _IsExtinct = false; // 已灭绝。
        private bool _InDoubt = false; // 分类地位存疑。

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

        private List<string> Urls => _Urls;

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

        public bool InDoubt
        {
            get => _InDoubt;
            set => _InDoubt = value;
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

        // 修复当前类群、姊妹类群的次序。
        public void RepairIndex()
        {
            if (_Parent is null)
            {
                _Index = -1;
            }
            else
            {
                for (int i = 0; i < _Parent._Children.Count; i++)
                {
                    _Parent._Children[i]._Index = i;
                }
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
            RepairIndex();
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
            RepairIndex();
        }

        // 添加一个上层父类群。
        public Taxon AddParentUplevel()
        {
            Taxon parent = new Taxon();

            parent._Parent = _Parent;

            _Parent._Children[_Parent._Children.IndexOf(this)] = parent;
            _Parent = parent;

            parent.RepairLevel();

            parent._Index = _Index;
            _Index = 0;

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

            parent._Index = 0;

            return parent;
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

                child.RepairIndex();
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

            _Children.Add(child);

            child._Parent = this;
            child._Level = _Level + 1;
            child._Index = _Children.Count - 1;

            return child;
        }

        // 添加一个子类群。
        public Taxon AddChild(int index)
        {
            Taxon child = new Taxon();

            _Children.Insert(index, child);

            child._Parent = this;
            child._Level = _Level + 1;

            child.RepairIndex();

            return child;
        }

        // 删除当前类群。
        public void RemoveCurrent()
        {
            if (!(_Parent is null))
            {
                _Parent._Children.Remove(this);

                RepairIndex();

                _Parent = null;
            }
        }
    }
}