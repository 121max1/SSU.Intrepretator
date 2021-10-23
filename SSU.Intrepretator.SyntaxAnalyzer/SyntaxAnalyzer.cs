using SSU.Intrepretator.LexicalAnalyzer;
using System.Collections.Generic;
using System.Linq;

namespace SSU.Intrepretator.SyntaxAnalyzer
{
    public enum EntryType { Command, Variable, Constant, CommandPointer }

    public enum CommandType {JMP , JZ, SET, ADD, SUB, MULT, DIV, AND, OR, CMPE, CMPNE, CMPl, CMPG,}

    public class SyntAnalyzer
    {

        private readonly LexBuffer buffer;
        //private List<Lex> _tokens;
        private LinkedList<Lex> _tokens;
        private LinkedListNode<Lex> _current;
        private Result result;


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
        public Result Run()
        {
            if (_current is null | _current.Value.Type != LexType.Begin)
            {
                result = new Result(false, $"Ожидается begin на позиции {_current.Value.Position}");
                return result;
            }

            _current = _current.Next;


            while(_current.Value.Type != LexType.End)
            {
                var start = _current;
                if (IsAssignmentStatement())
                {
                    continue;
                }
                else
                {
                    _current = start;
                }
                if (IsOutputExpression())
                {
                    continue;
                }
                else
                {
                    _current = start;
                }
                if (IsWhileStatement())
                {
                    continue;
                }
                else
                {
                    _current = start;
                }
                //if (!isValid)
                //{
                //    return new Result(true, $"Лишние символы в позиции {_current.Value.Position}");
                //}

            }

            if (_current is null | _current.Value.Type != LexType.End)
            {
                return result;
            }
            return new Result(true, "Ошибок не найдено");
        }

        private bool IsWhileStatement()
        {
            if (_current is null | _current.Value.Type!= LexType.Do)
            {
                result = new Result(false, $"Ожидается do на позиции {_current.Value.Position}");
                return false;
            }
            _current = _current.Next;
            if(_current is null | _current.Value.Type!= LexType.Until)
            {
                result = new Result(false, $"Ожидается until на позиции {_current.Value.Position}");
                return false;
            }
            _current = _current.Next;
            if (!IsCondition()) return false;
            while(_current.Value.Type!=LexType.Loop)
            {
                var start = _current;
                bool isValid = false;
                if (IsAssignmentStatement())
                {              
                    continue;
                }
                else
                {
                    _current = start;
                }
                if (IsOutputExpression())
                {
                    continue;
                }
                else
                {
                    _current = start;
                }
                if (!isValid)
                {
                    return false;
                }
                // _current = _current.Next;
            }
            if(_current is null | _current.Value.Type!= LexType.Loop)
            {
                result = new Result(false, $"Ожидается loop на позиции {_current.Value.Position}");
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
                result = new Result(false, $"Ожидается индентификатор или константа на позиции {_current.Value.Position}");
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
                result = new Result(false, $"Ожидается индентификатор на позиции {_current.Value.Position}");
                return false;
            }
            _current = _current.Next;
            if(_current is null | _current.Value.Type != LexType.As)
            {
                result = new Result(false, $"Ожидается оператор присваивания на позиции {_current.Value.Position}");
                return false;
            }
            _current = _current.Next;
            if (!IsExpression()) return false;
            if (_current is null | _current.Value.Type != LexType.SEMICOLON)
            {
                result = new Result(false, $"Ожидается ; на позиции {_current.Value.Position}");
                return false;
            }
            _current = _current.Next;
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

        private bool IsOutputExpression()
        {
            if(_current.Value.Type != LexType.Output)
            {
                return false;
            }
            _current = _current.Next;
            if (_current.Value.Class != LexClass.Constant && _current.Value.Class != LexClass.Identifier)
            {
                result = new Result(false, $"Ожидается константа или идентификатор на позиции {_current.Value.Position}");
                return false;
            }
            _current = _current.Next;
            if (_current.Value.Type != LexType.SEMICOLON)
            {
                result = new Result(false, $"Ожидается ; на позиции {_current.Value.Position}");
                return false;
            }
            _current = _current.Next;
            return true;
        }

        private bool IsExpression()
        {
            if (!IsTerm()) return false;
            while(_current.Value.Value == "*" || _current.Value.Value == "/")
            {
                _current = _current.Next;
                if (!IsFactor()) return false;
            }
            return true;

        }

        private bool IsTerm()
        {
            if (!IsFactor()) return false;
            while(_current.Value.Value == "+" || _current.Value.Value == "-")
            {
                _current = _current.Next;
                IsFactor();
            }
            return true;
        }

        private bool IsFactor()
        {
            if (_current.Value.Class == LexClass.Identifier)
            {
                _current = _current.Next;
                if (_current.Value.Type == LexType.OPENPAREN)
                {
                    _current = _current.Next;
                    //if (!IsParameters()) return false;
                    if (_current.Value.Type != LexType.CLOSEPAREN)
                    {
                        result = new Result(false, $"Ожидается ) на позиции {_current.Value.Position}");
                        return false;
                    }
                    _current = _current.Next;
                    
                }
            }
            else if (_current.Value.Class == LexClass.Constant)
            {
                _current = _current.Next;
            }
            else if (_current.Value.Type == LexType.OPENPAREN)
            {
                _current = _current.Next;
                if (!IsExpression()) return false;
                else if (_current.Value.Type != LexType.CLOSEPAREN)
                {
                    result = new Result(false, $"Ожидается ) на позиции {_current.Value.Position}");
                    return false;
                }
                _current = _current.Next;
            }
            else
            {
                return false;
            }

            return true;
        }

        private bool IsParameters()
        {
            if (!IsExpression()) return false;
            return true;
        }


    }
}
