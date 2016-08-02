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

bye
