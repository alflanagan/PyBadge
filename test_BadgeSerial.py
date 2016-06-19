#!/usr/bin/env python3
# -*- coding: utf-8; -*-
# coding decl for both python and emacs

import unittest
import timeit
import random

from BadgeSerial import BadgeSerial

class BadgeSerialTestCase(unittest.TestCase):

    TEST_CMAP = ((0b0000000000000111, (0, 0, 7)),   # d_blue
                 (0b1000010000011111, (16, 32, 31)),   # b_blue
                 (0b0000000000011111, (0, 0, 31)),   # blue
                 (0b0000011111100000, (0, 63, 0)),   # green
                 (0b1111100000000000, (31, 0, 0)),   # red
                 (0b0000000000000000, (0, 0, 0)),   # black
                 (0b0000100001000001, (1, 2, 1)),   # grey1
                 (0b0001000010000010, (2, 4, 2)),   # grey2
                 (0b0010000100000100, (4, 8, 4)),   # grey4
                 (0b0100001000001000, (8, 16, 8)),   # grey8
                 (0b1000010000010000, (16, 32, 16)),   # grey16
                 (0b1111111111111111, (31, 63, 31)),   # white
                 (0b0000011111111111, (0, 63, 31)),   # cyan
                 (0b1111111111100000, (31, 63, 0)),   # yellow
                 (0b1111100000011111, (31, 0, 31)),   # magent
                 (0b0110001100011000, (12, 24, 24)),   # grey
)

    def test_pack_rgb(self):
        """Unit tests for `BadgeSerial.pack_rgb`."""
        for value, triplet in self.TEST_CMAP:
            self.assertEqual(value, BadgeSerial.pack_rgb(*triplet))


if __name__ == "__main__":
    unittest.main()


