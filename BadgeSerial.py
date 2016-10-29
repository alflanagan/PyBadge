#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import serial
import time
import logging

# Access to colormap
def_cmap = dict(
    d_blue = 0b0000000000000111,
    b_blue = 0b1000010000011111,
    blue   = 0b0000000000011111,
    green  = 0b0000011111100000,
    red    = 0b1111100000000000,
    black  = 0b0000000000000000,
    grey1  = 0b0000100001000001,
    grey2  = 0b0001000010000010,
    grey4  = 0b0010000100000100,
    grey8  = 0b0100001000001000,
    grey16 = 0b1000010000010000,
    white  = 0b1111111111111111,
    cyan   = 0b0000011111111111,
    yellow = 0b1111111111100000,
    magent = 0b1111100000011111,
    grey   = 0b0110001100011000,
)


class BadgeSerialException(Exception):
    """An error occurred in the BadgeSerial class, such as a communications failure."""
    pass


class BadgeSerial(object):
    """
    Encapsulates functionality needed to send arbitrary data to the badge.
    Therefore, does not include anything protocol specific - implementing a
    protocol is probably best performed by deriving from this class and
    adding methods that use make_bytes, _write, etc.

    :param str device: A serial device. If ``None``, tries '/dev/ttyACM0' ...
        '/dev/ttyACM9' to find the first active device.

    :param serial.Serial ser: A serial connection object. If provided does
        not attempt to open `device`.

    :param float min_write_dt: minimum time (in seconds) between writes when
        using `_flushing_write`.

    :param dict cmap: Dictionary from color name to color value.

    """
    def_dev_to_try = ['/dev/ttyACM%d'%i for i in range(0, 10)]

    # TODO: Support other OS serial port mappings
    def __init__(self, device=None, ser=None, min_write_dt=0.001,
                 cmap=None):
        if cmap is None:
            cmap = def_cmap

        if device is None:
            try_devs = BadgeSerial.def_dev_to_try
        else:
            try_devs = [device]

        # Try a bunch of device names
        # probably a way to do this more intelligently
        dev = None
        if ser is None:
            for td in try_devs:
                try:
                    ser = BadgeSerial.connect_to_badge(td)
                    dev = td
                except:
                    continue

        if dev is None:
            raise BadgeSerialException("Unable to find an attached badge!")

        self.os_device = dev
        self.os_ser = ser

        self.cmap = cmap

        self.last_write_time = time.time()
        self.min_write_dt = min_write_dt

        self.return_lines = list()

        # Moar hax - throttled is safest, but should find better way
        if self.min_write_dt is None:
            self._write = self._flushing_write
        else:
            self._write = self._throttled_write

    @staticmethod
    def connect_to_badge(port):
        """Creates and returns a `serial.Serial` connection to the badge."""
        # TODO: Not sure what appropriate settings are here
        #       since nearly everything I've tried seems
        #       to work the same. Lower baudrate may help
        #       the buffering problems though?
        return serial.Serial(
            port=port,
            #baudrate=9600,
            baudrate=115200,
            xonxoff=True,
            parity=serial.PARITY_ODD,
            stopbits=serial.STOPBITS_TWO,
            #bytesize=serial.SEVENBITS
            bytesize=serial.EIGHTBITS
        )

    @staticmethod
    def make_bytes(*args):
        b_str = b''
        for d in args:
            if isinstance(d, str):
                b_str += bytes(d, 'utf-8')
            else:
                b_str += bytes([d])
        return b_str

    def _flushing_write(self, data, save_ret=False,
                        bypass_ret_length=False, **kwargs):
        #while time.time() - self.last_write_time < self.min_
        #self.os_ser.flush()
        #self.os_ser.flushOutput()
        #while self.os_ser.inWaiting() > 0:
        #    print("In waiting: %d"% (self.os_ser.in_waiting))
        #    print("\tREAD: %s" % str(self.os_ser.read_all()))
        #    time.sleep(0.1)

        total_len = len(data)
        words = data.decode().split()

        # EX:  b'1 Ok redled\nOk '
        # 'Ok ' between each word, then the final newline ('\nOk')
        ret_length= total_len + (len(words) - 1)*len('Ok ') + len('\nOk')
        ret_length = 0
        #print("DATA: %s" % data.decode())
        #print("Ret len: %d" % ret_length)

        write_cnt = self.os_ser.write(data)

        if not bypass_ret_length:

            self.os_ser.flush()
            read_cnt = len(self.os_ser.read_all())
            #print("Performing ret length checks")
            while ret_length  > read_cnt:
                read_cnt += len(self.os_ser.read_all())
                #print("Read count: %d/%d" % (read_cnt, ret_length))
                logging.info("Read count: %d/%d", read_cnt, ret_length)
                time.sleep(1)

            cnt = read_cnt
        else:
            self.os_ser.flush()
            cnt = len(self.os_ser.read_all())
            #print("!!!Bypassed ret length checks")

        #if cnt != len(data):
        #    print("Wrote %d of %d bytes" % (cnt, len(data)))

        #while self.os_ser.out_waiting > 0:
        #    time.sleep(0.1)
        #    print("Out waiting: %d"% (self.os_ser.out_waiting))

        #while self.os_ser.inWaiting() > 0:
        #    print("In waiting: %d"% (self.os_ser.in_waiting))
        #    print("\tREAD after: %s" % str(self.os_ser.read_all()))
        #    time.sleep(0.1)

        return cnt


    def _throttled_write(self, data, save_ret=False, **kwargs):
        dt = time.time() - self.last_write_time
        if dt < self.min_write_dt:
            logging.warning("Too fast, blocking for %f", self.min_write_dt - dt)
            time.sleep(self.min_write_dt - dt)

        self.last_write_time = time.time()
        cnt = self.os_ser.write(data)
        self.os_ser.flushOutput()
        if save_ret:
            self.return_lines.append(self.os_ser.read_all())
        else:
            # still empty the buffer?
            self.os_ser.read_all()

        self.os_ser.flushInput()

        return cnt

    def _write_bytes(self, *args, write_kwargs=None):
        if write_kwargs is None:
            write_kwargs = dict()

        byte_data = BadgeSerial.make_bytes(*args)
        write_cnt =  self._write(byte_data, **write_kwargs)

    @staticmethod
    def pack_rgb(red, green, blue):
        """Create an RGB 16-bit value as used by badge.

        :returns int: bit pattern RRRRRGGGGGGBBBBB

        """
        return (red % 32) << 11 | (green % 64) << 5 | (blue % 32)

    def close(self):
        """Close the connection to the badge."""
        self.os_ser.close()
        self.os_ser = None

    def reconnnect(self):
        """Create a new connection to the badge.

        :returns BadgeSerial: this object.

        """
        self.os_ser = BadgeSerial.connect_to_badge(self.os_device)
        return self
