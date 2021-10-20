using System;
using System.Collections.Generic;
using System.Text;

namespace SSU.Intrepretator.SyntaxAnalyzer
{
    public class Result
    {
        public Result(bool value, string message)
        {
            Value = value;
            Message = message;
        }

        public bool Value { get; private set; }

        public string Message { get; private set; }
  
    }
}
