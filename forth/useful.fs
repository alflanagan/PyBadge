#! /usr/bin/gforth

: 3dup ( u1 u2 u3 -- u1 u2 u3 u1 u2 u3 )
    dup 2over rot ;

: cls ( u1 .. un -- )
    \ clear stack
    depth 0 u+do drop loop ; 

: 4drop ( u1 u2 u3 u4 -- )
    2drop 2drop ;

