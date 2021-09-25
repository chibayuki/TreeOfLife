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

namespace TreeOfLife.Core.Geology.Extensions
{
    // 地质年代的中文相关扩展方法。
    public static class GeoChronChineseExtension
    {
        private static class _Names
        {
            public const string Empty = "未指定";

            public const string Hadean = "冥古宙";
            public const string Archean = "太古宙";
            public const string Proterozoic = "元古宙";
            public const string Phanerozoic = "显生宙";

            // 太古宙
            public const string Eoarchean = "始太古代";
            public const string Paleoarchean = "古太古代";
            public const string Mesoarchean = "中太古代";
            public const string Neoarchean = "新太古代";

            // 元古宙
            public const string Paleoproterozoic = "古元古代";
            public const string Mesoproterozoic = "中元古代";
            public const string Neoproterozoic = "新元古代";

            // 显生宙
            public const string Paleozoic = "古生代";
            public const string Mesozoic = "中生代";
            public const string Cenozoic = "新生代";

            // 元古宙：

            // 古元古代
            public const string Siderian = "成铁纪";
            public const string Rhyacian = "层侵纪";
            public const string Orosirian = "造山纪";
            public const string Statherian = "固结纪";

            // 中元古代
            public const string Calymmian = "盖层纪";
            public const string Ectasian = "延展纪";
            public const string Stenian = "狭带纪";

            // 新元古代
            public const string Tonian = "拉伸纪";
            public const string Cryogenian = "成冰纪";
            public const string Ediacaran = "埃迪卡拉纪";

            // 显生宙：

            // 古生代
            public const string Cambrian = "寒武纪";
            public const string Ordovician = "奥陶纪";
            public const string Silurian = "志留纪";
            public const string Devonian = "泥盆纪";
            public const string Carboniferous = "石炭纪";
            public const string Permian = "二叠纪";

            // 中生代
            public const string Triassic = "三叠纪";
            public const string Jurassic = "侏罗纪";
            public const string Cretaceous = "白垩纪";

            // 新生代
            public const string Paleogene = "古近纪";
            public const string Neogene = "新近纪";
            public const string Quaternary = "第四纪";

            // 古生代：

            // 寒武纪
            public const string Terreneuvian = "纽芬兰世";
            public const string CambrianSeries2 = "第二世";
            public const string Miaolingian = "苗岭世";
            public const string Furongian = "芙蓉世";

            // 奥陶纪
            public const string LowerOrdovician = "早奥陶世";
            public const string MiddleOrdovician = "中奥陶世";
            public const string UpperOrdovician = "晚奥陶世";

            // 志留纪
            public const string Llandovery = "兰多维列世";
            public const string Wenlock = "温洛克世";
            public const string Ludlow = "罗德洛世";
            public const string Pridoli = "普里道利世";

            // 泥盆纪
            public const string LowerDevonian = "早泥盆世";
            public const string MiddleDevonian = "中泥盆世";
            public const string UpperDevonian = "晚泥盆世";

            // 石炭纪
            public const string Mississippian = "密西西比世";
            public const string Pennsylvanian = "宾夕法尼亚世";

            // 二叠纪
            public const string Cisuralian = "乌拉尔世";
            public const string Guadalupian = "瓜德鲁普世";
            public const string Lopingian = "乐平世";

            // 中生代：

            // 三叠纪
            public const string LowerTriassic = "早三叠世";
            public const string MiddleTriassic = "中三叠世";
            public const string UpperTriassic = "晚三叠世";

            // 侏罗纪
            public const string LowerJurassic = "早侏罗世";
            public const string MiddleJurassic = "中侏罗世";
            public const string UpperJurassic = "晚侏罗世";

            // 白垩纪
            public const string LowerCretaceous = "早白垩世";
            public const string UpperCretaceous = "晚白垩世";

            // 新生代：

            // 古近纪
            public const string Paleocene = "古新世";
            public const string Eocene = "始新世";
            public const string Oligocene = "渐新世";

            // 新近纪
            public const string Miocene = "中新世";
            public const string Pliocene = "上新世";

