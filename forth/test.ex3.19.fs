#! /usr/bin/gforth

include ex3.19.fs

: pow-print ( n u -- n)
    2dup swap . ." ^ " . ." == " ^ . CR ;

4 2 pow-print
4 3 pow-print

\ need a general purpose memoization routine:
\ given x, check table for f(x). if not present,
\ calculate f(x), add table entry
: showfib 25 0 u+do
        i fib . BL
    loop
CR ;

showfib

100 fib CR

: testfac
    ." factorial 5 is: "
    5 fac . CR
    ." factorial 12 is: "
    12 fac . CR
    ." factorial 25 is: "
    25 fac . CR
    \    ." factorial -3 is: "
    \    -3 fac . CR
    \ https://en.wikipedia.org/wiki/Gamma_function
;

testfac

bye
