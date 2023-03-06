// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

namespace Nocturne.Auth.Core.Shared.Extensions
{
    public static class HashSetExtensions
    {
        public static void AddRange<T>(
            this HashSet<T> source,
            IEnumerable<T> itens)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (itens is null)
            {
                return;
            }

            foreach (var item in itens)
            {
                source.Add(item);
            }
        }

        public static void RemoveRange<T>(
            this HashSet<T> source,
            IEnumerable<T> itens)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (itens is null)
            {
                return;
            }

            foreach (var item in itens)
            {
                source.Remove(item);
            }
        }
    }
}
