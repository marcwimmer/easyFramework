using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;

namespace MathML.Rendering
{
	/**
	 * calaulates sizes of table cells and spacing
	 * 
	 * This class does not calculate offsets of cells, that is the job of the formatter.
	 * This class only calculates sizes of cells and spaces, that is suffcient information
	 * to lay out the cells in a grid.
	 */
	internal class MathMLTableSizer
	{
		/**
		 * equalColumns attr read from table
		 */
		private bool equalColumns;

		/**
		 * collection of columns
		 */
		private Column[] columns;

		/**
		 * collection of cells, read from source table
		 */
		private MathMLTableCellElement[][] cells;	

		/**
		 * collection of caculated cell sizes
		 */
		private BoundingBox[][] cellSizes;

		//private float[] verticalSpaces;
		//private float[] horizontalSpaces;

		//private float shift;

		//public float verticalExtent;

		private BoundingBox box;

		/**
		 * lines that form the vertical and horizontal lines
		 * of this table.
		 */
		private PointF[] solidLines;
		private PointF[] dashedLines;

		private PointF[][] cellShifts;

		/**
		 * construct a new sizer. All calculations are done here.
		 */
		public MathMLTableSizer(FormattingContext ctx, MathMLTableElement table)
		{
			try
			{
				MathMLMeasurer measurer = new MathMLMeasurer();
				equalColumns = table.EqualColumns;

				// get the cells from the table and store them in a multi dim array
				cells = GetCells(table);

                // min area sizes for the cells
				BoundingBox[][] minCellSizes = measurer.MeasureElements(ctx, cells);
				DebugWriteCellSizes(minCellSizes);

				// count of columns that are actual table cells
				int cellColumnCount = GetCellColumnCount(cells);

				// column widths attr from dom
				Length[] columnWidths = GetColumnWidths(table, cellColumnCount);

				// column space attr from dom
				Length[] columnSpacing = GetColumnSpacing(table, cellColumnCount - 1);

				// frame spaceing attr from dom
				Length[] frameSpacing = table.FrameSpacing;

				// row spacing attr from dom
				Length[] rowSpacing = GetRowSpacing(table, cells.Length - 1);

				// create a set of columns from the cells
				columns = CreateColumns(ctx, cells, minCellSizes, cellColumnCount, columnWidths, 
					columnSpacing, frameSpacing[0]);

				// create a set of row measurments, this is both the cell rows and spacing rows
				Row[] rows = CreateRows(ctx, cells, minCellSizes, rowSpacing, frameSpacing[1]);

				// adjust rows and columns so there is enough space to fit spanning cells
				AdjustSpanningCells(cells, minCellSizes, rows, columns);

				Debug.WriteLine("retrieved " + columns.Length + "total columns, ");
				DebugWriteColumnWidths("Initial Column Widths: ");

				// get the minimum area for the columns
				float tableWidth = CalculateTableWidth(ctx, table, columns, equalColumns, cellColumnCount);
				Debug.WriteLine("Minimum Table Width: " + tableWidth);

				// find the space needed to fit the availible for the auto and fit columns
				float availSpace = CalculateColumnAvailSpace(columns, tableWidth);
				Debug.WriteLine("Availible Space For Columns: " + availSpace);

				// set the table widths
				DistributeSpace(columns, tableWidth, availSpace, cellColumnCount, equalColumns);

				DebugWriteColumnWidths("Column Widths After Formatting: ");               

				// calcuate the shift 
				float shift = 0;
				CalculateShiftAndSize(ctx, table, rows, columns, ref box, ref shift);

				// create the cell sizes and offsets
				CreateCellSizes(cells, rows, columns, ref cellSizes, ref cellShifts);
				

				// create the lines for the frame and cell separators
				LineStyle frame = table.Frame;
				LineStyle[] columnLines = GetColumnLines(table, cellColumnCount);
				LineStyle[] rowLines = GetRowLines(table, cells.Length);
				CreateLines(cellSizes, cells, rows, columns, columnLines, rowLines, frame);				
			}
			catch(Exception ex)
			{
				Debug.Assert(false, ex.Message);
			}
		}

        /** 
		 * the cells that compose this table
		 */
		public MathMLTableCellElement[][] Cells
		{
			get { return cells; }
		}

		/**
		 * calculated sizes for each cell
		 */
		public BoundingBox[][] CellSizes
		{
			get { return cellSizes; }
		}

		/** 
		 * get the solid lines for the frame and cells
		 */
		public PointF[] SolidLines
		{
			get { return solidLines; }
		}

