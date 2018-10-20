using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BasisFrameWork.Extension
{
    public static partial class DataExtension
    {
        /// <summary>
        /// 导出Excel内存流
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MemoryStream Export(this DataTable table) {
            var ms = new MemoryStream();
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet();

            IDataFormat dataformat = workbook.CreateDataFormat();
            ICellStyle style1 = workbook.CreateCellStyle();
            style1.DataFormat = dataformat.GetFormat("text");
            var headerRow = sheet.CreateRow(0);
            // handling header.
            foreach (DataColumn column in table.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);

            //If Caption not set, returns the ColumnName value
            // handling value.
            int rowIndex = 1;
            foreach (DataRow row in table.Rows) {
                var dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in table.Columns) {
                    var columnValue = row[column].ToString();
                    if (column.DataType == typeof(Enum)
                        || column.DataType.BaseType == typeof(Enum)) {
                        columnValue = column.DataType.GetDescription((int)row[column]);
                    }

                    var cell = dataRow.CreateCell(column.Ordinal);
                    cell.CellStyle = style1;
                    cell.SetCellValue(columnValue);
                }
                rowIndex++;
            }

            workbook.Write(ms);

            var returnStream = new System.IO.MemoryStream(ms.ToArray());
            return returnStream;
        }

        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<bool> ToExcelFile(this MemoryStream source, string fileName) {
            if (source == null)
                return false;

            using (FileStream fs = new FileStream(@fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)) {
                var buffer = source.ToArray();
                await fs.WriteAsync(buffer, 0, buffer.Length);
            }

            return File.Exists(fileName);
        }

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IList list) {
            DataTable rc = new DataTable();

            if (list != null && list.Count > 0) {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                //排除自定义的类型
                List<string> notInPropertyNames = new List<string>();
                foreach (PropertyInfo pi in propertys) {
                    if (pi.PropertyType.IsClass && !pi.PropertyType.IsPrimitive && pi.PropertyType.Name.ToLower() != "string") {
                        notInPropertyNames.Add(pi.Name);
                        continue;
                    }

                    if (pi.PropertyType.IsGenericType)
                        rc.Columns.Add(pi.Name, pi.PropertyType.GetGenericArguments()[0]);
                    else
                        rc.Columns.Add(pi.Name, pi.PropertyType);
                }
                for (int i = 0; i < list.Count; i++) {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys) {
                        if (notInPropertyNames.Contains(pi.Name))
                            continue;
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    rc.LoadDataRow(array, true);
                }
            }

            return rc;
        }

    }
}
