using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Timers;

namespace PabloUselessComponents
{
    public class Number_Components : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Number_Components class.
        /// </summary>
        public GH_Document docu;
        public int counter = 0;

        public Number_Components()
          : base("Number of Components", "N Comp.",
              "Count number of components in document",
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
            pManager.AddIntegerParameter("Number of componentes", "Num. Comp", "Number of components", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Get the document
            if (docu == null)
            {
                docu = Grasshopper.Instances.ActiveCanvas.Document;
            }


            //Subscribe to events Added and Deleted

            try
            {
                docu.ObjectsAdded -= OnChanged;
                docu.ObjectsAdded += OnChanged;
                docu.ObjectsDeleted -= OnChanged;
                docu.ObjectsDeleted += OnChanged;
            }

            catch { }

            int num_comp = docu.ObjectCount;
            DA.SetData(0, num_comp);



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
            get { return new Guid("1766b25a-ac50-4fe8-b14f-bff2deb8b7b0"); }
        }

        //Event handler that will be expire the solution whenever a component is added or deleted
        public void OnChanged(Object sender, GH_DocObjectEventArgs e)
        {
            ExpireSolution(true);
        }

    }
}