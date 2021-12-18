/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media;

using TreeOfLife.Core;

namespace TreeOfLife.UI.Views
{
    public sealed class ViewModel_About : ViewModel
    {
        private ImageSource _AppLogo = null;

        public ImageSource AppLogo
        {
            get => _AppLogo;

            set
            {
                _AppLogo = value;

                NotifyPropertyChanged(nameof(AppLogo));
            }
        }

        public string AppVersion => "版本: " + Entrance.AppVersion;
    }
}