/* (c) Promain Software-Betreuung GmbH 2004 */

//l�dt den Treeview neu; optional k�nnen Server-Parameter z.B. <svy_id>2</svy_id> �bergeben werden
function reloadTreeview(sDivId, sOptional_sServerParams, bUseLastServerParams) {

    if (bUseLastServerParams == null) bUseLastServerParams = false;

    var oTreeviewInfo = goGetTreeviewInfo(sDivId);
    var sServerParams = oTreeviewInfo.sServerParams; //either take server-params from initialized object or from parameter;
    var oNodeLastClicked = oTreeviewInfo.oGetNode(oTreeviewInfo.lastClickedNodeId, false);

    if (sOptional_sServerParams != null) {
        if (sOptional_sServerParams.length > 0) {
            sServerParams = sOptional_sServerParams;
        }
    }

    //falls die zuletzt verwendeten server-parameter verwendet werden sollen, hier auswerten
    if (bUseLastServerParams)
        sServerParams = oTreeviewInfo.sLastServerParams;
    else
        oTreeviewInfo.sLastServerParams = sServerParams;


    var sData = gsCallServerMethod(oTreeviewInfo.sDataPage, sServerParams);


    dataToTreeview(sData, sDivId, oTreeviewInfo.lNodeHeight)

    if (oNodeLastClicked != null) {
        gTreeviewSelectNode(sDivId, oNodeLastClicked.id, false);
    }
}

function gTreeviewSelectNode(sDivId, sNodeId, bErrIfNoNode) {
    if (bErrIfNoNode == null) bErrIfNoNode = true;

    var oTreeviewInfo = goGetTreeviewInfo(sDivId);

    var oNode = oTreeviewInfo.oGetNode(sNodeId, bErrIfNoNode);
    if (oNode == null) return;
    var oNodeLastClicked = oTreeviewInfo.oGetNode(oTreeviewInfo.lastClickedNodeId, false);

    if (oNodeLastClicked != null) {
        oNodeLastClicked.selected = false;
        oNodeLastClicked.render();
    }
    oNode.selected = true;
    oNode.render();
    oTreeviewInfo.lastClickedNodeId = sNodeId;
}

//--------------------------------private members-------------------------------
var aoTreeviews = new Array();
var praefix = "treenode_";
var treeview_seperator_treeviewid_nodeid = "-||-|-||-";
var oTransportInnerHtml = new Array();

function goGetTreeviewInfo(sDivId) { //liefert das treeviewinfo-objekt zur�ck, in dem Daten des Treeviews gespeichert sind
    var i = 0;

    for (i = 0; i < aoTreeviews.length; i++) {
        if (aoTreeviews[i].sDivId == sDivId) {
            return aoTreeviews[i];
        }
    }

    alert("treeview with id \"" + sDivId + "\" wasn't found!");
}


function TreeviewInfo(sDivId, sServerParams,
	sImage_I, sImage_R, sImage_L, sImage_LMinus,
	sImage_LPlus, sImage_T, sImage_TMinus, sImage_TPlus,
	sImage_Clear, lNodeHeight, sDataPage, sBackgroundColorSelectedItem) {

    //images, set by function   loadTreeView

    this.sDivId = sDivId;
    this.sServerParams = sServerParams;
    this.lNodeHeight = lNodeHeight;
    this.sTreeNodeImage_IGif = sImage_I;
    this.sTreeNodeImage_Clear = sImage_Clear;
    this.sTreeNodeImage_LGif = sImage_L;
    this.sTreeNodeImage_LMinusGif = sImage_LMinus;
    this.sTreeNodeImage_LPlusGif = sImage_LPlus;
    this.sTreeNodeImage_rGif = sImage_R;
    this.sTreeNodeImage_tGif = sImage_T;
    this.sTreeNodeImage_tMinusGif = sImage_TMinus;
    this.sTreeNodeImage_tPlusGif = sImage_TPlus;
    this.sDataPage = sDataPage;
    this.aoAllMainNodes = new Array(0);
    this.aoAllNodes = new Array(0);
    this.renderNodes = renderNodes;
    this.oGetNode = oGetNode;
    this.sBackgroundColorSelectedItem = sBackgroundColorSelectedItem;


    return this;

}


