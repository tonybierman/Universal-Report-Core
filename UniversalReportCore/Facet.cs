﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    /// <summary>
    /// Represents a named facet containing multiple filterable values.
    /// </summary>
    /// <typeparam name="T">The type of entity being filtered.</typeparam>
    public class Facet<T>
    {
        /// <summary>
        /// Gets the name of the facet, representing a category of filterable values.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the list of values associated with this facet.
        /// </summary>
        public List<FacetValue<T>> Values { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Facet{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the facet (e.g., "Category", "Status").</param>
        /// <param name="values">A list of filterable values associated with this facet.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> or <paramref name="values"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="values"/> contains duplicate keys.</exception>
        public Facet(string name, List<FacetValue<T>> values)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (values == null) throw new ArgumentNullException(nameof(values));

            var duplicateKeys = values
                .GroupBy(v => v.Key)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateKeys.Any())
            {
                throw new ArgumentException($"Duplicate FacetValue keys found: {string.Join(", ", duplicateKeys)}", nameof(values));
            }

            Name = name;
            Values = values;
        }
    }
}
