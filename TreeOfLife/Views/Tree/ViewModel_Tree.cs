/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1000.1000.M10.210130-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Media;

using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

namespace TreeOfLife.Views.Tree
{
    public class ViewModel_Tree : INotifyPropertyChanged
    {
        public ViewModel_Tree()
        {
        }

        //

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //

        #region 系统发生树（临时）

        private string _TreeText;

        public string TreeText
        {
            get => _TreeText;

            set
            {
                _TreeText = value;

                NotifyPropertyChanged(nameof(TreeText));
            }
        }

        private int _ParentLevels = 1; // 向上追溯的具名父类群的层数。
        private int _ChildrenLevels = 3; // 向下追溯的具名子类群的层数。
        private int _SiblingLevels = 0; // 旁系群向下追溯的具名子类群的层数。

        private int _LevelShift = 0; // 向上追溯的最高具名父类群的层级。

        // 为指定类群生成一行表示其在系统发生树中的字符串。
        private string _GenerateLine(Taxon taxon)
        {
            int relativeLevel = taxon.Level - _LevelShift;

            if (relativeLevel > 0)
            {
                // 每个类群用一行字符表示，类群名之前的字符数与类群的层级成正比（这里使用2倍关系）
                char[] ch = new char[relativeLevel * 2];

                // 首先全部初始化为空格，然后逆序判断和替换字符
                for (int i = 0; i < ch.Length; i++)
                {
                    ch[i] = '　';
                }

                int chIndex = ch.Length - 2;

                // 当类群的索引不是最后一个时，替换为├
                if (taxon.Index < taxon.Parent.Children.Count - 1)
                {
                    ch[chIndex] = '├';
                }
                // 当类群的索引是最后一个时，替换为└
                else
                {
                    ch[chIndex] = '└';
                }

                chIndex -= 2;

                Taxon t = taxon.Parent;

                // 逐个向上判断父类群
                while (chIndex >= 0 && t.Parent != null)
                {
                    // 只有当父类群的索引不是最后一个时，替换为│
                    if (t.Index < t.Parent.Children.Count - 1)
                    {
                        ch[chIndex] = '│';
                    }

                    chIndex -= 2;
                    t = t.Parent;
                }

                if (taxon.IsNamed())
                {
                    return (new string(ch) + taxon.GetLongName());
                }
                else
                {
                    return (new string(ch) + "─");
                }
            }
            else
            {
                if (taxon.IsNamed())
                {
                    return taxon.GetLongName();
                }
                else
                {
                    return "─";
                }
            }
        }

        private int _CurrentChildrenDepth; // 当前递归深度。

        // 递归填充系统发生树的子类群部分。
        private void _RecursiveGenerateLinesForChildren(StringBuilder sb, Taxon taxon)
        {
            if (sb == null || taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            if (!taxon.IsRoot)
            {
                sb.AppendLine(_GenerateLine(taxon));
            }

            if (taxon.IsRoot || taxon.IsNamed())
            {
                _CurrentChildrenDepth++;
            }

            if (_CurrentChildrenDepth < _ChildrenLevels)
            {
                foreach (var child in taxon.Children)
                {
                    _RecursiveGenerateLinesForChildren(sb, child);
                }
            }

            if (taxon.IsRoot || taxon.IsNamed())
            {
                _CurrentChildrenDepth--;
            }
        }

        private int _CurrentSiblingsDepth; // 当前递归深度。

        // 递归填充系统发生树的旁系群部分。
        private void _RecursiveGenerateLinesForSiblings(StringBuilder sb, Taxon taxon)
        {
            if (sb == null || taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            if (!taxon.IsRoot)
            {
                sb.AppendLine(_GenerateLine(taxon));
            }

            if (taxon.IsRoot || taxon.IsNamed())
            {
                _CurrentSiblingsDepth++;
            }

            if (_CurrentSiblingsDepth < _SiblingLevels)
            {
                foreach (var child in taxon.Children)
                {
                    _RecursiveGenerateLinesForSiblings(sb, child);
                }
            }

            if (taxon.IsRoot || taxon.IsNamed())
            {
                _CurrentSiblingsDepth--;
            }
        }

        // 递归填充纯文本形式的系统发生树。
        private void _RecursiveGenerateLinesForEvoTree(StringBuilder sb, Taxon taxon)
        {
            if (sb == null || taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            if (taxon == _NamedTaxon)
            {
                _CurrentChildrenDepth = -1;

                _RecursiveGenerateLinesForChildren(sb, taxon);
            }
            else if (taxon.IsNamed() && !_NamedTaxon.InheritFrom(taxon))
            {
                _CurrentSiblingsDepth = -1;

                _RecursiveGenerateLinesForSiblings(sb, taxon);
            }
            else
            {
                if (!taxon.IsRoot)
                {
                    sb.AppendLine(_GenerateLine(taxon));
                }

                foreach (var child in taxon.Children)
                {
                    _RecursiveGenerateLinesForEvoTree(sb, child);
                }
            }
        }

        private Taxon _NamedTaxon = null; // 当前类群或其最近的具名上级类群。

        public void UpdateTree()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            if (!currentTaxon.IsRoot && currentTaxon.IsAnonymous())
            {
                _NamedTaxon = currentTaxon.GetNamedParent();

                if (_NamedTaxon == null)
                {
                    _NamedTaxon = currentTaxon.Root;
                }
            }
            else
            {
                _NamedTaxon = currentTaxon;
            }

            Taxon parent = _NamedTaxon;

            for (int i = 0; i < _ParentLevels; i++)
            {
                parent = parent.GetNamedParent();

                if (parent == null)
                {
                    parent = _NamedTaxon.Root;

                    break;
                }
            }

            _LevelShift = parent.Level;

            StringBuilder sb = new StringBuilder();

            _RecursiveGenerateLinesForEvoTree(sb, parent);

            _NamedTaxon = null;

            TreeText = sb.ToString();
        }

        #endregion

        #region 主题

        private bool _IsDarkTheme;

        private Brush _TextBox_ForeGround;
        private Brush _TextBox_BackGround;
        private Brush _TextBox_Selection;
        private Brush _TextBox_SelectionText;

        private void _UpdateColors()
        {
            TextBox_ForeGround = Common.TextBox_ForeGround;
            TextBox_BackGround = Common.TextBox_BackGround;
            TextBox_Selection = Common.TextBox_Selection;
            TextBox_SelectionText = Common.TextBox_SelectionText;
        }

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateColors();
            }
        }

        public Brush TextBox_ForeGround
        {
            get => _TextBox_ForeGround;

            set
            {
                _TextBox_ForeGround = value;

                NotifyPropertyChanged(nameof(TextBox_ForeGround));
            }
        }

        public Brush TextBox_BackGround
        {
            get => _TextBox_BackGround;

            set
            {
                _TextBox_BackGround = value;

                NotifyPropertyChanged(nameof(TextBox_BackGround));
            }
        }

        public Brush TextBox_Selection
        {
            get => _TextBox_Selection;

            set
            {
                _TextBox_Selection = value;

                NotifyPropertyChanged(nameof(TextBox_Selection));
            }
        }

        public Brush TextBox_SelectionText
        {
            get => _TextBox_SelectionText;

            set
            {
                _TextBox_SelectionText = value;

                NotifyPropertyChanged(nameof(TextBox_SelectionText));
            }
        }

        #endregion
    }
}