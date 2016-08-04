#! /usr/bin/gforth

\ Assignment: Write a program to solve a quadratic equation.  Then
\ read 'Henry G. Baker, You Could Learn a Lot from a Quadratic
\ (http://home.pipeline.com/~hbaker1/sigplannotices/sigcol05.ps.gz),
\ ACM SIGPLAN Notices, 33(1):30-39, January 1998', and see if you can
\ improve your program.  Finally, find a test case where the original
\ and the improved version produce different results.

: f-rot frot frot ;

: fdeterm ( a b c -- d )
    \ compute the determinant b^2-4ac of 3 floating-point numbers
    fswap fdup f* f-rot 4e f* f* f- fsqrt ;

