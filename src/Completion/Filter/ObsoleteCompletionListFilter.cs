﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.WebTools.Languages.Css.Editor.Completion;
using Microsoft.WebTools.Languages.Css.Editor.Schemas;
using Microsoft.WebTools.Languages.Shared.Editor.Completion;

namespace CssTools
{
    [Export(typeof(ICssCompletionListFilter))]
	[Name("ObsoleteCompletionListFilter")]
	internal class ObsoleteCompletionListFilter : ICssCompletionListFilter
	{
		public void FilterCompletionList(IList<CssCompletionEntry> completions, CssCompletionContext context)
		{
			ICssSchemaInstance rootSchema = CssSchemaManager.SchemaManager.GetSchemaRoot(null);
			ICssSchemaInstance schema = CssSchemaManager.SchemaManager.GetSchemaForItem(rootSchema, context.ContextItem);

			foreach (CssCompletionEntry entry in completions)
			{
				ICssCompletionListEntry prop = GetSchemaEntry(schema, context, entry);

				if (prop != null && !string.IsNullOrEmpty(prop.GetAttribute("obsolete")))
					entry.FilterType = CompletionEntryFilterTypes.NeverVisible;
			}
		}

		private static ICssCompletionListEntry GetSchemaEntry(ICssSchemaInstance schema, CssCompletionContext context, CssCompletionEntry entry)
		{
			switch (context.ContextType)
			{
				case CssCompletionContextType.AtDirectiveName:
					return schema.GetAtDirective(entry.DisplayText);

				case CssCompletionContextType.PropertyName:
					return schema.GetProperty(entry.DisplayText);

				case CssCompletionContextType.PseudoClassOrElement:
					return schema.GetPseudo(entry.DisplayText);
			}

			return null;
		}
	}
}