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

namespace TreeOfLife.Geology
{
    // 地质年代类型。所有枚举用于计算哈希码，需要确保唯一性，下同。
    public enum GeoChronType
    {
        Empty = 0, // 未指定时期

        Age = 0x00000001, // 期
        Epoch = 0x00000010, // 世
        Period = 0x00000100, // 纪
        Era = 0x00001000, // 代
        Eon = 0x00010000, // 宙

        MaBP = 0x00100000, // 距今百万年前（相对于考古学基准年代）
        CEYear = 0x00200000, // 公元纪年年份

        Present = 0x0F000000 // 现代（指当代，而不是考古学基准年代）
    }

    // 宙。
    public enum Eon
    {
        Hadean = GeoChronType.Eon, // 冥古宙
        Archean = 2 * GeoChronType.Eon, // 太古宙
        Proterozoic = 3 * GeoChronType.Eon, // 元古宙
        Phanerozoic = 4 * GeoChronType.Eon // 显生宙
    }

    // 代。
    public enum Era
    {
        // 太古宙
        Eoarchean = Eon.Archean + GeoChronType.Era, // 始太古代
        Paleoarchean = Eon.Archean + 2 * GeoChronType.Era, // 古太古代
        Mesoarchean = Eon.Archean + 3 * GeoChronType.Era, // 中太古代
        Neoarchean = Eon.Archean + 4 * GeoChronType.Era, // 新太古代

        // 元古宙
        Paleoproterozoic = Eon.Proterozoic + GeoChronType.Era, // 古元古代
        Mesoproterozoic = Eon.Proterozoic + 2 * GeoChronType.Era, // 中元古代
        Neoproterozoic = Eon.Proterozoic + 3 * GeoChronType.Era, // 新元古代

        // 显生宙
        Paleozoic = Eon.Phanerozoic + GeoChronType.Era, // 古生代
        Mesozoic = Eon.Phanerozoic + 2 * GeoChronType.Era, // 中生代
        Cenozoic = Eon.Phanerozoic + 3 * GeoChronType.Era // 新生代
    }

    // 纪。
    public enum Period
    {
        // 元古宙：

        // 古元古代
        Siderian = Era.Paleoproterozoic + GeoChronType.Period, // 成铁纪
        Rhyacian = Era.Paleoproterozoic + 2 * GeoChronType.Period, // 层侵纪
        Orosirian = Era.Paleoproterozoic + 3 * GeoChronType.Period, // 造山纪
        Statherian = Era.Paleoproterozoic + 4 * GeoChronType.Period, // 固结纪

        // 中元古代
        Calymmian = Era.Mesoproterozoic + GeoChronType.Period, // 盖层纪
        Ectasian = Era.Mesoproterozoic + 2 * GeoChronType.Period, // 延展纪
        Stenian = Era.Mesoproterozoic + 3 * GeoChronType.Period, // 狭带纪

        // 新元古代
        Tonian = Era.Neoproterozoic + GeoChronType.Period, // 拉伸纪
        Cryogenian = Era.Neoproterozoic + 2 * GeoChronType.Period, // 成冰纪
        Ediacaran = Era.Neoproterozoic + 3 * GeoChronType.Period, // 埃迪卡拉纪

        // 显生宙：

        // 古生代
        Cambrian = Era.Paleozoic + GeoChronType.Period, // 寒武纪
        Ordovician = Era.Paleozoic + 2 * GeoChronType.Period, // 奥陶纪
        Silurian = Era.Paleozoic + 3 * GeoChronType.Period, // 志留纪
        Devonian = Era.Paleozoic + 4 * GeoChronType.Period, // 泥盆纪
        Carboniferous = Era.Paleozoic + 5 * GeoChronType.Period, // 石炭纪
        Permian = Era.Paleozoic + 6 * GeoChronType.Period, // 二叠纪

        // 中生代
        Triassic = Era.Mesozoic + GeoChronType.Period, // 三叠纪
        Jurassic = Era.Mesozoic + 2 * GeoChronType.Period, // 侏罗纪
        Cretaceous = Era.Mesozoic + 3 * GeoChronType.Period, // 白垩纪

        // 新生代
        Paleogene = Era.Cenozoic + GeoChronType.Period, // 古近纪
        Neogene = Era.Cenozoic + 2 * GeoChronType.Period, // 新近纪
        Quaternary = Era.Cenozoic + 3 * GeoChronType.Period // 第四纪
    }

    // 世。
    public enum Epoch
    {
        // 古生代：

        // 寒武纪
        Terreneuvian = Period.Cambrian + GeoChronType.Epoch, // 纽芬兰世
        CambrianSeries2 = Period.Cambrian + 2 * GeoChronType.Epoch, // 寒武纪第二世
        Miaolingian = Period.Cambrian + 3 * GeoChronType.Epoch, // 苗岭世
        Furongian = Period.Cambrian + 4 * GeoChronType.Epoch, // 芙蓉世

        // 奥陶纪
        LowerOrdovician = Period.Ordovician + GeoChronType.Epoch, // 早奥陶世
        MiddleOrdovician = Period.Ordovician + 2 * GeoChronType.Epoch, // 中奥陶世
        UpperOrdovician = Period.Ordovician + 3 * GeoChronType.Epoch, // 晚奥陶世

        // 志留纪
        Llandovery = Period.Silurian + GeoChronType.Epoch, // 兰多维列世
        Wenlock = Period.Silurian + 2 * GeoChronType.Epoch, // 温洛克世
        Ludlow = Period.Silurian + 3 * GeoChronType.Epoch, // 罗德洛世
        Pridoli = Period.Silurian + 4 * GeoChronType.Epoch, // 普里道利世

        // 泥盆纪
        LowerDevonian = Period.Devonian + GeoChronType.Epoch, // 早泥盆世
        MiddleDevonian = Period.Devonian + 2 * GeoChronType.Epoch, // 中泥盆世
        UpperDevonian = Period.Devonian + 3 * GeoChronType.Epoch, // 晚泥盆世

        // 石炭纪
        Mississippian = Period.Carboniferous + GeoChronType.Epoch, // 密西西比世
        Pennsylvanian = Period.Carboniferous + 2 * GeoChronType.Epoch, // 宾夕法尼亚世

