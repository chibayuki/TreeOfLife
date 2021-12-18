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

using System.Windows;

namespace TreeOfLife.UI.Views
{
    public sealed class ViewModel_MainWindow : ViewModel
    {
        private bool _TabPageButtonEnabled_File;
        private bool _TabPageButtonEnabled_Evo;
        private bool _TabPageButtonEnabled_Search;
        private bool _TabPageButtonEnabled_Statistics;
        private bool _TabPageButtonEnabled_Validation;
        private bool _TabPageButtonEnabled_About;

        private Visibility _TabPageVisibility_File;
        private Visibility _TabPageVisibility_Evo;
        private Visibility _TabPageVisibility_Search;
        private Visibility _TabPageVisibility_Statistics;
        private Visibility _TabPageVisibility_Validation;
        private Visibility _TabPageVisibility_About;

        public bool TabPageButtonEnabled_File
        {
            get => _TabPageButtonEnabled_File;

            set
            {
                _TabPageButtonEnabled_File = value;

                NotifyPropertyChanged(nameof(TabPageButtonEnabled_File));
            }
        }

        public bool TabPageButtonEnabled_Evo
        {
            get => _TabPageButtonEnabled_Evo;

            set
            {
                _TabPageButtonEnabled_Evo = value;

                NotifyPropertyChanged(nameof(TabPageButtonEnabled_Evo));
            }
        }

        public bool TabPageButtonEnabled_Search
        {
            get => _TabPageButtonEnabled_Search;

            set
            {
                _TabPageButtonEnabled_Search = value;

                NotifyPropertyChanged(nameof(TabPageButtonEnabled_Search));
            }
        }

        public bool TabPageButtonEnabled_Statistics
        {
            get => _TabPageButtonEnabled_Statistics;

            set
            {
                _TabPageButtonEnabled_Statistics = value;

                NotifyPropertyChanged(nameof(TabPageButtonEnabled_Statistics));
            }
        }

        public bool TabPageButtonEnabled_Validation
        {
            get => _TabPageButtonEnabled_Validation;

            set
            {
                _TabPageButtonEnabled_Validation = value;

                NotifyPropertyChanged(nameof(TabPageButtonEnabled_Validation));
            }
        }

        public bool TabPageButtonEnabled_About
        {
            get => _TabPageButtonEnabled_About;

            set
            {
                _TabPageButtonEnabled_About = value;

                NotifyPropertyChanged(nameof(TabPageButtonEnabled_About));
            }
        }

        public Visibility TabPageVisibility_File
        {
            get => _TabPageVisibility_File;

            set
            {
                _TabPageVisibility_File = value;

                NotifyPropertyChanged(nameof(TabPageVisibility_File));
            }
        }

        public Visibility TabPageVisibility_Evo
        {
            get => _TabPageVisibility_Evo;

            set
            {
                _TabPageVisibility_Evo = value;

                NotifyPropertyChanged(nameof(TabPageVisibility_Evo));
            }
        }

        public Visibility TabPageVisibility_Search
        {
            get => _TabPageVisibility_Search;

            set
            {
                _TabPageVisibility_Search = value;

                NotifyPropertyChanged(nameof(TabPageVisibility_Search));
            }
        }

        public Visibility TabPageVisibility_Statistics
        {
            get => _TabPageVisibility_Statistics;

            set
            {
                _TabPageVisibility_Statistics = value;

                NotifyPropertyChanged(nameof(TabPageVisibility_Statistics));
            }
        }

        public Visibility TabPageVisibility_Validation
        {
            get => _TabPageVisibility_Validation;

            set
            {
                _TabPageVisibility_Validation = value;

                NotifyPropertyChanged(nameof(TabPageVisibility_Validation));
            }
        }

        public Visibility TabPageVisibility_About
        {
            get => _TabPageVisibility_About;

            set
            {
                _TabPageVisibility_About = value;

                NotifyPropertyChanged(nameof(TabPageVisibility_About));
            }
        }
    }
}