		/**
		 * get the dashed lines for the frame and cells
		 */
		public PointF[] DashedLines
		{
			get { return dashedLines; }
		}

		/**
		 * get the location of each cell
		 */
		public PointF[][] CellShifts
		{
			get { return cellShifts; }
		}

		/**
		 * the the bounds of this table after formatting
		 */
		public BoundingBox BoundingBox
		{
			get { return box; }
		}

		/**
		 * calculates the minimum table width to satisfy all columns.
		 * This uses each column's Width property to determine the minimum
		 * required width to fit each column.
		 * 
		 * pre-reqs: columns, column.Width and equalColumns have been set
		 * readonly function
		 */
		private static float CalculateTableWidth(FormattingContext ctx, MathMLTableElement table, 
			Column[] columns, bool equalColumns, int effectiveColumns)
		{
			Length widthLength = table.Width;
			if(widthLength.Fixed)
			{
				return ctx.Evaluate(widthLength);
			}
			else
			{
				// length is auto, so need to calculate width from columns
				// sanity check
				Debug.Assert(widthLength.Type == LengthType.Auto, "table width is not fixed type but also is not auto");

				// summed widths
				float sumFixWidth = 0;
				float sumScaleWidth = 0;

				if(equalColumns)
				{
					// equal spacing, space the largest column width equally
					// first find largest column width
					float maxWidth = 0;

					for(int i = 0; i < columns.Length; i++)
					{
						switch(columns[i].Type)
						{							
							case ColumnType.Scale:
								sumScaleWidth += columns[i].ScaleWidth;
								break;
							case ColumnType.Fixed:
								sumFixWidth += columns[i].Width;
								break;
							default: // type is Auto or Fit
								if(columns[i].Width > maxWidth) maxWidth = columns[i].Width;
								break;
						}
					}
					return (((float)effectiveColumns * maxWidth) + sumFixWidth) / (1.0f - sumScaleWidth);
				}
				else
				{
					// look for a column that may have a wide min width but a very small percentage 
					// value. In this case, we could have a column that is specified to take up 1% 
					// of the table width, in that case, the rest of the table must be stretched
					// to take up the 99%
					float maxEvaluatedWidth = 0;

					// summed widths
					float sumAutoFitWidth = 0;


					for(int i = 0; i < columns.Length; i++)
					{
						switch(columns[i].Type)
						{							
							case ColumnType.Scale:
								sumScaleWidth += columns[i].ScaleWidth;
								// evaluate the width of the scaled column using the current width
								// of the column
								float evalWidth = columns[i].Width / columns[i].ScaleWidth;
								if(evalWidth > maxEvaluatedWidth) maxEvaluatedWidth = evalWidth;
								break;
							case ColumnType.Fixed:
								sumFixWidth += columns[i].FixedWidth;
								break;
							default: // column type is Auto or Fit
								sumAutoFitWidth += columns[i].Width;
								break;
						}
					}

					float width = (sumAutoFitWidth + sumFixWidth) / (1.0f - sumScaleWidth);
					return Math.Max(width, maxEvaluatedWidth);
				}
			}
		}

		/**
		 * calculates the space available for auto / fit columns
		 * this is the availible table space after the fix and scale 
		 * widths have been subtracted from the given table width.
		 * readonly function
		 * @param tableWidth the current width of the table
		 */
		private static float CalculateColumnAvailSpace(Column[] columns, float tableWidth)
		{
			float sumScale = 0;
			float sumFix = 0;
			for(int i = 0; i < columns.Length; i++)
			{
				if(columns[i].Type == ColumnType.Scale)
				{
					sumScale += columns[i].ScaleWidth;
				}
				else if(columns[i].Type == ColumnType.Fixed)
				{
					sumFix += columns[i].FixedWidth;
				}
			}
			return Math.Max(0.0f, (tableWidth * (1.0f - sumScale)) - sumFix); 
		}

