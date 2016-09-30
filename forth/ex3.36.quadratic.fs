#! /usr/bin/gforth

\ Assignment: Write a program to solve a quadratic equation.  Then
\ read 'Henry G. Baker, You Could Learn a Lot from a Quadratic
\ (http://home.pipeline.com/~hbaker1/sigplannotices/sigcol05.ps.gz),
\ ACM SIGPLAN Notices, 33(1):30-39, January 1998', and see if you can
\ improve your program.  Finally, find a test case where the original
\ and the improved version produce different results.

include fuseful.fs

: fdiscrim ( fa fb fc -- fa fb fc fd )
    \ compute the discriminant b^2-4ac of 3 floating-point numbers
    fover fdup f* \ b^2
    3 fpick 2 fpick 4e f* f* \ 4ac
    f- ;

: fquad ( fa fb fc -- fa fb fc fd fe )
    \ compute two roots of the quadratic a + bx + cx^2
    fdiscrim fsqrt 2 fpick fnegate fswap \ a b c -b sqrt(b^2-4ac)
    f2dup f+ \ a b c -b sqrt(b^2-4ac) (-b+sqrt(b^2-4ac))
    f-rot f- \ a b c (-b+sqrt(b^2-4ac)) (-b-sqrt(b^2-4ac))
    4 fpick 2e f* ftuck \ a b c (-b+sqrt(b^2-4ac)) 2a (-b-sqrt(b^2-4ac)) 2a
    f/ f-rot f/ ; \ a b c ((-b-sqrt(b^2-4ac))/2a) ((-b+sqrt(b^2-4ac))/2a)