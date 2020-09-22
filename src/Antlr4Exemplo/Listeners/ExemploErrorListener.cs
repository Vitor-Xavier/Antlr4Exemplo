using Antlr4.Runtime;
using Antlr4Exemplo.Models;
using System.Collections.Generic;
using System.IO;

namespace Antlr4Exemplo.Listeners
{
    public class ExemploErrorListener : BaseErrorListener
    {
        public ICollection<ExemploError> ExemploErrors { get; } = new HashSet<ExemploError>();

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            var message = e switch
            {
                NoViableAltException _ => $"Expressão inválida '{offendingSymbol}'",
                InputMismatchException _ => $"Entrada '{offendingSymbol}' não esperada",
                FailedPredicateException _ => $"Semántica '{offendingSymbol}' inválida",
                _ => "Erro indefinido"
            };
            ExemploErrors.Add(new ExemploError { Line = line, Column = charPositionInLine, Char = offendingSymbol.Text, Message = message });
        }
    }
}
