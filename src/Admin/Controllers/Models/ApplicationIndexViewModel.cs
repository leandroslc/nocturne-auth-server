// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using Nocturne.Auth.Core.Modules.Applications.Services;
using Nocturne.Auth.Core.Shared.Collections;

namespace Nocturne.Auth.Admin.Controllers.Models
{
    public class ApplicationIndexViewModel
    {
        public ApplicationIndexViewModel(
            IPagedCollection<ListApplicationsResult> applications,
            ListApplicationsCommand query)
        {
            Applications = applications;
            Query = query;
        }

        public IPagedCollection<ListApplicationsResult> Applications { get; }

        public ListApplicationsCommand Query { get; }
    }
}
