using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using System.Drawing;

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
            if (!upComponents.Contains(obj))
            {
                upComponents.Add(obj);
            }

            IList<IGH_Param> params2 = param.Sources;
            foreach (IGH_Param p in params2)
            {
                Guid id =  p.Attributes.GetTopLevel.InstanceGuid;
                IGH_DocumentObject comp2 = docu.FindObject(id, true);
                IGH_ActiveObject PA = comp2 as IGH_ActiveObject;
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
                if (!upComponents.Contains(obj))
                {
                    upComponents.Add(obj);
                }

            }

            else
            {
                if (!upComponents.Contains(obj))
                {
                    upComponents.Add(obj);
                }


                foreach (IGH_DocumentObject obj3 in PreviousComponents)
                {
                    UpStreamObjects(upComponents, obj3, docu);
                }
            }

        }






    }

    public static Color RGBFromHSV(double hue, double saturation, double value)
    {

        //The ranges are 0 - 360 for hue, and 0 - 1 for saturation or value.

        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value = value * 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if (hi == 0)
            return Color.FromArgb(255, v, t, p);
        else if (hi == 1)
            return Color.FromArgb(255, q, v, p);
        else if (hi == 2)
            return Color.FromArgb(255, p, v, t);
        else if (hi == 3)
            return Color.FromArgb(255, p, q, v);
        else if (hi == 4)
            return Color.FromArgb(255, t, p, v);
        else
            return Color.FromArgb(255, v, p, q);
    }

    public static void RGBToHSV(int r, int g, int b, out double out_h, out double out_s, out double out_v)
    {
        double delta, min;
        double h = 0, s, v;

        min = Math.Min(Math.Min(r, g), b);
        v = Math.Max(Math.Max(r, g), b);
        delta = v - min;

        if (v == 0.0)
            s = 0;
        else
            s = delta / v;

        if (s == 0)
            h = 0.0;

        else
        {
            if (r == v)
                h = (g - b) / delta;
            else if (g == v)
                h = 2 + (b - r) / delta;
            else if (b == v)
                h = 4 + (r - g) / delta;

            h *= 60;

            if (h < 0.0)
                h = h + 360;
        }

        out_h = h;
        out_v = v/255;
        out_s = s;
    }

}
