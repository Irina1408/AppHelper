using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppUtils.SerializationUtils
{
    public static class ObjectSerializer
    {
        /// <summary>
        /// Saves object to the file
        /// </summary>
        public static void Save<T>(T obj, string fileName = null) where T : class
        {
            fileName = SerializationHelper.GetFilePath<T>(fileName);

            SerializationHelper.Serialize(fileName, obj, new Type[] { });
        }

        /// <summary>
        /// Loads serialized object from the file
        /// </summary>
        public static T Load<T>(string fileName = null) where T : class
        {
            // build file path
            fileName = SerializationHelper.GetFilePath<T>(fileName);

            // check if file exists 
            if (!System.IO.File.Exists(fileName))
                return null;

            return SerializationHelper.Deserialize(fileName, typeof(T), new Type[] { }) as T;
        }

        /// <summary>
        /// Saves object to the file
        /// </summary>
        public static void SaveToAppFolder<T>(T obj, string fileName = null) where T : class
        {
            fileName = SerializationHelper.GetAppFolderFilePath<T>(fileName);

            SerializationHelper.Serialize(fileName, obj, new Type[] { });
        }

        /// <summary>
        /// Loads serialized object from the file
        /// </summary>
        public static T LoadFromAppFolder<T>(string fileName = null) where T : class
        {
            // build file path
            fileName = SerializationHelper.GetAppFolderFilePath<T>(fileName);

            // check if file exists 
            if (!System.IO.File.Exists(fileName))
                return null;

            return SerializationHelper.Deserialize(fileName, typeof(T), new Type[] { }) as T;
        }
    }

    public static class CryptoObjectSerializer
    {
        /// <summary>
        /// Saves object to the file
        /// </summary>
        public static void SaveEncryptedDes<T>(T obj, string key, string iv, string fileName = null) where T : class
        {
            fileName = SerializationHelper.GetFilePath<T>(fileName);

            using (var encryptor = new TripleDESCryptoServiceProvider())
            {
                encryptor.Key = Encoding.Default.GetBytes(key);
                encryptor.IV = Encoding.Default.GetBytes(iv);

                SerializationHelper.EncryptAndSerialize(fileName, obj, new Type[] { }, encryptor);

                encryptor.Dispose();
            }
        }

        /// <summary>
        /// Loads serialized object from the file
        /// </summary>
        public static T LoadDecryptedDes<T>(string key, string iv, string fileName = null) where T : class
        {
            // build file path
            fileName = SerializationHelper.GetFilePath<T>(fileName);

            // check if file exists 
            if (!System.IO.File.Exists(fileName))
                return null;

            T res;

            using (var decryptor = new TripleDESCryptoServiceProvider())
            {
                decryptor.Key = Encoding.Default.GetBytes(key);
                decryptor.IV = Encoding.Default.GetBytes(iv);

                res = SerializationHelper.DecryptAndDeserialize(fileName, typeof(T), new Type[] { }, decryptor) as T;

                decryptor.Dispose();
            }

            return res;
        }
    }

    internal static class SerializationHelper
    {
        public static object Deserialize(string fileName, Type objType, Type[] extraTypes)
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

        public static void Serialize(string fileName, object obj, Type[] extraTypes)
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


        public static void EncryptAndSerialize(string filename, object obj, Type[] extraTypes, SymmetricAlgorithm key)
        {
            using (var fs = File.Open(filename, FileMode.Create))
            {
                using (var cs = new CryptoStream(fs, key.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // create a serializer
                    var xmlser = new XmlSerializer(obj.GetType(), extraTypes);
                    xmlser.Serialize(cs, obj);

                    cs.Close();
                }
                fs.Close();
            }
        }

        public static object DecryptAndDeserialize(string filename, Type objType, Type[] extraTypes, SymmetricAlgorithm key)
        {
            object result;

            using (var fs = File.Open(filename, FileMode.Open))
            {
                using (var cs = new CryptoStream(fs, key.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    var xmlser = new XmlSerializer(objType, extraTypes);
                    result = xmlser.Deserialize(cs);

                    cs.Close();
                }
                fs.Close();
            }

            return result;
        }


        public static string GetFilePath<T>(string fileName)
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

        public static string GetAppFolderFilePath<T>(string fileName)
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
    }
}
