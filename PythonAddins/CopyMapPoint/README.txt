GISPD.com Sample Code

Disclaimer: These resources are intended as samples and learning tools only.

Authors:
	Jason Pardy, NewfoundGEO Consulting - newfoundgeo.com & gispd.com
	David A. Howes, David Howes, LLC - dhowes.com & gispd.com

Creation date: October 20, 2014

Type: Esri ArcMap 10.2.2 Python add-in

Development environment:
	Esri Python Add-In Wizard - http://www.arcgis.com/home/item.html?id=5f3aefe77f6b4f61ad3e4c62f30bff3b#!
	PyScripter 2.5.3.0 x64 - https://code.google.com/p/pyscripter/

Purpose:
ArcMap add-in tool to allow the user to click on the map and obtain a lat/long string representing the clicked location. The string can be used in Google or Bing Maps, for example.

Installation:
Double click on CopyMapPoint.esriaddin - this will install the add-in "GISPD-Copy Map Point"

Use:
1. Show the toolbar "GISPD-Copy Map Point"
2. Add at least one data layer to ArcMap
3. Open the Python window
4. Click on the tool "GISPD-Copy Map Point"
5. Click on the map - the lat/long string will appear in the Python window and be available in the paste buffer

Further resources:
See 2014 Northwest GIS Conference add-in presentation slides available from gispd.com Resources page for help links
(NW GIS 2014-Howes & Pardy-Extending ArcGIS for Desktop - Python and .NET Add-Ins in a Nutshell-20141015.pdf)

Esri-Generated README text:
This is a stub project created by the ArcGIS Desktop Python AddIn Wizard.


MANIFEST
========

README.txt   : This file

makeaddin.py : Running this script to creates or overwrites a .esriaddin file from this 
               project. The .esriaddin file is required for installing, sharing or deployment.

*.esriaddin  : The Esri AddIn file created from running the makeaddin.py script. Double clicking this file will install the AddIn.
               Optionally, you can copy this file to a well-known folder and use the AddIn manager in ArcMap to point to this location.

config.xml   : The AddIn configuration file containing the CopyMapPoint project properties and AddIn tool properties.
               This AddIn is for ArcGIS 10.2 To work in ArcGIS 10.1, change the ArcGIS version number to 10.1 and re-create the .esriaddin file.

Images/*     : All UI images for the project (icons, images for buttons, 
               etc)

Install/*    : The Python project used for the implementation of the
               AddIn. The specific python script to be used as the root
               module is specified in config.xml.
