/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

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

namespace TreeOfLife.UI.Views
{
    public sealed class ViewModel_Evo_EditMode : ViewModel
    {
        private string _Name;
        private string _ChName;

        private Rank _Rank;

        private bool _IsExtinct;
        private bool _IsUndet;

        private GeoChron _Birth;
        private GeoChron _Extinction;

        private Visibility _Visibility_Extinction;

        private string _Synonyms;
        private string _Tags;
        private string _Description;

        public string Name
        {
            get => _Name;

            set
            {
                _Name = value;

                //

                if (_LoadingFromTaxon)
                {
                    NotifyPropertyChanged(nameof(Name));
                }
                else
                {
                    Taxon currentTaxon = Common.CurrentTaxon;

                    if (!currentTaxon.IsRoot)
                    {
                        currentTaxon.ScientificName = _Name.Trim();

                        // 如果当前操作使类群变为匿名类群/不再为匿名类群，需要更新其分级为未指定/UI设置的分级
                        currentTaxon.Rank = currentTaxon.IsAnonymous ? Rank.Unranked : _Rank;

                        Common.NotifyEditOperation(Common.EditOperation.ScientificNameUpdated, currentTaxon);
                    }
                }
            }
        }

        public string ChName
        {
            get => _ChName;

            set
            {
                _ChName = value;

                //

                if (_LoadingFromTaxon)
                {
                    NotifyPropertyChanged(nameof(ChName));
                }
                else
                {
                    Taxon currentTaxon = Common.CurrentTaxon;

                    if (!currentTaxon.IsRoot)
                    {
                        currentTaxon.ChineseName = _ChName.Trim();

                        // 如果当前操作使类群变为匿名类群/不再为匿名类群，需要更新其分级为未指定/UI设置的分级
                        currentTaxon.Rank = currentTaxon.IsAnonymous ? Rank.Unranked : _Rank;

                        Common.NotifyEditOperation(Common.EditOperation.ChineseNameUpdated, currentTaxon);
                    }
                }
            }
        }

        public Rank Rank
        {
            get => _Rank;

            set
            {
                _Rank = value;

                //

                if (_LoadingFromTaxon)
                {
                    NotifyPropertyChanged(nameof(Rank));
                }
                else
                {
                    Taxon currentTaxon = Common.CurrentTaxon;

                    if (!currentTaxon.IsRoot)
                    {
                        // 只对具名类群应用分类阶元，匿名类群的分类阶元始终为未指定
                        currentTaxon.Rank = currentTaxon.IsAnonymous ? Rank.Unranked : _Rank;

                        Common.NotifyEditOperation(Common.EditOperation.RankUpdated, currentTaxon);
                    }
                }
            }
        }

        public bool IsExtinct
        {
            get => _IsExtinct;

            set
            {
                _IsExtinct = value;

                //

                if (_LoadingFromTaxon)
                {
                    NotifyPropertyChanged(nameof(IsExtinct));
                }
                else
                {
                    Taxon currentTaxon = Common.CurrentTaxon;

                    if (!currentTaxon.IsRoot)
                    {
                        currentTaxon.IsExtinct = _IsExtinct;

                        if (_IsExtinct)
                        {
                            Visibility_Extinction = Visibility.Visible;
                        }
                        else
                        {
                            Visibility_Extinction = Visibility.Collapsed;

                            _LoadingFromTaxon = true;
                            Extinction = GeoChron.Empty;
                            _LoadingFromTaxon = false;
                            Extinction = GeoChron.Empty;
                        }

                        Common.NotifyEditOperation(Common.EditOperation.IsExtinctUpdated, currentTaxon);
                    }
                }
            }
        }

        public bool IsUndet
        {
            get => _IsUndet;

            set
            {
                _IsUndet = value;

                //

                if (_LoadingFromTaxon)
                {
                    NotifyPropertyChanged(nameof(IsUndet));
                }
                else
                {
                    Taxon currentTaxon = Common.CurrentTaxon;

                    if (!currentTaxon.IsRoot)
                    {
                        currentTaxon.IsUndet = _IsUndet;

                        Common.NotifyEditOperation(Common.EditOperation.IsUndetUpdated, currentTaxon);
                    }
                }
            }
        }

