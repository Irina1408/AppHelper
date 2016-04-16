using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppUtils
{
    public class ApplicationInfo
    {
        #region private members

        private readonly string productName;
        private readonly string productVersion;
        private readonly string instanceName;
        private readonly string applicationFolderPath;

        #endregion

        #region singletone

        public static readonly ApplicationInfo Current = new ApplicationInfo();

        #endregion

        #region init

        private ApplicationInfo()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            productName = fvi.ProductName;
            productVersion = fvi.ProductVersion;
            applicationFolderPath = System.IO.Path.GetDirectoryName(assembly.Location);
            instanceName = applicationFolderPath.Substring(applicationFolderPath.LastIndexOf('/') + 1);
        }

        #endregion

        #region properties

        /// <summary>
        /// Product name
        /// </summary>
        public string ProductName
        {
            get { return productName; }
        }

        /// <summary>
        /// Current product version
        /// </summary>
        public string ProductVersion
        {
            get { return productVersion; }
        }

        /// <summary>
        /// Product installation instance name (installation directory name)
        /// </summary>
        public string InstanceName
        {
            get { return instanceName; }
        }

        /// <summary>
        /// Application home folder path
        /// </summary>
        /// <remarks>Same folder where there the entry executable file is located</remarks>
        public string ApplicationFolderPath
        {
            get { return applicationFolderPath; }
        }

        #endregion
    }
}
