/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1322.1000.M13.210925-1400

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreeOfLife.Core.Geology;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;

namespace TreeOfLife.Core.Validation
{
#if DEBUG

    public class LevelValidator : IValidator
    {
        public override string ToString() => "Level 错误";

        public bool IsValid(Taxon taxon) => taxon.Level == (taxon.IsRoot ? 0 : taxon.Parent.Level + 1);
    }

    public class IndexValidator : IValidator
    {
        public override string ToString() => "Index 错误";

        public bool IsValid(Taxon taxon) => taxon.Index == (taxon.IsRoot ? -1 : (taxon.Parent.Children as List<Taxon>).IndexOf(taxon));
    }

    public class InheritValidator : IValidator
    {
        public override string ToString() => "Inherit 关系错误";

        public bool IsValid(Taxon taxon) => !taxon.Children.Any((t) => t.Parent != taxon);
    }

    public class ExcludeValidator : IValidator
    {
        public override string ToString() => "Exclude 关系错误";

        public bool IsValid(Taxon taxon) => !taxon.ExcludeBy.Any((t) => !t.Excludes.Contains(taxon)) && !taxon.Excludes.Any((t) => !t.ExcludeBy.Contains(taxon));
    }

    public class IncludeValidator : IValidator
    {
        public override string ToString() => "Include 关系错误";

        public bool IsValid(Taxon taxon) => !taxon.IncludeBy.Any((t) => !t.Includes.Contains(taxon)) && !taxon.Includes.Any((t) => !t.IncludeBy.Contains(taxon));
    }

    public class NodeRankValidator : IValidator
    {
        public override string ToString() => "匿名节点不应具有分级";

        public bool IsValid(Taxon taxon) => taxon.IsNamed || taxon.Rank == Rank.Unranked;
    }

    public class BirthValidator : IValidator
    {
        public override string ToString() => "诞生年代不应是现代";

        public bool IsValid(Taxon taxon) => !taxon.Birth.IsPresent;
    }

    public class ExtinctionValidator : IValidator
    {
        public override string ToString() => "未灭绝类群不应具有灭绝年代，\n已灭绝类群的灭绝年代不应是现代";

        public bool IsValid(Taxon taxon) => taxon.IsExtinct ? !taxon.Extinction.IsPresent : taxon.Extinction == GeoChron.Empty;
    }

#endif

    // 长度：学名通常至少包含3个字符。
    public class NameLengthValidator : IValidator
    {
        public override string ToString() => "学名长度过短";

        public bool IsValid(Taxon taxon) => taxon.IsAnonymous || taxon.ScientificName.Length >= 3;
    }

    // 大小写：学名应该且仅应该首字母大写。
    public class NameUppercaseValidator : IValidator
    {
        public override string ToString() => "学名不符合仅首字母大写";

        public bool IsValid(Taxon taxon) => taxon.IsAnonymous || (!char.IsLower(taxon.ScientificName[0]) && !taxon.ScientificName[1..].Any((c) => char.IsUpper(c)));
    }

    // 单词数：学名通常是一个单词，种以上（不含）应该是1个单词，种应该是2个单词，亚种应该是3个单词。
    public class NameWordCountValidator : IValidator
    {
        public override string ToString() => "学名不符合单词数目要求";

        public bool IsValid(Taxon taxon)
        {
            if (taxon.IsAnonymous)
            {
                return true;
            }

            int wordCount = taxon.ScientificName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            if (taxon.Rank == Rank.Species)
            {
                return wordCount == 2;
            }
            else if (taxon.Rank == Rank.Subspecies)
            {
                return wordCount == 3;
            }
            else
            {
                return wordCount == 1;
            }
        }
    }

    // 字符构成：学名应该仅由拉丁字母、点（.）和空格（ ）组成，种以上（不含）应该仅由拉丁字母组成，种以下（含）还可包含点和空格。
    public class NameCharacterValidator : IValidator
    {
        public override string ToString() => "学名含有意外的字符";

        public bool IsValid(Taxon taxon)
        {
            if (taxon.IsAnonymous)
            {
                return true;
            }

            return !taxon.ScientificName.Any((c) => !char.IsLetter(c) && c != '.' && c != ' ');
        }
    }

    // 分隔符：学名如果包含点（.），那么点只应该出现在每个单词结尾，并且不应该出现在最后一个单词结尾，点后面必须有空格，点和空格不应该连续出现多个。
    public class NameSeparatorValidator : IValidator
    {
        public override string ToString() => "学名的单词分隔不正确";

