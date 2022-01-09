/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

namespace TreeOfLife.Core.Taxonomy
{
    // 生物分类阶元的匹配相关扩展方法（只提供Rank.ToString方法的逆，所以与Rank放在同一命名空间）。
    public static class RankParseExtension
    {
        // 如果Rank成员发生重命名，需要在此字典中添加对旧名称的兼容：
        // { "旧名称".ToUpperInvariant(), Rank.枚举 }
        private static Dictionary<string, Rank> _StringToRankTable = new Dictionary<string, Rank>()
        {
            { Rank.Unranked.ToString().ToUpperInvariant(), Rank.Unranked },

            { Rank.Clade.ToString().ToUpperInvariant(), Rank.Clade },

            { Rank.Strain.ToString().ToUpperInvariant(), Rank.Strain },

            { Rank.Subform.ToString().ToUpperInvariant(), Rank.Subform },
            { Rank.Form.ToString().ToUpperInvariant(), Rank.Form },

            { Rank.Subseries.ToString().ToUpperInvariant(), Rank.Subseries },
            { Rank.Series.ToString().ToUpperInvariant(), Rank.Series },

            { Rank.Subdivision.ToString().ToUpperInvariant(), Rank.Subdivision },
            { Rank.Division.ToString().ToUpperInvariant(), Rank.Division },

            { Rank.Subsection.ToString().ToUpperInvariant(), Rank.Subsection },
            { Rank.Section.ToString().ToUpperInvariant(), Rank.Section },

            { Rank.Infracohort.ToString().ToUpperInvariant(), Rank.Infracohort },
            { Rank.Subcohort.ToString().ToUpperInvariant(), Rank.Subcohort },
            { Rank.Cohort.ToString().ToUpperInvariant(), Rank.Cohort },
            { Rank.Supercohort.ToString().ToUpperInvariant(), Rank.Supercohort },
            { Rank.Megacohort.ToString().ToUpperInvariant(), Rank.Megacohort },

            { Rank.Subtribe.ToString().ToUpperInvariant(), Rank.Subtribe },
            { Rank.Tribe.ToString().ToUpperInvariant(), Rank.Tribe },

            { Rank.Subvariety.ToString().ToUpperInvariant(), Rank.Subvariety },
            { Rank.Variety.ToString().ToUpperInvariant(), Rank.Variety },
            { Rank.Subspecies.ToString().ToUpperInvariant(), Rank.Subspecies },
            { Rank.Species.ToString().ToUpperInvariant(), Rank.Species },

            { Rank.Subgenus.ToString().ToUpperInvariant(), Rank.Subgenus },
            { Rank.Genus.ToString().ToUpperInvariant(), Rank.Genus },

            { Rank.Subfamily.ToString().ToUpperInvariant(), Rank.Subfamily },
            { Rank.Family.ToString().ToUpperInvariant(), Rank.Family },
            { Rank.Superfamily.ToString().ToUpperInvariant(), Rank.Superfamily },

            { Rank.Parvorder.ToString().ToUpperInvariant(), Rank.Parvorder },
            { Rank.Infraorder.ToString().ToUpperInvariant(), Rank.Infraorder },
            { Rank.Suborder.ToString().ToUpperInvariant(), Rank.Suborder },
            { Rank.Order.ToString().ToUpperInvariant(), Rank.Order },
            { Rank.Mirorder.ToString().ToUpperInvariant(), Rank.Mirorder },
            { "Hyperorder".ToUpperInvariant(), Rank.Mirorder },
            { Rank.Grandorder.ToString().ToUpperInvariant(), Rank.Grandorder },
            { Rank.Superorder.ToString().ToUpperInvariant(), Rank.Superorder },
            { Rank.Megaorder.ToString().ToUpperInvariant(), Rank.Megaorder },

            { Rank.Parvclass.ToString().ToUpperInvariant(), Rank.Parvclass },
            { Rank.Infraclass.ToString().ToUpperInvariant(), Rank.Infraclass },
            { Rank.Subclass.ToString().ToUpperInvariant(), Rank.Subclass },
            { Rank.Class.ToString().ToUpperInvariant(), Rank.Class },
            { Rank.Superclass.ToString().ToUpperInvariant(), Rank.Superclass },
            { Rank.Megaclass.ToString().ToUpperInvariant(), Rank.Megaclass },

            { Rank.Parvphylum.ToString().ToUpperInvariant(), Rank.Parvphylum },
            { Rank.Infraphylum.ToString().ToUpperInvariant(), Rank.Infraphylum },
            { Rank.Subphylum.ToString().ToUpperInvariant(), Rank.Subphylum },
            { Rank.Phylum.ToString().ToUpperInvariant(), Rank.Phylum },
            { Rank.Superphylum.ToString().ToUpperInvariant(), Rank.Superphylum },

            { Rank.Infrakingdom.ToString().ToUpperInvariant(), Rank.Infrakingdom },
            { Rank.Subkingdom.ToString().ToUpperInvariant(), Rank.Subkingdom },
            { Rank.Kingdom.ToString().ToUpperInvariant(), Rank.Kingdom },
            { Rank.Superkingdom.ToString().ToUpperInvariant(), Rank.Superkingdom },

            { Rank.Domain.ToString().ToUpperInvariant(), Rank.Domain },
            { Rank.Superdomain.ToString().ToUpperInvariant(), Rank.Superdomain }
        };

        public static Rank? ParseRank(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }

            //

            if (_StringToRankTable.TryGetValue(text.ToUpperInvariant(), out Rank rank))
            {
                return rank;
            }
            else
            {
                return null;
            }
        }
    }
}