using System;
using System.Runtime.InteropServices;

namespace FontTester
{
	/**
	 * direct access to win32 api methods
	 */
	public unsafe class Win32
	{
		public const uint GDI_ERROR = 0xffffffff;

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct ABC
		{
			public int     abcA; 
			public uint    abcB; 
			public int     abcC; 
		}

		/**
		 * The ABCFLOAT structure contains the A, B, and C widths 
		 * of a font character. 
		 */
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct ABCFLOAT 
		{
			public float   abcfA; 
			public float   abcfB; 
			public float   abcfC; 
		} 


		/**
		 * The RECT structure defines the coordinates of the upper-left 
		 * and lower-right corners of a rectangle. 
		 */
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
			public struct RECT 
		{
			public int left; 
			public int top; 
			public int right; 
			public int bottom; 
		}

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
			public struct LOGFONT 
		{ 
			public const int LF_FACESIZE = 32;
			public int lfHeight; 
			public int lfWidth; 
			public int lfEscapement; 
			public int lfOrientation; 
			public int lfWeight; 
			public byte lfItalic; 
			public byte lfUnderline; 
			public byte lfStrikeOut; 
			public byte lfCharSet; 
			public byte lfOutPrecision; 
			public byte lfClipPrecision; 
			public byte lfQuality; 
			public byte lfPitchAndFamily;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=LF_FACESIZE)]
			public string lfFaceName; 
		}

		public const byte  OUT_DEFAULT_PRECIS =         0;
		public const byte  OUT_STRING_PRECIS  =         1;
		public const byte  OUT_CHARACTER_PRECIS =       2;
		public const byte  OUT_STROKE_PRECIS =          3;
		public const byte  OUT_TT_PRECIS  =             4;
		public const byte  OUT_DEVICE_PRECIS   =        5;
		public const byte  OUT_RASTER_PRECIS  =         6;
		public const byte  OUT_TT_ONLY_PRECIS  =        7;
		public const byte  OUT_OUTLINE_PRECIS =         8;
		public const byte  OUT_SCREEN_OUTLINE_PRECIS =  9;
		public const byte  OUT_PS_ONLY_PRECIS  =        10;

		public const byte  CLIP_DEFAULT_PRECIS =    0;
		public const byte  CLIP_CHARACTER_PRECIS  = 1;
		public const byte  CLIP_STROKE_PRECIS    =  2;
		public const byte  CLIP_MASK    =           0xf;
		public const byte  CLIP_LH_ANGLES       =   (1<<4);
		public const byte  CLIP_TT_ALWAYS        =  (2<<4);
		public const byte  CLIP_EMBEDDED        =   (8<<4);

		public const byte  DEFAULT_QUALITY    =     0;
		public const byte  DRAFT_QUALITY        =   1;
		public const byte  PROOF_QUALITY         =  2;

		public const byte  NONANTIALIASED_QUALITY = 3;
		public const byte  ANTIALIASED_QUALITY  =   4;
		public const byte  CLEARTYPE_QUALITY   =    5;
		public const byte  DEFAULT_PITCH =          0;
		public const byte  FIXED_PITCH   =          1;
		public const byte  VARIABLE_PITCH   =       2;
		public const byte  MONO_FONT     =          8;
		public const byte  ANSI_CHARSET    =        0;
		public const byte  DEFAULT_CHARSET   =      1;
		public const byte  SYMBOL_CHARSET =         2;
		public const byte  SHIFTJIS_CHARSET  =      128;
		public const byte  HANGEUL_CHARSET =        129;
		public const byte  HANGUL_CHARSET   =       129;
		public const byte  GB2312_CHARSET =         134;
		public const byte  CHINESEBIG5_CHARSET  =   136;
		public const byte  OEM_CHARSET     =        255;
		public const byte  JOHAB_CHARSET   =        130;
		public const byte  HEBREW_CHARSET    =      177;
		public const byte  ARABIC_CHARSET =         178;
		public const byte  GREEK_CHARSET =          161;
		public const byte  TURKISH_CHARSET    =     162;
		public const byte  VIETNAMESE_CHARSET =     163;
		public const byte  THAI_CHARSET   =         222;
		public const byte  EASTEUROPE_CHARSET  =    238;
		public const byte  RUSSIAN_CHARSET  =       204;
		public const byte  MAC_CHARSET  =           77;
		public const byte  BALTIC_CHARSET    =      186;

		public const uint  FS_LATIN1  =             0x00000001;
		public const uint  FS_LATIN2  =             0x00000002;
		public const uint  FS_CYRILLIC  =           0x00000004;
		public const uint  FS_GREEK  =              0x00000008;
		public const uint  FS_TURKISH   =           0x00000010;
		public const uint  FS_HEBREW =              0x00000020;
		public const uint  FS_ARABIC  =             0x00000040;
		public const uint  FS_BALTIC   =            0x00000080;
		public const uint  FS_VIETNAMESE  =         0x00000100;
		public const uint  FS_THAI       =          0x00010000;
		public const uint  FS_JISJAPAN    =         0x00020000;
		public const uint  FS_CHINESESIMP   =       0x00040000;
		public const uint  FS_WANSUNG      =        0x00080000;
		public const uint  FS_CHINESETRAD   =       0x00100000;
		public const uint  FS_JOHAB     =           0x00200000;
		public const uint  FS_SYMBOL     =          0x80000000;

		/* Font Families */
		public const uint  FF_DONTCARE  =       (0<<4);  /* Don't care or don't know. */
		public const uint  FF_ROMAN        =    (1<<4);  /* Variable stroke width, serifed. */
		/* Times Roman, Century Schoolbook, etc. */
		public const uint  FF_SWISS   =         (2<<4);  /* Variable stroke width, sans-serifed. */
		/* Helvetica, Swiss, etc. */
		public const uint  FF_MODERN       =    (3<<4);  /* Constant stroke width, serifed or sans-serifed. */
		/* Pica, Elite, Courier, etc. */
		public const uint  FF_SCRIPT     =      (4<<4);  /* Cursive, etc. */
		public const uint  FF_DECORATIVE   =    (5<<4);  /* Old English, etc. */

		/* Font Weights */
		public const uint  FW_DONTCARE    =     0;
		public const uint  FW_THIN        =     100;
		public const uint  FW_EXTRALIGHT =      200;
		public const uint  FW_LIGHT   =         300;
		public const uint  FW_NORMAL =          400;
		public const uint  FW_MEDIUM =          500;
		public const uint  FW_SEMIBOLD  =       600;
		public const uint  FW_BOLD  =           700;
		public const uint  FW_EXTRABOLD  =      800;
		public const uint  FW_HEAVY =           900;

		public const uint  FW_ULTRALIGHT  =     FW_EXTRALIGHT;
		public const uint  FW_REGULAR=          FW_NORMAL;
		public const uint  FW_DEMIBOLD   =      FW_SEMIBOLD;
		public const uint  FW_ULTRABOLD  =      FW_EXTRABOLD;
		public const uint  FW_BLACK  =          FW_HEAVY;


		/**
		 * The POINT structure defines the x- and y- coordinates of a point. 
		 */
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
			public struct POINT 
		{
			public int x; 
			public int y;
		}

		/**
		 * The SIZE structure specifies the width and height of a rectangle. 
		 */
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
			public struct SIZE
		{
			public int cx; 
			public int cy; 
		}

		/**
		 * The GLYPHMETRICS structure contains information about the placement 
		 * and orientation of a glyph in a character cell. 
		 */
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
			public struct GLYPHMETRICS 
		{
			public uint  gmBlackBoxX; 
			public uint  gmBlackBoxY; 
			public POINT gmptGlyphOrigin; 
			public short gmCellIncX; 
			public short gmCellIncY; 
		};

		/**
		 * layed out correctly, fract is before val, even though
		 * the header file has them the other way. tested sep 22 2003, 
		 * and verified the order of the fields. If the fields are 
		 * reversed, GetGlyphOutline fails
		 */
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct FIXED
		{
			public ushort fract;
			public short  val;	
		}

		/**
		 * The MAT2 structure contains the values for a transformation matrix 
		 * used by the GetGlyphOutline function. 
		 */
		[StructLayout(LayoutKind.Sequential)]
		public struct MAT2 
		{
			public FIXED eM11; 
			public FIXED eM12; 
			public FIXED eM21; 
			public FIXED eM22; 
		} 

		/**
		 * The TEXTMETRIC structure contains basic information about a physical font. 
		 * All sizes are specified in logical units; that is, they depend on the 
		 * current mapping mode of the display context. 
		 */
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
			public struct TEXTMETRIC 
		{ 
			public int tmHeight; 
			public int tmAscent; 
			public int tmDescent; 
			public int tmInternalLeading; 
			public int tmExternalLeading; 
			public int tmAveCharWidth; 
			public int tmMaxCharWidth; 
			public int tmWeight; 
			public int tmOverhang; 
			public int tmDigitizedAspectX; 
			public int tmDigitizedAspectY; 
			public char tmFirstChar; 
			public char tmLastChar; 
			public char tmDefaultChar; 
			public char tmBreakChar; 
			public byte tmItalic; 
			public byte tmUnderlined; 
			public byte tmStruckOut; 
			public byte tmPitchAndFamily; 
			public byte tmCharSet; 
		}

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct CHOOSEFONT
		{
			public uint		lStructSize;
			public IntPtr	hwndOwner;          // caller's window handle
			public IntPtr	hDC;                // printer DC/IC or NULL
			public IntPtr	lpLogFont;          // ptr. to a LOGFONT struct
			public int		iPointSize;         // 10 * size in points of selected font
			public uint		Flags;              // enum. type flags
			public uint		rgbColors;          // returned text color
			public uint		lCustData;          // data passed to hook fn.
			public IntPtr	lpfnHook;           // ptr. to hook function
			public string	lpTemplateName;     // custom template name
			public uint		hInstance;          // instance handle of.EXE that
			//   contains cust. dlg. template
			public string	lpszStyle;          // return the style field here
			// must be LF_FACESIZE or bigger
			public ushort	nFontType;          // same value reported to the EnumFonts
			//   call back with the extra FONTTYPE_
			//   bits added
			public ushort	___MISSING_ALIGNMENT__;
			public int		nSizeMin;           // minimum pt size allowed &
			public int		nSizeMax;           // max pt size allowed if
			//   CF_LIMITSIZE is used
		} 


		/**
		 * The WCRANGE structure specifies a range of Unicode characters.
		 */
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct WCRANGE 
		{
			public char  wcLow;
			public short cGlyphs;
		}

		/**
		 * The GLYPHSET structure contains information about a range of 
		 * Unicode code points.
		 */
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct GLYPHSET 
		{
			public uint		cbThis;
			public uint		flAccel;
			public uint		cGlyphsSupported;
			public uint		cRanges;
			//actually should be WCRANGE[1], but since this is inline
			//this will work too, and is simpler
			public WCRANGE*  ranges; 
		}




		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern int GetCharABCWidths(
			IntPtr hdc,      // handle to DC
			uint uFirstChar, // first character in range
			uint uLastChar,  // last character in range
			[In,Out] ABC[] lpabc      // array of character widths
			);

		/**
		 * The CreateFontIndirect function creates a logical font that has the 
		 * specified characteristics. The font can subsequently be selected as 
		 * the current font for any device context. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr CreateFontIndirect(ref LOGFONT lplf);

		/**
		 * The GetGlyphOutline function retrieves the outline or bitmap for a 
		 * character in the TrueType font that is selected into the specified 
		 * device context. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern uint GetGlyphOutline(
			IntPtr hdc,						// handle to DC
			uint uChar,						// character to query
			uint uFormat,					// data format
			ref GLYPHMETRICS lpgm,		// glyph metrics
			uint cbBuffer,					// size of data buffer
			IntPtr lpvBuffer,				// data buffer
			ref MAT2 lpmat2			// transformation matrix
			);

		
		/**
		 * Windows 2000/XP: The function retrieves the curve data as a cubic 
		 * Bézier spline (not in quadratic spline format). 
		 */
		public const uint GGO_BEZIER = 3; 
		 
		/**
		 * The function retrieves the glyph bitmap. For information about memory 
		 * allocation, see the following Remarks section. 
		 */
		public const uint GGO_BITMAP = 1;	
		
		/**
		 * Windows 95/98/Me, Windows NT 4.0 and later: Indicates that the uChar 
		 * parameter is a TrueType Glyph Index rather than a character code. 
		 * See the ExtTextOut function for additional remarks on Glyph Indexing. 		
		 */																												   
		public const uint GGO_GLYPH_INDEX = 0x0080;
		
		/**
		 * Windows 95/98/Me, Windows NT 4.0 and later: The function 
		 * retrieves a glyph bitmap that contains five levels of gray. 	
		 */																																																																																		  
		public const uint GGO_GRAY2_BITMAP = 4;
		
		/**
		 * Windows 95/98/Me, Windows NT 4.0 and later: The function retrieves a 
		 * glyph bitmap that contains 17 levels of gray. 
		 */
		public const uint GGO_GRAY4_BITMAP = 5;
		
		/**
		 * Windows 95/98/Me, Windows NT 4.0 and later: The function retrieves a glyph 
		 * bitmap that contains 65 levels of gray. 	
		 */																															  
		public const uint GGO_GRAY8_BITMAP = 6;
		
		/**
		 * The function only retrieves the GLYPHMETRICS structure specified by lpgm. 
		 * The other buffers are ignored. This value affects the meaning of the function's 
		 * return value upon failure; see the Return Values section. 
		 */																																																																
		public const uint GGO_METRICS = 0;
		
		/**
		 * The function retrieves the curve data points in the rasterizer's native 
		 * format and uses the font's design units.  
		 */
		public const uint GGO_NATIVE = 2;
		
		/** 
		 * Windows 2000/XP: The function only returns unhinted outlines. This flag 
		 * only works in conjunction with GGO_BEZIER and GGO_NATIVE. 
		 */
		public const uint GGO_UNHINTED = 0x0100;

		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern int GetCharWidthFloat(
			IntPtr hdc,					// handle to DC
			uint iFirstChar,			// first-character code point
			uint iLastChar,				// last-character code point
			[In,Out] float[] pxBuffer	// buffer for widths
			);

		/**
		 * The GetTextExtentPoint32 function computes the width and height of 
		 * the specified string of text. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern int GetTextExtentPoint32(
			IntPtr hdc,				// handle to DC
			String lpString,		// text string
			int cbString,			// characters in string
			[In,Out] SIZE lpSize    // string size
			);

		/**
		 * The GetTextMetrics function fills the specified buffer with the metrics 
		 * for the currently selected font. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern int GetTextMetrics(
			IntPtr hdc,				// handle to DC
			ref TEXTMETRIC lptm		// text metrics
			);

		/**
		 * The GetDC function retrieves a handle to a display device context (DC) 
		 * for the client area of a specified window or for the entire screen. 
		 * You can use the returned handle in subsequent GDI functions to draw 
		 * in the DC.
		 */
		[DllImport("User32.dll")]
		public static extern IntPtr GetDC(
			IntPtr hWnd				// handle to window
			);

		/**
		 * The ReleaseDC function releases a device context (DC), freeing it 
		 * for use by other applications. The effect of the ReleaseDC function 
		 * depends on the type of DC. It frees only common and window DCs. 
		 * It has no effect on class or private DCs. 
		 */
		[DllImport("User32.dll")]
		public static extern int ReleaseDC(
			IntPtr hWnd,	// handle to window
			IntPtr hDC		// handle to DC
			);

		/**
		 * The GetGlyphIndices function translates a string into an array of 
		 * glyph indices. The function can be used to determine whether a 
		 * glyph exists in a font.
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern uint GetGlyphIndices(
			IntPtr hdc,				// handle to DC
			string lpstr,			// string to convert
			int c,					// number of characters in string
			[In,Out] ushort[] pgi,	// array of glyph indices
			uint fl					// glyph options
			);

		public const uint GGI_MARK_NONEXISTING_GLYPHS = 0xffff;

		/**
		 * The GetLastError function retrieves the calling thread's last-error code 
		 * value. The last-error code is maintained on a per-thread basis. Multiple 
		 * threads do not overwrite each other's last-error code.
		 */
		[DllImport("Kernel32.dll")]
		public static extern uint GetLastError();

		/**
		 * The SelectObject function selects an object into the specified 
		 * device context (DC). The new object replaces the previous object 
		 * of the same type. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr SelectObject(
			IntPtr hdc,      // handle to DC
			IntPtr hgdiobj   // handle to object
			);

		/**
		 * The DeleteObject function deletes a logical pen, brush, font, 
		 * bitmap, region, or palette, freeing all system resources associated 
		 * with the object. After the object is deleted, the specified handle 
		 * is no longer valid. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern bool DeleteObject(IntPtr hObject);

		/**
		 * The GetCharABCWidthsI function retrieves the widths, in logical units, 
		 * of consecutive glyph indices in a specified range from the current 
		 * TrueType font. This function succeeds only with TrueType fonts. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern bool GetCharABCWidthsI(
			IntPtr hdc,				// handle to DC
			uint giFirst,			// first glyph index in range
			uint cgi,				// count of glyph indices in range
			[In,Out] ushort[] pgi,  // array of glyph indices
			[In,Out] ABC[] lpabc    // array of character widths
			);

		/**
		 * The DrawText function draws formatted text in the specified rectangle. 
		 * It formats the text according to the specified method (expanding tabs, 
		 * justifying characters, breaking lines, and so forth). 
		 * To specify additional formatting options, use the DrawTextEx function.
		 */
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern int DrawText(
            IntPtr hDC,				// handle to DC
			string lpString,		// text to draw
			int nCount,				// text length
			ref RECT lpRect,		// formatting dimensions
			uint uFormat			// text-drawing options
			);

		/**
		 * The ChooseFont function creates a Font dialog box that enables 
		 * the user to choose attributes for a logical font. These 
		 * attributes include a typeface name, style (bold, italic, or regular), 
		 * point size, effects (underline, strikeout, and text color), and a 
		 * script (or character set). 
		 */
		[DllImport("Comdlg32.dll", CharSet=CharSet.Auto)]
		public static extern bool ChooseFont(
            ref CHOOSEFONT lpcf   // initialization data
			);


		/**
		 * The GetFontUnicodeRanges function returns information about which 
		 * Unicode characters are supported by a font. The information is 
		 * returned as a GLYPHSET structure.
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern uint GetFontUnicodeRanges(
            IntPtr hdc,         // handle to DC
            GLYPHSET* lpgs   // glyph set
        );

		/**
		 * The GetTextFace function retrieves the typeface name of the font 
		 * that is selected into the specified device context. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern int GetTextFace(
            IntPtr hdc,					// handle to DC
			int nCount,					// length of typeface name buffer
			[In,Out] char[] lpFaceName  // typeface name buffer
		);

		/**
		 * The GetCharABCWidthsFloat function retrieves the widths, in logical units, 
		 * of consecutive characters in a specified range from the current font. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern bool GetCharABCWidthsFloat(
			IntPtr hdc,				 // handle to DC
			uint iFirstChar,		 // first character in range
			uint iLastChar,			 // last character in range
			[In,Out] ABCFLOAT[] lpABCF // array of character widths
			);

		/**
		 * The TextOut function writes a character string at the specified 
		 * location, using the currently selected font, background color, 
		 * and text color. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern bool  TextOut(
			IntPtr hdc,        // handle to DC
			int nXStart,       // x-coordinate of starting position
			int nYStart,       // y-coordinate of starting position
			String lpString,   // character string
			int cbString       // number of characters
			);

		/* Text Alignment Options */
		public const uint TA_NOUPDATECP = 0;
		public const uint TA_UPDATECP=    1;
		public const uint TA_LEFT =       0;
		public const uint TA_RIGHT =      2;
		public const uint TA_CENTER =     6;
		public const uint TA_TOP =        0;
		public const uint TA_BOTTOM =     8;
		public const uint TA_BASELINE =   24;
		public const uint TA_RTLREADING = 256;
		public const uint TA_MASK =       (TA_BASELINE+TA_CENTER+TA_UPDATECP+TA_RTLREADING);


		/**
		 * The SetTextAlign function sets the text-alignment flags for the 
		 * specified device context. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern uint SetTextAlign(
			IntPtr hdc,     // handle to DC
			uint fMode		// text-alignment option
			);

		/**
		 * The GetTextAlign function retrieves the text-alignment setting 
		 * for the specified device context. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern uint SetTextAlign(
			IntPtr hdc   // handle to DC
			);


		/**
		 * The LineTo function draws a line from the current position up to, but 
		 * not including, the specified point. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern bool LineTo(
			IntPtr hdc, // device context handle
			int nXEnd,  // x-coordinate of ending point
			int nYEnd   // y-coordinate of ending point
			);

		/**
		 * The MoveToEx function updates the current position to the specified 
		 * point and optionally returns the previous position. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern bool MoveToEx(
			IntPtr hdc,          // handle to device context
			int X,               // x-coordinate of new current position
			int Y,               // y-coordinate of new current position
			ref POINT lpPoint    // old current position
			);

		/**
		 * The Rectangle function draws a rectangle. The rectangle is outlined by 
		 * using the current pen and filled by using the current brush. 
		 */
		[DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
		public static extern bool Rectangle(
			IntPtr hdc,      // handle to DC
			int nLeftRect,   // x-coord of upper-left corner of rectangle
			int nTopRect,    // y-coord of upper-left corner of rectangle
			int nRightRect,  // x-coord of lower-right corner of rectangle
			int nBottomRect  // y-coord of lower-right corner of rectangle
			);





	}
}
