# AssemblyVersionInspector

Tooling project focused on .NET assembly inspection.

## Goal
Analyze assemblies in a folder, detect version mismatches and dependency conflicts, and help generate binding redirects for .NET Framework projects.

## Initial scope
- Scan DLLs in a target folder
- Read AssemblyVersion/FileVersion
- Detect duplicated assemblies with different versions
- Report potential conflicts
- Suggest binding redirects

## Status
Early private prototype.