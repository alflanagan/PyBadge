#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""A simple graphical app to allow user to send a Forth file to the badge."""
from pathlib import Path
import time

from tkinter.ttk import Frame, Button, Label
from tkinter import tix, Text
from tkinter import scrolledtext

from BadgeSerial import BadgeSerialException
from ForthBBProtocol import ForthBadge as Badge

SERIAL_POLL_INTERVAL = 5  # milliseconds

# pylint: disable=R0901
class Application(Frame):
    """The main Tk application, a simple dialog."""
    def __init__(self, master=None):
        super().__init__(master)
        self.badge = None
        self.grid()
        self.columnconfigure(0, minsize=200)
        self.columnconfigure(1, minsize=200)
        self.rowconfigure(0, minsize=300)
        self.rowconfigure(3, minsize=30)
        self.create_widgets()
        # self.output.insert("1.0", "This is a test\n")
        # self.sentto.insert("end", b"this is an error\n", "error")
        self.connect()
        self.after(SERIAL_POLL_INTERVAL, self.poll_serial)

    def create_widgets(self):
        """Sets up dialog elements."""
        da_row = 0
        "counter so I don't have to update every grid() call when I add/remove row"
        self.select = tix.FileSelectBox(self, browsecmd=self.on_file_selected,
                                        pattern="*.fs", directory="forth")
        # self.select["textVariable"] = self.forth_file
        self.select.grid(row=da_row, columnspan=2, sticky='nwes', pady=10)
        da_row += 1
        self.output_label = Label(self, text="Badge Output")
        self.output_label.grid(row=da_row, column=1, sticky='w', padx=10, pady=3)
        self.sentto_label = Label(self, text="Sent To Badge")
        self.sentto_label.grid(row=da_row, column=0, sticky='w', padx=10, pady=3)
        da_row += 1
        # height is in lines, width in characters
        self.output = Text(self, height=16, width=40)
        self.output.grid(row=da_row, column=1, padx=10)
        self.sentto = Text(self, height=16, width=40)
        self.sentto.tag_configure("error", background="red")
        self.sentto.grid(row=da_row, column=0, padx=10)
        da_row += 1
        self.connect_btn = Button(self, text="Connect",
                                  command=self.toggle_connect)
        self.connect_btn.grid(row=da_row, column=0, columnspan=2)
        da_row += 1
        self.exec_btn = Button(self, text="Execute", command=self.send_file)
        self.exec_btn.state(["disabled"])
        self.exec_btn.grid(row=da_row, column=0, sticky='w' + 'e', padx=10, pady=3)
        self.quit = Button(self, text="QUIT", command=self.master.destroy)
        self.quit.grid(row=da_row, column=1, sticky='w' + 'e', padx=10, pady=3)
        da_row += 1
        self.status_panel = Frame(self, relief="sunken", borderwidth=3)
        self.status_panel.grid(row=da_row, columnspan=2, sticky='nwse')
        self.connect_status = Label(self.status_panel, text="Not Connected")
        self.connect_status.grid(row=0, padx=10, pady=5, sticky="w")
        if self.badge is not None:
            self.connect_btn.state(["disabled"])
            self.connect_status.config(text="Connected: " + self.badge.os_device)

    def send_file(self, _retry=False):
        """Send the selected file to the badge."""
        if self.badge:
            try:
                # oddly, very first set LED seems to not set correct color
                self.badge.led(0, 0, 128)
                self.badge.led(0, 0, 128)
                with open(self.select.cget("value"), 'r') as forthin:
                    self.badge.forth_run(forthin.read())
                time.sleep(1)  # because forth_run() may be too fast
                self.badge.led(0, 128, 0)
            except IOError:
                if not _retry:
                    self.connect()
                    self.send_file(True)
                else:
                    raise

    def poll_serial(self):
        "Checks serial port for incoming bytes, reads and displays them."
        if self.badge is not None:
            bytes_in = self.badge.read_from()
            if bytes_in:
                self.output.insert("end", bytes_in + b'\n')
        self.after(SERIAL_POLL_INTERVAL, self.poll_serial)

    def toggle_connect(self):
        "If connected, disconnect, otherwise connect."
        if self.connect_btn.cget("text") == "Connect":
            self.connect()
        else:
            self.disconnect()

    def disconnect(self):
        "Disconnect from current badge."
        isinstance(self.badge, Badge)
        self.badge.close()
        self.badge = None
        self.connect_btn.config(text="Connect")
        self.connect_status.config(text="Not connected.")
        self.exec_btn.state(["disabled"])

    def connect(self):
        """Attempt to connect to a badge; toggle Connect button if successful."""
        try:
            self.badge = Badge(on_serial_write=self.on_bytes_sent, min_write_dt=0.01)
            self.connect_status.config(text="Connected: " + self.badge.os_device)
            self.connect_btn.config(text="Disconnect")
            # enable "Execute" if file is selected
            self.on_file_selected(self.select.cget("value"))
        except BadgeSerialException:
            self.connect_status.config(text="Not connected")

    def on_file_selected(self, selected_file):
        """Respond to user selection of file by enabling the Execute button."""
        if Path(selected_file).is_file:
            self.exec_btn.state(["!disabled"])
        else:
            self.exec_btn.state(["disabled"])

    def on_bytes_sent(self, data, count):
        self.sentto.insert("end", data[:count])
        if count < len(data):
            self.sentto.insert("end", data[count:], "error")
        self.sentto.insert("end", "\n")


def main():
    """Runs the Tk event loop."""
    root = tix.Tk()
    root.title("Send Forth File")
    # root.geometry("500x400")
    app = Application(master=root)
    app.mainloop()


if __name__ == '__main__':
    main()
