using System;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;
using System.Collections.Generic;

namespace UtilsSharp.Office
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// DataTable转Excel
        /// </summary>
        /// <param name="dt">datatable数据</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">sheet名称</param>
        public static void DataTableToExcel(DataTable dt, string filePath, string sheetName = "Sheet1")
        {
            IWorkbook workbook;
            if (Path.GetExtension(filePath) == ".xlsx")
                workbook = new XSSFWorkbook();
            else
                workbook = new HSSFWorkbook();

            ISheet sheet = workbook.CreateSheet(sheetName);
            int rowCount = 0;

            // 创建标题行并加粗字体
            IRow headerRow = sheet.CreateRow(rowCount++);
            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.BorderBottom = BorderStyle.Thin;
            headerStyle.BorderLeft = BorderStyle.Thin;
            headerStyle.BorderRight =BorderStyle.Thin;
            headerStyle.BorderTop = BorderStyle.Thin;
            headerStyle.Alignment = HorizontalAlignment.Center;
            //字体
            IFont headerfont = workbook.CreateFont();
            headerfont.Boldweight = (short)FontBoldWeight.Bold;
            headerStyle.SetFont(headerfont);

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var column = dt.Columns[i];
                var cell = headerRow.CreateCell(i);
                cell.SetCellValue(column.ColumnName);
                cell.CellStyle = headerStyle;
            }

            // 填充数据
            foreach (DataRow row in dt.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowCount++);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dataRow.CreateCell(i).SetCellValue(row[i].ToString());
                }
            }

            // 调整列宽
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            // 添加框线
            for (int i = 0; i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (row != null)
                    {
                        ICell cell = row.GetCell(j) ?? row.CreateCell(j);
                        ICellStyle style = workbook.CreateCellStyle();
                        style.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                        style.BorderBottom = BorderStyle.Thin;
                        style.BorderLeft = BorderStyle.Thin;
                        style.BorderRight = BorderStyle.Thin;
                        style.BorderTop = BorderStyle.Thin;
                        cell.CellStyle = style;
                    }
                }
            }

            // 保存到文件
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }

        /// <summary>
        /// Excel转DataTable
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">sheet名称</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filePath, string sheetName = "Sheet1")
        {
            IWorkbook workbook;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (Path.GetExtension(filePath) == ".xlsx")
                    workbook = new XSSFWorkbook(fs);
                else
                    workbook = new HSSFWorkbook(fs);
            }

            ISheet sheet = workbook.GetSheet(sheetName);
            DataTable dt = new DataTable();

            // 读取标题行
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int i = 0; i < cellCount; i++)
            {
                ICell cell = headerRow.GetCell(i);
                string columnName = (cell == null) ? "" : cell.ToString();
                dt.Columns.Add(columnName);
            }

            // 读取数据行
            int rowCount = sheet.LastRowNum;
            for (int i = 1; i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = row?.GetCell(j);
                    dataRow[j] = (cell == null) ? "" : cell.ToString();
                }
                dt.Rows.Add(dataRow);
            }

            return dt;
        }

        /// <summary>
        /// List转Excel
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="list">对象列表</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">sheet名称</param>
        public static void ListToExcel<T>(List<T> list, string filePath, string sheetName = "Sheet1")
        {
            DataTable dt = ToDataTable(list);
            DataTableToExcel(dt, filePath, sheetName);
        }

        /// <summary>
        /// Excel转List
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="sheetName">sheet名称</param>
        /// <returns></returns>
        public static List<T> ExcelToList<T>(string filePath, string sheetName = "Sheet1") where T : new()
        {
            DataTable dt = ExcelToDataTable(filePath, sheetName);
            return ToList<T>(dt);
        }

        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="list">对象列表</param>
        /// <returns></returns>
        private static DataTable ToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (var prop in typeof(T).GetProperties())
            {
                dt.Columns.Add(prop.Name);
            }

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();

                foreach (var prop in typeof(T).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="dt">datatable数据</param>
        /// <returns></returns>
        private static List<T> ToList<T>(DataTable dt) where T : new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                T item = new T();

                foreach (var prop in typeof(T).GetProperties())
                {
                    if (dt.Columns.Contains(prop.Name))
                    {
                        prop.SetValue(item, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                    }
                }

                list.Add(item);
            }

            return list;
        }
    }
}
