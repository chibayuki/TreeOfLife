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

using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;

namespace TreeOfLife.Core.Validation
{
#if DEBUG

    public class LevelValidator : IValidator
    {
        private LevelValidator() { }

        public static readonly LevelValidator Instance = new LevelValidator();

        public override string ToString() => "Level 错误";

        public bool IsValid(Taxon taxon) => taxon.Level == (taxon.IsRoot ? 0 : taxon.Parent.Level + 1) ? true : throw new ApplicationException();
    }

    public class IndexValidator : IValidator
    {
        private IndexValidator() { }

        public static readonly IndexValidator Instance = new IndexValidator();

        public override string ToString() => "Index 错误";

        public bool IsValid(Taxon taxon) => taxon.Index == (taxon.IsRoot ? -1 : (taxon.Parent.Children as List<Taxon>).IndexOf(taxon)) ? true : throw new ApplicationException();
    }

    public class InheritValidator : IValidator
    {
        private InheritValidator() { }

        public static readonly InheritValidator Instance = new InheritValidator();

        public override string ToString() => "Inherit 关系错误";

        public bool IsValid(Taxon taxon) => !taxon.Children.Any((t) => t.Parent != taxon) ? true : throw new ApplicationException();
    }

    public class ExcludeValidator : IValidator
    {
        private ExcludeValidator() { }

        public static readonly ExcludeValidator Instance = new ExcludeValidator();

        public override string ToString() => "Exclude 关系错误";

        public bool IsValid(Taxon taxon) => !taxon.ExcludeBy.Any((t) => !t.Excludes.Contains(taxon)) && !taxon.Excludes.Any((t) => !t.ExcludeBy.Contains(taxon)) ? true : throw new ApplicationException();
    }

    public class IncludeValidator : IValidator
    {
        private IncludeValidator() { }

        public static readonly IncludeValidator Instance = new IncludeValidator();

        public override string ToString() => "Include 关系错误";

        public bool IsValid(Taxon taxon) => !taxon.IncludeBy.Any((t) => !t.Includes.Contains(taxon)) && !taxon.Includes.Any((t) => !t.IncludeBy.Contains(taxon)) ? true : throw new ApplicationException();
    }

    public class NodeRankValidator : IValidator
    {
        private NodeRankValidator() { }

        public static readonly NodeRankValidator Instance = new NodeRankValidator();

        public override string ToString() => "匿名节点不应具有分级";

        public bool IsValid(Taxon taxon) => taxon.IsNamed || taxon.Rank == Rank.Unranked ? true : throw new ApplicationException();
    }

    public class BirthValidator : IValidator
    {
        private BirthValidator() { }

        public static readonly BirthValidator Instance = new BirthValidator();

        public override string ToString() => "诞生年代不应是现代";

        public bool IsValid(Taxon taxon) => !taxon.Birth.IsPresent ? true : throw new ApplicationException();
    }

    public class ExtinctionValidator : IValidator
    {
        private ExtinctionValidator() { }

        public static readonly ExtinctionValidator Instance = new ExtinctionValidator();

        public override string ToString() => "未灭绝类群不应具有灭绝年代，\n已灭绝类群的灭绝年代不应是现代";

        public bool IsValid(Taxon taxon) => taxon.IsExtinct ? !taxon.Extinction.IsPresent : taxon.Extinction.IsEmpty ? true : throw new ApplicationException();
    }

#endif

    // 学名缺失：不应该仅有中文名而没有学名。
    public class NameMissingValidator : IValidator
    {
        private NameMissingValidator() { }

        public static readonly NameMissingValidator Instance = new NameMissingValidator();

        public override string ToString() => "未设置学名";

        public bool IsValid(Taxon taxon) => !string.IsNullOrEmpty(taxon.ScientificName) || string.IsNullOrEmpty(taxon.ChineseName);
    }

    // 学名长度：学名通常至少包含3个字符。
    public class NameLengthValidator : IValidator
    {
        private NameLengthValidator() { }

        public static readonly NameLengthValidator Instance = new NameLengthValidator();

        public override string ToString() => "学名长度过短";

