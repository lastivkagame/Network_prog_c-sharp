using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Classwork_6_FTP_
{
    class Program
    {
        static void Main(string[] args)
        {
            //FTP - File transfer protocol: 21,20

            const string url = "ftp://92.52.138.128/";

            var request = (FtpWebRequest)WebRequest.Create(url);

            #region List Of directories 
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            var responce = (FtpWebResponse)request.GetResponse();
            var reader = new StreamReader(responce.GetResponseStream());
            var content = reader.ReadToEnd();
            Console.WriteLine(content);
            Console.WriteLine("Status: \t" + responce.StatusDescription);
            reader.Close();
            responce.Close();
            #endregion

            #region CreateFolder
            //var folder = "Folder for picture";

            //request = (FtpWebRequest)WebRequest.Create(Path.Combine(url, folder));
            //request.Method = WebRequestMethods.Ftp.MakeDirectory;

            //responce = (FtpWebResponse)request.GetResponse();
            //responce.Close();
            #endregion

            #region Folder Remove
            //try
            //{
            //    var folder = "Folder FOR REMOVE";
            //    request = (FtpWebRequest)WebRequest.Create(Path.Combine(url, folder));
            //    request = (FtpWebRequest)WebRequest.Create(url));
            //    request.Method = WebRequestMethods.Ftp.RemoveDirectory;

            //    responce = (FtpWebResponse)request.GetResponse();
            //    responce.Close();
            //}
            //catch (Exception ex)
            //{

            //    Console.WriteLine(ex.Message);
            //    //throw;
            //} 
            #endregion

            //#region CreateFolder
            //var folder = "Folder for picture";

            //var file = "text_student.txt";
            //request = (FtpWebRequest)WebRequest.Create(Path.Combine(url, file));
            //request.Method = WebRequestMethods.Ftp.UploadFile;


            //File.WriteAllText(file, DateTime.Now.ToShortDateString());
            //var writer = new StreamWriter(request.GetRequestStream());
            //writer.Write(File.ReadAllText(file));
            //writer.Close();

            //var file = "text_student.txt";
            //request = (FtpWebRequest)WebRequest.Create(Path.Combine(url, file));
            //request.Method = WebRequestMethods.Ftp.DownloadFile;
            //responce = (FtpWebResponse)request.GetResponse();

            //reader = new StreamReader(responce.GetResponseStream());
            //File.WriteAllText("", reader.ReadToEnd());

            //var writer = new StreamWriter(request.GetRequestStream());

            //responce = (FtpWebResponse)request.GetResponse();
            //responce.Close();
        }
    }
}