        // 二叠纪
        Cisuralian = Period.Permian + GeoChronType.Epoch, // 乌拉尔世
        Guadalupian = Period.Permian + 2 * GeoChronType.Epoch, // 瓜德鲁普世
        Lopingian = Period.Permian + 3 * GeoChronType.Epoch, // 乐平世

        // 中生代：

        // 三叠纪
        LowerTriassic = Period.Triassic + GeoChronType.Epoch, // 早三叠世
        MiddleTriassic = Period.Triassic + 2 * GeoChronType.Epoch, // 中三叠世
        UpperTriassic = Period.Triassic + 3 * GeoChronType.Epoch, // 晚三叠世

        // 侏罗纪
        LowerJurassic = Period.Jurassic + GeoChronType.Epoch, // 早侏罗世
        MiddleJurassic = Period.Jurassic + 2 * GeoChronType.Epoch, // 中侏罗世
        UpperJurassic = Period.Jurassic + 3 * GeoChronType.Epoch, // 晚侏罗世

        // 白垩纪
        LowerCretaceous = Period.Cretaceous + GeoChronType.Epoch, // 早白垩世
        UpperCretaceous = Period.Cretaceous + 2 * GeoChronType.Epoch, // 晚白垩世

        // 新生代：

        // 古近纪
        Paleocene = Period.Paleogene + GeoChronType.Epoch, // 古新世
        Eocene = Period.Paleogene + 2 * GeoChronType.Epoch, // 始新世
        Oligocene = Period.Paleogene + 3 * GeoChronType.Epoch, // 渐新世

        // 新近纪
        Miocene = Period.Neogene + GeoChronType.Epoch, // 中新世
        Pliocene = Period.Neogene + 2 * GeoChronType.Epoch, // 上新世

        // 第四纪
        Pleistocene = Period.Quaternary + GeoChronType.Epoch, // 更新世
        Holocene = Period.Quaternary + 2 * GeoChronType.Epoch // 全新世
        // Anthropocene = Period.Quaternary + 3 * GeoChronType.Epoch // 人类世
    }

    // 期。
    public enum Age
    {
        // 寒武纪：

        // 纽芬兰世
        Fortunian = Epoch.Terreneuvian + GeoChronType.Age, // 幸运期
        CambrianStage2 = Epoch.Terreneuvian + 2 * GeoChronType.Age, // 寒武纪第二期

        // 寒武纪第二世
        CambrianStage3 = Epoch.CambrianSeries2 + GeoChronType.Age, // 寒武纪第三期
        CambrianStage4 = Epoch.CambrianSeries2 + 2 * GeoChronType.Age, // 寒武纪第四期

        // 苗岭世
        Wuliuian = Epoch.Miaolingian + GeoChronType.Age, // 乌溜期
        Drumian = Epoch.Miaolingian + 2 * GeoChronType.Age, // 鼓山期
        Guzhangian = Epoch.Miaolingian + 3 * GeoChronType.Age, // 古丈期

        // 芙蓉世
        Paibian = Epoch.Furongian + GeoChronType.Age, // 排碧期
        Jiangshanian = Epoch.Furongian + 2 * GeoChronType.Age, // 江山期
        CambrianStage10 = Epoch.Furongian + 3 * GeoChronType.Age, // 寒武纪第十期

        // 奥陶纪：

        // 早奥陶世
        Tremadocian = Epoch.LowerOrdovician + GeoChronType.Age, // 特马豆克期
        Floian = Epoch.LowerOrdovician + 2 * GeoChronType.Age, // 弗洛期

        // 中奥陶世
        Dapingian = Epoch.MiddleOrdovician + GeoChronType.Age, // 大坪期
        Darriwilian = Epoch.MiddleOrdovician + 2 * GeoChronType.Age, // 达瑞威尔期

        // 晚奥陶世
        Sandbian = Epoch.UpperOrdovician + GeoChronType.Age, // 桑比期
        Katian = Epoch.UpperOrdovician + 2 * GeoChronType.Age, // 凯迪期
        Hirnantian = Epoch.UpperOrdovician + 3 * GeoChronType.Age, // 赫南特期

        // 志留纪：

        // 兰多维列世
        Rhuddanian = Epoch.Llandovery + GeoChronType.Age, // 鲁丹期
        Aeronian = Epoch.Llandovery + 2 * GeoChronType.Age, // 埃隆期
        Telychian = Epoch.Llandovery + 3 * GeoChronType.Age, // 特列奇期

        // 温洛克世
        Sheinwoodian = Epoch.Wenlock + GeoChronType.Age, // 申伍德期
        Homerian = Epoch.Wenlock + 2 * GeoChronType.Age, // 侯墨期

        // 罗德洛世
        Gorstian = Epoch.Ludlow + GeoChronType.Age, // 高斯特期
        Ludfordian = Epoch.Ludlow + 2 * GeoChronType.Age, // 卢德福特期

        // 普里道利世
        // （无）

        // 泥盆纪：

        // 早泥盆世
        Lochkovian = Epoch.LowerDevonian + GeoChronType.Age, // 洛赫考夫期
        Pragian = Epoch.LowerDevonian + 2 * GeoChronType.Age, // 布拉格期
        Emsian = Epoch.LowerDevonian + 3 * GeoChronType.Age, // 埃姆斯期

        // 中泥盆世
        Eifelian = Epoch.MiddleDevonian + GeoChronType.Age, // 艾菲尔期
        Givetian = Epoch.MiddleDevonian + 2 * GeoChronType.Age, // 吉维特期

        // 晚泥盆世
        Frasnian = Epoch.UpperDevonian + GeoChronType.Age, // 弗拉期
        Famennian = Epoch.UpperDevonian + 2 * GeoChronType.Age, // 法门期

        // 石炭纪：

        // 密西西比世
        Tournaisian = Epoch.Mississippian + GeoChronType.Age, // 杜内期
        Visean = Epoch.Mississippian + 2 * GeoChronType.Age, // 维宪期
        Serpukhovian = Epoch.Mississippian + 3 * GeoChronType.Age, // 谢尔普霍夫期

        // 宾夕法尼亚世
        Bashkirian = Epoch.Pennsylvanian + GeoChronType.Age, // 巴什基尔期
        Moscovian = Epoch.Pennsylvanian + 2 * GeoChronType.Age, // 莫斯科期
        Kasimovian = Epoch.Pennsylvanian + 3 * GeoChronType.Age, // 卡西莫夫期
        Gzhelian = Epoch.Pennsylvanian + 4 * GeoChronType.Age, // 格舍尔期

