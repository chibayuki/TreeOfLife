﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife.Core.Taxonomy.Extensions
{
    // 生物分类单元（类群）的匹配相关扩展方法。
    public static class TaxonParseExtension
    {
        // 识别一个名称，并应用到当前类群。
        public static void ParseCurrent(this Taxon taxon, string name)
        {
            name = name?.Trim();

            if (!string.IsNullOrEmpty(name))
            {
                List<char> chars = new List<char>(name);

                if (chars.Contains('?') || chars.Contains('？'))
                {
                    taxon.IsUndet = true;

                    chars.RemoveAll((ch) => ch == '?' || ch == '？');
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
                    chars = new List<char>(new string(chars.ToArray()).Trim());
                }

                if (chars.Count > 0)
                {
                    bool existLatin = false;
                    bool latinAtRight = false;

                    if (char.IsLower(chars[^1]) || char.IsUpper(chars[^1]) || char.IsDigit(chars[^1]) || chars[^1] == '.')
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

                            while (latinIndex >= 0 && (char.IsLower(chars[latinIndex]) || char.IsUpper(chars[latinIndex]) || char.IsWhiteSpace(chars[latinIndex]) || char.IsDigit(chars[latinIndex]) || chars[latinIndex] == '-' || chars[latinIndex] == '.' || chars[latinIndex] == '/'))
                            {
                                latinIndex--;
                            }

                            latinIndex++;

                            if (latinIndex > 0)
                            {
                                chsPart = new string(chars.ToArray(), 0, latinIndex);
                            }

                            while (latinIndex < chars.Count - 1 && char.IsWhiteSpace(chars[latinIndex]))
                            {
                                latinIndex++;
                            }

                            latinPart = new string(chars.ToArray(), latinIndex, chars.Count - latinIndex);
                        }
                        else
                        {
                            int chsIndex = 0;

                            while (chsIndex < chars.Count && (char.IsLower(chars[chsIndex]) || char.IsUpper(chars[chsIndex]) || char.IsWhiteSpace(chars[chsIndex]) || char.IsDigit(chars[chsIndex]) || chars[chsIndex] == '-' || chars[chsIndex] == '.' || chars[chsIndex] == '/'))
                            {
                                chsIndex++;
                            }

                            if (chsIndex < chars.Count)
                            {
                                chsPart = new string(chars.ToArray(), chsIndex, chars.Count - chsIndex);
                            }

                            while (chsIndex > 0 && (chsIndex >= chars.Count || char.IsWhiteSpace(chars[chsIndex])))
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
                        taxon.ScientificName = latinPart;
                    }

                    if (!string.IsNullOrWhiteSpace(chsPart))
                    {
                        Rank rank;
                        int rankNameIndex;

                        if (RankChineseExtension.TryParseRank(chsPart, out rank, out rankNameIndex))
                        {
                            // 特殊处理"类"，"类"一般指演化支
                            taxon.Rank = rank == Rank.Division ? Rank.Clade : rank;
                            taxon.ChineseName = rankNameIndex == 0 ? string.Empty : chsPart;
                        }
                        else
                        {
                            taxon.Rank = Rank.Clade;
                            taxon.ChineseName = chsPart;
                        }
                    }
                    else
                    {
                        taxon.Rank = Rank.Clade;
                    }

                    if (taxon.IsAnonymous)
                    {
                        taxon.Rank = Rank.Unranked;
                    }
                }
            }
        }

        // 识别若干个名称，并添加子类群。
        public static void ParseChildren(this Taxon taxon, params string[] names)
        {
            if (names is null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var name in names)
            {
                Taxon child = taxon.AddChild();

                child.ParseCurrent(name);

                // 按照"二名法"和"三名法"特殊处理"种"和"亚种"的匹配
                if (taxon.Rank.IsGenus() || taxon.Rank.IsSpecies())
                {
                    int wordNum = child.ScientificName.Count((ch) => ch == '.');

                    if (wordNum != 1 && wordNum != 2)
                    {
                        wordNum = child.ScientificName.Count((ch) => ch == ' ');
                    }

                    if (wordNum == 1)
                    {
                        child.Rank = Rank.Species;
                    }
                    else if (wordNum == 2)
                    {
                        child.Rank = Rank.Subspecies;
                    }
                }
            }
        }
    }
}