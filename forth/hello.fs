\ just a basic hello, badge! program

: fb." IMMEDIATE		( X Y -- )
	STATE @ IF	( compiling? )
	\	[COMPILE] S"	( read the string, and compile LITSTRING, etc. )
            \	' TELL ,	( compile the final TELL )
            char e fbchar
	ELSE
		( In immediate mode, just read characters and print them until we get
		  to the ending double quote. )
		BEGIN
			KEY
			DUP '"' = IF
				DROP	( drop the double quote character )
				EXIT	( return from this function )
			THEN
                        fbchar
		AGAIN
	THEN
;

: fb.s
    dsp@
    dup
    s0 \ top of param stack
    @ < if
        dup
        @
        fbchar
        char ' ' fbchar
        4+
    then
    drop
;

: fb." immediate
    begin
        key
        dup '"' <> IF
            fbchar
        then
            exit
        else
    again
;
\         dup '"' = IF
\             drop
\             exit
\         then
\         \ again
\         key
\         fbchar
\     \ loop
\ ;

: fbtest ( -- )
    10
    begin
        5 + dup
        0 120 30 fbline
        dup 35 = if exit then
    again
;
    
        
\ fbclear 5 10 fbmove \ fb." hello"
\ sayhello
\  0 0 120 30 fb.s
fbtest
\ 0 0 120 30 fbline
fbsb
bye