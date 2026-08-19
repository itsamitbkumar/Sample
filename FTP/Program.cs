using System;
using System.Net;
using System.Configuration;

namespace makedirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read from App.config appSettings first, then fall back to environment variables
            string GetSetting(string key, string env) {
                var v = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrWhiteSpace(v)) v = Environment.GetEnvironmentVariable(env);
                return v;
            }

            string host = GetSetting("FtpHost", "FTP_HOST");
            string user = GetSetting("FtpUser", "FTP_USER");
            string pass = GetSetting("FtpPassword", "FTP_PASS");

            string dirName = args != null && args.Length > 0 ? args[0] : null;

            if (string.IsNullOrWhiteSpace(host))
            {
                Console.WriteLine("Error: FTP host is not configured. Set AppSetting 'FtpHost' or environment variable FTP_HOST.");
                Console.WriteLine("Usage: FTP.exe <new-directory-name>\nEither set FtpHost/FtpUser/FtpPassword in App.config or set FTP_HOST/FTP_USER/FTP_PASS environment variables.");
                return;
            }

            // Ensure host ends with '/'
            if (!host.EndsWith("/")) host += "/";
            if (!string.IsNullOrEmpty(dirName)) host += dirName;

            Console.WriteLine($"Creating directory: {host}");

            var request = (FtpWebRequest)WebRequest.Create(host);

            if (!string.IsNullOrWhiteSpace(user) || !string.IsNullOrWhiteSpace(pass))
            {
                request.Credentials = new NetworkCredential(user ?? string.Empty, pass ?? string.Empty);
            }

            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;
            request.Method = WebRequestMethods.Ftp.MakeDirectory;

            try
            {
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"FTP response: {resp.StatusCode} - {resp.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("FTP operation failed: " + ex.Message);
                if (ex.Response is FtpWebResponse ftpResp)
                {
                    Console.WriteLine($"FTP response: {ftpResp.StatusCode} - {ftpResp.StatusDescription}");
                }
            }
        }
    }
}
