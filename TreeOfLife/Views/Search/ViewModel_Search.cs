/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1100.1000.M11.210405-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Media;

namespace TreeOfLife.Views.Search
{
    public class ViewModel_Search : INotifyPropertyChanged
    {
        public ViewModel_Search()
        {
        }

        //

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //

        #region 搜索

        public Action ClickSearchResult { get; set; }

        private string _KeyWord;

        public string KeyWord
        {
            get => _KeyWord;

            set
            {
                _KeyWord = value;

                NotifyPropertyChanged(nameof(KeyWord));
            }
        }

        #endregion

        #region 主题

        private bool _IsDarkTheme;

        private Brush _Button_ForeGround;
        private Brush _Button_BackGround;
        private Brush _SubTitle_ForeGround;
        private Brush _SubTitle_BackGround;
        private Brush _TextBox_ForeGround;
        private Brush _TextBox_BackGround;
        private Brush _TextBox_Selection;
        private Brush _TextBox_SelectionText;

        private void _UpdateColors()
        {
            Button_ForeGround = Common.Button_ForeGround;
            Button_BackGround = Common.Button_BackGround;
            SubTitle_ForeGround = Common.SubTitle_ForeGround;
            SubTitle_BackGround = Common.SubTitle_BackGround;
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

        public Brush Button_ForeGround
        {
            get => _Button_ForeGround;

            set
            {
                _Button_ForeGround = value;

                NotifyPropertyChanged(nameof(Button_ForeGround));
            }
        }

        public Brush Button_BackGround
        {
            get => _Button_BackGround;

            set
            {
                _Button_BackGround = value;

                NotifyPropertyChanged(nameof(Button_BackGround));
            }
        }

        public Brush SubTitle_ForeGround
        {
            get => _SubTitle_ForeGround;

            set
            {
                _SubTitle_ForeGround = value;

                NotifyPropertyChanged(nameof(SubTitle_ForeGround));
            }
        }

        public Brush SubTitle_BackGround
        {
            get => _SubTitle_BackGround;

            set
            {
                _SubTitle_BackGround = value;

                NotifyPropertyChanged(nameof(SubTitle_BackGround));
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