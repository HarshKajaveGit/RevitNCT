using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevitTask
{
    [Transaction(TransactionMode.Manual)]
    public class Task3_ActivateAndHighlightWalls : IExternalCommand
    {
        public static string thisClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        public static string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

        public Document Doc { set; get; }
        public UIDocument UiDoc { set; get; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UiDoc = commandData.Application.ActiveUIDocument;
                Doc = UiDoc.Document;

                FilteredElementCollector viewCollector = new FilteredElementCollector(Doc).OfClass(typeof(View));
                FilteredElementCollector wallCollector = new FilteredElementCollector(Doc).OfClass(typeof(Wall));

                List<ElementId> wallids = new List<ElementId>();

                foreach (Element viewElement in viewCollector)
                {
                    View view = (View)viewElement;

                    if(view.Title.Contains("Floor"))
                    {
                        if(view.Name == UiDoc.ActiveView.Name)
                        {
                            UiDoc.ActiveView = view;
                            foreach(Element wall in wallCollector)
                            {
                                wallids.Add(wall.Id);
                            }
                            UiDoc.Selection.SetElementIds(wallids);
                        }
                    }
                }
                if (wallids.Count == 0)
                {
                    MessageBox.Show("Walls is not present on Active Floor Plan");
                }
            }
            catch(Exception ex) { }
            return Result.Succeeded;
        }
    }

}
