using SSU.Intrepretator.LexicalAnalyzer;
using System;
using System.Collections.Generic;

namespace SSU.Intrepretator.SyntaxAnalyzer
{
    public class SyntaxAnalyzer
    {
        private readonly LexBuffer buffer;
        public SyntaxAnalyzer(List<Lex> tokens)
        {
            buffer = new LexBuffer(tokens);
        }

        private static void Operand()
        {

        }



    }
}
