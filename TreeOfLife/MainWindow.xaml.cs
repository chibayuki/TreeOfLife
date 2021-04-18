/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1100.1000.M11.210405-0000

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
using System.IO;
using System.Reflection;

using TreeOfLife.Packaging;
using TreeOfLife.Phylogeny;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

using Pages = TreeOfLife.ViewModel_MainWindow.Pages;

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
            Views.Common.UpdateCurrentTaxonInfo = () =>
            {
                if (Views.Common.EditMode ?? false)
                {
                    view_Evo_EditMode.UpdateCurrentTaxonInfo();
                }
                else
                {
                    view_Evo_ViewMode.UpdateCurrentTaxonInfo();
                }
            };
            Views.Common.ApplyToTaxon = view_Evo_EditMode.ViewModel.ApplyToTaxon;
            Views.Common.UpdateTree = view_Tree.UpdateSubTree;

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

        private void _SelectPage(Pages tabPage)
        {
            if (ViewModel.CurrentPage != tabPage)
            {
                _SetEditMode(false);

                //

                ViewModel.CurrentPage = tabPage;
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

            if (r ?? false)
            {
                result = Phylogenesis.Open(_OpenFileDialog.FileName);
            }

            if (result ?? false)
            {
                _SetCurrentTaxon(Phylogenesis.Root);
                view_Tree.UpdateSubTree();

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
                if (File.Exists(Phylogenesis.FileName))
                {
                    if (Phylogenesis.PackageVersion == PackageVersion.Latest.Version)
                    {
                        // 当且仅当（1）已经保存、（2）文件存在、（3）版本最新，才认为不需要（重新）保存且保存成功
                        result = true;
                    }
                    // 如果文件版本不是最新，那么重新保存
                    else
                    {
                        result = Phylogenesis.Save();
                    }
                }
                // 如果文件已经保存，但不存在（被删除/移动存储介质弹出），那么重新保存
                else
                {
                    result = Phylogenesis.Save();

                    // 如果保存失败，那么另存为
                    if (!result.Value)
                    {
                        bool? r = _SaveFileDialog.ShowDialog();

                        if (r ?? false)
                        {
                            result = Phylogenesis.SaveAs(_SaveFileDialog.FileName);
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Phylogenesis.FileName))
                {
                    result = Phylogenesis.Save();
                }
                // 首次保存等同于另存为
                else
                {
                    bool? r = _SaveFileDialog.ShowDialog();

                    if (r ?? false)
                    {
                        result = Phylogenesis.SaveAs(_SaveFileDialog.FileName);
                    }
                }
            }

            _Saved = (result ?? false);

            return result;
        }

        // true=成功，false=失败，null=取消
        private bool? _SaveAs()
        {
            bool? result = null;

            bool? r = _SaveFileDialog.ShowDialog();

            if (r ?? false)
            {
                result = Phylogenesis.SaveAs(_SaveFileDialog.FileName);
            }

            _Saved = (result ?? false);

            return result;
        }

        // true=成功，false=失败
        private bool _Close()
        {
            Views.Common.RightButtonTaxon = null;
            Views.Common.SelectedTaxon = null;

            view_Search.ClearSearchResult();

            bool result = Phylogenesis.Close();

            if (result)
            {
                _SetCurrentTaxon(Phylogenesis.Root);
                view_Tree.UpdateSubTree();

                _Saved = false;
            }

            return result;
        }

        // true=成功，false=失败或取消
        private bool _TrySaveAndClose()
        {
            // 未保存过且内容为空，无需关闭，认为关闭成功
            if (string.IsNullOrEmpty(Phylogenesis.FileName) && Phylogenesis.IsEmpty)
            {
                // 这种状态下有可能右键选择了顶级类群
                Views.Common.RightButtonTaxon = null;
                Views.Common.SelectedTaxon = null;

                return true;
            }
            // 已保存，直接关闭
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
            // 未保存，提示是否保存
            else
            {
                MessageBoxResult r = MessageBox.Show("是否保存？", _AppName, MessageBoxButton.YesNoCancel);

                switch (r)
                {
                    // 选择取消，什么也不做
                    case MessageBoxResult.Cancel: return false;

                    // 选择保存，先保存再关闭
                    case MessageBoxResult.Yes:
                        {
                            bool? save = _Save();

                            if (save != null)
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

                    // 选择不保存，直接关闭
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

        // 进入/退出编辑模式。
        private void _SetEditMode(bool editMode)
        {
            if (Views.Common.EditMode != editMode)
            {
                if (Views.Common.EditMode ?? false)
                {
                    view_Evo_EditMode.ViewModel.ApplyToTaxon();
                }

                //

                Views.Common.EditMode = editMode;

                //

                view_Evo_ViewMode.Visibility = (!editMode ? Visibility.Visible : Visibility.Collapsed);
                view_Evo_EditMode.Visibility = (editMode ? Visibility.Visible : Visibility.Collapsed);

                //

                if (editMode)
                {
                    _Saved = false;

                    //

                    view_Evo_EditMode.UpdateCurrentTaxonInfo();
                    view_Tree.UpdateSubTree();
                }
                else
                {
                    Taxon currentTaxon = Views.Common.CurrentTaxon;

                    // 退出编辑模式时，应位于具名类群（或顶级类群）
                    if (!currentTaxon.IsRoot && currentTaxon.IsAnonymous())
                    {
                        Taxon parent = currentTaxon.GetNamedParent();

                        if (parent == null)
                        {
                            parent = currentTaxon.Root;
                        }

                        _SetCurrentTaxon(parent);
                    }
                    else
                    {
                        view_Evo_ViewMode.UpdateCurrentTaxonInfo();
                        view_Tree.UpdateSubTree();
                    }
                }
            }
        }

        // 设置当前选择的类群。
        private void _SetCurrentTaxon(Taxon taxon)
        {
            // 防止选择已删除的类群。
            if (taxon.IsRoot && taxon != Phylogenesis.Root)
            {
                throw new InvalidOperationException();
            }

            //

            if (Views.Common.CurrentTaxon != taxon)
            {
                if (Views.Common.EditMode ?? false)
                {
                    view_Evo_EditMode.ViewModel.ApplyToTaxon();
                }

                //

                Views.Common.CurrentTaxon = taxon;

                if (Views.Common.EditMode ?? false)
                {
                    view_Evo_EditMode.UpdateCurrentTaxonInfo();
                }
                else
                {
                    view_Evo_ViewMode.UpdateCurrentTaxonInfo();
                }

                view_Tree.UpdateSubTree();
            }
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