#! /usr/bin/gforth

needs ex_23_1.fs

create v3
5 , 4 , 3 , 2 , 1 ,

." sum is = "

v3 5 vsum . CR

create v1
5 ,

." sum is = "

v1 1 vsum . CR

variable v20 20 cells allot

v20 20 vsum . CR

: testxx ( -- ) 20 0 u+do
    v20 i cells + @ . CR loop ;
