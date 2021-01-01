/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.800.1000.M8.201231-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife.Taxonomy.Extensions
{
    // 生物分类单元（类群）的搜索相关扩展方法。
    public static class TaxonSearchExtension
    {
        // 获取两个字符串的最大相同子串的长度。
        private static int _GetCommonPartLength(string str1, string str2)
        {
            // 原理：
            // 将两个字符串（之前各空一位再）分别横排和竖排，以二者的长度加一为行列数目构造一个矩阵，
            // 则矩阵的 (i, j) 元素表示 str1[i - 1] 与 str2[j - 1] 的比较，矩阵的 (i + 1, j + 1) 元素表示 str1[i] 与 str2[j] 的比较，
            // 类推可知，矩阵中所有与主对角线平行的对角元素表示将两个字符串相对滑动一段距离后重叠部分的逐字符比较，
            // 那么所有对角线上连续比中最多的数目即最大相同子串的长度，
            // 设法使矩阵的每个元素表示所在对角线的连续比中数目，
            // 令矩阵的第 0 行和第 0 列元素全部为 0，每当比中一个字符，令当前元素 (i, j) 等于 (i - 1, j - 1) + 1，否则等于 0,
            // 那么矩阵中所有元素的最大值即最大相同子串的长度。

            int len1 = str1.Length;
            int len2 = str2.Length;

            int result = 0;

            int[,] matrix = new int[len1 + 1, len2 + 1];

            for (int i = 0; i <= len1; i++)
            {
                matrix[i, 0] = 0;
            }

            for (int j = 0; j <= len2; j++)
            {
                matrix[0, j] = 0;
            }

            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    if (str1[i - 1] == str2[j - 1])
                    {
                        matrix[i, j] = matrix[i - 1, j - 1] + 1;

                        result = Math.Max(result, matrix[i, j]);
                    }
                    else
                    {
                        matrix[i, j] = 0;
                    }
                }
            }

            return result;
        }

        // 获取两个字符串的匹配率。
        private static double _GetMatchValueOfTwoString(string str1, string str2)
        {
            int commonPartLength = _GetCommonPartLength(str1.ToUpperInvariant(), str2.ToUpperInvariant());

            return ((double)commonPartLength * commonPartLength / str1.Length / str2.Length);
        }

        // 将类群的中文名分割为分类阶元与不含表示分类阶元的部分。
        private static (string chineseNameWithoutCategory, TaxonomicCategory? category) _SplitChineseName(string str)
        {
            TaxonomicCategory? categoryN = TaxonomicCategoryChineseExtension.ParseCategory(str);

            if (categoryN.HasValue)
            {
                return (string.Empty, categoryN);
            }
            else
            {
                TaxonomicCategory category;
                int categoryNameIndex;

                if (TaxonomicCategoryChineseExtension.TryParseCategory(str, out category, out categoryNameIndex))
                {
                    if (categoryNameIndex > 0)
                    {
                        return (str[0..categoryNameIndex], category);
                    }
                    else
                    {
                        return (string.Empty, category);
                    }
                }
                else
                {
                    return (str, null);
                }
            }
        }

        //

        // 搜索结果。
        private struct _SearchResult
        {
            // 类群。
            public Taxon Taxon { get; set; }

            // 匹配率。
            public double MatchValue { get; set; }

            // 比较器。
            public struct Comparer : IComparer<_SearchResult>
            {
                public int Compare(_SearchResult x, _SearchResult y) => x.MatchValue.CompareTo(y.MatchValue);
            }
        }

        private static List<_SearchResult> _SearchResults; // 搜索结果列表。

        private static string _KeyWord; // 关键字。
        private static string _KeyWordWithoutCategory; // 关键字除了表示分类阶元（如果有）之外的部分。
        private static TaxonomicCategory? _KeyWordCategory; // 关键字中包含的分类阶元。

        // 获取当前匹配条件下指定类群的匹配率。
        private static double _GetMatchValueOfTaxon(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            double matchValue = 0;

            // 关键字"是"或者"包含"分类阶元名称的
            if (_KeyWordCategory.HasValue)
            {
                // 关键字"是"分类阶元名称的
                if (string.IsNullOrEmpty(_KeyWordWithoutCategory))
                {
                    // 类群具有中文名的，将中文名的长度作为分母
                    if (!string.IsNullOrEmpty(taxon.ChineseName))
                    {
                        if (taxon.Category == _KeyWordCategory.Value)
                        {
                            matchValue = (double)_KeyWord.Length / taxon.ChineseName.Length;
                        }
                        // 仅基本分类阶元相同的，匹配率降低1/2
                        else if (taxon.Category.BasicCategory() == _KeyWordCategory.Value.BasicCategory())
                        {
                            matchValue = 0.5 * _KeyWord.Length / taxon.ChineseName.Length;
                        }
                    }
                    // 类群没有中文名的，将关键字的长度与学名的长度之和作为分母
                    else
                    {
                        if (taxon.Category == _KeyWordCategory.Value)
                        {
                            matchValue = (double)_KeyWord.Length / (_KeyWord.Length + taxon.BotanicalName.Length);
                        }
                        // 仅基本分类阶元相同的，匹配率降低1/2
                        else if (taxon.Category.BasicCategory() == _KeyWordCategory.Value.BasicCategory())
                        {
                            matchValue = 0.5 * _KeyWord.Length / (_KeyWord.Length + taxon.BotanicalName.Length);
                        }
                    }

                    // 未匹配到分类阶元，做全字符串匹配，并且匹配率降低1/2
                    if (matchValue <= 0)
                    {
                        if (!string.IsNullOrEmpty(taxon.ChineseName))
                        {
                            matchValue = _GetMatchValueOfTwoString(_KeyWord, taxon.ChineseName);
                        }

                        if (matchValue < 1 && !string.IsNullOrEmpty(taxon.BotanicalName))
                        {
                            matchValue = Math.Max(matchValue, _GetMatchValueOfTwoString(_KeyWord, taxon.BotanicalName));
                        }

                        if (matchValue < 1 && taxon.Tags.Count > 0)
                        {
                            foreach (var tag in taxon.Tags)
                            {
                                if (!string.IsNullOrEmpty(tag))
                                {
                                    matchValue = Math.Max(matchValue, _GetMatchValueOfTwoString(_KeyWord, tag));

                                    if (matchValue >= 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (matchValue < 1 && taxon.Synonyms.Count > 0)
                        {
                            foreach (var synonym in taxon.Synonyms)
                            {
                                if (!string.IsNullOrEmpty(synonym))
                                {
                                    matchValue = Math.Max(matchValue, _GetMatchValueOfTwoString(_KeyWord, synonym));

                                    if (matchValue >= 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        matchValue *= 0.5;
                    }
                }
                // 关键字"包含"分类阶元名称，还包含除此之外的
                else
                {
                    double c = 0;

                    // 分类阶元相同的，匹配率基数为1
                    if (taxon.Category == _KeyWordCategory.Value)
                    {
                        c = 1;
                    }
                    // 仅基本分类阶元相同的，匹配率基数降低1/2
                    else if (taxon.Category.BasicCategory() == _KeyWordCategory.Value.BasicCategory())
                    {
                        c = 0.5;
                    }

                    // 匹配到分类阶元，做名称部分匹配
                    if (c > 0)
                    {
                        double mv = 0;

                        if (!string.IsNullOrEmpty(taxon.ChineseName))
                        {
                            var split = _SplitChineseName(taxon.ChineseName);

                            string chsNameWithoutCategory = (string.IsNullOrEmpty(split.chineseNameWithoutCategory) ? taxon.ChineseName : split.chineseNameWithoutCategory);

                            if (!string.IsNullOrEmpty(chsNameWithoutCategory))
                            {
                                mv = _GetMatchValueOfTwoString(_KeyWordWithoutCategory, chsNameWithoutCategory);
                            }
                        }

                        if (mv < 1 && !string.IsNullOrEmpty(taxon.BotanicalName))
                        {
                            mv = Math.Max(mv, _GetMatchValueOfTwoString(_KeyWordWithoutCategory, taxon.BotanicalName));
                        }

                        matchValue = c * mv;
                    }

                    // 未匹配到分类阶元，或者名称部分匹配失败，做全字符串匹配，并且匹配率降低1/2
                    if (matchValue <= 0)
                    {
                        if (!string.IsNullOrEmpty(taxon.ChineseName))
                        {
                            matchValue = _GetMatchValueOfTwoString(_KeyWord, taxon.ChineseName);
                        }

                        if (matchValue < 1 && !string.IsNullOrEmpty(taxon.BotanicalName))
                        {
                            matchValue = Math.Max(matchValue, _GetMatchValueOfTwoString(_KeyWord, taxon.BotanicalName));
                        }

                        if (matchValue < 1 && taxon.Tags.Count > 0)
                        {
                            foreach (var tag in taxon.Tags)
                            {
                                if (!string.IsNullOrEmpty(tag))
                                {
                                    matchValue = Math.Max(matchValue, _GetMatchValueOfTwoString(_KeyWord, tag));

                                    if (matchValue >= 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (matchValue < 1 && taxon.Synonyms.Count > 0)
                        {
                            foreach (var synonym in taxon.Synonyms)
                            {
                                if (!string.IsNullOrEmpty(synonym))
                                {
                                    matchValue = Math.Max(matchValue, _GetMatchValueOfTwoString(_KeyWord, synonym));

                                    if (matchValue >= 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        matchValue *= 0.5;
                    }
                }
            }
            // 关键字"不含"分类阶元名称的，做全字符串匹配
            else
            {
                if (!string.IsNullOrEmpty(taxon.ChineseName))
                {
                    matchValue = _GetMatchValueOfTwoString(_KeyWord, taxon.ChineseName);
                }

                if (matchValue < 1 && !string.IsNullOrEmpty(taxon.BotanicalName))
                {
                    matchValue = Math.Max(matchValue, _GetMatchValueOfTwoString(_KeyWord, taxon.BotanicalName));
                }

                if (matchValue < 1 && taxon.Tags.Count > 0)
                {
                    foreach (var tag in taxon.Tags)
                    {
                        if (!string.IsNullOrEmpty(tag))
                        {
                            matchValue = Math.Max(matchValue, _GetMatchValueOfTwoString(_KeyWord, tag));

                            if (matchValue >= 1)
                            {
                                break;
                            }
                        }
                    }
                }

                if (matchValue < 1 && taxon.Synonyms.Count > 0)
                {
                    foreach (var synonym in taxon.Synonyms)
                    {
                        if (!string.IsNullOrEmpty(synonym))
                        {
                            matchValue = Math.Max(matchValue, _GetMatchValueOfTwoString(_KeyWord, synonym));

                            if (matchValue >= 1)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return matchValue;
        }

        // 递归获取所有符合匹配条件的子类群。
        private static void _GetMatchedChildren(this Taxon taxon)
        {
            foreach (var child in taxon.Children)
            {
                if (child.IsNamed())
                {
                    double matchValue = _GetMatchValueOfTaxon(child);

                    // 丢弃匹配率过低的结果
                    if (matchValue > 0.1)
                    {
                        _SearchResults.Add(new _SearchResult() { Taxon = child, MatchValue = matchValue });
                    }
                }

                child._GetMatchedChildren();
            }
        }

        // 搜索符合指定的关键词的子类群。
        public static IReadOnlyList<Taxon> Search(this Taxon taxon, string keyWord)
        {
            if (taxon == null || string.IsNullOrWhiteSpace(keyWord))
            {
                throw new ArgumentNullException();
            }

            //

            _SearchResults = new List<_SearchResult>();

            _KeyWord = keyWord;
            (_KeyWordWithoutCategory, _KeyWordCategory) = _SplitChineseName(_KeyWord);

            taxon._GetMatchedChildren();

            _SearchResults.Sort(new _SearchResult.Comparer());
            _SearchResults.Reverse();

            List<Taxon> result = new List<Taxon>(_SearchResults.Count);

            foreach (var sr in _SearchResults)
            {
                result.Add(sr.Taxon);
            }

            _SearchResults.Clear();

            return result;
        }
    }
}