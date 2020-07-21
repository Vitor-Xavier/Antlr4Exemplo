using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Antlr4Exemplo.Implementation;
using Antlr4Exemplo.Listeners;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Antlr4Exemplo
{
    class Program
    {
        private static readonly ExemploErrorListener _exemploErrorListener = new ExemploErrorListener();

        static void Main()
        {
            Console.WriteLine("Antlr 4 C# Exemplo\n");

            string text = @"
                var b = 100 / 10 * @Teste;
                b;";

            var result = Execute(text, new Dictionary<string, ExemploValue> { { "Teste", new ExemploValue(10) } });

            if (_exemploErrorListener.ExemploErrors.Any())
            {
                Console.WriteLine("## Erro(s) de sintaxe");
                foreach (var exemploError in _exemploErrorListener.ExemploErrors)
                {
                    Console.WriteLine($"Linha: {exemploError.Line}\nColuna: {exemploError.Column}\nCarácter: {exemploError.Char}\nMensagem: {exemploError.Message}\n");
                }
                return;
            }

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

        private static IParseTree Evaluate(string text)
        {
            var parser = Setup(text);

#if RELEASE
            parser.Interpreter.PredictionMode = Antlr4.Runtime.Atn.PredictionMode.SLL;
#else
            parser.Interpreter.PredictionMode = Antlr4.Runtime.Atn.PredictionMode.LL;
#endif

            parser.RemoveErrorListeners();
            parser.AddErrorListener(_exemploErrorListener);

            return parser.rule_set();
        }

        /// <summary>
        /// Analisa e executa uma entrada de texto.
        /// </summary>
        /// <param name="text">Texto</param>
        /// <param name="externalMemory">Memória Externa para a execução</param>
        /// <returns>Resultado da Execução</returns>
        private static ExemploValue Execute(string text, IDictionary<string, ExemploValue> externalMemory = null)
        {
            var defaultParserTree = Evaluate(text);

            var visitor = new ExemploVisitorFinal(externalMemory);
            return visitor.Visit(defaultParserTree);
        }

        /// <summary>
        /// Executa uma fórmula já analisada.
        /// </summary>
        /// <param name="parseTree">Fórmula analisada</param>
        /// <param name="externalMemory">Memória Externa para a execução</param>
        /// <returns>Resultado da Execução</returns>
        private static ExemploValue Execute(IParseTree parseTree, IDictionary<string, ExemploValue> externalMemory = null)
        {
            var visitor = new ExemploVisitorFinal(externalMemory);
            return visitor.Visit(parseTree);
        }

        private static void TestConcurrentDictionary()
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