//the following function is used to set-up the treeview:
function loadTreeView(sDivId, sDataPage, sServerParams,
	sImage_I, sImage_R, sImage_L, sImage_LMinus,
	sImage_LPlus, sImage_T, sImage_TMinus, sImage_TPlus,
	sImage_Clear, lNodeHeight, sBackgroundColorSelectedItem) {


    var treeviewInfo = new TreeviewInfo(sDivId, sServerParams,
		sImage_I, sImage_R, sImage_L, sImage_LMinus,
		sImage_LPlus, sImage_T, sImage_TMinus, sImage_TPlus,
		sImage_Clear, lNodeHeight, sDataPage, sBackgroundColorSelectedItem);

    aoTreeviews.push(treeviewInfo);

    reloadTreeview(sDivId);


}

//this function converts tereview-structure information to the treeview-nodes:
function dataToTreeview(sData, sDivId, lNodeHeight) {
    //renders the given-data to a treeview:

    var sLineEnd = new String("-||-")
    var sColumnSep = new String("|")

    var sLocalData = new String(sData);
    //just do char 13 instead of 13 + 10:
    sLocalData = sLocalData.replace(String.fromCharCode(13), "");
    sLocalData = sLocalData.replace(String.fromCharCode(10), "");

    //check for OK:
    if (sLocalData.substring(0, 6) != "OK-||-") {
        efMsgBox(sLocalData, "ERROR");
        return
    } else {
        sLocalData = sLocalData.substring(6);
    }


    //alle bestehenden Nodes entfernen, clean-up:
    clearNodesOfTreeview(sDivId);


    while (sLocalData.indexOf(sLineEnd) >= 0) {
        var sLine = new String(sLocalData.substr(0, sLocalData.indexOf(sLineEnd)));
        sLocalData = sLocalData.substring(sLocalData.indexOf(sLineEnd) +
			sLineEnd.length, sLocalData.length);

        var bMainItem = false;
        var sParentID;
        var sID;
        var sCaption;
        var sCommand;
        var sIconNormal;
        var sIconOpened;
        var bIsFolder;

        //first column is parent-id:
        sParentID = sLine.substr(0, sLine.indexOf(sColumnSep));
        if (sParentID == "NULL" || sParentID == "null") {
            sParentID = "";
            bMainItem = true;
        } else bMainItem = false;
        sLine = sLine.substring(sLine.indexOf(sColumnSep) + 1, sLine.length);

        //second column is id:
        sID = sLine.substr(0, sLine.indexOf(sColumnSep));
        sLine = sLine.substring(sLine.indexOf(sColumnSep) + 1, sLine.length);

        //third column is caption:
        sCaption = sLine.substr(0, sLine.indexOf(sColumnSep));
        sLine = sLine.substring(sLine.indexOf(sColumnSep) + 1, sLine.length);

        //fourth column is command:
        sCommand = sLine.substr(0, sLine.indexOf(sColumnSep));
        sLine = sLine.substring(sLine.indexOf(sColumnSep) + 1, sLine.length);

        //fifth column is bool - folder:
        bIsFolder = !(sLine.substr(0, sLine.indexOf(sColumnSep)) == "0");
        sLine = sLine.substring(sLine.indexOf(sColumnSep) + 1, sLine.length);

        //sixth column is image normal
        sIconNormal = sLine.substr(0, sLine.indexOf(sColumnSep));
        sLine = sLine.substring(sLine.indexOf(sColumnSep) + 1, sLine.length);

        //seventh column is image opened
        sIconOpened = sLine.substr(0, sLine.indexOf(sColumnSep));
        sLine = sLine.substring(sLine.indexOf(sColumnSep) + 1, sLine.length);


        //create node:
        if (bMainItem == true)
            createMainNode(sDivId, sID, sCaption, sCommand, bIsFolder, sIconOpened, sIconNormal, sDivId);
        else
            appendNode(sParentID, sID, sCaption, sCommand, bIsFolder, sIconOpened, sIconNormal, sDivId);


    }
    var oTreeviewInfo = goGetTreeviewInfo(sDivId);
    goGetTreeviewInfo(sDivId).renderNodes();

}