        // 二叠纪：

        // 乌拉尔世
        Asselian = Epoch.Cisuralian + GeoChronType.Age, // 阿瑟尔期
        Sakmarian = Epoch.Cisuralian + 2 * GeoChronType.Age, // 萨克马尔期
        Artinskian = Epoch.Cisuralian + 3 * GeoChronType.Age, // 亚丁斯克期
        Kungurian = Epoch.Cisuralian + 4 * GeoChronType.Age, // 空谷期

        // 瓜德鲁普世
        Roadian = Epoch.Guadalupian + GeoChronType.Age, // 罗德期
        Wordian = Epoch.Guadalupian + 2 * GeoChronType.Age, // 沃德期
        Capitanian = Epoch.Guadalupian + 3 * GeoChronType.Age, // 卡匹敦期

        // 乐平世
        Wuchiapingian = Epoch.Lopingian + GeoChronType.Age, // 吴家坪期
        Changhsingian = Epoch.Lopingian + 2 * GeoChronType.Age, // 长兴期

        // 三叠纪：

        // 早三叠世
        Induan = Epoch.LowerTriassic + GeoChronType.Age, // 印度期
        Olenekian = Epoch.LowerTriassic + 2 * GeoChronType.Age, // 奥伦尼克期

        // 中三叠世
        Anisian = Epoch.MiddleTriassic + GeoChronType.Age, // 安尼期
        Ladinian = Epoch.MiddleTriassic + 2 * GeoChronType.Age, // 拉丁期

        // 晚三叠世
        Carnian = Epoch.UpperTriassic + GeoChronType.Age, // 卡尼期
        Norian = Epoch.UpperTriassic + 2 * GeoChronType.Age, // 诺利期
        Rhaetian = Epoch.UpperTriassic + 3 * GeoChronType.Age, // 瑞替期

        // 侏罗纪：

        // 早侏罗世
        Hettangian = Epoch.LowerJurassic + GeoChronType.Age, // 赫塘期
        Sinemurian = Epoch.LowerJurassic + 2 * GeoChronType.Age, // 辛涅缪尔期
        Pliensbachian = Epoch.LowerJurassic + 3 * GeoChronType.Age, // 普林斯巴期
        Toarcian = Epoch.LowerJurassic + 4 * GeoChronType.Age, // 托阿尔期

        // 中侏罗世
        Aalenian = Epoch.MiddleJurassic + GeoChronType.Age, // 阿林期
        Bajocian = Epoch.MiddleJurassic + 2 * GeoChronType.Age, // 巴柔期
        Bathonian = Epoch.MiddleJurassic + 3 * GeoChronType.Age, // 巴通期
        Callovian = Epoch.MiddleJurassic + 4 * GeoChronType.Age, // 卡洛夫期

        // 晚侏罗世
        Oxfordian = Epoch.UpperJurassic + GeoChronType.Age, // 牛津期
        Kimmeridgian = Epoch.UpperJurassic + 2 * GeoChronType.Age, // 钦莫利期
        Tithonian = Epoch.UpperJurassic + 3 * GeoChronType.Age, // 提塘期

        // 白垩纪：

        // 早白垩世
        Berriasian = Epoch.LowerCretaceous + GeoChronType.Age, // 贝里阿斯期
        Valanginian = Epoch.LowerCretaceous + 2 * GeoChronType.Age, // 瓦兰今期
        Hauterivian = Epoch.LowerCretaceous + 3 * GeoChronType.Age, // 欧特里夫期
        Barremian = Epoch.LowerCretaceous + 4 * GeoChronType.Age, // 巴雷姆期
        Aptian = Epoch.LowerCretaceous + 5 * GeoChronType.Age, // 阿普特期
        Albian = Epoch.LowerCretaceous + 6 * GeoChronType.Age, // 阿尔布期

        // 晚白垩世
        Cenomanian = Epoch.UpperCretaceous + GeoChronType.Age, // 赛诺曼期
        Turonian = Epoch.UpperCretaceous + 2 * GeoChronType.Age, // 土仑期
        Coniacian = Epoch.UpperCretaceous + 3 * GeoChronType.Age, // 康尼亚克期
        Santonian = Epoch.UpperCretaceous + 4 * GeoChronType.Age, // 圣通期
        Campanian = Epoch.UpperCretaceous + 5 * GeoChronType.Age, // 坎潘期
        Maastrichtian = Epoch.UpperCretaceous + 6 * GeoChronType.Age, // 马斯特里赫特期

        // 古近纪：

        // 古新世
        Danian = Epoch.Paleocene + GeoChronType.Age,  // 丹麦期
        Selandian = Epoch.Paleocene + 2 * GeoChronType.Age, // 塞兰特期
        Thanetian = Epoch.Paleocene + 3 * GeoChronType.Age, // 坦尼特期

        // 始新世
        Ypresian = Epoch.Eocene + GeoChronType.Age, // 伊普里斯期
        Lutetian = Epoch.Eocene + 2 * GeoChronType.Age, // 卢泰特期
        Bartonian = Epoch.Eocene + 3 * GeoChronType.Age, // 巴顿期
        Priabonian = Epoch.Eocene + 4 * GeoChronType.Age, // 普利亚本期

        // 渐新世
        Rupelian = Epoch.Oligocene + GeoChronType.Age, // 吕珀尔期
        Chattian = Epoch.Oligocene + 2 * GeoChronType.Age, // 夏特期

        // 新近纪：

        // 中新世
        Aquitanian = Epoch.Miocene + GeoChronType.Age, // 阿基坦期
        Burdigalian = Epoch.Miocene + 2 * GeoChronType.Age, // 波尔多期
        Langhian = Epoch.Miocene + 3 * GeoChronType.Age, // 兰盖期
        Serravallian = Epoch.Miocene + 4 * GeoChronType.Age, // 赛拉瓦莱期
        Tortonian = Epoch.Miocene + 5 * GeoChronType.Age, // 托尔托纳期
        Messinian = Epoch.Miocene + 6 * GeoChronType.Age, // 墨西拿期

        // 上新世
        Zanclean = Epoch.Pliocene + GeoChronType.Age, // 赞克勒期
        Piacenzian = Epoch.Pliocene + 2 * GeoChronType.Age, // 皮亚琴察期

        // 第四纪：

