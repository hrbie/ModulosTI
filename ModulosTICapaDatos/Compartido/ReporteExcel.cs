using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaDatos.Compartido
{
    public class ReporteExcel
    {
        public SpreadsheetGear.IWorkbook generarReporte(string nombreHoja, List<string> titulos, List<List<string>> contenido)
        {
            // Create a new workbook.
            SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook();
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets["Sheet1"];
            SpreadsheetGear.IRange cells = worksheet.Cells;

            // Set the worksheet name.
            if (nombreHoja.Length > 31)
                worksheet.Name = nombreHoja.Replace('/', '-').Substring(0, 31);
            else
                worksheet.Name = nombreHoja.Replace('/', '-');
            
            string ultimaColumna = "";
            int tituloIndex = 1;
            // Load column titles.
            for (char c = 'A'; tituloIndex <= titulos.Count(); c++)
            {
                cells[c.ToString() + "1"].Formula = titulos[tituloIndex - 1];

                if (tituloIndex == titulos.Count())
                {
                    ultimaColumna = c.ToString();
                }
                tituloIndex++;
            }
            //centra los titulos del reporte
            cells["A1:" + ultimaColumna + "1"].HorizontalAlignment = SpreadsheetGear.HAlign.Center;

            //carga el contenido del reporte
            for (int i = 0; i < contenido.Count; i++)
            {
                for (int j = 0; j < contenido[i].Count; j++)
                {
                    // 65 = 'A', 66 = 'B', etc. Empieza en la A2, B2, C2 ... y luego cambia de fila
                    string celda = (char)(j + 65) + (i + 2).ToString();
                    cells[celda].Formula = contenido[i][j];
                }
            }

            cells["A1:" + ultimaColumna + "100"].Columns.AutoFit();

            // Stream the Excel spreadsheet to the client in a format
            // compatible with Excel 97/2000/XP/2003/2007/2010.

            return workbook;
        }
    }
}
