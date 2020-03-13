using Antlr4.Runtime;
using ConsoleApp2.Implementation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        class Envelope
        {
            public int EnvelopeId { get; set; }
            public string Formula { get; set; }
            public double Resultado { get; set; }
            public bool Set { get; set; }

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string text = @"
                var teste = null;
                if (teste == 3) 
                {
                    teste = 1;
                }
                else
                {
                    teste = 2;
                }
                teste *= 2; 
                10 ^ teste;";
            try
            {
                var parser = Setup(text);
                var defaultParserTree = parser.rule_set();

                var visitor = new ExemploVisitorFinal();
                var result = visitor.Visit(defaultParserTree);

                Console.WriteLine("## Exemplo");
                Console.WriteLine($"Fórmula: {text}");
                Console.WriteLine($"Resultado Final: {result.Value}");
            }
            catch (Exception e)
            {
                throw e;
            }

            //var listForeachLimite = new List<double>();
            //var listForeachFor = new List<double>();
            //var listForeach = new List<Envelope>();

            //Stopwatch stopwatch = new Stopwatch();

            //var list = new List<Envelope>();
            //var operacoes = new string[] { "+", "-", "*", "/" };
            //for (int i = 0; i < 1000000; i++)
            //{
            //    var random = new Random();
            //    var formula = string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4}", random.Next(1, 500) + random.NextDouble(), operacoes[random.Next(0, 3)], random.Next(1, 500) + random.NextDouble(), operacoes[random.Next(0, 3)], random.Next(1, 500) + random.NextDouble());
            //    list.Add(new Envelope
            //    {
            //        EnvelopeId = i,
            //        Formula = formula
            //    });
            //}

            //// List
            //stopwatch.Start();
            //Parallel.ForEach(list, new ParallelOptions { MaxDegreeOfParallelism = 100 }, (item, i) =>
            // {
            //     ExemploParser parser = Setup(item.Formula);
            //     ExemploParser.ExpressionContext expressionContext = parser.expression();

            //     var visitor = new ExemploVisitorFinal();
            //     item.Resultado = (double)visitor.Visit(expressionContext).Value;
            //     item.Set = true;
            // });
            //stopwatch.Stop();
            //Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
            //Console.WriteLine($"Count: {list.Where(x => x.Set).Count()}");

            // ConcurrentDictionary
            //Console.WriteLine("Concurrent dictionary");
            //var dictionaryLimite = new ConcurrentDictionary<int, double>();
            //stopwatch.Reset();
            //stopwatch.Start();
            //Parallel.ForEach(list, new ParallelOptions { MaxDegreeOfParallelism = 100 }, (item, i) =>
            //{
            //    ExemploParser parser = Setup(item.Formula);
            //    ExemploParser.ExpressionContext expressionContext = parser.expression();

            //    var visitor = new ExemploVisitorFinal();
            //    dictionaryLimite.TryAdd(item.EnvelopeId, (double)visitor.Visit(expressionContext).Value);
            //});
            //stopwatch.Stop();
            //Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
            //Console.WriteLine($"Count: {dictionaryLimite.Count}");

            // For
            //stopwatch.Reset();
            //stopwatch.Start();
            //list.ForEach(item =>
            //{
            //    TesteParser parser = Setup(item);
            //    TesteParser.ExpressionContext expressionContext = parser.expression();

            //    var visitor = new TesteVisitorFinal();
            //    listForeachFor.Add(visitor.Visit(expressionContext));
            //});
            //stopwatch.Stop();
            //Console.WriteLine($"Elapsed time For: {stopwatch.Elapsed}");
            //Console.WriteLine($"Count: {listForeachFor.Count}");

            // Sem limite
            //stopwatch.Reset();
            //stopwatch.Start();
            //Parallel.ForEach(list, (item, i) =>
            //{
            //    TesteParser parser = Setup(item);
            //    TesteParser.ExpressionContext expressionContext = parser.expression();

            //    var visitor = new TesteVisitorFinal();
            //    listForeach.Add(visitor.Visit(expressionContext));
            //});
            //stopwatch.Stop();
            //Console.WriteLine($"Elapsed time Sem limite: {stopwatch.Elapsed}");
            //Console.WriteLine($"Count: {listForeach.Count}");

            // Sem limite 
            //var cb = new ConcurrentBag<double>();
            //var bagTasks = new List<Task>();

            //stopwatch.Reset();
            //stopwatch.Start();

            //Parallel.ForEach(list, (item, i) =>
            //{
            //    TesteParser parser = Setup(item);
            //    TesteParser.ExpressionContext expressionContext = parser.expression();

            //    var visitor = new TesteVisitorFinal();
            //    bagTasks.Add(Task.Run(() => cb.Add(visitor.Visit(expressionContext))));
            //});
            //Task.WaitAll(bagTasks.ToArray());
            //stopwatch.Stop();
            //Console.WriteLine($"Elapsed time bag: {stopwatch.Elapsed}");
            //Console.WriteLine($"Bag items: {cb.Count}");
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

        private static void TestComparison()
        {
            string text = @"var teste = 10; teste += 20; 10 + teste;";
            try
            {
                ExemploParser parser = Setup(text);
                var defaultParserTree = parser.rule_set();

                var visitor = new ExemploVisitorFinal();
                var result = visitor.Visit(defaultParserTree);

                Console.WriteLine("## Exemplo");
                Console.WriteLine($"Resultado: {text} = {result.Value}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void TestConcurrentDictionary()
        {
            Console.WriteLine("## Concurrent Dictionary");

            var dictionaryLimite = new ConcurrentDictionary<int, double>();

            var list = new List<Envelope>();
            var operacoes = new string[] { "+", "-", "*", "/" };
            for (int i = 0; i < 1000000; i++)
            {
                var random = new Random();
                var formula = string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4}", random.Next(1, 500) + random.NextDouble(), operacoes[random.Next(0, 3)], random.Next(1, 500) + random.NextDouble(), operacoes[random.Next(0, 3)], random.Next(1, 500) + random.NextDouble());
                list.Add(new Envelope
                {
                    EnvelopeId = i,
                    Formula = formula
                });
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.ForEach(list, new ParallelOptions { MaxDegreeOfParallelism = 100 }, (item, i) =>
            {
                ExemploParser parser = Setup(item.Formula);
                ExemploParser.ExpressionContext expressionContext = parser.expression();

                var visitor = new ExemploVisitorFinal();
                dictionaryLimite.TryAdd(item.EnvelopeId, (double)visitor.Visit(expressionContext).Value);
            });
            stopwatch.Stop();
            Console.WriteLine($"Tempo: {stopwatch.Elapsed}");
            Console.WriteLine($"Quantidade registros: {dictionaryLimite.Count}");
        }
    }
}
