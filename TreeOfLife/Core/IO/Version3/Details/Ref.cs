/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TreeOfLife.Core.IO.Version3.Details
{
    // 系统发生树展开的表示引用关系的数据结构，包含所有并系群、复系群对其他类群的排除或包含关系。
    public sealed class Ref
    {
        private List<RefAtom> _Atoms = new List<RefAtom>();

        //

        public Ref()
        {
        }

        //

        [JsonPropertyName("Relations")]
        public List<RefAtom> Atoms
        {
            get => _Atoms;
            set => _Atoms = value;
        }

        //

        // 序列化。
        public void Serialize(string fileName)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            string jsonText = JsonSerializer.Serialize(this, options);

            File.WriteAllText(fileName, jsonText);
        }

        // 反序列化。
        public static Ref Deserialize(string fileName)
        {
            string jsonText = File.ReadAllText(fileName);

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            return JsonSerializer.Deserialize<Ref>(jsonText, options);
        }
    }
}