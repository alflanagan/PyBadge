\ Define for gforth words which are used in badge Forth

variable errNo
0 errNo !
create errBuf 80 cells allot
\ todo: initialize errBuf
: errOk .s" Ok" ;
: errSyntac .s" <-wtf?" ;
: errUnderflow .s" Underflow" ;
\ todo: create table errno -> errstring
\ 2 errOk
\ 6 errSyntax
\ 9 errUnderflow

variable errors \ error table

: s0 ( -- addr ) sp0 ; \ initial value of data stack pointer

variable idleword \ ???
variable echo
variable base \ arithmetic base, default 10
10 base !

	\ BUILT-IN CONSTANTS ----------------------------------------------------------------------

	\ VERSION		Is the current version of this FORTH.
	\ R0		The address of the top of the return stack.
	\ DOCOL		Pointer to DOCOL.
	\ F_IMMED		The IMMEDIATE flag's actual value.
	\ F_HIDDEN	The HIDDEN flag's actual value.
	\ F_LENMASK	The length mask in the flags/len byte.

: version .s" rv5tch 20160319" ;
: r0 rp0 ;

\	defconst "docol",5,,__DOCOL,DOCOL,RZ
\	defconst "Fimmed",6,,__F_IMMED,F_IMMED,__DOCOL
\	defconst "Fhidden",7,,__F_HIDDEN,F_HIDDEN,__F_IMMED
\	defconst "Flenmask",8,,__F_LENMASK,F_LENMASK,__F_HIDDEN

: rsp@ ( -- a-addr ) rp@ ;

: rsp! ( w -- ; R: -- w ) rp! ;

\ PARAMETER (DATA) STACK ----------------------------------------------------------------------
\ defcode "dsp@",4,,DSPFETCH,RDROP
\ defcode "dsp!",4,,DSPSTORE,DSPFETCH

: yield ( c-addr -- ) drop ;

: ?key ( -- n ) 0 ; \ not sure how to simulate this (should be equiv. in gforth)

\ Redefine ' so it only works in compiled mode? Nah....

: 0branch ( n -- )
    0= if branch then ;

\ ODDS AND ENDS ----------------------------------------------------------------------

: readCoreTimer utime ;

: redled 0 ;
: greenled 0 ;
: blueled 0 ;


variable fbbgc \ FbBackgroundColor
variable fbcolor \ FbColor

: fbclear ;
: lcdreset fbbgc 0 ! fbcolor 255 ! ;


\ Define a buffer for the frame buffer, and output routines?


\ 	.global FbLine
\ 	defcode "fbline",6,,Ffbline,Ffbcolor
\     POP $a3
\     POP $a2
\     POP $a1
\     POP $a0
\ 	la	$t0, FbLine
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global FbMove
\ 	defcode "fbmove",6,,Ffbmove,Ffbline
\     POP $a1
\     POP $a0
\ 	la	$t0, FbMove
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global FbSwapBuffers
\ 	defcode "fbsb",4,,Ffbsb,Ffbmove
\ 	la	$t0, FbSwapBuffers
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global FbPushBuffer
\ 	defcode "fbpb",4,,Ffbpb,Ffbsb
\ 	la	$t0, FbPushBuffer
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global FbCharacter
\ 	defcode "fbchar",6,,Ffbchar,Ffbpb
\     POP $a0
\ 	la	$t0, FbCharacter
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global FbFilledRectangle
\ 	defcode "fbfrect",7,,Ffbfrect,Ffbchar
\     POP $a1
\     POP $a0
\ 	la	$t0, FbFilledRectangle
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global FbPoint
\ 	defcode "fbpoint",7,,Ffbpoint,Ffbfrect
\     POP $a1
\     POP $a0
\ 	la	$t0, FbPoint
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global FbHorizontalLine
\ 	defcode "fbhline",7,,Ffbhline,Ffbpoint
\     POP $a3
\     POP $a2
\     POP $a1
\     POP $a0
\ 	la	$t0, FbHorizontalLine
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global FbVerticalLine
\ 	defcode "fbvline",7,,Ffbvline,Ffbhline
\     POP $a3
\     POP $a2
\     POP $a1
\     POP $a0
\ 	la	$t0, FbVerticalLine
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global FbRectangle
\ 	defcode "fbrect",6,,Ffbrect,Ffbvline
\     POP $a1
\     POP $a0
\ 	la	$t0, FbRectangle
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global setNote
\ 	defcode "setnote",7,,Fsetnote,Ffbrect
\     POP $a1
\     POP $a0
\ 	la	$t0, setNote
\ 	jalr	$ra, $t0
\ 	NEXT



