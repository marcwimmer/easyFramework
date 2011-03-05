using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace FontTester
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private FontViewer fontViewer;
		private System.Windows.Forms.Button fontChooseButton;
		private System.Windows.Forms.GroupBox tmBox;
		private System.Windows.Forms.Label tmLabel;
		private System.Windows.Forms.GroupBox gmBox;
		private System.Windows.Forms.Label gmLabel;
		private System.Windows.Forms.GroupBox abcBox;
		private System.Windows.Forms.Label abcLabel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label glyphLabel;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox offset;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label bbLabel;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private static String FormatTextmetric(Win32.TEXTMETRIC tm)
		{
			string format = @"tmHeight =           {0}, 
			tmAscent =           {1},
			tmDescent =          {2},
			tmInternalLeading =  {3},
			tmExternalLeading =  {4},
			tmAveCharWidth =     {5},
			tmMaxCharWidth =     {6},
			tmWeight =           {7},
			tmOverhang =         {8},
			tmDigitizedAspectX = {9},
			tmDigitizedAspectY = {10},
			tmFirstChar =        {11},
			tmLastChar =         {12},
			tmDefaultChar =      {13},
			tmBreakChar =        {14},
			tmItalic =           {15},
			tmUnderlined =       {16},
			tmStruckOut =        {17},
			tmPitchAndFamily =   {18},
			tmCharSet =          {19}";

			return String.Format(format, 
				tm.tmHeight,
				tm.tmAscent, 
				tm.tmDescent, 
				tm.tmInternalLeading, 
				tm.tmExternalLeading, 
				tm.tmAveCharWidth, 
				tm.tmMaxCharWidth, 
				tm.tmWeight, 
				tm.tmOverhang, 
				tm.tmDigitizedAspectX, 
				tm.tmDigitizedAspectY, 
				(ulong)tm.tmFirstChar, 
				(ulong)tm.tmLastChar, 
				(ulong)tm.tmDefaultChar, 
				(ulong)tm.tmBreakChar, 
				tm.tmItalic, 
				tm.tmUnderlined, 
				tm.tmStruckOut, 
				tm.tmPitchAndFamily, 
				tm.tmCharSet);
		}
		
		public static String FormatGlyphmetrics(Win32.GLYPHMETRICS gm)
		{
			string format = @"gmBlackBoxX =        {0} 
			gmBlackBoxY =        {1} 			 
			gmCellIncX =         {2} 
			gmCellIncY =         {3} 
			gmptGlyphOrigin.x =  {4}
			gmptGlyphOrigin.y =  {5}";

			return String.Format(format, 
				gm.gmBlackBoxX, 
				gm.gmBlackBoxY, 
				gm.gmCellIncX, 
				gm.gmCellIncY, 
				gm.gmptGlyphOrigin.x, 
				gm.gmptGlyphOrigin.y);
		}

		public static String FormatBoundingBox(Win32.GLYPHMETRICS gm)
		{
			string format = @"Width =          {0}, 
			Height =         {1}, 
			Depth =          {2}, 
			VerticalExtent = {3}";

            // the total cell advance
			float width = gm.gmCellIncX;

			// the height above the baseline
			float height = gm.gmptGlyphOrigin.y;

			// the total height - height above baseline
			float depth = gm.gmBlackBoxY - gm.gmptGlyphOrigin.y;

			float ve = height + depth;

			return String.Format(format, 
				width, 
				height, 
				depth, 
				ve);

			// distance from cell origin to left edge of glyph
			//left = gm.gmptGlyphOrigin.x;

			// the space on the right edge of the glyph.
			// get this by taking the total advance, sub bouding box, 
			// and origin.
			//right = gm.gmCellIncX - gm.gmBlackBoxX - gm.gmptGlyphOrigin.x;
		}

		public static String FormatAbcfloat(Win32.ABCFLOAT abc)
		{
			string format = @"a =                  {0} 
			b =                  {1} 			 
			c =                  {2}";

			return String.Format(format, abc.abcfA, abc.abcfB, abc.abcfC);
		}


		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.fontViewer = new FontTester.FontViewer();
			this.fontChooseButton = new System.Windows.Forms.Button();
			this.tmBox = new System.Windows.Forms.GroupBox();
			this.tmLabel = new System.Windows.Forms.Label();
			this.gmBox = new System.Windows.Forms.GroupBox();
			this.gmLabel = new System.Windows.Forms.Label();
			this.abcBox = new System.Windows.Forms.GroupBox();
			this.abcLabel = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.glyphLabel = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.offset = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.bbLabel = new System.Windows.Forms.Label();
			this.tmBox.SuspendLayout();
			this.gmBox.SuspendLayout();
			this.abcBox.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// fontViewer
			// 
			this.fontViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.fontViewer.BackColor = System.Drawing.SystemColors.Window;
			this.fontViewer.Location = new System.Drawing.Point(166, 8);
			this.fontViewer.Name = "fontViewer";
			this.fontViewer.Size = new System.Drawing.Size(621, 736);
			this.fontViewer.TabIndex = 1;
			this.fontViewer.GlyphChanged += new System.EventHandler(this.fontViewer_GlyphChanged);
			// 
			// fontChooseButton
			// 
			this.fontChooseButton.Location = new System.Drawing.Point(6, 8);
			this.fontChooseButton.Name = "fontChooseButton";
			this.fontChooseButton.Size = new System.Drawing.Size(154, 24);
			this.fontChooseButton.TabIndex = 2;
			this.fontChooseButton.Text = "Choose Font";
			this.fontChooseButton.Click += new System.EventHandler(this.fontChooseButton_Click);
			// 
			// tmBox
			// 
			this.tmBox.Controls.Add(this.tmLabel);
			this.tmBox.Location = new System.Drawing.Point(8, 152);
			this.tmBox.Name = "tmBox";
			this.tmBox.Size = new System.Drawing.Size(154, 296);
			this.tmBox.TabIndex = 3;
			this.tmBox.TabStop = false;
			this.tmBox.Text = "Text Metrics";
			// 
			// tmLabel
			// 
			this.tmLabel.Location = new System.Drawing.Point(6, 16);
			this.tmLabel.Name = "tmLabel";
			this.tmLabel.Size = new System.Drawing.Size(143, 272);
			this.tmLabel.TabIndex = 0;
			// 
			// gmBox
			// 
			this.gmBox.Controls.Add(this.gmLabel);
			this.gmBox.Location = new System.Drawing.Point(8, 456);
			this.gmBox.Name = "gmBox";
			this.gmBox.Size = new System.Drawing.Size(154, 112);
			this.gmBox.TabIndex = 4;
			this.gmBox.TabStop = false;
			this.gmBox.Text = "Glyph Metrics";
			// 
			// gmLabel
			// 
			this.gmLabel.Location = new System.Drawing.Point(6, 16);
			this.gmLabel.Name = "gmLabel";
			this.gmLabel.Size = new System.Drawing.Size(143, 88);
			this.gmLabel.TabIndex = 0;
			// 
			// abcBox
			// 
			this.abcBox.Controls.Add(this.abcLabel);
			this.abcBox.Location = new System.Drawing.Point(8, 576);
			this.abcBox.Name = "abcBox";
			this.abcBox.Size = new System.Drawing.Size(154, 64);
			this.abcBox.TabIndex = 5;
			this.abcBox.TabStop = false;
			this.abcBox.Text = "ABC Float Width";
			// 
			// abcLabel
			// 
			this.abcLabel.Location = new System.Drawing.Point(6, 16);
			this.abcLabel.Name = "abcLabel";
			this.abcLabel.Size = new System.Drawing.Size(143, 40);
			this.abcLabel.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.glyphLabel);
			this.groupBox1.Location = new System.Drawing.Point(8, 96);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(152, 48);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Selected Glyph";
			// 
			// glyphLabel
			// 
			this.glyphLabel.Location = new System.Drawing.Point(8, 16);
			this.glyphLabel.Name = "glyphLabel";
			this.glyphLabel.Size = new System.Drawing.Size(136, 24);
			this.glyphLabel.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.offset);
			this.groupBox2.Location = new System.Drawing.Point(8, 40);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(152, 40);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Font starting offset";
			// 
			// offset
			// 
			this.offset.Location = new System.Drawing.Point(8, 16);
			this.offset.Name = "offset";
			this.offset.Size = new System.Drawing.Size(136, 20);
			this.offset.TabIndex = 0;
			this.offset.Text = "0";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.bbLabel);
			this.groupBox3.Location = new System.Drawing.Point(8, 648);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(152, 96);
			this.groupBox3.TabIndex = 8;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "BoundingBox";
			// 
			// bbLabel
			// 
			this.bbLabel.Location = new System.Drawing.Point(8, 16);
			this.bbLabel.Name = "bbLabel";
			this.bbLabel.Size = new System.Drawing.Size(136, 72);
			this.bbLabel.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 749);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.abcBox);
			this.Controls.Add(this.gmBox);
			this.Controls.Add(this.tmBox);
			this.Controls.Add(this.fontChooseButton);
			this.Controls.Add(this.fontViewer);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "Form1";
			this.Text = "Font Tester";
			this.tmBox.ResumeLayout(false);
			this.gmBox.ResumeLayout(false);
			this.abcBox.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private unsafe void testButton_Click(object sender, System.EventArgs e)
		{
			Win32.ABC[] abc = new Win32.ABC[1];
            Win32.TEXTMETRIC tm = new Win32.TEXTMETRIC();	
			Win32.GLYPHMETRICS gm = new Win32.GLYPHMETRICS();
			Win32.LOGFONT lf = new Win32.LOGFONT();
			Win32.MAT2 mat = new Win32.MAT2();
			Win32.GLYPHSET gs = new Win32.GLYPHSET();
			float[] buffer = new float[56];
			string str = "abcdefghigjklmnopqrstuvwxyz";
			ushort[] indices = new ushort[str.Length];
			char[] face_buffer = new Char[100];

			lf.lfHeight = -12;
			lf.lfWidth = 0;
			lf.lfEscapement = 0;
			lf.lfOrientation = 0;
			lf.lfWeight = 400;
			lf.lfItalic = 0;
			lf.lfUnderline = 0;
			lf.lfCharSet = Win32.ANSI_CHARSET;
			lf.lfOutPrecision = Win32.OUT_DEFAULT_PRECIS;
			lf.lfClipPrecision = Win32.CLIP_DEFAULT_PRECIS;
			lf.lfQuality = Win32.DEFAULT_QUALITY;
			lf.lfPitchAndFamily = Win32.DEFAULT_PITCH;
			lf.lfFaceName = "Times New Roman";

			IntPtr font = Win32.CreateFontIndirect(ref lf);

			IntPtr dc = Win32.GetDC(Handle);

			IntPtr oldFont = Win32.SelectObject(dc, font);

			Win32.GetTextMetrics(dc, ref tm);

			int size = sizeof(Win32.TEXTMETRIC);

			mat.eM11.val = 1;
			mat.eM22.val = 1;

			Win32.GetGlyphIndices(dc, str, str.Length, indices, 0);

			Win32.GetCharABCWidths(dc, 'a', 'a', abc);
			if(Win32.GetGlyphOutline(dc, 'a', Win32.GGO_METRICS, ref gm, 0, IntPtr.Zero, ref mat)
				== Win32.GDI_ERROR)
			{
				uint error = Win32.GetLastError();
				MessageBox.Show(String.Format("Error: {0}", error));
			}

			if(Win32.GetGlyphOutline(dc, 'I', Win32.GGO_METRICS, ref gm, 0, IntPtr.Zero, ref mat)
				== Win32.GDI_ERROR)
			{
				uint error = Win32.GetLastError();
				MessageBox.Show(String.Format("Error: {0}", error));
			}

			Win32.GetCharWidthFloat(dc, 'a', 'z', buffer);

			gs.cbThis = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(Win32.GLYPHSET));
			
			Win32.GetTextFace(dc, face_buffer.Length, face_buffer);

			String s = new String(face_buffer);

			Win32.SelectObject(dc, oldFont);
			Win32.DeleteObject(font);

			Win32.ReleaseDC(Handle, dc);
			MessageBox.Show(FormatTextmetric(tm));
		}

		private void fontChooseButton_Click(object sender, System.EventArgs e)
		{
			FontDialog d = new FontDialog();
			d.Font = fontViewer.Font;


			try
			{
				fontViewer.offset = Int32.Parse(offset.Text);
			}
			catch(Exception)
			{
				fontViewer.offset = 0;
				offset.Text = "0";
			}

			if(d.ShowDialog() == DialogResult.OK)
			{
				fontViewer.Font = d.Font;
				fontViewer.Refresh();
			}
		}

		private void fontViewer_GlyphChanged(object sender, System.EventArgs e)
		{
			tmLabel.Text = FormatTextmetric(fontViewer.Textmetric);
			gmLabel.Text = FormatGlyphmetrics(fontViewer.Glyphmetrics);
			abcLabel.Text = FormatAbcfloat(fontViewer.AbcFloat);
			bbLabel.Text = FormatBoundingBox(fontViewer.Glyphmetrics);
			glyphLabel.Text = String.Format("Index: {0}, char value: \"{1}\"", 
				fontViewer.SelectedGlyphIndex, (char)fontViewer.SelectedGlyphIndex);
			
		}

		private void testButton1_Click(object sender, System.EventArgs e)
		{
			Win32.LOGFONT lf = new Win32.LOGFONT();
			IntPtr font1, font2;

			lf.lfHeight = -12;
			lf.lfWidth = 0;
			lf.lfEscapement = 0;
			lf.lfOrientation = 0;
			lf.lfWeight = 400;
			lf.lfItalic = 0;
			lf.lfUnderline = 0;
			lf.lfCharSet = Win32.ANSI_CHARSET;
			lf.lfOutPrecision = Win32.OUT_DEFAULT_PRECIS;
			lf.lfClipPrecision = Win32.CLIP_DEFAULT_PRECIS;
			lf.lfQuality = Win32.DEFAULT_QUALITY;
			lf.lfPitchAndFamily = Win32.DEFAULT_PITCH;
			lf.lfFaceName = "Times New Roman";

			font1 = Win32.CreateFontIndirect(ref lf);
			font2 = Win32.CreateFontIndirect(ref lf);
		
		}
	}
}
