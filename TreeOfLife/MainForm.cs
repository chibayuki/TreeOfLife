/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.200.1000.M3.201111-0000

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
    internal partial class MainForm : Form
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

            const string fileName = @".\Phylogenesis.json";

            if (Phylogenesis.Open(fileName))
            {
                Taxon Therapsida = Phylogenesis.Root;
                while (Therapsida.Children.Count > 0)
                {
                    Therapsida = Therapsida.Children[0];

                    if (Therapsida.ChineseName == "兽孔目")
                    {
                        break;
                    }
                }

                List<Taxon> parents;
                parents = Therapsida.GetSummaryParents(false);
                parents.Reverse();
                parents.Add(Therapsida);

                taxonNameButtonGroup_Parents.StartEditing();
                taxonNameButtonGroup_Parents.IsDarkTheme = (Me.Theme == Theme.DarkGray || Me.Theme == Theme.Black);
                int groupIndex = 0;
                TaxonomicCategory currentCategory = TaxonomicCategory.Unranked;
                for (int i = 0; i < parents.Count; i++)
                {
                    Taxon taxon = parents[i];

                    if (i == 0)
                    {
                        currentCategory = taxon.Category.BasicCategory();

                        taxonNameButtonGroup_Parents.AddGroup(((currentCategory.IsPrimaryCategory() || currentCategory.IsSecondaryCategory()) ? currentCategory.Name() : string.Empty), taxon.GetThemeColor());
                    }
                    else
                    {
                        TaxonomicCategory basicCategory = taxon.GetInheritedBasicCategory();

                        if (currentCategory != basicCategory)
                        {
                            currentCategory = basicCategory;

                            taxonNameButtonGroup_Parents.AddGroup(currentCategory.Name(), taxon.GetThemeColor());

                            groupIndex++;
                        }
                    }

                    TaxonNameButton button = new TaxonNameButton() { Taxon = taxon };

                    if (i == parents.Count - 1)
                    {
                        button.Checked = true;
                    }

                    taxonNameButtonGroup_Parents.AddButton(button, groupIndex);
                }
                taxonNameButtonGroup_Parents.AutoSize = true;
                taxonNameButtonGroup_Parents.FinishEditing();

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

                Phylogenesis.SaveAs(fileName);
            }
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