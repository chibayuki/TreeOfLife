﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using TreeOfLife.Core;
using TreeOfLife.Core.Search.Extensions;
using TreeOfLife.Core.Taxonomy;

using ColorX = Com.Chromatics.ColorX;
using Pages = TreeOfLife.UI.Views.ViewModel_MainWindow.Pages;

namespace TreeOfLife.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //

            _OpenFileDialog = new OpenFileDialog();
            _OpenFileDialog.Filter = "OTD 文件|*.otd";

            _SaveFileDialog = new SaveFileDialog();
            _SaveFileDialog.Filter = "OTD 文件|*.otd";

            //

            Common.SetCurrentTaxon = _SetCurrentTaxon;
            Common.EnterEditMode = () => _SetEditMode(true);
            Common.ExitEditMode = () => _SetEditMode(false);
            Common.UpdateCurrentTaxonInfo = () =>
            {
                if (Common.EditMode ?? false)
                {
                    view_Evo_EditMode.UpdateCurrentTaxonInfo();
                }
                else
                {
                    view_Evo_ViewMode.UpdateCurrentTaxonInfo();
                }
            };
            Common.UpdateTree = view_Tree.UpdateSubTree;

            AsyncMethod.Start = () =>
            {
                if (grid_WaitingForBackgroundTaskPrompt.IsVisible)
                {
                    MessageBox.Show("非法操作。", Entrance.AppName, MessageBoxButton.OK);
                    Environment.Exit(-1);
                }

                grid_WaitingForBackgroundTaskPrompt.Visibility = Visibility.Visible;
            };
            AsyncMethod.Finish = () => grid_WaitingForBackgroundTaskPrompt.Visibility = Visibility.Collapsed;

            view_File.ViewModel.OpenAsync = _OpenAsync;
            view_File.ViewModel.SaveAsync = _SaveAsync;
            view_File.ViewModel.SaveAsAsync = _SaveAsAsync;
            view_File.ViewModel.TrySaveAndCloseAsync = _TrySaveAndCloseAsync;

            view_File.ViewModel.OpenDone = () => _SelectPage(Pages.Evo);

            view_Search.ViewModel.ClickedSearchResult = () => _SelectPage(Pages.Evo);

            //

            this.Loaded += (s, e) =>
            {
                Entrance.New();

                _SetCurrentTaxon(Entrance.Root);
                _SetEditMode(false);

                _SelectPage(Pages.File);
            };

            this.Closing += (s, e) =>
            {
                if (grid_WaitingForBackgroundTaskPrompt.IsVisible || !_TrySaveAndClose())
                {
                    e.Cancel = true;
                }
            };

            button_File.Click += (s, e) => _SelectPage(Pages.File);
            button_Evo.Click += (s, e) => _SelectPage(Pages.Evo);
            button_Search.Click += (s, e) => _SelectPage(Pages.Search);
            button_About.Click += (s, e) => _SelectPage(Pages.About);

            button_IsDarkTheme.Click += (s, e) => Theme.IsDarkTheme = !Theme.IsDarkTheme;

            //

            Theme.IsDarkThemeChanged += (s, e) => button_IsDarkTheme.Content = Theme.IsDarkTheme ? "☼" : "☽";

            Random random = new Random(Environment.TickCount);
            Theme.ThemeColor = ColorX.FromHSL(random.Next(360), random.Next(20, 60), 50);
            Theme.IsDarkTheme = false;
        }

        //

        public ViewModel_MainWindow ViewModel => this.DataContext as ViewModel_MainWindow;

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
        private async Task<bool?> _OpenAsync()
        {
            bool? result = null;

            bool? r = _OpenFileDialog.ShowDialog();

            if (r ?? false)
            {
                result = await AsyncMethod.OpenAsync(_OpenFileDialog.FileName);
            }

            if (result ?? false)
            {
                _SetCurrentTaxon(Entrance.Root);
                view_Tree.UpdateSubTree();

                _Saved = true;
            }

            return result;
        }

        // true=成功，false=失败，null=取消
        private async Task<bool?> _SaveAsync()
        {
            bool? result = null;

            if (_Saved)
            {
                if (File.Exists(Entrance.FileName))
                {
                    if (Entrance.IsLatestVersion)
                    {
                        // 当且仅当（1）已经保存、（2）文件存在、（3）版本最新，才认为不需要（重新）保存且保存成功
                        result = true;
                    }
                    // 如果文件版本不是最新，那么重新保存
                    else
                    {
                        result = await AsyncMethod.SaveAsync();
                    }
                }
                // 如果文件已经保存，但不存在（被删除/移动存储介质弹出），那么重新保存
                else
                {
                    result = await AsyncMethod.SaveAsync();

                    // 如果保存失败，那么另存为
                    if (!result.Value)
                    {
                        bool? r = _SaveFileDialog.ShowDialog();

                        if (r ?? false)
                        {
                            result = await AsyncMethod.SaveAsAsync(_SaveFileDialog.FileName);
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Entrance.FileName))
                {
                    result = await AsyncMethod.SaveAsync();
                }
                // 首次保存等同于另存为
                else
                {
                    bool? r = _SaveFileDialog.ShowDialog();

                    if (r ?? false)
                    {
                        result = await AsyncMethod.SaveAsAsync(_SaveFileDialog.FileName);
                    }
                }
            }

            _Saved = result ?? false;

            return result;
        }

        // true=成功，false=失败，null=取消
        private bool? _Save()
        {
            bool? result = null;

            if (_Saved)
            {
                if (File.Exists(Entrance.FileName))
                {
                    if (Entrance.IsLatestVersion)
                    {
                        // 当且仅当（1）已经保存、（2）文件存在、（3）版本最新，才认为不需要（重新）保存且保存成功
                        result = true;
                    }
                    // 如果文件版本不是最新，那么重新保存
                    else
                    {
                        result = Entrance.Save();
                    }
                }
                // 如果文件已经保存，但不存在（被删除/移动存储介质弹出），那么重新保存
                else
                {
                    result = Entrance.Save();

                    // 如果保存失败，那么另存为
                    if (!result.Value)
                    {
                        bool? r = _SaveFileDialog.ShowDialog();

                        if (r ?? false)
                        {
                            result = Entrance.SaveAs(_SaveFileDialog.FileName);
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Entrance.FileName))
                {
                    result = Entrance.Save();
                }
                // 首次保存等同于另存为
                else
                {
                    bool? r = _SaveFileDialog.ShowDialog();

                    if (r ?? false)
                    {
                        result = Entrance.SaveAs(_SaveFileDialog.FileName);
                    }
                }
            }

            _Saved = result ?? false;

            return result;
        }

        // true=成功，false=失败，null=取消
        private async Task<bool?> _SaveAsAsync()
        {
            bool? result = null;

            bool? r = _SaveFileDialog.ShowDialog();

            if (r ?? false)
            {
                result = await AsyncMethod.SaveAsAsync(_SaveFileDialog.FileName);
            }

            _Saved = result ?? false;

            return result;
        }

        // true=成功，false=失败
        private async Task<bool> _CloseAsync()
        {
            Common.RightButtonTaxon = null;
            Common.SelectedTaxon = null;

            view_Search.ClearSearchResult();

            bool result = await AsyncMethod.CloseAsync();

            if (result)
            {
                _SetCurrentTaxon(Entrance.Root);
                view_Tree.UpdateSubTree();

                _Saved = false;
            }

            return result;
        }

        // true=成功，false=失败
        private bool _Close()
        {
            Common.RightButtonTaxon = null;
            Common.SelectedTaxon = null;

            view_Search.ClearSearchResult();

            bool result = Entrance.Close();

            if (result)
            {
                _SetCurrentTaxon(Entrance.Root);
                view_Tree.UpdateSubTree();

                _Saved = false;
            }

            return result;
        }

        // true=成功，false=失败或取消
        private async Task<bool> _TrySaveAndCloseAsync()
        {
            // 未保存过且内容为空，无需关闭，认为关闭成功
            if (string.IsNullOrEmpty(Entrance.FileName) && Entrance.IsEmpty)
            {
                // 这种状态下有可能右键选择了顶级类群
                Common.RightButtonTaxon = null;
                Common.SelectedTaxon = null;

                view_Search.ClearSearchResult();

                return true;
            }
            // 已保存，直接关闭
            else if (_Saved)
            {
                if (await _CloseAsync())
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("关闭失败。", Entrance.AppName, MessageBoxButton.OK);

                    return false;
                }
            }
            // 未保存，提示是否保存
            else
            {
                MessageBoxResult r = MessageBox.Show("是否保存？", Entrance.AppName, MessageBoxButton.YesNoCancel);

                switch (r)
                {
                    // 选择取消，什么也不做
                    case MessageBoxResult.Cancel: return false;

                    // 选择保存，先保存再关闭
                    case MessageBoxResult.Yes:
                        {
                            bool? save = await _SaveAsync();

                            if (save is not null)
                            {
                                if (save.Value)
                                {
                                    if (await _CloseAsync())
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("关闭失败。", Entrance.AppName, MessageBoxButton.OK);

                                        return false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("保存失败。", Entrance.AppName, MessageBoxButton.OK);

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
                        if (await _CloseAsync())
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("关闭失败。", Entrance.AppName, MessageBoxButton.OK);

                            return false;
                        }

                    default: return false;
                }
            }
        }

        // true=成功，false=失败或取消
        private bool _TrySaveAndClose()
        {
            // 未保存过且内容为空，无需关闭，认为关闭成功
            if (string.IsNullOrEmpty(Entrance.FileName) && Entrance.IsEmpty)
            {
                // 这种状态下有可能右键选择了顶级类群
                Common.RightButtonTaxon = null;
                Common.SelectedTaxon = null;

                view_Search.ClearSearchResult();

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
                    MessageBox.Show("关闭失败。", Entrance.AppName, MessageBoxButton.OK);

                    return false;
                }
            }
            // 未保存，提示是否保存
            else
            {
                MessageBoxResult r = MessageBox.Show("是否保存？", Entrance.AppName, MessageBoxButton.YesNoCancel);

                switch (r)
                {
                    // 选择取消，什么也不做
                    case MessageBoxResult.Cancel: return false;

                    // 选择保存，先保存再关闭
                    case MessageBoxResult.Yes:
                        {
                            bool? save = _Save();

                            if (save is not null)
                            {
                                if (save.Value)
                                {
                                    if (_Close())
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("关闭失败。", Entrance.AppName, MessageBoxButton.OK);

                                        return false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("保存失败。", Entrance.AppName, MessageBoxButton.OK);

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
                            MessageBox.Show("关闭失败。", Entrance.AppName, MessageBoxButton.OK);

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
            if (Common.EditMode != editMode)
            {
                Common.EditMode = editMode;

                //

                view_Evo_ViewMode.Visibility = !editMode ? Visibility.Visible : Visibility.Collapsed;
                view_Evo_EditMode.Visibility = editMode ? Visibility.Visible : Visibility.Collapsed;

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
                    Taxon currentTaxon = Common.CurrentTaxon;

                    // 退出编辑模式时，应位于具名类群（或顶级类群）
                    if (!currentTaxon.IsRoot && currentTaxon.IsAnonymous)
                    {
                        Taxon parent = currentTaxon.GetNamedParent();

                        if (parent is null)
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
            if (taxon.IsRoot && taxon != Entrance.Root)
            {
                throw new InvalidOperationException();
            }

            //

            if (Common.CurrentTaxon != taxon)
            {
                Common.CurrentTaxon = taxon;

                if (Common.EditMode ?? false)
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
    }
}