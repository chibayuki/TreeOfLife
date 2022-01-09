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

namespace TreeOfLife.UI.Views
{
    public sealed class ViewModel_Evo_ViewMode : ViewModel
    {
        private string _Desc;

        public string Desc
        {
            get => _Desc;

            set
            {
                _Desc = value;

                NotifyPropertyChanged(nameof(Desc));
            }
        }

        public void LoadFromCurrentTaxon()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            Desc = currentTaxon.Description;
        }
    }
}