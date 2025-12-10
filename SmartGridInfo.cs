using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace SmartGrid
{
    public class SmartGridInfo : GH_AssemblyInfo
    {
        public override string Name => "SmartGrid";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("d779a60c-766a-4810-ad1e-9a7ec2f07935");

        //Return a string identifying you or your company.
        public override string AuthorName => "Dwarkesh deore";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";

        //Return a string representing the version.  This returns the same version as the assembly.
        public override string AssemblyVersion => GetType().Assembly.GetName().Version.ToString();
    }
}