		/**
		 * distrubute the calculated table width and column space
		 * to scaled, auto and fited columns
		 */
		private static void DistributeSpace(Column[] columns, float tableWidth, float availSpace, 
			int effectiveColumns, bool equalColumns)
		{
			Debug.WriteLine("distrubuting space for tableWidth of: " + tableWidth);
			// number of fit / auto columns
			int numFit = 0;
			int numAuto = 0;

			if(equalColumns)
			{
				for(int i = 0; i < columns.Length; i++)
				{
					// 1: leave the width alone if it is fixed, it is allready set
					// 2: scale the scale type columns
					// 3: distribute space evenly amoung the fit / auto columns
					if(columns[i].Type == ColumnType.Scale)
					{
						columns[i].Width = tableWidth * columns[i].ScaleWidth;
					}
					else if(columns[i].Type == ColumnType.Fit)
					{
						columns[i].Width = availSpace / (float)effectiveColumns;
					}
					else if(columns[i].Type == ColumnType.Auto)
					{
						columns[i].Width = availSpace / (float)effectiveColumns;
					}
				}
			}
			else
			{				
				float sumMinWidths = 0;
				// calulate num of fit columns and summed min widths of auto and fit columns
				for(int i = 0; i < columns.Length; i++) 
				{
					if(columns[i].Type == ColumnType.Fit) 
					{
						numFit++;
						sumMinWidths += columns[i].MinimumWidth;
					}
					else if(columns[i].Type == ColumnType.Auto)
					{
						numAuto++;
						sumMinWidths += columns[i].MinimumWidth;
					}
				}

				// value to be added to fit columns to fill up avail space
				float fitScale = numFit > 0 ? (availSpace - sumMinWidths) / (float)numFit : 0;				

				// value to be added to auto columns to fill up avail space if we have no fit columns
				float autoScale = numAuto > 0 ? (availSpace - sumMinWidths) / (float)numAuto : 0;	

				// distribute space
				// if we have fitting columns, these are stretched to fill the
				// given space. If we have no fit columns, then we stretch the auto
				// columns
				for(int i = 0; i < columns.Length; i++)
				{
					// 1: leave the width alone if it is fixed, it is allready set
					// 2: scale the scale type columns
					// 3: distribute space evenly amoung the fit / auto columns
					if(columns[i].Type == ColumnType.Scale)
					{
						columns[i].Width = tableWidth * columns[i].ScaleWidth;
					}
					else if(columns[i].Type == ColumnType.Auto)
					{
						columns[i].Width = numFit == 0 ? columns[i].MinimumWidth + autoScale : columns[i].MinimumWidth;
					}
					else if(columns[i].Type == ColumnType.Fit)
					{
						columns[i].Width = columns[i].MinimumWidth + fitScale;
					}
				}               
			}
		}

		/**
		* a table element generaly has fewer length types than columns. This 
		* method, using the spec of setting the last length from the table's
		* 'columnwidths' creates a array of lengths, one for each column, and
		* sets each item to the last real value if the lenthts from the table
		* is less than the number of columns
		*/
		private static Length[] GetColumnWidths(MathMLTableElement table, int columnCount)
		{
			// cache table column widths
			Length[] tcw = table.ColumnWidth;
			Length[] columnWidths = new Length[columnCount];
			for(int i = 0; i < columnWidths.Length; i++)
			{
				columnWidths[i] = tcw[i < tcw.Length ? i : tcw.Length - 1];				
			}
			return columnWidths;
		}

		private static Length[] GetColumnSpacing(MathMLTableElement table, int spaceCount)
		{
			Length[] columnSpacing = table.ColumnSpacing;
			Length[] columnSpaces = new Length[spaceCount];
			for(int i = 0; i < columnSpaces.Length; i++)
			{
				columnSpaces[i] = columnSpacing[i < columnSpacing.Length ? i : columnSpacing.Length - 1];
			}
			return columnSpaces;
		}

		/**
		 * grab the column styles from the table. 
		 */
		private static LineStyle[] GetColumnLines(MathMLTableElement table, int columnCount)
		{
			LineStyle[] columnLines = table.ColumnLines;
			LineStyle[] result = new LineStyle[columnCount - 1];
			for(int i = 0; i < result.Length; i++)
			{
				result[i] = columnLines[i < columnLines.Length ? i : columnLines.Length - 1];
			}
			return result;			
		}

		private static LineStyle[] GetRowLines(MathMLTableElement table, int rowCount)
		{
			LineStyle[] rowLines = table.RowLines;
			LineStyle[] result = new LineStyle[rowCount - 1];
			for(int i = 0; i < result.Length; i++)
			{
				result[i] = rowLines[i < rowLines.Length ? i : rowLines.Length - 1];
			}
			return result;
		}

		private static Length[] GetRowSpacing(MathMLTableElement table, int spaceCount)
		{
			Length[] rowSpacing = table.RowSpacing;
			Length[] rowSpaces = new Length[spaceCount];
			for(int i = 0; i < rowSpaces.Length; i++)
			{
				rowSpaces[i] = rowSpacing[i < rowSpacing.Length ? i : rowSpacing.Length - 1];
			}
			return rowSpaces;
		}

