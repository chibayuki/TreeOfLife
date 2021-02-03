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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// TreeNodeButton.xaml 的交互逻辑
    /// </summary>
    public partial class TreeNodeButton : UserControl
    {
        public TreeNodeButton()
        {
            InitializeComponent();

            //

            _UpdateTaxon();
        }

        //

        private Taxon _Taxon;

        private bool _IsRoot;
        private bool _IsFinal;
        private bool _IsFirst;
        private bool _IsLast;
        private bool _ShowButton;

        private void _UpdateTaxon()
        {
            if (_IsRoot)
            {
                grid_LeftPart.Visibility = Visibility.Collapsed;
                grid_RightPart.Visibility = Visibility.Visible;
            }
            else
            {
                if (_IsFirst && _IsLast)
                {
                    grid_Single.Visibility = Visibility.Visible;
                    grid_First.Visibility = Visibility.Collapsed;
                    grid_Last.Visibility = Visibility.Collapsed;
                    grid_Normal.Visibility = Visibility.Collapsed;
                }
                else if (_IsFirst)
                {
                    grid_Single.Visibility = Visibility.Collapsed;
                    grid_First.Visibility = Visibility.Visible;
                    grid_Last.Visibility = Visibility.Collapsed;
                    grid_Normal.Visibility = Visibility.Collapsed;
                }
                else if (_IsLast)
                {
                    grid_Single.Visibility = Visibility.Collapsed;
                    grid_First.Visibility = Visibility.Collapsed;
                    grid_Last.Visibility = Visibility.Visible;
                    grid_Normal.Visibility = Visibility.Collapsed;
                }
                else
                {
                    grid_Single.Visibility = Visibility.Collapsed;
                    grid_First.Visibility = Visibility.Collapsed;
                    grid_Last.Visibility = Visibility.Collapsed;
                    grid_Normal.Visibility = Visibility.Visible;
                }

                grid_LeftPart.Visibility = Visibility.Visible;
                grid_RightPart.Visibility = (_IsFinal || !_ShowButton ? Visibility.Collapsed : Visibility.Visible);
            }

            if (_ShowButton && _Taxon != null)
            {
                label_TaxonName.Content = _Taxon.GetLongName();

                grid_MiddlePart.Visibility = Visibility.Visible;
            }
            else
            {
                grid_MiddlePart.Visibility = Visibility.Collapsed;
            }
        }

        public Taxon Taxon
        {
            get => _Taxon;

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                //

                _Taxon = value;

                _UpdateTaxon();
            }
        }

        public bool IsRoot
        {
            get => _IsRoot;

            set
            {
                _IsRoot = value;

                _UpdateTaxon();
            }
        }

        public bool IsFinal
        {
            get => _IsFinal;

            set
            {
                _IsFinal = value;

                _UpdateTaxon();
            }
        }

        public bool IsFirst
        {
            get => _IsFirst;

            set
            {
                _IsFirst = value;

                _UpdateTaxon();
            }
        }

        public bool IsLast
        {
            get => _IsLast;

            set
            {
                _IsLast = value;

                _UpdateTaxon();
            }
        }

        public bool ShowButton
        {
            get => _ShowButton;

            set
            {
                _ShowButton = value;

                _UpdateTaxon();
            }
        }
    }
}