/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.617.1000.M6.201225-2240

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Media;

using TreeOfLife.Extensions;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Views.Evo.EditMode
{
    public class ViewModel_Evo_EditMode : INotifyPropertyChanged
    {
        public ViewModel_Evo_EditMode()
        {
        }

        //

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //

        #region 类群信息

        private Taxon _Taxon;

        private string _CategoryName;
        private string _TaxonName;

        private string _Name;
        private string _ChsName;

        private bool _IsExtinct;
        private bool _IsUnsure;

        private TaxonomicCategory _Category;

        private string _Synonyms;
        private string _Tags;
        private string _Description;

        private void _UpdateTitle()
        {
            if (_Taxon != null)
            {
                TaxonomicCategory category = _Category;

                TaxonColor = (_Taxon.IsRoot || category.IsPrimaryCategory() || category.IsSecondaryCategory() ? category.GetThemeColor() : _Taxon.Parent.GetThemeColor());

                string name = _Name?.Trim();
                string chsName = _ChsName?.Trim();

                if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(chsName))
                {
                    CategoryName = string.Empty;
                    TaxonName = (_Taxon.IsRoot ? string.Empty : "(未命名)");
                }
                else
                {
                    CategoryName = category.Name();

                    StringBuilder taxonName = new StringBuilder();

                    if (_IsUnsure || _IsExtinct)
                    {
                        if (_IsUnsure)
                        {
                            taxonName.Append('?');
                        }

                        if (_IsExtinct)
                        {
                            taxonName.Append('†');
                        }

                        taxonName.Append(' ');
                    }

                    if (!string.IsNullOrWhiteSpace(chsName))
                    {
                        taxonName.Append(chsName);

                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            taxonName.Append('\n');
                            taxonName.Append(name);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(name))
                    {
                        taxonName.Append(name);
                    }

                    TaxonName = taxonName.ToString();
                }

                _UpdateColors();
            }
        }

        public Taxon Taxon => _Taxon;

        public string CategoryName
        {
            get => _CategoryName;

            set
            {
                if (_CategoryName != value)
                {
                    _CategoryName = value;

                    NotifyPropertyChanged(nameof(CategoryName));
                }
            }
        }

        public string TaxonName
        {
            get => _TaxonName;

            set
            {
                if (_TaxonName != value)
                {
                    _TaxonName = value;

                    NotifyPropertyChanged(nameof(TaxonName));
                }
            }
        }

        public string Name
        {
            get => _Name;

            set
            {
                if (_Name != value)
                {
                    _Name = value;

                    NotifyPropertyChanged(nameof(Name));

                    _UpdateTitle();
                }
            }
        }

        public string ChsName
        {
            get => _ChsName;

            set
            {
                if (_ChsName != value)
                {
                    _ChsName = value;

                    NotifyPropertyChanged(nameof(ChsName));

                    _UpdateTitle();
                }
            }
        }

        public bool IsExtinct
        {
            get => _IsExtinct;

            set
            {
                if (_IsExtinct != value)
                {
                    _IsExtinct = value;

                    NotifyPropertyChanged(nameof(IsExtinct));

                    _UpdateTitle();
                }
            }
        }

        public bool IsUnsure
        {
            get => _IsUnsure;

            set
            {
                if (_IsUnsure != value)
                {
                    _IsUnsure = value;

                    NotifyPropertyChanged(nameof(IsUnsure));

                    _UpdateTitle();
                }
            }
        }

        public TaxonomicCategory Category
        {
            get => _Category;

            set
            {
                if (_Category != value)
                {
                    _Category = value;

                    _UpdateTitle();
                }
            }
        }

        public string Synonyms
        {
            get => _Synonyms;

            set
            {
                if (_Synonyms != value)
                {
                    _Synonyms = value;

                    NotifyPropertyChanged(nameof(Synonyms));
                }
            }
        }

        public string Tags
        {
            get => _Tags;

            set
            {
                if (_Tags != value)
                {
                    _Tags = value;

                    NotifyPropertyChanged(nameof(Tags));
                }
            }
        }

        public string Description
        {
            get => _Description;

            set
            {
                if (_Description != value)
                {
                    _Description = value;

                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }

        public void UpdateFromTaxon(Taxon taxon)
        {
            _Taxon = taxon;

            TaxonColor = _Taxon.GetThemeColor();

            CategoryName = (_Taxon.IsRoot || _Taxon.IsAnonymous() ? string.Empty : _Taxon.Category.Name());
            TaxonName = (_Taxon.IsRoot ? string.Empty : _Taxon.ShortName('\n'));

            Name = _Taxon.BotanicalName;
            ChsName = _Taxon.ChineseName;

            IsExtinct = _Taxon.IsExtinct;
            IsUnsure = _Taxon.IsUnsure;

            Category = _Taxon.Category;

            Synonyms = string.Join(Environment.NewLine, _Taxon.Synonyms.ToArray());
            Tags = string.Join(Environment.NewLine, _Taxon.Tags.ToArray());
            Description = _Taxon.Description;
        }

        public void ApplyToTaxon()
        {
            if (!_Taxon.IsRoot)
            {
                _Taxon.BotanicalName = _Name;
                _Taxon.ChineseName = _ChsName;

                _Taxon.IsExtinct = _IsExtinct;
                _Taxon.IsUnsure = _IsUnsure;

                _Taxon.Category = _Category;

                _Taxon.Synonyms.Clear();
                _Taxon.Synonyms.AddRange(from s in _Synonyms.Split(Environment.NewLine) where !string.IsNullOrWhiteSpace(s.Trim()) select s.Trim());
                _Taxon.Tags.Clear();
                _Taxon.Tags.AddRange(from s in _Tags.Split(Environment.NewLine) where !string.IsNullOrWhiteSpace(s.Trim()) select s.Trim());
                _Taxon.Description = _Description;
            }
        }

        #endregion

        #region 主题

        private ColorX _TaxonColor;

        private bool _IsDarkTheme;

        private Brush _Button_ForeGround;
        private Brush _Button_BackGround;
        private Brush _CategoryName_ForeGround;
        private Brush _CategoryName_BackGround;
        private Brush _TaxonName_ForeGround;
        private Brush _TaxonName_BackGround;
        private Brush _SubTitle_ForeGround;
        private Brush _SubTitle_BackGround;
        private Brush _TextBox_ForeGround;
        private Brush _TextBox_BackGround;
        private Brush _TextBox_Selection;
        private Brush _TextBox_SelectionText;
        private Brush _CheckBox_ForeGround;

        private void _UpdateColors()
        {
            Button_ForeGround = Views.Common.Button_ForeGround;
            Button_BackGround = Views.Common.Button_BackGround;
            CategoryName_ForeGround = (_IsDarkTheme ? Brushes.Black : Brushes.White);
            CategoryName_BackGround = new SolidColorBrush(_TaxonColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToWpfColor());
            TaxonName_ForeGround = new SolidColorBrush(_TaxonColor.AtLightness_LAB(_IsDarkTheme ? 60 : 40).ToWpfColor());
            TaxonName_BackGround = new SolidColorBrush(_TaxonColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90).ToWpfColor());
            SubTitle_ForeGround = Views.Common.SubTitle_ForeGround;
            SubTitle_BackGround = Views.Common.SubTitle_BackGround;
            TextBox_ForeGround = Views.Common.TextBox_ForeGround;
            TextBox_BackGround = Views.Common.TextBox_BackGround;
            TextBox_Selection = Views.Common.TextBox_Selection;
            TextBox_SelectionText = Views.Common.TextBox_SelectionText;
            CheckBox_ForeGround = Views.Common.CheckBox_ForeGround;
        }

        public ColorX TaxonColor
        {
            get => _TaxonColor;

            set
            {
                if (_TaxonColor != value)
                {
                    _TaxonColor = value;

                    _UpdateColors();
                }
            }
        }

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateColors();
            }
        }

        public Brush Button_ForeGround
        {
            get => _Button_ForeGround;

            set
            {
                if (_Button_ForeGround != value)
                {
                    _Button_ForeGround = value;

                    NotifyPropertyChanged(nameof(Button_ForeGround));
                }
            }
        }

        public Brush Button_BackGround
        {
            get => _Button_BackGround;

            set
            {
                if (_Button_BackGround != value)
                {
                    _Button_BackGround = value;

                    NotifyPropertyChanged(nameof(Button_BackGround));
                }
            }
        }

        public Brush CategoryName_ForeGround
        {
            get => _CategoryName_ForeGround;

            set
            {
                if (_CategoryName_ForeGround != value)
                {
                    _CategoryName_ForeGround = value;

                    NotifyPropertyChanged(nameof(CategoryName_ForeGround));
                }
            }
        }

        public Brush CategoryName_BackGround
        {
            get => _CategoryName_BackGround;

            set
            {
                if (_CategoryName_BackGround != value)
                {
                    _CategoryName_BackGround = value;

                    NotifyPropertyChanged(nameof(CategoryName_BackGround));
                }
            }
        }

        public Brush TaxonName_ForeGround
        {
            get => _TaxonName_ForeGround;

            set
            {
                if (_TaxonName_ForeGround != value)
                {
                    _TaxonName_ForeGround = value;

                    NotifyPropertyChanged(nameof(TaxonName_ForeGround));
                }
            }
        }

        public Brush TaxonName_BackGround
        {
            get => _TaxonName_BackGround;

            set
            {
                if (_TaxonName_BackGround != value)
                {
                    _TaxonName_BackGround = value;

                    NotifyPropertyChanged(nameof(TaxonName_BackGround));
                }
            }
        }

        public Brush SubTitle_ForeGround
        {
            get => _SubTitle_ForeGround;

            set
            {
                if (_SubTitle_ForeGround != value)
                {
                    _SubTitle_ForeGround = value;

                    NotifyPropertyChanged(nameof(SubTitle_ForeGround));
                }
            }
        }

        public Brush SubTitle_BackGround
        {
            get => _SubTitle_BackGround;

            set
            {
                if (_SubTitle_BackGround != value)
                {
                    _SubTitle_BackGround = value;

                    NotifyPropertyChanged(nameof(SubTitle_BackGround));
                }
            }
        }

        public Brush TextBox_ForeGround
        {
            get => _TextBox_ForeGround;

            set
            {
                if (_TextBox_ForeGround != value)
                {
                    _TextBox_ForeGround = value;

                    NotifyPropertyChanged(nameof(TextBox_ForeGround));
                }
            }
        }

        public Brush TextBox_BackGround
        {
            get => _TextBox_BackGround;

            set
            {
                if (_TextBox_BackGround != value)
                {
                    _TextBox_BackGround = value;

                    NotifyPropertyChanged(nameof(TextBox_BackGround));
                }
            }
        }

        public Brush TextBox_Selection
        {
            get => _TextBox_Selection;

            set
            {
                if (_TextBox_Selection != value)
                {
                    _TextBox_Selection = value;

                    NotifyPropertyChanged(nameof(TextBox_Selection));
                }
            }
        }

        public Brush TextBox_SelectionText
        {
            get => _TextBox_SelectionText;

            set
            {
                if (_TextBox_SelectionText != value)
                {
                    _TextBox_SelectionText = value;

                    NotifyPropertyChanged(nameof(TextBox_SelectionText));
                }
            }
        }

        public Brush CheckBox_ForeGround
        {
            get => _CheckBox_ForeGround;

            set
            {
                if (_CheckBox_ForeGround != value)
                {
                    _CheckBox_ForeGround = value;

                    NotifyPropertyChanged(nameof(CheckBox_ForeGround));
                }
            }
        }

        #endregion
    }
}