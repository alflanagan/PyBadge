#! /usr/bin/gforth

\ define 'min' without using 'else'

vocabulary foo also foo definitions
\ above creates new namespace -- stop warning

: min ( n1 n2 -- n ) 2dup >= if swap then drop ;

CR 5 7 min . CR

3 7 4 12 min min min . CR

\ but this still gives warning, as we're in same namespace
: min { a b -- n } a b a b >= if swap then drop ;

bye