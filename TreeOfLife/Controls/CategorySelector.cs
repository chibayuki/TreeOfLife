/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.415.1000.M5.201204-2200

This file is part of "生命树" (TreeOfLife)

"生命树" (TreeOfLife) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeOfLife
{
    public partial class CategorySelector : UserControl
    {
        private class _Group
        {
            private bool _DarkTheme = false; // 是否为暗色主题。

            private Panel _GroupPanel = new Panel();
            private List<CategoryNameButton> _Buttons = new List<CategoryNameButton>();

            public _Group(bool isDarkTheme)
            {
                _DarkTheme = isDarkTheme;
            }

            public bool IsDarkTheme
            {
                get
                {
                    return _DarkTheme;
                }

                set
                {
                    _DarkTheme = value;

                    UpdateColor();
                }
            }

            public Panel GroupPanel => _GroupPanel;

            public List<CategoryNameButton> Buttons => _Buttons;

            public void UpdateFont(Font font)
            {
                foreach (var button in _Buttons)
                {
                    button.Font = font;
                }
            }

            public void UpdateColor()
            {
                foreach (var button in _Buttons)
                {
                    button.IsDarkTheme = _DarkTheme;
                }
            }

            public void UpdateLayout(int groupWidth, Size minButtonSize, Padding buttonPadding)
            {
                if (_Buttons.Count > 0)
                {
                    for (int i = 0; i < _Buttons.Count; i++)
                    {
                        _Buttons[i].MinimumSize = minButtonSize;
                        _Buttons[i].AutoSize = false;
                        _Buttons[i].AutoSize = true;

                        if (i > 0)
                        {
                            if (_Buttons[i - 1].Right + buttonPadding.Vertical + _Buttons[i].Width > groupWidth)
                            {
                                _Buttons[i].Location = new Point(0, _Buttons[i - 1].Bottom + buttonPadding.Vertical);
                            }
                            else
                            {
                                _Buttons[i].Location = new Point(_Buttons[i - 1].Right + buttonPadding.Horizontal, _Buttons[i - 1].Top);
                            }
                        }
                        else
                        {
                            _Buttons[i].Location = new Point(0, 0);
                        }
                    }

                    _GroupPanel.Size = new Size(groupWidth, _Buttons[_Buttons.Count - 1].Bottom);
                }
                else
                {
                    _GroupPanel.Size = new Size(groupWidth, 0);
                }
            }
        }

        //

        private CategoryNameButton _PrimaryButton = null;
        private CategoryNameButton _SecondaryButton = null;
        private CategoryNameButton _CladeButton = null;
        private CategoryNameButton _UnrankedButton = null;
        private _Group _Level1Group = null;
        private Dictionary<CategoryNameButton, _Group> _Level2Groups = new Dictionary<CategoryNameButton, _Group>();
        private Dictionary<CategoryNameButton, _Group> _Level3Groups = new Dictionary<CategoryNameButton, _Group>();

        private CategoryNameButton _Level1Button = null;
        private CategoryNameButton _Level2Button = null;
        private CategoryNameButton _Level3Button = null;
        private _Group _Level2Group = null;
        private _Group _Level3Group = null;

        private Size _MinButtonSize = new Size(30, 22); // 按钮最小大小。
        private Padding _ButtonPadding = new Padding(2, 2, 2, 2); // 按钮外边距。
        private Padding _GroupPadding = new Padding(0, 4, 0, 4); // 组外边距。

        private bool _DarkTheme = false; // 是否为暗色主题。

        private TaxonomicCategory _Category = TaxonomicCategory.Unranked; // 当前选择的分类阶元。

        //

        public CategorySelector()
        {
            InitializeComponent();

            //

            _InitLevel1();
            _InitLevel2();
            _InitLevel3();

            //

            this.Load += CategorySelector_Load;
            this.SizeChanged += CategorySelector_SizeChanged;
            this.AutoSizeChanged += CategorySelector_AutoSizeChanged;
            this.FontChanged += CategorySelector_FontChanged;
            Panel_Main.Paint += Panel_Main_Paint;
        }

        private void _InitLevel1()
        {
            _Level1Group = new _Group(_DarkTheme);

            _Level1Group.GroupPanel.Visible = true;
            _Level1Group.GroupPanel.Location = new Point(0, 0);

            _PrimaryButton = new CategoryNameButton()
            {
                AutoSize = true,
                MinimumSize = _MinButtonSize,
                CategoryName = "主要级别",
                ThemeColor = Color.FromArgb(128, 128, 128)
            };

            _PrimaryButton.Click += (s, e) => _UpdateCategoryAndOnClick(TaxonomicCategory.Domain);

            _Level1Group.Buttons.Add(_PrimaryButton);
            _Level1Group.GroupPanel.Controls.Add(_PrimaryButton);

            _SecondaryButton = new CategoryNameButton()
            {
                AutoSize = true,
                MinimumSize = _MinButtonSize,
                CategoryName = "次要级别",
                ThemeColor = Color.FromArgb(128, 128, 128)
            };

            _SecondaryButton.Click += (s, e) => _UpdateCategoryAndOnClick(TaxonomicCategory.Tribe);

            _Level1Group.Buttons.Add(_SecondaryButton);
            _Level1Group.GroupPanel.Controls.Add(_SecondaryButton);

            _CladeButton = new CategoryNameButton()
            {
                AutoSize = true,
                MinimumSize = _MinButtonSize,
                Category = TaxonomicCategory.Clade,
                ThemeColor = Color.FromArgb(128, 128, 128)
            };

            _CladeButton.Click += (s, e) => _UpdateCategoryAndOnClick(TaxonomicCategory.Clade);

            _Level1Group.Buttons.Add(_CladeButton);
            _Level1Group.GroupPanel.Controls.Add(_CladeButton);

            _UnrankedButton = new CategoryNameButton()
            {
                AutoSize = true,
                MinimumSize = _MinButtonSize,
                Category = TaxonomicCategory.Unranked,
                ThemeColor = Color.FromArgb(128, 128, 128)
            };

            _UnrankedButton.Click += (s, e) => _UpdateCategoryAndOnClick(TaxonomicCategory.Unranked);

            _Level1Group.Buttons.Add(_UnrankedButton);
            _Level1Group.GroupPanel.Controls.Add(_UnrankedButton);

            Panel_Level1.Controls.Add(_Level1Group.GroupPanel);
        }

        private void _InitLevel2()
        {
            _Group primaryGroup = new _Group(_DarkTheme);

            primaryGroup.GroupPanel.Visible = false;
            primaryGroup.GroupPanel.Location = new Point(0, 0);

            TaxonomicCategory[] primaryCategories =
            {
                TaxonomicCategory.Domain,
                TaxonomicCategory.Kingdom,
                TaxonomicCategory.Phylum,
                TaxonomicCategory.Class,
                TaxonomicCategory.Order,
                TaxonomicCategory.Family,
                TaxonomicCategory.Genus,
                TaxonomicCategory.Species
            };

            foreach (var category in primaryCategories)
            {
                CategoryNameButton button2 = new CategoryNameButton()
                {
                    AutoSize = true,
                    MinimumSize = _MinButtonSize,
                    Category = category,
                    ThemeColor = category.GetThemeColor()
                };

                primaryGroup.Buttons.Add(button2);
                primaryGroup.GroupPanel.Controls.Add(button2);

                button2.Click += (s, e) => _UpdateCategoryAndOnClick(button2.Category.Value);
            }

            Panel_Level2.Controls.Add(primaryGroup.GroupPanel);

            _Level2Groups.Add(_PrimaryButton, primaryGroup);

            //

            _Group secondaryGroup = new _Group(_DarkTheme);

            secondaryGroup.GroupPanel.Visible = false;
            secondaryGroup.GroupPanel.Location = new Point(0, 0);

            TaxonomicCategory[] secondaryCategories =
            {
                TaxonomicCategory.Tribe,
                TaxonomicCategory.Cohort,
                TaxonomicCategory.Section,
                TaxonomicCategory.Division,
                TaxonomicCategory.Series,
                TaxonomicCategory.Form,
                TaxonomicCategory.Strain
            };

            foreach (var category in secondaryCategories)
            {
                CategoryNameButton button2 = new CategoryNameButton()
                {
                    AutoSize = true,
                    MinimumSize = _MinButtonSize,
                    Category = category,
                    ThemeColor = category.GetThemeColor()
                };

                secondaryGroup.Buttons.Add(button2);
                secondaryGroup.GroupPanel.Controls.Add(button2);

                button2.Click += (s, e) => _UpdateCategoryAndOnClick(button2.Category.Value);
            }

            Panel_Level2.Controls.Add(secondaryGroup.GroupPanel);

            _Level2Groups.Add(_SecondaryButton, secondaryGroup);
        }

        private void _InitLevel3()
        {
            foreach (var group2 in _Level2Groups.Values)
            {
                foreach (var button2 in group2.Buttons)
                {
                    _Group group3 = new _Group(_DarkTheme);

                    group3.GroupPanel.Visible = false;
                    group3.GroupPanel.Location = new Point(0, 0);

                    TaxonomicCategory[] categories = null;

                    switch (button2.Category.Value)
                    {
                        case TaxonomicCategory.Domain:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Superdomain,
                                TaxonomicCategory.Domain
                            };
                            break;

                        case TaxonomicCategory.Kingdom:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Superkingdom,
                                TaxonomicCategory.Kingdom,
                                TaxonomicCategory.Subkingdom,
                                TaxonomicCategory.Infrakingdom
                            };
                            break;

                        case TaxonomicCategory.Phylum:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Superphylum,
                                TaxonomicCategory.Phylum,
                                TaxonomicCategory.Subphylum,
                                TaxonomicCategory.Infraphylum,
                                TaxonomicCategory.Parvphylum
                            };
                            break;

                        case TaxonomicCategory.Class:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Megaclass,
                                TaxonomicCategory.Superclass,
                                TaxonomicCategory.Grandclass,
                                TaxonomicCategory.Hyperclass,
                                TaxonomicCategory.Class,
                                TaxonomicCategory.Subclass,
                                TaxonomicCategory.Infraclass,
                                TaxonomicCategory.Parvclass
                            };
                            break;

                        case TaxonomicCategory.Order:
                            categories = new TaxonomicCategory[]
                            {
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
                                TaxonomicCategory.Parvorder
                            };
                            break;

                        case TaxonomicCategory.Family:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Gigafamily,
                                TaxonomicCategory.Megafamily,
                                TaxonomicCategory.Superfamily,
                                TaxonomicCategory.Grandfamily,
                                TaxonomicCategory.Hyperfamily,
                                TaxonomicCategory.Epifamily,
                                TaxonomicCategory.Family,
                                TaxonomicCategory.Subfamily,
                                TaxonomicCategory.Infrafamily
                            };
                            break;

                        case TaxonomicCategory.Genus:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Genus,
                                TaxonomicCategory.Subgenus,
                                TaxonomicCategory.Infragenus
                            };
                            break;

                        case TaxonomicCategory.Species:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Superspecies,
                                TaxonomicCategory.Species,
                                TaxonomicCategory.Subspecies,
                                TaxonomicCategory.Variety,
                                TaxonomicCategory.Subvariety
                            };
                            break;

                        //

                        case TaxonomicCategory.Tribe:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Supertribe,
                                TaxonomicCategory.Tribe,
                                TaxonomicCategory.Subtribe,
                                TaxonomicCategory.Infratribe
                            };
                            break;

                        case TaxonomicCategory.Cohort:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Megacohort,
                                TaxonomicCategory.Supercohort,
                                TaxonomicCategory.Cohort,
                                TaxonomicCategory.Subcohort,
                                TaxonomicCategory.Infracohort
                            };
                            break;

                        case TaxonomicCategory.Section:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Supersection,
                                TaxonomicCategory.Section,
                                TaxonomicCategory.Subsection,
                                TaxonomicCategory.Infrasection
                            };
                            break;

                        case TaxonomicCategory.Division:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Superdivision,
                                TaxonomicCategory.Division,
                                TaxonomicCategory.Subdivision,
                                TaxonomicCategory.Infradivision
                            };
                            break;

                        case TaxonomicCategory.Series:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Series,
                                TaxonomicCategory.Subseries
                            };
                            break;

                        case TaxonomicCategory.Form:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Form,
                                TaxonomicCategory.Subform
                            };
                            break;

                        case TaxonomicCategory.Strain:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Strain
                            };
                            break;
                    }

                    foreach (var category in categories)
                    {
                        CategoryNameButton button3 = new CategoryNameButton()
                        {
                            AutoSize = true,
                            MinimumSize = _MinButtonSize,
                            Category = category,
                            ThemeColor = category.GetThemeColor()
                        };

                        group3.Buttons.Add(button3);
                        group3.GroupPanel.Controls.Add(button3);

                        button3.Click += (s, e) => _UpdateCategoryAndOnClick(button3.Category.Value);
                    }

                    Panel_Level3.Controls.Add(group3.GroupPanel);

                    _Level3Groups.Add(button2, group3);
                }
            }
        }

        //

        private void CategorySelector_Load(object sender, EventArgs e)
        {
            _UpdateFont();
            _UpdateCategory();
            _UpdateLayout();
        }

        private void CategorySelector_SizeChanged(object sender, EventArgs e)
        {
            _UpdateLayout();
        }

        private void CategorySelector_AutoSizeChanged(object sender, EventArgs e)
        {
            _AutoSizeChanged();
        }

        private void CategorySelector_FontChanged(object sender, EventArgs e)
        {
            _UpdateFont();
        }

        private void Panel_Main_Paint(object sender, PaintEventArgs e)
        {
            _UpdateGroupSeparatorLine();
        }

        //

        private void _AutoSizeChanged()
        {
            if (this.AutoSize)
            {
                _AutoSize();
            }
        }

        private void _AutoSize()
        {
            this.Height = Panel_Level3.Bottom;
        }

        private void _UpdateFont()
        {
            _Level1Group.UpdateFont(this.Font);

            foreach (var group in _Level2Groups.Values)
            {
                group.UpdateFont(this.Font);
            }

            foreach (var group in _Level3Groups.Values)
            {
                group.UpdateFont(this.Font);
            }
        }

        private void _UpdateColor()
        {
            _Level1Group.IsDarkTheme = _DarkTheme;
            _Level1Group.UpdateColor();

            foreach (var group in _Level2Groups.Values)
            {
                group.IsDarkTheme = _DarkTheme;
                group.UpdateColor();
            }

            foreach (var group in _Level3Groups.Values)
            {
                group.IsDarkTheme = _DarkTheme;
                group.UpdateColor();
            }
        }

        private void _UpdateLayout()
        {
            _Level1Group.UpdateLayout(this.Width, _MinButtonSize, _ButtonPadding);

            foreach (var group in _Level2Groups.Values)
            {
                group.UpdateLayout(this.Width, _MinButtonSize, _ButtonPadding);
            }

            foreach (var group in _Level3Groups.Values)
            {
                group.UpdateLayout(this.Width, _MinButtonSize, _ButtonPadding);
            }

            Panel_Level1.Size = new Size(this.Width, _Level1Group.GroupPanel.Height);
            Panel_Level2.Size = new Size(this.Width, (_CurrentLevel2Group == null ? 0 : _CurrentLevel2Group.GroupPanel.Height));
            Panel_Level3.Size = new Size(this.Width, (_CurrentLevel3Group == null ? 0 : _CurrentLevel3Group.GroupPanel.Height));

            Panel_Level1.Top = 0;
            Panel_Level2.Top = Panel_Level1.Bottom + (Panel_Level2.Height > 0 ? _GroupPadding.Vertical : 0);
            Panel_Level3.Top = Panel_Level2.Bottom + (Panel_Level3.Height > 0 ? _GroupPadding.Vertical : 0);

            //

            if (this.AutoSize)
            {
                _AutoSize();
            }
        }

        private void _UpdateGroupSeparatorLine()
        {
            using (Graphics graphics = Panel_Main.CreateGraphics())
            {
                if (_CurrentLevel1Button != null && _CurrentLevel2Group != null)
                {
                    graphics.DrawLine(new Pen(_CurrentLevel1Button.ThemeColor.ToColor(), 1F), new Point(0, Panel_Level1.Bottom + _GroupPadding.Bottom), new Point(Panel_Main.Width, Panel_Level1.Bottom + _GroupPadding.Bottom));
                }

                if (_CurrentLevel2Button != null && _CurrentLevel3Group != null)
                {
                    graphics.DrawLine(new Pen(_CurrentLevel2Button.ThemeColor.ToColor(), 1F), new Point(0, Panel_Level2.Bottom + _GroupPadding.Bottom), new Point(Panel_Main.Width, Panel_Level2.Bottom + _GroupPadding.Bottom));
                }
            }
        }

        private void _UpdateCategory()
        {
            _Level2Button = null;
            _Level3Button = null;

            if (_Category == TaxonomicCategory.Clade || _Category == TaxonomicCategory.Unranked)
            {
                if (_Category == TaxonomicCategory.Clade)
                {
                    _CurrentLevel1Button = _CladeButton;
                }
                else
                {
                    _CurrentLevel1Button = _UnrankedButton;
                }

                _CurrentLevel2Group = null;
                _CurrentLevel3Group = null;
            }
            else
            {
                if (_Category.IsPrimaryCategory())
                {
                    _CurrentLevel1Button = _PrimaryButton;
                    _CurrentLevel2Group = _Level2Groups[_PrimaryButton];
                }
                else
                {
                    _CurrentLevel1Button = _SecondaryButton;
                    _CurrentLevel2Group = _Level2Groups[_SecondaryButton];
                }

                //

                foreach (var button in _CurrentLevel2Group.Buttons)
                {
                    if (_Category.BasicCategory() == button.Category.Value)
                    {
                        button.Checked = true;

                        _Level2Button = button;
                    }
                    else
                    {
                        button.Checked = false;
                    }
                }

                //

                _CurrentLevel3Group = _Level3Groups[_Level2Button];

                foreach (var button in _CurrentLevel3Group.Buttons)
                {
                    if (_Category == button.Category.Value)
                    {
                        button.Checked = true;

                        _Level3Button = button;
                    }
                    else
                    {
                        button.Checked = false;
                    }
                }
            }

            //

            _UpdateLayout();

            //

            _UpdateGroupSeparatorLine();
        }

        private void _UpdateCategoryAndOnClick(TaxonomicCategory category)
        {
            if (_Category != category)
            {
                _Category = category;

                _UpdateCategory();

                base.OnClick(EventArgs.Empty);
            }
        }

        //

        private CategoryNameButton _CurrentLevel1Button
        {
            get
            {
                return _Level1Button;
            }

            set
            {
                if (_Level1Button != null)
                {
                    _Level1Button.Checked = false;
                }

                _Level1Button = value;

                if (_Level1Button != null)
                {
                    _Level1Button.Checked = true;
                }
            }
        }

        private CategoryNameButton _CurrentLevel2Button => _Level2Button;

        private CategoryNameButton _CurrentLevel3Button => _Level3Button;

        private _Group _CurrentLevel2Group
        {
            get
            {
                return _Level2Group;
            }

            set
            {
                if (_Level2Group != null)
                {
                    _Level2Group.GroupPanel.Visible = false;
                }

                _Level2Group = value;

                if (_Level2Group != null)
                {
                    _Level2Group.GroupPanel.Visible = true;
                }
            }
        }

        private _Group _CurrentLevel3Group
        {
            get
            {
                return _Level3Group;
            }

            set
            {
                if (_Level3Group != null)
                {
                    _Level3Group.GroupPanel.Visible = false;
                }

                _Level3Group = value;

                if (_Level3Group != null)
                {
                    _Level3Group.GroupPanel.Visible = true;
                }
            }
        }

        //

        public Size MinButtonSize
        {
            get
            {
                return _MinButtonSize;
            }

            set
            {
                _MinButtonSize = value;

                _UpdateLayout();
            }
        }

        public Padding ButtonPadding
        {
            get
            {
                return _ButtonPadding;
            }

            set
            {
                _ButtonPadding = value;

                _UpdateLayout();
            }
        }

        public Padding GroupPadding
        {
            get
            {
                return _GroupPadding;
            }

            set
            {
                _GroupPadding = value;

                _UpdateLayout();
            }
        }

        public bool IsDarkTheme
        {
            get
            {
                return _DarkTheme;
            }

            set
            {
                _DarkTheme = value;

                _UpdateColor();
            }
        }

        public TaxonomicCategory Category
        {
            get
            {
                return _Category;
            }

            set
            {
                _Category = value;

                _UpdateCategory();
            }
        }
    }
}