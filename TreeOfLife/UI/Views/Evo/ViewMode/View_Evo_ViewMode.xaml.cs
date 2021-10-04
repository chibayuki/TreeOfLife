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

using TreeOfLife.Core.Geology;
using TreeOfLife.Core.Search.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.UI.Extensions;

namespace TreeOfLife.UI.Views
{
    public partial class View_Evo_ViewMode : UserControl
    {
        public View_Evo_ViewMode()
        {
            InitializeComponent();

            //

            button_Edit.Click += (s, e) => Common.EnterEditMode();

            Action<GetParentsOption> setGetParentsOption = (getParentsOption) =>
            {
                _GetParentsOption = getParentsOption;

                button_Least.IsEnabled = _GetParentsOption != GetParentsOption.Least;
                button_Summary.IsEnabled = _GetParentsOption != GetParentsOption.Summary;
                button_Full.IsEnabled = _GetParentsOption != GetParentsOption.Full;

                path_Least.Visibility = _GetParentsOption == GetParentsOption.Least ? Visibility.Visible : Visibility.Hidden;
                path_Summary.Visibility = _GetParentsOption == GetParentsOption.Summary ? Visibility.Visible : Visibility.Hidden;
                path_Full.Visibility = _GetParentsOption == GetParentsOption.Full ? Visibility.Visible : Visibility.Hidden;
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

            taxonButtonGroup_Parents.MouseLeftButtonClick += (s, e) => Common.SetCurrentTaxon(e.Taxon);
            taxonButtonGroup_Children.MouseLeftButtonClick += (s, e) => Common.SetCurrentTaxon(e.Taxon);
            taxonButtonGroup_Excludes.MouseLeftButtonClick += (s, e) => Common.SetCurrentTaxon(e.Taxon);

            //

            Theme.IsDarkThemeChanged += (s, e) =>
            {
                taxonTitle.IsDarkTheme = Theme.IsDarkTheme;
                geoChronSpan.IsDarkTheme = Theme.IsDarkTheme;
                tagGroup_Tags.IsDarkTheme = Theme.IsDarkTheme;
                tagGroup_Synonyms.IsDarkTheme = Theme.IsDarkTheme;
                taxonButtonGroup_Parents.IsDarkTheme = Theme.IsDarkTheme;
                taxonButtonGroup_Children.IsDarkTheme = Theme.IsDarkTheme;
                taxonButtonGroup_Excludes.IsDarkTheme = Theme.IsDarkTheme;
            };
        }

        //

        public ViewModel_Evo_ViewMode ViewModel => this.DataContext as ViewModel_Evo_ViewMode;

        //

        private GetParentsOption _GetParentsOption;

        // 更新父类群。
        private void _UpdateParents()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            if (currentTaxon.IsRoot)
            {
                taxonButtonGroup_Parents.Clear();
            }
            else
            {
                List<Taxon> parents = new List<Taxon>(currentTaxon.GetParents(_GetParentsOption));

                if (parents.Count > 0)
                {
                    parents.Reverse();
                }

                parents.Add(currentTaxon);

                Common.UpdateTaxonListAndGroupByRank(taxonButtonGroup_Parents, parents);
            }
        }

        // 更新子类群。
        private void _UpdateChildren()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            List<(Taxon, bool)> children = new List<(Taxon, bool)>();

            foreach (var child in currentTaxon.GetNamedChildren(true))
            {
                // 添加当前类群的具名子类群（若当前类群是并系群，则只添加未被排除的具名子类群）
                if (currentTaxon.Excludes.Count > 0)
                {
                    bool childIsExcludedByCurrent = false;

                    foreach (var exclude in currentTaxon.Excludes)
                    {
                        if (exclude.IsNamed)
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
                        children.Add((child, false));
                    }
                }
                else
                {
                    children.Add((child, false));
                }

                // 若具名子类群是并系群，添加并系群排除的类群
                foreach (var exclude in child.Excludes)
                {
                    if (exclude.IsNamed)
                    {
                        children.Add((exclude, true));
                    }
                    else
                    {
                        foreach (var item in exclude.GetNamedChildren(true))
                        {
                            children.Add((item, true));
                        }
                    }
                }
            }

            // 若当前类群是复系群，添加复系群包含的类群
            foreach (var include in currentTaxon.Includes)
            {
                if (include.IsNamed)
                {
                    children.Add((include, true));
                }
                else
                {
                    foreach (var item in include.GetNamedChildren(true))
                    {
                        children.Add((item, true));
                    }
                }
            }

            Common.UpdateTaxonList(taxonButtonGroup_Children, children);
        }

        // 更新 Excludes。
        private void _UpdateExcludes()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            List<Taxon> excludes = new List<Taxon>();

            foreach (var exclude in currentTaxon.Excludes)
            {
                if (exclude.IsNamed)
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

            Common.UpdateTaxonList(taxonButtonGroup_Excludes, excludes);
        }

        // 更新可见性。
        private void _UpdateVisibility()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            grid_Tags.Visibility = currentTaxon.Tags.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            grid_Synonyms.Visibility = currentTaxon.Synonyms.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            stackPanel_TagsAndSynonyms.Visibility = currentTaxon.Tags.Count > 0 || currentTaxon.Synonyms.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            grid_Parents.Visibility = !currentTaxon.IsRoot ? Visibility.Visible : Visibility.Collapsed;
            grid_Children.Visibility = taxonButtonGroup_Children.GetGroupCount() > 0 ? Visibility.Visible : Visibility.Collapsed;
            grid_Excludes.Visibility = currentTaxon.Excludes.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            grid_Desc.Visibility = !string.IsNullOrWhiteSpace(currentTaxon.Description) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void _UpdateTitle()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            taxonTitle.ThemeColor = currentTaxon.GetThemeColor();
            taxonTitle.TaxonName = currentTaxon.GetShortName('\n');
            taxonTitle.Rank = currentTaxon.IsAnonymous ? null : currentTaxon.Rank;
            taxonTitle.IsParaphyly = currentTaxon.IsParaphyly;
            taxonTitle.IsPolyphyly = currentTaxon.IsPolyphyly;

            if ((currentTaxon.IsExtinct && (!currentTaxon.Birth.IsEmpty || !currentTaxon.Extinction.IsEmpty)) || (!currentTaxon.IsExtinct && !currentTaxon.Birth.IsEmpty))
            {
                geoChronSpan.Update(currentTaxon.Birth, currentTaxon.IsExtinct ? currentTaxon.Extinction : GeoChron.Present, currentTaxon.GetInheritedRank());

                geoChronSpan.Visibility = Visibility.Visible;
            }
            else
            {
                geoChronSpan.Visibility = Visibility.Collapsed;
            }
        }

        public void UpdateCurrentTaxonInfo()
        {
            ViewModel.LoadFromTaxon();

            Taxon currentTaxon = Common.CurrentTaxon;

            tagGroup_Tags.UpdateContent(currentTaxon.Tags);
            tagGroup_Synonyms.UpdateContent(currentTaxon.Synonyms);
            tagGroup_Synonyms.ThemeColor = currentTaxon.GetThemeColor();

            _UpdateParents();
            _UpdateChildren();
            _UpdateExcludes();

            _UpdateVisibility();

            _UpdateTitle();
        }
    }
}