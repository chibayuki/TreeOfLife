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

namespace TreeOfLife.Views.File
{
    public class ViewModel_File : INotifyPropertyChanged
    {
        public ViewModel_File()
        {
        }

        //

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //

        #region 文件信息

        private DateTime _CreationTime; // 创建时间。
        private string _CreationTimeString; // 创建时间。
        private DateTime _ModificationTime; // 修改时间。
        private string _ModificationTimeString; // 修改时间。

        public DateTime CreationTime
        {
            get => _CreationTime;

            set
            {
                if (_CreationTime != value)
                {
                    _CreationTime = value;

                    if (_CreationTime == DateTime.MinValue)
                    {
                        CreationTimeString = string.Empty;
                    }
                    else
                    {
                        CreationTimeString = string.Concat("创建时间: ", _CreationTime.ToLocalTime().ToLongDateString(), " ", _CreationTime.ToLocalTime().ToLongTimeString());
                    }
                }
            }
        }

        public string CreationTimeString
        {
            get => _CreationTimeString;

            set
            {
                if (_CreationTimeString != value)
                {
                    _CreationTimeString = value;

                    NotifyPropertyChanged(nameof(CreationTimeString));
                }
            }
        }

        public DateTime ModificationTime
        {
            get => _ModificationTime;

            set
            {
                if (_ModificationTime != value)
                {
                    _ModificationTime = value;

                    if (_ModificationTime == DateTime.MinValue)
                    {
                        ModificationTimeString = string.Empty;
                    }
                    else
                    {
                        ModificationTimeString = string.Concat("修改时间: ", _ModificationTime.ToLocalTime().ToLongDateString(), " ", _ModificationTime.ToLocalTime().ToLongTimeString());
                    }
                }
            }
        }

        public string ModificationTimeString
        {
            get => _ModificationTimeString;

            set
            {
                if (_ModificationTimeString != value)
                {
                    _ModificationTimeString = value;

                    NotifyPropertyChanged(nameof(ModificationTimeString));
                }
            }
        }

        #endregion

        #region 文件操作

        public Func<bool> Open { get; set; }
        public Func<bool> Save { get; set; }
        public Func<bool> SaveAs { get; set; }
        public Func<bool> Close { get; set; }
        public Func<bool> TrySaveAndClose { get; set; }

        public Action OpenDone { get; set; }

        #endregion

        #region 主题

        private bool _IsDarkTheme;

        private void _UpdateColors()
        {

        }

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                if (_IsDarkTheme != value)
                {
                    _IsDarkTheme = value;

                    _UpdateColors();
                }
            }
        }

        #endregion
    }
}