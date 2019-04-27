using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

namespace PabloUselessComponents
{
    public class MoveComponents : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MoveComponents class.
        /// </summary>
        public GH_Document docu;
        public MoveComponents()
          : base("Move_Components", "Move Comp",
              "Move components in canvas to follow a given curve",
              "Useless Components", "Components")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Crv", "Curve to follow", GH_ParamAccess.item);
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
            Curve refCurve = null;
            DA.GetData(0, ref refCurve);
            docu = this.OnPingDocument();
            List<IGH_DocumentObject> objects = new List<IGH_DocumentObject>();
            try
            {
                objects = docu.Objects.ToList<IGH_DocumentObject>();

                objects = GH_Document.FilterSelected(objects);
            }
            catch
            {

            }

            if (objects.Count > 1)
            {

                int elements = objects.Count();
                float minX = objects[0].Attributes.Pivot.X;
                float maxX = objects[0].Attributes.Pivot.X;
                float minY = objects[0].Attributes.Pivot.Y;
                float maxY = objects[0].Attributes.Pivot.Y;



                foreach (IGH_DocumentObject obj in objects)
                {
                    if (obj != this)
                    {
                        if (obj.Attributes.Pivot.X < minX) minX = obj.Attributes.Pivot.X;
                        if (obj.Attributes.Pivot.X > maxX) maxX = obj.Attributes.Pivot.X;
                        if (obj.Attributes.Pivot.Y < minY) minY = obj.Attributes.Pivot.Y;
                        if (obj.Attributes.Pivot.Y > maxY) maxY = obj.Attributes.Pivot.Y;
                    }

                }

                float xAmplitude = Math.Abs(minX - maxX);
                float yAmplitude = Math.Abs(minY - maxY);

                float minMapping;
                float maxMapping;

                if (xAmplitude >= yAmplitude)
                {
                    minMapping = minX;
                    maxMapping = maxX;
                }

                else
                {
                    minMapping = minY;
                    maxMapping = maxY;

                }

                refCurve.Domain = new Interval(0, 1);
                List<double> parameters = new List<double>();
                for (double i = 0; i < 1; i += 1.0 / elements)
                {
                    parameters.Add(i);
                }

                List<Point3d> insertionPoints = new List<Point3d>();



                List<double> Xs = new List<double>();
                List<double> Ys = new List<double>();


                foreach (double param in parameters)
                {
                    Point3d pt = refCurve.PointAt(param);
                    Xs.Add(pt.X);
                    Ys.Add(pt.Y);
                }

                List<double> mappedXs = new List<double>();
                List<double> mappedYs = new List<double>();

                double minFX = Xs.Min();
                double maxFX = Xs.Max();
                double minFY = Ys.Min();
                double maxFY = Ys.Max();

                foreach (double x in Xs)
                {
                    mappedXs.Add(x.Remap(minFX, maxFX, minMapping, maxMapping));
                }

                foreach (double y in Ys)
                {
                    mappedYs.Add(y.Remap(minFY, maxFY, maxMapping, minMapping));
                }

                for (int i = 0; i < objects.Count(); i++)
                {
                    objects[i].Attributes.Pivot = new System.Drawing.PointF((float)mappedXs[i], (float)mappedYs[i]);
                }
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2d534592-fe45-428b-98cb-48070b438b06"); }
        }
    }
}
