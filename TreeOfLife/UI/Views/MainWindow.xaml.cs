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

namespace TreeOfLife.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

            Common.IsEditModeChanged += _IsEditModeChanged;
            Common.CurrentTaxonChanged += _CurrentTaxonChanged;
            Common.CurrentTabPageChanged += _CurrentTabPageChanged;

            bool backgroundTaskIsRunning = false;
            AsyncMethod.Start = () =>
            {
                if (backgroundTaskIsRunning)
                {
#if DEBUG
                    throw new InvalidOperationException();
#else
                    MessageBox.Show("无效的操作。", Entrance.AppName, MessageBoxButton.OK);
                    Environment.Exit(-1);
#endif
                }

                grid_WaitingForBackgroundTaskPrompt.Visibility = Visibility.Visible;
                backgroundTaskIsRunning = true;
            };
            AsyncMethod.Finish = () =>
            {
                grid_WaitingForBackgroundTaskPrompt.Visibility = Visibility.Collapsed;
                backgroundTaskIsRunning = false;
            };

            view_File.OpenAsync = _OpenAsync;
            view_File.SaveAsync = _SaveAsync;
            view_File.SaveAsAsync = _SaveAsAsync;
            view_File.TrySaveAndCloseAsync = _TrySaveAndCloseAsync;

            //

            this.Loaded += (s, e) =>
            {
                Entrance.New();

                Common.CurrentTaxon = Entrance.Root;
                Common.IsEditMode = false;
                _IsEditModeChanged(null, Common.IsEditMode);
                Common.CurrentTabPage = Common.TabPage.File;
            };

            this.Closing += (s, e) =>
            {
                if (backgroundTaskIsRunning || !_TrySaveAndClose())
                {
                    e.Cancel = true;
                }
            };

            button_File.Click += (s, e) => Common.CurrentTabPage = Common.TabPage.File;
            button_Evo.Click += (s, e) => Common.CurrentTabPage = Common.TabPage.Evo;
            button_Search.Click += (s, e) => Common.CurrentTabPage = Common.TabPage.Search;
            button_Statistics.Click += (s, e) => Common.CurrentTabPage = Common.TabPage.Statistics;
            button_Validation.Click += (s, e) => Common.CurrentTabPage = Common.TabPage.Validation;
            button_About.Click += (s, e) => Common.CurrentTabPage = Common.TabPage.About;

            RoutedEventHandler switchThemeMode = (s, e) => Theme.IsDarkTheme = !Theme.IsDarkTheme;
            button_LightTheme.Click += switchThemeMode;
            button_DarkTheme.Click += switchThemeMode;

            Theme.IsDarkThemeChanged += (s, e) =>
            {
                button_LightTheme.Visibility = Theme.IsDarkTheme ? Visibility.Visible : Visibility.Collapsed;
                button_DarkTheme.Visibility = !Theme.IsDarkTheme ? Visibility.Visible : Visibility.Collapsed;
            };

            //

            Random random = new Random(Environment.TickCount);
            Theme.ThemeColor = ColorX.FromHSL(random.Next(360), random.Next(20, 60), 50);
            Theme.IsDarkTheme = false;
        }

        //

        #region 回调函数

        private void _CurrentTabPageChanged(object sender, Common.TabPage tabPage)
        {
            Common.IsEditMode = false;

            //

            ViewModel.TabPageButtonEnabled_File = tabPage != Common.TabPage.File;
            ViewModel.TabPageButtonEnabled_Evo = tabPage != Common.TabPage.Evo;
            ViewModel.TabPageButtonEnabled_Search = tabPage != Common.TabPage.Search;
            ViewModel.TabPageButtonEnabled_Statistics = tabPage != Common.TabPage.Statistics;
            ViewModel.TabPageButtonEnabled_Validation = tabPage != Common.TabPage.Validation;
            ViewModel.TabPageButtonEnabled_About = tabPage != Common.TabPage.About;

            ViewModel.TabPageVisibility_File = !ViewModel.TabPageButtonEnabled_File ? Visibility.Visible : Visibility.Collapsed;
            ViewModel.TabPageVisibility_Evo = !ViewModel.TabPageButtonEnabled_Evo ? Visibility.Visible : Visibility.Collapsed;
            ViewModel.TabPageVisibility_Search = !ViewModel.TabPageButtonEnabled_Search ? Visibility.Visible : Visibility.Collapsed;
            ViewModel.TabPageVisibility_Statistics = !ViewModel.TabPageButtonEnabled_Statistics ? Visibility.Visible : Visibility.Collapsed;
            ViewModel.TabPageVisibility_Validation = !ViewModel.TabPageButtonEnabled_Validation ? Visibility.Visible : Visibility.Collapsed;
            ViewModel.TabPageVisibility_About = !ViewModel.TabPageButtonEnabled_About ? Visibility.Visible : Visibility.Collapsed;
        }

        private void _IsEditModeChanged(object sender, bool editMode)
        {
            view_Evo_ViewMode.Visibility = !editMode ? Visibility.Visible : Visibility.Collapsed;
            view_Evo_EditMode.Visibility = editMode ? Visibility.Visible : Visibility.Collapsed;

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

                    Common.CurrentTaxon = parent;
                }
                else
                {
                    view_Evo_ViewMode.UpdateCurrentTaxonInfo();
                    view_Tree.UpdateSubTree();
                }
            }
        }

        private void _CurrentTaxonChanged(object sender, Taxon taxon)
        {
            // 防止选择已删除的类群。
            if (taxon.IsRoot && taxon != Entrance.Root)
            {
                throw new InvalidOperationException();
            }

            //

            if (Common.IsEditMode)
            {
                view_Evo_EditMode.UpdateCurrentTaxonInfo();
            }
            else
            {
                view_Evo_ViewMode.UpdateCurrentTaxonInfo();
            }

            view_Tree.UpdateSubTree();
        }

        #endregion

        #region 文件操作

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
                Common.CurrentTaxon = Entrance.Root;
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
                    // 当且仅当已经保存且文件存在，才认为不需要（重新）保存且保存成功
                    result = true;
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
                    // 当且仅当已经保存且文件存在，才认为不需要（重新）保存且保存成功
                    result = true;
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
            view_Statistics.ClearStatisticsResult();
            view_Validation.ClearValidateResult();

            bool result = await AsyncMethod.CloseAsync();

            if (result)
            {
                Common.CurrentTaxon = Entrance.Root;
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
            view_Statistics.ClearStatisticsResult();
            view_Validation.ClearValidateResult();

            bool result = Entrance.Close();

            if (result)
            {
                Common.CurrentTaxon = Entrance.Root;
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
                view_Statistics.ClearStatisticsResult();
                view_Validation.ClearValidateResult();

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
                view_Statistics.ClearStatisticsResult();
                view_Validation.ClearValidateResult();

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
    }
}