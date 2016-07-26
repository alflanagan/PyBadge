#! /usr/local/bin/gforth
\ calculate (a + b)(a - b)
: ex36last ( n1 n2 -- n3 )
    2dup - -rot + * ;

4 5 ex36last . CR
2 6 ex36last . CR
6 2 ex36last . CR

: ex36last { a b -- (a-b)(a+b) } a b - a b +  * ;

4 5 ex36last . CR
2 6 ex36last . CR
6 2 ex36last . CR

bye