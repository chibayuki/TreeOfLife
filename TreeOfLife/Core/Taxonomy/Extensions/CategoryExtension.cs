/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1240.1000.M12.210718-2000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife.Core.Taxonomy.Extensions
{
    // 生物分类阶元的扩展方法。
    public static class CategoryExtension
    {
        public static Category BasicCategory(this Category category)
        {
            if (category.IsUnranked()) return Category.Unranked;

            else if (category.IsClade()) return Category.Clade;

            else if (category.IsStrain()) return Category.Strain;
            else if (category.IsForm()) return Category.Form;
            else if (category.IsSeries()) return Category.Series;
            else if (category.IsDivision()) return Category.Division;
            else if (category.IsSection()) return Category.Section;
            else if (category.IsCohort()) return Category.Cohort;
            else if (category.IsTribe()) return Category.Tribe;

            else if (category.IsSpecies()) return Category.Species;
            else if (category.IsGenus()) return Category.Genus;
            else if (category.IsFamily()) return Category.Family;
            else if (category.IsOrder()) return Category.Order;
            else if (category.IsClass()) return Category.Class;
            else if (category.IsPhylum()) return Category.Phylum;
            else if (category.IsKingdom()) return Category.Kingdom;
            else if (category.IsDomain()) return Category.Domain;

            else return Category.Unranked;
        }

        //

        public static bool IsUnranked(this Category category) => category == Category.Unranked;

        public static bool IsClade(this Category category) => category == Category.Clade;

        public static bool IsSecondaryCategory(this Category category) => category >= Category.Strain && category <= Category.Supertribe;

        public static bool IsBasicSecondaryCategory(this Category category) => category is Category.Strain or Category.Form or Category.Series or Category.Division or Category.Section or Category.Cohort or Category.Tribe;

        public static bool IsStrain(this Category category) => category == Category.Strain;

        public static bool IsForm(this Category category) => category >= Category.Subform && category <= Category.Form;

        public static bool IsSeries(this Category category) => category >= Category.Subseries && category <= Category.Series;

        public static bool IsDivision(this Category category) => category >= Category.Infradivision && category <= Category.Superdivision;

        public static bool IsSection(this Category category) => category >= Category.Infrasection && category <= Category.Supersection;

        public static bool IsCohort(this Category category) => category >= Category.Infracohort && category <= Category.Megacohort;

        public static bool IsTribe(this Category category) => category >= Category.Infratribe && category <= Category.Supertribe;

        public static bool IsPrimaryCategory(this Category category) => category >= Category.Subvariety && category <= Category.Superdomain;

        public static bool IsBasicPrimaryCategory(this Category category) => category is Category.Species or Category.Genus or Category.Family or Category.Order or Category.Class or Category.Phylum or Category.Kingdom or Category.Domain;

        public static bool IsSpecies(this Category category) => category >= Category.Subvariety && category <= Category.Superspecies;

        public static bool IsGenus(this Category category) => category >= Category.Infragenus && category <= Category.Genus;

        public static bool IsFamily(this Category category) => category >= Category.Infrafamily && category <= Category.Gigafamily;

        public static bool IsOrder(this Category category) => category >= Category.Parvorder && category <= Category.Gigaorder;

        public static bool IsClass(this Category category) => category >= Category.Parvclass && category <= Category.Megaclass;

        public static bool IsPhylum(this Category category) => category >= Category.Parvphylum && category <= Category.Superphylum;

        public static bool IsKingdom(this Category category) => category >= Category.Infrakingdom && category <= Category.Superkingdom;

        public static bool IsDomain(this Category category) => category >= Category.Domain && category <= Category.Superdomain;

        public static bool IsPrimaryOrSecondaryCategory(this Category category) => IsSecondaryCategory(category) || IsPrimaryCategory(category);

        public static bool IsBasicCategory(this Category category) => IsBasicSecondaryCategory(category) || IsBasicPrimaryCategory(category);
    }
}