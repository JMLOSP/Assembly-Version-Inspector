# AssemblyVersionInspector

Small .NET tool to inspect assemblies in a folder and display version information.

## Why this tool?

Working with .NET Framework applications often leads to assembly version conflicts that are hard to diagnose.

This tool helps developers quickly inspect assemblies and understand version discrepancies in a given folder.

## Features

- Scan a folder for .dll files
- Detect managed vs non-managed assemblies
- Extract:
  - Assembly Name
  - Assembly Version
  - File Version
  - Full Assembly Name

## Usage

```bash
AssemblyVersionInspector.Cli <folder-path>