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
    // 生物分类单元（类群）的搜索相关扩展方法。
    public static class TaxonSearchExtension
    {
        // 获取两个字符串的最大相同子串/子序列的长度（LCS）。
        private static int _GetCommonSubsequenceLength(string str1, string str2, bool continuous = true)
        {
            // 原理：
            // 将两个字符串（之前各空一位再）分别横排和竖排，以二者的长度加一为行列数目构造一个矩阵，
            // 则矩阵的 (i, j) 元素表示 str1[i - 1] 与 str2[j - 1] 的比较，矩阵的 (i + 1, j + 1) 元素表示 str1[i] 与 str2[j] 的比较，
            // 类推可知，矩阵中所有与主对角线平行的对角元素表示将两个字符串相对滑动一段距离后重叠部分的逐字符比较，
            // 那么所有对角线上连续比中最多的数目即最大相同子串的长度，
            // 设法使矩阵的每个元素表示所在对角线的连续比中数目，
            // 令矩阵的第 0 行和第 0 列元素全部为 0，每当比中一个字符，令当前元素 (i, j) 等于 (i - 1, j - 1) + 1，
            // 否则，对于要求子序列是连续的（子串）令其等于 0，对于不要求子序列是连续的令其等于 Max((i - 1, j), (i, j - 1))，
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
                        matrix[i, j] = (continuous ? 0 : Math.Max(matrix[i - 1, j], matrix[i, j - 1]));
                    }
                }
            }

            return result;
        }

        // 获取两个字符串的连续-离散 LCS 几何平均匹配率和匹配字符数。
        private static (double matchValue, double matchLength) _GetMatchValueOfTwoString(string str1, string str2)
        {
            if (!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2))
            {
                // 出于性能和实际情况考虑，最多只比较前32个字符，并且先转换为大写再比较
                string s1 = (str1.Length > 32 ? str1[0..32] : str1).ToUpperInvariant();
                string s2 = (str2.Length > 32 ? str2[0..32] : str2).ToUpperInvariant();

                // int commonPartLength = _GetCommonSubsequenceLength(s1, s2);
                // return ((double)commonPartLength * commonPartLength / str1.Length / str2.Length, commonPartLength);
                // 使用加法而非乘法，使得匹配率与匹配字符数更好地符合线性关系，从而使搜索结果更符合肉眼直觉。
                // 例如：
                // (1) "ABCD"与"ABXY"，匹配率按乘法计算为 (2*2)/(4*4)=1/4，按加法计算为 (2+2)/(4+4)=1/2，
                //     后者更好地表示"二者有 1/2 是相同的"。
                // (2) "ABCD"与"AXYZ"，匹配率按乘法计算为 (1*1)/(4*4)=1/16，按加法计算为 (1+1)/(4+4)=1/4，
                //     后者不仅更好地表示"二者有 1/4 是相同的"，还更好的表示"与 (1) 相比，相似的程度下降了 1/2"。
                // (3) "A"与"AXYZ"，匹配率按乘法计算为 (1*1)/(1*4)=1/4，按加法计算为 (1+1)/(1+4)=2/5，
                //     按乘法计算得到了与 (1) 相同的匹配率，按加法计算得到的匹配率则介于 (1) 与 (2) 之间，后者得到的相似程度的排序关系更符合肉眼直觉。
                // 除此之外，当匹配率存在阈值约束时，二者经筛选留下的结果显然是不同的。
                // 以及，对于同一组字符串对，二者得到的排序是可以不同的，例如：
                // (4*4)/(16*16)=16/256 < (4*4)/(12*21)=16/252，而 (4+4)/(16+16)=8/32 > (4+4)/(12+21)=8/33。

                int continueousCommonPartLength = _GetCommonSubsequenceLength(s1, s2, true);
                int discreteCommonPartLength = _GetCommonSubsequenceLength(s1, s2, false);
                double matchLength = Math.Sqrt((double)continueousCommonPartLength * discreteCommonPartLength);

                return (2 * matchLength / (str1.Length + str2.Length), matchLength);
            }
            else
            {
                return (0, 0);
            }
        }

        // 获取类群的中文名不含分类阶元的部分。
        private static string _GetChineseNameWithoutCategory(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            // 特殊处理"类"字，使演化支类群只去除中文名结尾的"类"字
            if (taxon.Category.IsClade() && taxon.ChineseName.EndsWith("类"))
            {
                return taxon.ChineseName[..^1];
            }
            else
            {
                return TaxonomicCategoryChineseExtension.SplitChineseName(taxon.ChineseName).headPart;
            }
        }

        // 分类阶元的相关性。
        private enum _CategoryRelativity
        {
            Equals,
            Relevant,
            Irrelevant
        }

        // 获取两个分类阶元的相关性。
        private static _CategoryRelativity _GetCategoryRelativity(TaxonomicCategory category1, TaxonomicCategory category2)
        {
            // 分类阶元相同
            if (category1 == category2)
            {
                return _CategoryRelativity.Equals;
            }
            // 仅基本分类阶元相同
            else if (category1.BasicCategory() == category2.BasicCategory())
            {
                return _CategoryRelativity.Relevant;
            }
            // 分类阶元不相关
            else
            {
                return _CategoryRelativity.Irrelevant;
            }
        }

        // 匹配的对象。
        private enum _MatchObject
        {
            ChineseNameWithoutCategory,
            ChineseName,
            BotanicalName,
            Synonyms,
            Tags
        }

        // 获取对指定类群做全字符串匹配的最佳匹配率和匹配对象。
        private static (double matchValue, double matchLength, _MatchObject matchObject) _GetMatchValueAndObject(Taxon taxon, string str, bool matchChineseName = true, bool matchBotanicalName = true, bool matchSynonyms = true, bool matchTags = true)
        {
            double matchValue = 0;
            double matchLength = 0;
            _MatchObject matchObject = _MatchObject.ChineseName;

            if (matchChineseName && !string.IsNullOrEmpty(taxon.ChineseName))
            {
                (matchValue, matchLength) = _GetMatchValueOfTwoString(str, taxon.ChineseName);
            }

            if (matchBotanicalName && matchValue < 1 && !string.IsNullOrEmpty(taxon.BotanicalName))
            {
                var mv = _GetMatchValueOfTwoString(str, taxon.BotanicalName);

                if (matchValue < mv.matchValue || (matchValue == mv.matchValue && matchLength < mv.matchLength))
                {
                    (matchValue, matchLength) = mv;
                    matchObject = _MatchObject.BotanicalName;
                }
            }

            if (matchSynonyms && matchValue < 1 && taxon.Synonyms.Count > 0)
            {
                foreach (var synonym in taxon.Synonyms)
                {
                    if (!string.IsNullOrEmpty(synonym))
                    {
                        var mv = _GetMatchValueOfTwoString(str, synonym);

                        if (matchValue < mv.matchValue || (matchValue == mv.matchValue && matchLength < mv.matchLength))
                        {
                            (matchValue, matchLength) = mv;
                            matchObject = _MatchObject.Synonyms;
                        }

                        if (matchValue >= 1)
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
                        var mv = _GetMatchValueOfTwoString(str, tag);

                        if (matchValue < mv.matchValue || (matchValue == mv.matchValue && matchLength < mv.matchLength))
                        {
                            (matchValue, matchLength) = mv;
                            matchObject = _MatchObject.Tags;
                        }

                        if (matchValue >= 1)
                        {
                            break;
                        }
                    }
                }
            }

            return (matchValue, matchLength, matchObject);
        }

        // 匹配结果。
        private struct _MatchResult
        {
            public Taxon Taxon { get; set; }

            // 连续-离散 LCS 几何平均匹配率。
            public double MatchValue { get; set; }

            // 连续-离散 LCS 几何平均匹配字符数。
            public double MatchLength { get; set; }

            public _CategoryRelativity CategoryRelativity { get; set; }

            public _MatchObject MatchObject { get; set; }

            //

#if DEBUG

            public override string ToString()
            {
                return string.Concat("{Taxon=", Taxon, ", MatchValue=", MatchValue, ", MatchLength=", MatchLength, ", CategoryRelativity=", CategoryRelativity, ", MatchObject=", MatchObject, "}");
            }

#endif
        }

        private static List<_MatchResult> _MatchResults; // 匹配结果列表。

        private static string _KeyWord; // 关键字。
        private static TaxonomicCategory? _KeyWordCategory; // 关键字中包含的分类阶元。
        private static string _KeyWordWithoutCategory; // 关键字除了表示分类阶元（如果有）之外的部分。
        private static string _KeyWordWithoutCategoryAsClade; // 当关键字中包含的分类阶元为类并将其视为演化支时，关键字除了表示分类阶元之外的部分。

        // 获取指定类群在当前匹配条件下的匹配结果。
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
                    // 特殊处理"类"字，使其在特定情况下也表示演化支
                    if (taxon.Category.IsClade() && _KeyWordCategory.Value.IsDivision() && taxon.ChineseName.EndsWith("类"))
                    {
                        result.CategoryRelativity = _CategoryRelativity.Equals;
                    }
                    else
                    {
                        result.CategoryRelativity = _GetCategoryRelativity(taxon.Category, _KeyWordCategory.Value);
                    }

                    // 做部分关键字与部分中文名的匹配
                    if (!string.IsNullOrEmpty(taxon.ChineseName))
                    {
                        string chsNameWithoutCategory = _GetChineseNameWithoutCategory(taxon);
                        string keyWordWithoutCategory = (taxon.Category.IsClade() ? _KeyWordWithoutCategoryAsClade : _KeyWordWithoutCategory);

                        (result.MatchValue, result.MatchLength) = _GetMatchValueOfTwoString(keyWordWithoutCategory, chsNameWithoutCategory);
                        result.MatchObject = _MatchObject.ChineseNameWithoutCategory;
                    }

                    // 尝试获得更高的匹配率，继续做关键字的全字符串匹配
                    if (result.MatchValue < 1)
                    {
                        var m = _GetMatchValueAndObject(taxon, _KeyWord, matchChineseName: false);

                        if (result.MatchValue < m.matchValue || (result.MatchValue == m.matchValue && result.MatchLength < m.matchLength))
                        {
                            (result.MatchValue, result.MatchLength, result.MatchObject) = m;
                        }
                    }
                }
                // 关键字仅包含分类阶元名称的
                else
                {
                    result.CategoryRelativity = _GetCategoryRelativity(taxon.Category, _KeyWordCategory.Value);

                    // 分类阶元相同或相关的
                    if (result.CategoryRelativity is _CategoryRelativity.Equals or _CategoryRelativity.Relevant)
                    {
                        result.MatchValue = 1;
                        result.MatchLength = _KeyWord.Length;
                        result.MatchObject = _MatchObject.ChineseName;
                    }
                    // 分类阶元不相关的
                    else
                    {
                        // 做关键字的全字符串匹配
                        (result.MatchValue, result.MatchLength, result.MatchObject) = _GetMatchValueAndObject(taxon, _KeyWord);
                    }
                }
            }
            // 关键字不含分类阶元名称的
            else
            {
                result.CategoryRelativity = _CategoryRelativity.Irrelevant;

                // 做关键字的全字符串匹配
                (result.MatchValue, result.MatchLength, result.MatchObject) = _GetMatchValueAndObject(taxon, _KeyWord);
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

                    // 丢弃匹配率过低的结果，至少每 5 个字符中需要有 1 个匹配的字符，(1+1)/(1+5)=1/3
                    if (mr.MatchValue >= 1.0 / 3.0)
                    {
                        _MatchResults.Add(mr);
                    }
                }

                child._GetMatchedChildren();
            }
        }

        // 匹配程度
        public enum MatchLevel
        {
            Perfect, // 完全匹配
            High, // 匹配度较高
            Low // 匹配度较低
        }

        // 搜索符合指定的关键词的子类群。
        public static IReadOnlyList<(Taxon taxon, MatchLevel matchLevel)> Search(this Taxon taxon, string keyWord)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            keyWord = keyWord?.Trim();

            if (string.IsNullOrEmpty(keyWord))
            {
                return null;
            }
            else
            {
                _MatchResults = new List<_MatchResult>();

                _KeyWord = keyWord;
                (_KeyWordWithoutCategory, _, _KeyWordCategory) = TaxonomicCategoryChineseExtension.SplitChineseName(_KeyWord);

                if (_KeyWordCategory != null && _KeyWordCategory.Value.IsDivision() && _KeyWord.EndsWith("类"))
                {
                    _KeyWordWithoutCategoryAsClade = _KeyWord[..^1];
                }
                else
                {
                    _KeyWordWithoutCategoryAsClade = _KeyWordWithoutCategory;
                }

                taxon._GetMatchedChildren();

                var matchResults = from mr in _MatchResults
                                   orderby mr.MatchValue descending,
                                   mr.MatchLength descending,
                                   mr.CategoryRelativity ascending,
                                   mr.MatchObject ascending
                                   select mr;

                List<(Taxon, MatchLevel)> result = new List<(Taxon, MatchLevel)>();

                // 关键字包含分类阶元名称的
                if (_KeyWordCategory != null)
                {
                    // 关键字包含分类阶元名称，还包含除此之外的，按匹配率与分类阶元相关性设定匹配程度
                    if (!string.IsNullOrEmpty(_KeyWordWithoutCategory))
                    {
                        foreach (var mr in matchResults)
                        {
                            if (mr.MatchValue >= 1.0 && mr.CategoryRelativity == _CategoryRelativity.Equals)
                            {
                                result.Add((mr.Taxon, MatchLevel.Perfect));
                            }
                            else if (mr.MatchValue >= 1.0 || (mr.MatchValue >= 2.0 / 3.0 && (mr.CategoryRelativity is _CategoryRelativity.Equals or _CategoryRelativity.Relevant)))
                            {
                                result.Add((mr.Taxon, MatchLevel.High));
                            }
                            else
                            {
                                result.Add((mr.Taxon, MatchLevel.Low));
                            }
                        }
                    }
                    // 关键字仅包含分类阶元名称的，按分类阶元相关性设定匹配程度
                    else
                    {
                        foreach (var mr in matchResults)
                        {
                            if (mr.CategoryRelativity == _CategoryRelativity.Equals)
                            {
                                result.Add((mr.Taxon, MatchLevel.Perfect));
                            }
                            else if (mr.CategoryRelativity == _CategoryRelativity.Relevant)
                            {
                                result.Add((mr.Taxon, MatchLevel.High));
                            }
                            else
                            {
                                result.Add((mr.Taxon, MatchLevel.Low));
                            }
                        }
                    }
                }
                // 关键字不含分类阶元名称的，按匹配率设定匹配程度
                else
                {
                    foreach (var mr in matchResults)
                    {
                        if (mr.MatchValue >= 1.0)
                        {
                            result.Add((mr.Taxon, MatchLevel.Perfect));
                        }
                        else if (mr.MatchValue >= 2.0 / 3.0)
                        {
                            result.Add((mr.Taxon, MatchLevel.High));
                        }
                        else
                        {
                            result.Add((mr.Taxon, MatchLevel.Low));
                        }
                    }
                }

                _MatchResults.Clear();

                return result;
            }
        }
    }
}