\ : fb." IMMEDIATE
\     STATE @ IF	( compiling? )
\         [COMPILE] S"	( read the string, and compile LITSTRING, etc. )
\         ' fbwriteline ,	( compile the final TELL )
\     ELSE
\         ( In immediate mode, just read characters and print them until we get
\         to the ending double quote. )
\         BEGIN
\             KEY
\             DUP '"' = IF
\                 DROP	( drop the double quote character )
\                 EXIT	( return from this function )
\             THEN
\             EMIT
\         AGAIN
\     THEN
\ ;


\ draws several lines fanning out from a single point
: fbtest ( -- )
    10
    begin
        15 + dup
        0 120 30 fbline
        dup 105 > if exit then
    again
;

\ write string to display. not working ?
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

\ clear data stack. Standard word clears fp stack but we don't have one
: clearstacks ( ... -- )
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

\ display "fan" then return to regularly-scheduled programs
fbclear
\ stackclear
\ 10 10 fbmove
fbtest
\ char l
\ char e
\ char h
\ dumbdumps
fbsb
\ wait for button press?
yield
