using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Antlr4Exemplo.Listeners;
using System.Collections.Generic;

namespace Antlr4Exemplo.Implementation
{
    public static class ExemploHandler
    {
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

        /// <summary>
        /// Interpreta uma entrada de texto em uma árvore estruturada.
        /// </summary>
        /// <param name="text">Texto</param>
        /// <returns>Árvore estruturada</returns>
        public static IParseTree Evaluate(string text, ExemploErrorListener exemploErrorListener = null)
        {
            var parser = Setup(text);

#if RELEASE
            parser.Interpreter.PredictionMode = Antlr4.Runtime.Atn.PredictionMode.SLL;
#else
            parser.Interpreter.PredictionMode = Antlr4.Runtime.Atn.PredictionMode.LL;
#endif

            if (exemploErrorListener != null)
            {
                parser.RemoveErrorListeners();
                parser.AddErrorListener(exemploErrorListener);
            }

            return parser.rule_set();
        }

        /// <summary>
        /// Analisa e executa uma entrada de texto.
        /// </summary>
        /// <param name="text">Texto</param>
        /// <param name="externalMemory">Memória Externa para a execução</param>
        /// <returns>Resultado da Execução</returns>
        public static ExemploValue Execute(string text, IDictionary<string, ExemploValue> externalMemory)
        {
            var defaultParserTree = Evaluate(text);

            var visitor = new ExemploVisitor(externalMemory);
            return visitor.Visit(defaultParserTree);
        }

        /// <summary>
        /// Executa uma fórmula já analisada.
        /// </summary>
        /// <param name="parseTree">Fórmula analisada</param>
        /// <param name="externalMemory">Memória Externa para a execução</param>
        /// <returns>Resultado da Execução</returns>
        public static ExemploValue Execute(IParseTree parseTree, IDictionary<string, ExemploValue> externalMemory)
        {
            var visitor = new ExemploVisitor(externalMemory);
            return visitor.Visit(parseTree);
        }
    }
}
