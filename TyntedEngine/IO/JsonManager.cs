using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tynted.IO
{
    public static class JsonManager
    {
        /// <summary>
        /// Loads a json object from file.
        /// </summary>
        /// <param name="filePath">The filepath with extension.</param>
        /// <returns>Json object with parsed data.</returns>
        public static JObject LoadFile(string filePath)
        {
            try
            {
                return JObject.Parse(FileManager.LoadFromFile(filePath));
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not load from " + filePath + ". Issue: " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Saves the json string to a file.
        /// </summary>
        /// <param name="filePath">The file to save to with extension.</param>
        /// <param name="json">The json data to save to the file.</param>
        public static void SaveFile(string filePath, string json)
        {
            try
            {
                FileManager.SaveFile(filePath, json);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not save to " + filePath + ". Issue: " + e.Message);
            }
        }
    }
}