\ 	.global print_forth_flag1
\ 	defcode "getctfflag",10,,Fprint_forth_flag1,Fsetnote
\ 	la	$t0, print_forth_flag1
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global send_forth_flag
\ 	defcode "sendctfflag",11,,Fsend_forth_flag,Fprint_forth_flag1
\ 	POP $a0
\     la	$t0, send_forth_flag
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global messenger_set_outgoing_msg_char
\ 	defcode "msgset",6,,Fmsg_set_outgoing_char,Fsend_forth_flag
\ 	POP $a1
\ 	POP $a0
\     la	$t0, messenger_set_outgoing_msg_char
\ 	jalr	$ra, $t0
\ 	NEXT

\ 	.global messenger_send_outgoing_msg
\ 	defcode "msg_send_outgoing_msg",21,,Fmsg_send_outgoing_msg,Fmsg_set_outgoing_char
\ 	POP $a0
\     la	$t0, messenger_send_outgoing_msg
\ 	jalr	$ra, $t0
\ 	NEXT


: '\n' lit 10 ;
: not 0= ;


\ 	defword "[compile]",9,F_IMMED,COMPILE,DOTCHAR
\ 	.int	WORD
\ 	.int	FIND
\ 	.int	TCFA
\ 	.int	COMMA
\ 	.int	EXIT



\ 	defword "_u.",3,,_UDOT,HEX
\ 	.int	BASE
\ 	.int	FETCH
\ 	.int	DIVMOD
\ 	.int	QDUP
\ 	.int	ZBRANCH
\ 	.int	8
\ 	.int	_UDOT
\ 	.int	DUP
\ 	.int	LIT
\ 	.int	10
\ 	.int	LT
\ 	.int	ZBRANCH
\ 	.int	16
\ 	.int	ZEROCHAR
\ 	.int	BRANCH
\ 	.int	20
\ 	.int	LIT
\ 	.int	10
\ 	.int	SUB
\ 	.int	ACHAR
\ 	.int	ADD
\ 	.int	EMIT
\ 	.int	EXIT

\ 	defword ".s",2,,DOTS,_UDOT
\ 	.int	DSPFETCH
\ 	.int	DUP
\ 	.int	SZ
\ 	.int	FETCH
\ 	.int	LT
\ 	.int	ZBRANCH
\ 	.int	32
\ 	.int	DUP
\ 	.int	FETCH
\ 	.int	_UDOT
\ 	.int	SPACE
\ 	.int	INCR4
\ 	.int	BRANCH
\ 	.int	-48
\ 	.int	DROP
\ 	.int	EXIT

\ 	defword "uwidth",6,,UWIDTH,DOTS
\ 	.int	BASE
\ 	.int	FETCH
\ 	.int	DIVIDE
\ 	.int	QDUP
\ 	.int	ZBRANCH
\ 	.int	20
\ 	.int	UWIDTH
\ 	.int	INCR
\ 	.int	BRANCH
\ 	.int	12
\ 	.int	LIT
\ 	.int	1
\ 	.int	EXIT

\ 	defword "u.r",3,,UDOTR,UWIDTH
\ 	.int	SWAP
\ 	.int	DUP
\ 	.int	UWIDTH
\ 	.int	ROT
\ 	.int	SWAP
\ 	.int	SUB
\ 	.int	SPACES
\ 	.int	_UDOT
\ 	.int	EXIT

