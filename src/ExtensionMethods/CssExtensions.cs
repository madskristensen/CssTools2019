using System;
using System.Collections.Generic;
using Microsoft.WebTools.Languages.Css.Editor.Schemas;
using Microsoft.WebTools.Languages.Css.Parser;
using Microsoft.WebTools.Languages.Css.TreeItems;
using Microsoft.WebTools.Languages.Css.TreeItems.Functions;
using Microsoft.WebTools.Languages.Css.TreeItems.Selectors;

namespace CssTools
{
    public static class CssExtensions
    {
        ///<summary>Gets the selector portion of the text of a Selector object, excluding any trailing comma.</summary>
        public static string SelectorText(this Selector selector)
        {
            if (selector.Comma == null) return selector.Text;
            return selector.Text.Substring(0, selector.Comma.Start - selector.Start).Trim();
        }

        public static bool IsPseudoElement(this ParseItem item)
        {
            if (item.Text.StartsWith("::", StringComparison.Ordinal))
                return true;

            var schema = CssSchemaManager.SchemaManager.GetSchemaRoot(null);
            return schema.GetPseudo(":" + item.Text) != null;
        }

        public static bool IsDataUri(this UrlItem item)
        {
            if (item.UrlString == null || string.IsNullOrEmpty(item.UrlString.Text))
                return false;

            return item.UrlString.Text.Contains(";base64,");
        }

        /// <summary>
        /// Use this to make things work on 15.4 and 15.5+
        /// </summary>
        public static IEnumerable<Declaration> GetDeclarations(this RuleBlock rule)
        {
            var declaration = rule.GetType().GetProperty("Declarations")?.GetValue(rule);
            if (declaration == null)
                return System.Linq.Enumerable.Empty<Declaration>();
            else
                return declaration as IEnumerable<Declaration>;
        }

        //[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Justification = "Match enum name")]
        //public static CssErrorFlags ToCssErrorFlags(this WarningLocation location)
        //{
        //    switch (location)
        //    {
        //        case WarningLocation.Warnings:
        //            return CssErrorFlags.UnderlinePurple | CssErrorFlags.TaskListWarning;

        //        default:
        //            return CssErrorFlags.UnderlinePurple | CssErrorFlags.TaskListMessage;
        //    }
        //}
    }
}
