using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace makedirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            //FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://risk.com/" + "//Staffer/pkumar69/NewDirectory");
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://risk.com/" + "//Staffer/pkumar69/"+args[0]);
            request.Credentials = new NetworkCredential("user", "pasword");
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;
            Console.WriteLine("Getting the response");

            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            

            using (var resp = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine(resp.StatusCode);
            }

        }

    }
}