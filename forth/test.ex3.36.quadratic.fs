include ex3.36.quadratic.fs

: f3drop ( f1 f2 f3 -- )
    fdrop fdrop fdrop ;

: fsempty? ( f* -- )
    fdepth 0> if
        ." Error: fp stack depth is: " fdepth . CR
        fcls
    then ;

: assert_fdepth ( n -- )
    fdepth 2dup <> if
        ." Error: fdepth is " . ." but should be " . CR
    else
        2drop
    then ;

: checkquad ( f1 f2 f1` f2` -- )
    0 \ error status
    frot f2dup 9 assert_fdepth f<>  if
        ." Error: second root should be " f. ." but is " f. CR
        1+
    else
        fdrop fdrop
    then
    f2dup f<> if
        ." Error: first root should be " f. ." but is " f. CR
        1+ 1+
    else
        fdrop fdrop
    then
    f3drop  \ drop coefficients
    fdepth 0> if
        ." Error in checkquad: fp stack depth is " fdepth . ." on exit" CR
        fcls
        4 +
    then
    0= if
        ." passed!" CR
    then ;
    
." fdiscrim tests" CR

1e 4e 3e fdiscrim f. CR \ 4.0
f3drop fsempty?
2e 4e 2e fdiscrim f. CR \ 0.0
f3drop fsempty?
1e 3e 2.5e fdiscrim f. CR \ -1.0
f3drop fsempty?
3e 9e 7e fdiscrim f. CR \ -3.0
f3drop fsempty?

." fquad tests" CR

." x^2 + 4x + 4 = 0 " 
1e 4e 4e fquad
-2e -2e checkquad
." x^2 -4x + 4 = 0 "
1e -4e 4e fquad
2e 2e checkquad

bye
