"""CopyMapPoint ArcMap Tool"""
import os
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


class CopyMapPoint(object):
    """Implementation for CopyMapPoint_addin.copy_map_point (Tool)"""
    def __init__(self):
        self.enabled = True
        self.cursor = 3

    def onMouseDownMap(self, x, y, button, shift):
        """Copies the map's x,y location to the clip board
        in degrees, minutes, seconds. These coordinates can be copied into
        maps.google.com.
        """
        mxd = arcpy.mapping.MapDocument('current')
        map_sr = mxd.activeDataFrame.spatialReference
        map_point = arcpy.PointGeometry(arcpy.Point(x, y), map_sr)
        wgs84_sr = arcpy.SpatialReference(4326)
        transformation = arcpy.ListTransformations(map_sr, wgs84_sr)
        if transformation:
            wgs84_pt = map_point.projectAs(wgs84_sr, transformation[0])
        else:
            wgs84_pt = map_point.projectAs(wgs84_sr)

        if wgs84_pt.firstPoint.X > 0:
            east_or_west = 'E'
        else:
            east_or_west = 'W'
        if wgs84_pt.firstPoint.Y < 0:
            south_or_north = 'S'
        else:
            south_or_north = 'N'
        x_dms = dd_to_dms(wgs84_pt.firstPoint.X)
        y_dms = dd_to_dms(wgs84_pt.firstPoint.Y)
        add_to_clip_board("""{} {} {}{}  {} {} {}{}""".format(x_dms[0], x_dms[1], x_dms[2], east_or_west, y_dms[0], y_dms[1], y_dms[2], south_or_north))
