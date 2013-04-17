using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ModulosTIControlador.Clases
{
    public class GenExcell
    {
        StreamWriter w;

        public int DoExcell(string ruta, List<List<object>> resultado)
        {
            FileStream fs = new FileStream(ruta, FileMode.Create, FileAccess.ReadWrite);
            w = new StreamWriter(fs);
            EscribeCabeceraCSV();

            for (int i = 0; i < resultado.Count; i++)
            {
                String login = resultado[i].ElementAt(0).ToString();
                String carrera = resultado[i].ElementAt(1).ToString();
                String fecha = resultado[i].ElementAt(2).ToString();
                EscribeLineaCSV(i, login, carrera, fecha);
            }

            //EscribePiePagina();
            w.Close();
            return 0;
        }

        public void EscribeCabecera()
        {
            StringBuilder html = new StringBuilder();
            html.Append("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">");
            html.Append("<html>");
            html.Append("  <head>");
            html.Append("<title>www.devjoker.com</title>");
            html.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
            html.Append("  </head>");
            html.Append("<body>");
            html.Append("<p>");
            html.Append("<table>");
            html.Append("<tr style=\"font-weight: bold;font-size: 14px;color: white;\">");
            html.Append("<td bgcolor=\"Blue\">Login</td>");
            html.Append("<td bgcolor=\"Blue\">Carrera</td>");
            html.Append("<td bgcolor=\"Blue\">Fecha de Creación</td>");
            html.Append("</tr>");

            w.Write(html.ToString());
        }

        public void EscribeCabeceraCSV()
        {
            w.Write("Login ; Carrera ; Fecha de Creacion ;\n");
        }

        public void EscribeLinea(int i, String login, String carrera, String FechaCreacion)
        {
            string bgColor = "", fontColor = "";
            if (i % 2 == 0)
            {
                bgColor = " bgcolor=\"LightBlue\" ";
                fontColor = " style=\"color: white;\" ";
            }
            w.Write(@"<tr><td {3} {4}>{0}</td><td {3} {4}>{1}</td><td {3} {4}>{2}</td></tr>\n", login, carrera, FechaCreacion, bgColor, fontColor);
        }

        public void EscribeLineaCSV(int i, String login, String carrera, String FechaCreacion)
        {
            w.Write(login + " ; " + carrera + " ; " + FechaCreacion +" ;\n");
        }

        public void EscribePiePagina()
        {
            StringBuilder html = new StringBuilder();
            html.Append("  </table>");
            html.Append("</p>");
            html.Append(" </body>");
            html.Append("</html>");
            w.Write(html.ToString());
        }

    }

}
