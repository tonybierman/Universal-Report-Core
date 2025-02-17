using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UniversalReportCore
{
    /// <summary>
    /// Represents a paginated list of items with support for metadata, aggregation, and mapping.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated list.</typeparam>
    public class PaginatedList<T> : List<T>, IPaginatedList
    {
        // Current page index
        public int PageIndex { get; private set; }

        // Total number of pages
        public int TotalPages { get; private set; }

        // Total number of items across all pages
        public int TotalItems { get; private set; }

        // Number of items per page
        public int PageSize { get; private set; }

        // Aggregate values (e.g., SUM, AVG) calculated from the source data
        public Dictionary<string, dynamic>? Aggregates { get; private set; }

        // Additional metadata associated with the paginated list
        public Dictionary<string, dynamic>? Meta { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedList{T}"/> class.
        /// </summary>
        /// <param name="items">The items for the current page.</param>
        /// <param name="count">The total number of items across all pages.</param>
        /// <param name="pageIndex">The current page index.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="aggregates">Optional aggregate values (e.g., SUM, AVG).</param>
        /// <param name="meta">Optional metadata for the list.</param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, Dictionary<string, dynamic>? aggregates = null, Dictionary<string, dynamic>? meta = null)
        {
            PageIndex = pageIndex;
            TotalItems = count;
            PageSize = pageSize;
            Aggregates = aggregates;
            Meta = meta;

            // Calculate the total number of pages. If pageSize is 0, everything is on one page.
            TotalPages = pageSize > 0 ? (int)Math.Ceiling(count / (double)pageSize) : 1;

            AddRange(items);  // Add the items to the paginated list
        }

        /// <summary>
        /// Gets the total number of items in the current page.
        /// </summary>
        public new int Count => base.Count;

        /// <summary>
        /// Checks if the paginated list contains any items.
        /// </summary>
        /// <returns>True if the list has at least one item; otherwise, false.</returns>
        public bool Any() => Count > 0;

        /// <summary>
        /// Indicates if there are multiple pages.
        /// </summary>
        public bool HasMultiplePages => PageSize > 0 && TotalItems > PageSize;

        /// <summary>
        /// Indicates if there is a previous page.
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// Indicates if there is a next page.
        /// </summary>
        public bool HasNextPage => PageSize > 0 && PageIndex < TotalPages;

        /// <summary>
        /// Gets the starting item number for the current page.
        /// </summary>
        public int StartItem => PageSize > 0 ? (PageIndex - 1) * PageSize + 1 : 1;

        /// <summary>
        /// Gets the ending item number for the current page.
        /// </summary>
        public int EndItem => PageSize > 0 ? Math.Min(PageIndex * PageSize, TotalItems) : TotalItems;

        /// <summary>
        /// Gets a display message summarizing the current page items.
        /// </summary>
        public string DisplayMessage => $"Showing items {StartItem} through {EndItem} of {TotalItems}";

        /// <summary>
        /// Returns an enumerator for the paginated list.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Creates a paginated list from an IQueryable source.
        /// </summary>
        /// <param name="source">The source query.</param>
        /// <param name="pageIndex">The current page index.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list.</returns>
        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();  // Count the total number of items
            var items = pageSize > 0
                ? await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
                : await source.ToListAsync();  // Fetch items for the current page

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        /// <summary>
        /// Creates a paginated list with items mapped from a source type to a result type.
        /// </summary>
        /// <typeparam name="TSource">The type of the source items.</typeparam>
        /// <typeparam name="TResult">The type of the mapped items.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="pageIndex">The current page index.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="mapFunction">A function to map each source item to the result type.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list.</returns>
        public static async Task<PaginatedList<TResult>> CreateMappedAsync<TSource, TResult>(
            IQueryable<TSource> source,
            int pageIndex,
            int pageSize,
            Func<TSource, TResult> mapFunction)
        {
            var count = await source.CountAsync();
            var items = pageSize > 0
                ? await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
                : await source.ToListAsync();

            var mappedItems = items.Select(mapFunction).ToList();  // Map each item to the result type

            return new PaginatedList<TResult>(mappedItems, count, pageIndex, pageSize);
        }

        /// <summary>
        /// Creates a paginated list with aggregated values (e.g., SUM, AVG).
        /// </summary>
        public static async Task<PaginatedList<T>> CreateWithAggregatesAsync(
            IQueryable<T> source, int pageIndex, int pageSize,
            Func<IQueryable<T>, Task<Dictionary<string, dynamic>>> aggregateFunction,
            Func<IQueryable<T>, Task<Dictionary<string, dynamic>>>? metaFunction = null)
        {
            var count = await source.CountAsync();
            var items = pageSize > 0
                ? await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
                : await source.ToListAsync();

            var aggregates = await aggregateFunction(source);  // Compute aggregates (e.g., total sum)
            var meta = metaFunction == null ? null : await metaFunction(source);

            return new PaginatedList<T>(items, count, pageIndex, pageSize, aggregates, meta);
        }

        /// <summary>
        /// Creates a paginated list with mapped items and aggregates.
        /// </summary>
        public static async Task<PaginatedList<TResult>> CreateMappedWithAggregatesAsync<TSource, TResult>(
            IQueryable<TSource> source, int pageIndex, int pageSize, Func<TSource, TResult> mapFunction,
            Func<IQueryable<TSource>, Task<Dictionary<string, dynamic>>> aggregateFunction,
            Func<IQueryable<TSource>, Task<Dictionary<string, dynamic>>>? metaFunction = null)
        {
            var count = await source.CountAsync();
            var items = pageSize > 0
                ? await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
                : await source.ToListAsync();

            var mappedItems = items.Select(mapFunction).ToList();
            var aggregates = await aggregateFunction(source);
            var meta = metaFunction == null ? null : await metaFunction(source);

            return new PaginatedList<TResult>(mappedItems, count, pageIndex, pageSize, aggregates, meta);
        }
    }
}
