import serial
import time
import base64
from BadgeSerial import BadgeSerial

import logging

class ForthBadge(BadgeSerial):
    def __init__(self):
        self.forth_is_ready = False
        super().__init__()

    def forth_run(self, *args, auto_polish=False):
        if not self.forth_is_ready:
            self._write_bytes('!',
                              write_kwargs=dict(bypass_ret_length=True))
            self._write_bytes('0 echo !',
                              write_kwargs=dict(bypass_ret_length=True))

            self.forth_is_ready = True

        tb = " ".join([str(a) for a in args])
        tb = tb + '\n'
        self._write_bytes(*tb)

        return self

    def led(self, red=None, green=None, blue=None):
        # With no args, turn off the LED
        if red is None and green is None and blue is None:
            self.led(0, 0, 0)
            return self

        if red is not None:
            self.forth_run(int(red), 'redled')
        if green is not None:
            self.forth_run(int(green), 'greenled')
        if blue is not None:
            self.forth_run(int(blue), 'blueled')
        return self

    # Contrast of ~50 is pretty nice
    def contrast(self, contrast_value):
        #self._write_bytes('@C', 50)
        return self

    def sound(self, note, duration=None):
        if duration is None:
            duration = 128

        #self._write_bytes('@s', note, duration)
        return self

    def clear(self):
        self.forth_run('fbclear')
        return self

    def set_background_color(self, color='grey'):
        if isinstance(color, str):
            color = self.cmap[color]

        self.forth_run(color, 'fbbgc')
        return self

    def set_draw_color(self, color='red'):
        if isinstance(color, str):
            color = self.cmap[color]

        self.forth_run(color, 'fbcolor')
        return self

    def draw_line(self, x0, y0, x1, y1):
        self.forth_run(y1, x1, y0, x0, 'fbline')
        return self

    def draw_hline(self, x0, y0, x1, y1):
        self.forth_run(y1, x1, y0, x0, 'fbhline')
        return self

    def draw_vline(self, x0, y0, x1, y1):
        self.forth_run(y1, x1, y0, x0, 'fbvline')
        return self

    def draw_point(self, x, y):
        self.forth_run(y, x, 'fbpoint')
        return self

    def draw_rect(self, width, height, filled=False):
        if not filled:
            self.forth_run(height, width, 'fbrect')
        else:
            self.forth_run(height, width, 'fbfrect')
        return self

    def draw_char(self, char):
        self.forth_run(char, "fbchar")
        return self

    def set_cursor(self, x, y):
        self.forth_run(y, x, 'fbmove')
        return self

    def swap_buffer(self):
        self.forth_run('fbsb')
        return self

    def push_buffer(self):
        self.forth_run('fbpb')
        return self

    def get_vertical_slider_pos(self):
        self.forth_run('get_vert_pos')
        return self
