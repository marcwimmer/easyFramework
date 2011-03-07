using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{

    public class multistructureDemo : efDialogPage
    {

        #region " Vom Web Form Designer generierter Code "

        //Dieser Aufruf ist fr den Web Form-Designer erforderlich.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {

        }
        protected easyFramework.Frontend.ASP.ComplexObjects.efMultiStructure EfMultiStructure1;

        //HINWEIS: Die folgende Platzhalterdeklaration ist fr den Web Form-Designer erforderlich.
        //Nicht lschen oder verschieben.
        private System.Object designerPlaceholderDeclaration;

        private void Page_Init(System.Object sender, System.EventArgs e)
        {
            //CODEGEN: Dieser Methodenaufruf ist fr den Web Form-Designer erforderlich
            //Verwenden Sie nicht den Code-Editor zur Bearbeitung.
            InitializeComponent();
        }

        #endregion

        private void Page_Load(XmlDocument oXmlRequest)
        {
            // Hier Benutzercode zur Seiteninitialisierung einfgen


            this.EfMultiStructure1.sDefinitionFile = "xmlMultiStructure.xml";
            this.EfMultiStructure1.sTopMostElementValue = oClientInfo.rsLoggedInUser["usr_id"].sValue;
            this.EfMultiStructure1.sTopMostElementName = "any_id0";

            //---------------js, css, title-----------
            sTitle = "Demo Multistructure";
            gAddScriptLink("../js/efStandard.js", true, "Javascript");
            gAddScriptLink("../js/efWindow.js", true, "Javascript");
            gAddScriptLink("../js/efDlgParams.js", true, "Javascript");
            gAddScriptLink("../js/efServerProcess.js", true, "Javascript");
            gAddScriptLink("../js/efTreeview.js", true, "Javascript");
            gAddScriptLink("../js/efDataTable.js", true, "Javascript");
            gAddScriptLink("../js/efTabDialog.js", true, "Javascript");
            gAddScriptLink("../js/efPopupMenue.js", true, "Javascript");
            gAddScriptLink("../js/efMultiStructure.js", true, "Javascript");
            gAddCss("../css/efstyledefault.css", true);
            gAddCss("../css/efstyledialogtable.css", true);
            gAddCss("../css/efstyledatatable.css", true);
            gAddCss("../css/efstyletabdlg.css", true);

        }

    }

}
