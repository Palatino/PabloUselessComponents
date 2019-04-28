using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

public static class Helpers
{
    public static double Remap(this double value, double from1, double to1, double from2, double to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static void UpStreamObjects(List<IGH_DocumentObject> upComponents , IGH_DocumentObject obj, GH_Document docu)
    {
        //Find all the upstream components from a given starting node.


        IGH_Param param = obj as IGH_Param;
        if (param != null)
        {
            upComponents.Add(obj);
            IList<IGH_Param> params2 = param.Sources;
            foreach (IGH_Param p in params2)
            {
                IGH_ActiveObject PA = p as IGH_ActiveObject;
                UpStreamObjects(upComponents, PA, docu);
            }

        }

        IGH_Component comp = obj as IGH_Component;

        if(comp!= null)
        {
            List<IGH_DocumentObject> PreviousComponents = new List<IGH_DocumentObject>();


            List<IGH_Param> paramtrs = comp.Params.Input;

            foreach (IGH_Param pm in paramtrs)
            {
                IList<IGH_Param> sources = pm.Sources;
                foreach (IGH_Param pm2 in sources)
                {
                    Guid id = pm2.Attributes.GetTopLevel.InstanceGuid;
                    IGH_DocumentObject comp2 = docu.FindObject(id, true);
                    if (comp2 != null)
                    {
                        PreviousComponents.Add(comp2);
                    }

                }

            }

            if (PreviousComponents.Count == 0)
            {
                upComponents.Add(obj);
            }

            else
            {
                upComponents.Add(obj);
                foreach (IGH_DocumentObject obj3 in PreviousComponents)
                {
                    UpStreamObjects(upComponents, obj3, docu);
                }
            }

        }






    }
}