            // 第四纪
            public const string Pleistocene = "更新世";
            public const string Holocene = "全新世";
            // public const string Anthropocene = "人类世";

            // 寒武纪：

            // 纽芬兰世
            public const string Fortunian = "幸运期";
            public const string CambrianStage2 = "第二期";

            // 寒武纪第二世
            public const string CambrianStage3 = "第三期";
            public const string CambrianStage4 = "第四期";

            // 苗岭世
            public const string Wuliuian = "乌溜期";
            public const string Drumian = "鼓山期";
            public const string Guzhangian = "古丈期";

            // 芙蓉世
            public const string Paibian = "排碧期";
            public const string Jiangshanian = "江山期";
            public const string CambrianStage10 = "第十期";

            // 奥陶纪：

            // 早奥陶世
            public const string Tremadocian = "特马豆克期";
            public const string Floian = "弗洛期";

            // 中奥陶世
            public const string Dapingian = "大坪期";
            public const string Darriwilian = "达瑞威尔期";

            // 晚奥陶世
            public const string Sandbian = "桑比期";
            public const string Katian = "凯迪期";
            public const string Hirnantian = "赫南特期";

            // 志留纪：

            // 兰多维列世
            public const string Rhuddanian = "鲁丹期";
            public const string Aeronian = "埃隆期";
            public const string Telychian = "特列奇期";

            // 温洛克世
            public const string Sheinwoodian = "申伍德期";
            public const string Homerian = "侯墨期";

            // 罗德洛世
            public const string Gorstian = "高斯特期";
            public const string Ludfordian = "卢德福特期";

            // 普里道利世
            // （无）

            // 泥盆纪：

            // 早泥盆世
            public const string Lochkovian = "洛赫考夫期";
            public const string Pragian = "布拉格期";
            public const string Emsian = "埃姆斯期";

            // 中泥盆世
            public const string Eifelian = "艾菲尔期";
            public const string Givetian = "吉维特期";

            // 晚泥盆世
            public const string Frasnian = "弗拉期";
            public const string Famennian = "法门期";

            // 石炭纪：

            // 密西西比世
            public const string Tournaisian = "杜内期";
            public const string Visean = "维宪期";
            public const string Serpukhovian = "谢尔普霍夫期";

            // 宾夕法尼亚世
            public const string Bashkirian = "巴什基尔期";
            public const string Moscovian = "莫斯科期";
            public const string Kasimovian = "卡西莫夫期";
            public const string Gzhelian = "格舍尔期";

            // 二叠纪：

            // 乌拉尔世
            public const string Asselian = "阿瑟尔期";
            public const string Sakmarian = "萨克马尔期";
            public const string Artinskian = "亚丁斯克期";
            public const string Kungurian = "空谷期";

            // 瓜德鲁普世
            public const string Roadian = "罗德期";
            public const string Wordian = "沃德期";
            public const string Capitanian = "卡匹敦期";

            // 乐平世
            public const string Wuchiapingian = "吴家坪期";
            public const string Changhsingian = "长兴期";

            // 三叠纪：

            // 早三叠世
            public const string Induan = "印度期";
            public const string Olenekian = "奥伦尼克期";

            // 中三叠世
            public const string Anisian = "安尼期";
            public const string Ladinian = "拉丁期";

            // 晚三叠世
            public const string Carnian = "卡尼期";
            public const string Norian = "诺利期";
            public const string Rhaetian = "瑞替期";

            // 侏罗纪：

            // 早侏罗世
            public const string Hettangian = "赫塘期";
            public const string Sinemurian = "辛涅缪尔期";
            public const string Pliensbachian = "普林斯巴期";
            public const string Toarcian = "托阿尔期";

            // 中侏罗世
            public const string Aalenian = "阿林期";
            public const string Bajocian = "巴柔期";
            public const string Bathonian = "巴通期";
            public const string Callovian = "卡洛夫期";

            // 晚侏罗世
            public const string Oxfordian = "牛津期";
            public const string Kimmeridgian = "钦莫利期";
            public const string Tithonian = "提塘期";

