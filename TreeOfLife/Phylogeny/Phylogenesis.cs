/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.323.1000.M4.201128-1700

This file is part of "生命树" (TreeOfLife)

"生命树" (TreeOfLife) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TreeOfLife
{
    // 系统发生学。
    internal static class Phylogenesis
    {
        public const string FileVersionJsonPropertyName = "FileVersion";

        //

        private static PhylogeneticTree _PhylogeneticTree = null;

        private static string _FileName = null;

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

        public static string FileName => _FileName;

        //

        // 新建文件。
        public static bool New()
        {
            _PhylogeneticTree = new PhylogeneticTree();
            _FileName = null;

            return true;
        }

        private static int _CheckFileVersion(string fileName)
        {
            int fileVersion = 0;

            string jsonText = File.ReadAllText(fileName);

            using (JsonDocument document = JsonDocument.Parse(jsonText))
            {
                JsonElement element = document.RootElement.GetProperty(FileVersionJsonPropertyName);

                fileVersion = element.GetInt32();
            }

            return fileVersion;
        }

        // 打开文件。
        public static bool Open(string fileName)
        {
            bool result = true;

            _FileName = fileName;

            try
            {
                int fileVersion = _CheckFileVersion(_FileName);

                switch (fileVersion)
                {
                    case 1:
                        {
                            string jsonText = File.ReadAllText(fileName);

                            JsonSerializerOptions options = new JsonSerializerOptions();
                            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

                            PhylogeneticUnwindV1 unwindObject = JsonSerializer.Deserialize<PhylogeneticUnwindV1>(jsonText, options);

                            _PhylogeneticTree = unwindObject.Rebuild();

                            result = true;
                        }
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
                PhylogeneticUnwindV1 unwindObject = _PhylogeneticTree.Unwind();

                JsonSerializerOptions options = new JsonSerializerOptions();
                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                options.WriteIndented = true;

                string jsonText = JsonSerializer.Serialize(unwindObject, options);

                File.WriteAllText(fileName, jsonText);

                result = true;
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
            return New();
        }
    }
}