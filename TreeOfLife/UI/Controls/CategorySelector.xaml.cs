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

using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;

namespace TreeOfLife.UI.Controls
{
    /// <summary>
    /// CategorySelector.xaml 的交互逻辑
    /// </summary>
    public partial class CategorySelector : UserControl
    {
        private Dictionary<Category, CategoryNameButton> _BasicCategoryButtons = new Dictionary<Category, CategoryNameButton>();
        private Dictionary<Category, CategoryNameButton> _CategoriesButtons = new Dictionary<Category, CategoryNameButton>();
        private Dictionary<Category, Panel> _CategoriesPanels = new Dictionary<Category, Panel>();

        //

        private void radioButton_PrimaryCategory_Checked(object sender, RoutedEventArgs e) => Category = Category.Domain;
        private void radioButton_SecondaryCategory_Checked(object sender, RoutedEventArgs e) => Category = Category.Tribe;
        private void radioButton_Clade_Checked(object sender, RoutedEventArgs e) => Category = Category.Clade;
        private void radioButton_Unranked_Checked(object sender, RoutedEventArgs e) => Category = Category.Unranked;

        private void _AddRadioButtonsEvents()
        {
            radioButton_PrimaryCategory.Checked += radioButton_PrimaryCategory_Checked;
            radioButton_SecondaryCategory.Checked += radioButton_SecondaryCategory_Checked;
            radioButton_Clade.Checked += radioButton_Clade_Checked;
            radioButton_Unranked.Checked += radioButton_Unranked_Checked;
        }

        private void _RemoveRadioButtonsEvents()
        {
            radioButton_PrimaryCategory.Checked -= radioButton_PrimaryCategory_Checked;
            radioButton_SecondaryCategory.Checked -= radioButton_SecondaryCategory_Checked;
            radioButton_Clade.Checked -= radioButton_Clade_Checked;
            radioButton_Unranked.Checked -= radioButton_Unranked_Checked;
        }

        //

        private void _InitBasicCategoryControls(Panel basicCategoryPanel, Category basicCategory, Panel categoriesPanel, IEnumerable<Category> categories)
        {
            CategoryNameButton button_Basic = new CategoryNameButton()
            {
                Category = basicCategory,
                Margin = new Thickness(0, 6, 6, 0)
            };

            _BasicCategoryButtons.Add(basicCategory, button_Basic);

            basicCategoryPanel.Children.Add(button_Basic);

            //

            WrapPanel wrapPanel = new WrapPanel()
            {
                Orientation = Orientation.Horizontal,
                Visibility = Visibility.Collapsed
            };

            _CategoriesPanels.Add(basicCategory, wrapPanel);

            foreach (var category in categories)
            {
                CategoryNameButton button = new CategoryNameButton()
                {
                    Category = category,
                    Margin = new Thickness(0, 6, 6, 0)
                };

                _CategoriesButtons.Add(category, button);

                wrapPanel.Children.Add(button);
            }

            categoriesPanel.Children.Add(wrapPanel);
        }

