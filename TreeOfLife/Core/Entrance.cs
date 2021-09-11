﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1240.1000.M12.210718-2000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using TreeOfLife.Core.Packaging;
using TreeOfLife.Core.Taxonomy;

namespace TreeOfLife.Core
{
    // 提供内存数据访问与文件操作的单例。
    public static class Entrance
    {
        private static PhylogeneticTree _PhylogeneticTree = null;

        private static Package _Package = null;

        //

        public static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name;

        public static Taxon Root => _PhylogeneticTree?.Root;

        public static bool IsEmpty => _PhylogeneticTree is null || _PhylogeneticTree.Root.IsFinal;

        public static string FileName => _Package?.FileName;

        public static long PackageSize => _Package is null ? 0 : _Package.PackageSize;

        public static int PackageVersion => _Package is null ? 0 : _Package.Version.Version;

        public static DateTime CreationTime => _Package is null ? DateTime.MinValue : _Package.Info.CreationTime;

        public static DateTime ModificationTime => _Package is null ? DateTime.MinValue : _Package.Info.ModificationTime;

        //

        // 新建文件。
        public static bool New()
        {
            _PhylogeneticTree = new PhylogeneticTree();

            _Package = Package.CreateNew();

            return true;
        }

        // 打开文件。
        public static bool Open(string fileName)
        {
            bool result = true;

#if !DEBUG
            try
#endif
            {
                _Package = Package.OpenFromFile(fileName, out IPackageContent packageContent);

                _PhylogeneticTree = new PhylogeneticTree();

                packageContent.TranslateTo(_PhylogeneticTree);
            }
#if !DEBUG
            catch
            {
                _PhylogeneticTree = new PhylogeneticTree();

                _Package?.Close();
                _Package = Package.CreateNew();

                result = false;
            }
#endif

            return result;
        }

        // 保存文件。
        public static bool Save()
        {
            bool result = true;

#if !DEBUG
            try
#endif
            {
                IPackageContent packageContent = PackageContentVersions.CreateLatestVersion();

                packageContent.TranslateFrom(_PhylogeneticTree);

                _Package.SaveToFile(packageContent);
            }
#if !DEBUG
            catch
            {
                result = false;
            }
#endif

            return result;
        }

        // 另存为文件。
        public static bool SaveAs(string fileName)
        {
            bool result = true;

#if !DEBUG
            try
#endif
            {
                IPackageContent packageContent = PackageContentVersions.CreateLatestVersion();

                packageContent.TranslateFrom(_PhylogeneticTree);

                _Package.SaveToFile(packageContent, fileName);
            }
#if !DEBUG
         catch
            {
                result = false;
            }
#endif

            return result;
        }

        // 关闭文件。
        public static bool Close()
        {
            bool result = true;

#if !DEBUG
            try
#endif
            {
                _Package?.Close();

                result = New();
            }
#if !DEBUG
           catch
            {
                result = false;
            }
#endif

            return result;
        }
    }
}