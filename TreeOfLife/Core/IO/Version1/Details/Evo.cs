/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TreeOfLife.Core.IO.Version1.Details
{
    // 系统发生树展开的表示演化关系的数据结构，包含所有单系群。
    public sealed class Evo
    {
        private List<EvoAtom> _Atoms = new List<EvoAtom>();

        //

        public Evo()
        {
        }

        //

        [JsonPropertyName("Taxons")]
        public List<EvoAtom> Atoms
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
        public static Evo Deserialize(string fileName)
        {
            string jsonText = File.ReadAllText(fileName);

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            return JsonSerializer.Deserialize<Evo>(jsonText, options);
        }
    }
}