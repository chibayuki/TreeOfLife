﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.608.1000.M6.201219-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;
using System.Reflection;

using TreeOfLife.Phylogeny;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

namespace TreeOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string _AppName = Assembly.GetExecutingAssembly().GetName().Name;

        //

        public MainWindow()
        {
            InitializeComponent();

            //

            _OpenFileDialog = new OpenFileDialog();
            _OpenFileDialog.Filter = "OTD 文件|*.otd";

            _SaveFileDialog = new SaveFileDialog();
            _SaveFileDialog.Filter = "OTD 文件|*.otd";

            //

            Views.Evo.Common.SetCurrentTaxon = _SetCurrentTaxon;
            Views.Evo.Common.EnterEditMode = () => _SetEditMode(true);
            Views.Evo.Common.ExitEditMode = () => _SetEditMode(false);

            view_File.ViewModel.Open = _Open;
            view_File.ViewModel.Save = _Save;
            view_File.ViewModel.SaveAs = _SaveAs;
            view_File.ViewModel.Close = _Close;
            view_File.ViewModel.TrySaveAndClose = _TrySaveAndClose;
            view_File.ViewModel.OpenDone = () => _SelectPage(Pages.Evo);

            //

            this.Loaded += (s, e) =>
            {
                _SetCurrentTaxon(Phylogenesis.Root);
                _SetEditMode(false);

                //

                _SelectPage(Pages.File);
            };

            this.Closing += (s, e) =>
            {
                if (!_TrySaveAndClose())
                {
                    e.Cancel = true;
                }
            };

            button_File.Click += (s, e) => _SelectPage(Pages.File);
            button_Evo.Click += (s, e) => _SelectPage(Pages.Evo);
            button_Search.Click += (s, e) => _SelectPage(Pages.Search);
            button_About.Click += (s, e) => _SelectPage(Pages.About);
        }

        #region 主题

        private bool _IsDarkTheme = false;

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                if (_IsDarkTheme != value)
                {
                    _IsDarkTheme = value;

                    view_File.IsDarkTheme = _IsDarkTheme;
                    view_Evo_ViewMode.IsDarkTheme = _IsDarkTheme;
                    view_Evo_EditMode.IsDarkTheme = _IsDarkTheme;
                }
            }
        }

        #endregion

        #region 页面切换

        private enum Pages
        {
            File,
            Evo,
            Search,
            About
        }

        private Pages? _CurrentPage = null;

        private void _SelectPage(Pages tabPage)
        {
            if (!_CurrentPage.HasValue || _CurrentPage.Value != tabPage)
            {
                _CurrentPage = tabPage;

                grid_File.Visibility = (_CurrentPage.Value == Pages.File ? Visibility.Visible : Visibility.Collapsed);
                grid_Evo.Visibility = (_CurrentPage.Value == Pages.Evo ? Visibility.Visible : Visibility.Collapsed);
                grid_Search.Visibility = (_CurrentPage.Value == Pages.Search ? Visibility.Visible : Visibility.Collapsed);
                grid_About.Visibility = (_CurrentPage.Value == Pages.About ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        #endregion

        #region "文件"页面

        private OpenFileDialog _OpenFileDialog;
        private SaveFileDialog _SaveFileDialog;

        private bool _Saved = false;

        private bool _Open()
        {
            bool result = true;

            bool? r = _OpenFileDialog.ShowDialog();

            if (r.HasValue && r.Value)
            {
                result = Phylogenesis.Open(_OpenFileDialog.FileName);
            }

            if (result)
            {
                _SetCurrentTaxon(Phylogenesis.Root);
                _SetEditMode(false);
                _UpdatePhylogeneticTree();

                _Saved = true;
            }

            return result;
        }

        private bool _Save()
        {
            bool result = true;

            if (!string.IsNullOrEmpty(Phylogenesis.FileName))
            {
                if (_EditMode.HasValue && _EditMode.Value)
                {
                    _SetEditMode(false);
                }

                result = Phylogenesis.Save();
            }
            else
            {
                bool? r = _SaveFileDialog.ShowDialog();

                if (r.HasValue && r.Value)
                {
                    if (_EditMode.HasValue && _EditMode.Value)
                    {
                        _SetEditMode(false);
                    }

                    result = Phylogenesis.SaveAs(_SaveFileDialog.FileName);
                }
            }

            if (result)
            {
                _Saved = true;
            }

            return result;
        }

        private bool _SaveAs()
        {
            bool result = true;

            bool? r = _SaveFileDialog.ShowDialog();

            if (r.HasValue && r.Value)
            {
                if (_EditMode.HasValue && _EditMode.Value)
                {
                    _SetEditMode(false);
                }

                result = Phylogenesis.SaveAs(_SaveFileDialog.FileName);
            }

            if (result)
            {
                _Saved = true;
            }

            return result;
        }

        private bool _Close()
        {
            bool result = true;

            result = Phylogenesis.Close();

            if (result)
            {
                _SetCurrentTaxon(Phylogenesis.Root);
                _SetEditMode(false);
                _UpdatePhylogeneticTree();

                _Saved = false;
            }

            return result;
        }

        private bool _TrySaveAndClose()
        {
            if (string.IsNullOrEmpty(Phylogenesis.FileName) && Phylogenesis.IsEmpty)
            {
                return true;
            }
            else if (_Saved)
            {
                if (_Close())
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("关闭失败。", _AppName, MessageBoxButton.OK);

                    return false;
                }
            }
            else
            {
                MessageBoxResult r = MessageBox.Show("是否保存?", _AppName, MessageBoxButton.YesNoCancel);

                switch (r)
                {
                    case MessageBoxResult.Cancel: return false;

                    case MessageBoxResult.Yes:
                        if (_Save())
                        {
                            if (_Close())
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("关闭失败。", _AppName, MessageBoxButton.OK);

                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("保存失败。", _AppName, MessageBoxButton.OK);

                            return false;
                        }

                    case MessageBoxResult.No:
                        if (_Close())
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("关闭失败。", _AppName, MessageBoxButton.OK);

                            return false;
                        }

                    default: return false;
                }
            }
        }

        #endregion

        #region "分类学"页面

        // 更新当前类群的所有信息。
        private void _UpdateCurrentTaxonInfo()
        {
            if (_EditMode.HasValue && _EditMode.Value)
            {
                view_Evo_EditMode.SetTaxon(_CurrentTaxon);
            }
            else
            {
                view_Evo_ViewMode.SetTaxon(_CurrentTaxon);
            }
        }

        private bool? _EditMode = null; // 是否为编辑模式。

        // 进入/退出编辑模式。
        private void _SetEditMode(bool editMode)
        {
            if (!_EditMode.HasValue || _EditMode.Value != editMode)
            {
                if (_EditMode.HasValue && _EditMode.Value)
                {
                    view_Evo_EditMode.ViewModel.ApplyToTaxon();
                }

                //

                _EditMode = editMode;

                view_Evo_ViewMode.Visibility = (!_EditMode.Value ? Visibility.Visible : Visibility.Collapsed);
                view_Evo_EditMode.Visibility = (_EditMode.Value ? Visibility.Visible : Visibility.Collapsed);

                //

                if (_EditMode.Value)
                {
                    _Saved = false;
                }

                //

                _UpdateCurrentTaxonInfo();

                //

                if (!_EditMode.Value)
                {
                    _UpdatePhylogeneticTree();
                }
            }
        }

        private Taxon _CurrentTaxon = null; // 当前选择的类群。

        // 设置当前选择的类群。
        private void _SetCurrentTaxon(Taxon taxon)
        {
            if (_CurrentTaxon != taxon)
            {
                if (_EditMode.HasValue && _EditMode.Value)
                {
                    view_Evo_EditMode.ViewModel.ApplyToTaxon();
                }

                //

                _CurrentTaxon = taxon;
                Views.Evo.Common.CurrentTaxon = taxon;

                _UpdateCurrentTaxonInfo();
            }
        }

        #endregion

        #region 系统发生树（临时）

        private void _RecursiveFillStringBuilder(StringBuilder sb, Taxon taxon)
        {
            if (sb == null || taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var child in taxon.Children)
            {
                char[] ch = new char[child.Level * 2];

                for (int i = 0; i < ch.Length; i++)
                {
                    ch[i] = '　';
                }

                int chIndex = ch.Length - 2;

                if (child.Index < child.Parent.Children.Count - 1)
                {
                    ch[chIndex] = '├';
                }
                else
                {
                    ch[chIndex] = '└';
                }

                chIndex -= 2;

                Taxon t = child.Parent;

                while (t.Parent != null)
                {
                    if (t.Index < t.Parent.Children.Count - 1)
                    {
                        ch[chIndex] = '│';
                    }

                    chIndex -= 2;
                    t = t.Parent;
                }

                sb.Append(ch);

                if (child.IsNamed())
                {
                    sb.AppendLine(child.LongName());
                }
                else
                {
                    sb.AppendLine("─");
                }

                _RecursiveFillStringBuilder(sb, child);
            }
        }

        // 更新系统发生树。
        private void _UpdatePhylogeneticTree()
        {
            textBox_EvoTree.Clear();

            StringBuilder sb = new StringBuilder();

            _RecursiveFillStringBuilder(sb, Phylogenesis.Root);

            textBox_EvoTree.Text = sb.ToString();
        }

        #endregion
    }
}