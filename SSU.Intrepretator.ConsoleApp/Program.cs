using System;
using SSU.Intrepretator.LexicalAnalyzer;

namespace SSU.Intrepretator.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LexixalAnylyser anylyser = new LexixalAnylyser();
            var result = anylyser.LexAnalyzer("do until a<>5 a:a+1 loop");
            Console.WriteLine(result);
            if (result)
            {
                foreach (var item in anylyser.Tokens)
                {
                    Console.WriteLine($"Class: {item.Class}, Type: {item.Type}, Value {item.Value}, Id: {item.Id}");
                }
            }
        }
    }
}
