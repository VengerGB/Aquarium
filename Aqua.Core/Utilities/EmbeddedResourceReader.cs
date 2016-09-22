namespace Aqua.Core.Utils
{
    using System.IO;
    using System.Reflection;

    public static class EmbeddedResourceReader
    {
        public static string ReadTextResource(string resourceName)
        {
            using (Stream stream = EmbeddedResourceReader.ReadEmbeddedData(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static Stream ReadEmbeddedData(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resource = "Aqua.Core." + resourceName;

            return assembly.GetManifestResourceStream(resource);
        }
    }
}