/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.112.1000.M2.201110-2050

This file is part of "生命树" (TreeOfLife)

"生命树" (TreeOfLife) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ColorManipulation = Com.ColorManipulation;
using ColorX = Com.ColorX;
using FormManager = Com.WinForm.FormManager;
using Theme = Com.WinForm.Theme;

namespace TreeOfLife
{
    public partial class MainForm : Form
    {
        #region 窗口定义

        private FormManager Me;

        public FormManager FormManager
        {
            get
            {
                return Me;
            }
        }

        private void _Ctor(FormManager owner)
        {
            InitializeComponent();

            //

            if (owner != null)
            {
                Me = new FormManager(this, owner);
            }
            else
            {
                Me = new FormManager(this);
            }

            //

            FormDefine();
        }

        public MainForm()
        {
            _Ctor(null);
        }

        public MainForm(FormManager owner)
        {
            _Ctor(owner);
        }

        private void FormDefine()
        {
            Me.Caption = Application.ProductName;
            Me.ShowCaptionBarColor = false;
            Me.EnableCaptionBarTransparent = false;
            Me.Theme = Theme.White;
            Me.ThemeColor = ColorManipulation.GetRandomColorX();

            Me.Loading += Me_Loading;
            Me.Loaded += Me_Loaded;
            Me.Closed += Me_Closed;
            Me.Resize += Me_Resize;
            Me.SizeChanged += Me_SizeChanged;
            Me.ThemeChanged += Me_ThemeChanged;
            Me.ThemeColorChanged += Me_ThemeChanged;
        }

        #endregion

        #region 窗口事件回调

        private void Me_Loading(object sender, EventArgs e)
        {

        }