//functions for generating a treeview:


//folder-object:
//parameters:
//sHtml_id: eindeutige Html-id, z.B. efTreeview1_2606
//sId: ID-Wert, den der Node tr�gt, kann z.B. 2606 sein
function Node(sHtmlId, sId, sText, sCommand, oParentNode, oContainer, bIsFolder,
		sOpenImage, sClosedImage, sDivId, lNodeHeight) { //constructor



    //methods private:
    this.render = nodeRender;
    this.getLevel = nodeGetLevels;
    this.getHeight = nodeLGetHeight;
    this.getPrecedingNodes = nodeGetPrecedingNodes;
    this.getSuccessingNodes = nodeGetSuccessingNodes;
    this.getShallTop = nodeGetShallTop;
    this.areThereSuccessors = nodeAreThereSuccessors;
    this.getParentByLevel = nodeGetParentByLevel;
    this.isAnyParentNotOpened = nodeIsAnyParentNotOpened;
    this.getVisibility = nodeGetVisibility;
    this.sCommand = sCommand;
    this.sHtmlId = sHtmlId;
    this.sDivId = sDivId;
    this.lNodeHeight = lNodeHeight;

    //methods public:

    //properties
    this.bIsFolder = bIsFolder;
    this.bIsOpened = false;
    this.id = praefix + sId;
    this.oParentNode = oParentNode;
    this.oContainer = oContainer; // the div of the node
    this.sText = sText;
    this.sOpenImage = sOpenImage;
    this.sClosedImage = sClosedImage;
    this.children = new Array;


    //the belonging div-element:
    this.oDiv = document.createElement("div");
    this.oDiv.style.position = "absolute";
    this.oDiv.style.top = 30;
    this.oDiv.style.left = 0;
    this.oDiv.style.height = lNodeHeight + 2;
    this.oContainer.appendChild(this.oDiv);

    return this;
}

function nodeGetLevels() {

    if (this.oParentNode == null)
        return 1;
    else
        return this.oParentNode.getLevel() + 1;
}

