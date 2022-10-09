using SSU.Intrepretator.SyntaxAnalyzer;
using System;
using System.Collections.Generic;

namespace SSU.Intrepretator.Intrepretator
{
    public class IntrepretatorWorker
    {
        private List<PostfixEntry> _reversePolishNotation;
        private Dictionary<int, int> _constants;
        private Dictionary<int, string> _variables;
        private Dictionary<int, int> _pointers;
        private Dictionary<string, int> _variableValues;
        private Stack<StackEntry> _stack;

        public IntrepretatorWorker(List<PostfixEntry> reversePolishNotation, Dictionary<int, int> constants, Dictionary<int, string> variables, Dictionary<int, int> pointers)
        {
            _reversePolishNotation = reversePolishNotation;
            _constants = constants;
            _variables = variables;
            _pointers = pointers;
            _stack = new Stack<StackEntry>();
            _variableValues = new Dictionary<string, int>();
            foreach (var pair in _variables)
            {
                if(!_variableValues.ContainsKey(pair.Value))
                    _variableValues.Add(pair.Value, 0);
            }
        }

        public void Run()
        {
            int _currentPolishIndex = 0;
            int tmp = 0;
            while (_currentPolishIndex < _reversePolishNotation.Count)
            {
                if (_reversePolishNotation[_currentPolishIndex].EntryType == EntryType.Command)
                {
                    switch (_reversePolishNotation[_currentPolishIndex].Type)
                    {
                        case CommandType.JMP:
                            _currentPolishIndex = PopValue();
                            break;
                        case CommandType.JZ:
                            tmp = PopValue();
                            if (PopValue() == 1)
                            {
                                _currentPolishIndex++;
                            }
                            else
                            {
                                _currentPolishIndex = tmp;
                            }
                            break;
                        case CommandType.SET:
                            SetVariableAndPop(PopValue());
                            _currentPolishIndex++;
                            break;
                        case CommandType.ADD:
                            PushValue(PopValue() + PopValue());
                            _currentPolishIndex++;
                            break;
                        case CommandType.SUB:
                            PushValue(-PopValue() + PopValue());
                            _currentPolishIndex++;
                            break;
                        case CommandType.MULT:
                            PushValue(PopValue() * PopValue());
                            _currentPolishIndex++;
                            break;
                        case CommandType.DIV:
                            int tmpDiv = PopValue();
                            PushValue(PopValue() / tmpDiv);
                            break;
                        case CommandType.OR:
                            PushValue((PopValue() == 1 || PopValue() == 1) ? 1 : 0);
                            _currentPolishIndex++;
                            break;
                        case CommandType.AND:
                            PushValue((PopValue() == 1 && PopValue() == 1) ? 1 : 0);
                            _currentPolishIndex++;
                            break;
                        case CommandType.CMPE:
                            PushValue((PopValue() == PopValue()) ? 1 : 0);
                            _currentPolishIndex++;
                            break;
                        case CommandType.CMPNE:
                            PushValue((PopValue() != PopValue()) ? 1 : 0);
                            _currentPolishIndex++;
                            break;
                        case CommandType.CMPG:
                            PushValue((PopValue() < PopValue()) ? 1 : 0);
                            _currentPolishIndex++;
                            break;
                        case CommandType.CMPl:
                            PushValue((PopValue() > PopValue()) ? 1 : 0);
                            _currentPolishIndex++;
                            break;
                        case CommandType.OUT:
                            Console.WriteLine(PopValue());
                            _currentPolishIndex++;
                            break;
                    }
                }
                else
                {
                    PushElement(_reversePolishNotation[_currentPolishIndex++]);
                }
            }
        }

        private int PopValue()
        {
            var element = _stack.Pop();
            if (element.StackEntryType == StackEntryType.Variable)
                return _variableValues[element.VariableName];
            else
                return element.Value;
        }
        
        private void PushValue(int value)
        {
            var element = new StackEntry();
            element.StackEntryType = StackEntryType.Constant;
            element.Value = value;
            _stack.Push(element);
        }

        private void PushElement(PostfixEntry element)
        {
            var stackElement = new StackEntry();
            if (element.EntryType == EntryType.Constant)
            {
                stackElement.StackEntryType = StackEntryType.Constant;
                stackElement.Value = _constants[element.Index];
            }
            else if (element.EntryType == EntryType.CommandPointer)
            {
                stackElement.StackEntryType = StackEntryType.Constant;
                stackElement.Value = _pointers[element.Index];
            }
            else if (element.EntryType == EntryType.Variable)
            {
                stackElement.StackEntryType = StackEntryType.Variable;
                stackElement.Value = _variableValues[_variables[element.Index]];
                stackElement.VariableName = _variables[element.Index];
            }
            _stack.Push(stackElement);
        }

        private void SetVariableAndPop(int value)
        {
            var element = _stack.Pop();
            _variableValues[element.VariableName] = value;
        }
        
    }
}
