using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace PabloUselessComponents
{
    public class PabloUselessComponentsInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Useless Components";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Explore Grasshopper API";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("6963ce24-82a6-4436-9b26-d381ec30fdae");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Pablo Alvarez - 2019";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "www.linkedin.com/in/palvarezrio";
            }
        }
    }
}
