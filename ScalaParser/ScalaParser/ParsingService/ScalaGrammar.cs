using Irony.Parsing;

namespace ScalaParser
{


    public class ScalaGrammar : Grammar
    {
        public ScalaGrammar()
        {
            var identifier = TerminalFactory.CreateCSharpIdentifier("identifier");
            var number = TerminalFactory.CreateCSharpNumber("number");
            var addOperator = ToTerm("+", "operator"); // Пример оператора
            var defKeyword = ToTerm("def");

            var function = new NonTerminal("function");
            var expression = new NonTerminal("expression");

            function.Rule = defKeyword + identifier + "(" + identifier + ")" + "{" + expression + "}";
            expression.Rule = number | identifier | (identifier + addOperator + number);

            Root = function;
        }
    }
}

