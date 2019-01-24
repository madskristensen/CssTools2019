﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Microsoft.WebTools.Languages.Shared.ContentTypes;

namespace CssTools
{
    [Export(typeof(ISignatureHelpSourceProvider))]
    [Name("Value Order Signature Help Source")]
    [Order(Before = "CSS Signature Help Source")]
    [ContentType(CssContentTypeDefinition.CssContentType)]
    internal class ValueOrderSignatureHelpSourceProvider : ISignatureHelpSourceProvider
    {
        public ISignatureHelpSource TryCreateSignatureHelpSource(ITextBuffer textBuffer)
        {
            return new ValueOrderSignatureHelpSource(textBuffer);
        }
    }

    [Export(typeof(ISignatureHelpSourceProvider))]
    [Name("Value Order Signature Help Source2")]
    [Order(After = "Default")]
    [ContentType(CssContentTypeDefinition.CssContentType)]
    internal class RemoveCssSignatureHelpSourceProvider : ISignatureHelpSourceProvider
    {
        public ISignatureHelpSource TryCreateSignatureHelpSource(ITextBuffer textBuffer)
        {
            return new RemoveCssSignatureHelpSource(textBuffer);
        }
    }
}
