using Irony.Parsing;

public class ScalaGrammar : Grammar
{
    public ScalaGrammar()
    {
        // Терминалы
        var identifier = TerminalFactory.CreateCSharpIdentifier("identifier");
        var number = TerminalFactory.CreateCSharpNumber("number");
        var stringLiteral = TerminalFactory.CreateCSharpString("string");

        var addOperator = ToTerm("+", "operator");
        var eqOperator = ToTerm("==", "operator");
        var assignOperator = ToTerm("=", "assign");

        var defKeyword = ToTerm("def", "keyword");
        var classKeyword = ToTerm("class", "keyword");
        var caseKeyword = ToTerm("case", "keyword");
        var varKeyword = ToTerm("var", "keyword");
        var valKeyword = ToTerm("val", "keyword");
        var ifKeyword = ToTerm("if", "keyword");
        var elseKeyword = ToTerm("else", "keyword");
        var matchKeyword = ToTerm("match", "keyword");
        var returnKeyword = ToTerm("return", "keyword");

        var listKeyword = ToTerm("List", "keyword");
        var optionKeyword = ToTerm("Option", "keyword");

        // Операции с коллекциями
        var find = ToTerm("find", "operator");
        var filterNot = ToTerm("filterNot", "operator");
        var map = ToTerm("map", "operator");
        var foreacher = ToTerm("foreach", "operator");

        // Символы
        var comma = ToTerm(",", "comma");
        var dot = ToTerm(".", "dot");
        var leftParen = ToTerm("(", "leftParen");
        var rightParen = ToTerm(")", "rightParen");
        var leftBrace = ToTerm("{", "leftBrace");
        var rightBrace = ToTerm("}", "rightBrace");
        var colon = ToTerm(":", "colon");
        var arrow = ToTerm("=>", "arrow");

        // Нетерминалы
        var function = new NonTerminal("function");
        var expression = new NonTerminal("expression");
        var statement = new NonTerminal("statement");
        var classDef = new NonTerminal("classDef");
        var caseClassDef = new NonTerminal("caseClassDef");
        var methodCall = new NonTerminal("methodCall");
        var matchExpression = new NonTerminal("matchExpression");
        var caseClause = new NonTerminal("caseClause");
        var list = new NonTerminal("list");
        var option = new NonTerminal("option");
        var paramList = new NonTerminal("paramList");
        var param = new NonTerminal("param");
        var typeName = new NonTerminal("typeName");

        // Тип данных (String, Int, Boolean, List[Book], Option[Book] и т. д.)
        typeName.Rule = identifier | (identifier + leftParen + typeName + rightParen) | (identifier + leftBrace + typeName + rightBrace);

        // Список параметров
        param.Rule = identifier + colon + typeName;
        paramList.Rule = MakeStarRule(paramList, comma, param);

        // Case-классы
        caseClassDef.Rule = caseKeyword + classKeyword + identifier + leftParen + paramList + rightParen;

        // Обычные классы
        classDef.Rule = classKeyword + identifier + leftBrace + statement + rightBrace;

        // Функции и методы
        function.Rule = defKeyword + identifier + leftParen + paramList + rightParen + colon + typeName + assignOperator + expression;

        // Вызовы методов
        methodCall.Rule = identifier + dot + identifier + leftParen + expression + rightParen;

        // Выражения
        expression.Rule = number | stringLiteral | identifier | methodCall | matchExpression | list | option;

        // Оператор if-else
        statement.Rule =
            expression |
            ifKeyword + leftParen + expression + rightParen + leftBrace + statement + rightBrace |
            ifKeyword + leftParen + expression + rightParen + leftBrace + statement + rightBrace + elseKeyword + leftBrace + statement + rightBrace;

        // Оператор match-case
        matchExpression.Rule = matchKeyword + expression + leftBrace + MakeStarRule(caseClause, caseKeyword + expression + arrow + statement) + rightBrace;

        // Операции со списками
        list.Rule = identifier + assignOperator + listKeyword + leftParen + MakeStarRule(new NonTerminal("elements"), comma, expression) + rightParen;

        // Опциональные значения
        option.Rule = optionKeyword + leftBrace + typeName + rightBrace + leftParen + expression + rightParen;

        // Корневой нетерминал
        var program = new NonTerminal("program");
        program.Rule = MakeStarRule(program, statement | classDef | caseClassDef | function);

        // Устанавливаем корневой элемент
        Root = program;

        // Игнорируем ненужные символы
        MarkPunctuation(",", ".", "(", ")", "{", "}", ":", "=>", "=");
    }
}
