using System;
using System.Text;

namespace SSU.Intrepretator.LexicalAnalyzer
{
    enum State { S, ID, REL, CON, ERR, FIN, COMP, COMPN, ARIFM, ASSIGN }
    enum LexType {Do, Until, Loop, Not, And, Or, Output, Rel, Ao, As }
    public class LexixalAnylyser
    {
       public void LexAnalyzer(string text)
        {
            State state = State.S, prevState;
            bool add;
            StringBuilder lexBuf = new StringBuilder();
            int textIndex = 0;
            while(state != State.ERR && state!= State.FIN)
            {
                prevState = state;
                add = true;
                char symbol = text[textIndex];
                if(textIndex != text.Length)
                {
                    state = State.FIN;
                    break;
                }
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
                            lexBuf.Append(symbol);
                        break;
                    case State.COMP:
                        if (char.IsWhiteSpace(symbol)) state = State.S;
                        else if (char.IsLetter(symbol)) state = State.ID;
                        else if (char.IsDigit(symbol)) state = State.CON;
                        else state = State.ERR;
                        break;
                    case State.COMPN:
                        if (char.IsWhiteSpace(symbol)) state = State.S;
                        else if (symbol == '>') state = State.COMP;
                        else if (char.IsLetter(symbol)) state = State.ID;
                        else if (char.IsDigit(symbol)) state = State.CON;
                        else state = State.ERR;
                        if (state != State.ERR && state != State.S)
                            lexBuf.Append(symbol);
                        break;
                    case State.ASSIGN:
                        if (symbol != '=') state = State.ERR;
                        else state = State.S;
                        if (state == State.S)
                            lexBuf.Append(symbol);
                        break;
                    case State.CON:
                        if (char.IsWhiteSpace(symbol)) state = State.S;
                        else if (char.IsDigit(symbol)) state = State.CON;
                        else if (symbol == '<') state = State.COMPN;
                        else if (symbol == '>' || symbol == '=') state = State.COMP;
                        else if (symbol == '+' || symbol == '-' || symbol == '/' || symbol == '*') state = State.ARIFM;
                        else state = State.ERR;
                        break;
                    case State.ID:
                        if (char.IsWhiteSpace(symbol)) state = State.S;
                        if (char.IsDigit(symbol) || char.IsLetter(symbol)) state = State.ID
                        break;

                }
            }

        }
    }
}
