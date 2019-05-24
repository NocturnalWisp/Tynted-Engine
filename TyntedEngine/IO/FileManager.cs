using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tynted.IO
{
    public static class FileManager
    {
        /// <summary>
        /// Loads a file from a specified file path,
        /// needs to have permission to grab it.
        /// </summary>
        /// <param name="filePath">The path with extension.</param>
        /// <returns>The resultant string if found, otherwise empty.</returns>
        public static string LoadFromFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not load from " + filePath + ". Issue: " + e.Message);
                return "";
            }
        }

        /// <summary>
        /// Saves a string to the specified file path.
        /// </summary>
        /// <param name="filePath">Path with extension.</param>
        /// <param name="toSave">The string to save.</param>
        public static void SaveFile(string filePath, string toSave)
        {
            try
            {
                File.WriteAllText(filePath, toSave);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not save to " + filePath + ". Issue: " + e.Message);
            }
        }
    }
}