            // 白垩纪：

            // 早白垩世
            public const string Berriasian = "贝里阿斯期";
            public const string Valanginian = "瓦兰今期";
            public const string Hauterivian = "欧特里夫期";
            public const string Barremian = "巴雷姆期";
            public const string Aptian = "阿普特期";
            public const string Albian = "阿尔布期";

            // 晚白垩世
            public const string Cenomanian = "赛诺曼期";
            public const string Turonian = "土仑期";
            public const string Coniacian = "康尼亚克期";
            public const string Santonian = "圣通期";
            public const string Campanian = "坎潘期";
            public const string Maastrichtian = "马斯特里赫特期";

            // 古近纪：

            // 古新世
            public const string Danian = "丹麦期";
            public const string Selandian = "塞兰特期";
            public const string Thanetian = "坦尼特期";

            // 始新世
            public const string Ypresian = "伊普里斯期";
            public const string Lutetian = "卢泰特期";
            public const string Bartonian = "巴顿期";
            public const string Priabonian = "普利亚本期";

            // 渐新世
            public const string Rupelian = "吕珀尔期";
            public const string Chattian = "夏特期";

            // 新近纪：

            // 中新世
            public const string Aquitanian = "阿基坦期";
            public const string Burdigalian = "波尔多期";
            public const string Langhian = "兰盖期";
            public const string Serravallian = "赛拉瓦莱期";
            public const string Tortonian = "托尔托纳期";
            public const string Messinian = "墨西拿期";

            // 上新世
            public const string Zanclean = "赞克勒期";
            public const string Piacenzian = "皮亚琴察期";

            // 第四纪：

            // 更新世
            public const string Gelasian = "杰拉期";
            public const string Calabrian = "卡拉布里期";
            public const string Chibanian = "千叶期";
            public const string UpperPleistocene = "晚更新期";

            // 全新世
            public const string Greenlandian = "格陵兰期";
            public const string Northgrippian = "诺斯格瑞比期";
            public const string Meghalayan = "梅加拉亚期";

            // 人类世
            // （无）

            public const string Present = "至今";
        }

