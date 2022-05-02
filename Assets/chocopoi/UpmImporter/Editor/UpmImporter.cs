using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;

namespace Chocopoi.UpmImporter
{
    /// <summary>
    /// An utility class that allows installations of UPM packages from non-UPM scripts
    /// </summary>
    [InitializeOnLoad]
    public class UpmImporter
    {
        /// <summary>
        /// A version constant that defines the version of this UpmImporter. Reserved for future uses.
        /// </summary>
        public const string Version = "1.0.0";

        private static readonly string[] PackagesToInstall = new string[] { "com.unity.nuget.newtonsoft-json" };

        /// <summary>
        /// Installs the dependency "com.unity.nuget.newtonsoft-json" that ScopedRegistryImporter requires
        /// </summary>
        static UpmImporter()
        {
            InstallUpmPackages(PackagesToInstall);
        }

        /// <summary>
        /// Installs (or updates) all the packages provided. Blocks thread until all packages are installed.
        /// </summary>
        /// <param name="packagesToInstall">A string array containing all UPM identifiers</param>
        public static void InstallUpmPackages(string[] packagesToInstall)
        {
            foreach (string identifier in packagesToInstall)
            {
                InstallUpmPackage(identifier);
            }
        }

        /// <summary>
        /// Installs (or updates) all the single package provided. Blocks thread until the package is installed.
        /// </summary>
        /// <param name="identifier">The identifier of the package</param>
        /// <returns>A boolean whether the operation is successful.</returns>
        public static bool InstallUpmPackage(string identifier)
        {
            var req = Client.Add(identifier);

            // block until complete
            while (!req.IsCompleted) { };

            if (req.Error != null)
            {
                Debug.LogError(req.Error);
            }

            return req.Error == null;
        }
    }
}