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

namespace TreeOfLife
{
    // 版本信息。
    public class VersionInfo
    {
        private int _FileVersion = 1; // 文件版本。

        //

        public VersionInfo()
        {
        }

        public VersionInfo(int fileVersion)
        {
            _FileVersion = fileVersion;
        }

        //

        [JsonPropertyName("FileVersion")]
        public int FileVersion
        {
            get => _FileVersion;
            set => _FileVersion = value;
        }
    }

    // 文件信息。
    public class FileInfo
    {
        private DateTime _CreationTime; // 创建时间。
        private DateTime _ModificationTime; // 创建时间。

        //

        public FileInfo()
        {
        }

        public FileInfo(DateTime creationTime)
        {
            _CreationTime = creationTime;
        }

        //

        [JsonPropertyName("CreationTime")]
        public DateTime CreationTime
        {
            get => _CreationTime;
            set => _CreationTime = value;
        }

        [JsonPropertyName("ModificationTime")]
        public DateTime ModificationTime
        {
            get => _ModificationTime;
            set => _ModificationTime = value;
        }
    }

    // 容器。
    public class Package
    {
        private const int _LatestVersion = 1; // 最新的文件版本/展开版本。

        //

        private bool _Opened = false; // 文件是否已经作为容器打开。

        private string _FileName = null; // 文件名。

        private string _TempDir = null; // 临时目录。
        private string _PackageDir = null; // 容器目录。
        private string _VersionInfoFileName = null; // 版本信息文件。
        private string _InfoDir = null; // 文件信息目录。
        private string _FileInfoFileName = null; // 文件信息文件。
        private string _WorkingDir = null; // 工作目录。

        private VersionInfo _VersionInfo = null; // 版本信息。
        private FileInfo _FileInfo = null; // 文件信息。

        private void _InitDirs()
        {
            _TempDir = Path.Combine(Path.GetTempPath(), "TreeOfLife", Path.GetRandomFileName());
            _PackageDir = Path.Combine(_TempDir, "package");
            _VersionInfoFileName = Path.Combine(_PackageDir, "_version");
            _InfoDir = Path.Combine(_PackageDir, "info");
            _FileInfoFileName = Path.Combine(_InfoDir, "_fileinfo");
            _WorkingDir = Path.Combine(_PackageDir, "data");
        }

        //

        public Package(string fileName)
        {
            _FileName = fileName;

            _FileInfo = new FileInfo(DateTime.UtcNow);
        }

        //

        public bool Opened => _Opened;

        public string FileName => _FileName;

        public string WorkingDir => _WorkingDir;

        public VersionInfo VersionInfo => _VersionInfo;

        public FileInfo FileInfo => _FileInfo;

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

        // 读取版本信息。
        private static VersionInfo _ReadVersionInfo(string fileName)
        {
            string jsonText = File.ReadAllText(fileName);

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            return JsonSerializer.Deserialize<VersionInfo>(jsonText, options);
        }

        // 写入版本信息。
        private static void _WriteVersionInfo(string fileName, VersionInfo versionInfo)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;

            string jsonText = JsonSerializer.Serialize(versionInfo, options);

            File.WriteAllText(fileName, jsonText);
        }

        // 读取文件信息。
        private static FileInfo _ReadFileInfo(string fileName)
        {
            string jsonText = File.ReadAllText(fileName);

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            return JsonSerializer.Deserialize<FileInfo>(jsonText, options);
        }

        // 写入文件信息。
        private static void _WriteFileInfo(string fileName, FileInfo fileInfo)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;

            string jsonText = JsonSerializer.Serialize(fileInfo, options);

            File.WriteAllText(fileName, jsonText);
        }

        //

        // 打开。
        public void Open()
        {
            if (_Opened)
            {
                throw new InvalidOperationException();
            }

            //

            _Opened = true;

            _InitDirs();

            _Extract(_FileName, _PackageDir);

            _VersionInfo = _ReadVersionInfo(_VersionInfoFileName);
            _FileInfo = _ReadFileInfo(_FileInfoFileName);
        }

        // 保存。
        public void Save()
        {
            if (!_Opened)
            {
                _Opened = true;

                _InitDirs();
            }

            _VersionInfo = new VersionInfo(_LatestVersion);
            _WriteVersionInfo(_VersionInfoFileName, _VersionInfo);

            if (!Directory.Exists(_InfoDir))
            {
                Directory.CreateDirectory(_InfoDir);
            }

            _FileInfo.ModificationTime = DateTime.UtcNow;
            _WriteFileInfo(_FileInfoFileName, _FileInfo);

            _Compress(_PackageDir, _FileName);
        }

        // 另存为。
        public void SaveAs(string fileName)
        {
            if (_FileName != fileName)
            {
                _FileName = fileName;

                _FileInfo.CreationTime = DateTime.UtcNow;
            }

            if (!_Opened)
            {
                _Opened = true;

                _InitDirs();
            }

            _VersionInfo = new VersionInfo(_LatestVersion);
            _WriteVersionInfo(_VersionInfoFileName, _VersionInfo);

            if (!Directory.Exists(_InfoDir))
            {
                Directory.CreateDirectory(_InfoDir);
            }

            _FileInfo.ModificationTime = DateTime.UtcNow;
            _WriteFileInfo(_FileInfoFileName, _FileInfo);

            _Compress(_PackageDir, _FileName);
        }

        // 关闭。
        public void Close()
        {
            if (_Opened)
            {
                _Opened = false;

                try
                {
                    if (Directory.Exists(_TempDir))
                    {
                        Directory.Delete(_TempDir, true);
                    }
                }
                catch { }

                _TempDir = null;
                _PackageDir = null;
                _VersionInfoFileName = null;
                _InfoDir = null;
                _FileInfoFileName = null;
                _WorkingDir = null;

                _VersionInfo = null;
            }
        }
    }
}