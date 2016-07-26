#! /usr/bin/gforth
vocabulary foo also foo definitions

\ define 'min' without using 'if'

: min ( n1 n2 -- min[n1,n2] )
    2dup <
    \ copy that result to pair with first operand, inverse pairs with 2nd
    tuck invert
    \ replace top 2 with min or 0
    and
    \ and move other pair to top
    -rot
    \ replace top 2 with min or 0
    and
    \ now one value is 0 and other is the minimum, so
    or ;

see min CR

5 6 min . CR 6 5 min . CR 3 3 min . CR

: min2 { a b -- min(a,b) }
    \ get first operand or 0
    a a b < and
    \ get 2nd operand or 0
    b b a <= and
    \ or b a b < invert and
    \ select minimum
    or ;

see min2 CR
    
4 7 min2 . CR 7 4 min2 . CR 3 3 min2 . CR bye