		/**
		 * find the maximum column count from a collection of cells, 
		 * this is the row with the largest number of cells
		 */
		public int GetCellColumnCount(MathMLTableCellElement[][] cells)
		{
			int colCount = 0;
			// find max column count
			for(int i = 0; i < cells.Length; i++) 
			{
				if(cells[i].Length > colCount) colCount = cells[i].Length;
			}
			return colCount;
		}

		/**
		 * grab all the cells from a table and return them in a 2 dimensional array
		 * TODO optimize (cache) cell.attribute* calls
		 */
		public static MathMLTableCellElement[][] GetCells(MathMLTableElement table)
		{
			int i = 0;
			MathMLNodeList tableRows = table.Rows;
			MathMLTableCellElement[][] cells = new MathMLTableCellElement[tableRows.Count][];	
			int[] remainderCols = new int[0];
			ArrayList rowCellsList = new ArrayList();

			foreach(MathMLTableRowElement row in tableRows)
			{				
				MathMLNodeList rowCells = row.Cells;
				rowCellsList.Clear();

				foreach(MathMLTableCellElement cell in rowCells)
				{
					if(rowCellsList.Count < remainderCols.Length && remainderCols[rowCellsList.Count] > 0)
					{
						remainderCols[rowCellsList.Count] = remainderCols[rowCellsList.Count] - 1;
						rowCellsList.Add(null);						
					}

					rowCellsList.Add(cell);

					for(int j = 1; j < cell.ColumnSpan; j++)
					{
						// deal with overlapping cells
						if(rowCellsList.Count < remainderCols.Length && remainderCols[rowCellsList.Count] > 0)
						{
							remainderCols[rowCellsList.Count] = remainderCols[rowCellsList.Count] - 1;				
						}
						rowCellsList.Add(null);
					}
				}

				cells[i] = new MathMLTableCellElement[rowCellsList.Count];
				for(int j = 0; j < rowCellsList.Count; j++)
				{
					cells[i][j] = (MathMLTableCellElement)rowCellsList[j];
				}

				if(remainderCols.Length < cells[i].Length)
				{
					int[] tmp = new int[cells[i].Length];
					for(int j = 0; j < remainderCols.Length; j++)
					{
						tmp[j] = remainderCols[j];
					}
					for(int j = remainderCols.Length; j < tmp.Length; j++)
					{
						tmp[j] = 0;
					}
					remainderCols = tmp;
				}

				for(int j = 0; j < cells[i].Length; j++)
				{
					if(cells[i][j] != null)
					{
						Debug.Assert(remainderCols[j] == 0, "remainder columns value should be zero if we have a current cell");
						remainderCols[j] = cells[i][j].RowSpan - 1;
					}					
				}				
				i++; // next row
			}
			return cells;
		}

		private static Row[] CreateRows(FormattingContext ctx, MathMLTableCellElement[][] cells, 
			BoundingBox[][] minCellSizes, Length[] rowSpacing, Length frameSpacing)
		{
			int i = 0;
			int space = 0;
			// new row array: row count + spacing rows + border spacing
			// =                 (row count) + (row count - 1) + 2
			// =                 (row count) + (row count) + 1
			Row[] rows = new Row[minCellSizes.Length + minCellSizes.Length + 1];

			// frame spacing
			rows[i++] = new Row(ctx.Evaluate(frameSpacing));

			for(int j = 0; j < minCellSizes.Length; j++)
			{
				// cell row
				rows[i++] = new Row(cells[j], minCellSizes[j]);

				// spacing row
				if(j < minCellSizes.Length - 1)
				{
					rows[i++] = new Row(ctx.Evaluate(rowSpacing[space++]));
				}
			}

			// final frame spacing
			rows[i++] = new Row(ctx.Evaluate(frameSpacing));

			// sanity check
			Debug.Assert(i == rows.Length, "error, row count and processed row count do not match");

			return rows;
		}

		/**
		 * calculate the required space to fit all the columns
		 */
		private static Column[] CreateColumns(FormattingContext ctx, MathMLTableCellElement[][] cells, 
			BoundingBox[][] minCellSizes, int columnCount, Length[] columnWidths, 
			Length[] spaceWidths, Length frameSpacing)
		{
			Column[] columns = null;		
			int c = 0;
			int colWidth = 0;
			int spaceWidth = 0;		
			MathMLMeasurer measurer = new MathMLMeasurer();  

			// new column array: max column count + spacing columns + border spacing
			// =                 (max column count) + (max column count - 1) + 2
			// =                 (max column count) + (max column count) + 1
			columns = new Column[columnCount + columnCount + 1];

			// set the frame spacing columns
			columns[c++] = new Column(ctx, frameSpacing, 0, true);

			// outer loop: columns
			for(int i = 0; i < columnCount; i++)
			{
				BoundingBox[] columnCells = new BoundingBox[cells.Length];
				// inner loop: rows
				for(int j = 0; j < cells.Length; j++)
				{
					columnCells[j] = i < cells[j].Length ? minCellSizes[j][i] : BoundingBox.New();
				}

				// set the column
				columns[c++] = new Column(ctx, columnWidths[colWidth++], GetRequiredWidth(columnCells), false);

				// set the space column
				if(i < columnCount - 1)
				{
					columns[c++] = new Column(ctx, spaceWidths[spaceWidth++], 0, true);
				}
			}

			// set the right spacing column
			columns[c++] = new Column(ctx, frameSpacing, 0, true);

			// sanity check
			Debug.Assert(c == columns.Length, "error, maxColumnCount and processed column count do not match");

			return columns;
		}

