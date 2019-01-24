﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Reflection;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Utilities;
using Microsoft.WebTools.Languages.Css.Editor.Classify;
using Microsoft.WebTools.Languages.Css.Editor.Completion;
using Microsoft.WebTools.Languages.Shared.Editor.Completion.Presenter;

namespace CssTools
{
    [Export(typeof(ICssCompletionProvider))]
    [Name("ColorCompletionProvider")]
    [Order(Before = "Default PropertyValue")]
    internal class ColorCompletionProvider : ICssCompletionListProvider, ICssCompletionPresenterProvider
    {
        public CssCompletionContextType ContextType
        {
            get { return CssCompletionContextType.PropertyValue; }
        }

        public IEnumerable<ICssCompletionListEntry> GetListEntries(CssCompletionContext context)
        {
            yield break;
        }

        public CompletionPresenterInfo TryCreateCompletionPresenter(ICompletionSession session, CssCompletionContext context)
        {
            string text = context.Snapshot.GetText(context.SpanStart, context.SpanLength);
            if (Color.FromName(text).IsKnownColor)
            {
                return CreatePresenter(session, context);
            }

            return new CompletionPresenterInfo(null, true);
        }
        
        private static CompletionPresenterInfo CreatePresenter(ICompletionSession session, CssCompletionContext context)
        {
            object[] parameters = new object[] { session, context };
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            Type type = typeof(CssClassifier).Assembly.GetType("Microsoft.WebTools.Languages.Css.Editor.Completion.ColorPicker.ColorPickerPresenter");
            object colorPicker = Activator.CreateInstance(type, flags, null, parameters, null);

            return new CompletionPresenterInfo((IIntellisensePresenter)colorPicker, false);
        }
    }
}