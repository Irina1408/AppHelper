using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppUtils.SerializationUtils
{
    public static class ObjectSerializer
    {
        #region public methods

        /// <summary>
        /// Saves settings object to the file
        /// </summary>
        public static void Save<T>(T obj, string fileName = null) where T : class
        {
            fileName = GetFilePath<T>(fileName);

            Serialize(fileName, obj, new Type[] { });
        }

        /// <summary>
        /// Loads serialized settings object from the file
        /// </summary>
        public static T Load<T>(string fileName = null) where T : class
        {
            // build file path
            fileName = GetFilePath<T>(fileName);

            // check if file exists 
            if (!System.IO.File.Exists(fileName))
                return null;

            return Deserialize(fileName, typeof(T), new Type[] { }) as T;
        }

        /// <summary>
        /// Saves settings object to the file
        /// </summary>
        public static void SaveToAppFolder<T>(T obj, string fileName = null) where T : class
        {
            fileName = GetAppFolderFilePath<T>(fileName);

            Serialize(fileName, obj, new Type[] { });
        }

        /// <summary>
        /// Loads serialized settings object from the file
        /// </summary>
        public static T LoadFromAppFolder<T>(string fileName = null) where T : class
        {
            // build file path
            fileName = GetAppFolderFilePath<T>(fileName);

            // check if file exists 
            if (!System.IO.File.Exists(fileName))
                return null;

            return Deserialize(fileName, typeof(T), new Type[] { }) as T;
        }
        #endregion

        #region helper methods

        private static object Deserialize(string fileName, Type objType, Type[] extraTypes)
        {
            object result;
            // open file
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                // create a serializer
                var serializer = new XmlSerializer(objType, extraTypes);
                // deserialize the object from the file
                result = serializer.Deserialize(stream);

                stream.Close();
            }
            return result;
        }

        private static void Serialize(string fileName, object obj, Type[] extraTypes)
        {
            // open file
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                // create a serializer
                var serializer = new XmlSerializer(obj.GetType(), extraTypes);
                // serialize the object to the file
                serializer.Serialize(stream, obj);

                stream.Close();
            }
        }

        private static string GetFilePath<T>(string fileName)
        {
            // user type name as filename by default
            if (fileName == null)
                fileName = string.Format("{0}.xml", typeof(T).Name);
            // build file path
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = Path.Combine(dir, ApplicationInfo.Current.ProductName);
            //dir = Path.Combine(dir, ApplicationInfo.Current.InstanceName);
            fileName = Path.Combine(dir, fileName);

            // create directory if required
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return fileName;
        }

        private static string GetAppFolderFilePath<T>(string fileName)
        {
            // user type name as filename by default
            if (fileName == null)
                fileName = string.Format("{0}.xml", typeof(T).Name);
            // build file path
            var dir = ApplicationInfo.Current.ApplicationFolderPath;
            fileName = Path.Combine(dir, fileName);

            // create directory if required
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return fileName;
        }

        #endregion
    }
}
