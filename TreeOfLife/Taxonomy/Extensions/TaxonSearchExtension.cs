/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.708.1000.M7.201230-2100

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
        private struct _SearchResult
        {
            public Taxon Taxon { get; set; }
            public double Matching { get; set; }

            public struct Comparer : IComparer<_SearchResult>
            {
                public int Compare(_SearchResult x, _SearchResult y) => x.Matching.CompareTo(y.Matching);
            }
        }

        private static string _KeyWord;
        private static double _KeyWordLength;
        private static TaxonomicCategory? _ParsedCategory;
        private static List<_SearchResult> _SearchResults;

        private static void _GetMatchedChildren(this Taxon taxon)
        {
            foreach (var child in taxon.Children)
            {
                if (child.IsNamed())
                {
                    _SearchResult sr = new _SearchResult() { Taxon = child };

                    double matching = 0;

                    // 为了匹配"是"或者"继承"某一分类阶元的类群
                    if (_ParsedCategory.HasValue)
                    {
                        // 使名称更短的类群匹配率更高，分类阶元的中文名按平均1.5个字符计，学名和中文名长度做加权平均处理
                        if (child.Category == _ParsedCategory.Value)
                        {
                            matching = 1.5 / (0.5 * child.BotanicalName.Length + (1.5 + child.ChineseName.Length));
                        }
                        // 对于"继承"，匹配率减至1/3
                        else if (child.Category.BasicCategory() == _ParsedCategory.Value)
                        {
                            matching = 0.5 / (0.5 * child.BotanicalName.Length + (1.5 + child.ChineseName.Length));
                        }
                    }

                    // 只有未匹配到分类阶元，才有必要继续匹配
                    if (matching <= 0)
                    {
                        if (matching < 1 && child.BotanicalName.Contains(_KeyWord))
                        {
                            matching = Math.Max(matching, _KeyWordLength / child.BotanicalName.Length);
                        }

                        if (matching < 1 && child.ChineseName.Contains(_KeyWord))
                        {
                            matching = Math.Max(matching, _KeyWordLength / child.ChineseName.Length);
                        }

                        if (matching < 1 && child.Synonyms.Count > 0)
                        {
                            foreach (var synonym in child.Synonyms)
                            {
                                if (synonym.Contains(_KeyWord))
                                {
                                    matching = Math.Max(matching, _KeyWordLength / synonym.Length);

                                    if (matching >= 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (matching < 1 && child.Tags.Count > 0)
                        {
                            foreach (var tag in child.Tags)
                            {
                                if (tag.Contains(_KeyWord))
                                {
                                    matching = Math.Max(matching, _KeyWordLength / tag.Length);

                                    if (matching >= 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (matching > 0)
                    {
                        sr.Matching = matching;

                        _SearchResults.Add(sr);
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

            _KeyWord = keyWord;
            _KeyWordLength = _KeyWord.Length;
            _ParsedCategory = TaxonomicCategoryChineseExtension.ParseCategory(_KeyWord);
            _SearchResults = new List<_SearchResult>();

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