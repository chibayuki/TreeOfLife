/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.617.1000.M6.201225-2240

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Media;

using TreeOfLife.Phylogeny;
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
                if (_TreeText != value)
                {
                    _TreeText = value;

                    NotifyPropertyChanged(nameof(TreeText));
                }
            }
        }

        private void _RecursiveFillStringBuilder(StringBuilder sb, Taxon taxon)
        {
            if (sb == null || taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var child in taxon.Children)
            {
                char[] ch = new char[child.Level * 2];

                for (int i = 0; i < ch.Length; i++)
                {
                    ch[i] = '　';
                }

                int chIndex = ch.Length - 2;

                if (child.Index < child.Parent.Children.Count - 1)
                {
                    ch[chIndex] = '├';
                }
                else
                {
                    ch[chIndex] = '└';
                }

                chIndex -= 2;

                Taxon t = child.Parent;

                while (t.Parent != null)
                {
                    if (t.Index < t.Parent.Children.Count - 1)
                    {
                        ch[chIndex] = '│';
                    }

                    chIndex -= 2;
                    t = t.Parent;
                }

                sb.Append(ch);

                if (child.IsNamed())
                {
                    sb.AppendLine(child.LongName());
                }
                else
                {
                    sb.AppendLine("─");
                }

                _RecursiveFillStringBuilder(sb, child);
            }
        }

        public void UpdateTree()
        {
            StringBuilder sb = new StringBuilder();

            _RecursiveFillStringBuilder(sb, Phylogenesis.Root);

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
                if (_TextBox_ForeGround != value)
                {
                    _TextBox_ForeGround = value;

                    NotifyPropertyChanged(nameof(TextBox_ForeGround));
                }
            }
        }

        public Brush TextBox_BackGround
        {
            get => _TextBox_BackGround;

            set
            {
                if (_TextBox_BackGround != value)
                {
                    _TextBox_BackGround = value;

                    NotifyPropertyChanged(nameof(TextBox_BackGround));
                }
            }
        }

        public Brush TextBox_Selection
        {
            get => _TextBox_Selection;

            set
            {
                if (_TextBox_Selection != value)
                {
                    _TextBox_Selection = value;

                    NotifyPropertyChanged(nameof(TextBox_Selection));
                }
            }
        }

        public Brush TextBox_SelectionText
        {
            get => _TextBox_SelectionText;

            set
            {
                if (_TextBox_SelectionText != value)
                {
                    _TextBox_SelectionText = value;

                    NotifyPropertyChanged(nameof(TextBox_SelectionText));
                }
            }
        }

        #endregion
    }
}