		/** 
		 * find the largest minimum width in a collection of cells
		 */
		public static float GetRequiredWidth(BoundingBox[] cells)
		{
			float min = 0;
			for(int i = 0; i < cells.Length; i++)
			{
				if(cells[i].Width > min) min = cells[i].Width;
			}
			return min;
		}

		private static void AdjustSpanningCells(MathMLTableCellElement[][] cells, 
			BoundingBox[][] minCellSizes, Row[] rows, Column[] columns)
		{
			// init to 1 to skip over first spacing row for frame 
			int rowIndex = 1;

            // outer loop - rows
			for(int i = 0; i < cells.Length; i++)
			{
				// init to 1 to skip over first spacing column for frame
				int colIndex = 1;	

                // inner loop - columns
				for(int j = 0; j < cells[i].Length; j++)
				{
					if(cells[i][j] != null)
					{
						/*
						int columnSpan = cells[i][j].ColumnSpan;
						if(columnSpan > 1)
						{
							float columnSpanWidth = columns[colIndex].Width;
							for(int k = 1; k < columnSpan; i++)
							{
								// add spaceing space (k starts at one)
								columnSpanWidth += columns[colIndex + 2 * k].Width;
								// add next column
								columnSpanWidth += columns[colIndex + 2 * k + 1].Width;
							}
							float reqWidth = minCellSizes[i][j].Width - columnSpanWidth;
							if(reqWidth > 0)
							{
							}
						}
						*/

                        int rowSpan = cells[i][j].RowSpan;
						if(rowSpan > 1)
						{
							//float rowSpanHeight = rows[rowIndex].Height;
							//float rowSpanDepth = rows[rowIndex].Depth;
							float rowSpanVertExt = 0;
							for(int k = 0; k < rowSpan; k++)
							{
								int k2 = 2 * k;
								if(k2 < rows.Length)
								{
									// content row
									rowSpanVertExt += rows[rowIndex + k2].VerticalExtent;

									// only add inner space rows 
									if(k < rowSpan - 1)
									{										
										rowSpanVertExt += rows[rowIndex + k2 + 1].VerticalExtent;	
									}
								}
								else
								{
									Debug.WriteLine("warning, row span exceeds row count");
								}
							}
							float reqVertExt = minCellSizes[i][j].VerticalExtent - rowSpanVertExt;
							if(reqVertExt > 0)
							{
								Debug.WriteLine("need " + reqVertExt + " to accomdate spanning row");
								float newSpace = reqVertExt / (float)rowSpan;
								for(int k = 0; k < rowSpan; k++)
								{
									int k2 = 2 * k;
									if(k2 < rows.Length)
									{
										rows[rowIndex + k2].Depth += newSpace / 2.0f;
										rows[rowIndex + k2].Height += newSpace / 2.0f;
									}
									else
									{
										Debug.WriteLine("warning, row span exceeds row count");
									}						
								}
							}
						}
					}

					// set index to next non space column
					colIndex += 2;
				}
				// set index to next non space row
				rowIndex += 2;
			}
		}

