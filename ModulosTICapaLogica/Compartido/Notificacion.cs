using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace ModulosTICapaLogica.Compartido
{
    public class Notificacion
    {
        #region Constructor

        public Notificacion()
        {
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que se encarga de enviar un correo a algún usuario especificado
        /// </summary>
        /// <param name="mensaje">Cuerpo del correo</param>
        /// <param name="correo">Correo del usuario al que se quiere notificar</param>
        /// <param name="asunto">Asunto del correo</param>
        /// <returns>Retorna true en caso de éxito, false en caso contrario</returns>

        //revisar q se recibe xq no envia correo y en la prueba unitaria si
        public Boolean enviarCorreo(string mensaje, string correo, string asunto)
        {
           
            MailMessage message;
            SmtpClient clienteSmtp;
           
            message = new MailMessage();
            message.From = new MailAddress("plataforma.oficina.ti@gmail.com");
            clienteSmtp = new SmtpClient("smtp.gmail.com");
            clienteSmtp.Port = 587;
            
            message.To.Add(correo);
            message.Bcc.Add("herberthtorres1@gmail.com");
            message.Subject = asunto;
            message.IsBodyHtml = true; //el texto del mensaje lo pueden poner en HTML y darle formato
            message.Body = mensaje;
            //Establesco que usare seguridad (ssl = Secure Sockets Layer) 
            clienteSmtp.EnableSsl = true;
            clienteSmtp.UseDefaultCredentials = false;
            clienteSmtp.Credentials = new NetworkCredential("plataforma.oficina.ti@gmail.com", "Pti20122013");
            try{
                clienteSmtp.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}


