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
using System.Collections;

namespace RevitTask
{
    [Transaction(TransactionMode.Manual)]
    class Task11_HeirarchyShowElementCount : IExternalCommand
    {
        public static string thisClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
        public static string thisAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        public UIDocument Uid {set; get; }
        public Document Doc { set; get; }
        TaskControl11 TaskObj = new TaskControl11();

        //List<string> output_data = new List<string>();


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Uid = commandData.Application.ActiveUIDocument;
            Doc = Uid.Document;

            PopulateTreeview();

            TaskObj.Show();
            return Result.Succeeded;
        }

        public void PopulateTreeview()
        {
            FilteredElementCollector Levels = new FilteredElementCollector(Doc).OfClass(typeof(Level));

            FilteredElementCollector allElementsInView = new FilteredElementCollector(Doc, Doc.ActiveView.Id);
            IList elementsInView = (IList)allElementsInView.ToElements();

            foreach (Level l in Levels)
            {
                TreeViewItem levels = new TreeViewItem();
                levels.Header = l.Name;
                TaskObj.treeView.Items.Add(levels);

                Dictionary<string, int> ElementAndCount = new Dictionary<string, int>();
                foreach (Element ele in elementsInView)
                {
                    if (null != ele.Category && ele.Category.HasMaterialQuantities)
                    {
                        if(l.Id == ele.LevelId)
                        {
                            if (!ElementAndCount.ContainsKey(ele.Category.Name))
                            {
                                ElementAndCount.Add(ele.Category.Name, 1);
                            }
                            else
                            {
                                ElementAndCount = ElementAndCount.ToDictionary(kvp => kvp.Key, kv => kv.Value + 1);
                            }
                        }
                    }
                }

                foreach (KeyValuePair<string, int> entry in ElementAndCount)
                {
                    TreeViewItem elements = new TreeViewItem();
                    elements.Header = string.Format("{0}" + "(" + "{1}" + ")", entry.Key, entry.Value);
                    levels.Items.Add(elements);
                }
            }

        }
    }
}
