using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace RevitTask
{
    [Transaction (TransactionMode.Manual)]
    public class Task2_ActivateFloorPlan: IExternalCommand
    {
        public static string thisClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        public static string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

        public Document Doc { get; set; }
        public UIDocument UiDoc { get; set; }

        TaskControl2 taskObj = new TaskControl2();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UiDoc = commandData.Application.ActiveUIDocument;
                Doc = UiDoc.Document;

                FilteredElementCollector levels = new FilteredElementCollector(Doc);
                levels.OfClass(typeof(Level));

                if(levels != null)
                {
                    taskObj.combo_2.AllowDrop = true;

                    foreach (Level e in levels)
                    {
                        taskObj.combo_2.Items.Add(e.Name);
                    }
                    taskObj.Show();

                    taskObj.btn_1.Click += Show_Button_Click;
                }
                else
                {
                    MessageBox.Show("Level is not present in active-view");
                }
            }catch(Exception ex) { }

            return Result.Succeeded;
        }
        public void Show_Button_Click(object sender, RoutedEventArgs e)
        {
            Click_Show(Doc, UiDoc);
        }

        public void Click_Show(Document Doc, UIDocument UiDoc)
        {
            try
            {
                FilteredElementCollector ViewCollector = new FilteredElementCollector(Doc).OfClass(typeof(Autodesk.Revit.DB.View));

                foreach(Element viewElement in ViewCollector)
                {
                    View view = (View)viewElement;
                    if (view.Title.Contains("Floor"))
                    {
                        if(view.Name == taskObj.combo_2.Text)
                        {
                            UiDoc.ActiveView = view;
                            break;
                        }
                    }
                    else if (taskObj.combo_2.Text == "")
                    {
                        MessageBox.Show("Please select Appropriate level");
                        break;
                    }
                }
            }catch(Exception ex) { }            

        }
    }
}
