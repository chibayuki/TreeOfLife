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
    // 地质年代类型。所有枚举用于计算 GeoChron 的哈希值，需要确保唯一性，下同。
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
}