using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Imaging;
using System.Xml;

namespace MathML.Rendering.Test1
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form : System.Windows.Forms.Form
	{
		private readonly string startupDirectory;
		private MathML.Rendering.MathMLControl mathMLControl;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem miFile;
		private System.Windows.Forms.MenuItem miOpen;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.CheckBox readOnlyChk;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form()
		{
			startupDirectory = System.Environment.CurrentDirectory;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			bool f = mathMLControl.Focus();

			//MessageBox.Show("focus: " + f);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl ();
			bool f = this.mathMLControl.Enabled;
			f = this.mathMLControl.Focus();

		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus (e);
			bool f = this.mathMLControl.Focus();
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated (e);
			bool f = this.mathMLControl.Focus();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form));
			this.mathMLControl = new MathML.Rendering.MathMLControl();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.miFile = new System.Windows.Forms.MenuItem();
			this.miOpen = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.readOnlyChk = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.menuItem18 = new System.Windows.Forms.MenuItem();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mathMLControl
			// 
			this.mathMLControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mathMLControl.AutoScroll = true;
			this.mathMLControl.BackColor = System.Drawing.Color.White;
			this.mathMLControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mathMLControl.BackgroundImage")));
			this.mathMLControl.ForeColor = System.Drawing.Color.Black;
			this.mathMLControl.HorizontalShift = 0F;
			this.mathMLControl.InputLocation = null;
			this.mathMLControl.Location = new System.Drawing.Point(0, 0);
			this.mathMLControl.MathElement = null;
			this.mathMLControl.MathFontSizeInPoints = 13;
			this.mathMLControl.Name = "mathMLControl";
			this.mathMLControl.ReadOnly = true;
			this.mathMLControl.SelectedElement = null;
			this.mathMLControl.SelectionColor = System.Drawing.Color.LightBlue;
			this.mathMLControl.Size = new System.Drawing.Size(776, 448);
			this.mathMLControl.TabIndex = 0;
			this.mathMLControl.VerticalShift = 0F;
			this.mathMLControl.ElementSelected += new MathML.Rendering.MathMLControlEventHandler(this.mathMLControl_AfterSelect);
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.miFile,
																					 this.menuItem12,
																					 this.menuItem1});
			// 
			// miFile
			// 
			this.miFile.Index = 0;
			this.miFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				   this.miOpen,
																				   this.menuItem17,
																				   this.menuItem16,
																				   this.menuItem18});
			this.miFile.Text = "File";
			// 
			// miOpen
			// 
			this.miOpen.Index = 0;
			this.miOpen.Text = "Open";
			this.miOpen.Click += new System.EventHandler(this.miOpen_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 1;
			this.menuItem17.Text = "Save As Image";
			this.menuItem17.Click += new System.EventHandler(this.miSaveImage_Click);
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 2;
			this.menuItem16.Text = "Set Background Image";
			this.menuItem16.Click += new System.EventHandler(this.menuItem16_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 1;
			this.menuItem12.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItem13,
																					   this.menuItem14,
																					   this.menuItem15});
			this.menuItem12.Text = "Edit";
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 0;
			this.menuItem13.Text = "Copy to clipboard as Metafile";
			this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 1;
			this.menuItem14.Text = "Copy to clipboard as Bitmap";
			this.menuItem14.Click += new System.EventHandler(this.menuItem14_Click);
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 2;
			this.menuItem15.Text = "Copy element to clipboard as string";
			this.menuItem15.Click += new System.EventHandler(this.menuItem15_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem3,
																					  this.menuItem4,
																					  this.menuItem5,
																					  this.menuItem6,
																					  this.menuItem7,
																					  this.menuItem8,
																					  this.menuItem9,
																					  this.menuItem10,
																					  this.menuItem11});
			this.menuItem1.Text = "Font Size";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "6";
			this.menuItem2.Click += new System.EventHandler(this.fontSize_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "8";
			this.menuItem3.Click += new System.EventHandler(this.fontSize_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "10";
			this.menuItem4.Click += new System.EventHandler(this.fontSize_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 3;
			this.menuItem5.Text = "12";
			this.menuItem5.Click += new System.EventHandler(this.fontSize_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 4;
			this.menuItem6.Text = "14";
			this.menuItem6.Click += new System.EventHandler(this.fontSize_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 5;
			this.menuItem7.Text = "16";
			this.menuItem7.Click += new System.EventHandler(this.fontSize_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 6;
			this.menuItem8.Text = "18";
			this.menuItem8.Click += new System.EventHandler(this.fontSize_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 7;
			this.menuItem9.Text = "20";
			this.menuItem9.Click += new System.EventHandler(this.fontSize_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 8;
			this.menuItem10.Text = "24";
			this.menuItem10.Click += new System.EventHandler(this.fontSize_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 9;
			this.menuItem11.Text = "26";
			this.menuItem11.Click += new System.EventHandler(this.fontSize_Click);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 508);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(776, 22);
			this.statusBar.TabIndex = 3;
			// 
			// readOnlyChk
			// 
			this.readOnlyChk.Checked = true;
			this.readOnlyChk.CheckState = System.Windows.Forms.CheckState.Checked;
			this.readOnlyChk.Location = new System.Drawing.Point(8, 16);
			this.readOnlyChk.Name = "readOnlyChk";
			this.readOnlyChk.Size = new System.Drawing.Size(280, 32);
			this.readOnlyChk.TabIndex = 5;
			this.readOnlyChk.Text = "ReadOnly (un-check to allow editing, set cursor loation with the mouse, then move" +
				" with arrows)";
			this.readOnlyChk.CheckedChanged += new System.EventHandler(this.readOnlyChk_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.readOnlyChk);
			this.groupBox1.Location = new System.Drawing.Point(24, 456);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(368, 56);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Allow MathML Editing, EXPERIMENTAL, USE AT YOUR OWN RISK";
			// 
			// menuItem18
			// 
			this.menuItem18.Index = 3;
			this.menuItem18.Text = "Clear Background Image";
			this.menuItem18.Click += new System.EventHandler(this.menuItem18_Click);
			// 
			// Form
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(776, 530);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.mathMLControl);
			this.Menu = this.mainMenu;
			this.Name = "Form";
			this.Text = "Test1";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form());
		}

		private void miOpen_Click(object sender, System.EventArgs e)
		{
			try
			{
				OpenFileDialog open = new OpenFileDialog();
				open.Filter = "MathML Files (*.mml; *.mathml; *.xml; *.html)|*.mml;*.mathml;*.xml;*.html";
				if(open.ShowDialog() == DialogResult.OK)
				{
					System.Environment.CurrentDirectory = startupDirectory;
					MathML.MathMLDocument doc = new MathML.MathMLDocument();
					doc.Load(open.FileName);
					mathMLControl.MathElement = (MathMLMathElement)doc.DocumentElement;
					Text = open.FileName;
				}
			}
			catch(Exception exp)
			{
				MessageBox.Show(exp.Message + "trace: " + exp.StackTrace);
			}
		}

		private void miSaveImage_Click(object sender, System.EventArgs e)
		{
			try
			{
				SaveFileDialog dialog = new SaveFileDialog();
				dialog.Filter = "bitmap image (BMP)|*.bmp|" +
					"enhanced Windows metafile (EMF)|*.emf|" +
					"Exchangeable Image File (EXIF)|*.exif|" +
					"Graphics Interchange Format (GIF)|*.gif|" +
					"Windows icon image (ICO)|*.ico|" + 
					"Jpeg image format (JPEG)|*.jpeg|" +
					"Portable Network Graphics (PNG)|*.png|" +
					"Tag Image File Format (TIFF)|*.tiff|" +
					"Windows metafile (WMF)|*.wmf";
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					ImageFormat format;
					string[] split = dialog.FileName.Split(new char[] {'.'});
					string ext = split[split.Length - 1].ToLower();
					switch(ext)
					{
						case "jpeg": case "jpg": format = ImageFormat.Jpeg; break;
						case "bmp": format = ImageFormat.Bmp; break;
						case "emf" : format = ImageFormat.Emf; break;
						case "gif" : format = ImageFormat.Gif; break;
						case "ico" : format = ImageFormat.Icon; break;
						case "png" : format = ImageFormat.Png; break;
						case "tiff" : format = ImageFormat.Tiff; break;
						case "wmf" : format = ImageFormat.Wmf; break;
						default: throw new Exception(ext + " is an invalid image file format");
					}

					mathMLControl.Save(dialog.FileName, format);
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error saving image: " + ex.ToString());
			}
		
		}

		private void miCopy_Click(object sender, System.EventArgs e)
		{
			mathMLControl.CopyToClipboard();
		}

		private void fontSize_Click(object sender, System.EventArgs e)
		{
			int size = Int32.Parse(((MenuItem)sender).Text);
			mathMLControl.MathFontSizeInPoints = size;
		}

		private void mathMLControl_AfterSelect(object sender, MathML.Rendering.MathMLControlEventArgs e)
		{
			XmlNode n = e.Element;
			String text =  ":" + e.Element.InnerXml;
			while(n != null && n.ParentNode != null)
			{
				text = "<" + n.Name + ">" + text;
				n = n.ParentNode;
			}

			statusBar.Text = "Selected Node: " + text;
		}

		private void readOnlyChk_CheckedChanged(object sender, System.EventArgs e)
		{
			mathMLControl.ReadOnly = readOnlyChk.Checked;
		}

		private void menuItem13_Click(object sender, System.EventArgs e)
		{
			mathMLControl.CopyToClipboard(typeof(Metafile));
		}

		private void menuItem15_Click(object sender, System.EventArgs e)
		{
			mathMLControl.CopyToClipboard(typeof(String));
		
		}

		private void menuItem14_Click(object sender, System.EventArgs e)
		{
			mathMLControl.CopyToClipboard(typeof(Bitmap));
		}

		private void menuItem16_Click(object sender, System.EventArgs e)
		{
			try
			{
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					Image image = Image.FromFile(dialog.FileName);
					mathMLControl.BackgroundImage = image;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error opening image: " + ex.ToString());
			}		
		}

		private void menuItem18_Click(object sender, System.EventArgs e)
		{
			mathMLControl.BackgroundImage = null;
		}
	}
}
