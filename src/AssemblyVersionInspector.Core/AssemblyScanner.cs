using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace AssemblyVersionInspector.Core
{
  public class AssemblyScanner
  {
    private const string cDllSearchPattern = "*.dll";
    private const string cNotManagedAssemblyMessage = "Not a managed .NET assembly.";

    public List<AssemblyScanResult> ScanFolder(string folderPath)
    {
      //validate input
      if (!Directory.Exists(folderPath))
        throw new DirectoryNotFoundException(string.Format("Folder not found: {0}", folderPath));

      //get all dll files in the folder
      string[] dllFiles = Directory.GetFiles(folderPath, cDllSearchPattern, SearchOption.TopDirectoryOnly);

      //scan each dll file
      return ProcessFiles(dllFiles);
    }

    private List<AssemblyScanResult> ProcessFiles(string[] dllFiles)
    {
      //prepare results list
      List<AssemblyScanResult> results = new List<AssemblyScanResult>();

      //scan each dll file
      foreach (string dllFile in dllFiles)
      {
        //initialize result for this file
        AssemblyScanResult result = new AssemblyScanResult
        {
          FilePath = dllFile,
          FileName = Path.GetFileName(dllFile)
        };

        //try to read assembly info
        try
        {
          //getName
          AssemblyName assemblyName = AssemblyName.GetAssemblyName(dllFile);

          //if successful, it's a managed assembly
          result.IsManagedAssembly = true;
          result.AssemblyName = assemblyName.Name;
          result.AssemblyVersion = GetAssemblyVersion(assemblyName);
          result.AssemblyFullName = assemblyName.FullName;

          //get file version
          FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(dllFile);

          //if file version is not available, it will return empty string
          result.FileVersion = fileVersionInfo.FileVersion;
        }
        catch (BadImageFormatException)
        {
          //not a valid .NET assembly
          result.IsManagedAssembly = false;
          result.ErrorMessage = cNotManagedAssemblyMessage;
        }
        catch (Exception ex)
        {
          //other errors (e.g. file access issues)
          result.IsManagedAssembly = false;
          result.ErrorMessage = ex.Message;
        }

        //add result to list
        results.Add(result);
      }

      //return results
      return results;
    }

    private string GetAssemblyVersion(AssemblyName assemblyName)
    {
      //if version is not available, return empty string
      if (assemblyName.Version == null)
        return string.Empty;

      //return version as string
      return assemblyName.Version.ToString();
    }
  }
}