        private void _InitCategoryControls()
        {
            foreach (var item in new (Category basicCategory, Category[] categories)[] {
                (Category.Domain, new Category[] {
                    Category.Superdomain,
                    Category.Domain }),
                (Category.Kingdom, new Category[] {
                    Category.Superkingdom,
                    Category.Kingdom,
                    Category.Subkingdom,
                    Category.Infrakingdom }),
                (Category.Phylum, new Category[] {
                    Category.Superphylum,
                    Category.Phylum,
                    Category.Subphylum,
                    Category.Infraphylum,
                    Category.Parvphylum }),
                (Category.Class, new Category[] {
                    Category.Megaclass,
                    Category.Superclass,
                    Category.Grandclass,
                    Category.Hyperclass,
                    Category.Class,
                    Category.Subclass,
                    Category.Infraclass,
                    Category.Parvclass }),
                (Category.Order, new Category[] {
                    Category.Gigaorder,
                    Category.Megaorder,
                    Category.Superorder,
                    Category.Grandorder,
                    Category.Hyperorder,
                    Category.Order,
                    Category.Nanorder,
                    Category.Hypoorder,
                    Category.Minorder,
                    Category.Suborder,
                    Category.Infraorder,
                    Category.Parvorder }),
                (Category.Family, new Category[] {
                    Category.Gigafamily,
                    Category.Megafamily,
                    Category.Superfamily,
                    Category.Grandfamily,
                    Category.Hyperfamily,
                    Category.Epifamily,
                    Category.Family,
                    Category.Subfamily,
                    Category.Infrafamily }),
                (Category.Genus, new Category[] {
                    Category.Genus,
                    Category.Subgenus,
                    Category.Infragenus }),
                (Category.Species, new Category[] {
                    Category.Superspecies,
                    Category.Species,
                    Category.Subspecies,
                    Category.Variety,
                    Category.Subvariety })
            })
            {
                _InitBasicCategoryControls(stackPanel_BasicPrimaryCategories, item.basicCategory, grid_PrimaryCategories, item.categories);
            }

            foreach (var item in new (Category basicCategory, Category[] categories)[] {
                (Category.Tribe, new Category[] {
                    Category.Supertribe,
                    Category.Tribe,
                    Category.Subtribe,
                    Category.Infratribe }),
                (Category.Cohort, new Category[] {
                    Category.Megacohort,
                    Category.Supercohort,
                    Category.Cohort,
                    Category.Subcohort,
                    Category.Infracohort }),
                (Category.Section, new Category[] {
                    Category.Supersection,
                    Category.Section,
                    Category.Subsection,
                    Category.Infrasection }),
                (Category.Division, new Category[] {
                    Category.Superdivision,
                    Category.Division,
                    Category.Subdivision,
                    Category.Infradivision }),
                (Category.Series, new Category[] {
                    Category.Series,
                    Category.Subseries }),
                (Category.Form, new Category[] {
                    Category.Form,
                    Category.Subform }),
                (Category.Strain, new Category[] {
                    Category.Strain })
            })
            {
                _InitBasicCategoryControls(stackPanel_BasicSecondaryCategories, item.basicCategory, grid_SecondaryCategories, item.categories);
            }

            //

            CategoryNameButton button = null;

            grid_Categories.AddHandler(UIElement.MouseLeftButtonDownEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is CategoryNameButton source)
                {
                    button = source;
                }
            }));

            grid_Categories.AddHandler(UIElement.MouseLeftButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is CategoryNameButton source && source == button)
                {
                    Category = source.Category;
                    button = null;
                }
            }));
        }

        //

        private Category _Category = Category.Unranked; // 当前选择的分类阶元。

        private void _UpdateCategory(Category oldCategory, Category newCategory)
        {
            if (oldCategory != newCategory)
            {
                if (oldCategory.IsPrimaryOrSecondaryCategory() && newCategory.IsPrimaryOrSecondaryCategory())
                {
                    CategoryNameButton button;
                    Panel panel;

                    //

                    if (oldCategory.IsPrimaryCategory())
                    {
                        stackPanel_PrimaryCategory.Visibility = Visibility.Collapsed;
                    }
                    else if (oldCategory.IsSecondaryCategory())
                    {
                        stackPanel_SecondaryCategory.Visibility = Visibility.Collapsed;
                    }

                    if (newCategory.IsPrimaryCategory())
                    {
                        radioButton_PrimaryCategory.IsChecked = true;
                        stackPanel_PrimaryCategory.Visibility = Visibility.Visible;
                    }
                    else if (newCategory.IsSecondaryCategory())
                    {
                        radioButton_SecondaryCategory.IsChecked = true;
                        stackPanel_SecondaryCategory.Visibility = Visibility.Visible;
                    }

                    //

                    if (_BasicCategoryButtons.TryGetValue(oldCategory.BasicCategory(), out button) && button is not null)
                    {
                        button.IsChecked = false;
                    }

                    if (_BasicCategoryButtons.TryGetValue(newCategory.BasicCategory(), out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    //

                    if (_CategoriesButtons.TryGetValue(oldCategory, out button) && button is not null)
                    {
                        button.IsChecked = false;
                    }

                    if (_CategoriesButtons.TryGetValue(newCategory, out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    //

                    if (_CategoriesPanels.TryGetValue(oldCategory.BasicCategory(), out panel) && panel is not null)
                    {
                        panel.Visibility = Visibility.Collapsed;
                    }

                    if (_CategoriesPanels.TryGetValue(newCategory.BasicCategory(), out panel) && panel is not null)
                    {
                        panel.Visibility = Visibility.Visible;
                    }
                }
                else if (oldCategory.IsPrimaryOrSecondaryCategory())
                {
                    if (oldCategory.IsPrimaryCategory())
                    {
                        stackPanel_PrimaryCategory.Visibility = Visibility.Collapsed;
                    }
                    else if (oldCategory.IsSecondaryCategory())
                    {
                        stackPanel_SecondaryCategory.Visibility = Visibility.Collapsed;
                    }

                    foreach (var item in _BasicCategoryButtons)
                    {
                        if (item.Value.IsChecked)
                        {
                            item.Value.IsChecked = false;
                        }
                    }

                    foreach (var item in _CategoriesButtons)
                    {
                        if (item.Value.IsChecked)
                        {
                            item.Value.IsChecked = false;
                        }
                    }

                    foreach (var item in _CategoriesPanels)
                    {
                        item.Value.Visibility = Visibility.Collapsed;
                    }

                    //

                    if (newCategory.IsClade())
                    {
                        radioButton_Clade.IsChecked = true;
                    }
                    else if (newCategory.IsUnranked())
                    {
                        radioButton_Unranked.IsChecked = true;
                    }
                }
                else if (newCategory.IsPrimaryOrSecondaryCategory())
                {
                    CategoryNameButton button;
                    Panel panel;

                    if (newCategory.IsPrimaryCategory())
                    {
                        radioButton_PrimaryCategory.IsChecked = true;
                        stackPanel_PrimaryCategory.Visibility = Visibility.Visible;
                    }
                    else if (newCategory.IsSecondaryCategory())
                    {
                        radioButton_SecondaryCategory.IsChecked = true;
                        stackPanel_SecondaryCategory.Visibility = Visibility.Visible;
                    }

                    if (_BasicCategoryButtons.TryGetValue(newCategory.BasicCategory(), out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    if (_CategoriesButtons.TryGetValue(newCategory, out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    if (_CategoriesPanels.TryGetValue(newCategory.BasicCategory(), out panel) && panel is not null)
                    {
                        panel.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (newCategory.IsClade())
                    {
                        radioButton_Clade.IsChecked = true;
                    }
                    else if (newCategory.IsUnranked())
                    {
                        radioButton_Unranked.IsChecked = true;
                    }
                }
            }
        }

        //

        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateTheme()
        {
            foreach (var item in _BasicCategoryButtons)
            {
                item.Value.IsDarkTheme = _IsDarkTheme;
            }

            foreach (var item in _CategoriesButtons)
            {
                item.Value.IsDarkTheme = _IsDarkTheme;
            }
        }

        //

        public CategorySelector()
        {
            InitializeComponent();

            //

            _AddRadioButtonsEvents();

            _InitCategoryControls();
        }

        //

        public Category Category
        {
            get => _Category;

            set
            {
                if (_Category != value)
                {
                    _RemoveRadioButtonsEvents();
                    _UpdateCategory(_Category, value);
                    _AddRadioButtonsEvents();

                    _Category = value;

                    CategoryChanged?.Invoke(this, _Category);
                }
            }
        }

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateTheme();
            }
        }

        //

        public EventHandler<Category> CategoryChanged;
    }
}