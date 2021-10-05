using System;
using System.Collections.Generic;
using System.Text;

namespace SSU.Intrepretator.LexicalAnalyzer
{
    public class Lex
    {
        public Lex(LexType type, string value, LexClass lexClass)
        {
            GlobalId++;
            Id = GlobalId;
            Type = type;
            Value = value;
            Class = lexClass;
        }

        public static int GlobalId { get; private set; }

        public int Id { get; set; }
        public LexType Type { get; private set; }

        public LexClass Class { get; private set; }
        public string Value { get; private set; }
    }
}
