/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// CategorySelector.xaml 的交互逻辑
    /// </summary>
    public partial class CategorySelector : UserControl
    {
        private Dictionary<TaxonomicCategory, CategoryNameButton> _BasicCategoryButtons = new Dictionary<TaxonomicCategory, CategoryNameButton>();
        private Dictionary<TaxonomicCategory, CategoryNameButton> _CategoriesButtons = new Dictionary<TaxonomicCategory, CategoryNameButton>();
        private Dictionary<TaxonomicCategory, Panel> _CategoriesPanels = new Dictionary<TaxonomicCategory, Panel>();

        //

        private void radioButton_PrimaryCategory_Checked(object sender, RoutedEventArgs e) => Category = TaxonomicCategory.Domain;
        private void radioButton_SecondaryCategory_Checked(object sender, RoutedEventArgs e) => Category = TaxonomicCategory.Tribe;
        private void radioButton_Clade_Checked(object sender, RoutedEventArgs e) => Category = TaxonomicCategory.Clade;
        private void radioButton_Unranked_Checked(object sender, RoutedEventArgs e) => Category = TaxonomicCategory.Unranked;

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

        private void _InitBasicCategoryControls(Panel basicCategoryPanel, TaxonomicCategory basicCategory, Panel categoriesPanel, IEnumerable<TaxonomicCategory> categories)
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
            foreach (var item in new (TaxonomicCategory basicCategory, TaxonomicCategory[] categories)[] {
                (TaxonomicCategory.Domain, new TaxonomicCategory[] {
                    TaxonomicCategory.Superdomain,
                    TaxonomicCategory.Domain }),
                (TaxonomicCategory.Kingdom, new TaxonomicCategory[] {
                    TaxonomicCategory.Superkingdom,
                    TaxonomicCategory.Kingdom,
                    TaxonomicCategory.Subkingdom,
                    TaxonomicCategory.Infrakingdom }),
                (TaxonomicCategory.Phylum, new TaxonomicCategory[] {
                    TaxonomicCategory.Superphylum,
                    TaxonomicCategory.Phylum,
                    TaxonomicCategory.Subphylum,
                    TaxonomicCategory.Infraphylum,
                    TaxonomicCategory.Parvphylum }),
                (TaxonomicCategory.Class, new TaxonomicCategory[] {
                    TaxonomicCategory.Megaclass,
                    TaxonomicCategory.Superclass,
                    TaxonomicCategory.Grandclass,
                    TaxonomicCategory.Hyperclass,
                    TaxonomicCategory.Class,
                    TaxonomicCategory.Subclass,
                    TaxonomicCategory.Infraclass,
                    TaxonomicCategory.Parvclass }),
                (TaxonomicCategory.Order, new TaxonomicCategory[] {
                    TaxonomicCategory.Gigaorder,
                    TaxonomicCategory.Megaorder,
                    TaxonomicCategory.Superorder,
                    TaxonomicCategory.Grandorder,
                    TaxonomicCategory.Hyperorder,
                    TaxonomicCategory.Order,
                    TaxonomicCategory.Nanorder,
                    TaxonomicCategory.Hypoorder,
                    TaxonomicCategory.Minorder,
                    TaxonomicCategory.Suborder,
                    TaxonomicCategory.Infraorder,
                    TaxonomicCategory.Parvorder }),
                (TaxonomicCategory.Family, new TaxonomicCategory[] {
                    TaxonomicCategory.Gigafamily,
                    TaxonomicCategory.Megafamily,
                    TaxonomicCategory.Superfamily,
                    TaxonomicCategory.Grandfamily,
                    TaxonomicCategory.Hyperfamily,
                    TaxonomicCategory.Epifamily,
                    TaxonomicCategory.Family,
                    TaxonomicCategory.Subfamily,
                    TaxonomicCategory.Infrafamily }),
                (TaxonomicCategory.Genus, new TaxonomicCategory[] {
                    TaxonomicCategory.Genus,
                    TaxonomicCategory.Subgenus,
                    TaxonomicCategory.Infragenus }),
                (TaxonomicCategory.Species, new TaxonomicCategory[] {
                    TaxonomicCategory.Superspecies,
                    TaxonomicCategory.Species,
                    TaxonomicCategory.Subspecies,
                    TaxonomicCategory.Variety,
                    TaxonomicCategory.Subvariety })
            })
            {
                _InitBasicCategoryControls(stackPanel_BasicPrimaryCategories, item.basicCategory, grid_PrimaryCategories, item.categories);
            }

            foreach (var item in new (TaxonomicCategory basicCategory, TaxonomicCategory[] categories)[] {
                (TaxonomicCategory.Tribe, new TaxonomicCategory[] {
                    TaxonomicCategory.Supertribe,
                    TaxonomicCategory.Tribe,
                    TaxonomicCategory.Subtribe,
                    TaxonomicCategory.Infratribe }),
                (TaxonomicCategory.Cohort, new TaxonomicCategory[] {
                    TaxonomicCategory.Megacohort,
                    TaxonomicCategory.Supercohort,
                    TaxonomicCategory.Cohort,
                    TaxonomicCategory.Subcohort,
                    TaxonomicCategory.Infracohort }),
                (TaxonomicCategory.Section, new TaxonomicCategory[] {
                    TaxonomicCategory.Supersection,
                    TaxonomicCategory.Section,
                    TaxonomicCategory.Subsection,
                    TaxonomicCategory.Infrasection }),
                (TaxonomicCategory.Division, new TaxonomicCategory[] {
                    TaxonomicCategory.Superdivision,
                    TaxonomicCategory.Division,
                    TaxonomicCategory.Subdivision,
                    TaxonomicCategory.Infradivision }),
                (TaxonomicCategory.Series, new TaxonomicCategory[] {
                    TaxonomicCategory.Series,
                    TaxonomicCategory.Subseries }),
                (TaxonomicCategory.Form, new TaxonomicCategory[] {
                    TaxonomicCategory.Form,
                    TaxonomicCategory.Subform }),
                (TaxonomicCategory.Strain, new TaxonomicCategory[] {
                    TaxonomicCategory.Strain })
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

        private TaxonomicCategory _Category = TaxonomicCategory.Unranked; // 当前选择的分类阶元。

        private void _UpdateCategory(TaxonomicCategory oldCategory, TaxonomicCategory newCategory)
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

        public TaxonomicCategory Category
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

        public EventHandler<TaxonomicCategory> CategoryChanged;
    }
}