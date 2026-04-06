using System;
using System.Collections.Generic;

using AssemblyVersionInspector.Core;

internal class Program
{
  private static void Main(string[] args)
  {
    //check arguments
    if (args.Length == 0)
    {
      Console.WriteLine("Usage: AssemblyVersionInspector.Cli <folder-path>");

      return;
    }

    //get folder path
    string folderPath = args[0];

    try
    {
      //scan folder
      AssemblyScanner scanner = new AssemblyScanner();

      //get results
      List<AssemblyScanResult> results = scanner.ScanFolder(folderPath);

      //display results
      foreach (AssemblyScanResult result in results)
      {
        Console.WriteLine(string.Format("File: {0}", result.FileName));

        if (result.IsManagedAssembly)
        {
          Console.WriteLine(string.Format("  AssemblyName: {0}", result.AssemblyName));
          Console.WriteLine(string.Format("  AssemblyVersion: {0}", result.AssemblyVersion));
        }
        else
          Console.WriteLine(string.Format("  Error: {0}", result.ErrorMessage));

        Console.WriteLine();
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(string.Format("Fatal error: {0}", ex.Message));
    }
  }
}