: fb." IMMEDIATE
    STATE @ IF	( compiling? )
        [COMPILE] S"	( read the string, and compile LITSTRING, etc. )
        ' fbwriteline ,	( compile the final TELL )
    ELSE
        ( In immediate mode, just read characters and print them until we get
        to the ending double quote. )
        BEGIN
            KEY
            DUP '"' = IF
                DROP	( drop the double quote character )
                EXIT	( return from this function )
            THEN
            EMIT
        AGAIN
    THEN
;


: fbtest ( -- )
    10
    begin
        15 + dup
        0 120 30 fbline
        dup 105 > if exit then
    again
;

: dumps ( u1 u2 ... -- )
    10 10 fbmove
    10  \ initial x
    begin
        depth 2 = if
            drop
            exit
        then
        swap  \ get next value 
        fbchar \ display
        \ x = x + 8
        8 +
        dup
        10 fbmove
    again
;

: stackclear
    begin
        depth 0 = if
            exit
        then
        drop
    again
;

: dumbdumps
    10
    begin
        depth 1 = if
            drop
            exit
        then
        swap
        drop
        8 +
        dup
        drop
    again
;

fbclear
\ stackclear
\ 10 10 fbmove
\ fbtest
char l
char e
char h
dumbdumps
fbsb
bye