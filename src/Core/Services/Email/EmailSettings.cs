// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Globalization;

namespace Nocturne.Auth.Core.Services.Email
{
    public sealed class EmailSettings
    {
        public EmailSettings(
            string templatesPath,
            string templateExtension)
        {
            TemplatesPath = templatesPath;
            TemplateExtension = templateExtension;
        }

        public string TemplatesPath { get; }

        public string TemplateExtension { get; }

        public string GetExactTemplateFilePath(string templateName)
        {
            const string templateNameFormat = "{0}.{1}";

            string filePath = string.Format(
                CultureInfo.InvariantCulture,
                templateNameFormat,
                templateName,
                TemplateExtension);

            return Path.Combine(TemplatesPath, filePath);
        }

        public string FindTemplateFile(string templateName, string culture)
        {
            var files = Directory.GetFiles(TemplatesPath, $"{templateName}.{culture}*.{TemplateExtension}");

            return files.FirstOrDefault();
        }
    }
}
