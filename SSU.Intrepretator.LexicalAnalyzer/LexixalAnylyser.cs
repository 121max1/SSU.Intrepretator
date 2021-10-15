using System;
using System.Collections.Generic;
using System.Text;

namespace SSU.Intrepretator.LexicalAnalyzer
{
    public enum LexType {Do, Until, Loop, Not, And, Or, Output, Rel, Ao, As, Undefined }

    public enum LexClass {Keyword, Identifier, Constant, SpecialSymbols, Undefined }

    public class LexixalAnylyser
    {

        public List<Lex> Tokens { get; private set; }
        public LexixalAnylyser()
        {
            Tokens = new List<Lex>();
        }

        private string[] _keyWords = { "do", "until", "loop", "not", "and", "or", "output" };
        enum State { S, ID, CON, ERR, FIN, COMP, COMPN, ARIFM, ASSIGN }

        public bool LexAnalyzer(string text)
        {
            State state = State.S, prevState;
            bool add;
            text += " ";
            StringBuilder lexBufNext = new StringBuilder();
            StringBuilder lexBufCur = new StringBuilder();
            int textIndex = 0;
            while(state != State.ERR && state!= State.FIN)
            { 
                prevState = state;
                add = true;
                if (textIndex == text.Length && state!=State.ERR)
                {
                    state = State.FIN;
                    break;
                }
                if(textIndex == text.Length)
                {
                    break;
                }
                char symbol = text[textIndex];
                switch (state)
                {
                    case State.S:
                        if (char.IsWhiteSpace(symbol)) state = State.S;
                        else if (char.IsDigit(symbol)) state = State.CON;
                        else if (char.IsLetter(symbol)) state = State.ID;
                        else if (symbol == '<') state = State.COMPN;
                        else if (symbol == '=') state = State.COMP;
                        else if (symbol == '>') state = State.COMP;
                        else if (symbol == '+' || symbol == '-' || symbol == '/' || symbol == '*') state = State.ARIFM;
                        else if (symbol == ':') state = State.ASSIGN;
                        else state = State.ERR;
                        add = false;
                        if (!char.IsWhiteSpace(symbol))
                            lexBufCur.Append(symbol);
                        break;
                    case State.COMP:
                        if (char.IsWhiteSpace(symbol))
                        {
                            state = State.S;
                        }
                        else if (char.IsLetter(symbol))
                        {
                            state = State.ID;
                            lexBufNext.Append(symbol);
                        }
                        else if (char.IsDigit(symbol))
                        {
                            state = State.CON;
                            lexBufNext.Append(symbol);
                        }
                        else
                        {
                            state = State.ERR;
                            add = false;
                        }

                        break;
                    case State.COMPN:
                        if (char.IsWhiteSpace(symbol)) state = State.S;
                        else if (symbol == '>')
                        {
                            state = State.S;
                            lexBufCur.Append(symbol);
                        }
                        else if (char.IsLetter(symbol))
                        {
                            state = State.ID;
                            lexBufNext.Append(symbol);
                        }
                        else if (char.IsDigit(symbol))
                        {
                            state = State.CON;
                            lexBufNext.Append(symbol);
                        }
                        else
                        {
                            state = State.ERR;
                            add = false;
                        }
                        break;
                    case State.ASSIGN:
                        if (symbol != '=')
                        {
                            state = State.ERR;
                            add = false;
                        }
                        else
                        {
                            state = State.S;
                            lexBufCur.Append(symbol);
                        }
                        break;
                    case State.CON:
                        if (char.IsWhiteSpace(symbol)) state = State.S;
                        else if (char.IsDigit(symbol))
                        {
                            state = State.CON;
                            lexBufCur.Append(symbol);
                        }
                        else if (symbol == '<')
                        {
                            state = State.COMPN;
                            lexBufNext.Append(symbol);
                        }
                        else if (symbol == '>' || symbol == '=')
                        {
                            state = State.COMP;
                            lexBufNext.Append(symbol);
                        }
                        else if (symbol == '+' || symbol == '-' || symbol == '/' || symbol == '*')
                        {
                            state = State.ARIFM;
                            lexBufNext.Append(symbol);
                        }
                        else
                        {
                            state = State.ERR;
                            add = false;
                        }
                        break;
                    case State.ID:
                        if (char.IsWhiteSpace(symbol)) state = State.S;
                        else if (char.IsDigit(symbol) || char.IsLetter(symbol))
                        {
                            state = State.ID;
                            add = false;
                            lexBufCur.Append(symbol);
                        }
                        else if (symbol == '<')
                        {
                            state = State.COMPN;
                            lexBufNext.Append(symbol);
                        }
                        else if (symbol == '>' || symbol == '=')
                        {
                            state = State.COMP;
                            lexBufNext.Append(symbol);
                        }
                        else if (symbol == '+' || symbol == '-' || symbol == '/' || symbol == '*')
                        {
                            state = State.ARIFM;
                            lexBufNext.Append(symbol);
                        }
                        else if (symbol == ':')
                        {
                            state = State.ASSIGN;
                            lexBufNext.Append(symbol);
                        }
                        else
                        {
                            state = State.ERR;
                            add = false;
                        }
                        break;
                    case State.ARIFM:
                        if (char.IsWhiteSpace(symbol))
                        {
                            state = State.S;
                        }
                        else if (char.IsLetter(symbol))
                        {
                            state = State.ID;
                            lexBufNext.Append(symbol);
                        }
                        else if (char.IsDigit(symbol))
                        {
                            state = State.CON;
                            lexBufNext.Append(symbol);
                        }
                        else
                        {
                            state = State.ERR;
                            add = false;
                        }
                        break; 
                }
                if (add)
                {
                    AddLex(prevState, lexBufCur.ToString());
                    lexBufCur = new StringBuilder(lexBufNext.ToString());
                    lexBufNext.Clear();
                }
                textIndex++;
            }

            return state == State.FIN;

        }

        private void AddLex(State prevState, string value)
        {
            LexType lexType = LexType.Undefined;
            LexClass lexClass = LexClass.Undefined;
            if(prevState == State.ARIFM)
            {
                lexType = LexType.Ao;
                lexClass = LexClass.SpecialSymbols;
            }
            else if(prevState == State.ASSIGN)
            {
                lexType = LexType.As;
                lexClass = LexClass.SpecialSymbols;
            }
            else if(prevState == State.CON)
            {
                lexType = LexType.Undefined;
                lexClass = LexClass.Constant;
            }
            else if(prevState == State.COMPN)
            {
                lexType = LexType.Rel;
                lexClass = LexClass.SpecialSymbols;
            }
            else if(prevState == State.COMP)
            {
                lexType = LexType.Rel;
                lexClass = LexClass.SpecialSymbols;
            }
            else if(prevState == State.ID)
            {
                bool isKeyword = true;
                if (value.ToLower() == "not") lexType = LexType.Not;
                else if (value.ToLower() == "and") lexType = LexType.And;
                else if (value.ToLower() == "or") lexType = LexType.Or;
                else if (value.ToLower() == "loop") lexType = LexType.Loop;
                else if (value.ToLower() == "output") lexType = LexType.Output;
                else if (value.ToLower() == "do") lexType = LexType.Do;
                else if (value.ToLower() == "until") lexType = LexType.Until;
                else
                {
                    lexType = LexType.Undefined;
                    isKeyword = false;
                }
                if (isKeyword) lexClass = LexClass.Keyword;
                else lexClass = LexClass.Identifier;
            }

            Tokens.Add(new Lex(lexType, value, lexClass));
        }
    }
}
