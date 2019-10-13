using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbn.Products.SVN.SVNClient
{
    public class Tool
    {

		public static List<string> ProjectFilesExclude = new List<string>();//global::Sbn.Products.SVN.SVNClient.Properties.Settings.Default.sProjectFilesExclude.Split(',');//  new string[] { ".dll", ".exe", ".pdb", ".cache", ".resources" };
		public static List<string> ProjectFilesExtention = new List<string>();//global::Sbn.Products.SVN.SVNClient.Properties.Settings.Default.sProjectFilesExtention.Split(',');//new string[] { ".csproj" };
		public static List<string> ProjectPathsExclude = new List<string>();
		public static string ProjectState = "projectState.stat";
		public static string BranchState = "branchState.stat";
		public static string BranchInfo = "branch.info";


        public static void DecompressContentToFile(byte[] inputCompressedContent , string fullPath)
        {
			if (inputCompressedContent != null)
			{
				byte[] decompressContent = DecompressContent(inputCompressedContent);

				FileStream fs = File.Create(fullPath);
				fs.Write(decompressContent, 0, decompressContent.Length);
				fs.Close();
			}
        }

        public static byte[] DecompressContent(byte[] inputCompressedContent)
        {

            byte[] returenedArray = null;
            using (MemoryStream originalFileStream = new MemoryStream(inputCompressedContent))
            {
                using (System.IO.MemoryStream decompressedFileStream = new System.IO.MemoryStream())
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);

                        returenedArray = decompressedFileStream.ToArray();
                    }
                }
            }
            return returenedArray;
        }



        public static byte[] CompressFile(string fullPath)
        {
            byte[] returenedArray = CompressContent(File.ReadAllBytes(fullPath));

            return returenedArray;
        }

        public static byte[] CompressContent(byte[] inputContent)
        {
            byte[] returenedArray = null;

            using (MemoryStream compressedMemoryStream = new MemoryStream())
            {
                using (GZipStream compressionStream = new GZipStream(compressedMemoryStream, CompressionMode.Compress))
                {
                    compressionStream.Write(inputContent, 0, inputContent.Length);
                    compressionStream.Close();

                    returenedArray = compressedMemoryStream.ToArray();
                }
            }

            return returenedArray;
        }


        public static Dictionary<string , string> ReadAllSettings()
        {
            try
            {
                Dictionary<string, string> sRets = new Dictionary<string, string>();

                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    return new Dictionary<string, string>();
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        sRets.Add(key , appSettings[key]);
                        //Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                return null;
                //Console.WriteLine("Error reading app settings");
            }
            return null;
        }

        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                return null;
            }
        }

        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                return;
                //Console.WriteLine("Error writing app settings");
            }
        }



        //
       

    }
}
