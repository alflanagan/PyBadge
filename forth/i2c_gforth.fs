\ I2C interface for Gforth under Unix -*- mode: forth; coding: us-ascii -*-

\ WARNING! This program can confuse your I2C bus, cause data loss and worse!

\ I started with the following tutorial:
\ https://learn.sparkfun.com/tutorials/raspberry-pi-spi-and-i2c-tutorial
\ http://wiringpi.com/download-and-install/
\ https://fossies.org/dox/i2c-tools-3.1.2/i2cdetect_8c_source.html
\ http://askubuntu.com/questions/625523/libtool-installed-but-not-on-path-after-installation

\ Then took the following from unix/serial.fs

c-library i2c
    \c #include <sys/ioctl.h>
    \c #include <stdio.h>
    \c #include <fcntl.h>

    c-function ioctl ioctl n n a -- n ( fd cmd ptr -- n )
    c-function open open a n n -- n ( path flags mode -- fd )
    c-function read read n a n -- n ( fd addr u -- u' )
    c-function write write n a n -- n ( fd addr u -- u' )
    c-function close close n -- n ( fd -- r )
end-c-library

: open-i2c-dev ( a # -- handle )
    O_RDWR open throw
;

: close-i2c-dev ( handle -- )
    close throw
;

0x0705 constant I2C-FUNCS \ Get the adapter functionality mask


variable buss
variable func

: i2c-detect ( -- )
    s" /dev/i2c-1" open-i2c-dev dup buss !
    dup I2C-FUNCS func ioctl 0<
    if  close-i2c-dev
        1 abort" IOCTL I2C_FUNCS Failed."
    else  close-i2c-dev
    then
;
