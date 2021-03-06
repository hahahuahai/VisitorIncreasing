﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisitAmountIncreaser
{
    class Program
    {
        static string MainUrl = "http://blog.csdn.net";//博客主网址，如http://blog.csdn.net。
        static string DetailedUrl = "";//具体的博客网址后缀，如：http://blog.csdn.net/u012130706/article/details/76285650 的后缀是u012130706/article/details/76285650。
        static void Main()
        {
            
            //string url = Console.ReadLine();
            int threadAcount = 2;

            Console.WriteLine("请输入博客主网址（默认为CSDN，则直接按回车键）：");
            if (Console.ReadLine() != "") MainUrl = Console.ReadLine();
            do
            {
                Console.WriteLine("请输入具体博客网址后缀(如：http://blog.csdn.net/u012130706/article/details/76285650 的后缀是u012130706/article/details/76285650)：");
                DetailedUrl = Console.ReadLine();
            } while (DetailedUrl == "");

            Console.WriteLine("The application is staring!");
            StartTask(MainUrl);
            string command = Console.ReadLine();
            if (string.CompareOrdinal(command, "X") == 0)
            {
                Console.WriteLine("Application will quit after 30 seconds!");
                Thread.Sleep(30000);
            }
        }

        static async void StartTask(string url)
        {
            while (true)
            {
                await RunVisit(url);
            }
        }

        static async Task RunVisit(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync(DetailedUrl);
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.

                    string content = await response.Content.ReadAsStringAsync();

                    string result = GetStringInBetween(content, "<span class=\"link_view\" title=\"阅读次数\">", "</span>");
                    Console.WriteLine(result);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static string GetStringInBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start = strSource.IndexOf(strStart, 0, System.StringComparison.Ordinal) + strStart.Length;
                int End = strSource.IndexOf(strEnd, Start, System.StringComparison.Ordinal);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }
}
