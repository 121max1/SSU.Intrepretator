using SSU.Intrepretator.LexicalAnalyzer;
using System.Collections.Generic;
using System.Linq;

namespace SSU.Intrepretator.SyntaxAnalyzer
{
    public class SyntAnalyzer
    {

        private readonly LexBuffer buffer;
        //private List<Lex> _tokens;
        private LinkedList<Lex> _tokens;
        private LinkedListNode<Lex> _current;



        public SyntAnalyzer(List<Lex> tokens)
        {
            buffer = new LexBuffer(tokens);
            _tokens = new LinkedList<Lex>();
            foreach (var item in tokens)
            {
                _tokens.AddLast(item);
            }
            _current = _tokens.First;
        }
        public bool Run()
        {
            return IsProgram();
        }

        private bool IsProgram()
        {
            if (_current is null | _current.Value.Type != LexType.Begin)
            {
                return false;
            }

            _current = _current.Next;

            if (!IsWhileStatement()) return false;

            if (_current is null | _current.Value.Type != LexType.End)
            {
                return false;
            }
            return true;
        }
        private bool IsWhileStatement()
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
            _current = _current.Next;
            if (!IsCondition()) return false;
            if (!IsAssignmentStatement()) return false;
            if(_current is null | _current.Value.Type!= LexType.Loop)
            {
                return false;
            }
            _current = _current.Next;
            return true;
        }

        private bool IsCondition()
        {
            if (!IsLogExpession()) return false;
            while(_current!=null & _current.Value.Type==LexType.Or){
                _current = _current.Next;
                if (!IsLogExpession()) return false;
            }
            return true;
        }

        private bool IsLogExpession()
        {
            if (!IsRelExpression()) return false;
            while(_current != null && _current.Value.Type == LexType.And)
            {
                _current = _current.Next;
                if (!IsRelExpression()) return false;
            }
            return true;
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
            if(_current is null | _current.Value.Type != LexType.As)
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
