using System;
using SSU.Intrepretator.LexicalAnalyzer;
using SSU.Intrepretator.SyntaxAnalyzer;

namespace SSU.Intrepretator.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LexixalAnylyser anylyser = new LexixalAnylyser();
            var result = anylyser.LexAnalyzer("begin do until a>5 b:=2+3+4*6 loop end");
            Console.WriteLine(result);
            if (result)
            {
                foreach (var item in anylyser.Tokens)
                {
                    Console.WriteLine($"Class: {item.Class}, Type: {item.Type}, Value {item.Value}, Id: {item.Id}");
                }
            }
            var syntAnalyzer = new SyntAnalyzer(anylyser.Tokens);
            Console.WriteLine(syntAnalyzer.Run());
        }
    }
}