        private static readonly Dictionary<int, string> _EnumValueToNameTable = new Dictionary<int, string>()
        {
            { (int)GeoChronType.Empty, _Names.Empty },

            { (int)Eon.Hadean, _Names.Hadean },
            { (int)Eon.Archean, _Names.Archean },
            { (int)Eon.Proterozoic, _Names.Proterozoic },
            { (int)Eon.Phanerozoic, _Names.Phanerozoic },

            // 太古宙
            { (int)Era.Eoarchean, _Names.Eoarchean },
            { (int)Era.Paleoarchean, _Names.Paleoarchean },
            { (int)Era.Mesoarchean, _Names.Mesoarchean },
            { (int)Era.Neoarchean, _Names.Neoarchean },

            // 元古宙
            { (int)Era.Paleoproterozoic, _Names.Paleoproterozoic },
            { (int)Era.Mesoproterozoic, _Names.Mesoproterozoic },
            { (int)Era.Neoproterozoic, _Names.Neoproterozoic },

            // 显生宙
            { (int)Era.Paleozoic, _Names.Paleozoic },
            { (int)Era.Mesozoic, _Names.Mesozoic },
            { (int)Era.Cenozoic, _Names.Cenozoic },

            // 元古宙：

            // 古元古代
            { (int)Period.Siderian, _Names.Siderian },
            { (int)Period.Rhyacian, _Names.Rhyacian },
            { (int)Period.Orosirian, _Names.Orosirian },
            { (int)Period.Statherian, _Names.Statherian },

            // 中元古代
            { (int)Period.Calymmian, _Names.Calymmian },
            { (int)Period.Ectasian, _Names.Ectasian },
            { (int)Period.Stenian, _Names.Stenian },

            // 新元古代
            { (int)Period.Tonian, _Names.Tonian },
            { (int)Period.Cryogenian, _Names.Cryogenian },
            { (int)Period.Ediacaran, _Names.Ediacaran },

            // 显生宙：

            // 古生代
            { (int)Period.Cambrian, _Names.Cambrian },
            { (int)Period.Ordovician, _Names.Ordovician },
            { (int)Period.Silurian, _Names.Silurian },
            { (int)Period.Devonian, _Names.Devonian },
            { (int)Period.Carboniferous, _Names.Carboniferous },
            { (int)Period.Permian, _Names.Permian },

            // 中生代
            { (int)Period.Triassic, _Names.Triassic },
            { (int)Period.Jurassic, _Names.Jurassic },
            { (int)Period.Cretaceous, _Names.Cretaceous },

            // 新生代
            { (int)Period.Paleogene, _Names.Paleogene },
            { (int)Period.Neogene, _Names.Neogene },
            { (int)Period.Quaternary, _Names.Quaternary },

            // 古生代：

            // 寒武纪
            { (int)Epoch.Terreneuvian, _Names.Terreneuvian },
            { (int)Epoch.CambrianSeries2, _Names.CambrianSeries2 },
            { (int)Epoch.Miaolingian, _Names.Miaolingian },
            { (int)Epoch.Furongian, _Names.Furongian },

            // 奥陶纪
            { (int)Epoch.LowerOrdovician, _Names.LowerOrdovician },
            { (int)Epoch.MiddleOrdovician, _Names.MiddleOrdovician },
            { (int)Epoch.UpperOrdovician, _Names.UpperOrdovician },

            // 志留纪
            { (int)Epoch.Llandovery, _Names.Llandovery },
            { (int)Epoch.Wenlock, _Names.Wenlock },
            { (int)Epoch.Ludlow, _Names.Ludlow },
            { (int)Epoch.Pridoli, _Names.Pridoli },

            // 泥盆纪
            { (int)Epoch.LowerDevonian, _Names.LowerDevonian },
            { (int)Epoch.MiddleDevonian, _Names.MiddleDevonian },
            { (int)Epoch.UpperDevonian, _Names.UpperDevonian },

            // 石炭纪
            { (int)Epoch.Mississippian, _Names.Mississippian },
            { (int)Epoch.Pennsylvanian, _Names.Pennsylvanian },

            // 二叠纪
            { (int)Epoch.Cisuralian, _Names.Cisuralian },
            { (int)Epoch.Guadalupian, _Names.Guadalupian },
            { (int)Epoch.Lopingian, _Names.Lopingian },

            // 中生代：

            // 三叠纪
            { (int)Epoch.LowerTriassic, _Names.LowerTriassic },
            { (int)Epoch.MiddleTriassic, _Names.MiddleTriassic },
            { (int)Epoch.UpperTriassic, _Names.UpperTriassic },

            // 侏罗纪
            { (int)Epoch.LowerJurassic, _Names.LowerJurassic },
            { (int)Epoch.MiddleJurassic, _Names.MiddleJurassic },
            { (int)Epoch.UpperJurassic, _Names.UpperJurassic },

            // 白垩纪
            { (int)Epoch.LowerCretaceous, _Names.LowerCretaceous },
            { (int)Epoch.UpperCretaceous, _Names.UpperCretaceous },

            // 新生代：

            // 古近纪
            { (int)Epoch.Paleocene, _Names.Paleocene },
            { (int)Epoch.Eocene, _Names.Eocene },
            { (int)Epoch.Oligocene, _Names.Oligocene },

            // 新近纪
            { (int)Epoch.Miocene, _Names.Miocene },
            { (int)Epoch.Pliocene, _Names.Pliocene },

            // 第四纪
            { (int)Epoch.Pleistocene, _Names.Pleistocene },
            { (int)Epoch.Holocene, _Names.Holocene },
            // { (int)Epoch.Anthropocene, _Names.Anthropocene },

            // 寒武纪：

            // 纽芬兰世
            { (int)Age.Fortunian, _Names.Fortunian },
            { (int)Age.CambrianStage2, _Names.CambrianStage2 },

            // 寒武纪第二世
            { (int)Age.CambrianStage3, _Names.CambrianStage3 },
            { (int)Age.CambrianStage4, _Names.CambrianStage4 },

            // 苗岭世
            { (int)Age.Wuliuian, _Names.Wuliuian },
            { (int)Age.Drumian, _Names.Drumian },
            { (int)Age.Guzhangian, _Names.Guzhangian },

            // 芙蓉世
            { (int)Age.Paibian, _Names.Paibian },
            { (int)Age.Jiangshanian, _Names.Jiangshanian },
            { (int)Age.CambrianStage10, _Names.CambrianStage10 },

            // 奥陶纪：

            // 早奥陶世
            { (int)Age.Tremadocian, _Names.Tremadocian },
            { (int)Age.Floian, _Names.Floian },

            // 中奥陶世
            { (int)Age.Dapingian, _Names.Dapingian },
            { (int)Age.Darriwilian, _Names.Darriwilian },

            // 晚奥陶世
            { (int)Age.Sandbian, _Names.Sandbian },
            { (int)Age.Katian, _Names.Katian },
            { (int)Age.Hirnantian, _Names.Hirnantian },

            // 志留纪：

            // 兰多维列世
            { (int)Age.Rhuddanian, _Names.Rhuddanian },
            { (int)Age.Aeronian, _Names.Aeronian },
            { (int)Age.Telychian, _Names.Telychian },

            // 温洛克世
            { (int)Age.Sheinwoodian, _Names.Sheinwoodian },
            { (int)Age.Homerian, _Names.Homerian },

            // 罗德洛世
            { (int)Age.Gorstian, _Names.Gorstian },
            { (int)Age.Ludfordian, _Names.Ludfordian },

            // 普里道利世
            // （无）

            // 泥盆纪：

            // 早泥盆世
            { (int)Age.Lochkovian, _Names.Lochkovian },
            { (int)Age.Pragian, _Names.Pragian },
            { (int)Age.Emsian, _Names.Emsian },

            // 中泥盆世
            { (int)Age.Eifelian, _Names.Eifelian },
            { (int)Age.Givetian, _Names.Givetian },

            // 晚泥盆世
            { (int)Age.Frasnian, _Names.Frasnian },
            { (int)Age.Famennian, _Names.Famennian },

            // 石炭纪：

            // 密西西比世
            { (int)Age.Tournaisian, _Names.Tournaisian },
            { (int)Age.Visean, _Names.Visean },
            { (int)Age.Serpukhovian, _Names.Serpukhovian },

            // 宾夕法尼亚世
            { (int)Age.Bashkirian, _Names.Bashkirian },
            { (int)Age.Moscovian, _Names.Moscovian },
            { (int)Age.Kasimovian, _Names.Kasimovian },
            { (int)Age.Gzhelian, _Names.Gzhelian },

            // 二叠纪：

            // 乌拉尔世
            { (int)Age.Asselian, _Names.Asselian },
            { (int)Age.Sakmarian, _Names.Sakmarian },
            { (int)Age.Artinskian, _Names.Artinskian },
            { (int)Age.Kungurian, _Names.Kungurian },

            // 瓜德鲁普世
            { (int)Age.Roadian, _Names.Roadian },
            { (int)Age.Wordian, _Names.Wordian },
            { (int)Age.Capitanian, _Names.Capitanian },

            // 乐平世
            { (int)Age.Wuchiapingian, _Names.Wuchiapingian },
            { (int)Age.Changhsingian, _Names.Changhsingian },

            // 三叠纪：

            // 早三叠世
            { (int)Age.Induan, _Names.Induan },
            { (int)Age.Olenekian, _Names.Olenekian },

            // 中三叠世
            { (int)Age.Anisian, _Names.Anisian },
            { (int)Age.Ladinian, _Names.Ladinian },

            // 晚三叠世
            { (int)Age.Carnian, _Names.Carnian },
            { (int)Age.Norian, _Names.Norian },
            { (int)Age.Rhaetian, _Names.Rhaetian },

            // 侏罗纪：

            // 早侏罗世
            { (int)Age.Hettangian, _Names.Hettangian },
            { (int)Age.Sinemurian, _Names.Sinemurian },
            { (int)Age.Pliensbachian, _Names.Pliensbachian },
            { (int)Age.Toarcian, _Names.Toarcian },

            // 中侏罗世
            { (int)Age.Aalenian, _Names.Aalenian },
            { (int)Age.Bajocian, _Names.Bajocian },
            { (int)Age.Bathonian, _Names.Bathonian },
            { (int)Age.Callovian, _Names.Callovian },

            // 晚侏罗世
            { (int)Age.Oxfordian, _Names.Oxfordian },
            { (int)Age.Kimmeridgian, _Names.Kimmeridgian },
            { (int)Age.Tithonian, _Names.Tithonian },

            // 白垩纪：

            // 早白垩世
            { (int)Age.Berriasian, _Names.Berriasian },
            { (int)Age.Valanginian, _Names.Valanginian },
            { (int)Age.Hauterivian, _Names.Hauterivian },
            { (int)Age.Barremian, _Names.Barremian },
            { (int)Age.Aptian, _Names.Aptian },
            { (int)Age.Albian, _Names.Albian },

            // 晚白垩世
            { (int)Age.Cenomanian, _Names.Cenomanian },
            { (int)Age.Turonian, _Names.Turonian },
            { (int)Age.Coniacian, _Names.Coniacian },
            { (int)Age.Santonian, _Names.Santonian },
            { (int)Age.Campanian, _Names.Campanian },
            { (int)Age.Maastrichtian, _Names.Maastrichtian },

            // 古近纪：

            // 古新世
            { (int)Age.Danian, _Names.Danian },
            { (int)Age.Selandian, _Names.Selandian },
            { (int)Age.Thanetian, _Names.Thanetian },

            // 始新世
            { (int)Age.Ypresian, _Names.Ypresian },
            { (int)Age.Lutetian, _Names.Lutetian },
            { (int)Age.Bartonian, _Names.Bartonian },
            { (int)Age.Priabonian, _Names.Priabonian },

            // 渐新世
            { (int)Age.Rupelian, _Names.Rupelian },
            { (int)Age.Chattian, _Names.Chattian },

            // 新近纪：

            // 中新世
            { (int)Age.Aquitanian, _Names.Aquitanian },
            { (int)Age.Burdigalian, _Names.Burdigalian },
            { (int)Age.Langhian, _Names.Langhian },
            { (int)Age.Serravallian, _Names.Serravallian },
            { (int)Age.Tortonian, _Names.Tortonian },
            { (int)Age.Messinian, _Names.Messinian },

            // 上新世
            { (int)Age.Zanclean, _Names.Zanclean },
            { (int)Age.Piacenzian, _Names.Piacenzian },

            // 第四纪：

            // 更新世
            { (int)Age.Gelasian, _Names.Gelasian },
            { (int)Age.Calabrian, _Names.Calabrian },
            { (int)Age.Chibanian, _Names.Chibanian },
            { (int)Age.UpperPleistocene, _Names.UpperPleistocene },

            // 全新世
            { (int)Age.Greenlandian, _Names.Greenlandian },
            { (int)Age.Northgrippian, _Names.Northgrippian },
            { (int)Age.Meghalayan, _Names.Meghalayan },

            // 人类世
            // （无）

            { (int)GeoChronType.Present, _Names.Present }
        };

        // 获取地质年代的中文名。
        public static string GetChineseName(this GeoChron geoChron)
        {
            if (geoChron.Type == GeoChronType.MaBP)
            {
                return $"{geoChron.MaBP} Ma";
            }
            else if (geoChron.Type == GeoChronType.CEYear)
            {
                return geoChron.CEYear < 0 ? $"BC {-geoChron.CEYear}" : $"AD {geoChron.CEYear}";
            }
            else
            {
                if (_EnumValueToNameTable.TryGetValue(geoChron.GetHashCode(), out string chineseName))
                {
                    return chineseName;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }
    }
}