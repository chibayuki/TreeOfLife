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

namespace TreeOfLife.Core.Taxonomy.Extensions
{
    // 生物分类单元（类群）的筛选条件。
    [Flags]
    public enum TaxonFilter
    {
        None = 0, // 不匹配任何类群

        Named = 0x00000001, // 匹配具名类群
        Anonymous = 0x00000002, // 匹配匿名类群
        NamedOrAnonymous = Named | Anonymous, // 匹配具名或匿名类群

        Unranked = 0x00000004, // 匹配未分级类群
        Clade = 0x00000008, // 匹配演化支类群
        BasicSecondaryCategory = 0x00000010, // 匹配基本次要级别类群
        BasicPrimaryCategory = 0x00000020, // 匹配基本主要级别类群
        BasicCategory = BasicSecondaryCategory | BasicPrimaryCategory, // 匹配基本级别类群
        SecondaryCategory = 0x00000040, // 匹配次要级别类群
        PrimaryCategory = 0x00000080, // 匹配主要级别类群
        PrimaryOrSecondaryCategory = SecondaryCategory | PrimaryCategory, // 匹配主要或次要级别类群
        AnyCategory = Unranked | Clade | SecondaryCategory | PrimaryCategory, // 匹配任意分级类群

        Any = NamedOrAnonymous | AnyCategory // 匹配任意类群
    }

    // 生物分类单元（类群）的继承相关扩展方法。
    public static class TaxonInheritExtension
    {
        // 获取继承的分类阶元。
        public static Category GetInheritedCategory(this Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (taxon.Category.IsPrimaryOrSecondaryCategory())
            {
                return taxon.Category;
            }
            else
            {
                Taxon nearestPrimaryOrSecondaryCategoryParent = null;
                Taxon parent = taxon.Parent;

                while (parent is not null)
                {
                    if (parent.Category.IsPrimaryOrSecondaryCategory())
                    {
                        nearestPrimaryOrSecondaryCategoryParent = parent;

                        break;
                    }
                    else
                    {
                        parent = parent.Parent;
                    }
                }

                if (nearestPrimaryOrSecondaryCategoryParent is null)
                {
                    return taxon.Root.Category;
                }
                else
                {
                    return nearestPrimaryOrSecondaryCategoryParent.Category;
                }
            }
        }

        // 获取继承的基本分类阶元。
        public static Category GetInheritedBasicCategory(this Taxon taxon)
        {
            return taxon.GetInheritedCategory().BasicCategory();
        }

        // 获取继承的主要分类阶元。
        public static Category GetInheritedPrimaryCategory(this Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (taxon.Category.IsPrimaryCategory())
            {
                return taxon.Category;
            }
            else
            {
                Taxon nearestPrimaryCategoryParent = null;
                Taxon parent = taxon.Parent;

                while (parent is not null)
                {
                    if (parent.Category.IsPrimaryCategory())
                    {
                        nearestPrimaryCategoryParent = parent;

                        break;
                    }
                    else
                    {
                        parent = parent.Parent;
                    }
                }

                if (nearestPrimaryCategoryParent is null)
                {
                    return taxon.Root.Category;
                }
                else
                {
                    return nearestPrimaryCategoryParent.Category;
                }
            }
        }

        // 获取继承的基本主要分类阶元。
        public static Category GetInheritedBasicPrimaryCategory(this Taxon taxon)
        {
            return taxon.GetInheritedPrimaryCategory().BasicCategory();
        }

        //

