using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitTask.Views;
using System.Windows;
using Autodesk.Revit.Attributes;

namespace RevitTask
{
    [Transaction(TransactionMode.Manual)]
    class Task5_ActivateAndShowRoomList : IExternalCommand
    {
        public static string thisClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
        public static string thisAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        TaskControl5 TaskControl5Obj = new TaskControl5();

        public Document Doc { set; get; }
        public UIDocument Uid {set; get;}

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Uid = commandData.Application.ActiveUIDocument;
                Doc = Uid.Document;

                FilteredElementCollector collector = new FilteredElementCollector(Doc).OfClass(typeof(View));

                TaskControl5Obj.Combo.AllowDrop = true;

                foreach(Element viewElement in collector)
                {
                    Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;

                    if (view.Title.Contains("Floor"))
                    {
                        TaskControl5Obj.Combo.Items.Add(view.Name);
                    }
                }
                TaskControl5Obj.Show();

                TaskControl5Obj.btn_Activate_Plan.Click += Activate_Plan_Click;
                TaskControl5Obj.btn_Show_Room.Click += Show_Room_Click;
            }
            catch(Exception e)
            {

            }
            return Result.Succeeded;
        }

        public void Activate_Plan_Click(object sender, RoutedEventArgs e)
        {
            ActivatePlan(Doc, Uid);
        }

        public void Show_Room_Click(object sender, RoutedEventArgs e)
        {
            RoomList(Doc, Uid);
        }

        public void ActivatePlan(Document Doc, UIDocument Uid)
        {
            try
            {
                FilteredElementCollector collector = new FilteredElementCollector(Doc).OfClass(typeof(View));

                foreach(Element viewElement in collector)
                {
                    Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;

                    if (view.Title.Contains("Floor"))
                    {
                        if(view.Name == TaskControl5Obj.Combo.Text)
                        {
                            Uid.ActiveView = view;
                            break;    
                        }
                    }
                    else if (TaskControl5Obj.Combo.Text == "")
                    {
                        System.Windows.MessageBox.Show("Please Select Appropriate Level from Dropdownlist");
                        break;
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        public void RoomList(Document Doc, UIDocument Uid)
        {
            try
            {
                FilteredElementCollector collector = new FilteredElementCollector(Doc); //search  filter the set of elements in a document.

                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Rooms);

                IList<Element> floors = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

                List<string> all_details = new List<string>();
                for (int i = 0; i < floors.Count; i++)
                {
                    all_details.Add(floors[i].Name);
                }
                var message1 = string.Join(Environment.NewLine, all_details);
                System.Windows.MessageBox.Show(message1, "Rooms present in this view");
            }
            catch(Exception e)
            {
                MessageBox.Show("Room's are not present in an Active view");
            }
        }
    }
}
