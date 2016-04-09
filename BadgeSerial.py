import serial
import time
import base64

import logging


class BadgeSerial(object):
    """
    Encapsulates functionality needed to send arbitrary data to the badge.
    Therefore, does not include anything protocol specific - implementing
    a protocol is probably best performed by deriving from this class
    and adding methods that use make_bytes, _write, etc.
    """
    def_dev_to_try = ['/dev/ttyACM%d'%i for i in range(0, 10)]
    # TODO: Support other OS serial port mappings
    def __init__(self, device=None, ser=None, min_write_dt=0.001):
        if device is None:
            try_devs = BadgeSerial.def_dev_to_try
        else:
            try_devs = [device]

        # Try a bunch of device names
        # probably a way to do this more intelligently
        if ser is None:
            for td in try_devs:
                try:
                    ser = BadgeSerial.connect_to_badge(td)
                    dev = td
                except:
                    continue

        self.os_device = dev
        self.os_ser = ser

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
        # TODO: Not sure what appropriate settings are here
        #       since nearly everything I've tried seems to
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
                logging.info("Read count: %d/%d" % (read_cnt, ret_length))
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
            logging.warning("To fast, blocking for %f" % (self.min_write_dt - dt))
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

    def close(self):
        self.os_ser.close()

    def reconnnect(self):
        self.os_ser = BadgeSerial.connect_to_badge(self.os_device)
        return self
