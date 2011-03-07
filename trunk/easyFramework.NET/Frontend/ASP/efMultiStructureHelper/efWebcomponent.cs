using System;
using System.ComponentModel;
using System.Web.UI;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Sys;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Frontend.ASP.WebComponents;

namespace easyFramework.Frontend.ASP.ComplexObjects
{
    //================================================================================
    //Class:     efMultiStructure

    //--------------------------------------------------------------------------------'
    //Module:    efMultiStructure.vb
    //--------------------------------------------------------------------------------'
    //Copyright: Promain Software-Betreuung GmbH
    //--------------------------------------------------------------------------------'
    //Purpose:   for structuring several xml-dialogs;
    //           with plus/minus-button to show and hide
    //           with add-new button
    //--------------------------------------------------------------------------------'
    //Created:   02.05.2004 18:28:00
    //--------------------------------------------------------------------------------'
    //Changed:
    //--------------------------------------------------------------------------------'



    //================================================================================
    //Imports:
    //================================================================================


    [DefaultProperty("Text"), ToolboxData("<{0}:efMultiStructure runat=server></{0}:efMultiStructure>")]
    public class efMultiStructure : efBaseElement
    {
        public efMultiStructure()
        {
            msFormName = "";
            msStoreDataPage = "/ASP/system/multistructure/storing/storeDialogData.aspx";
            msRenderAllLevelsPage = "/ASP/system/multistructure/ondemandrender/renderAllLevelForReload.aspx";
        }



        //================================================================================
        //Private Fields:
        //================================================================================
        private string msDefinitionFile;
        private string msFormName;

        private string msTopMostElementName;
        private string msTopMostElementValue;
        private string msStoreDataPage;
        private string msRenderAllLevelsPage;
        private string msOnAfterAskToSave;


        //================================================================================
        //Public Properties:
        //================================================================================

        //================================================================================
        //Property:  sRenderAllLevelsPage
        //--------------------------------------------------------------------------------'
        //Purpose:   the page, which renders the html of several levels at once
        //--------------------------------------------------------------------------------'
        //Params:
        //--------------------------------------------------------------------------------'
        //Created:   16.06.2004 14:23:37
        //--------------------------------------------------------------------------------'
        //Changed:
        //--------------------------------------------------------------------------------'
        public string sRenderAllLevelsPage
        {
            get
            {
                return msRenderAllLevelsPage;
            }
            set
            {
                msRenderAllLevelsPage = value;
            }
        }
        //================================================================================
        //Property:  sOnAfterAskToSave
        //--------------------------------------------------------------------------------'
        //Purpose:   raised, if the dialog is dirty, and the user jumps from one page
        //           to another; if the user answers "Do you want to save changes" with
        //           yes, then this event is raised
        //--------------------------------------------------------------------------------'
        //Params:
        //--------------------------------------------------------------------------------'
        //Created:   16.06.2004 13:30:24
        //--------------------------------------------------------------------------------'
        //Changed:
        //--------------------------------------------------------------------------------'
        public string sOnAfterAskToSave
        {
            get
            {
                return msOnAfterAskToSave;
            }
            set
            {
                msOnAfterAskToSave = value;
            }
        }

        //================================================================================
        //Property:  sDataPage
        //--------------------------------------------------------------------------------'
        //Purpose:   the data-page, which delievers the content for the dialog
        //--------------------------------------------------------------------------------'
        //Params:
        //--------------------------------------------------------------------------------'
        //Created:   05.05.2004 09:42:46
        //--------------------------------------------------------------------------------'
        //Changed:
        //--------------------------------------------------------------------------------'
        public string sStoreDataPage
        {
            get
            {
                return msStoreDataPage;
            }
            set
            {
                msStoreDataPage = value;
            }
        }

        //================================================================================
        //Property:  sTopMostElementName
        //--------------------------------------------------------------------------------'
        //Purpose:   the multi-structure xml references at the top-most level a root-parent-id
        //           the name of this variable can be set here;
        //           if you have a survey for example, which has several sub-questions,
        //           then you can name the variable, which has the survey-id, "svy_id" and
        //           set the value by the property sTopMostElementValue
        //--------------------------------------------------------------------------------'
        //Params:
        //--------------------------------------------------------------------------------'
        //Created:   04.05.2004 12:41:15
        //--------------------------------------------------------------------------------'
        //Changed:
        //--------------------------------------------------------------------------------'
        public string sTopMostElementName
        {
            get
            {
                return msTopMostElementName;
            }
            set
            {
                msTopMostElementName = value;
            }
        }


