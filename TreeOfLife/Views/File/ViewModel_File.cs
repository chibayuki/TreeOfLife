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
using System.IO;
using System.Windows.Media;

using TreeOfLife.Phylogeny;

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

        private string _FileName; // 文件名。
        private string _FileSize; // 文件大小。
        private string _CreationTime; // 创建时间。
        private string _ModificationTime; // 修改时间。

        public string FileName
        {
            get => _FileName;

            set
            {
                if (_FileName != value)
                {
                    _FileName = value;

                    NotifyPropertyChanged(nameof(FileName));
                }
            }
        }

        public string FileSize
        {
            get => _FileSize;

            set
            {
                if (_FileSize != value)
                {
                    _FileSize = value;

                    NotifyPropertyChanged(nameof(FileSize));
                }
            }
        }

        public string CreationTime
        {
            get => _CreationTime;

            set
            {
                if (_CreationTime != value)
                {
                    _CreationTime = value;

                    NotifyPropertyChanged(nameof(CreationTime));
                }
            }
        }

        public string ModificationTime
        {
            get => _ModificationTime;

            set
            {
                if (_ModificationTime != value)
                {
                    _ModificationTime = value;

                    NotifyPropertyChanged(nameof(ModificationTime));
                }
            }
        }

        private string _GetSizeString(long size)
        {
            if (size <= 0)
            {
                return "0 B";
            }
            else
            {
                string s = size.ToString("N0") + " B";

                if (size < 1000L)
                {
                    return s;
                }
                else if (size < 1000L * 1024)
                {
                    return string.Concat((size / 1024.0).ToString("N0"), " KB (", s, ")");
                }
                else if (size < 1000L * 1024 * 1024)
                {
                    return string.Concat((size / 1024.0 / 1024.0).ToString("N0") + " MB (", s, ")");
                }
                else
                {
                    return string.Concat((size / 1024.0 / 1024.0 / 1024.0).ToString("N0") + " GB (", s, ")");
                }
            }
        }

        public void UpdateFileInfo()
        {
            if (string.IsNullOrWhiteSpace(Phylogenesis.FileName))
            {
                FileName = "(未保存)";
                FileSize = "(未保存)";
                CreationTime = "(未保存)";
                ModificationTime = "(未保存)";
            }
            else
            {
                FileName = Path.GetFileNameWithoutExtension(Phylogenesis.FileName);
                FileSize = _GetSizeString(Phylogenesis.FileSize);
                CreationTime = string.Concat(Phylogenesis.CreationTime.ToLocalTime().ToLongDateString(), " ", Phylogenesis.CreationTime.ToLocalTime().ToLongTimeString());
                ModificationTime = string.Concat(Phylogenesis.ModificationTime.ToLocalTime().ToLongDateString(), " ", Phylogenesis.ModificationTime.ToLocalTime().ToLongTimeString());
            }
        }

        #endregion

        #region 文件操作

        public Func<bool?> Open { get; set; }
        public Func<bool?> Save { get; set; }
        public Func<bool?> SaveAs { get; set; }
        public Func<bool> Close { get; set; }
        public Func<bool> TrySaveAndClose { get; set; }

        public Action OpenDone { get; set; }

        #endregion

        #region 主题

        private bool _IsDarkTheme;

        private Brush _SubTitle_ForeGround;
        private Brush _SubTitle_BackGround;
        private Brush _FileInfo_ForeGround;

        private void _UpdateColors()
        {
            SubTitle_ForeGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(208, 208, 208) : Color.FromRgb(48, 48, 48));
            SubTitle_BackGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(48, 48, 48) : Color.FromRgb(208, 208, 208));
            FileInfo_ForeGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(192, 192, 192) : Color.FromRgb(64, 64, 64));
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

        public Brush SubTitle_ForeGround
        {
            get => _SubTitle_ForeGround;

            set
            {
                if (_SubTitle_ForeGround != value)
                {
                    _SubTitle_ForeGround = value;

                    NotifyPropertyChanged(nameof(SubTitle_ForeGround));
                }
            }
        }

        public Brush SubTitle_BackGround
        {
            get => _SubTitle_BackGround;

            set
            {
                if (_SubTitle_BackGround != value)
                {
                    _SubTitle_BackGround = value;

                    NotifyPropertyChanged(nameof(SubTitle_BackGround));
                }
            }
        }

        public Brush FileInfo_ForeGround
        {
            get => _FileInfo_ForeGround;

            set
            {
                if (_FileInfo_ForeGround != value)
                {
                    _FileInfo_ForeGround = value;

                    NotifyPropertyChanged(nameof(FileInfo_ForeGround));
                }
            }
        }

        #endregion
    }
}