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
    // 生物分类阶元的扩展方法。
    public static class TaxonomicCategoryExtension
    {
        public static TaxonomicCategory BasicCategory(this TaxonomicCategory category)
        {
            if (category.IsUnranked()) return TaxonomicCategory.Unranked;

            else if (category.IsClade()) return TaxonomicCategory.Clade;

            else if (category.IsStrain()) return TaxonomicCategory.Strain;
            else if (category.IsForm()) return TaxonomicCategory.Form;
            else if (category.IsSeries()) return TaxonomicCategory.Series;
            else if (category.IsDivision()) return TaxonomicCategory.Division;
            else if (category.IsSection()) return TaxonomicCategory.Section;
            else if (category.IsCohort()) return TaxonomicCategory.Cohort;
            else if (category.IsTribe()) return TaxonomicCategory.Tribe;

            else if (category.IsSpecies()) return TaxonomicCategory.Species;
            else if (category.IsGenus()) return TaxonomicCategory.Genus;
            else if (category.IsFamily()) return TaxonomicCategory.Family;
            else if (category.IsOrder()) return TaxonomicCategory.Order;
            else if (category.IsClass()) return TaxonomicCategory.Class;
            else if (category.IsPhylum()) return TaxonomicCategory.Phylum;
            else if (category.IsKingdom()) return TaxonomicCategory.Kingdom;
            else if (category.IsDomain()) return TaxonomicCategory.Domain;

            else return TaxonomicCategory.Unranked;
        }

        //

        public static bool IsUnranked(this TaxonomicCategory category)
        {
            return (category == TaxonomicCategory.Unranked);
        }

        public static bool IsClade(this TaxonomicCategory category)
        {
            return (category == TaxonomicCategory.Clade);
        }

        public static bool IsSecondaryCategory(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Strain && category <= TaxonomicCategory.Supertribe);
        }

        public static bool IsBasicSecondaryCategory(this TaxonomicCategory category)
        {
            return (category == TaxonomicCategory.Strain || category == TaxonomicCategory.Form || category == TaxonomicCategory.Series || category == TaxonomicCategory.Division || category == TaxonomicCategory.Section || category == TaxonomicCategory.Cohort || category == TaxonomicCategory.Tribe);
        }

        public static bool IsStrain(this TaxonomicCategory category)
        {
            return (category == TaxonomicCategory.Strain);
        }

        public static bool IsForm(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Subform && category <= TaxonomicCategory.Form);
        }

        public static bool IsSeries(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Subseries && category <= TaxonomicCategory.Series);
        }

        public static bool IsDivision(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Infradivision && category <= TaxonomicCategory.Superdivision);
        }

        public static bool IsSection(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Infrasection && category <= TaxonomicCategory.Supersection);
        }

        public static bool IsCohort(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Infracohort && category <= TaxonomicCategory.Megacohort);
        }

        public static bool IsTribe(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Infratribe && category <= TaxonomicCategory.Supertribe);
        }

        public static bool IsPrimaryCategory(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Subvariety && category <= TaxonomicCategory.Superdomain);
        }

        public static bool IsBasicPrimaryCategory(this TaxonomicCategory category)
        {
            return (category == TaxonomicCategory.Species || category == TaxonomicCategory.Genus || category == TaxonomicCategory.Family || category == TaxonomicCategory.Order || category == TaxonomicCategory.Class || category == TaxonomicCategory.Phylum || category == TaxonomicCategory.Kingdom || category == TaxonomicCategory.Domain);
        }

        public static bool IsSpecies(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Subvariety && category <= TaxonomicCategory.Superspecies);
        }

        public static bool IsGenus(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Infragenus && category <= TaxonomicCategory.Genus);
        }

        public static bool IsFamily(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Infrafamily && category <= TaxonomicCategory.Gigafamily);
        }

        public static bool IsOrder(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Parvorder && category <= TaxonomicCategory.Gigaorder);
        }

        public static bool IsClass(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Parvclass && category <= TaxonomicCategory.Megaclass);
        }

        public static bool IsPhylum(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Parvphylum && category <= TaxonomicCategory.Superphylum);
        }

        public static bool IsKingdom(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Infrakingdom && category <= TaxonomicCategory.Superkingdom);
        }

        public static bool IsDomain(this TaxonomicCategory category)
        {
            return (category >= TaxonomicCategory.Domain && category <= TaxonomicCategory.Superdomain);
        }
    }
}