using System;
using System.Collections.Generic;
using System.Windows;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace MyRhinoPlugin
{
    public class GeneratePlane_Command : Command
    {
        public GeneratePlane_Command()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static GeneratePlane_Command Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "MyRhinoCommand";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {

            Point3d centerPoint = new Point3d(0, 0, 0);
            float lenX = 10.0f;
            float lenY = 10.0f;

            GetPoint gp = new GetPoint();
            gp.SetCommandPrompt("Input Center Point of the plate");

            GetResult get_center = gp.Get();
            if (get_center == GetResult.Point)
            {
                centerPoint = gp.Point();
                //doc.Objects.AddPoint(gp.Point());
            }

            GetInteger getX = new GetInteger();
            getX.SetCommandPrompt("Input Length X of plate");
            GetResult getLengthX = getX.Get();
            if (getX.CommandResult() != Result.Success) return getX.CommandResult();
            if (getLengthX == GetResult.Number)
            {
                lenX = getX.Number();
            }


            GetInteger getY = new GetInteger();
            getY.SetCommandPrompt("Input Length Y of plate");
            GetResult getLengthY = getY.Get();
            if (getY.CommandResult() != Result.Success) return getY.CommandResult();
            if (getLengthY == GetResult.Number)
            {
                lenY = getY.Number();
            }

            MessageBox.Show($"LengthX: {lenX.ToString()} \nLengthY: {lenY.ToString()}");

            Point3d pt0 = new Point3d(centerPoint.X - lenX / 2, centerPoint.Y - lenY / 2, centerPoint.Z);
            Point3d pt1 = new Point3d(centerPoint.X + lenX / 2, centerPoint.Y + lenY / 2, centerPoint.Z);

            //doc.Objects.AddLine(pt0, pt1);

            Point3d p1 = pt0;
            Point3d p4 = pt1;
            Point3d p2 = new Point3d(p4.X, p1.Y, p1.Z);
            Point3d p3 = new Point3d(p1.X, p4.Y, p1.Z);

            var surface = NurbsSurface.CreateFromCorners(p1, p2, p4, p3);
            doc.Objects.AddSurface(surface);

            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command added one plate to the document.", EnglishName);

            return Result.Success;
        }
    }
}
