/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.700.1000.M7.201226-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife.Taxonomy.Extensions
{
    // 生物分类单元（类群）的父类群筛选条件。
    public class TaxonParentFilterCondition
    {
        private enum _FilterType
        {
            OnlyPrimaryCategory, // 仅主要分类阶元。
            Any // 任何。
        }

        private _FilterType _Filter; // 筛选类型。

        private bool _OnlyUplevel; // 仅更高的主要分类阶元。
        private bool _AllowEquals; // 允许相等的的主要分类阶元。

        private bool _AllowAnonymous; // 允许匿名。
        private bool _AllowUnranked; // 允许未分级。
        private bool _AllowClade; // 允许演化支。

        private TaxonParentFilterCondition(_FilterType filter, bool onlyUplevel, bool allowEquals, bool allowAnonymous, bool allowUnranked, bool allowClade)
        {
            _Filter = filter;

            _OnlyUplevel = onlyUplevel;
            _AllowEquals = allowEquals;

            _AllowAnonymous = allowAnonymous;
            _AllowUnranked = allowUnranked;
            _AllowClade = allowClade;
        }

        public bool IsMatched(Taxon taxon, Taxon parent)
        {
            if (taxon == null || parent == null)
            {
                throw new ArgumentNullException();
            }

            //

            switch (_Filter)
            {
                case _FilterType.OnlyPrimaryCategory:
                    if (parent.Category.IsPrimaryCategory())
                    {
                        if (taxon.Category.IsPrimaryCategory())
                        {
                            if (_OnlyUplevel)
                            {
                                if (parent.Category > taxon.Category) return true;
                                else if (parent.Category == taxon.Category) return _AllowEquals;
                                else return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }

                case _FilterType.Any:
                    if (parent.Category.IsPrimaryCategory() || parent.Category.IsSecondaryCategory())
                    {
                        return true;
                    }
                    else
                    {
                        if (parent.IsAnonymous() && !_AllowAnonymous)
                        {
                            return false;
                        }
                        else
                        {
                            if (parent.Category.IsUnranked()) return _AllowUnranked;
                            else if (parent.Category.IsClade()) return _AllowClade;
                            else return false;
                        }
                    }

                default: return false;
            }
        }

        public static TaxonParentFilterCondition OnlyPrimaryCategory(bool onlyUplevel = false, bool allowEquals = false)
        {
            return new TaxonParentFilterCondition(_FilterType.OnlyPrimaryCategory, onlyUplevel, allowEquals, false, false, false);
        }

        public static TaxonParentFilterCondition AnyTaxon(bool allowAnonymous = false, bool allowUnranked = true, bool allowClade = true)
        {
            return new TaxonParentFilterCondition(_FilterType.Any, false, false, allowAnonymous, allowUnranked, allowClade);
        }
    }

    // 生物分类单元（类群）的父类群筛选终止条件。
    public class TaxonParentFilterTerminationCondition
    {
        private enum _UntilType
        {
            UntilRoot, // 直到顶级类群。
            UntilUplevelPrimaryCategory, // 直到更高的主要分类阶元。
            UntilAnyPrimaryCategory // 直到任何主要分类阶元。
        }

        private _UntilType _Until; // 终止类型。
        private TaxonomicCategory _Category; // 高于指定的主要分类阶元。
        private bool _AllowEquals; // 允许相等的的主要分类阶元。

        private TaxonParentFilterTerminationCondition(_UntilType until, TaxonomicCategory category, bool allowEquals)
        {
            if (until == _UntilType.UntilUplevelPrimaryCategory && !category.IsPrimaryCategory())
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            _Until = until;
            _Category = category;
            _AllowEquals = allowEquals;
        }

        public bool IsMatched(Taxon parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException();
            }

            //

            switch (_Until)
            {
                case _UntilType.UntilRoot: return parent.IsRoot;

                case _UntilType.UntilUplevelPrimaryCategory:
                    if (parent.Category.IsPrimaryCategory())
                    {
                        if (parent.Category > _Category) return true;
                        else if (parent.Category == _Category) return _AllowEquals;
                        else return false;
                    }
                    else
                    {
                        return false;
                    }

                case _UntilType.UntilAnyPrimaryCategory: return parent.Category.IsPrimaryCategory();

                default: return false;
            }
        }

        public static TaxonParentFilterTerminationCondition UntilRoot()
        {
            return new TaxonParentFilterTerminationCondition(_UntilType.UntilRoot, (TaxonomicCategory)0, false);
        }

        public static TaxonParentFilterTerminationCondition UntilUplevelPrimaryCategory(TaxonomicCategory category, bool allowEquals = false)
        {
            return new TaxonParentFilterTerminationCondition(_UntilType.UntilUplevelPrimaryCategory, category, allowEquals);
        }

        public static TaxonParentFilterTerminationCondition UntilAnyPrimaryCategory()
        {
            return new TaxonParentFilterTerminationCondition(_UntilType.UntilAnyPrimaryCategory, (TaxonomicCategory)0, false);
        }
    }

    // 生物分类单元（类群）的父类群相关扩展方法。
    public static class TaxonParentExtension
    {
        // 获取最近的符合筛选条件的父类群。
        public static Taxon GetNearestParent(this Taxon taxon, TaxonParentFilterCondition filterCondition)
        {
            if (taxon == null || filterCondition == null)
            {
                throw new ArgumentNullException();
            }

            //

            Taxon parent = taxon.Parent;

            while (parent != null)
            {
                if (filterCondition.IsMatched(taxon, parent))
                {
                    return parent;
                }
                else
                {
                    parent = parent.Parent;
                }
            }

            return null;
        }

        // 获取所有符合筛选条件的父类群。
        public static List<Taxon> GetParents(this Taxon taxon, TaxonParentFilterCondition filterCondition, bool recursiveInsteadOfLoop = false, TaxonParentFilterTerminationCondition filterTerminationCondition = null)
        {
            if (taxon == null || filterCondition == null)
            {
                throw new ArgumentNullException();
            }

            //

            List<Taxon> parents = new List<Taxon>();

            if (!taxon.IsRoot)
            {
                bool terminationConditionExist = false;

                if (filterTerminationCondition == null)
                {
                    terminationConditionExist = true;
                }
                else
                {
                    Taxon parent = taxon.Parent;

                    while (parent != null)
                    {
                        if (filterTerminationCondition.IsMatched(parent))
                        {
                            terminationConditionExist = true;

                            break;
                        }

                        parent = parent.Parent;
                    }
                }

                if (terminationConditionExist)
                {
                    if (recursiveInsteadOfLoop)
                    {
                        Taxon current = taxon;
                        Taxon parent = current.GetNearestParent(filterCondition);

                        while (parent != null)
                        {
                            parents.Add(parent);

                            if (filterTerminationCondition != null && filterTerminationCondition.IsMatched(parent))
                            {
                                break;
                            }

                            current = parent;
                            parent = current.GetNearestParent(filterCondition);
                        }
                    }
                    else
                    {
                        Taxon current = taxon;
                        Taxon parent = current.Parent;

                        while (parent != null)
                        {
                            if (filterCondition.IsMatched(taxon, parent))
                            {
                                parents.Add(parent);
                            }

                            if (filterTerminationCondition != null && filterTerminationCondition.IsMatched(parent))
                            {
                                break;
                            }

                            current = parent;
                            parent = current.Parent;
                        }
                    }
                }
            }

            return parents;
        }
    }
}