function nodeRender() {

    var trPraefix = "treenodeRow_";

    var bShowItems;
    if (this.oParentNode != null)
        bShowItems = !this.isAnyParentNotOpened();
    else bShowItems = true;

    var sInnerHtml = "";
    var oTreeviewInfo = goGetTreeviewInfo(this.sDivId);

    if (bShowItems == false) {

    }
    else {


        var sBackGroundColor = "";

        if (this.selected)
            sBackGroundColor = oTreeviewInfo.sBackgroundColorSelectedItem;

        sInnerHtml = "";
        sInnerHtml = sInnerHtml + "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">" +
				"<tr style=\"cursor: pointer; cursor:hand\" id=\"" + this.sHtmlId + "\" onclick=\"onNodeClick('" + this.sHtmlId + "');\">";


        //onNodeClick(" this.sHtmlId + "_click()


        var lLevel = this.getLevel();
        for (i = 0; i < lLevel - 1; i++) {

            var oParentByLevel = this.getParentByLevel(i + 1);
            if (oParentByLevel != null) {
                if (oParentByLevel.areThereSuccessors() == true)
                    sInnerHtml += "<td width=\"1\" align=\"left\" valign=\"top\" class=\"dlgField\"><img src=\"" + oTreeviewInfo.sTreeNodeImage_IGif + "\"></td>";
                else
                    sInnerHtml += "<td width=\"1\" align=\"left\" valign=\"top\"  class=\"dlgField\"><img src=\"" + oTreeviewInfo.sTreeNodeImage_Clear + "\"></td>";
            }
        }

        if (lLevel >= 2) {
            sInnerHtml += "<td width=\"1\"  align=\"left\" valign=\"top\" class=\"dlgField\"><img src=\"";


            //decide wether to take the L or t -element:
            var bTakeT = true; //if not t-element, then the L-element

            //when to take the t-element? if there is at least on successor, then
            //take the t, else the l:
            if (this.areThereSuccessors() == true)
                bTakeT = true;
            else
                bTakeT = false;


            if (this.bIsFolder == true) {
                if (this.bIsOpened == true) {
                    if (bTakeT == true) {
                        sInnerHtml += oTreeviewInfo.sTreeNodeImage_tMinusGif;
                    }
                    else sInnerHtml += oTreeviewInfo.sTreeNodeImage_LMinusGif;
                }
                else {
                    if (bTakeT == true) {
                        sInnerHtml += oTreeviewInfo.sTreeNodeImage_tPlusGif;
                    }
                    else sInnerHtml += oTreeviewInfo.sTreeNodeImage_LPlusGif;
                }
            }
            else {
                if (bTakeT == true)
                    sInnerHtml += oTreeviewInfo.sTreeNodeImage_tGif;
                else
                    sInnerHtml += oTreeviewInfo.sTreeNodeImage_LGif;

            }

            sInnerHtml += "\"></td>";
        }

        //icon:
        var sImageIcon;
        if (this.bIsOpened)
            sImageIcon = this.sOpenImage;
        else sImageIcon = this.sClosedImage;

        sInnerHtml += "<td class=\"dlgField\"><img src=\"" +
				sImageIcon + "\"></td>"

        //text:		
        sInnerHtml += "<td nowrap=\"true\" class=\"dlgField\" style=\"background-color:" + sBackGroundColor + ";\">" + this.sText + "</td>";

        sInnerHtml += "</td></tr></table>";

    }
    sInnerHtml += "<td nowrap=\"true\" class=\"dlgField\">";
    //setting the top-value:
    this.oDiv.style.top = this.getShallTop() - oTreeviewInfo.lNodeHeight;


    //display the html:			
    //sInnerHtml = sInnerHtml.replace ("'", "''");
    //ein Array benutzen, das den Html-Code enth�lt; kein Problem mit Apostrophen dann
    oTransportInnerHtml.push(sInnerHtml);

    var sCommand = new String("goGetTreeviewInfo('$1').oGetNode('$2').oDiv.innerHTML = oTransportInnerHtml[$3];");
    sCommand = sCommand.replace("$1", this.sDivId);
    sCommand = sCommand.replace("$2", this.id);
    sCommand = sCommand.replace("$3", oTransportInnerHtml.length - 1);

    window.setTimeout(sCommand, 0);

    //render the children:
    var i = 0;

    if (this.children.length > 0)
        for (i = 0; i < this.children.length; i++) {
            this.children[i].render();
        }
}

function nodeIsAnyParentNotOpened() {
    //returns true, if there is any parent, that is not opened:


    if (this.oParentNode != null) {
        if (this.oParentNode.bIsOpened == false)
            return true;
        else
            return this.oParentNode.isAnyParentNotOpened();
    }
    else {
        if (this.bIsOpened == false)
            return true;
        else
            return false;
    }
}

function nodeGetParentByLevel(lLevel) {
    //retrieves the parent-node by the given level:
    if (this.getLevel() == lLevel) {
        return this;
    }
    else {
        if (this.oParentNode != null) {
            return this.oParentNode.getParentByLevel(lLevel);
        }
    }
}

function nodeGetShallTop() {
    //returns the top-value, this element SHOULD have:

    var aoGetPrecedingNodes = this.getPrecedingNodes();
    var lTop = 0;
    if (this.oParentNode != null) {
        lTop = this.oParentNode.getShallTop()

    }

    for (i = 0; i < aoGetPrecedingNodes.length; i++) {
        lTop += aoGetPrecedingNodes[i].getHeight();
    }

    lTop += this.lNodeHeight;


    return lTop;

}

function nodeGetPrecedingNodes() {
    //returns all preceding nodes of a node:
    var oParent = this.oParentNode;
    var oTreeviewInfo = goGetTreeviewInfo(this.sDivId);

    if (oParent == null) { //we are a top-element; the preceding elements are the other top-elements before
        var result = new Array();
        for (i = 0; i < oTreeviewInfo.aoAllMainNodes.length; i++) {
            if (oTreeviewInfo.aoAllMainNodes[i].id != this.id) {
                result.push(oTreeviewInfo.aoAllMainNodes[i]);
            }
            else break;
        }

        return result;
    }
    else { //we are a sub-element; go to the parent and get all preceding children:
        var result = new Array();
        for (i = 0; i < oParent.children.length; i++) {
            if (oParent.children[i].id != this.id) {
                result.push(oParent.children[i]);
            }
            else break;
        }
        return result;

    }

}

