﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using System.IO;
using System.IO.Compression;
using System.Text.Json;

namespace TreeOfLife.Core.IO
{
    // 文件封装。
    public sealed class Package
    {
        private bool _Closed = false; // 文件是否已关闭。

        private string _UserFile = null; // 用户文件。

        private string _TempDir = null; // 存放此文件封装的临时目录（Temp）。
        private string _PackageDir = null; // 文件封装所在目录（Temp\package\_version）。
        private string _VersionFile = null; // 文件封装版本文件（Temp\package）。
        private string _InfoDir = null; // 文件封装信息所在目录（Temp\package\info）。
        private string _InfoFile = null; // 文件封装信息文件（Temp\package\info\_info）。
        private string _PackageContentDir = null; // 文件封装内容所在目录（Temp\package\data）。

        private PackageVersion _Version; // 文件封装版本。
        private PackageInfo _Info; // 文件封装信息。

        private void _InitDirs()
        {
            _TempDir = Path.Combine(Path.GetTempPath(), Entrance.AppName, Guid.NewGuid().ToString().ToUpperInvariant());
            _PackageDir = Path.Combine(_TempDir, "package");
            _VersionFile = Path.Combine(_PackageDir, "_version");
            _InfoDir = Path.Combine(_PackageDir, "info");
            _InfoFile = Path.Combine(_InfoDir, "_info");
            _PackageContentDir = Path.Combine(_PackageDir, "data");
        }

        private void _TryCreateDirs()
        {
            if (!Directory.Exists(_InfoDir))
            {
                Directory.CreateDirectory(_InfoDir);
            }

            if (!Directory.Exists(_PackageContentDir))
            {
                Directory.CreateDirectory(_PackageContentDir);
            }
        }

        //

        private Package()
        {
            _InitDirs();

            _Info = new PackageInfo(DateTime.UtcNow);
        }

        private Package(string fileName) : this()
        {
            _UserFile = fileName;
        }

        //

        public string UserFile => _UserFile;

        public long PackageSize
        {
            get
            {
                if (Directory.Exists(_PackageDir))
                {
                    return _GetDirSize(new DirectoryInfo(_PackageDir));
                }
                else
                {
                    return 0;
                }
            }
        }

        public PackageVersion Version => _Version;

        public PackageInfo Info => _Info;

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

            string fileName = Path.GetFileNameWithoutExtension(destinationArchiveFileName);
            string randomFileName = Path.GetRandomFileName();
            string tmpFile = Path.Combine(dir, $"{(fileName.Length > 8 ? fileName[..8] : fileName)}~{(randomFileName.Length > 3 ? randomFileName[..3] : randomFileName)}.tmp");

            // 先压缩到用户文件同目录下的临时文件，压缩成功之后，再更新用户文件
            ZipFile.CreateFromDirectory(sourceDirectoryName, tmpFile, CompressionLevel.Optimal, false);

            File.Move(tmpFile, destinationArchiveFileName, true);
        }

        // 读取文件封装版本。
        private static PackageVersion _ReadPackageVersion(string fileName)
        {
            string jsonText = File.ReadAllText(fileName);

            JsonSerializerOptions options = new JsonSerializerOptions();

            return JsonSerializer.Deserialize<PackageVersion>(jsonText, options);
        }

        // 写入文件封装版本。
        private static void _WritePackageVersion(string fileName, PackageVersion versionInfo)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };

            string jsonText = JsonSerializer.Serialize(versionInfo, options);

            File.WriteAllText(fileName, jsonText);
        }

        // 读取文件封装信息。
        private static PackageInfo _ReadPackageInfo(string fileName)
        {
            string jsonText = File.ReadAllText(fileName);

            JsonSerializerOptions options = new JsonSerializerOptions();

            return JsonSerializer.Deserialize<PackageInfo>(jsonText, options);
        }

        // 写入文件封装信息。
        private static void _WritePackageInfo(string fileName, PackageInfo fileInfo)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };

            string jsonText = JsonSerializer.Serialize(fileInfo, options);

            File.WriteAllText(fileName, jsonText);
        }

        // 获取目录大小。
        private static long _GetDirSize(DirectoryInfo dirInfo)
        {
            long size = 0;

            FileInfo[] fileInfos = dirInfo.GetFiles();

            foreach (var fi in fileInfos)
            {
                size += fi.Length;
            }

            DirectoryInfo[] dirInfos = dirInfo.GetDirectories();

            foreach (var di in dirInfos)
            {
                size += _GetDirSize(di);
            }

            return size;
        }

        //

        // 创建新对象。
        public static Package CreateNew()
        {
            return new Package();
        }

        // 从文件打开。
        public static Package OpenFromFile(string fileName, out IPackageContent packageContent)
        {
            if (fileName is null)
            {
                throw new ArgumentNullException();
            }

            //

            Package package = new Package(fileName);

            _Extract(package._UserFile, package._PackageDir);

            package._Version = _ReadPackageVersion(package._VersionFile);
            package._Info = _ReadPackageInfo(package._InfoFile);

            packageContent = PackageContentVersions.CreateFromVersion(package._Version);

            packageContent.Deserialize(package._PackageContentDir);

            return package;
        }

        // 保存到当前关联的文件。
        public void SaveToFile(IPackageContent packageContent)
        {
            if (_Closed)
            {
                throw new InvalidOperationException();
            }

            if (_UserFile is null)
            {
                throw new ArgumentNullException();
            }

            //

            _TryCreateDirs();

            _Version = packageContent.Version;
            _WritePackageVersion(_VersionFile, _Version);

            _Info.ModificationTime = DateTime.UtcNow;
            _WritePackageInfo(_InfoFile, _Info);

            packageContent.Serialize(_PackageContentDir);

            _Compress(_PackageDir, _UserFile);
        }

        // 保存到其他文件。
        public void SaveToFile(IPackageContent packageContent, string fileName)
        {
            if (_Closed)
            {
                throw new InvalidOperationException();
            }

            if (fileName is null)
            {
                throw new ArgumentNullException();
            }

            //

            _UserFile = fileName;

            SaveToFile(packageContent);
        }

        // 关闭并不再使用此对象。
        public void Close()
        {
            if (_Closed)
            {
                throw new InvalidOperationException();
            }

            //

            _Closed = true;

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
            _VersionFile = null;
            _InfoDir = null;
            _InfoFile = null;
            _PackageContentDir = null;
        }
    }
}