        // 更新世
        Gelasian = Epoch.Pleistocene + GeoChronType.Age, // 杰拉期
        Calabrian = Epoch.Pleistocene + 2 * GeoChronType.Age, // 卡拉布里期
        Chibanian = Epoch.Pleistocene + 3 * GeoChronType.Age, // 千叶期
        UpperPleistocene = Epoch.Pleistocene + 4 * GeoChronType.Age, // 晚更新期

        // 全新世
        Greenlandian = Epoch.Holocene + GeoChronType.Age, // 格陵兰期
        Northgrippian = Epoch.Holocene + 2 * GeoChronType.Age, // 诺斯格瑞比期
        Meghalayan = Epoch.Holocene + 3 * GeoChronType.Age, // 梅加拉亚期

        // 人类世
        // （无）
    }

    // 地质年代。
    public sealed class GeoChron
    {
        private const double _AgeOfEarth = 4600; // 地球年龄（Ma B.P.）
        private const int _BPBaseYear = 1950; // 考古学基准年代

        private const string _EmptyString = "Empty";
        private const string _PresentString = "Present";
        private const string _MaBPSuffix = "Ma";
        private const string _BCPrefix = "BC";
        private const string _ADPrefix = "AD";

        private static bool _GeoChronTreeIsReady = false;
        private static GeoChron[] _Eons = null;
        private static Dictionary<int, GeoChron> _EnumValueToGeoChronTable = null;
        private static Dictionary<string, GeoChron> _StringToGeoChronTable = null;

