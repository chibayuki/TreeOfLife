﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1322.1000.M13.210925-1400

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreeOfLife.Core;

namespace TreeOfLife.UI.Views
{
    public sealed class ViewModel_File : ViewModel
    {
        private string _FileName; // 文件名。
        private string _FileSize; // 文件大小。
        private string _DataSize; // 数据大小。
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

        public string DataSize
        {
            get => _DataSize;

            set
            {
                _DataSize = value;

                NotifyPropertyChanged(nameof(DataSize));
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
            DataSize = _GetSizeString(Entrance.DataSize);
            CreationTime = string.Concat(Entrance.CreationTime.ToLocalTime().ToLongDateString(), " ", Entrance.CreationTime.ToLocalTime().ToLongTimeString());

            if (string.IsNullOrWhiteSpace(Entrance.FileName))
            {
                FileName = "(未保存)";
                FileSize = "(未保存)";
                ModificationTime = "(未保存)";
            }
            else
            {
                FileName = Entrance.PureFileName;
                FileSize = _GetSizeString(Entrance.FileSize);
                ModificationTime = string.Concat(Entrance.ModificationTime.ToLocalTime().ToLongDateString(), " ", Entrance.ModificationTime.ToLocalTime().ToLongTimeString());
            }
        }
    }
}