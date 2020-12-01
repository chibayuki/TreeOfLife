/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.400.1000.M5.201129-0000

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
        private static PhylogeneticTree _PhylogeneticTree = null;

        private static string _FileName = null;

        private static string _TempDir = null; // 临时目录。
        private static string _WorkingDir => Path.Combine(_TempDir, "workDir"); // 工作目录。
        private static string _VersionInfoFile => Path.Combine(_WorkingDir, "_version"); // 版本信息文件。

        // 版本信息。
        private class _VersionInfo
        {
            private int _FileVersion = 1;

            //

            public _VersionInfo()
            {
            }

            //

            [JsonPropertyName("FileVersion")]
            public int FileVersion
            {
                get => _FileVersion;
                set => _FileVersion = value;
            }
        }

        //

        public static Taxon Root
        {
            get
            {
                if (_PhylogeneticTree is null)
                {
                    throw new InvalidOperationException();
                }

                //

                return _PhylogeneticTree.Root;
            }
        }

        public static bool IsEmpty
        {
            get
            {
                if (_PhylogeneticTree is null)
                {
                    throw new InvalidOperationException();
                }

                //

                return _PhylogeneticTree.Root.Children.Count <= 0;
            }
        }

        public static string FileName => _FileName;

        //

        // 解压文件。
        private static void _Extract(string sourceArchiveFileName, string destinationDirectoryName)
        {
            if (!Directory.Exists(destinationDirectoryName))
            {
                Directory.CreateDirectory(destinationDirectoryName);
            }

            ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
        }

        // 压缩文件。
        private static void _Compress(string sourceDirectoryName, string destinationArchiveFileName)
        {
            string dir = Path.GetDirectoryName(destinationArchiveFileName);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string tmpFile = Path.Combine(dir, Path.GetRandomFileName() + ".tmp");

            ZipFile.CreateFromDirectory(sourceDirectoryName, tmpFile, CompressionLevel.Optimal, false);

            if (File.Exists(destinationArchiveFileName))
            {
                File.Delete(destinationArchiveFileName);
            }

            File.Move(tmpFile, destinationArchiveFileName);
        }

        // 检查文件版本。
        private static int _ReadFileVersion(string fileName)
        {
            string jsonText = File.ReadAllText(fileName);

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            _VersionInfo versionInfo = JsonSerializer.Deserialize<_VersionInfo>(jsonText, options);

            return versionInfo.FileVersion;
        }

        // 写入文件版本。
        private static void _WriteFileVersion(string fileName)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;

            string jsonText = JsonSerializer.Serialize(new _VersionInfo(), options);

            File.WriteAllText(fileName, jsonText);
        }

        //

        // 新建文件。
        public static bool New()
        {
            bool result = true;

            _PhylogeneticTree = new PhylogeneticTree();

            _FileName = null;

            try
            {
                _TempDir = Path.Combine(Path.GetTempPath(), string.Concat(Application.ProductName, "_", Application.ProductVersion), Path.GetRandomFileName());
            }
            catch
            {
                _TempDir = null;

#if DEBUG
                throw;
#else
                result = false;
#endif
            }

            return result;
        }

        // 打开文件。
        public static bool Open(string fileName)
        {
            bool result = true;

            _FileName = fileName;

            try
            {
                _TempDir = Path.Combine(Path.GetTempPath(), string.Concat(Application.ProductName, "_", Application.ProductVersion), Path.GetRandomFileName());

                _Extract(_FileName, _WorkingDir);

                int fileVersion = _ReadFileVersion(_VersionInfoFile);

                switch (fileVersion)
                {
                    case 1:
                        _PhylogeneticTree = PhylogeneticUnwindV1.Deserialize(_WorkingDir).Rebuild();
                        break;

                    default:
                        result = false;
                        break;
                }
            }
            catch
            {
                _PhylogeneticTree = new PhylogeneticTree();

                _FileName = null;
                _TempDir = null;

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
            return SaveAs(_FileName);
        }

        // 另存为文件。
        public static bool SaveAs(string fileName)
        {
            bool result = true;

            _FileName = fileName;

            try
            {
                _WriteFileVersion(_VersionInfoFile);
                _PhylogeneticTree.Unwind().Serialize(_WorkingDir);
                _Compress(_WorkingDir, _FileName);
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
                if (Directory.Exists(_TempDir))
                {
                    Directory.Delete(_TempDir, true);
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