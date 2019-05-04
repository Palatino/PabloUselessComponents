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

        Random random = new Random();

        //List of active images;
        private List<MovingImage> activeImages = new List<MovingImage>();

        //List of component boundaries;
        public static List<RectangleF> boundaries = new List<RectangleF>();

        //Store coordinates of extreme coordinates
        static public float max_x;
        static public float min_x;
        static public float max_y;
        static public float min_y;

        //All this variables are used to change the colors in combination with sin functions
        static Color OriginalbackColor = Color.FromArgb(255, 212, 208, 200);
        double back_hue = 1;
        double hsl_counter = Math.PI;
        double factor = 0;
        double wirecolor_1 = 0;
        double wirecolor_2 = -0.5 * Math.PI;
        double wirecolor_3 = Math.PI;
        double shadow_color_counter = 0;
        double shadow_color_counter2 = 0;
        double triger = 0;
        float movement_counter = 0;
        double counter3 = 0;
        int even = 0;



        //This variables will store the original values of the document

        Color iniBackColor = Grasshopper.GUI.Canvas.GH_Skin.canvas_back;
        int iniColumn = Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_col;
        int iniRow = Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_row;
        int iniSHadowSize = Grasshopper.GUI.Canvas.GH_Skin.canvas_shade_size;
        Color iniWireColor = Grasshopper.GUI.Canvas.GH_Skin.wire_default;
        Color iniShadowColor = Grasshopper.GUI.Canvas.GH_Skin.canvas_shade;

        public Cubensis()
          : base("Cubensis", "Cubensis",
              "Changes colors",
              "Useless Components", "Components")
        {
            //Set timer
            myTimer = new Timer(100);
            myTimer.AutoReset = true;
            myTimer.Enabled = true;
            myTimer.Elapsed += OnElapsed;

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

            even += 1;
            even= even % 2;
            if (triger < 100)
            {
                triger += 0.25;
            }

            // The different effects are triggered base on the triger count

            if (even%2 == 0)
            {
                ChangeColors();
                if (triger > 10) ChangeShadow();
                if (triger > 30) ChangeWireColor();
                if (triger > 70)
                {
                    MoveComponents(movement_counter);
                    if (movement_counter < 3) movement_counter += (float)0.005;
                }
            }

            if(triger>90)
            {
                // Retrieve all the boundaries of the elements to calculate collisions
                IList<IGH_DocumentObject> objects = docu.Objects;

                //Empty boundary list
                boundaries = new List<RectangleF>();
                
                foreach (IGH_DocumentObject obj in objects)
                {
                    RectangleF rec = obj.Attributes.Bounds;
                    if (rec.Top > max_y) max_y = rec.Top;
                    if (rec.Top < min_y) min_y = rec.Top;
                    if (rec.Left > max_x) max_x = rec.Left;
                    if (rec.Left < min_x) min_x = rec.Left;
                    boundaries.Add(rec);
                }
                Grasshopper.Instances.ActiveCanvas.CanvasPaintBackground -= canvasPaintHandler;
                Grasshopper.Instances.ActiveCanvas.CanvasPaintBackground += canvasPaintHandler;
            }

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


        //Once element is deleted set the original settings back and remove images
        public override void RemovedFromDocument(GH_Document document)
        {
            base.RemovedFromDocument(document);
            Grasshopper.GUI.Canvas.GH_Skin.canvas_back = iniBackColor;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_col =  iniColumn;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_row = iniRow;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_shade_size =  iniSHadowSize;
            Grasshopper.GUI.Canvas.GH_Skin.wire_default =  iniWireColor;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_shade = iniShadowColor;
            Grasshopper.Instances.ActiveCanvas.CanvasPaintBackground -= canvasPaintHandler;

        }


        // This method will expire the solution forcing an update of the effects based on the timer
        public void OnElapsed(Object sender, EventArgs e)
        {
            ExpireSolution(true);
        }

        // This method draws the moving images on the background
        public void canvasPaintHandler(Grasshopper.GUI.Canvas.GH_Canvas canvas)
        {
            if (random.NextDouble() > 0.95)
            {
                MovingImage newImage = new MovingImage();
                activeImages.Add(newImage);

                if (activeImages.Count > 10) activeImages.RemoveAt(0);

            }

             foreach (MovingImage img in activeImages)
              {
                img.Update();
                img.Draw();
              }




        }

        // Create new color for background and set it
        public void ChangeColors()
        {
            Color back_color = GenerateNextColor(iniBackColor,back_hue, hsl_counter);
            Grasshopper.GUI.Canvas.GH_Skin.canvas_back = back_color;
            back_hue += 0.8;
            back_hue = back_hue % 360;
            hsl_counter += 0.025;


            if (factor < 1)
            { factor += 0.01; }



        }
        // Create new color for cables and set it
        public void ChangeWireColor()
        {
            Color wire_color = GenerateNextWireColor(iniWireColor, wirecolor_1, wirecolor_2, wirecolor_3);
            wire_color = Color.FromArgb(iniWireColor.A, wire_color.R, wire_color.G, wire_color.B);
            Grasshopper.GUI.Canvas.GH_Skin.wire_default = wire_color;

            wirecolor_1 += 1.23;
            wirecolor_1 = wirecolor_1 % 360;
            wirecolor_2 += 0.05;
            wirecolor_2 = wirecolor_2 % (2*Math.PI);
            wirecolor_3 += 0.01;
            wirecolor_3 = wirecolor_2 % (2*Math.PI);
        }
        // Move components around
        public void MoveComponents(float number)
        {
            
            GH_Document docu = this.OnPingDocument();
            IList<IGH_DocumentObject> objects  = docu.Objects;
            foreach (IGH_DocumentObject obj in objects)
            {


                float x = obj.Attributes.Pivot.X + (float) Math.Sin(hsl_counter) * number * (int)random.NextDouble().Remap(0,1,-1.1,1.1);
                float y = obj.Attributes.Pivot.Y + (float)Math.Cos(hsl_counter) * number * (int)random.NextDouble().Remap(0, 1, -1.1, 1.1);
                obj.Attributes.Pivot = new PointF(x, y);
                obj.Attributes.ExpireLayout();
            }
        }
        // Create new color for shadow and set it
        public void ChangeShadow()
        {

            Color colorshadow = GenerateNextShadowColor(iniShadowColor, shadow_color_counter, shadow_color_counter2, counter3);
            colorshadow = Color.FromArgb(iniShadowColor.A, colorshadow.R, colorshadow.G, colorshadow.B);
            Grasshopper.GUI.Canvas.GH_Skin.canvas_shade = colorshadow;


            if (counter3<0.6)counter3 += 0.01;
            shadow_color_counter += 3;
            shadow_color_counter = shadow_color_counter % 360;
            shadow_color_counter2 += 0.15;
            shadow_color_counter2 = shadow_color_counter2 % (2*Math.PI);
        }


        public Color GenerateNextColor(Color color, double hue_counter, double hs_counter)
        {
            double h;
            double s;
            double v;
            Helpers.RGBToHSV(color.R, color.G, color.B,out h,out s, out v);

            h = (h + factor* hue_counter)%360;
            s += factor * (Math.Sin(hs_counter).Remap(-1, 1, s, 1 - s)) * 0.5;
            if (s > 1.0) s = 1.0;
            if (s < 0) s = 0.0;
            v += factor*(Math.Sin(hs_counter).Remap(-1,1,-v,1-v))*0.2;
            if (v > 1) v = 1.0;
            if (v < 0) v = 0.0;


            return Helpers.RGBFromHSV(h, s, v);

        }
        public Color GenerateNextShadowColor(Color color, double hue_counter, double hs_counter, double counter3)
        {
            double h;
            double s;
            double v;
            Helpers.RGBToHSV(color.R, color.G, color.B, out h, out s, out v);

            h = (h + hue_counter) % 360;
            s += (Math.Sin(hs_counter).Remap(-1, 1, 0.3, 0.9));
            if (s > 1.0) s = 1.0;
            if (s < 0) s = 0.0;
            v += counter3;
            if (v > 0.6) v=0.6;
            if (v < 0.05) v = 0.05;


            return Helpers.RGBFromHSV(h, s, v);

        }
        public Color GenerateNextWireColor(Color color, double hue_counter, double hs_counter, double counter_3)
        {
            double h;
            double s;
            double v;
            Helpers.RGBToHSV(color.R, color.G, color.B, out h, out s, out v);

            h = (h + factor * hue_counter) % 360;
            s += (Math.Sin(hs_counter).Remap(-1, 1, 0.4, 0.8));
            if (s > 1.0) s = 1.0;
            if (s < 0) s = 0.0;
            v += (Math.Cos(counter_3).Remap(-1, 1, 0.0, 0.5));
            if (v > 1) v = 1.0;
            if (v < 0) v = 0.0;


            return Helpers.RGBFromHSV(h, s, v);

        }
    }
}