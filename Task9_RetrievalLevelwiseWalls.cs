using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitTask.Views;
using System.Collections;
using System.Windows.Forms;

namespace RevitTask
{
    [Transaction(TransactionMode.Manual)] 
    class Task9_RetrievalLevelwiseWalls : IExternalCommand
    {
        public static string thisClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
        public static string thisAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        List<string> wpf_output_data = new List<string>();

        TaskControl9 ControlObj = new TaskControl9();

        UIDocument Uid { get; set; }
        Document Doc { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Uid = commandData.Application.ActiveUIDocument;
            Doc = Uid.Document;

            LevelWiseWall();

            ControlObj.Show();

            ControlObj.textBox.Text = string.Join(Environment.NewLine, wpf_output_data);

            return Result.Succeeded;
        }
        public void LevelWiseWall()
        {
            FilteredElementCollector levelCollector = new FilteredElementCollector(Doc).OfClass(typeof(Level));

            FilteredElementCollector allElementsInView = new FilteredElementCollector(Doc, Doc.ActiveView.Id);

            foreach (Level level in levelCollector)
            {
                wpf_output_data.Add(level.Name);

                foreach(Element ele in allElementsInView)
                {
                    if(level.Id == ele.LevelId)
                    {
                        if(ele.GetType() == typeof(Wall))
                        {
                            wpf_output_data.Add("  Name: " + ele.Name);
                            wpf_output_data.Add("  Width: " + ((Autodesk.Revit.DB.Wall)ele).Width.ToString());
                            wpf_output_data.Add("  Length: " + ele.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble().ToString() + "\n");
                        }
                    }
                }
            }
        }
    }
}
