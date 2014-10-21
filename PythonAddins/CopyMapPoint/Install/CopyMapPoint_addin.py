# The MIT License (MIT)

# Copyright (c) [2014] [GIS Professional Development, gispd.com]

# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:

# The above copyright notice and this permission notice shall be included in all
# copies or substantial portions of the Software.

# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
# SOFTWARE.

import functools
import os
import threading
import webbrowser
import arcpy


def add_to_clip_board(text):
    """Copy text to the clip board."""
    print(text.strip())
    command = 'echo {}| clip'.format(text.strip())
    os.system(command)


def dd_to_dms(dd):
    """Convert decimal degrees to degrees, minutes, seconds."""
    minutes, seconds = divmod(abs(dd)*3600, 60)
    degrees, minutes = divmod(minutes, 60)
    seconds = float('{0:.2f}'.format(seconds))
    return int(degrees), int(minutes), seconds


# A decorator that will run its wrapped function in a new thread
def run_in_other_thread(function):
    # functool.wraps will copy over the docstring and some other metadata from the original function
    @functools.wraps(function)
    def fn_(*args, **kwargs):
        thread = threading.Thread(target=function, args=args, kwargs=kwargs)
        thread.start()
        thread.join()
    return fn_


class CopyMapPoint(object):
    """Implementation for CopyMapPoint_addin.copy_map_point (Tool)"""
    def __init__(self):
        self.enabled = True
        self.cursor = 3

    def onMouseDownMap(self, x, y, button, shift):
        """Copies map x,y to the clip board in degrees, minutes, seconds."""

        # Get the spatial reference from the data frame.
        mxd = arcpy.mapping.MapDocument('current')
        map_sr = mxd.activeDataFrame.spatialReference

        # Get the clicked point and reproject it.
        map_point = arcpy.PointGeometry(arcpy.Point(x, y), map_sr)
        wgs84_sr = arcpy.SpatialReference(4326)
        transformation = arcpy.ListTransformations(map_sr, wgs84_sr)
        if transformation:
            wgs84_pt = map_point.projectAs(wgs84_sr, transformation[0])
        else:
            wgs84_pt = map_point.projectAs(wgs84_sr)

         # Set the hemisphere indicators.
        if wgs84_pt.firstPoint.X > 0:
            east_or_west = 'E'
        else:
            east_or_west = 'W'
        if wgs84_pt.firstPoint.Y < 0:
            south_or_north = 'S'
        else:
            south_or_north = 'N'

        # Get the lat/long values in the required format.
        x_dms = dd_to_dms(wgs84_pt.firstPoint.X)
        y_dms = dd_to_dms(wgs84_pt.firstPoint.Y)
        add_to_clip_board("""{} {} {}{}  {} {} {}{}""".format(x_dms[0], x_dms[1], x_dms[2], east_or_west, y_dms[0], y_dms[1], y_dms[2], south_or_north))

        # Our new wrapped versions of os.startfile and webbrowser.open startfile = run_in_other_thread(os.startfile)
        open_browser = run_in_other_thread(webbrowser.open)
        open_browser("www.maps.google.com")

