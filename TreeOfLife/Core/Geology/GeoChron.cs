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

namespace TreeOfLife.Core.Geology
{
    // 地质年代。
    public sealed class GeoChron
    {
        private const double _AgeOfEarth = 4600; // 地球年龄（Ma）
        private const int _BPBaseYear = 1950; // 考古学基准年代

        private const string _EmptyString = "Empty";
        private const string _PresentString = "Present";
        private const string _MaSuffix = "Ma";
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

                // 构建叶节点
                // 每个叶节点起始于上一个叶节点的结束时刻，所以应确保全局所有叶节点按顺序调用
                // 第一个叶节点起始于地球年龄，最后一个叶节点结束于现代
                Func<int, double, GeoChron> BuildSubordinate = (enumValue, endMaBP) =>
                {
                    startTimepoint = endTimepoint;
                    endTimepoint = double.IsNaN(endMaBP) ? Present : new GeoChron(endMaBP);

                    GeoChron geoChron = new GeoChron(enumValue, new GeoChron[] { startTimepoint, endTimepoint });

                    _EnumValueToGeoChronTable.Add(enumValue, geoChron);
                    _StringToGeoChronTable.Add(geoChron._String.ToUpperInvariant(), geoChron);

                    return geoChron;
                };

                // 构建非叶节点
                // 应确保子节点符合先后顺序、层级顺序
                Func<int, IReadOnlyList<GeoChron>, GeoChron> BuildSuperior = (enumValue, subordinates) =>
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
                                    BuildSubordinate((int)Age.Kungurian, 273.01)
                                }),
                                // 瓜德鲁普世
                                BuildSuperior((int)Epoch.Guadalupian, new GeoChron[]
                                {
                                    BuildSubordinate((int)Age.Roadian, 266.9),
                                    BuildSubordinate((int)Age.Wordian, 264.28),
                                    BuildSubordinate((int)Age.Capitanian, 259.51)
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

                // 当某些地质年代（例如"寒武纪第二世"）正式命名后，在此处添加对旧名称的兼容：
                // _StringToGeoChronTable.Add("旧名称".ToUpperInvariant(), _EnumValueToGeoChronTable[枚举]);

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
                if (text.EndsWith(_MaSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    return CreateGeoChron(double.Parse(text[..^_MaSuffix.Length]));
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

        private static double _CEYearToMaBP(int ceYear) => (_BPBaseYear - (ceYear < 0 ? ceYear + 1 : ceYear)) * 0.000001;

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
            _String = maBP + _MaSuffix;
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
            _String = ceYear < 0 ? _BCPrefix + -ceYear : _ADPrefix + ceYear;
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

        public bool IsEmpty => _Type == GeoChronType.Empty;

        public bool IsPresent => _Type == GeoChronType.Present;

        public bool IsTimespan => _Type is GeoChronType.Eon or GeoChronType.Era or GeoChronType.Period or GeoChronType.Epoch or GeoChronType.Age;

        public bool IsTimepoint => _Type is GeoChronType.MaBP or GeoChronType.CEYear or GeoChronType.Present;

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

                return _EnumValue == geoChron._EnumValue && _Type == geoChron._Type && _MaBP == geoChron._MaBP && _CEYear == geoChron._CEYear;
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
                return left._EnumValue == right._EnumValue && left._Type == right._Type && left._MaBP == right._MaBP && left._CEYear == right._CEYear;
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
                return left._EnumValue != right._EnumValue || left._Type != right._Type || left._MaBP != right._MaBP || left._CEYear != right._CEYear;
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
                return left._StartTimepointAsMaBP > right._StartTimepointAsMaBP && left._EndTimepointAsMaBP >= right._StartTimepointAsMaBP;
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
                return left._EndTimepointAsMaBP < right._EndTimepointAsMaBP && left._StartTimepointAsMaBP <= right._EndTimepointAsMaBP;
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
                return left._StartTimepointAsMaBP >= right._StartTimepointAsMaBP && left._EndTimepointAsMaBP >= right._EndTimepointAsMaBP;
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
                return left._StartTimepointAsMaBP <= right._StartTimepointAsMaBP && left._EndTimepointAsMaBP <= right._EndTimepointAsMaBP;
            }
        }
    }
}