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
            var resultLex = anylyser.LexAnalyzer("begin a:=(begin1+5*(2+2)*2); b:=1; do until a<5 a:=a+1; output a; b:=b*2; output b; loop end");
            Console.WriteLine(resultLex);
            if (resultLex)
            {
                foreach (var item in anylyser.Tokens)
                {
                    Console.WriteLine($"Class: {item.Class}, Type: {item.Type}, Value {item.Value}, Id: {item.Id}");
                }
            }
            var syntAnalyzer = new SyntAnalyzer(anylyser.Tokens);
            var resultSynt = syntAnalyzer.Run();
            Console.WriteLine($"Результат: {resultSynt.Value}, Сообщение: {resultSynt.Message}");

        }
    }
}
