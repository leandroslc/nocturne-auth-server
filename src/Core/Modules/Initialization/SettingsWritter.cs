// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Nocturne.Auth.Core.Modules.Initialization
{
    public class SettingsWritter
    {
        private readonly string settingsFile;
        private readonly JsonNode rootNode;

        public SettingsWritter(string filepath)
        {
            settingsFile = filepath;
            rootNode = ParseJson();
        }

        public void Set(string property, JsonObject value)
        {
            rootNode[property] = value;
        }

        public void Write()
        {
            var options = new JsonWriterOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Indented = true,
            };

            using var writeStream = File.Open(
                settingsFile,
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.None);

            using var writer = new Utf8JsonWriter(writeStream, options);

            rootNode.WriteTo(writer);
        }

        private JsonNode ParseJson()
        {
            if (!File.Exists(settingsFile))
            {
                return new JsonObject();
            }

            using var readFileStream = File.Open(
                settingsFile,
                FileMode.Open,
                FileAccess.Read,
                FileShare.None);

            return JsonNode.Parse(readFileStream);
        }
    }
}
