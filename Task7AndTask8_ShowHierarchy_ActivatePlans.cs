using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitTask.Views;
using System.Windows.Controls;
using System.Windows.Forms;

namespace RevitTask
{
    [Transaction(TransactionMode.Manual)]
    class Task7AndTask8_ShowHierarchy_ActivatePlans : IExternalCommand
    {
        public static string thisClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
        public static string thisAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        List<string> floor_plans = new List<string>();
        
        UIDocument Uid { get; set; }
        Document Doc { get; set; }

        TaskControl7 TaskControl7obj = new TaskControl7();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Uid = commandData.Application.ActiveUIDocument;
            Doc = Uid.Document;

            //Filter Views
            FilteredElementCollector collector = new FilteredElementCollector(Doc).OfClass(typeof(Autodesk.Revit.DB.View));

            string DocName = Doc.Title;

            TaskControl7obj.treeView.Items.Add(DocName);

            TreeViewItem floorHeading = new TreeViewItem();
            floorHeading.Items.Add("Floor");

            TaskControl7obj.treeView.Items.Add(floorHeading);

            TreeViewItem floors = new TreeViewItem();

            foreach(Element viewElement in collector)
            {
                Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;
                if (view.Title.Contains("Floor"))
                {
                    floors.Items.Add(view.Name); 
                }
            }
            floorHeading.Items.Add(floors);
            //TaskControl7obj.treeView.Items.Add(floors);

            //string selected = TaskControl7obj.treeView.SelectedItem.ToString();
            //MessageBox.Show(selected);

            //TaskControl7obj.treeView.Items.Add("head2");

            TaskControl7obj.MouseDoubleClick += treeView_MouseDoubleClick;
            TaskControl7obj.Show();
            return Result.Succeeded;
        }
        private void treeView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Activate_Plan(Uid, Doc);
        }
        private void Activate_Plan(UIDocument uid, Document doc)
        {
            try
            {
                string selected = TaskControl7obj.treeView.SelectedItem.ToString();
                FilteredElementCollector collector = new FilteredElementCollector(Doc).OfClass(typeof(Autodesk.Revit.DB.View));

                foreach(Element viewElement in collector)
                {
                    Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;
                    if (view.Title.Contains("Floor"))
                    {
                        if(view.Name == selected)
                        {
                            uid.ActiveView = view;
                        }
                    }
                }
            }
            catch(Exception e)
            {

            }
        }
    }
}
