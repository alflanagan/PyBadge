#! /usr/bin/gforth
\ -*- mode: forth; coding: us-ascii -*-
\ various useful routines for floating-point values
\ mostly implementing standard integer ops for fp stack

: f-rot ( f1 f2 f3 -- f3 f1 fs )
    frot frot ;

: f2swap ( f1 f2 f3 f4 -- f3 f4 f1 f2 )
    pad f! f-rot pad f@ f-rot ;

: f2dup ( f1 f2 -- f1 f2 f1 f2 )
    fover fover ;

: fcls ( f1 .. fn -- )
    \ clear float stack
    fdepth 0 u+do
        fdrop
    loop ;

: f2over ( f1 f2 -- f1 f2 f1 f2 )
    fover fover ;

: f3dup ( f1 f2 f3 -- f1 f2 f3 f1 f2 f3 )
    \ fdup f2over frot ;
    2 fpick 2 fpick 2 fpick ; \ probably faster

