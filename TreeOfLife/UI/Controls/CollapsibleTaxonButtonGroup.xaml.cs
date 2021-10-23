/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TreeOfLife.Core.Taxonomy;

namespace TreeOfLife.UI.Controls
{
    public partial class CollapsibleTaxonButtonGroup : UserControl
    {
        private bool _Expanded = false;
        private string _Title = string.Empty;
        private List<TaxonItem> _TaxonItems = new List<TaxonItem>();

        //

        private void _UpdateTitle() => label_Title.Content = $"{_Title} ({_TaxonItems.Count})";

        private void _UpdateExpandState()
        {
            if (_Expanded)
            {
                button_Expand.Visibility = Visibility.Collapsed;
                button_Collapse.Visibility = Visibility.Visible;

                if (taxonNameButtonGroup.GetGroupCount() <= 0 && _TaxonItems.Count > 0)
                {
                    taxonNameButtonGroup.UpdateContent(_TaxonItems);
                }

                taxonNameButtonGroup.Visibility = Visibility.Visible;
            }
            else
            {
                button_Expand.Visibility = Visibility.Visible;
                button_Collapse.Visibility = Visibility.Collapsed;

                taxonNameButtonGroup.Visibility = Visibility.Collapsed;
            }
        }

        //

        public CollapsibleTaxonButtonGroup()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateTitle();
                _UpdateExpandState();
            };

            RoutedEventHandler switchExpandState = (s, e) =>
            {
                _Expanded = !_Expanded;
                _UpdateExpandState();
            };

            button_Expand.Click += switchExpandState;
            button_Collapse.Click += switchExpandState;
        }

        //

        public bool IsDarkTheme
        {
            get => taxonNameButtonGroup.IsDarkTheme;
            set => taxonNameButtonGroup.IsDarkTheme = value;
        }

        public string Title
        {
            get => _Title;

            set
            {
                _Title = value;

                _UpdateTitle();
            }
        }

        public bool Expanded
        {
            get => _Expanded;

            set
            {
                _Expanded = value;

                _UpdateExpandState();
            }
        }

        //

        public int GetButtonCount() => _TaxonItems.Count;

        public Taxon GetTaxon(int buttonIndex) => _TaxonItems[buttonIndex].Taxon;

        public void Clear()
        {
            _TaxonItems.Clear();

            _UpdateTitle();

            taxonNameButtonGroup.Clear();
        }

        public void UpdateContent(IEnumerable<TaxonItem> items)
        {
            Clear();

            _TaxonItems.AddRange(items);

            _UpdateTitle();

            if (_Expanded)
            {
                taxonNameButtonGroup.UpdateContent(_TaxonItems);
            }
        }

        //

        public event EventHandler<TaxonButton> MouseLeftButtonClick
        {
            add => taxonNameButtonGroup.MouseLeftButtonClick += value;
            remove => taxonNameButtonGroup.MouseLeftButtonClick -= value;
        }

        public event EventHandler<TaxonButton> MouseRightButtonClick
        {
            add => taxonNameButtonGroup.MouseRightButtonClick += value;
            remove => taxonNameButtonGroup.MouseRightButtonClick -= value;
        }
    }
}