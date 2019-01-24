﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.WebTools.Languages.Css.Editor.Completion;
using Microsoft.WebTools.Languages.Css.Parser;
using Microsoft.WebTools.Languages.Css.TreeItems;
using Microsoft.WebTools.Languages.Css.TreeItems.Selectors;
using Microsoft.WebTools.Languages.Extensions.Less.Parser;

namespace CssTools
{
    [Export(typeof(ICssCompletionContextProvider))]
	[Name("LessPseudoContextProvider")]
	[Order(Before = "Default Pseudo")]
	internal class LessPseudoContextProvider : ICssCompletionContextProvider
	{
		public IEnumerable<Type> ItemTypes
		{
			get
			{
				return new Type[]
				{
					typeof(PseudoClassFunctionSelector),
					typeof(PseudoClassSelector),
					typeof(PseudoElementFunctionSelector),
					typeof(PseudoElementSelector)
				};
			}
		}

		public CssCompletionContext GetCompletionContext(ParseItem item, int position)
		{
			RuleSet rule = item.FindType<RuleSet>();

			if (rule != null && rule.Parent is LessRuleBlock)
			{
				return new CssCompletionContext(CssCompletionContextType.Invalid, item.Start, item.Length, item);
			}

			return null;
		}
	}
}