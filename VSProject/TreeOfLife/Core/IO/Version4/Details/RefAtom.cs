﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using System.Text.Json.Serialization;

using TreeOfLife.Core.Taxonomy;

namespace TreeOfLife.Core.IO.Version4.Details
{
    // 系统发生树展开的表示引用关系的原子数据结构，表示一个并系群或复系群对其他类群的排除或包含关系。
    public sealed class RefAtom
    {
        private string _ID;
        private List<string> _Excludes = new List<string>();
        private List<string> _Includes = new List<string>();

        //

        public RefAtom()
        {
        }

        //

        [JsonPropertyName("ID")]
        public string ID
        {
            get => _ID;
            set => _ID = value;
        }

        [JsonPropertyName("Exclude")]
        public List<string> Excludes
        {
            get => _Excludes;
            set => _Excludes = value;
        }

        [JsonPropertyName("Include")]
        public List<string> Includes
        {
            get => _Includes;
            set => _Includes = value;
        }

        //

        public static RefAtom FromTaxon(Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            RefAtom atom = new RefAtom() { ID = Common.GetIdStringOfTaxon(taxon) };

            foreach (var exclude in taxon.Excludes)
            {
                atom.Excludes.Add(Common.GetIdStringOfTaxon(exclude));
            }

            foreach (var include in taxon.Includes)
            {
                atom.Includes.Add(Common.GetIdStringOfTaxon(include));
            }

            return atom;
        }
    }
}