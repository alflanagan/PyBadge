from ForthBBProtocol import ForthBadge as Badge
from PIL import Image

badge = Badge()
badge.set_draw_color('green')
x_offs = 5
y_offs = 5


im = Image.open("tests/smiley.jpg")
pix = im.load()
for x in range(0, im.size[0]):
    for y in range(0, im.size[1]):
        pix_val = pix[x,y]
        print("(%d, %d) = %s" %(x, y, pix_val))
        #badge.set_cursor(x_offs + x, y_offs + y)
        if sum(pix_val) < 10:
            badge.draw_point(x_offs + x, y_offs + y)
            #badge.draw_rect(1, 1)

badge.swap_buffer()
