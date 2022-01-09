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

using System.Drawing;

using TreeOfLife.Core.Geology;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Extensions
{
    // 地质年代的UI相关扩展方法。
    public static class GeoChronUIExtension
    {
        private static class _Colors
        {
            public static readonly ColorX Empty = Color.Black;

            public static readonly ColorX Hadean = ColorX.FromHSL(260, 40, 50);
            public static readonly ColorX Archean = ColorX.FromHSL(295, 40, 50);
            public static readonly ColorX Proterozoic = ColorX.FromHSL(352.5, 40, 50);
            public static readonly ColorX Phanerozoic = ColorX.FromHSL(190, 40, 50);

            // 太古宙
            public static readonly ColorX Eoarchean = ColorX.FromHSL(280, 40, 50);
            public static readonly ColorX Paleoarchean = ColorX.FromHSL(290, 40, 50);
            public static readonly ColorX Mesoarchean = ColorX.FromHSL(300, 40, 50);
            public static readonly ColorX Neoarchean = ColorX.FromHSL(310, 40, 50);

            // 元古宙
            public static readonly ColorX Paleoproterozoic = ColorX.FromHSL(332.5, 40, 50);
            public static readonly ColorX Mesoproterozoic = ColorX.FromHSL(355, 40, 50);
            public static readonly ColorX Neoproterozoic = ColorX.FromHSL(15, 40, 50);

            // 显生宙
            public static readonly ColorX Paleozoic = ColorX.FromHSL(120, 40, 50);
            public static readonly ColorX Mesozoic = ColorX.FromHSL(210, 40, 50);
            public static readonly ColorX Cenozoic = ColorX.FromHSL(50, 40, 50);

            // 元古宙：

            // 古元古代
            public static readonly ColorX Siderian = ColorX.FromHSL(325, 40, 50);
            public static readonly ColorX Rhyacian = ColorX.FromHSL(330, 40, 50);
            public static readonly ColorX Orosirian = ColorX.FromHSL(335, 40, 50);
            public static readonly ColorX Statherian = ColorX.FromHSL(340, 40, 50);

            // 中元古代
            public static readonly ColorX Calymmian = ColorX.FromHSL(350, 40, 50);
            public static readonly ColorX Ectasian = ColorX.FromHSL(355, 40, 50);
            public static readonly ColorX Stenian = ColorX.FromHSL(0, 40, 50);

            // 新元古代
            public static readonly ColorX Tonian = ColorX.FromHSL(10, 40, 50);
            public static readonly ColorX Cryogenian = ColorX.FromHSL(15, 40, 50);
            public static readonly ColorX Ediacaran = ColorX.FromHSL(20, 40, 50);

            // 显生宙：

            // 古生代
            public static readonly ColorX Cambrian = ColorX.FromHSL(110, 30, 50);
            public static readonly ColorX Ordovician = ColorX.FromHSL(170, 60, 50);
            public static readonly ColorX Silurian = ColorX.FromHSL(140, 30, 50);
            public static readonly ColorX Devonian = ColorX.FromHSL(35, 30, 50);
            public static readonly ColorX Carboniferous = ColorX.FromHSL(180, 40, 50);
            public static readonly ColorX Permian = ColorX.FromHSL(5, 60, 50);

            // 中生代
            public static readonly ColorX Triassic = ColorX.FromHSL(260, 50, 50);
            public static readonly ColorX Jurassic = ColorX.FromHSL(190, 70, 50);
            public static readonly ColorX Cretaceous = ColorX.FromHSL(90, 50, 50);

            // 新生代
            public static readonly ColorX Paleogene = ColorX.FromHSL(20, 60, 50);
            public static readonly ColorX Neogene = ColorX.FromHSL(45, 70, 50);
            public static readonly ColorX Quaternary = ColorX.FromHSL(55, 80, 50);

            // 古生代：

            // 寒武纪
            public static readonly ColorX Terreneuvian = ColorX.FromHSL(110, 45, 50);
            public static readonly ColorX CambrianSeries2 = ColorX.FromHSL(110, 35, 50);
            public static readonly ColorX Miaolingian = ColorX.FromHSL(110, 25, 50);
            public static readonly ColorX Furongian = ColorX.FromHSL(110, 15, 50);

            // 奥陶纪
            public static readonly ColorX LowerOrdovician = ColorX.FromHSL(170, 70, 50);
            public static readonly ColorX MiddleOrdovician = ColorX.FromHSL(170, 60, 50);
            public static readonly ColorX UpperOrdovician = ColorX.FromHSL(170, 50, 50);

            // 志留纪
            public static readonly ColorX Llandovery = ColorX.FromHSL(140, 45, 50);
            public static readonly ColorX Wenlock = ColorX.FromHSL(140, 35, 50);
            public static readonly ColorX Ludlow = ColorX.FromHSL(140, 25, 50);
            public static readonly ColorX Pridoli = ColorX.FromHSL(140, 15, 50);

            // 泥盆纪
            public static readonly ColorX LowerDevonian = ColorX.FromHSL(35, 40, 50);
            public static readonly ColorX MiddleDevonian = ColorX.FromHSL(35, 30, 50);
            public static readonly ColorX UpperDevonian = ColorX.FromHSL(35, 20, 50);

            // 石炭纪
            public static readonly ColorX Mississippian = ColorX.FromHSL(160, 40, 50);
            public static readonly ColorX Pennsylvanian = ColorX.FromHSL(180, 40, 50);

            // 二叠纪
            public static readonly ColorX Cisuralian = ColorX.FromHSL(5, 70, 50);
            public static readonly ColorX Guadalupian = ColorX.FromHSL(5, 60, 50);
            public static readonly ColorX Lopingian = ColorX.FromHSL(5, 50, 50);

            // 中生代：

            // 三叠纪
            public static readonly ColorX LowerTriassic = ColorX.FromHSL(260, 60, 50);
            public static readonly ColorX MiddleTriassic = ColorX.FromHSL(260, 50, 50);
            public static readonly ColorX UpperTriassic = ColorX.FromHSL(260, 40, 50);

            // 侏罗纪
            public static readonly ColorX LowerJurassic = ColorX.FromHSL(190, 80, 50);
            public static readonly ColorX MiddleJurassic = ColorX.FromHSL(190, 70, 50);
            public static readonly ColorX UpperJurassic = ColorX.FromHSL(190, 60, 50);

            // 白垩纪
            public static readonly ColorX LowerCretaceous = ColorX.FromHSL(90, 55, 50);
            public static readonly ColorX UpperCretaceous = ColorX.FromHSL(90, 45, 50);

            // 新生代：

            // 古近纪
            public static readonly ColorX Paleocene = ColorX.FromHSL(20, 70, 50);
            public static readonly ColorX Eocene = ColorX.FromHSL(20, 60, 50);
            public static readonly ColorX Oligocene = ColorX.FromHSL(20, 50, 50);

            // 新近纪
            public static readonly ColorX Miocene = ColorX.FromHSL(45, 75, 50);
            public static readonly ColorX Pliocene = ColorX.FromHSL(45, 65, 50);

            // 第四纪
            public static readonly ColorX Pleistocene = ColorX.FromHSL(55, 85, 50);
            public static readonly ColorX Holocene = ColorX.FromHSL(55, 75, 50);
            // public static readonly ColorX Anthropocene = Color.White;

            public static readonly ColorX Present = Color.Black;
        }

        private static readonly Dictionary<int, ColorX> _EnumValueToColorTable = new Dictionary<int, ColorX>()
        {
            { (int)GeoChronType.Empty, _Colors.Empty },

            { (int)Eon.Hadean, _Colors.Hadean },
            { (int)Eon.Archean, _Colors.Archean },
            { (int)Eon.Proterozoic, _Colors.Proterozoic },
            { (int)Eon.Phanerozoic, _Colors.Phanerozoic },

            // 太古宙
            { (int)Era.Eoarchean, _Colors.Eoarchean },
            { (int)Era.Paleoarchean, _Colors.Paleoarchean },
            { (int)Era.Mesoarchean, _Colors.Mesoarchean },
            { (int)Era.Neoarchean, _Colors.Neoarchean },

            // 元古宙
            { (int)Era.Paleoproterozoic, _Colors.Paleoproterozoic },
            { (int)Era.Mesoproterozoic, _Colors.Mesoproterozoic },
            { (int)Era.Neoproterozoic, _Colors.Neoproterozoic },

            // 显生宙
            { (int)Era.Paleozoic, _Colors.Paleozoic },
            { (int)Era.Mesozoic, _Colors.Mesozoic },
            { (int)Era.Cenozoic, _Colors.Cenozoic },

            // 元古宙：

            // 古元古代
            { (int)Period.Siderian, _Colors.Siderian },
            { (int)Period.Rhyacian, _Colors.Rhyacian },
            { (int)Period.Orosirian, _Colors.Orosirian },
            { (int)Period.Statherian, _Colors.Statherian },

            // 中元古代
            { (int)Period.Calymmian, _Colors.Calymmian },
            { (int)Period.Ectasian, _Colors.Ectasian },
            { (int)Period.Stenian, _Colors.Stenian },

            // 新元古代
            { (int)Period.Tonian, _Colors.Tonian },
            { (int)Period.Cryogenian, _Colors.Cryogenian },
            { (int)Period.Ediacaran, _Colors.Ediacaran },

            // 显生宙：

            // 古生代
            { (int)Period.Cambrian, _Colors.Cambrian },
            { (int)Period.Ordovician, _Colors.Ordovician },
            { (int)Period.Silurian, _Colors.Silurian },
            { (int)Period.Devonian, _Colors.Devonian },
            { (int)Period.Carboniferous, _Colors.Carboniferous },
            { (int)Period.Permian, _Colors.Permian },

            // 中生代
            { (int)Period.Triassic, _Colors.Triassic },
            { (int)Period.Jurassic, _Colors.Jurassic },
            { (int)Period.Cretaceous, _Colors.Cretaceous },

            // 新生代
            { (int)Period.Paleogene, _Colors.Paleogene },
            { (int)Period.Neogene, _Colors.Neogene },
            { (int)Period.Quaternary, _Colors.Quaternary },

            // 古生代：

            // 寒武纪
            { (int)Epoch.Terreneuvian, _Colors.Terreneuvian },
            { (int)Epoch.CambrianSeries2, _Colors.CambrianSeries2 },
            { (int)Epoch.Miaolingian, _Colors.Miaolingian },
            { (int)Epoch.Furongian, _Colors.Furongian },

            // 奥陶纪
            { (int)Epoch.LowerOrdovician, _Colors.LowerOrdovician },
            { (int)Epoch.MiddleOrdovician, _Colors.MiddleOrdovician },
            { (int)Epoch.UpperOrdovician, _Colors.UpperOrdovician },

            // 志留纪
            { (int)Epoch.Llandovery, _Colors.Llandovery },
            { (int)Epoch.Wenlock, _Colors.Wenlock },
            { (int)Epoch.Ludlow, _Colors.Ludlow },
            { (int)Epoch.Pridoli, _Colors.Pridoli },

            // 泥盆纪
            { (int)Epoch.LowerDevonian, _Colors.LowerDevonian },
            { (int)Epoch.MiddleDevonian, _Colors.MiddleDevonian },
            { (int)Epoch.UpperDevonian, _Colors.UpperDevonian },

            // 石炭纪
            { (int)Epoch.Mississippian, _Colors.Mississippian },
            { (int)Epoch.Pennsylvanian, _Colors.Pennsylvanian },

            // 二叠纪
            { (int)Epoch.Cisuralian, _Colors.Cisuralian },
            { (int)Epoch.Guadalupian, _Colors.Guadalupian },
            { (int)Epoch.Lopingian, _Colors.Lopingian },

            // 中生代：

            // 三叠纪
            { (int)Epoch.LowerTriassic, _Colors.LowerTriassic },
            { (int)Epoch.MiddleTriassic, _Colors.MiddleTriassic },
            { (int)Epoch.UpperTriassic, _Colors.UpperTriassic },

            // 侏罗纪
            { (int)Epoch.LowerJurassic, _Colors.LowerJurassic },
            { (int)Epoch.MiddleJurassic, _Colors.MiddleJurassic },
            { (int)Epoch.UpperJurassic, _Colors.UpperJurassic },

            // 白垩纪
            { (int)Epoch.LowerCretaceous, _Colors.LowerCretaceous },
            { (int)Epoch.UpperCretaceous, _Colors.UpperCretaceous },

            // 新生代：

            // 古近纪
            { (int)Epoch.Paleocene, _Colors.Paleocene },
            { (int)Epoch.Eocene, _Colors.Eocene },
            { (int)Epoch.Oligocene, _Colors.Oligocene },

            // 新近纪
            { (int)Epoch.Miocene, _Colors.Miocene },
            { (int)Epoch.Pliocene, _Colors.Pliocene },

            // 第四纪
            { (int)Epoch.Pleistocene, _Colors.Pleistocene },
            { (int)Epoch.Holocene, _Colors.Holocene },
            // { (int)Epoch.Anthropocene, _Names.Anthropocene },

            { (int)GeoChronType.Present, _Colors.Present }
        };

        // 获取分类阶元的主题颜色。
        public static ColorX GetThemeColor(this GeoChron geoChron)
        {
            if (geoChron is null)
            {
                throw new ArgumentNullException();
            }

            //
 
            if (geoChron.Type == GeoChronType.Age)
            {
                geoChron = geoChron.Superior;
            }

            if (_EnumValueToColorTable.TryGetValue(geoChron.GetHashCode(), out ColorX color))
            {
                return color;
            }
            else
            {
                return Color.Black;
            }
        }
    }
}