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
            if (!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2))
            {
                // 出于性能和实际情况考虑，最多只比较前32个字符，并且先转换为大写再比较
                string s1 = (str1.Length > 32 ? str1[0..32] : str1).ToUpperInvariant();
                string s2 = (str2.Length > 32 ? str2[0..32] : str2).ToUpperInvariant();

                int commonPartLength = _GetCommonPartLength(s1, s2);

                return ((double)commonPartLength * commonPartLength / str1.Length / str2.Length);
            }
            else
            {
                return 0;
            }
        }

        //

        // 分类阶元的相关性。
        private enum _CategoryRelativity
        {
            Equals,
            Relevant,
            Irrelevant
        }

        // 匹配的对象。
        private enum _MatchObject
        {
            PureChineseName,
            ChineseName,
            BotanicalName,
            Synonyms,
            Tags
        }

        // 匹配结果。
        private struct _MatchResult
        {
            public Taxon Taxon { get; set; }

            // 匹配率。
            public double MatchValue { get; set; }

            public _CategoryRelativity CategoryRelativity { get; set; }

            public _MatchObject MatchObject { get; set; }

            //

#if DEBUG

            public override string ToString()
            {
                return string.Concat("{Taxon=", Taxon, ", MatchValue=", MatchValue, ", CategoryRelativity=", CategoryRelativity, ", MatchObject=", MatchObject, "}");
            }

#endif
        }

        private static List<_MatchResult> _MatchResults; // 匹配结果列表。

        private static string _KeyWord; // 关键字。
        private static string _KeyWordWithoutCategory; // 关键字除了表示分类阶元（如果有）之外的部分。
        private static TaxonomicCategory? _KeyWordCategory; // 关键字中包含的分类阶元。

        // 获取对指定类群做全字符串匹配的最佳匹配率和匹配对象。
        private static (double matchValue, _MatchObject matchObject) _GetMatchValueAndObject(Taxon taxon, string str, bool matchChineseName = true, bool matchBotanicalName = true, bool matchSynonyms = true, bool matchTags = true)
        {
            double matchValue = 0;
            _MatchObject matchObject = _MatchObject.ChineseName;

            if (matchChineseName && !string.IsNullOrEmpty(taxon.ChineseName))
            {
                matchValue = _GetMatchValueOfTwoString(str, taxon.ChineseName);
            }

            if (matchBotanicalName && matchValue < 1 && !string.IsNullOrEmpty(taxon.BotanicalName))
            {
                double mv = _GetMatchValueOfTwoString(str, taxon.BotanicalName);

                if (matchValue < mv)
                {
                    matchValue = mv;
                    matchObject = _MatchObject.BotanicalName;
                }
            }

            if (matchSynonyms && matchValue < 1 && taxon.Synonyms.Count > 0)
            {
                foreach (var synonym in taxon.Synonyms)
                {
                    if (!string.IsNullOrEmpty(synonym))
                    {
                        double mv = _GetMatchValueOfTwoString(str, synonym);

                        if (matchValue < mv)
                        {
                            matchValue = mv;
                            matchObject = _MatchObject.Synonyms;
                        }

                        if (mv >= 1)
                        {
                            break;
                        }
                    }
                }
            }

            if (matchTags && matchValue < 1 && taxon.Tags.Count > 0)
            {
                foreach (var tag in taxon.Tags)
                {
                    if (!string.IsNullOrEmpty(tag))
                    {
                        double mv = _GetMatchValueOfTwoString(str, tag);

                        if (matchValue < mv)
                        {
                            matchValue = mv;
                            matchObject = _MatchObject.Tags;
                        }

                        if (mv >= 1)
                        {
                            break;
                        }
                    }
                }
            }

            return (matchValue, matchObject);
        }

        private static _CategoryRelativity _GetCategoryRelativity(TaxonomicCategory category1, TaxonomicCategory category2)
        {
            // 分类阶元相同
            if (category1 == category2 || ((category1, category2) is (TaxonomicCategory.Clade, TaxonomicCategory.Division) or (TaxonomicCategory.Division, TaxonomicCategory.Clade)))
            {
                return _CategoryRelativity.Equals;
            }
            else
            {
                TaxonomicCategory basicCategory1 = category1.BasicCategory();
                TaxonomicCategory basicCategory2 = category2.BasicCategory();

                // 仅基本分类阶元相同
                if (basicCategory1 == basicCategory2 || ((basicCategory1, basicCategory2) is (TaxonomicCategory.Clade, TaxonomicCategory.Division) or (TaxonomicCategory.Division, TaxonomicCategory.Clade)))
                {
                    return _CategoryRelativity.Relevant;
                }
                // 分类阶元不相关
                else
                {
                    return _CategoryRelativity.Irrelevant;
                }
            }
        }

        // 获取指定类群在当前匹配条件下模糊匹配的匹配结果。
        private static _MatchResult _GetMatchResultOfTaxon(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            _MatchResult result = new _MatchResult() { Taxon = taxon };

            // 关键字包含分类阶元名称的
            if (_KeyWordCategory != null)
            {
                // 关键字包含分类阶元名称，还包含除此之外的
                if (!string.IsNullOrEmpty(_KeyWordWithoutCategory))
                {
                    result.CategoryRelativity = _GetCategoryRelativity(taxon.Category, _KeyWordCategory.Value);

                    // 分类阶元相同或相关的，做部分关键字与部分中文名的匹配
                    if (result.CategoryRelativity is _CategoryRelativity.Equals or _CategoryRelativity.Relevant)
                    {
                        if (!string.IsNullOrEmpty(taxon.ChineseName))
                        {
                            (string chsNameWithoutCategory, _, _) = TaxonomicCategoryChineseExtension.SplitChineseName(taxon.ChineseName);

                            result.MatchValue = _GetMatchValueOfTwoString(_KeyWordWithoutCategory, chsNameWithoutCategory);
                            result.MatchObject = _MatchObject.PureChineseName;
                        }
                    }

                    // 尝试获得更高的匹配率，继续做部分关键字的全字符串匹配
                    if (result.MatchValue < 1)
                    {
                        var m = _GetMatchValueAndObject(taxon, _KeyWordWithoutCategory);

                        if (result.MatchValue < m.matchValue)
                        {
                            (result.MatchValue, result.MatchObject) = m;
                        }
                    }
                }
                // 关键字仅包含分类阶元名称的
                else
                {
                    result.CategoryRelativity = _GetCategoryRelativity(taxon.Category, _KeyWordCategory.Value);

                    // 分类阶元相同的
                    if (result.CategoryRelativity == _CategoryRelativity.Equals)
                    {
                        result.MatchValue = 1;
                        result.MatchObject = _MatchObject.ChineseName;
                    }
                    // 仅基本分类阶元相同的
                    else if (result.CategoryRelativity == _CategoryRelativity.Relevant)
                    {
                        result.MatchValue = 1;
                        result.MatchObject = _MatchObject.ChineseName;
                    }
                    // 分类阶元不相关的
                    else
                    {
                        // 做关键字的全字符串匹配
                        (result.MatchValue, result.MatchObject) = _GetMatchValueAndObject(taxon, _KeyWord);
                    }
                }
            }
            // 关键字不含分类阶元名称的
            else
            {
                result.CategoryRelativity = _CategoryRelativity.Irrelevant;

                // 做关键字与部分中文名的匹配
                if (!string.IsNullOrEmpty(taxon.ChineseName))
                {
                    (string chsNameWithoutCategory, _, _) = TaxonomicCategoryChineseExtension.SplitChineseName(taxon.ChineseName);

                    result.MatchValue = _GetMatchValueOfTwoString(_KeyWord, chsNameWithoutCategory);
                    result.MatchObject = _MatchObject.PureChineseName;
                }

                // 尝试获得更高的匹配率，继续做关键字的全字符串匹配
                if (result.MatchValue < 1)
                {
                    var m = _GetMatchValueAndObject(taxon, _KeyWord, matchChineseName: false);

                    if (result.MatchValue < m.matchValue)
                    {
                        (result.MatchValue, result.MatchObject) = m;
                    }
                }
            }

            return result;
        }

        // 递归获取所有符合匹配条件的子类群。
        private static void _GetMatchedChildren(this Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var child in taxon.Children)
            {
                if (child.IsNamed())
                {
                    _MatchResult mr = _GetMatchResultOfTaxon(child);

                    // 丢弃匹配率过低的结果
                    if (mr.MatchValue >= 0.1)
                    {
                        _MatchResults.Add(mr);
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

            _MatchResults = new List<_MatchResult>();

            _KeyWord = keyWord.Trim();
            (_KeyWordWithoutCategory, _, _KeyWordCategory) = TaxonomicCategoryChineseExtension.SplitChineseName(_KeyWord);

            taxon._GetMatchedChildren();

            var taxons = from mr in _MatchResults
                         orderby mr.MatchValue descending,
                         mr.CategoryRelativity ascending,
                         mr.MatchObject ascending
                         select mr.Taxon;

            List<Taxon> result = taxons.ToList();

            _MatchResults.Clear();

            return result;
        }
    }
}