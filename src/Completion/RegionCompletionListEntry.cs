﻿using System;
using Microsoft.VisualStudio.Text;
using Microsoft.WebTools.Languages.Css.Editor.Completion;
using Microsoft.WebTools.Languages.Css.Editor.Schemas.Browsers;
using Microsoft.WebTools.Languages.Shared.Editor.Completion;

namespace CssTools
{
    internal class RegionCompletionListEntry : ICssCompletionListEntry
    {
        public string Description
        {
            get { return string.Empty; }
        }

        public string DisplayText
        {
            get { return "Add region..."; }
        }

        public string GetSyntax(Version version)
        {
            return string.Empty;
        }

        public string GetAttribute(string name)
        {
            return string.Empty;
        }

        public string GetInsertionText(CssTextSource textSource, ITrackingSpan typingSpan)
        {
            return "region";//"/*#region MyRegion */\n\n\n\n/*#endregion*/";
        }

        public string GetVersionedAttribute(string name, Version version)
        {
            return GetAttribute(name);
        }

        public bool AllowQuotedString
        {
            get { return false; }
        }

        public bool IsBuilder
        {
            get { return true; }
        }

        public int SortingPriority { get { return 0; } }


        public bool IsSupported(BrowserVersion browser)
        {
            return true;
        }

        public bool IsSupported(Version cssVersion)
        {
            return true;
        }


        public ITrackingSpan ApplicableTo
        {
            get { return null; }
        }

        public CompletionEntryFilterTypes FilterType
        {
            get { return CompletionEntryFilterTypes.AlwaysVisible; }
        }

        public System.Windows.Media.ImageSource Icon
        {
            get { return null; }
        }

        public bool IsCommitChar(char typedCharacter)
        {
            return false;
        }

        public bool IsMuteCharacter(char typedCharacter)
        {
            return false;
        }

        public bool RetriggerIntellisense
        {
            get { return false; }
        }
    }
}