        //================================================================================
        //Property:  sTopMostElementValue
        //--------------------------------------------------------------------------------'
        //Purpose:   look at the description of sTopMostElementName
        //--------------------------------------------------------------------------------'
        //Params:
        //--------------------------------------------------------------------------------'
        //Created:   04.05.2004 12:43:30
        //--------------------------------------------------------------------------------'
        //Changed:
        //--------------------------------------------------------------------------------'
        public string sTopMostElementValue
        {
            get
            {
                return msTopMostElementValue;
            }
            set
            {
                msTopMostElementValue = value;
            }
        }




        //================================================================================
        //Property:  sDefinitionFile
        //--------------------------------------------------------------------------------'
        //Purpose:   the XML content of the dialog
        //--------------------------------------------------------------------------------'
        //Params:
        //--------------------------------------------------------------------------------'
        //Created:   22.03.2004 15:57:28
        //--------------------------------------------------------------------------------'
        //Changed:
        //--------------------------------------------------------------------------------'
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string sDefinitionFile
        {
            get
            {
                return msDefinitionFile;
            }

            set
            {

                msDefinitionFile = value;
            }
        }

        //================================================================================
        //Property:  sFormName
        //--------------------------------------------------------------------------------'
        //Purpose:   the name of the parent form
        //--------------------------------------------------------------------------------'
        //Params:
        //--------------------------------------------------------------------------------'
        //Created:   22.03.2004 15:57:28
        //--------------------------------------------------------------------------------'
        //Changed:
        //--------------------------------------------------------------------------------'
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string sFormName
        {
            get
            {
                return msFormName;
            }

            set
            {

                msFormName = value;
            }
        }

        //================================================================================
        //Public Methods:
        //================================================================================

        protected override void Render(System.Web.UI.HtmlTextWriter output)
        {

            if (msFormName == "")
            {
                throw (new efException("Form-Name required!"));
            }
            //----------------------create the parent div per javascript and append it as a child to the form----------------
            //........the div is not rendered directly. if this was so, then it had to be, that the form is rendered entirely
            //before. by this way, nevertheless, the form doesn't need to exist.
            efHtmlScript oScriptParentDiv = new efHtmlScript();
            string sVarName = ASPTools.Tools.sGetUniqueString();
            FastString oSb = new FastString();

            string sLeft;
            string sTopValue;
            string sWidth;
            string sHeight;
            sLeft = this.Left.ToString();

            if (sLeft == "")
            {
                sLeft = "0";
            }
            sTopValue = this.Top.ToString();

            if (sTopValue == "")
            {
                sTopValue = "0";
            }
            sWidth = this.Width.ToString();

            if (sWidth == "")
            {
                sWidth = "0";
            }
            sHeight = this.Height.ToString();

            if (sHeight == "")
            {
                sHeight = "0";
            }

            oScriptParentDiv.sCode = "var " + sVarName + " = document.createElement(\"div\");" + "\n" + sVarName + ".setAttribute(\"id\", \"" + this.ID + "\");" + "\n" + sVarName + ".style.position = \"absolute\";" + "\n" + sVarName + ".style.top = \"" + sTopValue + "\";" + "\n" + sVarName + ".style.left = \"" + sLeft + "\";" + "\n" + sVarName + ".style.width = \"" + sWidth + "\";" + "\n" + sVarName + ".style.height = \"" + sHeight + "\";" + "\n" + sVarName + ".style.overflow = \"" + efHtmlConsts.Overflow(efEnumOverflow.efVisible) + "\";" + "\n" + "document.forms['" + msFormName + "'].appendChild(" + sVarName + ");";
            oScriptParentDiv.gRender(oSb, 1);


            //---------------load xml-definition-------------------
            XmlDocument oXmlDefinition = new XmlDocument();
            oXmlDefinition.gLoad(ASPTools.Tools.sWebToAbsoluteFilename(Parent.Page.Request, this.sDefinitionFile, false));


            //---------------check data-page property-------------------
            if (msStoreDataPage == "")
            {
                throw (new efException("Please set the sDataPage-property of the multistructure-component!"));
            }



            //--------------render init of multi-struct-------------------
            MultistructureRenderer oMultiStructRenderer = new MultistructureRenderer();
            oSb.Append(oMultiStructRenderer.gsRenderInit(oXmlDefinition, ID, msFormName, ASPTools.Tools.sWebToAbsoluteFilename(Parent.Page.Request, sDefinitionFile, true), msTopMostElementName, msTopMostElementValue, ASPTools.Tools.sWebToAbsoluteFilename(Parent.Page.Request, msStoreDataPage, true), msOnAfterAskToSave, ASPTools.Tools.sWebToAbsoluteFilename(Parent.Page.Request, msRenderAllLevelsPage, true)));


            output.Write(oSb.ToString(), 1);

        }

    }

}
