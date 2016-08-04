#! /usr/bin/gforth

\ implement standard 'type'

: my-type ( addr u -- )
    bounds u+do
        i @ emit
        loop ;