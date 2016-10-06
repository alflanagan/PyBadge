\ Assignment: Define 'defer ( "name" -- )', which creates a word that
\ stores an XT (at the start the XT of 'abort'), and upon execution
\ 'execute's the XT. Define 'is ( xt "name" -- )' that stores 'xt'
\ into 'name', a word defined with 'defer'.  Indirect recursion is
\ one application of 'defer'.

: defer ( "name" -- )
    :  ['] abort !
  does>
    @ execute ;

: is ( xt "name" -- )
    ' >body !
    ;