		private static void CreateCellSizes(MathMLTableCellElement[][] cells, 
			Row[] rows, Column[] columns, ref BoundingBox[][] sizes, ref PointF[][] shifts)
		{
			sizes = new BoundingBox[cells.Length][];
			shifts = new PointF[cells.Length][];

			// init to 1 to skip over first spacing row for frame 
			int rowIndex = 1;

			// y shift, start with upper frame space
			float y = rows[0].VerticalExtent;

			// outer loop - rows
			for(int i = 0; i < cells.Length; i++)
			{
				// init to 1 to skip over first spacing column for frame
				int colIndex = 1;
				// start x shift with frame space
				float x = columns[0].Width;

				sizes[i] = new BoundingBox[cells[i].Length];
				shifts[i] = new PointF[cells[i].Length];		

				// inner loop - columns
				for(int j = 0; j < cells[i].Length; j++)
				{
					if(cells[i][j] != null)
					{
						BoundingBox box = BoundingBox.New();

						for(int k = 0; k < cells[i][j].ColumnSpan; k++)
						{
							int k2 = 2 * k;
							if(k2 < columns.Length)
							{
								// add spaceing space (k starts at one)
								box.Append(BoundingBox.New(columns[colIndex + k2].Width, 0, 0));
								// add next column
								box.Append(BoundingBox.New(columns[colIndex + k2 + 1].Width, 0, 0));
							}
							else
							{
								Debug.WriteLine("warning, not enough columns for spanning");
							}
						}

						for(int k = 0; k < cells[i][j].RowSpan; k++)
						{
							int k2 = 2 * k;
							if(k2 < rows.Length)
							{
								if(k2 == 0)
								{
									box.Height = rows[rowIndex].Height;
									box.Depth = rows[rowIndex].Depth;
								}
								else
								{
									// add spaceing space (k starts at one)
									box.Over(BoundingBox.New(0, rows[rowIndex + k2].Height, rows[rowIndex + k2].Depth));
                                    // add next row
									box.Over(BoundingBox.New(0, rows[rowIndex + k2 + 1].Height, rows[rowIndex + k2 + 1].Depth));																
								}
							}
							else
							{
								Debug.WriteLine("warning, not enough rows for spanning");
							}
						}

						Debug.Assert(box.Height == rows[rowIndex].Height);

						sizes[i][j] = box;
						shifts[i][j] = new PointF(x, y + box.Height);
					}

					// add width of current column and next column which is a spaceing column 
					// and set index to next non space column
					x += columns[colIndex++].Width;
					x += columns[colIndex++].Width;
				}
				// add vertical extent of current row and next row which is a spaceing row 
				// and set index to next non space row
				y += rows[rowIndex++].VerticalExtent;
				y += rows[rowIndex++].VerticalExtent;
			}
		}


