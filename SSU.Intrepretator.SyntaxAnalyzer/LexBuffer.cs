using SSU.Intrepretator.LexicalAnalyzer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SSU.Intrepretator.SyntaxAnalyzer
{
    public class LexBuffer
    {
        public int Position { get;  private set; }

        private readonly List<Lex> _tokens;
        public LexBuffer(List<Lex> tokens)
        {
            _tokens = tokens;
        }

        public Lex Next()
        {
            return _tokens[Position++];
        }

        public void Back()
        {
            Position--;
        }

    }
}
