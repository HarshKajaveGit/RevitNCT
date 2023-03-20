using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevitTask
{
    [Transaction(TransactionMode.Manual)]
    public class Task6_RetrievalWallsOfSelectedRoom : IExternalCommand
    {
        public static string thisClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
        public static string thisAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        public ElementType etype { get; set; }
        public Element ele { get; set; }
        List<string> all_details = new List<string>();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument Uid = commandData.Application.ActiveUIDocument;
            Document Doc = Uid.Document;

            //Autodesk.Revit.UI.Selection.Selection selection = Uid.Selection;
            ICollection<Autodesk.Revit.DB.ElementId> selectedIds = Uid.Selection.GetElementIds();

            //Select a room
            if(selectedIds.Count != 0)
            {
                Room room = Doc.GetElement(selectedIds.ToArray()[0]) as Room;

                //MessageBox.Show(room.Name);
                //Family instance

                FilteredElementCollector rooms = new FilteredElementCollector(Doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_Rooms);

                SpatialElementBoundaryOptions opts = new SpatialElementBoundaryOptions();
           
                foreach (Room r in rooms)
                {
                    if (r.Name == room.Name)
                    {
                        IList<IList<BoundarySegment>> boundary = r.GetBoundarySegments(opts);

                        foreach (BoundarySegment bs in boundary[0])
                        {
                            Element eFromString = Doc.GetElement(bs.ElementId);
                            all_details.Add(eFromString.Name);
                        }
                    }
                }

                if(all_details.Count != 0)
                {
                    MessageBox.Show(string.Join(Environment.NewLine, all_details), "List of walls");
                }
                else
                {
                    MessageBox.Show("In this Model there are no room's");  //Added
                }
            }
            else
            {
                MessageBox.Show("Please select a room");    //Added
            }
            return Result.Succeeded;
        }
    }
}

//Added Validation 