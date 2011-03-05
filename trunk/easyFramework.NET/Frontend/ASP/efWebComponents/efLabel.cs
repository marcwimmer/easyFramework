using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;

namespace easyFramework.Frontend.ASP.WebComponents
{
	
	[DefaultProperty("Text"), ToolboxData("<{0}:efLabel runat=server></{0}:efLabel>")]public class efLabel : System.Web.UI.WebControls.WebControl
	{
		
		string _text;
		
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string @Text
		{
			get{
				return _text;
			}
			
			set
			{
				_text = value;
			}
		}
		
		protected override void Render (System.Web.UI.HtmlTextWriter output)
		{
			output.Write(@Text);
		}
		
	}
	
}
