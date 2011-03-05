using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace FontTester
{
	/**
	 * test control to display fonts, and test win32 interop 
	 * code. This IS NOT GOOD CODE, this is only TEST CODE
	 * and is REALLY SLOPPY
	 */
	public class FontViewer : System.Windows.Forms.Control
	{
		private int row = 0;
		private int column = 0;

		public Win32.TEXTMETRIC Textmetric = new Win32.TEXTMETRIC();
		public Win32.ABCFLOAT AbcFloat = new Win32.ABCFLOAT();
		public Win32.GLYPHMETRICS Glyphmetrics = new Win32.GLYPHMETRICS();

		public int offset = 20;

		public uint SelectedGlyphIndex
		{
			get
			{
				return (uint)(row * 20 + column + offset);
			}
		}

		public uint GlyphIndex = 0;

		public event EventHandler GlyphChanged;

		public FontViewer()
		{
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			if(!DesignMode)
			{
				int height;
				int width;
				IntPtr dc = pe.Graphics.GetHdc();
				Win32.POINT pt = new Win32.POINT();
				Win32.TEXTMETRIC tm = new Win32.TEXTMETRIC();
				IntPtr font = Font.ToHfont();
				IntPtr oldFont = Win32.SelectObject(dc, font);

				Win32.GetTextMetrics(dc, ref tm);
				height = tm.tmHeight + 10;
				width = tm.tmMaxCharWidth + 10;

				Win32.MoveToEx(dc, 0, 0, ref pt);

				for(int i = 0; i < 14; i++)
				{
					Win32.LineTo(dc, 20 * width, i * height);
					Win32.MoveToEx(dc, 0, i * height + height, ref pt);
				}
			
				Win32.MoveToEx(dc, 0, 0, ref pt);
				for(int i = 0; i < 21; i++)
				{
					Win32.LineTo(dc, i * width, 13 * height);
					Win32.MoveToEx(dc, i * width + width, 0, ref pt);

				}

				int left = column * width + 2;
				int top = row * height + 2;
				int right = left + width - 3;
				int bottom = top + height - 3;
				Win32.Rectangle(dc, left, top, right, bottom);

				int k = 0; 
				for(int i = 0; k < 256; i++)
				{
					for(int j = 0; j < 20 && k < 256; j++)
					{
						String s = new String((char)(offset + k++), 1);
						Win32.TextOut(dc, j * width + 5, (i * height) + 5, s, s.Length);
					}
				}

				Win32.SelectObject(dc, oldFont);
				pe.Graphics.ReleaseHdc(dc);
			}
			base.OnPaint(pe);	
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				int row = 0;
				int column = 0;
				Win32.TEXTMETRIC tm = new Win32.TEXTMETRIC();
				IntPtr dc = Win32.GetDC(Handle);
				IntPtr font = Font.ToHfont();
				IntPtr oldFont = Win32.SelectObject(dc, font);

				Win32.GetTextMetrics(dc, ref tm);

				row = (int)Math.Floor((float)e.Y / (float)(tm.tmHeight + 10));
				column = (int)Math.Floor((float)e.X / (float)(tm.tmMaxCharWidth + 10));

				if(row < 13 && row * 20 + column < 256)
				{
					this.row = row;
					this.column = column;

					uint glyph = SelectedGlyphIndex;

					GlyphIndex = glyph;

					Win32.MAT2 mat = new Win32.MAT2();

					mat.eM11.val = 1;
					mat.eM22.val = 1;

					Win32.ABCFLOAT[] abc = new Win32.ABCFLOAT[1];

					Win32.GetTextMetrics(dc, ref Textmetric);
					Win32.GetCharABCWidthsFloat(dc, glyph, glyph, abc);

					AbcFloat = abc[0];

					Win32.GetGlyphOutline(dc, glyph, Win32.GGO_METRICS, ref Glyphmetrics, 0, IntPtr.Zero, ref mat);

					if(GlyphChanged != null)
					{
						GlyphChanged(this, null);
					}

					Refresh();
				}


				Win32.SelectObject(dc, oldFont);
				Win32.ReleaseDC(Handle, dc);
			}
			base.OnMouseDown(e);
		}

		
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);
		}
	}
}
