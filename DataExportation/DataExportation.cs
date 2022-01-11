using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace DataExportation
{
    public class DataExportation
    {
        /// <summary>
        /// 输出数据为.xlsx文件，不支持旧版本excel
        /// </summary>
        /// <param name="path">文件路径，包括文件名和后缀</param>
        /// <param name="data">计算数据</param>
        public static void ExportAsXlsx(string path, List<List<double>> data)
        {
            XSSFWorkbook workbook = new();
            var sheet = workbook.CreateSheet();
            string[] Head = new string[] { "傅汝德数", "兴波阻力", "兴波阻力系数" };
            for (int i = 0; i < data.Count; i++)
            {
                var row = sheet.CreateRow(i);
                row.CreateCell(0).SetCellValue(Head[i]);
                for (int j = 0; j < data[i].Count; j++)
                {
                    row.CreateCell(j + 1).SetCellValue(data[i][j]);
                }
            }
            Stream excelStream = File.Create(path);
            workbook.Write(excelStream);
            workbook.Close();
            excelStream.Dispose();
        }
        /// <summary>
        /// 输出数据为.xls文件，兼容旧版本excel
        /// </summary>
        /// <param name="path">文件路径，包括文件名和后缀</param>
        /// <param name="data">计算数据</param>
        public static void ExportAsXls(string path, List<List<double>> data)
        {
            HSSFWorkbook workbook = new();
            var sheet = workbook.CreateSheet();
            string[] Head = new string[] { "傅汝德数", "兴波阻力", "兴波阻力系数" };
            for (int i = 0; i < data.Count; i++)
            {
                var row = sheet.CreateRow(i);
                row.CreateCell(0).SetCellValue(Head[i]);
                for (int j = 0; j < data[i].Count; j++)
                {
                    row.CreateCell(j + 1).SetCellValue(data[i][j]);
                }
            }
            Stream excelStream = File.Create(path);
            workbook.Write(excelStream);
            workbook.Close();
            excelStream.Dispose();
        }
        /// <summary>
        /// 输出数据为.CSV文件，使用UTF-8编码
        /// </summary>
        /// <param name="path">文件路径，包括文件名和后缀</param>
        /// <param name="data">计算数据</param>
        public static void ExportAsCSV(string path, List<List<double>> data)
        {
            Stream fileStream = File.Create(path);
            string[] RowHead = new string[] { "傅汝德数", "兴波阻力", "兴波阻力系数" };
            StreamWriter writer = new(fileStream);
            for (int i = 0; i < data.Count; i++)
            {
                writer.Write(RowHead[i]);
                for (int j = 0; j < data[i].Count; j++)
                {
                    writer.Write(",");
                    writer.Write(data[i][j]);
                }
                writer.Write("\n");
            }
            writer.Close();
            fileStream.Dispose();
        }
    }
}