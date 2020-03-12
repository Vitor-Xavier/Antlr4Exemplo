grammar Exemplo;

/*
 * Parser Rules
 */

rule_set
	: rule_block*
	;

rule_block
    : assignment
    | variable_declaration
    | expression
    | statement
    ;

expression  
    : arithmetic_expression
    | comparison_expression
    | loop_expression
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
    | comparison_expression AND comparison_expression                   #andComparisonExpression
    | comparison_expression OR comparison_expression                    #orComparisonExpression
    | LPAREN comparison_expression RPAREN                               #parenthesisComparisonExpression
    ;

loop_expression
    : WHILE LPAREN comparison_expression RPAREN LBRACE rule_block* RBRACE #whileExpression
    ;

/* 
* null_coalescing_expression
*    : arithmetic_expression (COALESCE_OPERATOR null_coalescing_expression)? #coalesceExpression
*    ;
*/

variable_declaration
    : VAR VARIABLE ASSIGNMENT expression SEMI #variableDeclaration
    ;

assignment
    : VARIABLE assignment_operator expression SEMI #variableAssignment
	;

statement
    : IF LPAREN comparison_expression RPAREN LBRACE rule_block* RBRACE (ELSE LBRACE rule_block RBRACE)? #ifStatement
    ;
atom
    : NUMBER #numberAtom
    | NULL #nullAtom
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

assignment_operator
    : ASSIGNMENT
    | PLUS_ASSIGNMENT
    | MINUS_ASSIGNMENT
    | DIV_ASSIGNMENT
    | TIMES_ASSIGNMENT
    ;

/*
 * Lexer Rules
 */

ASSIGNMENT: '=';
PLUS_ASSIGNMENT: '+=';
MINUS_ASSIGNMENT: '-=';
DIV_ASSIGNMENT: '/=';
TIMES_ASSIGNMENT: '*=';

GT: '>';
GE: '>=';
LT: '<';
LE: '<=';
EQ: '==';
NEQ: '!=';

PLUS: '+';
MINUS: '-';
DIV: '/';
TIMES: '*';

LBRACE: '{';
RBRACE: '}';

LPAREN: '(';
RPAREN: ')';

SEMI: ';';
COMMA: ',';

WHITESPACE: [ \r\t\u000C\n]+ -> skip;

SINGLE_LINE_COMMENT: '//'  .*? -> skip;
DELIMITED_COMMENT: '/*'  .*? '*/' -> skip;

AND: '&&';
OR: '||';

IF: 'if';
ELSE: 'else';
WHILE: 'while';

COALESCE_OPERATOR: '??';

NULL: 'null';
VAR: 'var';

NUMBER: '-'?[0-9]+('.'[0-9]+)?;
VARIABLE: [a-zA-Z_][a-zA-Z_0-9]*;