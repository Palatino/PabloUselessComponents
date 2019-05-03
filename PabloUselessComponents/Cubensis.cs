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
        
        static Color OriginalbackColor = Color.FromArgb(255, 212, 208, 200);
        double back_hue = 1;
        double hsl_counter = Math.PI;
        double factor = 0;
        double wirecolor_1 = 0;
        double wirecolor_2 = -0.5 * Math.PI;
        double wirecolor_3 = -0.5 * Math.PI;
        double shadow_color_counter = 0;
        double shadow_color_counter2 = 0;
        double triger = 0;
        float movement_counter = 0;



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
            myTimer = new Timer(150);
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

            ChangeColors();
            if (triger > 10) ChangeShadow();
            if (triger > 30) ChangeWireColor();
            if (triger > 70)
            {   MoveComponents(movement_counter);
                if (movement_counter < 3) movement_counter += (float)0.005;
            }
            if (triger > 10) DrawParticles();
            
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
            Grasshopper.GUI.Canvas.GH_Skin.canvas_shade = iniShadowColor;

        }

        
        public void OnElapsed(Object sender, EventArgs e)
        {
            ExpireSolution(true);
        }



        public void ChangeColors()
        {



            Color back_color = GenerateNextColor(iniBackColor,back_hue, hsl_counter);
            Grasshopper.GUI.Canvas.GH_Skin.canvas_back = back_color;
            back_hue += 0.8;
            back_hue = back_hue % 360;
            hsl_counter += 0.025;


            if (factor < 1)
            { factor += 0.01; }

            if (triger < 100)
            {
                triger += 0.25;
            }

        }
        public void ChangeWireColor()
        {
            Color wire_color = GenerateNextWireColor(iniWireColor, wirecolor_1, wirecolor_2, wirecolor_3);
            wire_color = Color.FromArgb(iniWireColor.A, wire_color.R, wire_color.G, wire_color.B);
            Grasshopper.GUI.Canvas.GH_Skin.wire_default = wire_color;

            wirecolor_1 += 1.23;
            wirecolor_1 = wirecolor_1 % 360;
            wirecolor_2 += 0.1;
            wirecolor_2 = wirecolor_2 % (2*Math.PI);
            wirecolor_3 += 0.05;
            wirecolor_3 = wirecolor_2 % (2*Math.PI);
        }
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
        public void ChangeShadow()
        {
            //Grasshopper.GUI.Canvas.GH_Skin.canvas_shade_size = 100;
            //Grasshopper.GUI.Canvas.GH_Skin.canvas_shade_size += (int)(Math.Sin(shadow_counter).Remap(-1,1,-iniSHadowSize,iniSHadowSize) * factor );
            Color colorshadow = GenerateNextShadowColor(iniShadowColor, shadow_color_counter, shadow_color_counter2);
            colorshadow = Color.FromArgb(iniShadowColor.A, colorshadow.R, colorshadow.G, colorshadow.B);
            Grasshopper.GUI.Canvas.GH_Skin.canvas_shade = colorshadow;



            shadow_color_counter += 3;
            shadow_color_counter = shadow_color_counter % 360;
        }
        public void DrawParticles()
        {
            Grasshopper.GUI.Canvas.GH_Canvas canvas = Grasshopper.Instances.ActiveCanvas;
            Graphics graphics = canvas.CreateGraphics();
            Bitmap img = new Bitmap(Properties.Resources.Cubensis2);
            graphics.DrawImage(img, this.Attributes.Pivot);
            for (int i = 0; i < 10; i++)
            {
                //Pen pen = new Pen(Color.FromArgb((int)(random.NextDouble() * 255), (int)(random.NextDouble() * 255), (int)(random.NextDouble() * 255)));
                //PointF p1 = new PointF((float)random.NextDouble() * 1920, (float)random.NextDouble() * 1080);
                ////PointF p2 = new PointF((float)(p1.X + random.NextDouble()*20) , (float)(p1.Y + random.NextDouble() * 20));
                //PointF p2 = new PointF(p1.X + 1, p1.Y);
                ////graphics.DrawEllipse(pen, p1.X, p1.Y, 5, 5);
                //Rectangle rec = new Rectangle((int)p1.X, (int)p1.Y, 5, 5);
                //SolidBrush brush = new SolidBrush(Color.FromArgb(100, (int)(random.NextDouble() * 255), (int)(random.NextDouble() * 255), (int)(random.NextDouble() * 255)));
                //graphics.FillEllipse(brush, rec);



            }
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
        public Color GenerateNextShadowColor(Color color, double hue_counter, double hs_counter)
        {
            double h;
            double s;
            double v;
            Helpers.RGBToHSV(color.R, color.G, color.B, out h, out s, out v);

            h = (h + hue_counter) % 360;
            s += (Math.Sin(hs_counter).Remap(-1, 1, 0.2, 0.8));
            if (s > 1.0) s = 1.0;
            if (s < 0) s = 0.0;
            v += (Math.Cos(hs_counter).Remap(-1, 1, 0.2, 0.8));
            if (v > 1) v = 1.0;
            if (v < 0) v = 0.0;


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
            v += (Math.Cos(counter_3).Remap(-1, 1, 0.4, 0.8));
            if (v > 1) v = 1.0;
            if (v < 0) v = 0.0;


            return Helpers.RGBFromHSV(h, s, v);

        }
    }
}