        // 获取具名父类群。
        public static Taxon GetNamedParent(this Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            Taxon parent = taxon.Parent;

            while (parent is not null)
            {
                if (parent.IsNamed)
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

        // 获取具名子类群（并递归获取匿名子类群的所有的具名子类群）。
        public static IReadOnlyList<Taxon> GetNamedChildren(this Taxon taxon, bool recursive = true)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            List<Taxon> children = new List<Taxon>();

            foreach (var child in taxon.Children)
            {
                if (child.IsNamed)
                {
                    children.Add(child);
                }
                else
                {
                    if (recursive)
                    {
                        children.AddRange(child.GetNamedChildren(true));
                    }
                }
            }

            return children;
        }

        //

        private static bool _IsMatched(this Taxon taxon, TaxonFilter filter)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (filter == TaxonFilter.None)
            {
                return false;
            }
            else if (filter == TaxonFilter.Any)
            {
                return true;
            }
            else
            {
                bool nameIsMatched = false;
                bool categoryIsMatched = false;

                if (filter.HasFlag(TaxonFilter.NamedOrAnonymous))
                {
                    nameIsMatched = true;
                }
                else if (taxon.IsNamed && filter.HasFlag(TaxonFilter.Named))
                {
                    nameIsMatched = true;
                }
                else if (taxon.IsAnonymous && filter.HasFlag(TaxonFilter.Anonymous))
                {
                    nameIsMatched = true;
                }

                if (nameIsMatched)
                {
                    if (filter.HasFlag(TaxonFilter.AnyCategory))
                    {
                        categoryIsMatched = true;
                    }
                    else if (taxon.Category.IsUnranked() && filter.HasFlag(TaxonFilter.Unranked))
                    {
                        categoryIsMatched = true;
                    }
                    else if (taxon.Category.IsClade() && filter.HasFlag(TaxonFilter.Clade))
                    {
                        categoryIsMatched = true;
                    }
                    else if (taxon.Category.IsBasicSecondaryCategory() && filter.HasFlag(TaxonFilter.BasicSecondaryCategory))
                    {
                        categoryIsMatched = true;
                    }
                    else if (taxon.Category.IsSecondaryCategory() && filter.HasFlag(TaxonFilter.SecondaryCategory))
                    {
                        categoryIsMatched = true;
                    }
                    else if (taxon.Category.IsBasicPrimaryCategory() && filter.HasFlag(TaxonFilter.BasicPrimaryCategory))
                    {
                        categoryIsMatched = true;
                    }
                    else if (taxon.Category.IsPrimaryCategory() && filter.HasFlag(TaxonFilter.PrimaryCategory))
                    {
                        categoryIsMatched = true;
                    }
                }

                return nameIsMatched && categoryIsMatched;
            }
        }

        // 获取最近的符合筛选条件的父类群。
        public static Taxon GetNearestParent(this Taxon taxon, TaxonFilter filter)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            Taxon parent = taxon.Parent;

            while (parent is not null)
            {
                if (taxon._IsMatched(filter))
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

        private static Taxon _GetParentSkipParaphyly(this Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (taxon.ExcludeBy.Count > 0)
            {
                Taxon parent = null;

                foreach (var excludeBy in taxon.ExcludeBy)
                {
                    if (parent is null || parent.Level > excludeBy.Parent.Level)
                    {
                        parent = excludeBy.Parent;
                    }
                }

                return parent;
            }
            else
            {
                return taxon.Parent;
            }
        }

        // 获取所有符合筛选条件的父类群。
        public static List<Taxon> GetParents(this Taxon taxon, TaxonFilter filter, TaxonFilter terminationCondition = TaxonFilter.None, bool includeTermination = true, bool skipParaphyly = true)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            List<Taxon> parents = new List<Taxon>();

            if (!taxon.IsRoot)
            {
                Taxon parent = skipParaphyly ? taxon._GetParentSkipParaphyly() : taxon.Parent;

                while (parent is not null)
                {
                    if (parent._IsMatched(terminationCondition))
                    {
                        if (includeTermination)
                        {
                            parents.Add(parent);
                        }

                        break;
                    }

                    if (parent._IsMatched(filter))
                    {
                        parents.Add(parent);
                    }

                    parent = skipParaphyly ? parent._GetParentSkipParaphyly() : parent.Parent;
                }
            }

            return parents;
        }
    }
}