        private void Me_Loaded(object sender, EventArgs e)
        {
            Me.OnThemeChanged();
            Me.OnSizeChanged();

            //

            Taxon root = PhylogeneticTree.Root;

            Taxon t = root.AddChild();
            t.ParseCurrent("露卡 LUCA");

            t = t.AddChild();
            t.ParseCurrent("新壁总域 Neomura");

            t = t.AddChild();
            t.ParseCurrent("真核域 Eukaryota");

            t = t.AddChild();
            t.ParseCurrent("单鞭毛生物 Unikonta");

            t = t.AddChild();
            t.ParseCurrent("后鞭毛生物 Opisthokonta");

            t = t.AddChild();
            t.ParseCurrent("动物总界 Holozoa");

            t = t.AddChild();
            t.ParseCurrent("蜷丝动物 Filozoa");

            t = t.AddChild();
            t.ParseCurrent("聚胞动物 Apoikozoa");

            t = t.AddChild();
            t.ParseCurrent("动物界 Animalia");

            t = t.AddChild();
            t.ParseCurrent("真后生动物亚界 Eumetazoa");

            t = t.AddChild();
            t.ParseCurrent("副同源异形基因动物 ParaHoxozoa");

            t = t.AddChild();
            t.ParseCurrent("浮浪幼虫样动物 Planulozoa");

            t = t.AddChild();
            t.ParseCurrent("两侧对称动物 Bilateria");

            t = t.AddChild();
            t.ParseCurrent("肾管动物 Nephrozoa");

            t = t.AddChild();
            t.ParseCurrent("后口动物总门 Deuterostomia");

            t = t.AddChild();
            t.ParseCurrent("脊索动物门 Chordata");

            t = t.AddChild();
            t.ParseCurrent("嗅球类 Olfactores");

            t = t.AddChild();
            t.ParseCurrent("有头动物 Craniata");

            t = t.AddChild();
            t.ParseCurrent("脊椎动物亚门 Vertebrata");

            t = t.AddChild();
            t.ParseCurrent("有颌下门 Gnathostomata");

            t = t.AddChild();
            t.ParseCurrent("真有颌小门 Eugnathostomata");

            t = t.AddChild();
            t.ParseCurrent("真口类 Teleostomi");

            t = t.AddChild();
            t.ParseCurrent("硬骨鱼高纲 Osteichthyes");

            t = t.AddChild();
            t.ParseCurrent("肉鳍鱼总纲 Sarcopterygii");

            t = t.AddChild();
            t.ParseCurrent("扇鳍类 Rhipidistia");

            t = t.AddChild();
            t.ParseCurrent("肺鱼四足纲 Dipnotetrapodomorpha");

            t = t.AddChild();
            t.ParseCurrent("四足形亚纲 Tetrapodomorpha");

            t = t.AddChild();
            t.ParseCurrent("骨鳞鱼总目 Osteolepidida");

            t = t.AddChild();
            t.ParseCurrent("始四足类 Eotetrapodiformes");

            t = t.AddChild();
            t.ParseCurrent("希望螈类 Elpistostegalia");

            t = t.AddChild();
            t.ParseCurrent("坚头类 Stegocephalia");

            t = t.AddChild();
            t.ParseCurrent("四足总纲 Tetrapoda");

            t = t.AddChild();
            t.ParseCurrent("爬行形类 Reptiliomorpha");

            t = t.AddChild();
            t.ParseCurrent("羊膜动物 Amniota");

            t = t.AddChild();
            t.ParseCurrent("合弓纲 Synapsida");

            t = t.AddChild();
            t.ParseCurrent("盘龙目 Pelycosauria");

            t = t.AddChild();
            t.ParseCurrent("真盘龙亚目 Eupelycosauria");

            t = t.AddChild();
            t.ParseCurrent("支 Metopophora");

            t = t.AddChild();
            t.ParseCurrent("支 Haptodontiformes");

            t = t.AddChild();
            t.ParseCurrent("楔齿龙形态类 Sphenacomorpha");

            t = t.AddChild();
            t.ParseCurrent("楔齿龙类 Sphenacodontia");

            t = t.AddChild();
            t.ParseCurrent("支 Pantherapsida");

            t = t.AddChild();
            t.ParseCurrent("楔齿龙超科 Sphenacodontoidea");

            Taxon Therapsida = t.AddChild();
            Therapsida.ParseCurrent("兽孔目 Therapsida");

            List<Taxon> parents;
            parents = Therapsida.GetSummaryParents();
            //parents = t.GetParents(TaxonParentFilterCondition.AnyTaxon(true));
            parents.Reverse();
            parents.Add(Therapsida);

            taxonNameButtonGroup_Parents.StartEditing();
            taxonNameButtonGroup_Parents.IsDarkTheme = (Me.Theme == Theme.DarkGray || Me.Theme == Theme.Black);
            int index = 0;
            int gIndex = 0;
            TaxonomicCategory currentCategory = TaxonomicCategory.Unranked;
            while (index < parents.Count)
            {
                Taxon _t = parents[index];

                if (index == 0)
                {
                    currentCategory = _t.Category.BasicCategory();

                    taxonNameButtonGroup_Parents.AddGroup(((currentCategory.IsPrimaryCategory() || currentCategory.IsSecondaryCategory()) ? currentCategory.Name() : string.Empty), _t.GetThemeColor());
                }
                else
                {
                    TaxonomicCategory basicCategory = _t.GetInheritedBasicCategory();

                    if (currentCategory != basicCategory)
                    {
                        currentCategory = basicCategory;

                        taxonNameButtonGroup_Parents.AddGroup(currentCategory.Name(), _t.GetThemeColor());

                        gIndex++;
                    }
                }

                TaxonNameButton button = new TaxonNameButton() { Taxon = _t };

                if (index == parents.Count - 1)
                {
                    button.Checked = true;
                }

                taxonNameButtonGroup_Parents.AddButton(button, gIndex);

                index++;
            }
            taxonNameButtonGroup_Parents.AutoSize = true;
            taxonNameButtonGroup_Parents.FinishEditing();

            /*Therapsida.ParseChildren(
                "?†四角兽属 Tetraceratops",
                "†珍稀兽属 Raranimus",
                "†巴莫鳄亚目 Biarmosuchia",
                "真兽孔类 Eutherapsida");*/
            Therapsida.ParseChildren(
                "?†四角兽属 Tetraceratops",
                "†珍稀兽属 Raranimus",
                "†巴莫鳄亚目 Biarmosuchia",
                "†恐头兽亚目 Dinocephalia",
                "†异齿亚目 Anomodontia",
                "†丽齿兽亚目 Gorgonopsia",
                "†兽头亚目 Therocephalia",
                "犬齿兽亚目 Cynodontia");
            taxonNameButtonGroup_Children.StartEditing();
            foreach (var item in Therapsida.GetNamedChildren())
            {
                ColorX color = item.GetThemeColor();

                taxonNameButtonGroup_Children.AddGroup(string.Empty, color);

                TaxonNameButton button = new TaxonNameButton() { Taxon = item };

                taxonNameButtonGroup_Children.AddButton(button, item.Index);
            }
            taxonNameButtonGroup_Children.Location = new Point(taxonNameButtonGroup_Parents.Left, taxonNameButtonGroup_Parents.Bottom + 20);
            taxonNameButtonGroup_Children.Width = taxonNameButtonGroup_Parents.Width;
            taxonNameButtonGroup_Children.AutoSize = true;
            taxonNameButtonGroup_Children.FinishEditing();

            Action<string, TaxonNameButton[]> setTaxonNameButton = (taxonName, taxonNameButtons) =>
            {
                Taxon taxon = new Taxon();
                taxon.ParseCurrent(taxonName);

                foreach (TaxonNameButton button in taxonNameButtons)
                {
                    button.Taxon = taxon;
                    button.IsDarkTheme = (Me.Theme == Theme.DarkGray || Me.Theme == Theme.Black);
                    button.ThemeColor = taxon.GetThemeColor();
                }
            };
            setTaxonNameButton("真核域 Eukaryota", new TaxonNameButton[] { taxonNameButtonA1, taxonNameButtonB1 });
            setTaxonNameButton("动物界 Animalia", new TaxonNameButton[] { taxonNameButtonA2, taxonNameButtonB2 });
            setTaxonNameButton("脊索动物门 Chordata", new TaxonNameButton[] { taxonNameButtonA3, taxonNameButtonB3 });
            setTaxonNameButton("哺乳纲 Mammalia", new TaxonNameButton[] { taxonNameButtonA4, taxonNameButtonB4 });
            setTaxonNameButton("食肉目 Carnivora", new TaxonNameButton[] { taxonNameButtonA5, taxonNameButtonB5 });
            setTaxonNameButton("犬科 Canidae", new TaxonNameButton[] { taxonNameButtonA6, taxonNameButtonB6 });
            setTaxonNameButton("犬属 Canis", new TaxonNameButton[] { taxonNameButtonA7, taxonNameButtonB7 });
            setTaxonNameButton("犬亚种 Familiaris", new TaxonNameButton[] { taxonNameButtonA8, taxonNameButtonB8 });
        }

        private void Me_Closed(object sender, EventArgs e)
        {

        }

        private void Me_Resize(object sender, EventArgs e)
        {

        }

        private void Me_SizeChanged(object sender, EventArgs e)
        {
            Me.OnResize();
        }

        private void Me_ThemeChanged(object sender, EventArgs e)
        {
            this.BackColor = Me.RecommendColors.FormBackground.ToColor();

        }

        #endregion
    }
}