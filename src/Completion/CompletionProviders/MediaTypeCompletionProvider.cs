﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Utilities;
using Microsoft.WebTools.Languages.Css.Editor.Completion;
using Microsoft.WebTools.Languages.Css.Parser;
using Microsoft.WebTools.Languages.Css.TreeItems.AtDirectives;

namespace CssTools
{
    [Export(typeof(ICssCompletionProvider))]
    [Name("MediaTypeCompletionProvider")]
    internal class MediaTypeCompletionProvider : ICssCompletionListProvider
    {
        public CssCompletionContextType ContextType
        {
            get { return (CssCompletionContextType)612; }
        }

        public IEnumerable<ICssCompletionListEntry> GetListEntries(CssCompletionContext context)
        {
            MediaQuery query = (MediaQuery)context.ContextItem;
            ParseItem item = query.StyleSheet.ItemAfterPosition(context.SpanStart);

            if (query.MediaType != null && query.MediaType.AfterEnd < item.Start)
            {
                yield return new CompletionListEntry("and", 0, StandardGlyphGroup.GlyphGroupOperator);
                yield break;
            }

            if (item != query.Operation || query.MediaType == null)
            {
                yield return new CompletionListEntry("all");
                yield return new CompletionListEntry("aural");
                yield return new CompletionListEntry("braille");
                yield return new CompletionListEntry("embossed");
                yield return new CompletionListEntry("handheld");
                yield return new CompletionListEntry("print");
                yield return new CompletionListEntry("projection");
                yield return new CompletionListEntry("screen");
                yield return new CompletionListEntry("tty");
                yield return new CompletionListEntry("tv");
            }

            if (item != query.MediaType || query.Operation == null)
            {
                yield return new CompletionListEntry("not", 1, StandardGlyphGroup.GlyphGroupOperator);
                yield return new CompletionListEntry("only", 1, StandardGlyphGroup.GlyphGroupOperator);
            }
        }
    }
}
