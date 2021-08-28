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

using TreeOfLife.Core.Geology;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;
using TreeOfLife.UI.Controls;

namespace TreeOfLife.UI.Views.Evo.EditMode
{
    public sealed class ViewModel_Evo_EditMode : ViewModel
    {
        public TaxonNameTitle TaxonNameTitle { get; set; }

        //

        private string _Name;
        private string _ChsName;

        private TaxonomicCategory _Category;

        private bool _IsExtinct;
        private bool _IsUnsure;

        private GeoChron _Birth;
        private GeoChron _Extinction;

        private Visibility _Grid_Extinction;

        private string _Synonyms;
        private string _Tags;
        private string _Description;

        public string Name
        {
            get => _Name;

            set
            {
                _Name = value;

                NotifyPropertyChanged(nameof(Name));

                //

                UpdateTitle();
            }
        }

        public string ChsName
        {
            get => _ChsName;

            set
            {
                _ChsName = value;

                NotifyPropertyChanged(nameof(ChsName));

                //

                UpdateTitle();
            }
        }

        public bool IsExtinct
        {
            get => _IsExtinct;

            set
            {
                _IsExtinct = value;

                NotifyPropertyChanged(nameof(IsExtinct));

                //

                UpdateTitle();

                //

                if (_IsExtinct)
                {
                    Grid_Extinction = Visibility.Visible;
                }
                else
                {
                    Grid_Extinction = Visibility.Collapsed;

                    Extinction = GeoChron.Empty;
                }
            }
        }

        public bool IsUnsure
        {
            get => _IsUnsure;

            set
            {
                _IsUnsure = value;

                NotifyPropertyChanged(nameof(IsUnsure));

                //

                UpdateTitle();
            }
        }

        public TaxonomicCategory Category
        {
            get => _Category;

            set
            {
                _Category = value;

                UpdateTitle();
            }
        }

        public GeoChron Birth
        {
            get => _Birth;
            set => _Birth = value;
        }

        public GeoChron Extinction
        {
            get => _Extinction;
            set => _Extinction = value;
        }

        public Visibility Grid_Extinction
        {
            get => _Grid_Extinction;

            set
            {
                _Grid_Extinction = value;

                NotifyPropertyChanged(nameof(Grid_Extinction));
            }
        }

        public string Synonyms
        {
            get => _Synonyms;

            set
            {
                _Synonyms = value;

                NotifyPropertyChanged(nameof(Synonyms));
            }
        }

        public string Tags
        {
            get => _Tags;

            set
            {
                _Tags = value;

                NotifyPropertyChanged(nameof(Tags));
            }
        }

        public string Description
        {
            get => _Description;

            set
            {
                _Description = value;

                NotifyPropertyChanged(nameof(Description));
            }
        }

        public void UpdateTitle()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            if (currentTaxon is not null)
            {
                TaxonomicCategory category = _Category;

                TaxonNameTitle.ThemeColor = (currentTaxon.IsRoot || category.IsPrimaryOrSecondaryCategory() ? category.GetThemeColor() : currentTaxon.Parent.GetThemeColor());

                string name = _Name?.Trim();
                string chsName = _ChsName?.Trim();

                if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(chsName))
                {
                    TaxonNameTitle.Category = null;
                    TaxonNameTitle.TaxonName = "(未命名)";
                }
                else
                {
                    TaxonNameTitle.Category = category;

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

                    if (!string.IsNullOrEmpty(chsName))
                    {
                        taxonName.Append(chsName);

                        if (!string.IsNullOrEmpty(name))
                        {
                            taxonName.Append('\n');
                            taxonName.Append(name);
                        }
                    }
                    else if (!string.IsNullOrEmpty(name))
                    {
                        taxonName.Append(name);
                    }

                    if (currentTaxon.IsPolyphyly)
                    {
                        taxonName.Append(" #");
                    }
                    else if (currentTaxon.IsParaphyly)
                    {
                        taxonName.Append(" *");
                    }

                    TaxonNameTitle.TaxonName = taxonName.ToString();
                }

                TaxonNameTitle.IsParaphyly = currentTaxon.IsParaphyly;
                TaxonNameTitle.IsPolyphyly = currentTaxon.IsPolyphyly;
            }
        }

        public void LoadFromTaxon()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            Name = currentTaxon.ScientificName;
            ChsName = currentTaxon.ChineseName;

            Category = currentTaxon.Category;

            IsExtinct = currentTaxon.IsExtinct;
            IsUnsure = currentTaxon.IsUnsure;

            Birth = currentTaxon.Birth;
            Extinction = currentTaxon.Extinction;

            Synonyms = string.Join(Environment.NewLine, currentTaxon.Synonyms.ToArray());
            Tags = string.Join(Environment.NewLine, currentTaxon.Tags.ToArray());
            Description = currentTaxon.Description;
        }

        public void ApplyToTaxon()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            if (!currentTaxon.IsRoot)
            {
                currentTaxon.ScientificName = _Name.Trim();
                currentTaxon.ChineseName = _ChsName.Trim();

                currentTaxon.IsExtinct = _IsExtinct;
                currentTaxon.IsUnsure = _IsUnsure;

                currentTaxon.Birth = _Birth;
                currentTaxon.Extinction = _Extinction;

                // 只对具名类群应用分类阶元
                if (currentTaxon.ScientificName.Length > 0 || currentTaxon.ChineseName.Length > 0)
                {
                    currentTaxon.Category = _Category;
                }
                // 匿名类群的分类阶元始终为未分级
                else
                {
                    currentTaxon.Category = TaxonomicCategory.Unranked;
                }

                currentTaxon.Synonyms.Clear();
                currentTaxon.Synonyms.AddRange(
                    from s in _Synonyms.Split(Environment.NewLine)
                    let synonym = s?.Trim()
                    where !string.IsNullOrEmpty(synonym)
                    select synonym);

                currentTaxon.Tags.Clear();
                currentTaxon.Tags.AddRange(
                    from s in _Tags.Split(Environment.NewLine)
                    let tag = s?.Trim()
                    where !string.IsNullOrEmpty(tag)
                    select tag);

                currentTaxon.Description = _Description;
            }
        }
    }
}