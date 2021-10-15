using SSU.Intrepretator.LexicalAnalyzer;
using System.Collections.Generic;
using System.Linq;

namespace SSU.Intrepretator.SyntaxAnalyzer
{
    public class SyntaxAnalyzer
    {
        private readonly LexBuffer buffer;
        //private List<Lex> _tokens;
        private LinkedList<Lex> _tokens;
        private LinkedListNode<Lex> _current;



        public SyntaxAnalyzer(List<Lex> tokens)
        {
            buffer = new LexBuffer(tokens);
            foreach (var item in tokens)
            {
                _tokens.AddLast(item);
            }
            _current = _tokens.First;
        }
        public void Run()
        {

            foreach (var token in _tokens)
            {

            }
        }

        private bool IsWhileStatement(int startIndex)
        {
            if (_current is null | _current.Value.Type!= LexType.Do)
            {
                return false;
            }
            _current = _current.Next;
            if(_current is null | _current.Value.Type!= LexType.Until)
            {
                return false;
            }

            
     

        }

        private bool IsCondition()
        {

        }

        private bool IsLogExpession()
        {

        }

        private bool IsRelExpression()
        {
            if (!IsOperand()) return false;
            if (_current.Next != null && _current.Value.Type == LexType.Rel)
            {
                _current = _current.Next;
                if (!IsOperand()) return false;
            }
            return true;
        }

        private bool IsOperand()
        {
            if (_current.Value.Class != LexClass.Identifier && _current.Value.Class != LexClass.Constant)
            {
                return false;
            }
            _current = _current.Next;
            return true;

        }

        private bool IsLogicalOperation()
        {
            if(_current.Value.Type!=LexType.And && _current.Value.Type!= LexType.Or)
            {
                return false;
            }
            _current = _current.Next;
            return true;
        }

        private bool IsAssignmentStatement()
        {
            if(_current.Value.Class != LexClass.Identifier)
            {
                return false;
            }
            _current = _current.Next;
            if(_current != null || _current.Value.Type != LexType.As)
            {
                return false;
            }
            _current = _current.Next;
            if (!IsArithmExpression()) return false;
            return true;
        }

        private bool IsArithmExpression()
        {
            if (!IsOperand()) return false;
            while(_current!=null && _current.Value.Type == LexType.Ao)
            {
                _current = _current.Next;
                if (!IsOperand()) return false;
            }
            return true;
        }








    }
}
