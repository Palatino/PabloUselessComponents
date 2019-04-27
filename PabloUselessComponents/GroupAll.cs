using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

namespace PabloUselessComponents
{
    public class GroupAll : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GroupAll class.
        /// </summary>

        public GH_Document docu;
        public GroupAll()
          : base("GroupAll", "Group All",
              "Description",
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
            docu = this.OnPingDocument();
            List<IGH_DocumentObject> objects = new List<IGH_DocumentObject>();
            try
            {
                objects = docu.Objects.ToList<IGH_DocumentObject>();
            }
            catch { }

            objects.Remove(this);

            Grasshopper.Kernel.Special.GH_Group group = new Grasshopper.Kernel.Special.GH_Group();
            group.Colour = System.Drawing.Color.AliceBlue;
            docu.AddObject(group, false, docu.ObjectCount);

            foreach ( IGH_DocumentObject obj in objects)
            {
                group.AddObject(obj.Attributes.InstanceGuid);
          
            }

            group.ExpireCaches();

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
            get { return new Guid("ec485bf2-fc15-4502-8f90-15d3e035b27a"); }
        }
    }
}