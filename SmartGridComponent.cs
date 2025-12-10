using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SmartGrid
{
    public class SmartGridComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SmartGridComponent()
          : base("SmartGridComponent", "SG",
            "Creates a smart grid of points with different modes",
            "SmartGrids", "Grids")
        {
        }

        /// Registers all the input parameters for this component.
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Width", "W", "Total width of rectangle in X-direction", GH_ParamAccess.item, 10.0);
            pManager.AddNumberParameter("Height", "H", "Total height of rectangle in Y-direction", GH_ParamAccess.item, 6.0);
            pManager.AddIntegerParameter("Columns", "C", "Number of columns", GH_ParamAccess.item, 5);
            pManager.AddIntegerParameter("Rows", "R", "Number of rows", GH_ParamAccess.item, 3);
            pManager.AddTextParameter("Mode", "M", "Grid mode: 'All' or 'Border'", GH_ParamAccess.item, "All");
        }

        /// Registers all the output parameters for this component.
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Grid points according to selected mode", GH_ParamAccess.list);
            pManager.AddTextParameter("Info", "I", "Information about the grid", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double width = 10.0;
            double height = 6.0;
            int cols = 5;
            int rows = 3;
            string mode = "All";

            if (!DA.GetData(0, ref width)) return;
            if (!DA.GetData(1, ref height)) return;
            if (!DA.GetData(2, ref cols)) return;
            if (!DA.GetData(3, ref rows)) return;
            if (!DA.GetData(4, ref mode)) return;

            // Validate inputs
            if (width <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Width must be greater than 0");
                return;
            }
            if (height <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Height must be greater than 0");
                return;
            }
            if (cols < 1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Columns must be at least 1");
                return;
            }
            if (rows < 1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Rows must be at least 1");
                return;
            }

            mode = mode.Trim();
            if (mode != "All" && mode != "Border")
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Mode must be either 'All' or 'Border'");
                return;
            }

            // Calculate grid points based on mode
            List<Point3d> points = new List<Point3d>();
            string info = "";

            if (mode == "All")
            {
                points = GenerateAllPoints(width, height, cols, rows);
                info = $"{points.Count} points, mode = All";
            }
            else if (mode == "Border")
            {
                points = GenerateBorderPoints(width, height, cols, rows);
                info = $"{points.Count} points, mode = Border";
            }

            DA.SetDataList(0, points);
            DA.SetData(1, info);
        }

        private List<Point3d> GenerateAllPoints(double width, double height, int cols, int rows)
        {
            List<Point3d> points = new List<Point3d>();

            double xStep = width / (cols - 1);
            double yStep = height / (rows - 1);
            double xStart = -width / 2;
            double yStart = -height / 2;

            for (int j = 0; j < cols; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    double x = xStart + i * xStep;
                    double y = yStart + j * yStep;
                    points.Add(new Point3d(x, y, 0));

                }
            }

            return points;

        }

        private List<Point3d> GenerateBorderPoints(double width, double height, int cols, int rows)
        {
            List<Point3d> points = new List<Point3d>();

            double xStep = width / (cols - 1);
            double yStep = height / (rows - 1);
            double xStart = -width / 2;
            double yStart = -height / 2;

            // Add all points from first and last rows
            for (int i = 0; i < cols; i++)
            {
                // First row (j = 0)
                double x1 = xStart + i * xStep;
                double y1 = yStart;
                points.Add(new Point3d(x1, y1, 0));

                // Last row (j = rows - 1)
                double x2 = xStart + i * xStep;
                double y2 = yStart + (rows - 1) * yStep;
                points.Add(new Point3d(x2, y2, 0));
            }

            // Add points from first and last columns (excluding corners which are already added)
            for (int j = 1; j < rows - 1; j++)
            {
                // First column (i = 0)
                double x1 = xStart;
                double y1 = yStart + j * yStep;
                points.Add(new Point3d(x1, y1, 0));

                // Last column (i = cols - 1)
                double x2 = xStart + (cols - 1) * xStep;
                double y2 = yStart + j * yStep;
                points.Add(new Point3d(x2, y2, 0));
            }

            return points;
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("70023b77-7d26-422b-8c15-1a5014faeb9a");
    }
}