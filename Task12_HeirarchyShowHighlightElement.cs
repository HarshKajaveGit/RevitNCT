using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RevitTask.Views;
using System.Windows;

namespace RevitTask
{
    [Transaction(TransactionMode.Manual)]
    class Task12_HeirarchyShowHighlightElement : IExternalCommand
    {
        public static string thisClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
        public static string thisAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        public UIDocument Uid { get; set; }
        public Document Doc { get; set; }

        TaskControl12 TaskObj = new TaskControl12();
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Uid = commandData.Application.ActiveUIDocument;
            Doc = Uid.Document;

            PopulateTreeView();
            TaskObj.Show();

            return Result.Succeeded;
        }
        public void PopulateTreeView()
        {
            FilteredElementCollector LevelCollector = new FilteredElementCollector(Doc).OfClass(typeof(Level));
            FilteredElementCollector AllVisibleView = new FilteredElementCollector(Doc, Doc.ActiveView.Id);

            foreach (Level lev in LevelCollector)
            {
                TreeViewItem levels = new TreeViewItem();
                levels.Header = lev.Name;
                TaskObj.TreeView.Items.Add(levels);

                Dictionary<string, int> ElementWithCount = new Dictionary<string, int>();
                foreach (Element ele in AllVisibleView)
                {
                    if (null != lev.Category.Name)
                    {
                        if (lev.Id == ele.LevelId)
                        {
                            if (!ElementWithCount.ContainsKey(ele.Category.Name))
                            {
                                ElementWithCount.Add(ele.Category.Name, 1);
                            }
                            else
                            {
                                ElementWithCount = ElementWithCount.ToDictionary(kvp => kvp.Key, kv => kv.Value + 1);
                            }
                        }
                    }
                }
                foreach (KeyValuePair<string, int> entry in ElementWithCount)
                {
                    TreeViewItem elements = new TreeViewItem();
                    elements.Header = string.Format("{0}" + " (" + "{1}" + ")", entry.Key, entry.Value);
                    levels.Items.Add(elements);

                    elements.MouseDoubleClick += Elements_MouseDoubleClick;
                }
            }
        }

        private void Elements_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Highlight_Elements();
        }
        public void Highlight_Elements()
        {
            try
            {
                string selected = TaskObj.TreeView.SelectedItem.ToString();

                //string[] selected = selectedTemp.Split('(');

                FilteredElementCollector collector = new FilteredElementCollector(Doc);
                ICollection<Element> collection = collector.OfClass(typeof(Level)).ToElements();

                FilteredElementCollector allElementsInView = new FilteredElementCollector(Doc, Doc.ActiveView.Id);
                IList elementsInView = (IList)allElementsInView.ToElements();

                List<ElementId> wallids = new List<ElementId>();
              
                foreach (Element elem in elementsInView)
                {
                    try
                    {
                        if (elem.Category != null)
                        {
                            string[] splitSelected = selected.Split('(');
                            if (selected.Contains(elem.Category.Name))
                            {
                                wallids.Add(elem.Id);
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                }

                Element e = Doc.GetElement(wallids.First());

                foreach (Level l in collector)
                {
                    if (l.Id == e.LevelId)
                    {
                        FilteredElementCollector viewCollecter = new FilteredElementCollector(Doc).OfClass(typeof(View));

                        foreach (View v in viewCollecter)
                        {
                            if (v.Title.Contains("Floor"))
                            {
                                if (v.Name == l.Name)
                                {
                                    Uid.ActiveView = v;
                                    Uid.Selection.SetElementIds(wallids);
                                    break;
                                }
                            }
                        }
                        if (selected == "")
                        {
                            System.Windows.MessageBox.Show("Please Select Appropriate View from Dropdownlist");
                            break;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }
    }
}
