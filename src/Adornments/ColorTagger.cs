﻿using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.WebTools.Languages.Css.Document;
using Microsoft.WebTools.Languages.Css.Editor.Completion.ColorPicker;
using Microsoft.WebTools.Languages.Css.Editor.Document;
using Microsoft.WebTools.Languages.Css.Parser;
using Microsoft.WebTools.Languages.Css.Tokens;
using Microsoft.WebTools.Languages.Css.TreeItems;
using Microsoft.WebTools.Languages.Css.TreeItems.Functions;
using Microsoft.WebTools.Languages.Css.TreeItems.PropertyValues;
using Microsoft.WebTools.Languages.Extensions.Css.Extensions;
using Microsoft.WebTools.Languages.Shared.Editor.ColorPicker;
using Microsoft.WebTools.Languages.Shared.Editor.Host;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Threading;

namespace CssTools
{
    internal sealed class ColorTagger : ITagger<ColorTag>
    {
        private ITextBuffer _buffer;
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        internal ColorTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
            _buffer.ChangedLowPriority += BufferChanged;
        }

        public IEnumerable<ITagSpan<ColorTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (WebEditor.Host == null || spans.Count == 0 || spans[0].Length == 0 || spans[0].Length >= _buffer.CurrentSnapshot.Length)
                yield break;

            var tree = CssEditorDocument.FromTextBuffer(_buffer).Tree;
            IEnumerable<ParseItem> items = GetColors(tree, spans[0]);

            foreach (var item in items.Where(i => (i.Start + i.Length) <= _buffer.CurrentSnapshot.Length))
            {
                SnapshotSpan span = new SnapshotSpan(_buffer.CurrentSnapshot, item.Start, item.Length);
                ColorModel colorModel = ColorParser.TryParseColor(item, ColorParser.Options.AllowAlpha | ColorParser.Options.AllowNames);
                // Fix up for rebeccapurple adornment as it isn't parsed by ColorParser currently
                if (colorModel == null && item.Text=="rebeccapurple")
                {
                    // as per http://lists.w3.org/Archives/Public/www-style/2014Jun/0312.html
                    colorModel = ColorParser.TryParseColor("#663399", ColorParser.Options.AllowAlpha | ColorParser.Options.AllowNames);
                }
                if (colorModel != null)
                {
                    yield return new TagSpan<ColorTag>(span, new ColorTag(colorModel.Color));
                }
            }
        }

        private static IEnumerable<ParseItem> GetColors(CssTree tree, SnapshotSpan span)
        {
            ParseItem complexItem = tree.StyleSheet.ItemFromRange(span.Start, span.Length);

            if (complexItem == null)
                return Enumerable.Empty<ParseItem>();

            var colorCrawler = new CssItemAggregator<ParseItem>(filter: e => e.AfterEnd > span.Start && e.Start < span.End)
            {
                (HexColorValue h) => h,
                (FunctionColor c) => c,
                (TokenItem i) => (i.PreviousSibling == null || (i.PreviousSibling.Text != "@" && i.PreviousSibling.Text != "$"))    // Ignore variable names that happen to be colors
                                 && i.TokenType == CssTokenType.Identifier
                                 && (i.FindType<Declaration>() != null || i.FindType<CssExpression>() != null)                       // Ignore classnames that happen to be colors
                                 && Color.FromName(i.Text).IsNamedColor
                                 ? i : null
            };

            return colorCrawler.Crawl(complexItem).Where(o => o != null);
        }

        private void BufferChanged(object sender, TextContentChangedEventArgs e)
        {
            if (e.Changes.Count == 0)
                return;

            var temp = TagsChanged;
            if (temp == null)
                return;

            // Combine all changes into a single span so that
            // the ITagger<>.TagsChanged event can be raised just once for a compound edit
            // with many parts.

            ITextSnapshot snapshot = e.After;

            int start = e.Changes[0].NewPosition;
            int end = e.Changes[e.Changes.Count - 1].NewEnd;

            SnapshotSpan totalAffectedSpan = new SnapshotSpan(
                snapshot.GetLineFromPosition(start).Start,
                snapshot.GetLineFromPosition(end).End);

            //temp(this, new SnapshotSpanEventArgs(totalAffectedSpan));

            Dispatcher.CurrentDispatcher.BeginInvoke(
                    new Action(() => temp(this, new SnapshotSpanEventArgs(totalAffectedSpan))), DispatcherPriority.ApplicationIdle);
        }
    }
}
