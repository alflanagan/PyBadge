#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""A simple graphical app to allow user to send a Forth file to the badge."""
from pathlib import Path
import time

from tkinter import constants
from tkinter.ttk import Frame, Button, Style, Label
from tkinter import tix

from BadgeSerial import BadgeSerialException
from ForthBBProtocol import ForthBadge as Badge


# pylint: disable=R0901
class Application(Frame):
    """The main Tk application, a simple dialog."""
    def __init__(self, master=None):
        super().__init__(master)
        self.grid()
        self.columnconfigure(0, minsize=200)
        self.columnconfigure(1, minsize=200)
        self.rowconfigure(0, minsize=300)
        self.rowconfigure(3, minsize=30)
        try:
            self.badge = Badge()
        except BadgeSerialException:
            self.badge = None  # connect later?
        self.create_widgets()

    def create_widgets(self):
        """Sets up dialog elements."""
        self.select = tix.FileSelectBox(self, browsecmd=self.on_file_selected,
                                        pattern="*.fs", directory="forth")
        # self.select["textVariable"] = self.forth_file
        self.select.grid(row=0, columnspan=2, sticky='n'+'w'+'e')
        self.connect_btn = Button(self, text="Connect", command=self.connect)
        self.connect_btn.grid(row=1, column=0, columnspan=2)
        self.exec_btn = Button(self, text="Execute", command=self.send_file)
        self.exec_btn.state(["disabled"])
        self.exec_btn.grid(row=2, column=0, sticky='w' + 'e', padx=5, pady=3)
        self.quit = Button(self, text="QUIT", command=self.master.destroy)
        self.quit.grid(row=2, column=1, sticky='w' + 'e', padx=5, pady=3)
        self.status_panel = Frame(self, relief="groove", borderwidth=3)
        self.status_panel.grid(row=3, columnspan=2, sticky='nwse')
        self.connect_status = Label(self.status_panel, text="Not Connected")
        self.connect_status.grid(row=0, padx=5, pady=5, sticky="w")
        if self.badge is not None:
            self.connect_btn.state(["disabled"])
            self.connect_status.config(text="Connected: " + self.badge.os_device)

    def send_file(self):
        """Send the selected file to the badge."""
        # oddly, very first set LED seems to not set correct color
        self.badge.led(0, 0, 128)
        self.badge.led(0, 0, 128)
        with open(self.select.cget("value"), 'r') as forthin:
            self.badge.forth_run(forthin.read())
        time.sleep(1)  # because forth_run() may be too fast
        self.badge.led(0, 128, 0)

    def connect(self):
        """Attempt to connect to a badge; disable Connect button if successful."""
        # TODO: replace 'Connect' with 'Disconnect'
        try:
            self.badge = Badge()
            self.connect_btn.state(["disabled"])
            self.connect_status.config(text="Connected: " + self.badge.os_device)
        except BadgeSerialException:
            self.connect_btn.state(["!disabled"])
            self.connect_status.config(text="Not connected")

    def on_file_selected(self, selected_file):
        """Respond to user selection of file by enabling the Execute button."""
        if Path(selected_file).is_file:
            self.exec_btn.state(["!disabled"])
        else:
            self.exec_btn.state(["disabled"])


def main():
    """Runs the Tk event loop."""
    root = tix.Tk()
    root.title("Send Forth File")
    # root.geometry("500x400")
    app = Application(master=root)
    app.mainloop()


if __name__ == '__main__':
    main()
