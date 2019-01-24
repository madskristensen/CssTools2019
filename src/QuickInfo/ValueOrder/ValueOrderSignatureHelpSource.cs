﻿using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.WebTools.Languages.Css.Editor.Document;
using Microsoft.WebTools.Languages.Css.Parser;
using Microsoft.WebTools.Languages.Css.TreeItems;

namespace CssTools
{
    internal class ValueOrderSignatureHelpSource : ISignatureHelpSource
    {
        private ITextBuffer _buffer;

        public ValueOrderSignatureHelpSource(ITextBuffer buffer)
        {
            _buffer = buffer;
        }

        public void AugmentSignatureHelpSession(ISignatureHelpSession session, IList<ISignature> signatures)
        {
            SnapshotPoint? point = session.GetTriggerPoint(_buffer.CurrentSnapshot);
            if (!point.HasValue)
                return;

            CssEditorDocument document = CssEditorDocument.FromTextBuffer(_buffer);
            ParseItem item = document.StyleSheet.ItemBeforePosition(point.Value.Position);

            if (item == null)
                return;

            Declaration dec = item.FindType<Declaration>();
            if (dec == null || dec.PropertyName == null || dec.Colon == null)
                return;

            int length = dec.Length - (dec.Colon.Start - dec.Start);
            var span = _buffer.CurrentSnapshot.CreateTrackingSpan(dec.Colon.Start, length, SpanTrackingMode.EdgeNegative);

            ValueOrderFactory.AddSignatures method = ValueOrderFactory.GetMethod(dec);

            if (method != null)
            {
                signatures.Clear();
                method(session, signatures, dec, span);

                Dispatcher.CurrentDispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        if (session == null || session.Properties == null)
                            return;

                        session.Properties.AddProperty("dec", dec);
                        session.Match();
                    }),
                    DispatcherPriority.Normal, null);
            }
        }

        public ISignature GetBestMatch(ISignatureHelpSession session)
        {
            int number = 0;

            if (session.Properties.ContainsProperty("dec"))
            {
                Declaration dec = session.Properties["dec"] as Declaration;
                string methodName = ValueOrderFactory.GetMethod(dec).Method.Name;
                if (dec.Values.Count > 0 && (methodName == "Margins" || methodName == "Corners"))
                {
                    number = 4 - dec.Values.Count;
                }
            }

            return (session.Signatures != null && session.Signatures.Count > number && number > -1)
                ? session.Signatures[number]
                : null;
        }

        private bool m_isDisposed;
        public void Dispose()
        {
            if (!m_isDisposed)
            {
                GC.SuppressFinalize(this);
                m_isDisposed = true;
            }
        }
    }
}
