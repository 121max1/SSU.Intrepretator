using SSU.Intrepretator.LexicalAnalyzer;
using System.Collections.Generic;

namespace SSU.Intrepretator.SyntaxAnalyzer
{
    public class SyntaxAnalyzer
    {
        private readonly LexBuffer buffer;
        private List<Lex> _tokens;

        public SyntaxAnalyzer(List<Lex> tokens)
        {
            buffer = new LexBuffer(tokens);
            _tokens = tokens;
        }
        public void Run()
        {
            foreach(var token in _tokens)
            {
                
            }
        }

        private bool IsWhileStatement(int startIndex)
        {
            if(d)
        }

        private bool IsCondition()
        {

        }

        private bool IsLogExpession()
        {

        }

        private bool IsRelExpression()
        {

        }

        private bool IsOperand()
        {

        }

        private bool IsLogicalOperation()
        {

        }

        private bool IsStatement()
        {

        }

        bool IsArithmExpression()
        {
                
        }








    }
}
