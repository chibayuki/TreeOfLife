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

using TreeOfLife.Packaging;
using TreeOfLife.Taxonomy;

namespace TreeOfLife.Phylogeny
{
    // 系统发生学。
    public static class Phylogenesis
    {
        private static PhylogeneticTree _PhylogeneticTree = null;

        private static Package _Package = null;

        //

        public static Taxon Root => _PhylogeneticTree?.Root;

        public static bool IsEmpty => (_PhylogeneticTree == null || _PhylogeneticTree.Root.IsFinal);

        public static string FileName => _Package?.FileName;

        public static long PackageSize => (_Package == null ? 0 : _Package.PackageSize);

        public static int PackageVersion => (_Package == null ? 0 : _Package.Version.Version);

        public static DateTime CreationTime => (_Package == null ? DateTime.MinValue : _Package.Info.CreationTime);

        public static DateTime ModificationTime => (_Package == null ? DateTime.MinValue : _Package.Info.ModificationTime);

        //

        // 新建文件。
        public static bool New()
        {
            _PhylogeneticTree = new PhylogeneticTree();

            _Package = Package.Create();

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
                _Package = Package.Open(fileName, out IPackageContent packageContent);

                _PhylogeneticTree = new PhylogeneticTree();

                packageContent.TranslateTo(_PhylogeneticTree);
            }
#if !DEBUG
            catch
            {
                _PhylogeneticTree = new PhylogeneticTree();

                _Package?.Close();
                _Package = Package.Create();

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

                _Package.Save(packageContent);
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

                _Package.SaveAs(packageContent, fileName);
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