        public GeoChron Birth
        {
            get => _Birth;

            set
            {
                _Birth = value;

                //

                if (_LoadingFromTaxon)
                {
                    NotifyPropertyChanged(nameof(Birth));
                }
                else
                {
                    Taxon currentTaxon = Common.CurrentTaxon;

                    if (!currentTaxon.IsRoot)
                    {
                        currentTaxon.Birth = _Birth;

                        Common.NotifyEditOperation(Common.EditOperation.BirthUpdated, currentTaxon);
                    }
                }
            }
        }

        public GeoChron Extinction
        {
            get => _Extinction;

            set
            {
                _Extinction = value;

                //

                if (_LoadingFromTaxon)
                {
                    NotifyPropertyChanged(nameof(Extinction));
                }
                else
                {
                    Taxon currentTaxon = Common.CurrentTaxon;

                    if (!currentTaxon.IsRoot)
                    {
                        currentTaxon.Extinction = _Extinction;

                        Common.NotifyEditOperation(Common.EditOperation.ExtinctionUpdated, currentTaxon);
                    }
                }
            }
        }

        public Visibility Visibility_Extinction
        {
            get => _Visibility_Extinction;

            set
            {
                _Visibility_Extinction = value;

                NotifyPropertyChanged(nameof(Visibility_Extinction));
            }
        }

        public string Synonyms
        {
            get => _Synonyms;

            set
            {
                _Synonyms = value;

                //

                if (_LoadingFromTaxon)
                {
                    NotifyPropertyChanged(nameof(Synonyms));
                }
                else
                {
                    Taxon currentTaxon = Common.CurrentTaxon;

                    if (!currentTaxon.IsRoot)
                    {
                        currentTaxon.Synonyms = _Synonyms.Split(Environment.NewLine);

                        Common.NotifyEditOperation(Common.EditOperation.SynonymsUpdated, currentTaxon);
                    }
                }
            }
        }

        public string Tags
        {
            get => _Tags;

            set
            {
                _Tags = value;

                //

                if (_LoadingFromTaxon)
                {
                    NotifyPropertyChanged(nameof(Tags));
                }
                else
                {
                    Taxon currentTaxon = Common.CurrentTaxon;

                    if (!currentTaxon.IsRoot)
                    {
                        currentTaxon.Tags = _Tags.Split(Environment.NewLine);

                        Common.NotifyEditOperation(Common.EditOperation.TagsUpdated, currentTaxon);
                    }
                }
            }
        }

        public string Description
        {
            get => _Description;

            set
            {
                _Description = value;

                //

                if (_LoadingFromTaxon)
                {
                    NotifyPropertyChanged(nameof(Description));
                }
                else
                {
                    Taxon currentTaxon = Common.CurrentTaxon;

                    if (!currentTaxon.IsRoot)
                    {
                        currentTaxon.Description = _Description;

                        Common.NotifyEditOperation(Common.EditOperation.DescriptionUpdated, currentTaxon);
                    }
                }
            }
        }

        private bool _LoadingFromTaxon = false;

        public void LoadFromCurrentTaxon()
        {
            _LoadingFromTaxon = true;

            //

            Taxon currentTaxon = Common.CurrentTaxon;

            Name = currentTaxon.ScientificName;
            ChName = currentTaxon.ChineseName;

            Rank = currentTaxon.Rank;

            IsExtinct = currentTaxon.IsExtinct;
            IsUndet = currentTaxon.IsUndet;

            Birth = currentTaxon.Birth;
            Extinction = currentTaxon.Extinction;

            Visibility_Extinction = IsExtinct ? Visibility.Visible : Visibility.Collapsed;

            Synonyms = string.Join(Environment.NewLine, currentTaxon.Synonyms);
            Tags = string.Join(Environment.NewLine, currentTaxon.Tags);
            Description = currentTaxon.Description;

            //

            _LoadingFromTaxon = false;
        }
    }
}