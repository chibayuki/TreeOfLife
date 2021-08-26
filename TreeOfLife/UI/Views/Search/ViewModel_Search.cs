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

namespace TreeOfLife.UI.Views.Search
{
    public sealed class ViewModel_Search : ViewModel
    {
        public Action ClickedSearchResult { get; set; }

        //

        private string _KeyWord;

        public string KeyWord
        {
            get => _KeyWord;

            set
            {
                _KeyWord = value;

                NotifyPropertyChanged(nameof(KeyWord));
            }
        }
    }
}