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

high_precedence_operator
    : TIMES 
    | DIV
    ;

low_precedence_operator
    : PLUS 
    | MINUS
    ;

arithmetic_expression
    : arithmetic_expression POW arithmetic_expression #powExpression
    | arithmetic_expression high_precedence_operator arithmetic_expression #highPrecedenceExpression
    | arithmetic_expression low_precedence_operator arithmetic_expression #lowPrecedenceExpression
    | LPAREN arithmetic_expression RPAREN #parenthesisExpression
    | null_coalescing_expression #coalesceArithmeticExpression
    | atom #atomExpression
    ;

comparison_expression
    : arithmetic_expression comparison_operator arithmetic_expression   #comparisonExpression
    | comparison_expression AND comparison_expression                   #andComparisonExpression
    | comparison_expression OR comparison_expression                    #orComparisonExpression
    | LPAREN comparison_expression RPAREN                               #parenthesisComparisonExpression
    ;

loop_expression
    : WHILE LPAREN comparison_expression RPAREN LBRACE statement_block RBRACE #whileExpression
    ;
 
null_coalescing_expression
    : atom (COALESCE_OPERATOR null_coalescing_expression)? #coalesceExpression
    ;

variable_declaration
    : VAR VARIABLE ASSIGNMENT expression SEMI #variableDeclaration
    ;

assignment
    : VARIABLE assignment_operator expression SEMI #variableAssignment
	;

statement
    : IF LPAREN comparison_expression RPAREN statement_block (ELSE statement_block)? #ifStatement
    ;

statement_block
    : LBRACE rule_block* RBRACE #statementBraceBlock
    | rule_block #statementRuleBlock
    ;

atom
    : NUMBER #numberAtom
    | TEXT #textAtom
    | NULL #nullAtom
    | variable #variableAtom
    | external #externalAtom
    ;

variable
    : VARIABLE
    ;

external
    : EXTERNAL VARIABLE
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
POW: '^';

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

EXTERNAL: '@';

COALESCE_OPERATOR: '??';

NULL: 'null';
VAR: 'var';

NUMBER: '-'?[0-9]+('.'[0-9]+)?;
VARIABLE: [a-zA-Z_][a-zA-Z_0-9]*;
TEXT: '"' .*? '"';