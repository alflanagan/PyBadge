#! /usr/bin/gforth
\ -*- mode: forth; coding: us-ascii -*-
\ various useful routines for integers

: 3dup ( u1 u2 u3 -- u1 u2 u3 u1 u2 u3 )
    dup 2over rot ;

: cls ( u1 .. un -- )
    \ clear stack
    depth 0 u+do drop loop ; 

: 4drop ( u1 u2 u3 u4 -- )
    2drop 2drop ;

: clearmem ( addr u -- )
    \ set cells from addr to addr+u to 0
    0 u+do
        dup 0 swap i cells + ! loop ;

: setmem ( addr u n -- )
    \ set cells from addr to addr+u to n
    swap 0 u+do
        2dup swap i cells + ! loop ;

: gcd
    ( n1 n2 -- n3 )  \ Euclidean algorithm
    over - abs
    2dup
    <> if
        swap  \ keep order of operands, or inf. loop
        recurse
    else
        drop \ duplicate value
    then ;
