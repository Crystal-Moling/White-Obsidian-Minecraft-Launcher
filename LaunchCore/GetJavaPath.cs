using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace White_Obsidian_Minecraft_Launcher.LaunchCore
{
    class GetJavaPath
    {
        /// <summary>
        ///     从注册表中查找可能的 javaw.exe 的路径。
        /// </summary>
        /// <returns>可能的 Java 路径构成的列表。</returns>
        public static IEnumerable<string> FindJava()
        {
            try
            {
                using var rootReg = Registry.LocalMachine.OpenSubKey("SOFTWARE");
                var javas = (rootReg == null ? Array.Empty<string>() : FindJavaInternal(rootReg))
                    .Union(FindJavaInternal(rootReg.OpenSubKey("Wow6432Node")))
                    .Union(FindJavaInOfficialGamePath())
                    .ToHashSet();

                var evJava = FindJavaUsingEnvironmentVariable();

                if (string.IsNullOrEmpty(evJava))
                    return javas;

                javas.Add(Path.Combine(evJava, "bin", "javaw.exe"));

                return javas;
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        static IEnumerable<string> FindJavaInOfficialGamePath()
        {
            var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ".minecraft", "runtime");

            var paths = new[] { "java-runtime-alpha", "jre-legacy" };

            return paths.Select(path => Path.Combine(basePath, path, "bin", "javaw.exe"))
                .Where(File.Exists);
        }

        static string FindJavaUsingEnvironmentVariable()
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .Build();
                var javaHome = configuration["JAVA_HOME"];
                return javaHome;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        static IEnumerable<string> FindJavaInternal(RegistryKey registry)
        {
            try
            {
                using var registryKey = registry.OpenSubKey("JavaSoft");
                if (registryKey == null || (registry = registryKey.OpenSubKey("Java Runtime Environment")) == null)
                    return Array.Empty<string>();
                return from ver in registry.GetSubKeyNames()
                       select registry.OpenSubKey(ver)
                    into command
                       where command != null
                       select command.GetValue("JavaHome")
                    into javaHomes
                       where javaHomes != null
                       select javaHomes.ToString()
                    into str
                       where !string.IsNullOrWhiteSpace(str)
                       select str + "\\bin\\javaw.exe";
            }
            catch
            {
                return Array.Empty<string>();
            }
        }
    }
}
