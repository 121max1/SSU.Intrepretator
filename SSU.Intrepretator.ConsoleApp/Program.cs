using System;
using SSU.Intrepretator.Intrepretator;
using SSU.Intrepretator.LexicalAnalyzer;
using SSU.Intrepretator.SyntaxAnalyzer;

namespace SSU.Intrepretator.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LexixalAnylyser anylyser = new LexixalAnylyser();
            //var resultLex = anylyser.LexAnalyzer("begin a:=0; b:=1; do until a<10 a:=a + 1; b:=b*2; output b; loop end");
            //var resultLex = anylyser.LexAnalyzer("begin a:=0; b:=1; a:= b + 2; output a; output b; end");
            var resultLex = anylyser.LexAnalyzer("begin a:=1; b:=1; do until a<10 do until b<10 c:=a*b; output c; b:= b+1; loop a:=a+1; output 1111111111; b:=1; loop end");
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

            Console.WriteLine(syntAnalyzer.GetInversedPolishNotation());
            var intrepretator = new IntrepretatorWorker(syntAnalyzer._reversePolishNotation,
                syntAnalyzer._constants,
                syntAnalyzer._variables,
                syntAnalyzer._pointers);
            Console.WriteLine("Result:");
            intrepretator.Run();

        }
    }
}
