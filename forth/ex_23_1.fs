: vsum ( addr u -- n )
    \ n is the sum of u cells, with the first cell at addr
    0 -rot 0  u+do  \ stack in loop:  sum addr
        dup i cells + @ \ calculate array offset, get value
        \ sum addr value
        rot
        \ addr value sum
        + swap
    loop drop ;
