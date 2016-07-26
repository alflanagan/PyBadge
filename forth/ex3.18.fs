#! /usr/bin/gforth

: log2 ( +n1 -- n2 )
    \ logarithmus dualis of n1>0, rounded down to the next integer
    assert( dup 0> )
    2/ 0 begin
        over 0> while
            1+ swap 2/ swap
    repeat
    nip ;
