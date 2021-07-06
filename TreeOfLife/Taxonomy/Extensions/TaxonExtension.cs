/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1134.1000.M11.210518-2200

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife.Taxonomy.Extensions
{
    // 生物分类单元（类群）的扩展方法。
    public static class TaxonExtension
    {
        // 判断类群是否匿名。
        public static bool IsAnonymous(this Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            return (string.IsNullOrEmpty(taxon.ScientificName) && string.IsNullOrEmpty(taxon.ChineseName));
        }

        // 判断类群是否具名。
        public static bool IsNamed(this Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            return (!string.IsNullOrEmpty(taxon.ScientificName) || !string.IsNullOrEmpty(taxon.ChineseName));
        }

        //

        // 获取类群的短名称。
        public static string GetShortName(this Taxon taxon, char separator = ' ')
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (taxon.IsAnonymous())
            {
                return "(未命名)";
            }
            else
            {
                StringBuilder taxonName = new StringBuilder();

                if (taxon.IsUnsure || taxon.IsExtinct)
                {
                    if (taxon.IsUnsure)
                    {
                        taxonName.Append('?');
                    }

                    if (taxon.IsExtinct)
                    {
                        taxonName.Append('†');
                    }

                    taxonName.Append(' ');
                }

                if (!string.IsNullOrEmpty(taxon.ChineseName))
                {
                    taxonName.Append(taxon.ChineseName);

                    if (!string.IsNullOrEmpty(taxon.ScientificName))
                    {
                        taxonName.Append(separator);
                        taxonName.Append(taxon.ScientificName);
                    }
                }
                else if (!string.IsNullOrEmpty(taxon.ScientificName))
                {
                    taxonName.Append(taxon.ScientificName);
                }

                if (taxon.IsPolyphyly)
                {
                    taxonName.Append(" #");
                }
                else if (taxon.IsParaphyly)
                {
                    taxonName.Append(" *");
                }

                return taxonName.ToString();
            }
        }

        // 获取类群的长名称。
        public static string GetLongName(this Taxon taxon, char separator = ' ')
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (taxon.IsAnonymous())
            {
                return "(未命名)";
            }
            else
            {
                StringBuilder taxonName = new StringBuilder();

                if (taxon.IsUnsure || taxon.IsExtinct)
                {
                    if (taxon.IsUnsure)
                    {
                        taxonName.Append('?');
                    }

                    if (taxon.IsExtinct)
                    {
                        taxonName.Append('†');
                    }

                    taxonName.Append(' ');
                }

                if (!string.IsNullOrEmpty(taxon.ChineseName))
                {
                    taxonName.Append(taxon.ChineseName);
                }
                else
                {
                    TaxonomicCategory category = taxon.Category;

                    if (category.IsUnranked() || category.IsClade())
                    {
                        if (taxon.IsParaphyly)
                        {
                            taxonName.Append("并系群");
                        }
                        else if (taxon.IsPolyphyly)
                        {
                            taxonName.Append("复系群");
                        }
                        else
                        {
                            if (category.IsClade())
                            {
                                taxonName.Append(category.GetChineseName());
                            }
                        }
                    }
                    else
                    {
                        taxonName.Append(category.GetChineseName());
                    }
                }

                if (!string.IsNullOrEmpty(taxon.ScientificName))
                {
                    taxonName.Append(separator);
                    taxonName.Append(taxon.ScientificName);
                }

                if (taxon.IsPolyphyly)
                {
                    taxonName.Append(" #");
                }
                else if (taxon.IsParaphyly)
                {
                    taxonName.Append(" *");
                }

                return taxonName.ToString();
            }
        }

        //

        // 获取继承的分类阶元。
        public static TaxonomicCategory GetInheritedCategory(this Taxon taxon)
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
        public static TaxonomicCategory GetInheritedBasicCategory(this Taxon taxon)
        {
            return taxon.GetInheritedCategory().BasicCategory();
        }

        // 获取继承的主要分类阶元。
        public static TaxonomicCategory GetInheritedPrimaryCategory(this Taxon taxon)
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
        public static TaxonomicCategory GetInheritedBasicPrimaryCategory(this Taxon taxon)
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
                if (parent.IsNamed())
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
                if (child.IsNamed())
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
    }
}