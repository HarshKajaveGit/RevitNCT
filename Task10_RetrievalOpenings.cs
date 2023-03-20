using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevitTask
{
    [Transaction(TransactionMode.Manual)]
    class Task10_RetrievalOpenings : IExternalCommand
    {
        public static string thisClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public static string thisAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        UIDocument Uid { get; set; }
        Document Doc { get; set; }

        List<string> Wall_Opening = new List<string>();
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Uid = commandData.Application.ActiveUIDocument;
            Doc = Uid.Document;

            GetWallOpenings();

            string allopening = string.Join(Environment.NewLine, Wall_Opening);
            MessageBox.Show(allopening);
           

            return Result.Succeeded;
        }
        public void GetWallOpenings()
        {
            try
            {
                //Selection selection = Uid.Selection;
                ICollection<Autodesk.Revit.DB.ElementId> selectedIds = Uid.Selection.GetElementIds();

                if (selectedIds.Count != 0)
                {
                    Wall selected_Wall = Doc.GetElement(selectedIds.ToArray()[0]) as Wall;

                    //FilteredElementCollector allElementsInView = new FilteredElementCollector(Doc, Doc.ActiveView.Id);
                    //IList elementIdInView = (IList)allElementsInView.ToElements();

                    Wall_Opening.Add("Wall Name: " + selected_Wall.Name);

                    IEnumerable<FamilyInstance> familyInstance = new FilteredElementCollector(Doc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType().Cast<FamilyInstance>();

                    foreach (FamilyInstance fi in familyInstance)
                    {
                        try
                        {
                            if (fi.Host != null)
                            {
                                if (fi.Host.Id.Equals(selected_Wall.Id))
                                {
                                    Wall_Opening.Add("     Opening present- Category :" + fi.Category.Name + "   " + "Name :" + fi.Name);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }
                    if (Wall_Opening.Count == 1)
                    {
                        Wall_Opening.Add(" No openings are present ");
                    }
                }
                else
                {
                    Wall_Opening.Add(" Please select wall before click on the button ");
                }
            }
            catch (Exception e)
            {

            }               
        }
    }
}