function nodeGetSuccessingNodes() {
    //returns all successing nodes of a node:
    var oParent = this.oParentNode;
    var oTreeviewInfo = goGetTreeviewInfo(this.sDivId);

    if (oParent == null) { //we are a top-element; the preceding elements are the other top-elements before
        var result = new Array();
        var bPassedMySelf = false;

        for (i = 0; i < oTreeviewInfo.aoAllMainNodes.length; i++) {

            if (bPassedMySelf == true)
                result.push(oTreeviewInfo.aoAllMainNodes[i]);

            if (oTreeviewInfo.aoAllMainNodes[i].id == this.id)
                bPassedMySelf = true;

        }

        return result;
    }
    else { //we are a sub-element; go to the parent and get all preceding children:
        var result = new Array();
        var bPassedMySelf = false;

        for (i = 0; i < oParent.children.length; i++) {


            if (bPassedMySelf == true)
                result.push(oParent.children[i]);

            if (oParent.children[i].id == this.id)
                bPassedMySelf = true;



        }
        return result;

    }

}

function nodeAreThereSuccessors() {

    //return bool, if there are successing elements:
    var oParent = this.oParentNode;
    var oTreeviewInfo = goGetTreeviewInfo(this.sDivId);

    if (oParent == null) { //we are a top-element; the preceding elements are the other top-elements before
        var result = new Array();
        for (i = 0; i < oTreeviewInfo.aoAllMainNodes.length; i++) {
            if (oTreeviewInfo.aoAllMainNodes[i].id == this.id) {
                if (i < oTreeviewInfo.aoAllMainNodes.length - 1)
                    return true;
                else
                    return false;
            }
        }

        return false;
    }
    else { //we are a sub-element; go to the parent and get all preceding children:
        var result = new Array();
        for (i = 0; i < oParent.children.length; i++) {
            if (oParent.children[i].id == this.id) {
                if (i < oParent.children.length - 1)
                    return true;
                else {
                    return false;
                }
            }
        }
        return false;

    }

}

function nodeGetVisibility() {
    //determines, wether the current node should be visible or not

    if (this.isAnyParentNotOpened() == true)
        return false;

}

function nodeLGetHeight() {
    //liefert die H�he des Elements inklusive der Children zur�ck:

    if (this.bIsOpened == false)
        return this.lNodeHeight;

    var lHeight = this.lNodeHeight;
    var i = 0;
    for (i = 0; i < this.children.length; i++) {
        lHeight += this.children[i].getHeight();

    }

    return lHeight;

}


function createMainNode(sParentDiv, sId, sText, sCommand, bIsFolder, sOpenImage, sClosedImage, sDivId) {

    var sHtmlId = sParentDiv + treeview_seperator_treeviewid_nodeid + sId;
    var oTreeviewInfo = goGetTreeviewInfo(sDivId);


    if (oTreeviewInfo.oGetNode(sHtmlId, false) != null) {
        alert("element with id " + sHtmlId + " already exists!");
        return
    }
    var oTreeviewInfo = goGetTreeviewInfo(sDivId);


    var n = new Node(sHtmlId, sId, sText, sCommand, null, document.getElementById(sParentDiv), bIsFolder, sOpenImage, sClosedImage, sDivId, oTreeviewInfo.lNodeHeight)
    //add into main-nodes:
    oTreeviewInfo.aoAllMainNodes.push(n);
    oTreeviewInfo.aoAllNodes.push(n);

    return n;
}

