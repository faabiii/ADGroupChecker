using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.Write("Bitte geben Sie die Domain ein: ");
        string domain = Console.ReadLine();
        Console.Write("Bitte geben Sie den LDAP-Pfad ein: ");
        string ldapPath = Console.ReadLine();

        DirectoryEntry entry = new DirectoryEntry(ldapPath);

        DirectorySearcher searcher = new DirectorySearcher(entry)
        {
            Filter = "(&(objectClass=group)(groupType:1.2.840.113556.1.4.803:=8)(mail=*))"
        };
        searcher.PropertiesToLoad.Add("cn");
        searcher.PropertiesToLoad.Add("mail");

        var distributionGroups = new List<string>();

        try
        {
            foreach (SearchResult result in searcher.FindAll())
            {
                string groupName = result.Properties["cn"][0].ToString();
                distributionGroups.Add(groupName);
                Console.WriteLine($"Gefundene Verteilergruppe: {groupName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler bei der Suche im AD: {ex.Message}");
        }

        Console.Write("Bitte geben Sie den Pfad zum Filestore ein: ");
        string fileStorePath = Console.ReadLine();

        var results = CheckFileStoreForGroups(fileStorePath, distributionGroups);

        DisplayResults(results);
    }

    static Dictionary<string, List<string>> CheckFileStoreForGroups(string path, List<string> groups)
    {
        var results = new Dictionary<string, List<string>>();
        var allFiles = GetFilesSafe(new DirectoryInfo(path));
        int totalFiles = allFiles.Count;
        int filesProcessed = 0;
        object lockObject = new object();

        Parallel.ForEach(allFiles, file =>
        {
            try
            {
                FileSecurity fileSecurity = file.GetAccessControl();
                AuthorizationRuleCollection acl = fileSecurity.GetAccessRules(true, true, typeof(NTAccount));

                foreach (FileSystemAccessRule rule in acl)
                {
                    string groupName = rule.IdentityReference.Value;

                    if (groups.Contains(groupName))
                    {
                        lock (lockObject)
                        {
                            if (!results.ContainsKey(groupName))
                            {
                                results[groupName] = new List<string>();
                            }
                            results[groupName].Add(file.FullName);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Zugriff verweigert auf Datei: {file.FullName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Überprüfen der Datei: {file.FullName}, Fehler: {ex.Message}");
            }

            lock (lockObject)
            {
                filesProcessed++;
                DisplayProgress(filesProcessed, totalFiles);
            }
        });

        return results;
    }

    static List<FileInfo> GetFilesSafe(DirectoryInfo directory)
    {
        var files = new List<FileInfo>();

        try
        {
            files.AddRange(directory.GetFiles());
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Zugriff verweigert auf Verzeichnis: {directory.FullName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Abrufen der Dateien in Verzeichnis: {directory.FullName}, Fehler: {ex.Message}");
        }

        try
        {
            foreach (var subDirectory in directory.GetDirectories())
            {
                files.AddRange(GetFilesSafe(subDirectory));
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Zugriff verweigert auf Verzeichnis: {directory.FullName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Abrufen der Dateien in Verzeichnis: {directory.FullName}, Fehler: {ex.Message}");
        }

        return files;
    }

    static void DisplayProgress(int filesProcessed, int totalFiles)
    {
        double progress = (double)filesProcessed / totalFiles * 100;
        Console.Write($"\rFortschritt: {progress:F2}% ({filesProcessed}/{totalFiles} Dateien verarbeitet)");
    }

    static void DisplayResults(Dictionary<string, List<string>> results)
    {
        Console.WriteLine("\n\nErgebnisse der Überprüfung:");
        foreach (var group in results)
        {
            Console.WriteLine($"\nGruppe: {group.Key}");
            Console.WriteLine("Verwendet in den folgenden Dateien:");
            foreach (var file in group.Value)
            {
                Console.WriteLine($"- {file}");
            }
        }
    }
}