		/**
		 * initialize the dashed and solid lines
		 * pre-req verticalExtent is set
		 */
		private void CreateLines(BoundingBox[][] extents, MathMLTableCellElement[][] cells, 
			Row[] rows, Column[] columns, LineStyle[] columnLines, LineStyle[] rowLines, LineStyle frame)
		{
			ArrayList dashedLineList = new ArrayList();
			ArrayList solidLineList = new ArrayList();	

            float height = 0;
            float width = 0;
			int col = 0;
			int row = 0;

			// do the columns (vertical lines)
			for(int i = 0; i < columns.Length; i++)
			{				
				// the left side frame
				if(i == 0) 
				{
					if(frame == LineStyle.Solid)
					{
						solidLineList.Add(new PointF(0, 0));
						solidLineList.Add(new PointF(0, box.VerticalExtent));
					}
					else if(frame == LineStyle.Dashed)
					{
						dashedLineList.Add(new PointF(0, 0));
						dashedLineList.Add(new PointF(0, box.VerticalExtent));
					}
					width += columns[i].Width;
				}
				// the right side frame
				else if(i == columns.Length - 1) 
				{
					width += columns[i].Width;
					// shift the right most frame 1 pixel to the left because the last pixel 
					// is usually beyond the clip range.
					if(frame == LineStyle.Solid)
					{
						solidLineList.Add(new PointF(width - 1, 0));
						solidLineList.Add(new PointF(width - 1, box.VerticalExtent));
					}
					else if(frame == LineStyle.Dashed)
					{
						dashedLineList.Add(new PointF(width - 1, 0));
						dashedLineList.Add(new PointF(width - 1, box.VerticalExtent));
					}
				}
				// inter-column lines
				else 
				{
					if(columns[i].Spacing)
					{
						// place the line in the middle of the space
						width += columns[i].Width / 2;
						height = 0;
						float prevHeight = rows[0].VerticalExtent / 2.0f;

						row = 0;
						for(int j = 0; j < rows.Length; j++)
						{							
							if(rows[j].Spacing)
							{
								// standard rows
								if(j > 0 && j < rows.Length)
								{
									if(columnLines[col] == LineStyle.Dashed)
									{
										dashedLineList.Add(new PointF(width, height + rows[j].VerticalExtent / 2.0f));
										dashedLineList.Add(new PointF(width, prevHeight));
									}
									else if(columnLines[col] == LineStyle.Solid)
									{
										solidLineList.Add(new PointF(width, height + rows[j].VerticalExtent / 2.0f));
										solidLineList.Add(new PointF(width, prevHeight));
									}
									prevHeight = height + rows[j].VerticalExtent / 2.0f;
								}								
								//row++;
							}
							height += rows[j].VerticalExtent;							
						}
						// other half of space
						width += columns[i].Width / 2;
						col++;
					}
					else
					{
						width += columns[i].Width;
					}
				}
            }

			// do the rows (horz lines)
			height = 0;
			width = 0;
			col = 0;
			row = 0;
			for(int i = 0; i < rows.Length; i++)
			{
				// the top frame
				if(i == 0)
				{
					if(frame == LineStyle.Solid)
					{
						solidLineList.Add(new PointF(0, box.VerticalExtent - 1));
						solidLineList.Add(new PointF(box.HorizontalExtent, box.VerticalExtent - 1));
					}
					else if(frame == LineStyle.Dashed)
					{
						dashedLineList.Add(new PointF(0, box.VerticalExtent - 1));
						dashedLineList.Add(new PointF(box.HorizontalExtent, box.VerticalExtent - 1));
					}
					height += rows[i].VerticalExtent;
				}
				// the bottom frame
				else if(i == rows.Length - 1)
				{
					if(frame == LineStyle.Solid)
					{
						solidLineList.Add(new PointF(0, 0));
						solidLineList.Add(new PointF(box.HorizontalExtent, 0));
					}
					else if(frame == LineStyle.Dashed)
					{
						dashedLineList.Add(new PointF(0, 0));
						dashedLineList.Add(new PointF(box.HorizontalExtent, 0));
					}
				}
				// rows
				else
				{
					if(rows[i].Spacing)
					{
						// place the line in the middle of the space
						height += rows[i].VerticalExtent / 2.0f;
						width = 0;
						float prevWidth = columns[0].Width / 2.0f;
						col = 0;
						for(int j = 0; j < columns.Length; j++)
						{								
							if(j > 0 && columns[j].Spacing)
							{
								if(cells[row + 1][col] != null)
								{
									if(rowLines[row] == LineStyle.Dashed)
									{
										dashedLineList.Add(new PointF(width + columns[j].Width / 2.0f, height));
										dashedLineList.Add(new PointF(prevWidth, height));
									}
									else if(rowLines[row] == LineStyle.Solid)
									{
										solidLineList.Add(new PointF(width + columns[j].Width / 2.0f, height));
										solidLineList.Add(new PointF(prevWidth, height));
									}
								}
								prevWidth = width + columns[j].Width / 2.0f;
								col++;						
							}
							width += columns[j].Width;							
						}
						// other half of space
						height += rows[i].VerticalExtent / 2;
						row++;
					}
					else
					{
						height += rows[i].VerticalExtent;
					}
				}
			}
			dashedLines = (PointF[])dashedLineList.ToArray(typeof(PointF));
			solidLines = (PointF[])solidLineList.ToArray(typeof(PointF));
		}

		/**
		 * initialize the shift and vertical extent vars
		 * The final shift is calcuated by the formatter because it requires information
		 * about the formatted size of the cell area.
		 */
		private static void CalculateShiftAndSize(FormattingContext ctx, MathMLTableElement table, Row[] rows, 
			Column[] columns, ref BoundingBox box, ref float shift)
		{
			shift = 0;
			TableAlign align = table.Align;
			float verticalExtent = 0;
			float width = 0;
			for(int i = 0; i < rows.Length; i++)
			{
				verticalExtent += rows[i].VerticalExtent;
			}

			for(int i = 0; i < columns.Length; i++)
			{
				width += columns[i].Width;
			}

			shift = -verticalExtent;

			box = BoundingBox.New(width, verticalExtent, 0);
		}

		/**
		 * print the widths of the given columns
		 */
		[Conditional("DEBUG")]
		private void DebugWriteColumnWidths(String msg)
		{
			Debug.Write(msg);
			for(int i = 0; i < columns.Length; i++)
			{
				Debug.Write(columns[i].Width);
				Debug.WriteIf(i < columns.Length - 1, ", ");
			}
			Debug.WriteLine("");
		}

		[Conditional("DEBUG")]
		private void DebugWriteCellSizes(BoundingBox[][] minCellSizes)
		{
			Debug.WriteLine("Minimum Cell Sizes: ");
			for(int i = 0; i < minCellSizes.Length; i++)
			{
				for(int j = 0; j < minCellSizes[i].Length; j++)
					Debug.WriteLine("Min Cell Size for [" + i + "][" + j + "]: " + minCellSizes[i][j]);
			}
		}

