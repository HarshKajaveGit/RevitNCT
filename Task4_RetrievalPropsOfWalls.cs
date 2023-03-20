using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RevitTask.Views;
using System.Windows;
using Autodesk.Revit.DB.Structure;

namespace RevitTask
{
    [Transaction(TransactionMode.Manual)]
    public class Task4_RetrievalPropsOfWalls : IExternalCommand
    {
        public static string thisClassName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        public static string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

        public Reference pickObj { set; get; }
        public Element element { set; get; }
        public ElementType eleType { set; get; }

        public UIDocument Uid { set; get; }
        public Document Doc { set; get; }

        public Wall selectedWall { get; set; }

        TaskControl4 TaskControlObj = new TaskControl4();

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {

                Uid = commandData.Application.ActiveUIDocument;
                Doc = Uid.Document;

                //Pick element from user
                pickObj = Uid.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                //Retrive Element
                ElementId eid = pickObj.ElementId;
                element = Doc.GetElement(eid);

                //Get element type
                ElementId EleTypeId = element.GetTypeId();
                eleType = Doc.GetElement(EleTypeId) as ElementType;

                selectedWall = Doc.GetElement(pickObj) as Wall;         //Added

                if (eleType.FamilyName.Contains("Wall"))
                {
                    if (pickObj != null)
                    {
                        TaskControlObj.Show();
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Select only Walls");
                }
                TaskControlObj.btn4.Click += ShowBtnClick;

            }
            catch (Exception ex) { }

            return Result.Succeeded;
        }
        public void ShowBtnClick(object sender, RoutedEventArgs e)
        {
            GetPropOfWall();                                    //Added
            //ShowProp();
        }
        public void ShowProp()
        {
            List<string> DetailList = new List<string>();

            string name = eleType.Name;

            string width = ((Autodesk.Revit.DB.WallType)eleType).Width.ToString();
            string height = element.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsValueString();

            DetailList.Add(name);
            DetailList.Add("Width " + width);
            DetailList.Add("Length " + height);

            var message = String.Join(Environment.NewLine, DetailList);

            MessageBox.Show(message);

        }
        public void GetPropOfWall()                                          //Added
        {
            try
            {
                List<string> wallProperties = new List<string>();

                wallProperties.Add("Wall Name: " + selectedWall.Name.ToString());
                wallProperties.Add(" CrossSection: " + selectedWall.CrossSection.ToString());
                //wallProperties.Add(selectedWall.CurtainGrid.ToString()); 
                wallProperties.Add(" Flipped: " + selectedWall.Flipped.ToString());
                wallProperties.Add(" isStackedWall: " + selectedWall.IsStackedWall.ToString());
                wallProperties.Add(" isStackedWallMember: " + selectedWall.IsStackedWallMember.ToString());
                wallProperties.Add(" Orientation: " + selectedWall.Orientation.ToString());
                wallProperties.Add(" StructuralUsage: " + selectedWall.StructuralUsage.ToString());
                wallProperties.Add(" WallType: : " + selectedWall.WallType.ToString());
                wallProperties.Add(" Width: " + selectedWall.Width.ToString());

                var msg = string.Join(Environment.NewLine, wallProperties);
                MessageBox.Show(msg);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
