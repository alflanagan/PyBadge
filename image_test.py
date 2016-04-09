from ForthBBProtocol import ForthBadge as Badge
#from ForthBBProtocol import pack_rgb
from PIL import Image

def draw_image_to_badge(image_path,
                        bg_color='green',
                        badge=None,
                        verbose=True):
    if badge is None:
        badge = Badge()

    x_offs = 5
    y_offs = 5

    badge.set_background_color(bg_color)
    im = Image.open(image_path)
    pix = im.load()
    for x in range(0, im.size[0]):
        for y in range(0, im.size[1]):
            pix_val = pix[x,y]
            if verbose:
                print("(%d, %d) = %s" %(x, y, pix_val))
            color = Badge.pack_rgb(*pix_val)
            badge.set_draw_color(color)
            badge.draw_point(x_offs + x,
                            y_offs + y)

    badge.swap_buffer()

#p = "tests/smiley.jpg"
p = "tests/bob.jpg"
draw_image_to_badge(p, bg_color='red')
