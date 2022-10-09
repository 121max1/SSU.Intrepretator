using SSU.Intrepretator.LexicalAnalyzer;
using System.Collections.Generic;
using System.Linq;

namespace SSU.Intrepretator.SyntaxAnalyzer
{
    public enum EntryType { Command, Variable, Constant, CommandPointer }

    public enum CommandType {JMP, JZ, SET, ADD, SUB, MULT, DIV, AND, OR, CMPE, CMPNE, CMPl, CMPG, None, OUT}

    public class SyntAnalyzer
    {
        private LinkedList<Lex> _tokens;
        private LinkedListNode<Lex> _current;
        private Result result;
        public List<PostfixEntry> _reversePolishNotation;
        public Dictionary<int, int> _constants;
        public Dictionary<int, string> _variables;
        public Dictionary<int, int> _pointers;
        private int _currentPolishNotationId;



        public SyntAnalyzer(List<Lex> tokens)
        {
            _reversePolishNotation = new List<PostfixEntry>();
            _constants = new Dictionary<int, int>();
            _variables = new Dictionary<int, string>();
            _pointers = new Dictionary<int, int>();
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
            int indFirst = _currentPolishNotationId;//??
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
            int indJmp = WriteCmdPtr(-1);
            WriteCmd(CommandType.JZ);
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
                if (IsWhileStatement())
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
            WriteCmdPtr(indFirst);
            int indLast = WriteCmd(CommandType.JMP);
            SetCmdPtr(indJmp, indLast + 1);
            _current = _current.Next;
            return true;
        }

        private bool IsCondition()
        {
            if (!IsLogExpession()) return false;
            while(_current!=null & _current.Value.Type==LexType.Or){
                _current = _current.Next;
                if (!IsLogExpession()) return false;
                WriteCmd(CommandType.OR);
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
                WriteCmd(CommandType.AND);
            }
            return true;
        }

        private bool IsRelExpression()
        {
            if (!IsOperand()) return false;
            if (_current.Next != null && _current.Value.Type == LexType.Rel)
            {
                CommandType command = default;
                if (_current.Value.Value == ">") command = CommandType.CMPG;
                else if (_current.Value.Value == "<") command = CommandType.CMPl;
                else if (_current.Value.Value == "=") command = CommandType.CMPE;
                else if (_current.Value.Value == "<>") command = CommandType.CMPNE;
                _current = _current.Next;
                if (!IsOperand()) return false;
                WriteCmd(command);

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
            if (_current.Value.Class == LexClass.Identifier) WriteVar(_currentPolishNotationId);
            else WriteConst(_currentPolishNotationId);
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
            WriteVar(_currentPolishNotationId);
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
            WriteCmd(CommandType.SET);
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
            if (_current.Value.Class == LexClass.Constant) WriteConst(_currentPolishNotationId);
            else if (_current.Value.Class == LexClass.Identifier) WriteVar(_currentPolishNotationId);
            _current = _current.Next;
            if (_current.Value.Type != LexType.SEMICOLON)
            {
                result = new Result(false, $"Ожидается ; на позиции {_current.Value.Position}");
                return false;
            }
            WriteCmd(CommandType.OUT);
            _current = _current.Next;
            return true;
        }

        private bool IsExpression()
        {
            if (!IsTerm()) return false;
            while(_current.Value.Value == "+" || _current.Value.Value == "-")
            {
                CommandType command = default;
                if (_current.Value.Value == "+") command = CommandType.ADD;
                else if (_current.Value.Value == "-") command = CommandType.SUB;
                _current = _current.Next;
                //WriteCmd(command);
                if (!IsFactor()) return false;
                WriteCmd(command);
            }
            return true;

        }

        private bool IsTerm()
        {
            if (!IsFactor()) return false;
            while(_current.Value.Value == "*" || _current.Value.Value == "/")
            {
                CommandType command = default;
                if (_current.Value.Value == "*") command = CommandType.MULT;
                else if (_current.Value.Value == "/") command = CommandType.DIV;// если что свапнуть +-
                _current = _current.Next;
                IsFactor();
                WriteCmd(command);

            }
            return true;
        }

        private bool IsFactor()
        {
            if (_current.Value.Class == LexClass.Identifier)
            {
                WriteVar(_currentPolishNotationId);
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
                WriteConst(_currentPolishNotationId);
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


        private int WriteVar(int id)
        {
            _reversePolishNotation.Add(new PostfixEntry(EntryType.Variable, id));
            _variables.Add(id, _current.Value.Value);
            return _currentPolishNotationId++;
        }

        private int WriteCmd(CommandType cmd)
        {
            _reversePolishNotation.Add(new PostfixEntry(cmd, _currentPolishNotationId));
            return _currentPolishNotationId++;
        }

        private int WriteConst(int id)
        {
            _reversePolishNotation.Add(new PostfixEntry(EntryType.Constant, id));
            _constants.Add(id, int.Parse(_current.Value.Value));
            return _currentPolishNotationId++;
        }

        private int WriteCmdPtr(int ptr)
        {
            _reversePolishNotation.Add(new PostfixEntry(EntryType.CommandPointer, _currentPolishNotationId));
            _pointers.Add(_currentPolishNotationId, ptr);
            return _currentPolishNotationId++;
        }

        void SetCmdPtr(int ind, int ptr)
        {
            _pointers[ind] = ptr;
        }

        public string GetInversedPolishNotation()
        {
            string result = "";
            foreach(var entry in _reversePolishNotation)
            {
                if(entry.EntryType == EntryType.Constant)
                {
                    result += _constants[entry.Index];
                }
                else if(entry.EntryType == EntryType.Variable)
                {
                    result += _variables[entry.Index];
                }
                else if(entry.EntryType == EntryType.Command)
                {
                    result += entry.Type.ToString();
                }
                else if(entry.EntryType == EntryType.CommandPointer)
                {
                    result += _pointers[entry.Index];
                }
                result += " ";
            }
            return result;
        }
    }
}