        public bool IsValid(Taxon taxon) => string.IsNullOrEmpty(taxon.ScientificName) || taxon.ScientificName.Length >= 3;
    }

    // 学名大小写：学名应该且仅应该首字母大写。
    public class NameUppercaseValidator : IValidator
    {
        private NameUppercaseValidator() { }

        public static readonly NameUppercaseValidator Instance = new NameUppercaseValidator();

        public override string ToString() => "学名不符合仅首字母大写";

        public bool IsValid(Taxon taxon) => string.IsNullOrEmpty(taxon.ScientificName) || (!char.IsLower(taxon.ScientificName[0]) && !taxon.ScientificName[1..].Any((c) => char.IsUpper(c)));
    }

    // 学名单词数：学名通常是一个单词，种以上（不含）应该是1个单词，种应该是2个单词，亚种应该是3个单词。
    public class NameWordCountValidator : IValidator
    {
        private NameWordCountValidator() { }

        public static readonly NameWordCountValidator Instance = new NameWordCountValidator();

        public override string ToString() => "学名不符合单词数目要求";

        public bool IsValid(Taxon taxon)
        {
            if (string.IsNullOrEmpty(taxon.ScientificName))
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

    // 学名字符构成：学名应该仅由拉丁字母、点（.）和空格（ ）组成，种以上（不含）应该仅由拉丁字母组成，种以下（含）还可包含点和空格。
    public class NameCharacterValidator : IValidator
    {
        private NameCharacterValidator() { }

        public static readonly NameCharacterValidator Instance = new NameCharacterValidator();

        public override string ToString() => "学名包含意外的字符";

        public bool IsValid(Taxon taxon)
        {
            if (string.IsNullOrEmpty(taxon.ScientificName))
            {
                return true;
            }

            return !taxon.ScientificName.Any((c) => !char.IsLetter(c) && c != '.' && c != ' ');
        }
    }

    // 学名分隔符：学名如果包含点（.），那么点只应该出现在每个单词结尾，并且不应该出现在最后一个单词结尾，点后面必须有空格，点和空格不应该连续出现多个。
    public class NameSeparatorValidator : IValidator
    {
        private NameSeparatorValidator() { }

        public static readonly NameSeparatorValidator Instance = new NameSeparatorValidator();

        public override string ToString() => "学名的单词分隔不正确";

        public bool IsValid(Taxon taxon)
        {
            if (string.IsNullOrEmpty(taxon.ScientificName))
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
        private TimelineCompletenessValidator() { }

        public static readonly TimelineCompletenessValidator Instance = new TimelineCompletenessValidator();

        public override string ToString() => "仅设置了诞生年代或灭绝年代之一";

        public string ToString(string message) => $"仅设置了{message}";

        public bool IsValid(Taxon taxon) => !taxon.IsExtinct || taxon.Birth.IsEmpty == taxon.Extinction.IsEmpty;
    }

    // 时间轴自洽：对于灭绝物种，灭绝年代应该晚于诞生年代。
    public class TimelineConsistencyValidator : IValidator
    {
        private TimelineConsistencyValidator() { }

        public static readonly TimelineConsistencyValidator Instance = new TimelineConsistencyValidator();

        public override string ToString() => "灭绝年代早于诞生年代";

        public bool IsValid(Taxon taxon) => !taxon.IsExtinct || taxon.Birth.IsEmpty || taxon.Extinction.IsEmpty || taxon.Birth <= taxon.Extinction;
    }

    // 演化顺序自洽：任何物种的诞生年代不应该早于其上级类群（祖先）的诞生年代。
    public class EvolutionOrderValidator : IValidator
    {
        private EvolutionOrderValidator() { }

        public static readonly EvolutionOrderValidator Instance = new EvolutionOrderValidator();

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

    // 未设置分级：具名类群应该设置分级。
    public class RankMissingValidator : IValidator
    {
        private RankMissingValidator() { }

        public static readonly RankMissingValidator Instance = new RankMissingValidator();

        public override string ToString() => "未设置分级";

        public bool IsValid(Taxon taxon) => taxon.IsAnonymous || !taxon.Rank.IsUnranked();
    }

    // 中文名后缀：中文名后缀应该与分级相符（种通常不需要后缀）。
    public class ChineseSuffixValidator : IValidator
    {
        private ChineseSuffixValidator() { }

        public static readonly ChineseSuffixValidator Instance = new ChineseSuffixValidator();

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
                if (split.tailPart == "类")
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

    // 异名无重复：异名不应该存在重复项。
    public class SynonymsUniqueValidator : IValidator
    {
        private SynonymsUniqueValidator() { }

        public static readonly SynonymsUniqueValidator Instance = new SynonymsUniqueValidator();

        public override string ToString() => "异名包含重复项";

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

            return true;
        }
    }

    // 标签无重复：标签不应该存在重复项。
    public class TagsUniqueValidator : IValidator
    {
        private TagsUniqueValidator() { }

        public static readonly TagsUniqueValidator Instance = new TagsUniqueValidator();

        public override string ToString() => "标签包含重复项";

        public bool IsValid(Taxon taxon)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

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

    // 异名与标签无重复：异名与标签之间不应该存在重复项。
    public class SynonymsTagUniqueValidator : IValidator
    {
        private SynonymsTagUniqueValidator() { }

        public static readonly SynonymsTagUniqueValidator Instance = new SynonymsTagUniqueValidator();

        public override string ToString() => "异名与标签之间包含重复项";

        public bool IsValid(Taxon taxon)
        {
            Dictionary<string, object> dict1 = new Dictionary<string, object>();
            Dictionary<string, object> dict2 = new Dictionary<string, object>();

            foreach (string synonym in taxon.Synonyms)
            {
                dict1.TryAdd(synonym, null);
            }

            foreach (string tag in taxon.Tags)
            {
                dict2.TryAdd(tag, null);
            }

            foreach (var pair in dict1)
            {
                if (!dict2.TryAdd(pair.Key, null))
                {
                    return false;
                }
            }

            return true;
        }
    }

    // 节点结构完整：匿名类群应该有姊妹群，并且应该有至少2个下级类群（后代）。
    public class NodeStructureValidator : IValidator
    {
        private NodeStructureValidator() { }

        public static readonly NodeStructureValidator Instance = new NodeStructureValidator();

        public override string ToString() => "节点缺少姊妹群或下级类群";

        public string ToString(string message) => $"节点缺少{message}";

        public bool IsValid(Taxon taxon) => taxon.IsNamed || taxon.IsRoot || (taxon.Parent.Children.Count > 1 && taxon.Children.Count > 1);
    }

    // 节点信息冗余：匿名类群不需要设置状态、年代、异名、标签、描述等信息。
    public class NodeInformationValidator : IValidator
    {
        private NodeInformationValidator() { }

        public static readonly NodeInformationValidator Instance = new NodeInformationValidator();

        public override string ToString() => "节点包含不需要设置的信息";

        public string ToString(string message) => $"节点不需要设置{message}";

        public bool IsValid(Taxon taxon) => taxon.IsNamed || (!taxon.IsExtinct && !taxon.IsUndet && taxon.Birth.IsEmpty && taxon.Extinction.IsEmpty && taxon.Synonyms.Count <= 0 && taxon.Tags.Count <= 0 && string.IsNullOrWhiteSpace(taxon.Description));
    }

    // 节点单系：匿名类群不应该是并系群或复系群。
    public class NodeMonophylyValidator : IValidator
    {
        private NodeMonophylyValidator() { }

        public static readonly NodeMonophylyValidator Instance = new NodeMonophylyValidator();

        public override string ToString() => "节点不应该是并系群或复系群";

        public string ToString(string message) => $"节点不应该是{message}";

        public bool IsValid(Taxon taxon) => taxon.IsNamed || taxon.IsMonophyly;
    }

    // 单系群：科学分类法要求所有类群都应该是单系的。
    public class MonophylyValidator : IValidator
    {
        private MonophylyValidator() { }

        public static readonly MonophylyValidator Instance = new MonophylyValidator();

        public override string ToString() => "不建议使用并系群或复系群";

        public string ToString(string message) => $"不建议使用{message}";

        public bool IsValid(Taxon taxon) => taxon.IsAnonymous || taxon.IsMonophyly;
    }
}