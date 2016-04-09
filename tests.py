import time
import math
import ForthBBProtocol as bb

def led_test(badge, count=100):
    strengths = list(range(0, 255, 5))
    colors = ['redled', 'greenled', 'blueled']

    for i in range(0, count):
        for c in colors:
            for b in strengths:
                badge.forth_run(b, c)
        strengths.reverse()

def led_test2(badge, count=100):
    strengths = list(range(0, 255, 5))
    colors = ['red', 'green', 'blue']

    for i in range(0, count):
        for c in colors:
            for b in strengths:
                badge.led(**{c:b})
        strengths.reverse()

def draw_sin_test(badge, num_points=50, step_size=0.1):
    x_vals = [i for i in range(0, num_points)]
    y_vals = [math.ceil(10 + 10*math.sin(x*step_size)) for x in x_vals]

    for x, y in zip(x_vals, y_vals):
        #print("(%f, %f)" % (x, y))
        badge.set_cursor(x, y)
        badge.draw_rect(3, 3, filled=True)
        time.sleep(0.01)

    badge.swap_buffer()



badge = bb.ForthBadge()

badge.set_draw_color('green')

led_test2(badge, count=2)

draw_sin_test(badge)
