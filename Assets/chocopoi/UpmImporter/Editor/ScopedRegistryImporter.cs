using System.IO;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace Chocopoi.UpmImporter
{
    /// <summary>
    /// An utility class to add a new scoped registry or scoped packages to the manifest JSON
    /// </summary>
    public class ScopedRegistryImporter
    {
        /// <summary>
        /// A version constant that defines the version of this ScopedRegistryImporter. Reserved for future uses.
        /// </summary>
        public const string Version = "1.0.0";

        private const string ManifestPath = "Packages/manifest.json";

        private bool modified;

        private JObject manifest;

        /// <summary>
        /// Initialises and load the manifest from the filesystem
        /// </summary>
        public ScopedRegistryImporter()
        {
            modified = false;
            manifest = GetManifest();
        }

        /// <summary>
        /// Adds packages (if not added) to the provided scoped registry
        /// </summary>
        /// <param name="scopedRegistry">The scoped registry JSON Object</param>
        /// <param name="packagesToScope">Package names to be scoped</param>
        public void AddScopedPackagesIfNotAdded(JObject scopedRegistry, string[] packagesToScope)
        {
            foreach (string packageName in packagesToScope)
            {
                if (!IsScopedRegistryPackageExist(scopedRegistry, packageName))
                {
                    modified = true;
                    Debug.Log("[ScopedRegistryImporter] Adding package to scope: " + packageName);
                    AddScopedRegistryPackage(scopedRegistry, packageName);
                }
            }
        }

        /// <summary>
        /// Adds a scoped registry. This does not check whether there's a existing scoped registry or not. Use FindScopedRegistry() before this function.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="url">URL</param>
        /// <param name="scopes">Scopes</param>
        /// <returns></returns>
        public JObject AddScopedRegistry(string name, string url, string[] scopes)
        {
            JArray scopedRegistries = (JArray)manifest["scopedRegistries"];

            if (scopedRegistries == null)
            {
                scopedRegistries = new JArray();
                manifest["scopedRegistries"] = scopedRegistries;
            }

            JObject newReg = new JObject(
                new JProperty("name", name),
                new JProperty("url", url),
                new JProperty("scopes", JArray.FromObject(scopes))
            );
            scopedRegistries.Add(newReg);

            modified = true;
            return newReg;
        }

        /// <summary>
        /// Finds the scoped registry with the specified URL. Returns null if not found.
        /// </summary>
        /// <param name="url">URL of the scoped registry</param>
        /// <returns>The scoped registry JSON Object</returns>
        public JObject FindScopedRegistry(string url)
        {
            JArray scopedRegistries = (JArray)manifest["scopedRegistries"];

            if (scopedRegistries == null)
            {
                return null;
            }

            foreach (JObject sreg in scopedRegistries)
            {
                if (sreg["url"].Value<string>() == url)
                {
                    return sreg;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks whether the specified package name exists in the scoped registry provided
        /// </summary>
        /// <param name="scopedRegistry">The scoped registry JSON Object</param>
        /// <param name="packageName">The package's name</param>
        /// <returns>A boolean</returns>
        public static bool IsScopedRegistryPackageExist(JObject scopedRegistry, string packageName)
        {
            foreach (string value in scopedRegistry["scopes"])
            {
                if (value == packageName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds a scoped registry package to the provided scoped registry. This does not check whether an existing package already exist.
        /// </summary>
        /// <param name="scopedRegistry"></param>
        /// <param name="packageName"></param>
        public void AddScopedRegistryPackage(JObject scopedRegistry, string packageName)
        {
            modified = true;
            scopedRegistry["scopes"].AddAfterSelf(packageName);
        }

        private static JObject GetManifest()
        {
            StreamReader reader = new StreamReader(ManifestPath);
            string str = reader.ReadToEnd();
            reader.Close();
            return JObject.Parse(str);
        }

        /// <summary>
        /// Saves the manifest to filesystem if its modified by this class. The UPM will start to work adding the packages after calling this.
        /// </summary>
        public void SaveIfModified()
        {
            if (modified)
            {
                StreamWriter writer = new StreamWriter(ManifestPath, false);
                writer.WriteLine(manifest.ToString());
                writer.Close();
            }
        }
    }
}
