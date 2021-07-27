﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

namespace TreeOfLife.Views
{
    public static class Common
    {
        public static Taxon CurrentTaxon { get; set; } = null; // 当前选择的类群。

        public static Action<Taxon> SetCurrentTaxon { get; set; } = null;

        //

        public static bool? EditMode { get; set; } = null; // 是否为编辑模式。

        public static Action EnterEditMode { get; set; } = null;
        public static Action ExitEditMode { get; set; } = null;

        //

        public static Taxon RightButtonTaxon { get; set; } = null;
        public static Taxon SelectedTaxon { get; set; } = null;

        public static Action UpdateCurrentTaxonInfo { get; set; } = null;
        public static Action ApplyToTaxon { get; set; } = null;

        public static Action UpdateTree { get; set; } = null;
    }
}