function appendNode(sParentId, sId, sText, sCommand, bIsFolder, sOpenImage, sClosedImage, sDivId) {

    //if you want to have more than one treeview on a web-page, you need the treeview-id within the id:
    var sHtmlId = sDivId + treeview_seperator_treeviewid_nodeid + sId;
    var oTreeviewInfo = goGetTreeviewInfo(sDivId);

    if (oTreeviewInfo.oGetNode(sHtmlId, false) != null) {
        alert("element with id " + sHtmlId + " already exists!");
        return
    }

    var oParentNode = oTreeviewInfo.oGetNode(sParentId);
    var oTreeviewInfo = goGetTreeviewInfo(sDivId);
    var n = new Node(sHtmlId, sId, sText, sCommand, oParentNode, oParentNode.oContainer, bIsFolder, sOpenImage, sClosedImage, sDivId, oTreeviewInfo.lNodeHeight)

    //add to children list:
    oTreeviewInfo.aoAllNodes.push(n);
    oParentNode.children.push(n);

    return n;
}

function oGetNode(sId, bErrorIfNotFound) {
    //returns a node from the node-element-array

    var i;

    var sLocalID = String(sId);
    if (sLocalID.substring(0, praefix.length) != praefix)
        sLocalID = praefix + sLocalID;

    for (i = 0; i < this.aoAllNodes.length; i++) {
        if (this.aoAllNodes[i].id == sLocalID) {

            return this.aoAllNodes[i]
        }
    }
    if (bErrorIfNotFound != false)
        alert("error getting node: " + sId);
}

function renderNodes() {

    var i = 0;
    var oTreeviewInfo = goGetTreeviewInfo(this.sDivId);

    for (i = 0; i < oTreeviewInfo.aoAllMainNodes.length; i++) {

        oTreeviewInfo.aoAllMainNodes[i].render();

    }

}

function onNodeClick(sHtmlId) {

    var sDivId = sHtmlId.split(treeview_seperator_treeviewid_nodeid)[0];
    var sNodeId = sHtmlId.split(treeview_seperator_treeviewid_nodeid)[1];
    var oTreeviewInfo = goGetTreeviewInfo(sDivId);

    //unselect the last clicked node:
    if (oTreeviewInfo.lastClickedNodeId != null) {
        var oNodeLastClicked = oTreeviewInfo.oGetNode(oTreeviewInfo.lastClickedNodeId, false);
        if (oNodeLastClicked != null) {
            oNodeLastClicked.selected = false;
            oNodeLastClicked.render();
        }
    }

    var oNode = oTreeviewInfo.oGetNode(sNodeId);
    oNode.bIsOpened = !oNode.bIsOpened;
    oNode.selected = true;
    oTreeviewInfo.lastClickedNodeId = sNodeId;

    //optimization: do not render all nodes; just the current node,
    //all children and all the nodes, which lie on the screen UNDER
    //the current node:

    oNode.render();

    var i = 0;
    for (i = oNode.getLevel(); i >= 1; i--) {
        var underlieingNode = oNode.getParentByLevel(i);
        var oSuccessors = underlieingNode.getSuccessingNodes();

        for (y = 0; y < oSuccessors.length; y++) {
            oSuccessors[y].render();

        }
    }


    if (oNode.sCommand != "") {
        if (oNode.sCommand != null) {
            eval(oNode.sCommand);
        }
    }

}


function gMakeNodeVisible(sNodeId) {
    //searches the nodes to find the given id; then all upper nodes are expanded, so that
    //this node is visible

    var oNode = oGetNode(sNodeId);

    mExpandParentNode(oNode);

    renderNodes();

}

function mExpandParentNode(oCurrentNode) {

    oCurrentNode.bIsOpened = true;

    if (oCurrentNode.oParentNode != null) {
        mExpandParentNode(oCurrentNode.oParentNode);
    }

}

//--------------r�umt einen Treeview auf, entfernt alle nodes und gespeicherten Html-Codes--------
function clearNodesOfTreeview(sDivId) {

    var oTreeviewInfo = goGetTreeviewInfo(sDivId);

    oTreeviewInfo.aoAllMainNodes = null;
    oTreeviewInfo.aoAllNodes = null;

    oTreeviewInfo.aoAllMainNodes = new Array(0);
    oTreeviewInfo.aoAllNodes = new Array(0);

    document.getElementById(sDivId).innerHTML = "";
}
	
	
	
	
	
	
	
	
	
	