        public bool IsValid(Taxon taxon)
        {
            if (taxon.IsAnonymous)
            {
                return true;
            }

            if (taxon.ScientificName.EndsWith('.'))
            {
                return false;
            }

            string[] str = taxon.ScientificName.Split(' ');

            if (str.Any((s) => string.IsNullOrEmpty(s) || (s.Contains('.') && s.IndexOf('.') != s.Length - 1 && s.LastIndexOf('.') != s.Length - 1)))
            {
                return false;
            }

            return true;
        }
    }

    // 时间轴完整：对于灭绝物种，不应该仅设置诞生年代或灭绝年代之一。
    public class TimelineCompletenessValidator : IValidator
    {
        public override string ToString() => "仅设置了诞生年代或灭绝年代之一";

        public bool IsValid(Taxon taxon) => !taxon.IsExtinct || taxon.Birth.IsEmpty == taxon.Extinction.IsEmpty;
    }

    // 时间轴自洽：对于灭绝物种，灭绝年代应该晚于诞生年代。
    public class TimelineConsistencyValidator : IValidator
    {
        public override string ToString() => "灭绝年代早于诞生年代";

        public bool IsValid(Taxon taxon) => !taxon.IsExtinct || taxon.Birth.IsEmpty || taxon.Extinction.IsEmpty || taxon.Birth <= taxon.Extinction;
    }

    // 演化顺序自洽：任何物种的诞生年代不应该早于其上级类群（祖先）的诞生年代。
    public class EvolutionOrderValidator : IValidator
    {
        public override string ToString() => "诞生年代早于其祖先";

        public bool IsValid(Taxon taxon)
        {
            if (!taxon.Birth.IsEmpty)
            {
                Taxon parent = taxon.Parent;

                while (!parent.IsRoot && parent.Birth.IsEmpty)
                {
                    parent = parent.Parent;
                }

                if (!parent.IsRoot && !parent.Birth.IsEmpty)
                {
                    if (taxon.Birth < parent.Birth)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    // 学名缺失：不应该仅有中文名而没有学名。
    public class NameMissingValidator : IValidator
    {
        public override string ToString() => "未设置学名";

        public bool IsValid(Taxon taxon) => !string.IsNullOrEmpty(taxon.ScientificName) || string.IsNullOrEmpty(taxon.ChineseName);
    }

    // 未设置分级：具名类群应该设置分级。
    public class RankMissingValidator : IValidator
    {
        public override string ToString() => "未设置分级";

        public bool IsValid(Taxon taxon) => taxon.IsAnonymous || !taxon.Rank.IsUnranked();
    }

    // 中文名后缀：中文名后缀应该与分级相符（种通常不需要后缀）。
    public class ChineseSuffixValidator : IValidator
    {
        public override string ToString() => "中文名与分级不符";

        public bool IsValid(Taxon taxon)
        {
            if (string.IsNullOrEmpty(taxon.ChineseName))
            {
                return true;
            }

            var split = RankChineseExtension.SplitChineseName(taxon.ChineseName);

            if (taxon.Rank.IsUnranked())
            {
                return split.rank is null || split.rank == taxon.Rank;
            }
            else if (taxon.Rank.IsClade())
            {
                if (taxon.ChineseName.EndsWith("类"))
                {
                    return true;
                }

                return split.rank is null || split.rank == taxon.Rank;
            }
            else if (taxon.Rank.IsSpecies())
            {
                return split.rank is null || split.rank == taxon.Rank;
            }
            else
            {
                return split.rank == taxon.Rank;
            }
        }
    }

    // 异名标签唯一：异名、标签、异名与标签之间不应该存在重复项。
    public class TagUniqueValidator : IValidator
    {
        public override string ToString() => "异名或标签含有重复项";

        public bool IsValid(Taxon taxon)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (string synonym in taxon.Synonyms)
            {
                if (!dict.TryAdd(synonym, null))
                {
                    return false;
                }
            }

            foreach (string tag in taxon.Tags)
            {
                if (!dict.TryAdd(tag, null))
                {
                    return false;
                }
            }

            return true;
        }
    }

    // 节点完整：匿名类群应该有姊妹群，并且应该有至少2个下级类群（后代）。
    public class NodeStructureValidator : IValidator
    {
        public override string ToString() => "节点结构不正确";

        public bool IsValid(Taxon taxon) => taxon.IsNamed || (taxon.Parent.Children.Count > 1 && taxon.Children.Count > 1);
    }
}