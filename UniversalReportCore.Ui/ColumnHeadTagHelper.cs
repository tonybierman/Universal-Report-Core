using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.Ui.Helpers;
using UniversalReportCore.ViewModels;

namespace UniversalReportCore.Ui
{
    [HtmlTargetElement("column-heading")]
    public class ColumnHeadTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public ColumnHeadTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [HtmlAttributeName("column")]
        public IReportColumnDefinition Column { get; set; } = null!;

        [HtmlAttributeName("model")]
        public IReportQueryParams? Model { get; set; }

        [HtmlAttributeName("viewmodel")]
        public BaseEntityViewModel? ViewModel { get; set; }

        [HtmlAttributeName("sort")]
        public string? Sort { get; set; }

        [HtmlAttributeName("page")]
        public string Page { get; set; } = "/Reports/Index";

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            // Set up the <th> element
            output.TagName = "th";
            output.Attributes.SetAttribute("class", $"text-left {(Column.HideInPortrait ? "hide-in-portrait" : "")}");

            // Display attribute for display name if available
            string? displayName = (Column?.PropertyName ?? Column?.ViewModelPropertyName) is string displayNamePropertyName
                ? ViewModel?.GetType().GetProperty(displayNamePropertyName)?.GetCustomAttribute<DisplayAttribute>()?.Name ?? null
                : null;

            // Tooltip with category if available
            string? toolTip = Column?.Description;
            if (toolTip == null)
            {
                toolTip = (Column?.PropertyName ?? Column?.ViewModelPropertyName) is string propertyName
                    ? ViewModel?.GetType().GetProperty(propertyName)?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? null
                    : null;
            }

            if (!string.IsNullOrEmpty(toolTip))
            {
                output.Attributes.SetAttribute("title", toolTip);
            }

            // If the column is sortable, generate the <a> link
            if (Column != null && Column.IsSortable)
            {
                var newSortOrder = Column.IsSortDescending ? $"{Column.PropertyName}Asc" : $"{Column.PropertyName}Desc";
                var routeValues = new Dictionary<string, object?>
                {
                    { "slug", Model?.Slug.Value },
                    { "ipp", Model?.Ipp.Value },
                    { "sortOrder", newSortOrder },
                    { "cohortIds", Model?.CohortIds.Value },
                    { "filters", Model?.FilterKeys.Value }
                };

                if (Model != null && Model.SearchQueries != null && Model.SearchQueries.Value != null)
                {
                    foreach (var search in Model.SearchQueries.Value)
                    {
                        routeValues.Add($"query{search.Key}", search.Value);
                    }
                }

                var url = urlHelper.Page(Page, routeValues);

                var linkTag = new TagBuilder("a");
                linkTag.Attributes["href"] = url;
                linkTag.InnerHtml.Append(Column.DisplayName ?? displayName ?? Column.ViewModelPropertyName ?? Column.PropertyName);

                output.Content.AppendHtml(linkTag);

                // Show sort indicator
                if (Sort != null && Column.PropertyName == Sort.Replace("Asc", "").Replace("Desc", ""))
                {
                    var sortIndicator = Column.IsSortDescending ? "↓" : "↑";
                    var spanTag = new TagBuilder("span");
                    spanTag.AddCssClass("sort-indicator");
                    spanTag.InnerHtml.Append(sortIndicator);
                    output.Content.AppendHtml(spanTag);
                }
            }
            else
            {
                // Just render the column display name if it's not sortable
                output.Content.Append(Column?.DisplayName ?? displayName ?? Column?.ViewModelPropertyName ?? Column?.PropertyName);
            }
        }
    }
}
