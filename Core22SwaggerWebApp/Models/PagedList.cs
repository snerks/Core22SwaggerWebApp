using System;
using System.Collections.Generic;
using System.Linq;

namespace Core22SwaggerWebApp.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    namespace Core22SwaggerWebApp.Models
    {
        public class PagingOptions
        {
            public const int MaximinumPageSize = 100;
            public const string MaximinumPageSizeText = "100";

            public const string MaximinumPageSizeMessage = 
                "PageSize must be greater than 0 and less than " + MaximinumPageSizeText + ".";

            [FromQuery]
            [Range(1, int.MaxValue, ErrorMessage = "Offset must be greater than 0.")]
            [Display(Name = "pageNumber")]
            [JsonProperty(PropertyName = "pageNumber")]
            public int? PageNumber { get; set; }

            [FromQuery]
            [Range(1, MaximinumPageSize, ErrorMessage = MaximinumPageSizeMessage)]
            [Display(Name = "pageSize")]
            [JsonProperty(PropertyName = "pageNumber")]
            public int? PageSize { get; set; }
        }
    }

    public class PagedList<T>
    {
        public PagedList(
            IQueryable<T> itemsQuery,
            int? pageNumberRequested,
            int? pageSizeRequested,
            int maximumPageSize = 100 /* Server-side limit */)
        {
            ItemsQuery = itemsQuery ?? throw new ArgumentNullException(nameof(itemsQuery));

            PageNumberRequested = pageNumberRequested;
            PageSizeRequested = pageSizeRequested;

            // >= 1
            var pageNumberNormalised = Math.Max(pageNumberRequested ?? 1, 1);

            // >= 1
            var maximumPageSizeNormalised = Math.Max(maximumPageSize, 1);

            // >= 1
            var pageSizeLowerLimit = Math.Max(pageSizeRequested ?? 10, 1);

            // <= maximumPageSizeNormalised
            var pageSizeNormalised = Math.Min(pageSizeLowerLimit, maximumPageSizeNormalised);

            PageNumber = pageNumberNormalised;
            PageSize = pageSizeNormalised;
            MaximumPageSize = maximumPageSizeNormalised;
        }

        private IQueryable<T> ItemsQuery { get; } = new List<T>().AsQueryable();

        public int? PageNumberRequested { get; }
        public int? PageSizeRequested { get; }

        public int PageNumber { get; } = 1;
        public int PageSize { get; } = 10;
        public int MaximumPageSize { get; } = 1;

        public IEnumerable<T> Items => ItemsQuery.Skip((PageNumber - 1) * PageSize).Take(PageSize);

        public int TotalItemCount => ItemsQuery.Count();
        public int TotalPageCount => (int)Math.Ceiling((double)TotalItemCount / PageSize);
    }

    ///// <summary>
    ///// Represents a single page of items with additional paging information like
    ///// page size, total count and total page size.
    ///// </summary>
    ///// <typeparam name="T">Type of the item in the list.</typeparam>
    //public class PagedList<T>
    //{
    //    public PagedList(
    //        IEnumerable<T> items,
    //        int pageNumber,
    //        int pageSize,
    //        int totalCount = 0)
    //    {
    //        Items = items ?? throw new ArgumentNullException(nameof(items));

    //        var pageNumberNormalised = Math.Max(pageNumber, 1);
    //        var pageSizeNormalised = Math.Min(pageSize, 100);
    //        var totalCountNormalised = Math.Max(Math.Max(0, totalCount), Items.Count());

    //        PageNumber = pageNumberNormalised;
    //        PageSize = pageSizeNormalised;
    //        TotalCount = totalCountNormalised;
    //    }

    //    public IEnumerable<T> Items { get; } = new List<T>();
    //    public int PageNumber { get; }
    //    public int PageSize { get; }
    //    public int TotalCount { get; }

    //    //public int TotalCount { get { return }  } = 0;

    //    ///// <summary>
    //    ///// Gets or sets one page of items.
    //    ///// </summary>
    //    //public IEnumerable<T> Items { get; set; } = new List<T>();

    //    ///// <summary>
    //    ///// Gets or sets the page number.
    //    ///// </summary>
    //    //public int PageNumber { get; set; } = 1;

    //    ///// <summary>
    //    ///// Gets or sets the page size.
    //    ///// </summary>
    //    //public int PageSize { get; set; } = 10;

    //    ///// <summary>
    //    ///// Gets or sets the total item count.
    //    ///// </summary>
    //    //public int TotalCount { get; set; } = 0;

    //    ///// <summary>
    //    ///// Gets the total page count.
    //    ///// </summary>
    //    //public int TotalPages =>
    //    //    (int)Math.Ceiling((double)TotalCount / PageSize);
    //}
}
