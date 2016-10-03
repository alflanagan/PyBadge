\ convention: [word] means word has non-default compilation semantics

: [foo] 5 . ; immediate
: bar [foo] ; \ 5  ok
see bar
: xxx [ 5 . ] ; \ 5  ok
see xxx 

: map-array ( ... addr u xt -- ... )
    \ executes xt ( ... x -- ... ) for every element of the array starting
    \ at addr and containing u elements
    { xt }
    cells over + swap ?do
        i @ xt execute
    1 cells +loop ;

create a 3 , 4 , 2 , -1 , 4 ,
a 5 ' . map-array .s
0 a 5 ' + map-array .
s" max-n" environment? drop .s
a 5 ' min map-array .

: constant ( n "name" -- )
    create ,
  does> ( -- n )
    ( addr ) @ ;

5 constant foo
foo .