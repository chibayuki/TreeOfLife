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

using TreeOfLife.Taxonomy;

namespace TreeOfLife.Packaging.Version2.Details
{
    public static class Common
    {
        public static List<int> GetIndexListOfTaxon(Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            List<int> indexList = new List<int>();

            if (!taxon.IsRoot)
            {
                Taxon t = taxon;

                while (!t.IsRoot)
                {
                    indexList.Add(t.Index);

                    t = t.Parent;
                }

                if (indexList.Count > 1)
                {
                    indexList.Reverse();
                }
            }

            return indexList;
        }

        public static string GetIdStringOfTaxon(Taxon taxon)
        {
            return IndexListToIdString(GetIndexListOfTaxon(taxon));
        }

        public static string IndexListToIdString(List<int> parentIndexList, int index)
        {
            if (parentIndexList is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (parentIndexList.Count > 1)
            {
                StringBuilder id = new StringBuilder();

                foreach (var parentIndex in parentIndexList)
                {
                    id.Append(parentIndex);
                    id.Append('-');
                }

                id.Append(index);

                return id.ToString();
            }
            else if (parentIndexList.Count == 1)
            {
                return string.Concat(parentIndexList[0], '-', index);
            }
            else
            {
                return index.ToString();
            }
        }

        public static string IndexListToIdString(List<int> indexList)
        {
            if (indexList is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (indexList.Count > 2)
            {
                StringBuilder id = new StringBuilder();

                for (int i = 0; i < indexList.Count - 1; i++)
                {
                    id.Append(indexList[i]);
                    id.Append('-');
                }

                id.Append(indexList[^1]);

                return id.ToString();
            }
            else if (indexList.Count == 2)
            {
                return string.Concat(indexList[0], '-', indexList[1]);
            }
            else if (indexList.Count == 1)
            {
                return indexList[0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static void IdStringToIndexList(string id, out List<int> parentIndexList, out int index)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            //

            string[] ids = id.Split('-');

            if (ids.Length > 1)
            {
                parentIndexList = new List<int>(ids.Length - 1);

                for (int i = 0; i < ids.Length - 1; i++)
                {
                    parentIndexList.Add(int.Parse(ids[i]));
                }

                index = int.Parse(ids[^1]);
            }
            else if (ids.Length == 1)
            {
                parentIndexList = new List<int>();
                index = int.Parse(ids[0]);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public static void IdStringToIndexList(string id, out List<int> indexList)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            //

            string[] ids = id.Split('-');

            if (ids.Length > 1)
            {
                indexList = new List<int>(ids.Length);

                for (int i = 0; i < ids.Length; i++)
                {
                    indexList.Add(int.Parse(ids[i]));
                }
            }
            else if (ids.Length == 1)
            {
                indexList = new List<int>() { int.Parse(ids[0]) };
            }
            else
            {
                indexList = new List<int>();
            }
        }
    }
}