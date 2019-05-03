using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PabloUselessComponents
{
    public class Reset_Canvas : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Reset_Canvas class.
        /// </summary>
        public Reset_Canvas()
          : base("Reset Canvas", "Reset",
              "Set default canvas values",
              "Useless Components", "Components")
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
            Grasshopper.GUI.Canvas.GH_Skin.canvas_back = System.Drawing.Color.FromArgb(255, 212, 208, 200);
            Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_col = 150;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_row = 50;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_shade_size = 50;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_shade = System.Drawing.Color.FromArgb(80, 0, 0, 0);
            Grasshopper.GUI.Canvas.GH_Skin.wire_default = System.Drawing.Color.FromArgb(150, 0, 0, 0);
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
            get { return new Guid("2dae6ba8-2a70-4f93-af21-5854404688f4"); }
        }
    }
}