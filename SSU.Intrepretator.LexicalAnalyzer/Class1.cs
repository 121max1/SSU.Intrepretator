using System;

namespace SSU.Intrepretator.LexicalAnalyzer
{
    enum State { S, E, F, Ai, Ac, As, Bs, Cs, Ds, Gs }
    enum LexType {Do, Until, Loop, Not, And, Or, Output, Rel, Ao, As }
    public class LexixalAnylyser
    {
       public void LexAnalyzer(string text)
        {
            State state = State.S, prevState;
            bool add;

            int textIndex = 0;
            while(state != State.E && state!= State.F)
            {
                prevState = state;
                add = true;
                char symbol = text[textIndex];
                switch (state)
                {
                    case State.S:
                        if (char.IsWhiteSpace(symbol)) state = State.S;
                        else if (char.IsDigit(symbol)) state = State.Ac;
                        else if (char.IsLetter(symbol)) state = State.Ai;
                        else if (symbol == '<') state 

                            break;
                    case
                }
            }

        }
    }
}
