using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace PabloUselessComponents
{
    public class Scribble : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Scribble class.
        /// </summary>
        public Scribble()
          : base("Scribble", "SCR",
              "PLaying with scribbles",
              "UselessComponents", "Components")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            GH_Document docu = this.OnPingDocument();

            Grasshopper.GUI.Canvas.GH_Canvas canvas = Grasshopper.Instances.ActiveCanvas;
            Graphics graphics = canvas.CreateGraphics();
            System.Drawing.Pen pen = new System.Drawing.Pen(Brushes.AliceBlue);
            pen.Width = 50;

            graphics.DrawLine(pen, 0,0,200, 200);




        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("937bd153-604f-426a-9c51-ae7e4b873f5f"); }
        }
    }
}