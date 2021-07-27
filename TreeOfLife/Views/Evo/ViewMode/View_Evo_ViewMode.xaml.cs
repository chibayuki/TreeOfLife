﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1240.1000.M12.210718-2000

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

using TreeOfLife.Geology;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

namespace TreeOfLife.Views.Evo.ViewMode
{
    /// <summary>
    /// View_Evo_ViewMode.xaml 的交互逻辑
    /// </summary>
    public partial class View_Evo_ViewMode : UserControl
    {
        public View_Evo_ViewMode()
        {
            InitializeComponent();

            //

            button_Edit.Click += (s, e) => Views.Common.EnterEditMode();

            Action<GetParentsOption> setGetParentsOption = (getParentsOption) =>
            {
                _GetParentsOption = getParentsOption;

                button_Least.IsEnabled = (_GetParentsOption != GetParentsOption.Least);
                button_Summary.IsEnabled = (_GetParentsOption != GetParentsOption.Summary);
                button_Full.IsEnabled = (_GetParentsOption != GetParentsOption.Full);

                path_Least.Visibility = (_GetParentsOption == GetParentsOption.Least ? Visibility.Visible : Visibility.Hidden);
                path_Summary.Visibility = (_GetParentsOption == GetParentsOption.Summary ? Visibility.Visible : Visibility.Hidden);
                path_Full.Visibility = (_GetParentsOption == GetParentsOption.Full ? Visibility.Visible : Visibility.Hidden);
            };

            setGetParentsOption(GetParentsOption.Summary);

            button_Least.Click += (s, e) =>
            {
                setGetParentsOption(GetParentsOption.Least);
                _UpdateParents();
            };

            button_Summary.Click += (s, e) =>
            {
                setGetParentsOption(GetParentsOption.Summary);
                _UpdateParents();
            };

            button_Full.Click += (s, e) =>
            {
                setGetParentsOption(GetParentsOption.Full);
                _UpdateParents();
            };

            taxonNameButtonGroup_Parents.MouseLeftButtonClick += (s, e) => Views.Common.SetCurrentTaxon(e.Taxon);
            taxonNameButtonGroup_Children.MouseLeftButtonClick += (s, e) => Views.Common.SetCurrentTaxon(e.Taxon);
            taxonNameButtonGroup_Excludes.MouseLeftButtonClick += (s, e) => Views.Common.SetCurrentTaxon(e.Taxon);

            //

            Theme.IsDarkThemeChanged += (s, e) =>
            {
                taxonNameTitle.IsDarkTheme = Theme.IsDarkTheme;
                geoChronSpan.IsDarkTheme = Theme.IsDarkTheme;
                tagGroup_Tags.IsDarkTheme = Theme.IsDarkTheme;
                tagGroup_Synonyms.IsDarkTheme = Theme.IsDarkTheme;
                taxonNameButtonGroup_Parents.IsDarkTheme = Theme.IsDarkTheme;
                taxonNameButtonGroup_Children.IsDarkTheme = Theme.IsDarkTheme;
                taxonNameButtonGroup_Excludes.IsDarkTheme = Theme.IsDarkTheme;
            };
        }

        //

        public ViewModel_Evo_ViewMode ViewModel => this.DataContext as ViewModel_Evo_ViewMode;

        //

        private GetParentsOption _GetParentsOption;

        // 更新父类群。
        private void _UpdateParents()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            if (currentTaxon.IsRoot)
            {
                taxonNameButtonGroup_Parents.Clear();
            }
            else
            {
                List<Taxon> parents = new List<Taxon>(currentTaxon.GetParents(_GetParentsOption));

                if (parents.Count > 0)
                {
                    parents.Reverse();
                }

                parents.Add(currentTaxon);

                Common.UpdateTaxonListAndGroupByCategory(taxonNameButtonGroup_Parents, parents);
            }
        }

        // 更新子类群。
        private void _UpdateChildren()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            List<(Taxon, int)> children = new List<(Taxon, int)>();

