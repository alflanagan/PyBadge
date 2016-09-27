: test_ex36last ( a b -- )
    2dup swap 2dup
    ." (" . ." - " . ." )(" . ." + " . ." ) => "
    ex36last . CR
;

: test_ex36last2 ( a b -- )
    2dup swap 2dup
    ." (" . ." - " . ." )(" . ." + " . ." ) => "
    ex36last2 . CR
;

4 5 test_ex36last
2 6 test_ex36last
6 2 test_ex36last

4 5 test_ex36last2
2 6 test_ex36last2
6 2 test_ex36last2
