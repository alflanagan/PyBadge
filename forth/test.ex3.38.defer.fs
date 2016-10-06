\ -*- forth -*-

\ remove existing words

: defer ( -- )
    100 throw ;

: is ( -- )
    101 throw ;

include ex3.38.defer.fs

: hello ( -- )
    CR s" hello!" type CR ;

defer fred

\ see fred

hello is fred

see fred
fred

