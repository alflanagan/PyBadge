\ routines for allocating memory and equivalents of calloc() and memset()
\ see 

: vsum ( addr u -- n )
    \ n is the sum of u cells, with the first cell at addr
    0 -rot 0  u+do  \ stack in loop:  sum addr
        dup i cells + @ \ calculate array offset, get value
        \ sum addr value
        rot
        \ addr value sum
        + swap
    loop drop ;

: clearmem ( addr u -- )
    \ set cells from addr to addr+u to 0
    0 u+do
        dup 0 swap i cells + ! loop ;

: setmem ( addr u n -- )
    \ set cells from addr to addr+u to n
    swap 0 u+do
        2dup swap i cells + ! loop ;
