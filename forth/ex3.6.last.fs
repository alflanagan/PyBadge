#! /usr/local/bin/gforth
\ calculate (a + b)(a - b)
: ex36last ( n1 n2 -- n3 )
    2dup - -rot + * ;

\ using gforth variable feature
: ex36last2 { a b -- (a-b)(a+b) } a b - a b +  * ;
