/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

namespace TreeOfLife.Taxonomy.Extensions
{
    // 生物分类单元（类群）的扩展方法。
    public static class TaxonExtension
    {
        // 判断类群是否匿名。
        public static bool IsAnonymous(this Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            return (string.IsNullOrWhiteSpace(taxon.BotanicalName) && string.IsNullOrWhiteSpace(taxon.ChineseName));
        }

        // 判断类群是否具名。
        public static bool IsNamed(this Taxon taxon)
        {
            return (!taxon.IsAnonymous());
        }

        //

        // 获取类群的短名称。
        public static string ShortName(this Taxon taxon, char separator = ' ')
        {
            if (taxon == null)
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

                if (taxon.Unsure || taxon.IsExtinct)
                {
                    if (taxon.Unsure)
                    {
                        taxonName.Append('?');
                    }

                    if (taxon.IsExtinct)
                    {
                        taxonName.Append('†');
                    }

                    taxonName.Append(' ');
                }

                if (!string.IsNullOrWhiteSpace(taxon.ChineseName))
                {
                    taxonName.Append(taxon.ChineseName);

                    if (!string.IsNullOrWhiteSpace(taxon.BotanicalName))
                    {
                        taxonName.Append(separator);
                        taxonName.Append(taxon.BotanicalName);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(taxon.BotanicalName))
                {
                    taxonName.Append(taxon.BotanicalName);
                }

                return taxonName.ToString();
            }
        }

        // 获取类群的长名称。
        public static string LongName(this Taxon taxon, char separator = ' ')
        {
            if (taxon == null)
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

                if (taxon.Unsure || taxon.IsExtinct)
                {
                    if (taxon.Unsure)
                    {
                        taxonName.Append('?');
                    }

                    if (taxon.IsExtinct)
                    {
                        taxonName.Append('†');
                    }

                    taxonName.Append(' ');
                }

                if (!string.IsNullOrWhiteSpace(taxon.ChineseName))
                {
                    taxonName.Append(taxon.ChineseName);
                }
                else
                {
                    taxonName.Append(taxon.Category.Name());
                }

                if (!string.IsNullOrWhiteSpace(taxon.BotanicalName))
                {
                    taxonName.Append(separator);
                    taxonName.Append(taxon.BotanicalName);
                }

                return taxonName.ToString();
            }
        }

        //

        // 识别一个名称，并应用到当前类群。
        public static void ParseCurrent(this Taxon taxon, string name)
        {
            name = name.Trim();

            if (!string.IsNullOrWhiteSpace(name))
            {
                List<char> chars = new List<char>(name);

                if (chars.Contains('?'))
                {
                    taxon.Unsure = true;

                    chars.RemoveAll((ch) => ch == '?');
                }

                if (chars.Count > 0)
                {
                    if (chars.Contains('†'))
                    {
                        taxon.IsExtinct = true;

                        chars.RemoveAll((ch) => ch == '†');
                    }
                }

                if (chars.Count > 0)
                {
                    bool existLatin = false;
                    bool latinAtRight = false;

                    if (char.IsLower(chars[chars.Count - 1]) || char.IsUpper(chars[chars.Count - 1]))
                    {
                        existLatin = true;
                        latinAtRight = true;
                    }
                    else if (char.IsLower(chars[0]) || char.IsUpper(chars[0]))
                    {
                        existLatin = true;
                    }

                    string latinPart = null;
                    string chsPart = null;

                    if (existLatin)
                    {
                        if (latinAtRight)
                        {
                            int latinIndex = chars.Count - 1;

                            while (latinIndex >= 0 && (char.IsLower(chars[latinIndex]) || char.IsUpper(chars[latinIndex]) || char.IsWhiteSpace(chars[latinIndex])))
                            {
                                latinIndex--;
                            }

                            latinIndex++;

                            if (latinIndex > 0)
                            {
                                chsPart = new string(chars.ToArray(), 0, latinIndex);
                            }

                            while (latinIndex < chars.Count && char.IsWhiteSpace(chars[latinIndex]))
                            {
                                latinIndex++;
                            }

                            latinPart = new string(chars.ToArray(), latinIndex, chars.Count - latinIndex);
                        }
                        else
                        {
                            int chsIndex = 0;

                            while (chsIndex < chars.Count && (char.IsLower(chars[chsIndex]) || char.IsUpper(chars[chsIndex]) || char.IsWhiteSpace(chars[chsIndex])))
                            {
                                chsIndex++;
                            }

                            if (chsIndex < chars.Count)
                            {
                                chsPart = new string(chars.ToArray(), chsIndex, chars.Count - chsIndex);
                            }

                            while (chsIndex > 0 && char.IsWhiteSpace(chars[chsIndex]))
                            {
                                chsIndex--;
                            }

                            latinPart = new string(chars.ToArray(), 0, chsIndex);
                        }
                    }
                    else
                    {
                        chsPart = new string(chars.ToArray());
                    }

                    if (!string.IsNullOrWhiteSpace(latinPart))
                    {
                        taxon.BotanicalName = latinPart;
                    }

                    if (!string.IsNullOrWhiteSpace(chsPart))
                    {
                        TaxonomicCategory category;
                        string parsedCategoryName;

                        if (TaxonomicCategoryChineseName.TryParseCategory(chsPart, out category, out parsedCategoryName))
                        {
                            taxon.Category = category;

                            if (chsPart == parsedCategoryName)
                            {
                                taxon.ChineseName = string.Empty;
                            }
                            else
                            {
                                taxon.ChineseName = chsPart;
                            }
                        }
                        else
                        {
                            taxon.Category = TaxonomicCategory.Clade;
                            taxon.ChineseName = chsPart;
                        }
                    }
                    else
                    {
                        taxon.Category = TaxonomicCategory.Clade;
                    }
                }
            }
        }

        // 识别若干个名称，并添加子类群。
        public static void ParseChildren(this Taxon taxon, params string[] names)
        {
            if (names == null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var name in names)
            {
                Taxon child = taxon.AddChild();

                child.ParseCurrent(name);
            }
        }

        //

        // 获取继承的分类阶元。
        public static TaxonomicCategory GetInheritedCategory(this Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            if (taxon.Category.IsPrimaryCategory() || taxon.Category.IsSecondaryCategory())
            {
                return taxon.Category;
            }
            else
            {
                Taxon nearestPrimaryOrSecondaryCategoryParent = null;
                Taxon parent = taxon.Parent;

                while (parent != null)
                {
                    if (parent.Category.IsPrimaryCategory() || parent.Category.IsSecondaryCategory())
                    {
                        nearestPrimaryOrSecondaryCategoryParent = parent;

                        break;
                    }
                    else
                    {
                        parent = parent.Parent;
                    }
                }

                if (nearestPrimaryOrSecondaryCategoryParent == null)
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
            if (taxon == null)
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

                while (parent != null)
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

                if (nearestPrimaryCategoryParent == null)
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
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            Taxon parent = taxon.Parent;

            while (parent != null)
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
        public static List<Taxon> GetNamedChildren(this Taxon taxon, bool recursive = true)
        {
            if (taxon == null)
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