        // 构建地质年代树。
        private static void _BuildGeoChronTree()
        {
            if (!_GeoChronTreeIsReady)
            {
                _EnumValueToGeoChronTable = new Dictionary<int, GeoChron>();
                _StringToGeoChronTable = new Dictionary<string, GeoChron>();

                _EnumValueToGeoChronTable.Add(Empty._EnumValue, Empty);
                _StringToGeoChronTable.Add(Empty._String.ToUpperInvariant(), Empty);

                _EnumValueToGeoChronTable.Add(Present._EnumValue, Present);
                _StringToGeoChronTable.Add(Present._String.ToUpperInvariant(), Present);

                GeoChron startTimepoint = null, endTimepoint = new GeoChron(_AgeOfEarth);

                Func<int, double, GeoChron> BuildSubordinate = (enumValue, endMaBP) =>
                {
                    startTimepoint = endTimepoint;
                    endTimepoint = (double.IsNaN(endMaBP) ? Present : new GeoChron(endMaBP));

                    GeoChron geoChron = new GeoChron(enumValue, new GeoChron[] { startTimepoint, endTimepoint });

                    _EnumValueToGeoChronTable.Add(enumValue, geoChron);
                    _StringToGeoChronTable.Add(geoChron._String.ToUpperInvariant(), geoChron);

                    return geoChron;
                };

                Func<int, GeoChron[], GeoChron> BuildSuperior = (enumValue, subordinates) =>
                {
                    GeoChron geoChron = new GeoChron(enumValue, subordinates);

                    _EnumValueToGeoChronTable.Add(enumValue, geoChron);
                    _StringToGeoChronTable.Add(geoChron._String.ToUpperInvariant(), geoChron);

                    return geoChron;
                };

                _Eons = new GeoChron[]
                {
                    // 冥古宙
                    BuildSubordinate((int)Eon.Hadean, 4000),
                    // 太古宙
                    BuildSuperior((int)Eon.Archean, new GeoChron[]
                    {
                        BuildSubordinate((int)Era.Eoarchean, 3600),
                        BuildSubordinate((int)Era.Paleoarchean, 3200),
                        BuildSubordinate((int)Era.Mesoarchean, 2800),
                        BuildSubordinate((int)Era.Neoarchean, 2500)
                    }),
                    // 元古宙
                    BuildSuperior((int)Eon.Proterozoic, new GeoChron[]
                    {
                        // 古元古代
                        BuildSuperior((int)Era.Paleoproterozoic, new GeoChron[]
                        {
                            BuildSubordinate((int)Period.Siderian, 2300),
                            BuildSubordinate((int)Period.Rhyacian, 2050),
                            BuildSubordinate((int)Period.Orosirian, 1800),
                            BuildSubordinate((int)Period.Statherian, 1600)
                        }),
                        // 中元古代
                        BuildSuperior((int)Era.Mesoproterozoic, new GeoChron[]
                        {
                            BuildSubordinate((int)Period.Calymmian, 1400),
                            BuildSubordinate((int)Period.Ectasian, 1200),
                            BuildSubordinate((int)Period.Stenian, 1000)
                        }),
                        // 新元古代
                        BuildSuperior((int)Era.Neoproterozoic, new GeoChron[]
                        {
                            BuildSubordinate((int)Period.Tonian, 720),
                            BuildSubordinate((int)Period.Cryogenian, 635),
                            BuildSubordinate((int)Period.Ediacaran, 541.0)
                        })
                    }),
                    // 显生宙
                    BuildSuperior((int)Eon.Phanerozoic, new GeoChron[]
                    {
                        // 古生代
                        BuildSuperior((int)Era.Paleozoic, new GeoChron[]
                        {
                            // 寒武纪
                            BuildSuperior((int)Period.Cambrian, new GeoChron[]
                            {
                                // 纽芬兰世
                                BuildSuperior((int)Epoch.Terreneuvian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Fortunian, 529),
                                    BuildSubordinate((int)Age.CambrianStage2, 521)
                                }),
                                // 寒武纪第二世
                                BuildSuperior((int)Epoch.CambrianSeries2, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.CambrianStage3, 514),
                                    BuildSubordinate((int)Age.CambrianStage4, 509)
                                }),
                                // 苗岭世
                                BuildSuperior((int)Epoch.Miaolingian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Wuliuian, 504.5),
                                    BuildSubordinate((int)Age.Drumian, 500.5),
                                    BuildSubordinate((int)Age.Guzhangian, 497)
                                }),
                                // 芙蓉世
                                BuildSuperior((int)Epoch.Furongian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Paibian, 494),
                                    BuildSubordinate((int)Age.Jiangshanian, 489.5),
                                    BuildSubordinate((int)Age.CambrianStage10, 485.4)
                                })
                            }),
                            // 奥陶纪
                            BuildSuperior((int)Period.Ordovician, new GeoChron[]
                            {
                                // 早奥陶世
                                BuildSuperior((int)Epoch.LowerOrdovician, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Tremadocian, 477.7),
                                    BuildSubordinate((int)Age.Floian, 470.0)
                                }),
                                // 中奥陶世
                                BuildSuperior((int)Epoch.MiddleOrdovician, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Dapingian, 467.3),
                                    BuildSubordinate((int)Age.Darriwilian, 458.4)
                                }),
                                // 晚奥陶世
                                BuildSuperior((int)Epoch.UpperOrdovician, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Sandbian, 453.0),
                                    BuildSubordinate((int)Age.Katian, 445.2),
                                    BuildSubordinate((int)Age.Hirnantian, 443.8)
                                })
                            }),
                            // 志留纪
                            BuildSuperior((int)Period.Silurian, new GeoChron[]
                            {
                                // 兰多维列世
                                BuildSuperior((int)Epoch.Llandovery, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Rhuddanian, 440.8),
                                    BuildSubordinate((int)Age.Aeronian, 438.5),
                                    BuildSubordinate((int)Age.Telychian, 433.4)
                                }),
                                // 温洛克世
                                BuildSuperior((int)Epoch.Wenlock, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Sheinwoodian, 430.5),
                                    BuildSubordinate((int)Age.Homerian, 427.4)
                                }),
                                // 罗德洛世
                                BuildSuperior((int)Epoch.Ludlow, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Gorstian, 425.6),
                                    BuildSubordinate((int)Age.Ludfordian, 423.0)
                                }),
                                // 普里道利世
                                BuildSubordinate((int)Epoch.Pridoli, 419.2)
                            }),
                            // 泥盆纪
                            BuildSuperior((int)Period.Devonian, new GeoChron[]
                            {
                                // 早泥盆世
                                BuildSuperior((int)Epoch.LowerDevonian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Lochkovian, 410.8),
                                    BuildSubordinate((int)Age.Pragian, 407.6),
                                    BuildSubordinate((int)Age.Emsian, 393.3)
                                }),
                                // 中泥盆世
                                BuildSuperior((int)Epoch.MiddleDevonian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Eifelian, 387.7),
                                    BuildSubordinate((int)Age.Givetian, 382.7)
                                }),
                                // 晚泥盆世
                                BuildSuperior((int)Epoch.UpperDevonian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Frasnian, 372.2),
                                    BuildSubordinate((int)Age.Famennian, 358.9)
                                })
                            }),
                            // 石炭纪
                            BuildSuperior((int)Period.Carboniferous, new GeoChron[]
                            {
                                // 密西西比世
                                BuildSuperior((int)Epoch.Mississippian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Tournaisian, 346.7),
                                    BuildSubordinate((int)Age.Visean, 330.9),
                                    BuildSubordinate((int)Age.Serpukhovian, 323.2)
                                }),
                                // 宾夕法尼亚世
                                BuildSuperior((int)Epoch.Pennsylvanian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Bashkirian, 315.2),
                                    BuildSubordinate((int)Age.Moscovian, 307.0),
                                    BuildSubordinate((int)Age.Kasimovian, 303.7),
                                    BuildSubordinate((int)Age.Gzhelian, 298.9)
                                })
                            }),
                            // 二叠纪
                            BuildSuperior((int)Period.Permian, new GeoChron[]
                            {
                                // 乌拉尔世
                                BuildSuperior((int)Epoch.Cisuralian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Asselian, 293.52),
                                    BuildSubordinate((int)Age.Sakmarian, 290.1),
                                    BuildSubordinate((int)Age.Artinskian, 283.5),
                                    BuildSubordinate((int)Age.Kungurian, 272.95)
                                }),
                                // 瓜德鲁普世
                                BuildSuperior((int)Epoch.Guadalupian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Roadian, 268.8),
                                    BuildSubordinate((int)Age.Wordian, 265.1),
                                    BuildSubordinate((int)Age.Capitanian, 259.1)
                                }),
                                // 乐平世
                                BuildSuperior((int)Epoch.Lopingian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Wuchiapingian, 254.14),
                                    BuildSubordinate((int)Age.Changhsingian, 251.902)
                                })
                            })
                        }),
                        // 中生代
                        BuildSuperior((int)Era.Mesozoic, new GeoChron[]
                        {
                            // 三叠纪
                            BuildSuperior((int)Period.Triassic, new GeoChron[]
                            {
                                // 早三叠世
                                BuildSuperior((int)Epoch.LowerTriassic, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Induan, 251.2),
                                    BuildSubordinate((int)Age.Olenekian, 247.2)
                                }),
                                // 中三叠世
                                BuildSuperior((int)Epoch.MiddleTriassic, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Anisian, 242),
                                    BuildSubordinate((int)Age.Ladinian, 237)
                                }),
                                // 晚三叠世
                                BuildSuperior((int)Epoch.UpperTriassic, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Carnian, 227),
                                    BuildSubordinate((int)Age.Norian, 208.5),
                                    BuildSubordinate((int)Age.Rhaetian, 201.3)
                                })
                            }),
                            // 侏罗纪
                            BuildSuperior((int)Period.Jurassic, new GeoChron[]
                            {
                                // 早侏罗世
                                BuildSuperior((int)Epoch.LowerJurassic, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Hettangian, 199.3),
                                    BuildSubordinate((int)Age.Sinemurian, 190.8),
                                    BuildSubordinate((int)Age.Pliensbachian, 182.7),
                                    BuildSubordinate((int)Age.Toarcian, 174.1)
                                }),
                                // 中侏罗世
                                BuildSuperior((int)Epoch.MiddleJurassic, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Aalenian, 170.3),
                                    BuildSubordinate((int)Age.Bajocian, 168.3),
                                    BuildSubordinate((int)Age.Bathonian, 166.1),
                                    BuildSubordinate((int)Age.Callovian, 163.5)
                                }),
                                // 晚侏罗世
                                BuildSuperior((int)Epoch.UpperJurassic, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Oxfordian, 157.3),
                                    BuildSubordinate((int)Age.Kimmeridgian, 152.1),
                                    BuildSubordinate((int)Age.Tithonian, 145.0)
                                })
                            }),
                            // 白垩纪
                            BuildSuperior((int)Period.Cretaceous, new GeoChron[]
                            {
                                // 早白垩世
                                BuildSuperior((int)Epoch.LowerCretaceous, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Berriasian, 139.8),
                                    BuildSubordinate((int)Age.Valanginian, 132.6),
                                    BuildSubordinate((int)Age.Hauterivian, 129.4),
                                    BuildSubordinate((int)Age.Barremian, 125.0),
                                    BuildSubordinate((int)Age.Aptian, 113.0),
                                    BuildSubordinate((int)Age.Albian, 100.5)
                                }),
                                // 晚白垩世
                                BuildSuperior((int)Epoch.UpperCretaceous, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Cenomanian, 93.9),
                                    BuildSubordinate((int)Age.Turonian, 89.8),
                                    BuildSubordinate((int)Age.Coniacian, 86.3),
                                    BuildSubordinate((int)Age.Santonian, 83.6),
                                    BuildSubordinate((int)Age.Campanian, 72.1),
                                    BuildSubordinate((int)Age.Maastrichtian, 66.0)
                                })
                            })
                        }),
                        // 新生代
                        BuildSuperior((int)Era.Cenozoic, new GeoChron[]
                        {
                            // 古近纪
                            BuildSuperior((int)Period.Paleogene, new GeoChron[]
                            {
                                // 古新世
                                BuildSuperior((int)Epoch.Paleocene, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Danian, 61.6),
                                    BuildSubordinate((int)Age.Selandian, 59.2),
                                    BuildSubordinate((int)Age.Thanetian, 56.0)
                                }),
                                // 始新世
                                BuildSuperior((int)Epoch.Eocene, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Ypresian, 47.8),
                                    BuildSubordinate((int)Age.Lutetian, 41.2),
                                    BuildSubordinate((int)Age.Bartonian, 37.71),
                                    BuildSubordinate((int)Age.Priabonian, 33.9)
                                }),
                                // 渐新世
                                BuildSuperior((int)Epoch.Oligocene, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Rupelian, 27.82),
                                    BuildSubordinate((int)Age.Chattian, 23.03)
                                })
                            }),
                            // 新近纪
                            BuildSuperior((int)Period.Neogene, new GeoChron[]
                            {
                                // 中新世
                                BuildSuperior((int)Epoch.Miocene, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Aquitanian, 20.44),
                                    BuildSubordinate((int)Age.Burdigalian, 15.97),
                                    BuildSubordinate((int)Age.Langhian, 13.82),
                                    BuildSubordinate((int)Age.Serravallian, 11.63),
                                    BuildSubordinate((int)Age.Tortonian, 7.246),
                                    BuildSubordinate((int)Age.Messinian, 5.333)
                                }),
                                // 上新世
                                BuildSuperior((int)Epoch.Pliocene, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Zanclean, 3.600),
                                    BuildSubordinate((int)Age.Piacenzian, 2.58)
                                })
                            }),
                            // 第四纪
                            BuildSuperior((int)Period.Quaternary, new GeoChron[]
                            {
                                // 更新世
                                BuildSuperior((int)Epoch.Pleistocene, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Gelasian, 1.80),
                                    BuildSubordinate((int)Age.Calabrian, 0.774),
                                    BuildSubordinate((int)Age.Chibanian, 0.129),
                                    BuildSubordinate((int)Age.UpperPleistocene, 0.011650)
                                }),
                                // 全新世
                                BuildSuperior((int)Epoch.Holocene, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Greenlandian, 0.008186),
                                    BuildSubordinate((int)Age.Northgrippian, 0.004200),
                                    BuildSubordinate((int)Age.Meghalayan, double.NaN)
                                })
                                // 人类世
                                // BuildSubordinate((int)Epoch.Anthropocene, double.NaN)
                            })
                        })
                    })
                };

                // 当某些地质年代（例如寒武纪第二世）正式命名后，在此处添加对旧名称的兼容：
                // _StringToGeoChronTable.Add(旧名称.ToUpperInvariant(), _EnumValueToGeoChronTable[枚举]);

                _GeoChronTreeIsReady = true;
            }
        }

        // 根据枚举值获取地质年代。
        private static GeoChron _GetGeoChronByEnumValue(int enumValue)
        {
            _BuildGeoChronTree();

            return _EnumValueToGeoChronTable[enumValue];
        }

        // 根据字符串获取地质年代或创建新实例。
        private static GeoChron _GetGeoChronByString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }

            //

            _BuildGeoChronTree();

            GeoChron geoChron;

            if (_StringToGeoChronTable.TryGetValue(text.ToUpperInvariant(), out geoChron))
            {
                return geoChron;
            }
            else
            {
                if (text.EndsWith(_MaBPSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    return CreateGeoChron(double.Parse(text[..^_MaBPSuffix.Length]));
                }
                else if (text.StartsWith(_BCPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    return CreateGeoChron(-int.Parse(text[_BCPrefix.Length..]));
                }
                else if (text.StartsWith(_ADPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    return CreateGeoChron(int.Parse(text[_ADPrefix.Length..]));
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        // 根据 MaBP 时间点获取所属地质年代。
        private static GeoChron _GetGeoChronByMaBP(double maBP)
        {
            _BuildGeoChronTree();

            if (maBP > _Eons[0]._StartTimepoint.MaBP)
            {
                return Empty;
            }
            else
            {
                IReadOnlyList<GeoChron> geoChrons = _Eons;
                GeoChron geoChron = Empty;

                while (geoChrons is not null)
                {
                    for (int i = 0; i < geoChrons.Count; i++)
                    {
                        if (maBP <= geoChrons[i]._StartTimepoint.MaBP && maBP > geoChrons[i]._EndTimepoint.MaBP)
                        {
                            geoChron = geoChrons[i];

                            break;
                        }
                    }

                    if (geoChron.HasTimespanSubordinates)
                    {
                        geoChrons = geoChron._Subordinates;
                    }
                    else
                    {
                        break;
                    }
                }

                return geoChron;
            }
        }

        // 根据 CEYear 时间点获取所属地质年代。
        private static GeoChron _GetGeoChronByCEYear(int ceYear) => _GetGeoChronByMaBP(_CEYearToMaBP(ceYear));

        private static double _CEYearToMaBP(int ceYear) => ((_BPBaseYear - (ceYear < 0 ? ceYear + 1 : ceYear)) * 0.000001);

        private int _EnumValue; // 枚举值
        private GeoChronType _Type; // 类型
        private double? _MaBP; // 距今百万年前（Ma Before Present）
        private int? _CEYear; // 公元纪年年份
        private string _String; // 文本表示
        private GeoChron _Superior; // 上级地质年代
        private GeoChron[] _Subordinates; // 下级地质年代
        private GeoChron _Start;
        private GeoChron _End;
        private GeoChron _StartTimepoint;
        private GeoChron _EndTimepoint;
        private bool _HasTimespanSubordinates;

        private double _StartTimepointAsMaBP
        {
            get
            {
                if (IsEmpty)
                {
                    return double.NaN;
                }
                else if (IsPresent)
                {
                    return _MaBP.Value;
                }
                else
                {
                    if (_StartTimepoint._Type == GeoChronType.MaBP)
                    {
                        return _StartTimepoint._MaBP.Value;
                    }
                    else
                    {
                        return _CEYearToMaBP(_StartTimepoint._CEYear.Value);
                    }
                }
            }
        }

        private double _EndTimepointAsMaBP
        {
            get
            {
                if (IsEmpty)
                {
                    return double.NaN;
                }
                else if (IsPresent)
                {
                    return _MaBP.Value;
                }
                else
                {
                    if (_EndTimepoint._Type is GeoChronType.MaBP or GeoChronType.Present)
                    {
                        return _EndTimepoint._MaBP.Value;
                    }
                    else
                    {
                        return _CEYearToMaBP(_EndTimepoint._CEYear.Value);
                    }
                }
            }
        }

        // 仅用于构造 Empty
        private GeoChron()
        {
            _EnumValue = (int)GeoChronType.Empty;
            _Type = GeoChronType.Empty;
            _MaBP = null;
            _CEYear = null;
            _String = _EmptyString;
            _Superior = null;
            _Subordinates = null;
            _Start = null;
            _End = null;
            _StartTimepoint = null;
            _EndTimepoint = null;
            _HasTimespanSubordinates = false;
        }

        // 仅用于构造 Present
        private GeoChron(object present)
        {
            _EnumValue = (int)GeoChronType.Present;
            _Type = GeoChronType.Present;
            _MaBP = double.NegativeInfinity;
            _CEYear = null;
            _String = _PresentString;
            _Superior = null;
            _Subordinates = null;
            _Start = this;
            _End = this;
            _StartTimepoint = this;
            _EndTimepoint = this;
            _HasTimespanSubordinates = false;
        }

        // 用于构造时间段
        private GeoChron(int enumValue, IReadOnlyList<GeoChron> subordinates)
        {
            if (subordinates is null)
            {
                throw new ArgumentNullException();
            }

            if (subordinates.Count <= 0)
            {
                throw new ArgumentException();
            }

            //

            if ((enumValue & ((int)GeoChronType.Age | ((int)GeoChronType.Age << 1) | ((int)GeoChronType.Age << 2) | ((int)GeoChronType.Age << 3))) != 0)
            {
                _Type = GeoChronType.Age;
                _String = ((Age)enumValue).ToString();
            }
            else if ((enumValue & ((int)GeoChronType.Epoch | ((int)GeoChronType.Epoch << 1) | ((int)GeoChronType.Epoch << 2) | ((int)GeoChronType.Epoch << 3))) != 0)
            {
                _Type = GeoChronType.Epoch;
                _String = ((Epoch)enumValue).ToString();
            }
            else if ((enumValue & ((int)GeoChronType.Period | ((int)GeoChronType.Period << 1) | ((int)GeoChronType.Period << 2) | ((int)GeoChronType.Period << 3))) != 0)
            {
                _Type = GeoChronType.Period;
                _String = ((Period)enumValue).ToString();
            }
            else if ((enumValue & ((int)GeoChronType.Era | ((int)GeoChronType.Era << 1) | ((int)GeoChronType.Era << 2) | ((int)GeoChronType.Era << 3))) != 0)
            {
                _Type = GeoChronType.Era;
                _String = ((Era)enumValue).ToString();
            }
            else if ((enumValue & ((int)GeoChronType.Eon | ((int)GeoChronType.Eon << 1) | ((int)GeoChronType.Eon << 2) | ((int)GeoChronType.Eon << 3))) != 0)
            {
                _Type = GeoChronType.Eon;
                _String = ((Eon)enumValue).ToString();
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            // 下级地质年代不能为 null 或 Empty，不能已有上级地质年代，且必须升序排列
            for (int i = 0; i < subordinates.Count; i++)
            {
                if (subordinates[i] is null)
                {
                    throw new ArgumentNullException();
                }

                if (subordinates[i].IsEmpty)
                {
                    throw new ArgumentException();
                }

                if (subordinates[i]._Superior is not null)
                {
                    throw new InvalidOperationException();
                }

                if (i > 0 && !(subordinates[i] > subordinates[i - 1]))
                {
                    throw new ArgumentException();
                }
            }

            // "期"的所有下级地质年代必须均为时间点，且必须为 2 个
            if (_Type == GeoChronType.Age)
            {
                if (subordinates.Count == 2 && subordinates[0].IsTimepoint && subordinates[1].IsTimepoint)
                {
                    _HasTimespanSubordinates = false;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                // 如果下级地质年代存在时间点，则所有下级地质年代必须均为时间点，且必须为 2 个
                if (subordinates[0].IsTimepoint)
                {
                    if (subordinates.Count == 2 && subordinates[1].IsTimepoint)
                    {
                        _HasTimespanSubordinates = false;
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
                // 否则所有下级地质年代必须均低于当前地质年代一个级别（这隐含了所有下级地质年代均不能为时间点）
                else
                {
                    _HasTimespanSubordinates = true;

                    GeoChronType subordinateType = _Type switch
                    {
                        GeoChronType.Eon => GeoChronType.Era,
                        GeoChronType.Era => GeoChronType.Period,
                        GeoChronType.Period => GeoChronType.Epoch,
                        GeoChronType.Epoch => GeoChronType.Age,
                        _ => GeoChronType.Empty
                    };

                    for (int i = 0; i < subordinates.Count; i++)
                    {
                        if (subordinates[i].Type != subordinateType)
                        {
                            throw new ArgumentException();
                        }
                    }
                }
            }

            //

            _EnumValue = enumValue;
            _MaBP = null;
            _CEYear = null;
            _Superior = null;
            _Subordinates = subordinates.ToArray();
            _Start = subordinates[0];
            _End = subordinates[^1];
            _StartTimepoint = subordinates[0]._StartTimepoint;
            _EndTimepoint = subordinates[^1]._EndTimepoint;

            if (_HasTimespanSubordinates)
            {
                foreach (var subordinate in subordinates)
                {
                    subordinate._Superior = this;
                }
            }
            else
            {
                subordinates[0]._Superior = this;

                if (subordinates[1].IsPresent)
                {
                    subordinates[1]._Superior = this;
                }
            }
        }

        // 用于构造 MaBP 时间点
        private GeoChron(double maBP)
        {
            if (double.IsNaN(maBP) || double.IsInfinity(maBP) || maBP < 0 || maBP > _AgeOfEarth)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            _EnumValue = (int)GeoChronType.MaBP;
            _Type = GeoChronType.MaBP;
            _MaBP = maBP;
            _CEYear = null;
            _String = maBP + _MaBPSuffix;
            _Superior = null;
            _Subordinates = null;
            _Start = this;
            _End = this;
            _StartTimepoint = this;
            _EndTimepoint = this;
            _HasTimespanSubordinates = false;
        }

        // 用于构造 CEYear 时间点
        private GeoChron(int ceYear)
        {
            if (ceYear == 0 || ceYear < -999999 || ceYear > 999999)
            {
                throw new ArgumentOutOfRangeException();
            }

            //

            _EnumValue = (int)GeoChronType.CEYear;
            _Type = GeoChronType.CEYear;
            _MaBP = null;
            _CEYear = ceYear;
            _String = (ceYear < 0 ? _BCPrefix + -ceYear : _ADPrefix + ceYear);
            _Superior = null;
            _Subordinates = null;
            _Start = this;
            _End = this;
            _StartTimepoint = this;
            _EndTimepoint = this;
            _HasTimespanSubordinates = false;
        }

        public static readonly GeoChron Empty = new GeoChron();

        public static readonly GeoChron Present = new GeoChron(null);

        public GeoChronType Type => _Type;

        public double? MaBP => _MaBP;

        public int? CEYear => _CEYear;

        public GeoChron Superior => _Superior;

        public IReadOnlyList<GeoChron> Subordinates => _Subordinates;

        public GeoChron Start => _Start;

        public GeoChron End => _End;

        public GeoChron StartTimepoint => _StartTimepoint;

        public GeoChron EndTimepoint => _EndTimepoint;

        public bool HasTimespanSubordinates => _HasTimespanSubordinates;

        public bool IsEmpty => (_Type == GeoChronType.Empty);

        public bool IsPresent => (_Type == GeoChronType.Present);

        public bool IsTimespan => (_Type is GeoChronType.Eon or GeoChronType.Era or GeoChronType.Period or GeoChronType.Epoch or GeoChronType.Age);

        public bool IsTimepoint => (_Type is GeoChronType.MaBP or GeoChronType.CEYear or GeoChronType.Present);

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj is null || !(obj is GeoChron))
            {
                return false;
            }
            else
            {
                GeoChron geoChron = obj as GeoChron;

                return (_EnumValue == geoChron._EnumValue && _Type == geoChron._Type && _MaBP == geoChron._MaBP && _CEYear == geoChron._CEYear);
            }
        }

        public override int GetHashCode()
        {
            switch (_Type)
            {
                case GeoChronType.MaBP: return (_EnumValue, _MaBP.Value).GetHashCode();
                case GeoChronType.CEYear: return (_EnumValue, _CEYear.Value).GetHashCode();
                default: return _EnumValue;
            }
        }

        public override string ToString() => _String;

        public static GeoChron GetGeoChron(Eon eon) => _GetGeoChronByEnumValue((int)eon);

        public static GeoChron GetGeoChron(Era era) => _GetGeoChronByEnumValue((int)era);

        public static GeoChron GetGeoChron(Period period) => _GetGeoChronByEnumValue((int)period);

        public static GeoChron GetGeoChron(Epoch epoch) => _GetGeoChronByEnumValue((int)epoch);

        public static GeoChron GetGeoChron(Age age) => _GetGeoChronByEnumValue((int)age);

        public static GeoChron CreateGeoChron(double maBP) => new GeoChron(maBP) { _Superior = _GetGeoChronByMaBP(maBP) };

        public static GeoChron CreateGeoChron(int ceYear) => new GeoChron(ceYear) { _Superior = _GetGeoChronByCEYear(ceYear) };

        public static GeoChron Parse(string str) => _GetGeoChronByString(str);

        public static bool TryParse(string str, out GeoChron geoChron)
        {
            try
            {
                geoChron = _GetGeoChronByString(str);

                return true;
            }
            catch
            {
                geoChron = null;

                return false;
            }
        }

        public static bool operator ==(GeoChron left, GeoChron right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (left is null || right is null)
            {
                return false;
            }
            else
            {
                return (left._EnumValue == right._EnumValue && left._Type == right._Type && left._MaBP == right._MaBP && left._CEYear == right._CEYear);
            }
        }

        public static bool operator !=(GeoChron left, GeoChron right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (left is null || right is null)
            {
                return true;
            }
            else
            {
                return (left._EnumValue != right._EnumValue || left._Type != right._Type || left._MaBP != right._MaBP || left._CEYear != right._CEYear);
            }
        }

        public static bool operator <(GeoChron left, GeoChron right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (left is null || right is null)
            {
                return false;
            }
            else
            {
                return (left._StartTimepointAsMaBP > right._StartTimepointAsMaBP && left._EndTimepointAsMaBP >= right._StartTimepointAsMaBP);
            }
        }

        public static bool operator >(GeoChron left, GeoChron right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return false;
            }
            else if (left is null || right is null)
            {
                return false;
            }
            else
            {
                return (left._EndTimepointAsMaBP < right._EndTimepointAsMaBP && left._StartTimepointAsMaBP <= right._EndTimepointAsMaBP);
            }
        }

        public static bool operator <=(GeoChron left, GeoChron right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (left is null || right is null)
            {
                return false;
            }
            else
            {
                return (left._StartTimepointAsMaBP >= right._StartTimepointAsMaBP && left._EndTimepointAsMaBP >= right._EndTimepointAsMaBP);
            }
        }

        public static bool operator >=(GeoChron left, GeoChron right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return true;
            }
            else if (left is null || right is null)
            {
                return false;
            }
            else
            {
                return (left._StartTimepointAsMaBP <= right._StartTimepointAsMaBP && left._EndTimepointAsMaBP <= right._EndTimepointAsMaBP);
            }
        }
    }
}