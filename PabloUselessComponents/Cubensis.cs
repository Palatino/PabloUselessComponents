using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;
using System.Timers;


namespace PabloUselessComponents
{
    public class Cubensis : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Cubensis class.
        /// </summary>
        
        Timer myTimer;
        
        static Color OriginalbackColor = Color.FromArgb(255, 212, 208, 200);
        double back_hue = 1;
        double hsl_counter = Math.PI;
        int grid_counter = 0;
        double shadow_counter = 0;

        //This variables will store the original values of the document

        Color iniBackColor = Grasshopper.GUI.Canvas.GH_Skin.canvas_back;
        int iniColumn = Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_col;
        int iniRow = Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_row;
        int iniSHadowSize = Grasshopper.GUI.Canvas.GH_Skin.canvas_shade_size;
        Color iniWireColor = Grasshopper.GUI.Canvas.GH_Skin.wire_default;

        public Cubensis()
          : base("Cubensis", "Cubensis",
              "Changes colors",
              "Useless Components", "Components")
        {
            //Set timer
            myTimer = new Timer(250);
            myTimer.AutoReset = true;
            myTimer.Enabled = true;
            myTimer.Elapsed += OnElapsed;

            //Get existing values to set them back when the node is deleted



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

            ChangeColors();
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
                return Properties.Resources.Cubensis;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("539d1b6b-c035-4e33-b02d-76851777dc75"); }
        }


        //Once element is deleted set the brint the original settings back
        public override void RemovedFromDocument(GH_Document document)
        {
            base.RemovedFromDocument(document);
            Grasshopper.GUI.Canvas.GH_Skin.canvas_back = iniBackColor;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_col =  iniColumn;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_row = iniRow;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_shade_size =  iniSHadowSize;
            Grasshopper.GUI.Canvas.GH_Skin.wire_default =  iniWireColor;

        }

        
        public void OnElapsed(Object sender, EventArgs e)
        {
            ExpireSolution(true);
        }


        [STAThread]
        public void ChangeColors()
        {



            Color back_color = GenerateNextColor(iniBackColor,back_hue, hsl_counter);
            Color wire_color = back_color;

            grid_counter = grid_counter % 10;

            Grasshopper.GUI.Canvas.GH_Skin.canvas_back = back_color;

            Grasshopper.GUI.Canvas.GH_Skin.canvas_shade_size = 200;
            Grasshopper.GUI.Canvas.GH_Skin.wire_default = wire_color;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_shade_size += (int)(Math.Sin(shadow_counter) * 50);

            back_hue += 0.1;
            back_hue = back_hue % 360;
            hsl_counter += 0.05;
            shadow_counter += 0.1;

        }

        public Color GenerateNextColor(Color color, double hue_counter, double hs_counter)
        {
            double h;
            double s;
            double v;
            Helpers.RGBToHSV(color.R, color.G, color.B,out h,out s, out v);

            h = (h * hue_counter)%360;
            s += Math.Sin(hs_counter) *s* 0.4;
            if (s > 1.0) s = 1.0;
            if (s < 0) s = 0.0;
            v += Math.Sin(hs_counter) * v* 0.1;
            if (v > 1) v = 1.0;
            if (v < 0) v = 0.0;


            return Helpers.RGBFromHSV(h, s, v);

        }
    }
}