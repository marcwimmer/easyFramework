using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Collections;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.Data
{
	//================================================================================
	// Project:      easyFramework
	// Copyright:    Promain Software-Betreuung GmbH
	// Component:    Exceptions.vb
	//--------------------------------------------------------------------------------
	// Purpose:      Individual Exceptions
	//--------------------------------------------------------------------------------
	// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	// Changed:
	//================================================================================
	
	
	
	public class efDataException : efException
	{
		
		public efDataException(string message) : base(message) {
		}
		
	}
	
}
