using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

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
      //use ConcurrentBag to store results from parallel processing
      ConcurrentBag<AssemblyScanResult> results = new ConcurrentBag<AssemblyScanResult>();

      //set up parallel options to use all available processors
      ParallelOptions options = new ParallelOptions
      {
        MaxDegreeOfParallelism = Environment.ProcessorCount
      };

      //scan each dll file in parallel for better performance
      Parallel.ForEach(dllFiles, options, dllFile =>
      {
        //process single file
        AssemblyScanResult result = ProcessSingleFile(dllFile);

        //add result to the concurrent collection
        results.Add(result);
      });

      //order results by file name for consistent display
      List<AssemblyScanResult> orderedResults = new List<AssemblyScanResult>(results);

      //sort results by file name (case-insensitive)
      orderedResults.Sort((x, y) => string.Compare(x.FileName, y.FileName, StringComparison.OrdinalIgnoreCase));

      //return ordered results
      return orderedResults;
    }

    private AssemblyScanResult ProcessSingleFile(string dllFile)
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

      //return result for this file
      return result;
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