\ 	defword ".r",2,,DOTR,UDOTR
\ 	.int	SWAP
\ 	.int	DUP
\ 	.int	ZLT
\ 	.int	ZBRANCH
\ 	.int	36
\ 	.int	NEGATE
\ 	.int	LIT
\ 	.int	1
\ 	.int	SWAP
\ 	.int	ROT
\ 	.int	DECR
\ 	.int	BRANCH
\ 	.int	20
\ 	.int	LIT
\ 	.int	0
\ 	.int	SWAP
\ 	.int	ROT
\ 	.int	SWAP
\ 	.int	DUP
\ 	.int	UWIDTH
\ 	.int	ROT
\ 	.int	SWAP
\ 	.int	SUB
\ 	.int	SPACES
\ 	.int	SWAP
\ 	.int	ZBRANCH
\ 	.int	12
\ 	.int	DASHCHAR
\ 	.int	EMIT
\ 	.int	_UDOT
\ 	.int	EXIT

\ 	defword ".",1,,DOT,DOTR
\ 	.int	LIT
\ 	.int	0
\ 	.int	DOTR
\ 	.int	SPACE
\ 	.int	EXIT

\ 	defword "u.",2,,UDOT,DOT
\ 	.int	_UDOT
\ 	.int	SPACE
\ 	.int	EXIT


\ 	defword "within",6,,WITHIN,QUESTION
\ 	.int	NROT
\ 	.int	OVER
\ 	.int	LE
\ 	.int	ZBRANCH
\ 	.int	40
\ 	.int	GT
\ 	.int	ZBRANCH
\ 	.int	16
\ 	.int	TRUE
\ 	.int	BRANCH
\ 	.int	8
\ 	.int	FALSE
\ 	.int	BRANCH
\ 	.int	12
\ 	.int	TWODROP
\ 	.int	FALSE
\ 	.int	EXIT

\ 	defword "depth",5,,DEPTH,WITHIN
\ 	.int	SZ
\ 	.int	FETCH
\ 	.int	DSPFETCH
\ 	.int	SUB
\ 	.int	DECR4
\ 	.int	EXIT

\ 	defword "aligned",7,,ALIGNED,DEPTH
\ 	.int	LIT
\ 	.int	3
\ 	.int	ADD
\ 	.int	LIT
\ 	.int	3
\ 	.int	INVERT
\ 	.int	AND
\ 	.int	EXIT

\ 	defword "align",5,,ALIGN,ALIGNED
\ 	.int	HERE
\ 	.int	FETCH
\ 	.int	ALIGNED
\ 	.int	HERE
\ 	.int	STORE
\ 	.int	EXIT

\ 	defword "c,",2,,CCOMMA,ALIGN
\ 	.int	HERE
\ 	.int	FETCH
\ 	.int	STOREBYTE
\ 	.int	LIT
\ 	.int	1
\ 	.int	HERE
\ 	.int	ADDSTORE
\ 	.int	EXIT

\ 	defword "s\"",2,F_IMMED,SQUOTE,CCOMMA
\ 	.int	STATE
\ 	.int	FETCH
\ 	.int	ZBRANCH
\ 	.int	120
\ 	.int	TICK
\ 	.int	LITSTRING
\ 	.int	COMMA
\ 	.int	HERE
\ 	.int	FETCH
\ 	.int	LIT
\ 	.int	0
\ 	.int	COMMA
\ 	.int	KEY
\ 	.int	DUP
\ 	.int	QUOTECHAR
\ 	.int	NEQU
\ 	.int	ZBRANCH
\ 	.int	16
\ 	.int	CCOMMA
\ 	.int	BRANCH
\ 	.int	-32
\ 	.int	DROP
\ 	.int	DUP
\ 	.int	HERE
\ 	.int	FETCH
\ 	.int	SWAP
\ 	.int	SUB
\ 	.int	DECR4
\ 	.int	SWAP
\ 	.int	STORE
\ 	.int	ALIGN
\ 	.int	BRANCH
\ 	.int	84
\ 	.int	HERE
\ 	.int	FETCH
\ 	.int	KEY
\ 	.int	DUP
\ 	.int	QUOTECHAR
\ 	.int	NEQU
\ 	.int	ZBRANCH
\ 	.int	24
\ 	.int	OVER
\ 	.int	STOREBYTE
\ 	.int	INCR
\ 	.int	BRANCH
\ 	.int	-40
\ 	.int	DROP
\ 	.int	HERE
\ 	.int	FETCH
\ 	.int	SUB
\ 	.int	HERE
\ 	.int	FETCH
\ 	.int	SWAP
\ 	.int	EXIT

