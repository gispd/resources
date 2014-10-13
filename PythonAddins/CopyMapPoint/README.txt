This is the CopyMapPoint project created by the ArcGIS Desktop Python AddIn Wizard. The CopyMapPoint_addin.py file includes the source code for this
AddIn.

This sample AddIn is an ArcMap tool that copies the map x, y coordinates as degrees, minutes, seconds to the clipboard.
The coordinates can be copied into maps.google.com. 

For more information about creating AddIns with Python, see the ArcGIS Guide book for building AddIns with Python:
http://resources.arcgis.com/en/help/main/10.2/#/What_is_a_Python_add_in/014p00000025000000/


MANIFEST
========

README.txt   : This file

makeaddin.py : Running this script to creates or overwrites a .esriaddin file from this 
               project. The .esriaddin file is required for installing, sharing or deployment.

*.esriaddin  : The Esri AddIn file created from running the makeaddin.py script. Double clicking this file will install the AddIn.
               Optionally, you can copy this file to a well-known folder and use the AddIn manager in ArcMap to point to this location.

config.xml   : The AddIn configuration file containing the CopyMapPoint project properties and AddIn tool properties.

Images/*     : All UI images for the project (icons, images for buttons, 
               etc)

Install/*    : The Python project used for the implementation of the
               AddIn. The specific python script to be used as the root
               module is specified in config.xml.