            foreach (var child in currentTaxon.GetNamedChildren(true))
            {
                // 添加当前类群的具名子类群（若当前类群是并系群，则只添加未被排除的具名子类群）
                if (currentTaxon.Excludes.Count > 0)
                {
                    bool childIsExcludedByCurrent = false;

                    foreach (var exclude in currentTaxon.Excludes)
                    {
                        if (exclude.IsNamed())
                        {
                            if (child == exclude)
                            {
                                childIsExcludedByCurrent = true;

                                break;
                            }
                        }
                        else
                        {
                            foreach (var item in exclude.GetNamedChildren(true))
                            {
                                if (child == item)
                                {
                                    childIsExcludedByCurrent = true;

                                    break;
                                }
                            }
                        }
                    }

                    if (!childIsExcludedByCurrent)
                    {
                        children.Add((child, 0));
                    }
                }
                else
                {
                    children.Add((child, 0));
                }

                // 若具名子类群是并系群，添加并系群排除的类群
                foreach (var exclude in child.Excludes)
                {
                    if (exclude.IsNamed())
                    {
                        children.Add((exclude, -1));
                    }
                    else
                    {
                        foreach (var item in exclude.GetNamedChildren(true))
                        {
                            children.Add((item, -1));
                        }
                    }
                }
            }

            // 若当前类群是复系群，添加复系群包含的类群
            foreach (var include in currentTaxon.Includes)
            {
                if (include.IsNamed())
                {
                    children.Add((include, +1));
                }
                else
                {
                    foreach (var item in include.GetNamedChildren(true))
                    {
                        children.Add((item, +1));
                    }
                }
            }

            Common.UpdateTaxonList(taxonNameButtonGroup_Children, children);
        }

        // 更新 Excludes。
        private void _UpdateExcludes()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            List<Taxon> excludes = new List<Taxon>();

            foreach (var exclude in currentTaxon.Excludes)
            {
                if (exclude.IsNamed())
                {
                    excludes.Add(exclude);
                }
                else
                {
                    foreach (var item in exclude.GetNamedChildren(true))
                    {
                        excludes.Add(item);
                    }
                }
            }

            Common.UpdateTaxonList(taxonNameButtonGroup_Excludes, excludes);
        }

        // 更新可见性。
        private void _UpdateVisibility()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            grid_Tags.Visibility = (currentTaxon.Tags.Count > 0 ? Visibility.Visible : Visibility.Collapsed);
            grid_Synonyms.Visibility = (currentTaxon.Synonyms.Count > 0 ? Visibility.Visible : Visibility.Collapsed);
            stackPanel_TagsAndSynonyms.Visibility = (currentTaxon.Tags.Count > 0 || currentTaxon.Synonyms.Count > 0 ? Visibility.Visible : Visibility.Collapsed);
            grid_Parents.Visibility = (!currentTaxon.IsRoot ? Visibility.Visible : Visibility.Collapsed);
            grid_Children.Visibility = (taxonNameButtonGroup_Children.GetGroupCount() > 0 ? Visibility.Visible : Visibility.Collapsed);
            grid_Excludes.Visibility = (currentTaxon.Excludes.Count > 0 ? Visibility.Visible : Visibility.Collapsed);
            grid_Desc.Visibility = (!string.IsNullOrWhiteSpace(currentTaxon.Description) ? Visibility.Visible : Visibility.Collapsed);
        }

        private void _UpdateTitle()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            taxonNameTitle.ThemeColor = currentTaxon.GetThemeColor();
            taxonNameTitle.TaxonName = currentTaxon.GetShortName('\n');
            taxonNameTitle.Category = (currentTaxon.IsAnonymous() ? null : currentTaxon.Category);
            taxonNameTitle.IsParaphyly = currentTaxon.IsParaphyly;
            taxonNameTitle.IsPolyphyly = currentTaxon.IsPolyphyly;

            if ((currentTaxon.IsExtinct && (!currentTaxon.Birth.IsEmpty || !currentTaxon.Extinction.IsEmpty)) || (!currentTaxon.IsExtinct && !currentTaxon.Birth.IsEmpty))
            {
                geoChronSpan.Update(currentTaxon.Birth, (currentTaxon.IsExtinct ? currentTaxon.Extinction : GeoChron.Present), currentTaxon.GetInheritedCategory());

                geoChronSpan.Visibility = Visibility.Visible;
            }
            else
            {
                geoChronSpan.Visibility = Visibility.Collapsed;
            }
        }

        public void UpdateCurrentTaxonInfo()
        {
            ViewModel.UpdateFromTaxon();

            _UpdateTitle();

            Taxon currentTaxon = Views.Common.CurrentTaxon;

            tagGroup_Tags.UpdateContent(currentTaxon.Tags);
            tagGroup_Synonyms.UpdateContent(currentTaxon.Synonyms);
            tagGroup_Synonyms.ThemeColor = currentTaxon.GetThemeColor();

            _UpdateParents();
            _UpdateChildren();
            _UpdateExcludes();

            _UpdateVisibility();
        }
    }
}