\ 	defword ".\"",2,F_IMMED,DOTQUOTE,SQUOTE
\ 	.int	STATE
\ 	.int	FETCH
\ 	.int	ZBRANCH
\ 	.int	28
\ 	.int	SQUOTE
\ 	.int	TICK
\ 	.int	TELL
\ 	.int	COMMA
\ 	.int	BRANCH
\ 	.int	48
\ 	.int	KEY
\ 	.int	DUP
\ 	.int	QUOTECHAR
\ 	.int	EQU
\ 	.int	ZBRANCH
\ 	.int	12
\ 	.int	DROP
\ 	.int	EXIT
\ 	.int	EMIT
\ 	.int	BRANCH
\ 	.int	-40
\ 	.int	EXIT


\ 	defword "value",5,,VALUE,VARIABLE
\ 	.int	WORD
\ 	.int	CREATE
\ 	.int	__DOCOL
\ 	.int	COMMA
\ 	.int	TICK
\ 	.int	LIT
\ 	.int	COMMA
\ 	.int	COMMA
\ 	.int	TICK
\ 	.int	EXIT
\ 	.int	COMMA
\ 	.int	EXIT

\ 	defword "to",2,F_IMMED,TO,VALUE
\ 	.int	WORD
\ 	.int	FIND
\ 	.int	TDFA
\ 	.int	INCR4
\ 	.int	STATE
\ 	.int	FETCH
\ 	.int	ZBRANCH
\ 	.int	40
\ 	.int	TICK
\ 	.int	LIT
\ 	.int	COMMA
\ 	.int	COMMA
\ 	.int	TICK
\ 	.int	STORE
\ 	.int	COMMA
\ 	.int	BRANCH
\ 	.int	8
\ 	.int	STORE
\ 	.int	EXIT

\ 	defword "+to",3,F_IMMED,ADDTO,TO
\ 	.int	WORD
\ 	.int	FIND
\ 	.int	TDFA
\ 	.int	INCR4
\ 	.int	STATE
\ 	.int	FETCH
\ 	.int	ZBRANCH
\ 	.int	40
\ 	.int	TICK
\ 	.int	LIT
\ 	.int	COMMA
\ 	.int	COMMA
\ 	.int	TICK
\ 	.int	ADDSTORE
\ 	.int	COMMA
\ 	.int	BRANCH
\ 	.int	8
\ 	.int	ADDSTORE
\ 	.int	EXIT

\ 	defword "id.",3,,IDDOT,ADDTO
\ 	.int	INCR4
\ 	.int	DUP
\ 	.int	FETCHBYTE
\ 	.int	__F_LENMASK
\ 	.int	AND
\ 	.int	DUP
\ 	.int	ZGT
\ 	.int	ZBRANCH
\ 	.int	40
\ 	.int	SWAP
\ 	.int	INCR
\ 	.int	DUP
\ 	.int	FETCHBYTE
\ 	.int	EMIT
\ 	.int	SWAP
\ 	.int	DECR
\ 	.int	BRANCH
\ 	.int	-48
\ 	.int	TWODROP
\ 	.int	EXIT

\ 	defword "?hidden",7,,QHIDDEN,IDDOT
\ 	.int	INCR4
\ 	.int	FETCHBYTE
\ 	.int	__F_HIDDEN
\ 	.int	AND
\ 	.int	EXIT

\ 	defword "?immediate",10,,QIMMEDIATE,QHIDDEN
\ 	.int	INCR4
\ 	.int	FETCHBYTE
\ 	.int	__F_IMMED
\ 	.int	AND
\ 	.int	EXIT

