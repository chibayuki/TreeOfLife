/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.415.1000.M5.201204-2200

This file is part of "生命树" (TreeOfLife)

"生命树" (TreeOfLife) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace TreeOfLife
{
    // 系统发生学。
    internal static class Phylogenesis
    {
        private static PhylogeneticTree _PhylogeneticTree = new PhylogeneticTree();

        private static Package _Package = null;

        //

        public static Taxon Root => _PhylogeneticTree.Root;

        public static bool IsEmpty => _PhylogeneticTree.Root.IsFinal;

        public static string FileName => (_Package is null ? null : _Package.FileName);

        //

        // 新建文件。
        public static bool New()
        {
            _PhylogeneticTree = new PhylogeneticTree();

            _Package = null;

            return true;
        }

        // 打开文件。
        public static bool Open(string fileName)
        {
            bool result = true;

            try
            {
                _Package = new Package(fileName);
                _Package.Open();

                switch (_Package.VersionInfo.FileVersion)
                {
                    case 1:
                        _PhylogeneticTree = PhylogeneticUnwindV1.Deserialize(_Package.WorkingDir).Rebuild();
                        break;

                    default:
                        result = false;
                        break;
                }
            }
            catch
            {
                _PhylogeneticTree = new PhylogeneticTree();

                _Package.Close();
                _Package = null;

#if DEBUG
                throw;
#else
                result = false;
#endif
            }

            return result;
        }

        // 保存文件。
        public static bool Save()
        {
            bool result = true;

            try
            {
                _PhylogeneticTree.Unwind().Serialize(_Package.WorkingDir);

                _Package.Save();
            }
            catch
            {
#if DEBUG
                throw;
#else
                result = false;
#endif
            }

            return result;
        }

        // 另存为文件。
        public static bool SaveAs(string fileName)
        {
            bool result = true;

            try
            {
                _PhylogeneticTree.Unwind().Serialize(_Package.WorkingDir);

                _Package.SaveAs(fileName);
            }
            catch
            {
#if DEBUG
                throw;
#else
                result = false;
#endif
            }

            return result;
        }

        // 关闭文件。
        public static bool Close()
        {
            bool result = true;

            try
            {
                if (!(_Package is null))
                {
                    _Package.Close();
                }

                result = New();
            }
            catch
            {
#if DEBUG
                throw;
#else
                result = false;
#endif
            }

            return result;
        }
    }
}