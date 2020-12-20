/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.608.1000.M6.201219-0000

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

        private Brush _TreeText_ForeGround;
        private Brush _TreeText_BackGround;
        private Brush _TreeText_Selection;
        private Brush _TreeText_SelectionText;

        private void _UpdateColors()
        {
            TreeText_ForeGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(192, 192, 192) : Color.FromRgb(64, 64, 64));
            TreeText_BackGround = new SolidColorBrush(_IsDarkTheme ? Colors.Black : Colors.White);
            TreeText_Selection = new SolidColorBrush(Color.FromRgb(0, 120, 215));
            TreeText_SelectionText = new SolidColorBrush(_IsDarkTheme ? Colors.Black : Colors.White);
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

        public Brush TreeText_ForeGround
        {
            get => _TreeText_ForeGround;

            set
            {
                if (_TreeText_ForeGround != value)
                {
                    _TreeText_ForeGround = value;

                    NotifyPropertyChanged(nameof(TreeText_ForeGround));
                }
            }
        }

        public Brush TreeText_BackGround
        {
            get => _TreeText_BackGround;

            set
            {
                if (_TreeText_BackGround != value)
                {
                    _TreeText_BackGround = value;

                    NotifyPropertyChanged(nameof(TreeText_BackGround));
                }
            }
        }

        public Brush TreeText_Selection
        {
            get => _TreeText_Selection;

            set
            {
                if (_TreeText_Selection != value)
                {
                    _TreeText_Selection = value;

                    NotifyPropertyChanged(nameof(TreeText_Selection));
                }
            }
        }

        public Brush TreeText_SelectionText
        {
            get => _TreeText_SelectionText;

            set
            {
                if (_TreeText_SelectionText != value)
                {
                    _TreeText_SelectionText = value;

                    NotifyPropertyChanged(nameof(TreeText_SelectionText));
                }
            }
        }

        #endregion
    }
}