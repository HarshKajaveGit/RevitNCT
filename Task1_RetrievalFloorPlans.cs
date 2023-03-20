using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Threading.Tasks;
using System.Reflection;
using Autodesk.Revit.Attributes;
using System.Windows;
using System.Collections;

namespace RevitTask
{
    [Transaction(TransactionMode.Manual)]       //Transaction behaviour
    [Regeneration(RegenerationOption.Manual)]
    public class Task1_RetrievalFloorPlans:IExternalCommand
    {
        //     System.Reflection.MethodBase.GetCurrentMethod is a static method that is called
        //     from within an executing method and that returns information about that method.
        //     A MethodBase object representing the currently executing method.

        public static string thisClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;  //Gets the class that declares this member.
        public static string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;           //Gets the full path or UNC location of the loaded file that contains the manifest.

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {


                //     External API commands can access this property in read-only mode only! The ability
                //     to modify the property is reserved for future implementations.
                UIDocument uidoc = commandData.Application.ActiveUIDocument;

                //     Returns the database level document represented by this UI-level document.
                Document doc = uidoc.Document;

                //Constructs a new FilteredElementCollector that will search and filter the set
                //     of elements in a document.
                FilteredElementCollector levels = new FilteredElementCollector(doc);
                levels.OfClass(typeof(Level));

                if (levels !=null)
                {
                    string level = "";
                    string plan = "";
                    foreach (Level e in levels)
                    {
                        level += "\n" + e.Name.ToString();
                        plan += "\n" + e.Elevation.ToString();
                        e.GetPlaneReference();
                    }

                    MessageBox.Show("Floor Levels: " + level + "\n" + "Floor Plans: " + plan);
                }
                else
                {
                    MessageBox.Show("Floor Levels is present in the active-view");
                }
            }catch(Exception ex)
            {

            }
            return Result.Succeeded;
        }
      
    }
}
