from ForthBBProtocol import ForthBadge as Badge
from datetime import timedelta, datetime
import socket
import pandas as pd
import psutil
import time

import logging
logger = logging.getLogger()
logger.setLevel(level=logging.ERROR)

class BadgeSysMon:
    def __init__(self, badge, max_history=65):
        self.done = False
        self.badge = badge
        self.max_history = max_history
        self.cpu_data = {i:[0]*max_history for i in range(0, psutil.cpu_count())}
        self.cpu_pct = [0]*max_history
        self.mem_pct = [0]*max_history
        self.net_conns = [0] * max_history
        self.hostname = socket.gethostname()


    @staticmethod
    def get_uptime():

        with open('/proc/uptime', 'r') as f:
            uptime_seconds = float(f.readline().split()[0])
            uptime_string = str(timedelta(seconds = uptime_seconds))

        return uptime_string

    def poll_system(self):
        cputimes = psutil.cpu_times_percent(percpu=True)
        for i, ct in enumerate(cputimes):
            self.cpu_data[i].append(ct)

            if len(self.cpu_data[i]) > self.max_history:
                self.cpu_data[i] = self.cpu_data[i][-self.max_history:]

        self.cpu_pct.append(int(psutil.cpu_percent()))
        if len(self.cpu_pct) > self.max_history:
            self.cpu_pct = self.cpu_pct[-self.max_history:]

        vm = psutil.virtual_memory()
        self.mem_pct.append(vm.used/vm.total)
        if len(self.mem_pct) > self.max_history:
            self.mem_pct = self.mem_pct[-self.max_history:]

        self.net_conns.append(len(psutil.net_connections()))
        if len(self.net_conns) > self.max_history:
            self.net_conns = self.net_conns[-self.max_history:]


    def draw_plot_line(self, points, color='cyan',
                    x_origin=3, y_origin=125,
                    step_size=2):
        self.badge.set_draw_color(color)
        for i in range(1, len(points)):
            x_start = x_origin+ i*step_size
            if x_start > 120:
                break

            self.badge.draw_line(x_start, y_origin - points[i-1],
                                step_size + x_start, y_origin - points[i])

        #self.badge.push_buffer()

    # TODO: replace magic numbers for x,y in draws with
    # placement logic (e.g. keep hostname centered at top)
    def render(self, plot_avg_cpu=True, data_scaling=.5):
        self.badge.set_cursor(10, 10).set_draw_color('green')
        self.badge.writeline(20, 10, self.hostname,
                             char_w=12)
                             #char_w=120.0/(len(self.hostname)*8))

        dt = datetime.now().strftime('%b-%d-%Y')
        self.badge.set_draw_color('white').writeline(3, 23, dt, char_w=7)


        self.badge.set_draw_color('grey')
        self.badge.writeline(88, 23, self.get_uptime().split(',')[0],
                             char_w=6)


        self.badge.set_draw_color('grey')
        self.badge.set_cursor(3, 65).draw_rect(118, 60, False)

        cpu_dfs = {i:pd.DataFrame(cd)*data_scaling
                   for i, cd in self.cpu_data.items()}
        tmp_cpu_pct = [int(c*data_scaling) for c in self.cpu_pct]
        tmp_mem_pct = [int(m*100.0*data_scaling) for m in self.mem_pct]
        tmp_net_conns = [int(n*data_scaling) for n in self.net_conns]

        if plot_avg_cpu or psutil.cpu_count() > 4:
            self.draw_plot_line(tmp_cpu_pct, 'cyan')
            self.badge.set_draw_color('cyan').writeline(5, 67, 'CPU', char_w=6)
        else:
            colors = ['yellow', 'cyan', 'green', 'red']
            for i in range(0, psutil.cpu_count()):
                self.draw_plot_line((cpu_dfs[i].user).astype(int),
                                    colors[i],
                                    x_origin=1, y_origin=125)

        self.draw_plot_line(tmp_mem_pct, 'green')
        self.badge.set_draw_color('green').writeline(40, 67, 'MEM', char_w=6)


        self.draw_plot_line(tmp_net_conns, 'yellow')
        self.badge.set_draw_color('yellow').writeline(80, 67, 'CON', char_w=6)

        self.badge.swap_buffer()

    def run(self, poll_delta=.25):
        churn = [self.poll_system() for i in range(0, self.max_history)]

        while not self.done:
            self.poll_system()

            self.render()

            time.sleep(poll_delta)

if __name__ == "__main__":
    badge = Badge()
    badge.clear()
    sysmon = BadgeSysMon(badge)
    sysmon.run()


