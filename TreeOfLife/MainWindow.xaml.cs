/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.700.1000.M7.201226-0000

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

        public ViewModel_MainWindow ViewModel => this.DataContext as ViewModel_MainWindow;

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

            Views.Common.SetCurrentTaxon = _SetCurrentTaxon;
            Views.Common.EnterEditMode = () => _SetEditMode(true);
            Views.Common.ExitEditMode = () => _SetEditMode(false);

            view_File.ViewModel.Open = _Open;
            view_File.ViewModel.Save = _Save;
            view_File.ViewModel.SaveAs = _SaveAs;
            view_File.ViewModel.TrySaveAndClose = _TrySaveAndClose;
            view_File.ViewModel.OpenDone = () => _SelectPage(Pages.Evo);

            view_Search.ViewModel.ClickSearchResult = () => _SelectPage(Pages.Evo);

            //

            this.Loaded += (s, e) =>
            {
                Phylogenesis.New();

                _SetCurrentTaxon(Phylogenesis.Root);
                _SetEditMode(false);

                //

                _SelectPage(Pages.File);

                //

                IsDarkTheme = false;
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

        //

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
                _SetEditMode(false);

                //

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

        // true=成功，false=失败，null=取消
        private bool? _Open()
        {
            bool? result = null;

            bool? r = _OpenFileDialog.ShowDialog();

            if (r.HasValue && r.Value)
            {
                result = Phylogenesis.Open(_OpenFileDialog.FileName);
            }

            if (result.HasValue && result.Value)
            {
                _SetCurrentTaxon(Phylogenesis.Root);
                _UpdateTree();

                _Saved = true;
            }

            return result;
        }

        // true=成功，false=失败，null=取消
        private bool? _Save()
        {
            bool? result = null;

            if (_Saved)
            {
                result = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(Phylogenesis.FileName))
                {
                    result = Phylogenesis.Save();
                }
                else
                {
                    bool? r = _SaveFileDialog.ShowDialog();

                    if (r.HasValue && r.Value)
                    {
                        result = Phylogenesis.SaveAs(_SaveFileDialog.FileName);
                    }
                }

                if (result.HasValue && result.Value)
                {
                    _Saved = true;
                }
            }

            return result;
        }

        // true=成功，false=失败，null=取消
        private bool? _SaveAs()
        {
            bool? result = null;

            bool? r = _SaveFileDialog.ShowDialog();

            if (r.HasValue && r.Value)
            {
                result = Phylogenesis.SaveAs(_SaveFileDialog.FileName);
            }

            if (result.HasValue && result.Value)
            {
                _Saved = true;
            }

            return result;
        }

        // true=成功，false=失败
        private bool _Close()
        {
            view_Search.ClearSearchResult();

            bool result = Phylogenesis.Close();

            if (result)
            {
                _SetCurrentTaxon(Phylogenesis.Root);
                _UpdateTree();

                _Saved = false;
            }

            return result;
        }

        // true=成功，false=失败或取消
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
                MessageBoxResult r = MessageBox.Show("是否保存？", _AppName, MessageBoxButton.YesNoCancel);

                switch (r)
                {
                    case MessageBoxResult.Cancel: return false;

                    case MessageBoxResult.Yes:
                        {
                            bool? save = _Save();

                            if (save.HasValue)
                            {
                                if (save.Value)
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
                            }
                            else
                            {
                                return false;
                            }
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

                    //

                    _UpdateCurrentTaxonInfo();
                }
                else
                {
                    Views.Evo.Common.RightButtonTaxon = null;
                    Views.Evo.Common.SelectedTaxon = null;

                    //

                    // 退出编辑模式时，应位于具名类群（或顶级类群）
                    if (!_CurrentTaxon.IsRoot && _CurrentTaxon.IsAnonymous())
                    {
                        Taxon parent = _CurrentTaxon.GetNamedParent();

                        if (parent == null)
                        {
                            parent = _CurrentTaxon.Root;
                        }

                        _SetCurrentTaxon(parent);
                    }
                    else
                    {
                        _UpdateCurrentTaxonInfo();
                    }

                    //

                    _UpdateTree();
                }
            }
        }

        private Taxon _CurrentTaxon = null; // 当前选择的类群。

        // 设置当前选择的类群。
        private void _SetCurrentTaxon(Taxon taxon)
        {
            if (taxon.IsRoot && taxon != Phylogenesis.Root)
            {
                throw new InvalidOperationException();
            }

            //

            if (_CurrentTaxon != taxon)
            {
                if (_EditMode.HasValue && _EditMode.Value)
                {
                    view_Evo_EditMode.ViewModel.ApplyToTaxon();
                }

                //

                _CurrentTaxon = taxon;
                Views.Common.CurrentTaxon = taxon;

                _UpdateCurrentTaxonInfo();
            }
        }

        #endregion

        #region 系统发生树（临时）

        // 更新系统发生树。
        private void _UpdateTree()
        {
            view_Tree.UpdateTree();
        }

        #endregion

        #region 主题

        private bool _IsDarkTheme = false;

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                Views.Common.IsDarkTheme = value;

                view_File.IsDarkTheme = _IsDarkTheme;
                view_Evo_ViewMode.IsDarkTheme = _IsDarkTheme;
                view_Evo_EditMode.IsDarkTheme = _IsDarkTheme;
                view_Search.IsDarkTheme = _IsDarkTheme;
                view_Tree.IsDarkTheme = _IsDarkTheme;

                ViewModel.IsDarkTheme = _IsDarkTheme;
            }
        }

        #endregion
    }
}