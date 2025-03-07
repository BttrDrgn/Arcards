using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arcards.Components.Pages;

namespace Arcards
{
    public static class IO
    {
        public static string GetPath(string path)
        {
#if ANDROID || IOS
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), path);
#else
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Drgn", "Arcards", path);
#endif
        }

        public static string ReadFile(string path)
        {
            path = GetPath(path);
            return File.ReadAllText(path);
        }

        public static void DeleteFile(string path)
        {
            path = GetPath(path);
            File.Delete(path);
        }

        public static void DeleteFolder(string path, bool recursive = false)
        {
            path = GetPath(path);
            Directory.Delete(path, recursive);
        }

        public static void WriteFile(string path, string content, bool recursiveFolder = true)
        {
            try
            {
                path = GetPath(path);

                if (recursiveFolder)
                {
                    string directoryPath = Path.GetDirectoryName(path);

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(path, content);
            }
            catch(Exception e)
            {
                Dev.ErrorLog(e.Message);
            }
        }

        public static bool Exists(string path)
        {
            path = GetPath(path);
            return File.Exists(path);
        }

        public static async Task<Stream> OpenPackageFile(string path)
        {
            return await FileSystem.OpenAppPackageFileAsync(path);
        }

        public static string GetOSLocale()
        {
            return CultureInfo.CurrentCulture.Name;
        }
    }
}
