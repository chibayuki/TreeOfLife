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

namespace TreeOfLife.Core.Taxonomy.Extensions
{
    // 生物分类阶元的扩展方法。
    public static class RankExtension
    {
        public static Rank BasicRank(this Rank rank)
        {
            if (rank.IsUnranked()) return Rank.Unranked;

            else if (rank.IsClade()) return Rank.Clade;

            else if (rank.IsStrain()) return Rank.Strain;
            else if (rank.IsForm()) return Rank.Form;
            else if (rank.IsSeries()) return Rank.Series;
            else if (rank.IsDivision()) return Rank.Division;
            else if (rank.IsSection()) return Rank.Section;
            else if (rank.IsCohort()) return Rank.Cohort;
            else if (rank.IsTribe()) return Rank.Tribe;

            else if (rank.IsSpecies()) return Rank.Species;
            else if (rank.IsGenus()) return Rank.Genus;
            else if (rank.IsFamily()) return Rank.Family;
            else if (rank.IsOrder()) return Rank.Order;
            else if (rank.IsClass()) return Rank.Class;
            else if (rank.IsPhylum()) return Rank.Phylum;
            else if (rank.IsKingdom()) return Rank.Kingdom;
            else if (rank.IsDomain()) return Rank.Domain;

            else return Rank.Unranked;
        }

        //

        public static bool IsUnranked(this Rank rank) => rank == Rank.Unranked;

        public static bool IsClade(this Rank rank) => rank == Rank.Clade;

        public static bool IsSecondaryRank(this Rank rank) => rank >= Rank.Strain && rank <= Rank.Tribe;

        public static bool IsBasicSecondaryRank(this Rank rank) => rank is Rank.Strain or Rank.Form or Rank.Series or Rank.Division or Rank.Section or Rank.Cohort or Rank.Tribe;

        public static bool IsStrain(this Rank rank) => rank == Rank.Strain;

        public static bool IsForm(this Rank rank) => rank >= Rank.Subform && rank <= Rank.Form;

        public static bool IsSeries(this Rank rank) => rank >= Rank.Subseries && rank <= Rank.Series;

        public static bool IsDivision(this Rank rank) => rank >= Rank.Subdivision && rank <= Rank.Division;

        public static bool IsSection(this Rank rank) => rank >= Rank.Subsection && rank <= Rank.Section;

        public static bool IsCohort(this Rank rank) => rank >= Rank.Infracohort && rank <= Rank.Megacohort;

        public static bool IsTribe(this Rank rank) => rank >= Rank.Subtribe && rank <= Rank.Tribe;

        public static bool IsPrimaryRank(this Rank rank) => rank >= Rank.Subvariety && rank <= Rank.Superdomain;

        public static bool IsBasicPrimaryRank(this Rank rank) => rank is Rank.Species or Rank.Genus or Rank.Family or Rank.Order or Rank.Class or Rank.Phylum or Rank.Kingdom or Rank.Domain;

        public static bool IsSpecies(this Rank rank) => rank >= Rank.Subvariety && rank <= Rank.Species;

        public static bool IsGenus(this Rank rank) => rank >= Rank.Subgenus && rank <= Rank.Genus;

        public static bool IsFamily(this Rank rank) => rank >= Rank.Subfamily && rank <= Rank.Superfamily;

        public static bool IsOrder(this Rank rank) => rank >= Rank.Parvorder && rank <= Rank.Megaorder;

        public static bool IsClass(this Rank rank) => rank >= Rank.Parvclass && rank <= Rank.Megaclass;

        public static bool IsPhylum(this Rank rank) => rank >= Rank.Parvphylum && rank <= Rank.Superphylum;

        public static bool IsKingdom(this Rank rank) => rank >= Rank.Infrakingdom && rank <= Rank.Superkingdom;

        public static bool IsDomain(this Rank rank) => rank >= Rank.Domain && rank <= Rank.Superdomain;

        public static bool IsPrimaryOrSecondaryRank(this Rank rank) => IsSecondaryRank(rank) || IsPrimaryRank(rank);

        public static bool IsBasicRank(this Rank rank) => IsBasicSecondaryRank(rank) || IsBasicPrimaryRank(rank);
    }
}