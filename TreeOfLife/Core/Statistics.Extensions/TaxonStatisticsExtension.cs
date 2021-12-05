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

using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;

namespace TreeOfLife.Core.Statistics.Extensions
{
    // 统计结果（按分类阶元分组）。
    public class StatisticsResultOfRank
    {
        public Rank Rank { get; set; }
        public int TaxonCount { get; set; }

#if DEBUG
        public override string ToString() => $"{{Rank={Rank.GetChineseName()}, TaxonCount={TaxonCount}}}";
#endif
    }

    // 统计结果（按基本分类阶元分组）。
    public class StatisticsResultOfBasicRank
    {
        public Rank BasicRank { get; set; }
        public int TaxonCount { get; set; }
        public IReadOnlyList<StatisticsResultOfRank> Details { get; set; }

#if DEBUG
        public override string ToString() => $"{{BasicRank={BasicRank.GetChineseName()}, TaxonCount={TaxonCount}}}";
#endif
    }

    // 统计结果。
    public class StatisticsResult
    {
        public int TaxonCount { get; set; }
        public int NodeCount { get; set; }
        public IReadOnlyList<StatisticsResultOfBasicRank> Details { get; set; }

#if DEBUG
        public override string ToString() => $"{{TaxonCount={TaxonCount}, NodeCount={NodeCount}}}";
#endif
    }

    // 生物分类单元（类群）的统计相关扩展方法。
    public static class TaxonStatisticsExtension
    {
        // 递归统计所有子类群按分类阶元统计的数目。
        private static void _RecursionStatisticsChildren(Taxon taxon, Dictionary<Rank, int> dict, ref int nodeCount)
        {
            if (taxon.IsNamed)
            {
                if (!dict.ContainsKey(taxon.Rank))
                {
                    dict.Add(taxon.Rank, 0);
                }

                dict[taxon.Rank]++;
            }
            else
            {
                nodeCount++;
            }

            foreach (var child in taxon.Children)
            {
                _RecursionStatisticsChildren(child, dict, ref nodeCount);
            }
        }

        // 统计所有子类群按分类阶元统计的数目。
        public static StatisticsResult Statistics(this Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            Dictionary<Rank, int> dict = new Dictionary<Rank, int>();
            int nodeCount = 0;

            foreach (var child in taxon.Children)
            {
                _RecursionStatisticsChildren(child, dict, ref nodeCount);
            }

            List<StatisticsResultOfBasicRank> detailsOfBasicRank = new List<StatisticsResultOfBasicRank>();
            StatisticsResult result = new StatisticsResult() { NodeCount = nodeCount, Details = detailsOfBasicRank };

            foreach (var pair in dict)
            {
                int count = pair.Value;

                result.TaxonCount += count;

                Rank rank = pair.Key;
                Rank basicRank = rank.BasicRank();

                StatisticsResultOfBasicRank resultOfBasicRank = null;
                List<StatisticsResultOfRank> detailsOfRank = null;

                foreach (var detail in detailsOfBasicRank)
                {
                    if (detail.BasicRank == basicRank)
                    {
                        resultOfBasicRank = detail;
                        detailsOfRank = resultOfBasicRank.Details as List<StatisticsResultOfRank>;

                        break;
                    }
                }

                if (resultOfBasicRank is null)
                {
                    detailsOfRank = new List<StatisticsResultOfRank>();
                    resultOfBasicRank = new StatisticsResultOfBasicRank() { BasicRank = basicRank, Details = detailsOfRank };

                    detailsOfBasicRank.Add(resultOfBasicRank);
                }

                resultOfBasicRank.TaxonCount += count;
                detailsOfRank.Add(new StatisticsResultOfRank() { Rank = rank, TaxonCount = count });
            }

            foreach (var detail in detailsOfBasicRank)
            {
                (detail.Details as List<StatisticsResultOfRank>).Sort((value1, value2) => ((int)value2.Rank).CompareTo((int)value1.Rank));
            }

            detailsOfBasicRank.Sort((value1, value2) => ((int)value2.BasicRank).CompareTo((int)value1.BasicRank));

            return result;
        }
    }
}