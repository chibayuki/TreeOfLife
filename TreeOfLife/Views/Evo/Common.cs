﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
using System.Windows.Input;

using System.Windows.Controls;

using TreeOfLife.Controls;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

namespace TreeOfLife.Views.Evo
{
    public static class Common
    {
        public static Taxon CurrentTaxon { get; set; }

        public static Action<Taxon> SetCurrentTaxon { get; set; }

        public static Action EnterEditMode { get; set; }
        public static Action ExitEditMode { get; set; }

        public static Taxon RightButtonTaxon { get; set; }

        //

        // 更新父类群控件。
        public static void UpdateParents(TaxonNameButtonGroup control, IReadOnlyList<Taxon> parents, ContextMenu menu = null)
        {
            control.StartEditing();
            control.Clear();

            int groupIndex = 0;
            TaxonomicCategory categoryOfGroup = TaxonomicCategory.Unranked;

            for (int i = 0; i < parents.Count; i++)
            {
                Taxon taxon = parents[i];

                if (i == 0)
                {
                    categoryOfGroup = taxon.Category.BasicCategory();

                    control.AddGroup(((categoryOfGroup.IsPrimaryCategory() || categoryOfGroup.IsSecondaryCategory()) ? categoryOfGroup.Name() : string.Empty), taxon.GetThemeColor());
                }
                else
                {
                    TaxonomicCategory basicCategory = taxon.GetInheritedBasicCategory();

                    if (categoryOfGroup != basicCategory)
                    {
                        categoryOfGroup = basicCategory;

                        control.AddGroup(categoryOfGroup.Name(), taxon.GetThemeColor());

                        groupIndex++;
                    }
                }

                TaxonNameButton button = new TaxonNameButton() { Taxon = taxon };

                button.MouseLeftButtonUp += (s, e) => SetCurrentTaxon(taxon);

                if (menu != null)
                {
                    button.ContextMenu = menu;

                    button.MouseRightButtonUp += (s, e) => { RightButtonTaxon = button.Taxon; (button.ContextMenu.DataContext as Action)?.Invoke(); };
                }

                if (taxon == CurrentTaxon)
                {
                    button.Checked = true;
                }

                control.AddButton(button, groupIndex);
            }

            control.FinishEditing();
        }

        // 更新子类群控件。
        public static void UpdateChildren(TaxonNameButtonGroup control, IReadOnlyList<Taxon> children, ContextMenu menu = null)
        {
            control.StartEditing();
            control.Clear();

            for (int i = 0; i < children.Count; i++)
            {
                Taxon taxon = children[i];

                control.AddGroup(string.Empty, taxon.GetThemeColor());

                TaxonNameButton button = new TaxonNameButton() { Taxon = taxon };

                button.MouseLeftButtonUp += (s, e) => SetCurrentTaxon(taxon);

                if (menu != null)
                {
                    button.ContextMenu = menu;

                    button.MouseRightButtonUp += (s, e) => { RightButtonTaxon = button.Taxon; (button.ContextMenu.DataContext as Action)?.Invoke(); };
                }

                control.AddButton(button, i);
            }

            control.FinishEditing();
        }
    }
}