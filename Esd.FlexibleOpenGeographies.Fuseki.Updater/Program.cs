using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Esd.FlexibleOpenGeographies.Fuseki.Updater
{
    class Program
    {
        public static string RootLocation;
        public static string WebLocation;
        public static string Port;

        private const int BatchSize = 1000;

        private static string DataLocation
        {
            get
            {
                return RootLocation + "Data";
            }
        }

        static void Main(string[] args)
        {
            RootLocation = ConfigurationManager.AppSettings["FusekiAppRoot"];
            WebLocation = ConfigurationManager.AppSettings["FusekiWebRoot"];
            Port = ConfigurationManager.AppSettings["FusekiPort"];

            try
            {
                KillFuseki();
                ClearOldData();
                StartFuseki(true);

                List<string> rdfs = new List<string>();

                var factory = new ContextFactory();

                var queryFactory = new QueryFactory(factory);

                using (var context = (FogContext)factory.Create())
                {
                    IEnumerable<AreaTypeBasic> areaTypes = queryFactory.CreateAreaTypesBasicQuery(false).Fetch();
                    foreach (AreaTypeBasic areaType in areaTypes)
                    {
                        Console.WriteLine("Import of " + areaType.Label + " starting.");

                        int step = 0;
                        bool hasData = true;

                        while (hasData)
                        {
                            using (var con = new MySqlConnection(context.Database.Connection.ConnectionString))
                            {
                                con.Open();

                                using (var command = con.CreateCommand())
                                {
                                    command.CommandTimeout = 3600;
                                    command.CommandText = "SELECT RDF FROM ViewRDFExport WHERE type_code = '" + areaType.Code + "' limit " + (step * BatchSize) + ", " + BatchSize;
                                    step++;

                                    using (var reader = command.ExecuteReader())
                                    {
                                        int i = 0;

                                        while (reader.Read())
                                        {
                                            i++;
                                            string rdf = reader.GetString(0);

                                            rdfs.Add(rdf);
                                            Console.WriteLine(rdf);

                                            if (rdfs.Count == 100)
                                            {
                                                Console.WriteLine(LoadRDF(rdfs));
                                            }
                                        }

                                        hasData = (i == BatchSize);
                                    }
                                }                                
                            }
                        }

                        Console.WriteLine("Import of " + areaType.Label + " finished.");
                    }

                    Console.WriteLine(LoadRDF(rdfs));
                }
            }
            finally
            {
                KillFuseki();
                StartFuseki(false);
            }
        }

        private static void StartFuseki(bool update)
        {
            string mode = string.Empty;
            if (update)
            {
                mode = "--update";
            }
            //start fuseki in update mode
            System.Diagnostics.Process clientProcess = new Process();
            clientProcess.StartInfo.FileName = "java";
            clientProcess.StartInfo.Arguments = "-jar " + RootLocation + "fuseki-server.jar " + mode + " --port="+Port+" --loc=\"" + DataLocation + "\" --pages=\"" + RootLocation + "pages\" /ds";
            clientProcess.StartInfo.UseShellExecute = false;
            clientProcess.StartInfo.CreateNoWindow = true;
            clientProcess.Start();
            Thread.Sleep(30000);
        }

        private static void ClearOldData()
        {
            if (Directory.Exists(DataLocation))
            {
                Console.WriteLine("Deleting data folder.");
                Directory.Delete(DataLocation, true);
            }
            Directory.CreateDirectory(DataLocation);
            Console.WriteLine("Creating data folder.");
        }

        private static void KillFuseki()
        {
            var query =
                "SELECT ProcessId "
                + "FROM Win32_Process "
                + "WHERE Name = 'java.exe' "
                + "AND CommandLine LIKE '%fuseki-server%'";

            List<Process> servers = null;
            using (var results = new ManagementObjectSearcher(query).Get())
            {
                servers = results.Cast<ManagementObject>()
                                 .Select(mo => Process.GetProcessById((int)(uint)mo["ProcessId"]))
                                 .ToList();

            }

            foreach (Process server in servers)
            {
                server.Kill();
                server.WaitForExit();
            }
        }

        public static string LoadRDF(List<string> rdfs)
        {
            HttpWebResponse response = null;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(WebLocation + "ds/data?default");
                request.Method = "PUT";
                request.ContentType = "text/turtle";
                Stream newStream = request.GetRequestStream();

                byte[] bytes = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, rdfs));

                newStream.Write(bytes, 0, bytes.Length);                

                response = (HttpWebResponse)request.GetResponse();

                rdfs.Clear();
            }
            catch
            {
                return "Import Failure!";
            }

            if ((response.StatusCode == HttpStatusCode.NoContent))
            {
                return "Import Success!";
            }
            return "Import Failure!";
        }
    }
}
