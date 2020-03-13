using Antlr4.Runtime;
using ConsoleApp2.Implementation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string text = @"
                var teste = null;
                var b = 4.1 * 2;
                if (teste == 3) 
                {
                    teste = 1.5;
                    b = 2;
                }
                else
                {
                    b = 1;
                    teste = 2;
                }
                teste *= 2; 
                teste + b;";

            var result = Execute(text);

            Console.WriteLine("## Exemplo");
            Console.WriteLine($"Fórmula: {text}");
            Console.WriteLine($"Resultado Final: {result.Value}");

            //TestConcurrentDictionary();
        }

        private static ExemploParser Setup(string text)
        {
            ICharStream target = new AntlrInputStream(text);
            ITokenSource lexer = new ExemploLexer(target);
            ITokenStream tokens = new CommonTokenStream(lexer);
            return new ExemploParser(tokens)
            {
                BuildParseTree = true
            };
        }

        private static ExemploValue Execute(string text)
        {
            var parser = Setup(text);
            var defaultParserTree = parser.rule_set();

            var visitor = new ExemploVisitorFinal();
            return visitor.Visit(defaultParserTree);
        }

        private static void TestConcurrentDictionary()
        {
            Console.WriteLine("## Concurrent Dictionary");

            var exceptions = new ConcurrentQueue<Exception>();
            var results = new ConcurrentDictionary<int, decimal>();

            var operacoes = new string[] { "+", "-", "*", "/", "^" };

            int size = 1000000;
            var scripts = new string[size];

            var random = new Random();
            for (int i = 0; i < size; i++)
            {
                scripts[i] = string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4}", random.Next(1, 250) + random.NextDouble(), operacoes[random.Next(0, 4)], random.Next(1, 250) + random.NextDouble(), operacoes[random.Next(0, 4)], random.Next(1, 250) + random.NextDouble());
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.For(0, size, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
            {
                try
                {
                    var result = Execute(scripts[i]);
                    results.TryAdd(i, (decimal)result);
                }
                catch (Exception e)
                {
                    exceptions.Enqueue(e);
                    throw e;
                }
            }); ;
            stopwatch.Stop();
            Console.WriteLine($"Tempo: {stopwatch.Elapsed}");
            Console.WriteLine($"Quantidade registros: {results.Count}");
        }
    }
}
