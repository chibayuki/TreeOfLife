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

using TreeOfLife.Core.Taxonomy;

namespace TreeOfLife.Core.Validation.Extensions
{
    // 生物分类单元（类群）的检查相关扩展方法。
    public static class TaxonValidateExtension
    {
        private static readonly IValidator[] _Validators = new IValidator[]
        {
#if DEBUG
            new LevelValidator(),
            new IndexValidator(),
            new InheritValidator(),
            new ExcludeValidator(),
            new IncludeValidator(),
            new NodeRankValidator(),
            new BirthValidator(),
            new ExtinctionValidator(),
#endif
            new NameLengthValidator(),
            new NameUppercaseValidator(),
            new NameWordCountValidator(),
            new NameCharacterValidator(),
            new NameSeparatorValidator(),
            new TimelineCompletenessValidator(),
            new TimelineConsistencyValidator(),
            new EvolutionOrderValidator(),
            new NameMissingValidator(),
            new RankMissingValidator(),
            new ChineseSuffixValidator(),
            new TagUniqueValidator(),
            new NodeStructureValidator()
        };

        // 检查当前类群的违规项。
        public static IReadOnlyList<IValidator> Validate(this Taxon taxon)
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

        private static void _ValidateChildren(List<(Taxon, IReadOnlyList<IValidator>)> result, Taxon taxon)
        {
            IReadOnlyList<IValidator> violations = taxon.Validate();

            if (violations.Any())
            {
                result.Add((taxon, violations));
            }

            foreach (var child in taxon.Children)
            {
                _ValidateChildren(result, child);
            }
        }

        // 检查当前类群的所有子类群的违规项。
        public static IReadOnlyList<(Taxon taxon, IReadOnlyList<IValidator> violations)> ValidateChildren(this Taxon taxon)
        {
            List<(Taxon, IReadOnlyList<IValidator>)> result = new List<(Taxon, IReadOnlyList<IValidator>)>();

            foreach (var child in taxon.Children)
            {
                _ValidateChildren(result, child);
            }

            return result;
        }

        // 检查当前类群的所有子类群的违规项（并按规则排序）。
        public static IReadOnlyDictionary<IValidator, IReadOnlyList<Taxon>> ValidateChildrenAndGroupByValidator(this Taxon taxon)
        {
            Dictionary<IValidator, IReadOnlyList<Taxon>> result = new Dictionary<IValidator, IReadOnlyList<Taxon>>();

            foreach ((Taxon t, IReadOnlyList<IValidator> violations) in ValidateChildren(taxon))
            {
                foreach (var violation in violations)
                {
                    if (!result.TryGetValue(violation, out IReadOnlyList<Taxon> list))
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