		/**
		 * The columnwidth attribute specifies how wide a column should be. The "auto" value means 
		 * that the column should be as wide as needed, which is the default. If an explicit value 
		 * is given, then the column is exactly that wide and the contents of that column are made 
		 * to fit in that width. The contents are linewrapped or clipped at the discretion of the 
		 * renderer. If "fit" is given as a value, the remaining page width after subtracting the 
		 * widths for columns specified as "auto" and/or specific widths is divided equally among 
		 * the "fit" columns and this value is used for the column width. If insufficient room 
		 * remains to hold the contents of the "fit" columns, renderers may linewrap or clip the 
		 * contents of the "fit" columns. When the columnwidth is specified as a percentage, the 
		 * value is relative to the width of the table. That is, a renderer should try to adjust the 
		 * width of the column so that it covers the specified percentage of the entire table width.
		 * 
		 * Table width types:
		 * 1: Scale is the set of columns that have a width define as x%. These
		 * will be a percentage of the total width.
		 * 
		 * 2: Fix: These have a pre-defined width
		 * 
		 * 3:
		 */ 
		internal enum ColumnType
		{
			Auto, Fixed, Scale, Fit
		}

		/**
		 * measurments for a column of table cells
		 */
		internal struct Column
		{
			/**
			 * construct a new TableColumn object
			 */
			public Column(FormattingContext ctx, Length columnWidth, 
				float evaluatedWidth, bool spacing)
			{
				Spacing = spacing;	
				if(spacing)
				{
					if(columnWidth.Type == LengthType.Percentage) 
					{
						MinimumWidth = 0;
					}
					else if(columnWidth.Fixed)
					{
						MinimumWidth = ctx.Evaluate(columnWidth);
					}
					else
					{
						// this is bad
						throw new Exception("space columns must be either fixed or scaled widths");
					}
				}
				else
				{
					MinimumWidth = evaluatedWidth;
				}

				if(columnWidth.Type == LengthType.Auto)
				{
					Type = ColumnType.Auto;
					ScaleWidth = 0;
					FixedWidth = 0;
					Width = MinimumWidth;
				}
				else if(columnWidth.Type == LengthType.Fit)
				{
					Type = ColumnType.Fit;
					ScaleWidth = 0;
					FixedWidth = 0;
					Width = MinimumWidth;
				}
				else if(columnWidth.Type == LengthType.Percentage)
				{
					Type = ColumnType.Scale;
					ScaleWidth = columnWidth.Value / 100.0f;
					FixedWidth = 0;
					Width = MinimumWidth;					
				}
				else if(columnWidth.Fixed)
				{
					Type = ColumnType.Fixed;
					ScaleWidth = 0;
					FixedWidth = MinimumWidth;
					Width = MinimumWidth;
				}
				else
				{
					// this is bad
					throw new Exception(columnWidth.ToString() + " is not a valid length type for a table");
				}
			}

			public readonly ColumnType Type;

			/** 
			 * if the width is a percentage, than this value is the pecent value / 100, so
			 * 50% becomes .50
			 */
			public readonly float ScaleWidth;

			/**
			 * fixed width value type, only columns that are of type fixed width
			 * have this value set. If a column is not a fixed width type, this value
			 * is zero
			 */
			public readonly float FixedWidth;

			/**
			 * current width of this column, initialially set to min width
			 */
			public float Width;

			/**
			 * is this a spacing or a normal column
			 */
			public readonly bool Spacing;

			/**
			 * minimum width the contained areas can be set to
			 */
			public readonly float MinimumWidth;

			/**
			 * the areas that make up this column. 
			 */    
		}

		private struct Row 
		{
			/**
			 * construct a row that minimally encoloses a set of cell sizes
			 */
			public Row(MathMLTableCellElement[] cells, BoundingBox[] minCellSizes)
			{
				Spacing = false;
				Height = 0; 
				Depth = 0;

				for(int i = 0; i < minCellSizes.Length; i++)
				{
					// do not count spanning rows, these are adjusted later
					if(cells[i] != null)
					{			
						float h =  minCellSizes[i].Height / (float)cells[i].RowSpan;
						float d =  minCellSizes[i].Depth / (float)cells[i].RowSpan;

						if(h > Height) Height = h;
						if(d > Depth) Depth = d;
					}
				}
			}

			/**
			 * construct a spacing row
			 */
			public Row(float size)
			{
				Spacing = true;
				Height = size / 2.0f;
				Depth = size / 2.0f;
			}

			public readonly bool Spacing;
			public float Height;
			public float Depth;

			public float VerticalExtent
			{
				get { return Height + Depth; }
			}
		}
	}
}
