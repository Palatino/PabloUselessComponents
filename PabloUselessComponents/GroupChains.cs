﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

namespace PabloUselessComponents
{
    public class GroupChains : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GroupAll class.
        /// </summary>

        public GH_Document docu;
        public GroupChains()
          : base("GroupChains", "Group Chains",
              "Create groups for each individual chain of components",
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

            //Get al components in the document
            List<IGH_DocumentObject> objects = new List<IGH_DocumentObject>();
            try
            {
                objects = docu.Objects.ToList<IGH_DocumentObject>();
            }
            catch { }

            //Filter those components to only get active components (remove groups, scribbles etc)
            List<IGH_ActiveObject> activeObjects = new List<IGH_ActiveObject>();

            foreach (IGH_DocumentObject obji in objects)
            {
                IGH_ActiveObject actiObj = obji as IGH_ActiveObject;
                if (actiObj != null & obji!= this)
                {
                    activeObjects.Add(actiObj);
                }
            }


            //Sort element in chains
            List<List<IGH_ActiveObject>> chains = new List<List<IGH_ActiveObject>>();

            while (activeObjects.Count > 0)
            {
                //Start with the first document of the list
                List<IGH_ActiveObject> chain = new List<IGH_ActiveObject>();
                IGH_ActiveObject obj = activeObjects[0];


                //Retrieve all the components downstream and retrieve last elements, once the last elements is found 
                //all the upstream elements will be colected from here
                List<IGH_ActiveObject> downObjects = docu.FindAllDownstreamObjects(obj);
                downObjects.Insert(0, obj);



                //Retrieve all the component upstream
                while (downObjects.Count != 0)
                {
                    IGH_ActiveObject lastElement = downObjects[downObjects.Count - 1];

                    List<IGH_DocumentObject> upstream = new List<IGH_DocumentObject>();
                    Helpers.UpStreamObjects(upstream, lastElement, docu);
                    foreach (GH_DocumentObject ob in upstream)
                    {
                        
                        IGH_ActiveObject ob_Active = ob as IGH_ActiveObject;
                        downObjects.Remove(ob_Active);
                        activeObjects.Remove(ob_Active);
                        if (!chain.Contains(ob_Active))
                        { chain.Add(ob_Active); }
                        
                    }


                }

                //Add the chain to the collection of chains
                chains.Add(chain);
            }

            //Group each of the chains

            foreach(List<IGH_ActiveObject>chain in chains)
            {

                Grasshopper.Kernel.Special.GH_Group group = new Grasshopper.Kernel.Special.GH_Group
                {
                    Colour = System.Drawing.Color.AliceBlue
                };
                docu.AddObject(group, false, docu.ObjectCount);

                foreach (IGH_ActiveObject obj in chain)
                {
                    group.AddObject(obj.Attributes.InstanceGuid);

                }

                group.ExpireCaches();

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
            get { return new Guid("ec485bf2-fc15-4502-8f90-15d3e035b27a"); }
        }
    }
}