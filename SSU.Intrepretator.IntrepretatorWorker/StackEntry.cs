using System;
using System.Collections.Generic;
using System.Text;

namespace SSU.Intrepretator.Intrepretator
{
    public enum StackEntryType {Variable, Constant}
    public class StackEntry
    {
        public StackEntryType StackEntryType { get; set; }

        public int Value { get; set; }

        public string VariableName { get; set; }

    }
}
