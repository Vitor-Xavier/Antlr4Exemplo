grammar Exemplo;

/*
 * Parser Rules
 */

rule_set
	: rule_block*
	;

rule_block
    : assignment
    | expression
    ;

expression  : arithmetic_expression
            | comparison_expression
            ;

arithmetic_expression
            : arithmetic_expression PLUS arithmetic_expression #plusExpression
            | arithmetic_expression MINUS arithmetic_expression #minusExpression
            | arithmetic_expression DIV arithmetic_expression #divExpression
            | arithmetic_expression TIMES arithmetic_expression #timesExpression
            | LPAREN arithmetic_expression RPAREN #parenthesisExpression
            | atom #atomExpression
            ;

comparison_expression
    : arithmetic_expression comparison_operator arithmetic_expression   #comparisonExpression
    | LPAREN comparison_expression RPAREN                               #parenthesisComparisonExpression
    ;

assignment
    : VAR* VARIABLE ASSIGNMENT expression SEMI #variableAssignment
	;

atom
   : NUMBER #numberAtom
   | variable #variableAtom
   ;

variable
   : VARIABLE
   ;

comparison_operator
    : GT
    | GE
    | LT
    | LE
    | EQ
    | NEQ
    ;

/*
 * Lexer Rules
 */

VAR: 'var';

NUMBER: '-'?[0-9]+('.'[0-9]+)?;
VARIABLE: [a-zA-Z_][a-zA-Z_0-9]*;

ASSIGNMENT: '=';

GT : '>' ;
GE : '>=' ;
LT : '<' ;
LE : '<=' ;
EQ : '==' ;
NEQ : '!=' ;

PLUS: '+';
MINUS: '-';
DIV: '/';
TIMES: '*';

LPAREN : '(' ;
RPAREN : ')' ;

SEMI: ';';

WHITESPACE: ' ' -> skip ;

SINGLE_LINE_COMMENT: '//'  .*? -> skip;
DELIMITED_COMMENT: '/*'  .*? '*/' -> skip;