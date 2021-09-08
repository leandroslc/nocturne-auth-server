// Copyright (c) Leandro Silva Luz do Carmo
// SPDX-License-Identifier: GPL-3.0-or-later

using System;
using System.Collections.Generic;

namespace Nocturne.Auth.Core.Shared.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="HashSet{T}" /> class
    /// </summary>
    public static class HashSetExtensions
    {
        /// <summary>
        /// Adds the specified sequence of elements to the set
        /// </summary>
        /// <param name="source">The current set</param>
        /// <param name="itens">The elements to be added</param>
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

        /// <summary>
        /// Removes the specified sequence of elements from the set
        /// </summary>
        /// <param name="source">The current set</param>
        /// <param name="itens">The elements to be removed</param>
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
