#! /usr/bin/gforth

\ Assignment: Write a program to solve a quadratic equation.  Then
\ read 'Henry G. Baker, You Could Learn a Lot from a Quadratic
\ (http://home.pipeline.com/~hbaker1/sigplannotices/sigcol05.ps.gz),
\ ACM SIGPLAN Notices, 33(1):30-39, January 1998', and see if you can
\ improve your program.  Finally, find a test case where the original
\ and the improved version produce different results.

: f-rot frot frot ;

: f2swap ( f1 f2 f3 f4 -- f3 f4 f1 f2 )
    pad f! f-rot pad f@ f-rot ;

: fdeterm ( a b c -- d )
    \ compute the determinant b^2-4ac of 3 floating-point numbers
    fswap fdup f* f-rot 4e f* f* f- ;

: 3dup ( u1 u2 u3 -- u1 u2 u3 u1 u2 u3 )
    dup 2over rot ;

: f2dup ( f1 f2 -- f1 f2 f1 f2 )
    fover fover ;

: fcls ( f1 f2 ... -- )
    \ clear float stack
    fdepth 0 u+do
        drop
    loop ;

: f2over ( f1 f2 f3 f4 -- f1 f2 f3 f4 f1 f2 )
    pad f! pad float + f! f2dup pad float + f@ f-rot pad f@ f-rot ;

: f3dup ( u1 u2 u3 -- u1 u2 u3 u1 u2 u3 )
    fdup f2over frot ;

: fquad ( a b c -- d e )
    \ compute two roots of the quadratic a + bx + cx^2
    f3dup fdeterm ;
