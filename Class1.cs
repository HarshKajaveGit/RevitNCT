using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using System.Reflection;

namespace RevitTask
{
    public class Class1 : IExternalApplication
    {
        public static void AddButton(string InternalName ,string ButtonContent, string thisClassName, string thisAssemblyPath,  RibbonPanel panel)
        {
            try
            {
                PushButtonData buttonData = new PushButtonData(InternalName, ButtonContent, thisAssemblyPath, thisClassName);
                PushButton pushButton = panel.AddItem(buttonData) as PushButton;
                //pushButton.ToolTip = "my first plugin\nVersion : 1.1.0";
                
            }
            catch (Exception ex)
            {
            }
        }

        Result IExternalApplication.OnStartup(UIControlledApplication application)
        {
            string ncircleTab = " Revit Tab";
            try
            {
                application.CreateRibbonTab(ncircleTab);        //Create Tab
            }
            catch (Exception e) { }

            //Panel Names
            string PanelName1 = " Revit First'st Panel";
            string PanelName2 = " Revit Second'nd Panel";
            string PanelName3 = " Revit Third'rd Panel";
            string PanelName4 = " Revit Four'th Panel";
            string PanelName5 = " Revit Fif'th Panel";
            string PanelName6 = " Revit Six'th Panel";
            string PanelName7_8 = " Revit Seven and Eight Panel";
            string PanelName9 = " Revit Nine'th Panel";
            string PanelName10 = " Revit Ten'th Panel";
            string PanelName11 = " Revit Eleventh'th Panel";
            string PanelName12 = " Revit Twelve Panel";

            //Creating Panels
            RibbonPanel Panel1 = application.CreateRibbonPanel(ncircleTab, PanelName1);
            RibbonPanel Panel2 = application.CreateRibbonPanel(ncircleTab, PanelName2);
            RibbonPanel Panel3 = application.CreateRibbonPanel(ncircleTab, PanelName3);
            RibbonPanel Panel4 = application.CreateRibbonPanel(ncircleTab, PanelName4);
            RibbonPanel Panel5 = application.CreateRibbonPanel(ncircleTab, PanelName5);
            RibbonPanel Panel6 = application.CreateRibbonPanel(ncircleTab, PanelName6);
            RibbonPanel Panel7_8 = application.CreateRibbonPanel(ncircleTab, PanelName7_8);
            RibbonPanel Panel9 = application.CreateRibbonPanel(ncircleTab, PanelName9);
            RibbonPanel Panel10 = application.CreateRibbonPanel(ncircleTab, PanelName10);
            RibbonPanel Panel11 = application.CreateRibbonPanel(ncircleTab, PanelName11);
            RibbonPanel Panel12 = application.CreateRibbonPanel(ncircleTab, PanelName12);

            //Create Buttons 
            AddButton("cmdFirst", "RetrievalFloorLevel", Task1_RetrievalFloorPlans.thisClassName, Task1_RetrievalFloorPlans.thisAssemblyPath, Panel1);
            AddButton("cmdSecond", "ActivationOfFloorlevel", Task2_ActivateFloorPlan.thisClassName, Task2_ActivateFloorPlan.thisAssemblyPath, Panel2);
            AddButton("cmdThird", "ActivationAndHighlightWalls", Task3_ActivateAndHighlightWalls.thisClassName, Task3_ActivateAndHighlightWalls.thisAssemblyPath, Panel3);
            AddButton("cmdFour", "RetrievalPropsOfSelectedWall", Task4_RetrievalPropsOfWalls.thisClassName, Task4_RetrievalPropsOfWalls.thisAssemblyPath, Panel4);
            AddButton("cmdFive", "ActivateLevelShowRoomList", Task5_ActivateAndShowRoomList.thisClassName, Task5_ActivateAndShowRoomList.thisAssemblyPath, Panel5);
            AddButton("cmdSix", "RetrievalWallsOfSelectedRoom", Task6_RetrievalWallsOfSelectedRoom.thisClassName, Task6_RetrievalWallsOfSelectedRoom.thisAssemblyPath, Panel6);
            AddButton("cmdSeven", "ShowHierarchyAndActivateFloor", Task7AndTask8_ShowHierarchy_ActivatePlans.thisClassName, Task7AndTask8_ShowHierarchy_ActivatePlans.thisAssemblyPath, Panel7_8);
            AddButton("cmdNine", "RetrievalLevelwiseWalls", Task9_RetrievalLevelwiseWalls.thisClassName, Task9_RetrievalLevelwiseWalls.thisAssemblyPath, Panel9);
            AddButton("cmdTen", "RetrievalOpeningOfSelectedWall", Task10_RetrievalOpenings.thisClassName, Task10_RetrievalOpenings.thisAssemblyPath, Panel10);
            AddButton("cmdEleven", "ShowHeirarchyAndElementWithCount", Task11_HeirarchyShowElementCount.thisClassName, Task11_HeirarchyShowElementCount.thisAssemblyPath, Panel11);
            AddButton("cmdTwelve", "HighlightAllElementsClickOnSpecific", Task12_HeirarchyShowHighlightElement.thisClassName, Task12_HeirarchyShowHighlightElement.thisAssemblyPath, Panel12);

            return Result.Succeeded;
        }
        Result IExternalApplication.OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}