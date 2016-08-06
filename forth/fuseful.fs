#! /usr/bin/gforth
\ -*- mode: forth; coding: us-ascii -*-
\ various useful routines for floating-point values
\ mostly implementing standard integer ops for fp stack

: f-rot frot frot ;

: f2swap ( f1 f2 f3 f4 -- f3 f4 f1 f2 )
    pad f! f-rot pad f@ f-rot ;

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