\ 	defword "words",5,,WORDS,QIMMEDIATE
\ 	.int	LATEST
\ 	.int	FETCH
\ 	.int	QDUP
\ 	.int	ZBRANCH
\ 	.int	48
\ 	.int	DUP
\ 	.int	QHIDDEN
\ 	.int	NOT
\ 	.int	ZBRANCH
\ 	.int	16
\ 	.int	DUP
\ 	.int	IDDOT
\ 	.int	SPACE
\ 	.int	FETCH
\ 	.int	BRANCH
\ 	.int	-52
\ 	.int	CR
\ 	.int	EXIT

\ 	defword "forget",6,,FORGET,WORDS
\ 	.int	WORD
\ 	.int	FIND
\ 	.int	QDUP
\ 	.int	ZBRANCH
\ 	.int	28
\ 	.int	DUP
\ 	.int	FETCH
\ 	.int	LATEST
\ 	.int	STORE
\ 	.int	HERE
\ 	.int	STORE
\ 	.int	EXIT

\ 	defword "case",4,F_IMMED,CASE,DUMP
\ 	.int	LIT
\ 	.int	0
\ 	.int	EXIT

\ 	defword "of",2,F_IMMED,OF,CASE
\ 	.int	TICK
\ 	.int	OVER
\ 	.int	COMMA
\ 	.int	TICK
\ 	.int	EQU
\ 	.int	COMMA
\ 	.int	IF
\ 	.int	TICK
\ 	.int	DROP
\ 	.int	COMMA
\ 	.int	EXIT

\ 	defword "endof",5,F_IMMED,ENDOF,OF
\ 	.int	ELSE
\ 	.int	EXIT

\ 	defword "endcase",7,F_IMMED,ENDCASE,ENDOF
\ 	.int	TICK
\ 	.int	DROP
\ 	.int	COMMA
\ 	.int	QDUP
\ 	.int	ZBRANCH
\ 	.int	16
\ 	.int	THEN
\ 	.int	BRANCH
\ 	.int	-20
\ 	.int	EXIT

\ 	defword "cfa>",4,,TOCFA,ENDCASE
\ 	.int	LATEST
\ 	.int	FETCH
\ 	.int	QDUP
\ 	.int	ZBRANCH
\ 	.int	44
\ 	.int	TWODUP
\ 	.int	SWAP
\ 	.int	LT
\ 	.int	ZBRANCH
\ 	.int	12
\ 	.int	NIP
\ 	.int	EXIT
\ 	.int	FETCH
\ 	.int	BRANCH
\ 	.int	-48
\ 	.int	DROP
\ 	.int	LIT
\ 	.int	0
\ 	.int	EXIT

\ 	defword ":noname",7,,NONAME,TOCFA
\ 	.int	LIT
\ 	.int	0
\ 	.int	LIT
\ 	.int	0
\ 	.int	CREATE
\ 	.int	HERE
\ 	.int	FETCH
\ 	.int	LIT
\ 	.int	DOCOL
\ 	.int	COMMA
\ 	.int	RBRAC
\ 	.int	EXIT

\ # : ['] IMMEDIATE ' LIT , ;
\ # compile TICK

\ 	defword "[']",3,F_IMMED,CTICK,NONAME
\ 	.int	TICK
\ 	.int	LIT
\ 	.int	COMMA
\ 	.int	EXIT

\ 	defword "idle",4,,IDLE,CTICK
\ 	.int	STATE
\ 	.int	FETCH
\ 	.int	ZEQU
\ 	.int	ZBRANCH
\ 	.int	88
\ 	.int	DSPFETCH
\ 	.int	SZ
\ 	.int	FETCH
\ 	.int	LE
\ 	.int	ZBRANCH
\ 	.int	28
\ 	.int	LITSTRING
\ 	.int	3
\ 	.ascii	"Ok "
\ 	.int	TELL
\ 	.int	BRANCH 
\ 	.int	40
\ 	.int	LITSTRING
\ 	.int	10
\ 	.ascii	"Underflow "
\ 	.int	TELL
\ 	.int	SZ
\ 	.int	FETCH
\ 	.int	DSPSTORE
\ 	.int	EXIT


\ /* : nidle state @ 0= 0branch 60 dsp@ s0 @ > 0branch 32 2 errNo ! s0 @ dsp! error ; */

