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

      //print results
      PrintResults(results);
    }
    catch (Exception ex)
    {
      Console.WriteLine(string.Format("Fatal error: {0}", ex.Message));
    }
  }

  private static void PrintResults(List<AssemblyScanResult> results)
  {
    Console.WriteLine("=== Assembly Inspection ===");
    Console.WriteLine();

    foreach (AssemblyScanResult result in results)
    {
      if (result.IsManagedAssembly)
        PrintManagedAssembly(result);
      else
        PrintErrorAssembly(result);

      Console.WriteLine();
    }
  }

  private static void PrintManagedAssembly(AssemblyScanResult result)
  {
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("[OK] ");
    Console.ResetColor();

    Console.WriteLine(result.FileName);

    Console.WriteLine(string.Format("  AssemblyName:     {0}", result.AssemblyName));
    Console.WriteLine(string.Format("  AssemblyVersion:  {0}", result.AssemblyVersion));
    Console.WriteLine(string.Format("  AssemblyFullName: {0}", result.AssemblyFullName));
    Console.WriteLine(string.Format("  FileVersion:      {0}", result.FileVersion));
  }

  private static void PrintErrorAssembly(AssemblyScanResult result)
  {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write("[ERROR] ");
    Console.ResetColor();

    Console.WriteLine(result.FileName);
    Console.WriteLine(string.Format("  {0}", result.ErrorMessage));
  }
}