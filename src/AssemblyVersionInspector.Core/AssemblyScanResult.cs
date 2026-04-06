namespace AssemblyVersionInspector.Core
{
  public class AssemblyScanResult
  {
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string AssemblyName { get; set; }
    public string AssemblyVersion { get; set; }
    public bool IsManagedAssembly { get; set; }
    public string ErrorMessage { get; set; }
  }
}