\ 	defcode "yeild",5,,YEILDC,IDLE
\ 	move	$t0, $gp
\ 	addiu	$t0, %gp_rel(forth_mode)

\ 	li	$t1, 0		// 0
\ 	sb	$t1, 0($t0)	// store 0 in forth_mode 
\ 	CALL	yeild
\ 	NEXT


\ 	/* SEE will crash on this word since it finds the begin/end */
\ 	/* and this word is between flash and ram words */
\ 	defword "_",1,F_HIDDEN,HANG,YEILDC
\ 	.int	EXIT


\ /*
\ 	DATA SEGMENT ----------------------------------------------------------------------

\ 	Here we set up the Linux data segment, used for user definitions and variously known as just
\ 	the 'data segment', 'user memory' or 'user definitions area'.  It is an area of memory which
\ 	grows upwards and stores both newly-defined FORTH words and global variables of various
\ 	sorts.

\ 	It is completely analogous to the C heap, except there is no generalised 'malloc' and 'free'
\ 	(but as with everything in FORTH, writing such functions would just be a Simple Matter
\ 	Of Programming).  Instead in normal use the data segment just grows upwards as new FORTH
\ 	words are defined/appended to it.

\ 	There are various "features" of the GNU toolchain which make setting up the data segment
\ 	more complicated than it really needs to be.  One is the GNU linker which inserts a random
\ 	"build ID" segment.  Another is Address Space Randomization which means we can't tell
\ 	where the kernel will choose to place the data segment (or the stack for that matter).

\ 	Therefore writing this set_up_data_segment assembler routine is a little more complicated
\ 	than it really needs to be.  We ask the Linux kernel where it thinks the data segment starts
\ 	using the brk(2) system call, then ask it to reserve some initial space (also using brk(2)).

\ 	You don't need to worry about this code.
\ */
\ #	.text
\ #	.set INITIAL_DATA_SEGMENT_SIZE,65536
\ #set_up_data_segment:
\ #	xor %ebx,%ebx		// Call brk(0)
\ #	movl $__NR_brk,%eax
\ #	int $0x80
\ #	movl %eax,var_HERE	// Initialise HERE to point at beginning of data segment.
\ #	addl $INITIAL_DATA_SEGMENT_SIZE,%eax	// Reserve nn bytes of memory for initial data segment.
\ #	movl %eax,%ebx		// Call brk(HERE+INITIAL_DATA_SEGMENT_SIZE)
\ #	movl $__NR_brk,%eax
\ #	int $0x80
\ #	ret

\ /*
\ 	We allocate static buffers for the return static and input buffer (used when
\ 	reading in files and text that the user types in).
\ */

\ 	.bss
\ #	.align	4096
\ 	.align	4

\ /* where compiled words go */
\ data_segment:  
\ 	/*  under EMU can be as big as we want */
\ 	.if	0
\ 	.equ	DATA_SEGMENT_SIZE, 5 * 1024
\ 	.else
\ 	.equ	DATA_SEGMENT_SIZE, 2 * 1024 /* 1k ram */
\ 	.endif
\ 	.space	DATA_SEGMENT_SIZE

\ /* FORTH return stack. grows UP */
\ /*	.bss */

\ return_stack_top:
\ 	.equ	RETURN_STACK_SIZE, 32
\ /*	.equ	RETURN_STACK_SIZE, 64 */
\ 	.space	RETURN_STACK_SIZE

\ /* FORTH data stack. grows DOWN  256/4 = 64 cells */
\ 	.equ	FORTH_STACK_SIZE, 128
\ /*	.equ	FORTH_STACK_SIZE, 256 */
\ 	.space	FORTH_STACK_SIZE
\ forth_stack_top:

\ /* in case of stack underflow */
\ 	.space	16	/* 4 cells */

\ errBuffer:
\ 	.space	80	/* 80 cells */

\ /* If we are running under the EMU */
\ /* we start the c process, so have to */
\ /* set this up */
\ 	.if	0
\ 	.section .stack
\ 	.equ	C_STACK_SIZE, 512
\ 	.space	C_STACK_SIZE
\ c_stack_top:
\ 	.endif

