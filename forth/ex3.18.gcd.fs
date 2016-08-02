#! /usr/bin/env gforth

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
    
