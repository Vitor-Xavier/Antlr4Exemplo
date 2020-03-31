namespace Antlr4Exemplo.Models
{
    public class ExemploError
    {
        public int Line { get; set; }

        public int Column { get; set; }

        public string Char { get; set; }

        public string Message { get; set; }
    }
}
