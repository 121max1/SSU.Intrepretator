using SSU.Intrepretator.LexicalAnalyzer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSU.Intrepretator.SyntaxAnalyzer
{
    public class PostfixEntry
    {
        public PostfixEntry(CommandType type, int index)
        {
            Type = type;
            Index = index;
            EntryType = EntryType.Command;
        }
        public PostfixEntry(EntryType type, int index)
        {
            Type = CommandType.None;
            Index = index;
            EntryType = type;
        }

        public PostfixEntry() { }

        public EntryType EntryType { get; set; }

        public CommandType Type { get; set; }

        public int Index { get; set; }


    }
}
