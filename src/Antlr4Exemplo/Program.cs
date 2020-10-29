using Antlr4Exemplo.Implementation;
using Antlr4Exemplo.Listeners;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Antlr4Exemplo
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Antlr 4 C# Exemplo\n");

            string text = @"
                var b = 100 / 10 * @Teste;
                b;";

            var exemploErrorListener = new ExemploErrorListener();
            var parseTree = ExemploHandler.Evaluate(text, exemploErrorListener);
            var result = ExemploHandler.Execute(parseTree, new Dictionary<string, ExemploValue> { { "Teste", new ExemploValue(1) } });

            if (exemploErrorListener.ExemploErrors.Any())
            {
                Console.WriteLine("## Erro(s) de sintaxe");
                foreach (var exemploError in exemploErrorListener.ExemploErrors)
                {
                    Console.WriteLine($"Linha: {exemploError.Line}\nColuna: {exemploError.Column}\nCarácter: {exemploError.Char}\nMensagem: {exemploError.Message}\n");
                }
                return;
            }

            Console.WriteLine("## Exemplo");
            Console.WriteLine($"Fórmula: {text}");
            Console.WriteLine($"Resultado Final: {result.Value}\n");

            using var source = new CancellationTokenSource();

            _ = Task.Run(() =>
            {
                Console.WriteLine("Para cancelar a execução do teste pressione Enter.");
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Enter) source.Cancel();
            });
            try
            {
                TestConcurrentDictionary(source.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operação foi cancelada pelo usuário");
            }
        }

        private static void TestConcurrentDictionary(CancellationToken token)
        {
            Console.WriteLine("## Concurrent Dictionary");

            var results = new ConcurrentDictionary<int, decimal>();

            string texto = @"
                var result = null;
                if (@Operation == ""+"")
                    result = @FirstValue + @SecondValue;
                else if (@Operation == ""-"")
                    result = @FirstValue - @SecondValue;
                else if (@Operation == ""*"")
                    result = @FirstValue * @SecondValue;
                else if (@Operation == ""/"")
                    result = @FirstValue / @SecondValue;
                else if (@Operation == ""^"")
                    result = @FirstValue ^ @SecondValue;
                result;
            ";

            var exemploErrorListener = new ExemploErrorListener();
            var parseTree = ExemploHandler.Evaluate(texto, exemploErrorListener);

            if (exemploErrorListener.ExemploErrors.Any())
            {
                Console.WriteLine("## Erro(s) de sintaxe TestConcurrentDictionary");
                foreach (var exemploError in exemploErrorListener.ExemploErrors)
                {
                    Console.WriteLine($"Linha: {exemploError.Line}\nColuna: {exemploError.Column}\nCarácter: {exemploError.Char}\nMensagem: {exemploError.Message}\n");
                }
                return;
            }

            var operacoes = new string[] { "+", "-", "*", "/", "^" };

            int size = 1000000;
            var scripts = new IDictionary<string, ExemploValue>[size];

            var random = new Random();
            for (int i = 0; i < size; i++)
                scripts[i] = new Dictionary<string, ExemploValue> {
                    { "Operation", new ExemploValue(operacoes[random.Next(0, 4)]) },
                    { "FirstValue", new ExemploValue(random.Next(1, 250)) },
                    { "SecondValue", new ExemploValue(random.Next(1, 250)) }
                };

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = token,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.For(0, size, parallelOptions, i =>
            {
                var result = ExemploHandler.Execute(parseTree, scripts[i]);
                results.TryAdd(i, (decimal)result);

                parallelOptions.CancellationToken.ThrowIfCancellationRequested();
            });
            stopwatch.Stop();
            Console.WriteLine($"Tempo: {stopwatch.Elapsed}");
            Console.WriteLine($"Quantidade registros: {results.Count}");
        }

        private static void TestConcurrentDictionaryCustom()
        {
            Console.WriteLine("## Concurrent Dictionary");

            var exceptions = new ConcurrentQueue<Exception>();
            var results = new ConcurrentDictionary<int, decimal>();

            var operacoes = new string[] { "+", "-", "*", "/", "^" };

            int size = 10000;
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
                    var result = ExemploHandler.Execute(scripts[i], null);
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
