/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.812.1000.M8.210108-2100

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

namespace TreeOfLife.Views.Evo.ViewMode
{
    /// <summary>
    /// View_Evo_ViewMode.xaml 的交互逻辑
    /// </summary>
    public partial class View_Evo_ViewMode : UserControl
    {
        public ViewModel_Evo_ViewMode ViewModel => this.DataContext as ViewModel_Evo_ViewMode;

        //

        public View_Evo_ViewMode()
        {
            InitializeComponent();

            //

            taxonNameButtonGroup_Children.GroupNameWidth = 0;
            taxonNameButtonGroup_Children.GroupMargin = new Thickness(0, 1, 0, 1);

            button_Edit.Click += (s, e) => Views.Common.EnterEditMode();
        }

        //

        #region 类群

        // 更新父类群。
        private void _UpdateParents()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            if (currentTaxon.IsRoot)
            {
                taxonNameButtonGroup_Parents.StartEditing();
                taxonNameButtonGroup_Parents.Clear();
                taxonNameButtonGroup_Parents.FinishEditing();
            }
            else
            {
                var parents = currentTaxon.GetSummaryParents(false);

                if (parents.Count > 0)
                {
                    parents.Reverse();
                }
                else
                {
                    parents.Add(currentTaxon.Parent);
                }

                parents.Add(currentTaxon);

                Common.UpdateParents(taxonNameButtonGroup_Parents, parents);
            }
        }

        // 更新子类群。
        private void _UpdateChildren()
        {
            var children = Views.Common.CurrentTaxon.GetNamedChildren(true);

            Common.UpdateChildren(taxonNameButtonGroup_Children, children);
        }

        // 更新可见性。
        private void _UpdateVisibility()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            grid_Tags.Visibility = (currentTaxon.Tags.Count <= 0 ? Visibility.Collapsed : Visibility.Visible);
            grid_Parents.Visibility = (currentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Children.Visibility = (currentTaxon.IsFinal ? Visibility.Collapsed : Visibility.Visible);
            grid_Synonyms.Visibility = (currentTaxon.Synonyms.Count <= 0 ? Visibility.Collapsed : Visibility.Visible);
            grid_Desc.Visibility = (string.IsNullOrWhiteSpace(currentTaxon.Description) ? Visibility.Collapsed : Visibility.Visible);
        }

        public void UpdateCurrentTaxonInfo()
        {
            ViewModel.UpdateFromTaxon();

            tagGroup_Tags.Tags = ViewModel.Tags;
            tagGroup_Synonyms.Tags = ViewModel.Synonyms;
            tagGroup_Synonyms.ThemeColor = ViewModel.TaxonColor;

            _UpdateParents();
            _UpdateChildren();
            _UpdateVisibility();
        }

        #endregion

        #region 主题

        private bool _IsDarkTheme = false;

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                tagGroup_Tags.IsDarkTheme = _IsDarkTheme;
                tagGroup_Synonyms.IsDarkTheme = _IsDarkTheme;
                taxonNameButtonGroup_Parents.IsDarkTheme = _IsDarkTheme;
                taxonNameButtonGroup_Children.IsDarkTheme = _IsDarkTheme;

                ViewModel.IsDarkTheme = _IsDarkTheme;
            }
        }

        #endregion
    }
}