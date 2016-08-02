: ^ ( n1 u -- n )
    \ n = the uth power of n1
    1 swap 0 u+do
        over *
    loop
    nip ;

: fac ( u -- u! )
    1 swap 1+ 1 u+do
        i *
    loop ;

: unsafe-fib ( u -- u1 )
    dup 1 > if
        1- dup 1- recurse swap recurse +
    then ;

\ TODO: define to use constant stack
\ helper tail-recursive word?
: fib ( u -- u1 )
    \ u1 is uth fibonacci number
    dup 49 > if
        ." Refusing to calculate fib(" . ." ) as it would take > 10 minutes." 0 CR
    else
        unsafe-fib
    then ;

