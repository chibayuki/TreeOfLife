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

namespace TreeOfLife.Core.Validation.Extensions
{
    // 生物分类单元（类群）的检查相关扩展方法。
    public static class TaxonValidateExtension
    {
        private static readonly IValidator[] _Validators = new IValidator[]
        {
#if DEBUG
            LevelValidator.Instance,
            IndexValidator.Instance,
            InheritValidator.Instance,
            ExcludeValidator.Instance,
            IncludeValidator.Instance,
            NodeRankValidator.Instance,
            BirthValidator.Instance,
            ExtinctionValidator.Instance,
#endif
            NameMissingValidator.Instance,
            NameLengthValidator.Instance,
            NameUppercaseValidator.Instance,
            NameWordCountValidator.Instance,
            NameCharacterValidator.Instance,
            NameSeparatorValidator.Instance,
            TimelineCompletenessValidator.Instance,
            TimelineConsistencyValidator.Instance,
            EvolutionOrderValidator.Instance,
            RankMissingValidator.Instance,
            ChineseSuffixValidator.Instance,
            SynonymsUniqueValidator.Instance,
            TagsUniqueValidator.Instance,
            SynonymsTagUniqueValidator.Instance,
            NodeStructureValidator.Instance,
            NodeInformationValidator.Instance,
            NodeMonophylyValidator.Instance,
            MonophylyValidator.Instance
        };

        // 检查指定类群的违规项。
        public static IReadOnlyCollection<IValidator> Validate(this Taxon taxon)
        {
            List<IValidator> result = new List<IValidator>();

            foreach (var validator in _Validators)
            {
                if (!validator.IsValid(taxon))
                {
                    result.Add(validator);
                }
            }

            return result;
        }

        // 递归检查所有子类群的违规项。
        private static void _ValidateChildren(Taxon taxon, List<(Taxon, IReadOnlyCollection<IValidator>)> result)
        {
            IReadOnlyCollection<IValidator> violations = taxon.Validate();

            if (violations.Any())
            {
                result.Add((taxon, violations));
            }

            foreach (var child in taxon.Children)
            {
                _ValidateChildren(child, result);
            }
        }

        // 检查所有子类群的违规项。
        public static IReadOnlyCollection<(Taxon taxon, IReadOnlyCollection<IValidator> violations)> ValidateChildren(this Taxon taxon)
        {
            List<(Taxon, IReadOnlyCollection<IValidator>)> result = new List<(Taxon, IReadOnlyCollection<IValidator>)>();

            foreach (var child in taxon.Children)
            {
                _ValidateChildren(child, result);
            }

            return result;
        }

        // 检查所有子类群的违规项（并按规则分组）。
        public static IReadOnlyDictionary<IValidator, IReadOnlyCollection<Taxon>> ValidateChildrenAndGroupByValidator(this Taxon taxon)
        {
            Dictionary<IValidator, IReadOnlyCollection<Taxon>> result = new Dictionary<IValidator, IReadOnlyCollection<Taxon>>();

            foreach ((Taxon t, IReadOnlyCollection<IValidator> violations) in ValidateChildren(taxon))
            {
                foreach (var violation in violations)
                {
                    if (!result.TryGetValue(violation, out IReadOnlyCollection<Taxon> list))
                    {
                        list = new List<Taxon>();

                        result.Add(violation, list);
                    }

                    (list as List<Taxon>).Add(t);
                }
            }

            return result;
        }
    }
}