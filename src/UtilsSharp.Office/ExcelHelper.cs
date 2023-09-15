using System;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
 

namespace UtilsSharp.Office
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public class ExcelHelper : IDisposable
    {
        private string fileALLPath = "";//Excel物理路径（绝对路径）
        private IWorkbook workbook = null;//使用NPOI初始化的Excel工作簿
        private FileStream fs = null;//Excel文件流
        private bool disposed;
        private string rowsCode = "<%ROWS:{0}%>";//数据行绑定码

        public ExcelHelper()
        {
            disposed = false;
        }

        public ExcelHelper(string fileALLPath)
        {
            this.fileALLPath = fileALLPath;
            disposed = false;
            //使用NPOI初始化的Excel工作簿
            this.fs = new FileStream(fileALLPath, FileMode.Open, FileAccess.Read);
            if (fileALLPath.IndexOf(".xlsx") > 0)
            {
                workbook = new XSSFWorkbook(fs);
            }
            else if (fileALLPath.IndexOf(".xls") > 0)
            {
                workbook = new HSSFWorkbook(fs);
            }
        }

        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;

            fs = new FileStream(fileALLPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileALLPath.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileALLPath.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }

                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                workbook.Write(fs); //写入到excel
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileALLPath, FileMode.Open, FileAccess.Read);
                if (fileALLPath.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileALLPath.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }

                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                cell.SetCellType(CellType.String);//设置为String
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取Excel中Sheet名称集合
        /// </summary>
        /// <returns></returns>
        public string[] SheetNames()
        {
            fs = new FileStream(fileALLPath, FileMode.Open, FileAccess.Read);
            if (fileALLPath.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook(fs);
            else if (fileALLPath.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook(fs);
            int sheetCount = workbook.NumberOfSheets;
            string[] sheetName = new string[sheetCount];//保存表的名称
            for (int i = 0; i < sheetCount; i++)
            {
                sheetName[i] = workbook.GetSheetName(i);
            }
            return sheetName;
        }

        /// <summary>
        /// 删除指定的Sheets
        /// </summary>
        public void RemoveSheetsByNames(string[] sheetNames)
        {
            foreach (string sheetName in sheetNames)
            {
                ISheet sheet = workbook.GetSheet(sheetName);
                int index = workbook.GetSheetIndex(sheet);
                workbook.RemoveSheetAt(index);
            }
        }

        /// <summary>
        /// 保存修改后的excel
        /// </summary>
        public void Save()
        {
            FileStream fileSave = new FileStream(fileALLPath, FileMode.Open, FileAccess.Write);
            workbook.Write(fileSave);
            fileSave.Close();
        }

        /// <summary>
        /// 填充Sheet
        /// </summary>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="dt">数据源（DataTable）</param>
        /// <param name="isList">填充类型："对象"还是"集合",true:"集合",false:"对象"</param>
        /// <param name="dtName">DataTable TableName</param>
        /// <param name="isAlarm">是否是：业务风险提示</param>
        /// <param name="rowIndex">遍历Excel范围：行数，默认30行</param>
        /// <param name="colIndex">遍历Excel范围：列数，默认20列</param>
        public void FillSheet(string sheetName, DataTable dt, bool isList, string dtName = "", bool isAlarm = false, int rowIndex = 30, int colIndex = 20)
        {
            if (dt == null || dt.Rows.Count <= 0)
                return;

            ISheet sheet = workbook.GetSheet(sheetName);
            if (sheet == null)
                return;

            if (!isList)
            {
                //"对象填充"
                foreach (DataColumn column in dt.Columns)
                {
                    for (int rowIn = 0; rowIn < rowIndex; rowIn++)
                    {
                        for (int colIn = 0; colIn < colIndex; colIn++)
                        {
                            IRow row = sheet.GetRow(rowIn);
                            if (row == null)
                                continue;
                            ICell cell = row.GetCell(colIn);
                            if (cell == null)
                                continue;

                            if (string.IsNullOrEmpty(dtName))
                            {
                                dtName = dt.TableName;
                            }
                            string a = string.Empty;
                            if (isAlarm)
                            {
                                a = "$" + column.ColumnName;
                            }
                            else
                            {
                                a = "<%" + dtName + ";" + column.ColumnName + "%>";
                            }
                            if (cell.ToString() == a)
                            {
                                var value = dt.Rows[0][column.ColumnName];

                                if (value == DBNull.Value || value == null)
                                {
                                    cell.SetCellValue(string.Empty);
                                }
                                else
                                {
                                    if (column.DataType.FullName == "System.DateTime")
                                    {
                                        cell.SetCellValue(Convert.ToDateTime(value).ToString("yyyy-MM-dd"));
                                    }
                                    else if (column.DataType.FullName == "System.Decimal")
                                    {
                                        cell.SetCellValue(double.Parse(Convert.ToDecimal(value).ToString("N2")));
                                    }
                                    else
                                    {
                                        cell.SetCellValue(value.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //"集合"填充
                ICellStyle style = workbook.CreateCellStyle();
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                        cell.CellStyle = style;
                    }
                }
            }
            sheet.ForceFormulaRecalculation = true;//刷新单元格公式，自动计算
        }

        /// <summary>
        /// 填充Sheet数据（对多Table数据填充）
        /// </summary>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="tableNum">table个数</param>
        /// <param name="tableNames">table的表名（用于匹配数据源中的table和数据模板中数据绑定码</param>
        /// <param name="cellKeys">table的数据列名（用于匹配数据源中对应列数据），如：{{"fieldName1,fliedName2"},{"fieldName1,fieldName2,fieldName3"}},字段名之间使用 , 分割</param>
        /// <param name="dts">数据源</param>
        /// <param name="cellCount">数据列数，默认为0，取第一行的数据列数</param>
        /// <param name="isSum">是否求和，默认不求和。如果为true，会搜索“合计”行，把数据类型为decimal的列求和</param>
        public void FillSheetEx(string sheetName, int tableNum, string[] tableNames, string[] cellKeys, DataSet dts, int cellCount = 0, bool isSum = false)
        {
            if (dts == null || dts.Tables.Count <= 0)
            {
                return;
            }

            ISheet sheet = workbook.GetSheet(sheetName);
            if (sheet == null)
            {
                return;
            }

            if (sheet != null)
            {
                if (cellCount == 0)
                {
                    cellCount = sheet.GetRow(0).LastCellNum;//列数
                }
                int searchRow = 0;//搜索开始行
                int startRow = 0;
                int startCell = 0;
                string content = "";
                for (int i = 0; i < tableNum; i++)
                {
                    content = string.Format(rowsCode, tableNames[i]);//数据绑定码
                    MatchSheetContent(sheet, content, out startRow, out startCell, cellCount, 5, searchRow);//获取插入数据的行和列
                    if (startRow > 0)
                    {
                        sheet.GetRow(startRow).GetCell(startCell).SetCellValue("");//清空数据绑定码信息
                        InsertSheetData(sheet, dts.Tables[tableNames[i]], startRow, dts.Tables[tableNames[i]].Columns.Count, cellKeys[i].Split(','), isSum);//插入数据

                        searchRow = startRow + dts.Tables[tableNames[i]].Rows.Count + 3; //3：表和表之间隔2行
                        if (isSum)
                        {
                            searchRow = searchRow + 1;//合计的1行
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            sheet.ForceFormulaRecalculation = true;//刷新单元格公式，自动计算
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (fs != null)
                        fs.Close();
                }

                fs = null;
                disposed = true;
            }
        }

        #region 其他成员方法
        /// <summary>
        /// 在sheet中的某个单元格设置计算函数（会导致Excel文件错误，方法待优化）
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="rowIndex">设置单元格行索引</param>
        /// <param name="cellIndex">设置单元格列索引</param>
        /// <param name="operatorCode">函数名</param>
        /// <param name="startRowIndex">开始行索引</param>
        /// <param name="startCellIndex">开始列索引</param>
        /// <param name="endRowIndex">结束行索引</param>
        /// <param name="endCellIndex">结束列索引</param>
        ///  <param name="dataFormat">默认：194：数值格式</param>
        public void OperatorFormula(string sheetName, int rowIndex, int cellIndex, string operatorCode, int startRowIndex, int startCellIndex, int endRowIndex, int endCellIndex, short dataFormat = 194)
        {
            ISheet sheet = workbook.GetSheet(sheetName);
            if (sheet == null)
            {
                return;
            }
            string strFormula = GetOperatorFormula(operatorCode, startRowIndex, startCellIndex, endRowIndex, endCellIndex);
            ICell cell = sheet.GetRow(rowIndex).GetCell(cellIndex);
            cell.CellStyle.DataFormat = dataFormat;
            cell.SetCellType(CellType.Formula);
            cell.SetCellFormula(strFormula);
        }
        #endregion

        #region 扩展方法

        /// <summary>
        /// 创建单元格样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="type">单元格类型</param>
        /// <param name="fontHeightInPoints">字号，传0为默认字号</param>
        /// <param name="foregroundColor">背景色（可通过NPOI.HSSF.Util.HSSFColor获取色值）</param>
        /// <returns></returns>
        public static ICellStyle CreateCellStyle(HSSFWorkbook workbook, SheetCellType type, short fontHeightInPoints = 0, short foregroundColor = 0)
        {
            ICellStyle cellStyle = workbook.CreateCellStyle();
            switch (type.ToString())
            {
                case "CellTitle":
                    #region 标题样式
                    //设置单元格边框
                    cellStyle.BorderBottom = BorderStyle.Thin;
                    cellStyle.BorderLeft = BorderStyle.Thin;
                    cellStyle.BorderRight = BorderStyle.Thin;
                    cellStyle.BorderTop = BorderStyle.Thin;
                    //设置单元格边框颜色
                    cellStyle.BottomBorderColor = HSSFColor.Black.Index;
                    cellStyle.LeftBorderColor = HSSFColor.Black.Index;
                    cellStyle.RightBorderColor = HSSFColor.Black.Index;
                    cellStyle.TopBorderColor = HSSFColor.Black.Index;
                    //设置水平垂直居中
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    //设置单元格背景颜色
                    if (foregroundColor > 0)
                    {
                        cellStyle.FillForegroundColor = foregroundColor;
                        cellStyle.FillPattern = FillPattern.SolidForeground;//填充背景样式
                    }
                    //新建一个字体样式对象
                    IFont titleFont = workbook.CreateFont();
                    //字体大小13.5pt,对应像素18px
                    titleFont.FontHeightInPoints = 16;
                    //设置字体加粗样式
                    titleFont.Boldweight = short.MaxValue;
                    //使用SetFont方法将字体样式添加到单元格样式中 
                    cellStyle.SetFont(titleFont);
                    #endregion
                    break;
                case "CellField":
                    #region 字段名样式
                    //设置单元格边框
                    cellStyle.BorderBottom = BorderStyle.Thin;
                    cellStyle.BorderLeft = BorderStyle.Thin;
                    cellStyle.BorderRight = BorderStyle.Thin;
                    cellStyle.BorderTop = BorderStyle.Thin;
                    //设置单元格边框颜色
                    cellStyle.BottomBorderColor = HSSFColor.Black.Index;
                    cellStyle.LeftBorderColor = HSSFColor.Black.Index;
                    cellStyle.RightBorderColor = HSSFColor.Black.Index;
                    cellStyle.TopBorderColor = HSSFColor.Black.Index;
                    //设置垂直水平居中
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    //设置单元格背景颜色
                    if (foregroundColor > 0)
                    {
                        cellStyle.FillForegroundColor = foregroundColor;
                        cellStyle.FillPattern = FillPattern.SolidForeground;//填充背景样式
                    }
                    IFont FiledFont = workbook.CreateFont();
                    //字体大小10.5pt,对应像素14px
                    FiledFont.FontHeightInPoints = fontHeightInPoints <= 0 ? (short)10 : fontHeightInPoints;
                    //设置字体加粗样式
                    FiledFont.Boldweight = short.MaxValue;
                    cellStyle.SetFont(FiledFont);
                    #endregion
                    break;
                case "CellValue":
                    #region 数据值样式
                    //设置单元格边框
                    cellStyle.BorderBottom = BorderStyle.Thin;
                    cellStyle.BorderLeft = BorderStyle.Thin;
                    cellStyle.BorderRight =BorderStyle.Thin;
                    cellStyle.BorderTop = BorderStyle.Thin;
                    //设置单元格边框颜色
                    cellStyle.BottomBorderColor = HSSFColor.Black.Index;
                    cellStyle.LeftBorderColor = HSSFColor.Black.Index;
                    cellStyle.RightBorderColor = HSSFColor.Black.Index;
                    cellStyle.TopBorderColor = HSSFColor.Black.Index;
                    //设置垂直水平居中
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    //设置单元格背景颜色
                    if (foregroundColor > 0)
                    {
                        cellStyle.FillForegroundColor = foregroundColor;
                        cellStyle.FillPattern = FillPattern.SolidForeground;//填充背景样式
                    }
                    IFont valueFont = workbook.CreateFont();
                    //字体大小10.5pt,对应像素14px
                    valueFont.FontHeightInPoints = fontHeightInPoints <= 0 ? (short)9 : fontHeightInPoints;
                    cellStyle.SetFont(valueFont);
                    #endregion
                    break;
            }
            return cellStyle;
        }

        /// <summary>
        /// 合并及填充单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="firstRow">第一行的index</param>
        /// <param name="lastRow">最后一行的index</param>
        /// <param name="firstCol">第一列的index</param>
        /// <param name="lastCol">最后一列的index</param>
        /// <param name="content">填充内容</param>
        /// <param name="cellHeight">单元格行高,默认高设置为0（标题一般传值：23.5f）</param>
        /// <param name="cellStyle">单元格样式接口实例</param>
        public static void MergeCell(HSSFSheet sheet, int firstRow, int lastRow, int firstCol, int lastCol, string content = "", float cellHeight = 0, ICellStyle cellStyle = null)
        {
            CellRangeAddress cellRange = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            sheet.AddMergedRegion(cellRange);

            //填充内容
            if (!string.IsNullOrWhiteSpace(content))
            {
                IRow row = sheet.CreateRow(firstRow);
                row.CreateCell(firstCol).SetCellValue(content);
                //对应高度30px
                if (cellHeight > 0)
                {
                    row.Height = (short)(cellHeight * 20);
                }
            }

            //合并单元格的样式
            for (int i = cellRange.FirstRow; i <= cellRange.LastRow; i++)
            {
                IRow row = HSSFCellUtil.GetRow(i, sheet);
                for (int j = cellRange.FirstColumn; j <= cellRange.LastColumn; j++)
                {
                    ICell singleCell = HSSFCellUtil.GetCell(row, (short)j);
                    if (cellStyle != null)
                    {
                        singleCell.CellStyle = cellStyle;
                    }
                }
            }
        }

        /// <summary>
        /// 设置表格自适应宽度
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="cellNum">表格列数</param>
        /// <param name="multiple">单元格显示宽度倍数（如1.5：是内容宽度的1.5倍）,默认和内容宽度一致</param>
        /// <param name="defaultLen">数据为空时，单元格的默认填充长度：2</param>
        public static void SetAdaptiveWidth(HSSFSheet sheet, int cellNum, double multiple = 1, int defaultLen = 2)
        {
            int width = defaultLen * 1024;//空数据指定宽度
            int times = Convert.ToInt32(multiple * 10);
            for (int i = 0; i <= cellNum; i++)
            {
                sheet.AutoSizeColumn(i);
                if (sheet.GetColumnWidth(i) >= 2048)
                {
                    width = sheet.GetColumnWidth(i);
                }
                sheet.SetColumnWidth(i, width * times / 10);
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 向Sheet的指定行中插入数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="dt">数据源</param>
        /// <param name="startRowIndex">开始插入数据的行索引</param>
        /// <param name="cellCount">数据列数</param>
        /// <param name="cellKeys">列匹配关键字</param>
        /// <param name="isSum">是否求和，默认不求和。如果为true，会搜索“合计”行，把数据类型为decimal的列求和</param>
        private void InsertSheetData(ISheet sheet, DataTable dt, int startRowIndex, int cellCount, string[] cellKeys, bool isSum = false)
        {
            int firstDataRow = startRowIndex;
            sheet.ShiftRows(startRowIndex, sheet.LastRowNum, dt.Rows.Count - 1, true, false);
            string dataType = "";
            IRow row;
            for (int i = 0, len = dt.Rows.Count; i < len; i++)
            {
                row = sheet.CreateRow(startRowIndex);
                //创建列并插入数据
                for (int index = 0; index < cellCount; index++)
                {
                    ICellStyle cellStyle = sheet.GetRow(startRowIndex - 1).GetCell(i).CellStyle;//获取上一行的数据格式
                    dataType = dt.Rows[i][cellKeys[index]].GetType().ToString();
                    if (dataType == "System.DBNull")
                    {
                        row.CreateCell(index).SetCellValue("");
                    }
                    else if (dataType == "System.Decimal")
                    {
                        row.CreateCell(index).SetCellValue(double.Parse(Convert.ToDecimal(dt.Rows[i][cellKeys[index]]).ToString("N2")));
                    }
                    else if (dataType == "System.DateTime")
                    {
                        row.CreateCell(index).SetCellValue(Convert.ToDateTime(dt.Rows[i][cellKeys[index]]).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        row.CreateCell(index).SetCellValue(!(dt.Rows[i][cellKeys[index]] is DBNull) ? dt.Rows[i][cellKeys[index]].ToString() : string.Empty);
                    }
                    row.GetCell(index).CellStyle = cellStyle;
                }
                startRowIndex = startRowIndex + 1;
            }

            if (isSum)//求和
            {
                int startRow = 0;
                int startCell = 0;
                MatchSheetContent(sheet, "合计", out startRow, out startCell, cellCount, 5, startRowIndex);
                if (startRow > 0)
                {
                    IRow sumRow = sheet.GetRow(startRow);
                    for (int index = 0; index < dt.Columns.Count; index++)
                    {
                        if (sumRow.GetCell(index).CellType == CellType.Formula)//格式为表达式，求和
                        {
                            sumRow.GetCell(index).SetCellFormula(GetOperatorFormula("SUM", firstDataRow, index, startRow - 1, index));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取Excel简单操作函数
        /// </summary>
        /// <param name="operatorCode">函数名，如：SUM</param>
        /// <param name="startRowIndex">开始行索引</param>
        /// <param name="startCellIndex">开始列索引</param>
        /// <param name="endRowIndex">结束行索引</param>
        /// <param name="endCellIndex">结束列索引</param>
        /// <returns></returns>
        private string GetOperatorFormula(string operatorCode, int startRowIndex, int startCellIndex, int endRowIndex, int endCellIndex)
        {
            string[,] code = { { "A" }, { "B" }, { "C" }, { "D" }, { "E" }, { "F" }, { "G" }, { "H" }, { "I" }, { "J" }, { "K" }, { "L" }, { "M" }, { "N" }, { "O" }, { "P" }, { "Q" }, { "R" }, { "S" }, { "T" }, { "U" }, { "V" }, { "W" }, { "X" }, { "Y" }, { "Z" } };
            string start = string.Format("{0}{1}", code[startCellIndex, 0], startRowIndex + 1);
            string end = string.Format("{0}{1}", code[endCellIndex, 0], endRowIndex + 1);

            return string.Format("{0}({1}:{2})", operatorCode, start, end);
        }

        /// <summary>
        /// 搜索Sheet中匹配信息的单元格所在行和列
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="matchContent">匹配信息</param>
        /// <param name="whichRow">返回信息：哪一行</param>
        /// <param name="whichCell">返回信息：哪一列</param>
        /// <param name="searchCells">搜索Sheet表格宽度，为0时默认取第一行的宽度</param>
        /// <param name="searchRows">搜索的行数，默认搜索5行</param>
        /// <param name="formWhichRow">从第几行开始搜索，默认从第一行</param>
        private void MatchSheetContent(ISheet sheet, string matchContent, out int whichRow, out int whichCell, int searchCells = 0, int searchRows = 5, int formWhichRow = 0)
        {
            whichRow = -1;
            whichCell = -1;
            IRow firstRow = sheet.GetRow(0);
            if (searchCells == 0)
            {
                searchCells = firstRow.LastCellNum;//一行最后一个cell的编号 即总的列数
            }

            //遍历Sheet,找到需要填充数据源的行位置
            IRow currentRow;
            ICell currentCell;
            bool isFinish = false;
            for (int i = formWhichRow; i < formWhichRow + searchRows; i++)
            {
                currentRow = sheet.GetRow(i);
                for (int j = firstRow.FirstCellNum; j < searchCells; j++)
                {
                    currentCell = currentRow.GetCell(j);
                    if (currentCell != null && currentCell.CellType == CellType.String)//仅检测字符串类型的单元格
                    {
                        string cellValue = currentCell.StringCellValue;
                        if (cellValue == matchContent)//找到需要填充数据的行
                        {
                            whichCell = currentCell.ColumnIndex;
                            whichRow = currentCell.RowIndex;
                            isFinish = true;
                            break;
                        }
                    }
                }

                if (isFinish)
                {
                    break;
                }
            }
        }
        #endregion
    }
}
