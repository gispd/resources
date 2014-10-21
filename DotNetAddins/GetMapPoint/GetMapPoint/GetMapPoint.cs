/*
The MIT License (MIT)

Copyright (c) [2014] [GIS Professional Development, gispd.com]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace GetMapPoint
{
    public class GetMapPoint : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        public GetMapPoint()
        {
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            this.Cursor = System.Windows.Forms.Cursors.Cross;
        }

        protected override void OnMouseDown(ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs arg)
        {
            //Create a screen point object for use in the GetMapCoordinatesFromScreenCoordinates call
            IPoint screenPoint = new PointClass();
            screenPoint.X = arg.X;
            screenPoint.Y = arg.Y;

            //Create an ActiveView object for use in the GetMapCoordinatesFromScreenCoordinates call
            IMxDocument mxDocument = ArcMap.Document;
            IMapDocument mapDocument = (IMapDocument)mxDocument;
            IActiveView activeView = mapDocument.ActiveView;

            //Call the GetMapCoordinatesFromScreenCoordinates method and put the result into a point object
            IPoint mapPoint = GetMapCoordinatesFromScreenCoordinates(screenPoint, activeView);

            //Get a geographic spatial reference object
            string spatialReferenceName = "esriSRGeoCS_WGS1984";
            ISpatialReference spatialReference = GetSpatialReferenceByName_Geographic(spatialReferenceName);

            //Reproject the point
            mapPoint.Project(spatialReference);

            //Determine the hemispheres and adjust the coordinate values
            string latHemisphere = "N";
            if (mapPoint.Y < 0)
            {
                latHemisphere = "S";
            }
            double latValue = Math.Abs(mapPoint.Y);

            string longHemisphere = "E";
            if (mapPoint.X < 0)
            {
                longHemisphere = "W";
            }
            double longValue = Math.Abs(mapPoint.X);

            string latValueString = GetDMSString(latValue);
            string longValueString = GetDMSString(longValue);

            string latLongString = latValueString + " " + latHemisphere + ", " +
                longValueString + " " + longHemisphere;

            //If the string is not empty, put it into the clipboard
            if (latLongString != null)
            {
                System.Windows.Forms.Clipboard.SetText(latLongString);
            }

            //Show latLongString in message box
            string message = latLongString +
                Environment.NewLine +
                Environment.NewLine +
                "String shown in message box for demo purposes";
            System.Windows.Forms.MessageBox.Show(message);
        }

        public IPoint GetMapCoordinatesFromScreenCoordinates
                (IPoint screenPoint, IActiveView
                activeView)
        {
            //http://help.arcgis.com/en/sdk/10.0/arcobjects_net/conceptualhelp/index.html#//00010000049z000000

            if (screenPoint == null || screenPoint.IsEmpty || activeView == null)
            {
                return null;
            }

            ESRI.ArcGIS.Display.IScreenDisplay screenDisplay = activeView.ScreenDisplay;
            ESRI.ArcGIS.Display.IDisplayTransformation displayTransformation =
                    screenDisplay.DisplayTransformation;

            return displayTransformation.ToMapPoint((System.Int32)screenPoint.X,
                    (System.Int32)screenPoint.Y); // Explicit cast.
        }

        public string GetDMSString(double decDeg)
        {
            //Return a DMS string for a decimal degrees value

            string dmsString = "";

            double decDegInt = Math.Truncate(decDeg);
            double decDegFrac = decDeg - decDegInt;
            double decMin = decDegFrac * 60;
            double decMinInt = Math.Truncate(decMin);
            double decMinFrac = decMin - decMinInt;
            double decSec = decMinFrac * 60;

            dmsString = decDegInt.ToString() + " " +
                decMinInt.ToString() + " " +
                decSec.ToString("0.##");

            return dmsString;
        }

        public ISpatialReference GetSpatialReferenceByName_Geographic(string requiredSpatialReferenceName)
        {
            //Return a spatial reference code for a named geographic coordinate system

            ISpatialReference spatialReference = null;

            // Set up the SpatialReferenceEnvironment.
            // SpatialReferenceEnvironment is a singleton object and needs to use the Activator class

            Type t = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
            System.Object obj = Activator.CreateInstance(t);
            ISpatialReferenceFactory srFact = obj as ISpatialReferenceFactory;

            // Use the enumeration to create an instance of the predefined object
            string[] names = Enum.GetNames(typeof(esriSRGeoCSType));
            esriSRGeoCSType[] values = (esriSRGeoCSType[])Enum.GetValues(typeof(esriSRGeoCSType));

            int gcsType = 0;

            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];

                if (name == requiredSpatialReferenceName)
                {
                    esriSRGeoCSType value = values[i];
                    gcsType = (int)value;
                    break;
                }
            }

            IGeographicCoordinateSystem geographicCS = srFact.CreateGeographicCoordinateSystem(gcsType);
            spatialReference = (ISpatialReference)geographicCS;

            return spatialReference;
        }
    }
}
