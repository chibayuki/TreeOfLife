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
        private string _PackageSize; // 包大小。
        private string _CreationTime; // 创建时间。
        private string _ModificationTime; // 修改时间。

        public string FileName
        {
            get => _FileName;

            set
            {
                _FileName = value;

                NotifyPropertyChanged(nameof(FileName));
            }
        }

        public string FileSize
        {
            get => _FileSize;

            set
            {
                _FileSize = value;

                NotifyPropertyChanged(nameof(FileSize));
            }
        }

        public string PackageSize
        {
            get => _PackageSize;

            set
            {
                _PackageSize = value;

                NotifyPropertyChanged(nameof(PackageSize));
            }
        }

        public string CreationTime
        {
            get => _CreationTime;

            set
            {
                _CreationTime = value;

                NotifyPropertyChanged(nameof(CreationTime));
            }
        }

        public string ModificationTime
        {
            get => _ModificationTime;

            set
            {
                _ModificationTime = value;

                NotifyPropertyChanged(nameof(ModificationTime));
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
                else
                {
                    double div = 1024;
                    string unit;

                    if (size < 1000 * div)
                    {
                        unit = "KB";
                    }
                    else
                    {
                        div *= 1024;

                        if (size < 1000 * div)
                        {
                            unit = "MB";
                        }
                        else
                        {
                            div *= 1024;

                            if (size < 1000 * div)
                            {
                                unit = "GB";
                            }
                            else
                            {
                                div *= 1024;
                                unit = "TB";
                            }
                        }
                    }

                    double value = size / div;

                    if (size < 10 * div)
                    {
                        return string.Concat(value.ToString("N2"), " ", unit, " (", s, ")");
                    }
                    else if (size < 100 * div)
                    {
                        return string.Concat(value.ToString("N1"), " ", unit, " (", s, ")");
                    }
                    else
                    {
                        return string.Concat(value.ToString("N0"), " ", unit, " (", s, ")");
                    }
                }
            }
        }

        public void UpdateFileInfo()
        {
            PackageSize = _GetSizeString(Phylogenesis.PackageSize);
            CreationTime = string.Concat(Phylogenesis.CreationTime.ToLocalTime().ToLongDateString(), " ", Phylogenesis.CreationTime.ToLocalTime().ToLongTimeString());

            if (string.IsNullOrWhiteSpace(Phylogenesis.FileName))
            {
                FileName = "(未保存)";
                FileSize = "(未保存)";
                ModificationTime = "(未保存)";
            }
            else
            {
                FileName = Path.GetFileNameWithoutExtension(Phylogenesis.FileName);
                FileSize = _GetSizeString(new FileInfo(Phylogenesis.FileName).Length);
                ModificationTime = string.Concat(Phylogenesis.ModificationTime.ToLocalTime().ToLongDateString(), " ", Phylogenesis.ModificationTime.ToLocalTime().ToLongTimeString());
            }
        }

        #endregion

        #region 文件操作

        public Func<bool?> Open { get; set; }
        public Func<bool?> Save { get; set; }
        public Func<bool?> SaveAs { get; set; }
        public Func<bool> TrySaveAndClose { get; set; }

        public Action OpenDone { get; set; }

        #endregion

        #region 主题

        private bool _IsDarkTheme;

        private Brush _Button_ForeGround;
        private Brush _Button_BackGround;
        private Brush _SubTitle_ForeGround;
        private Brush _SubTitle_BackGround;
        private Brush _FileInfo_ForeGround;

        private void _UpdateColors()
        {
            Button_ForeGround = Common.Button_ForeGround;
            Button_BackGround = Common.Button_BackGround;
            SubTitle_ForeGround = Common.SubTitle_ForeGround;
            SubTitle_BackGround = Common.SubTitle_BackGround;
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

        public Brush FileInfo_ForeGround
        {
            get => _FileInfo_ForeGround;

            set
            {
                _FileInfo_ForeGround = value;

                NotifyPropertyChanged(nameof(FileInfo_ForeGround));
            }
        }

        #endregion
    }
}