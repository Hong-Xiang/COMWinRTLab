using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace CSharpClientConsole;

/// <summary>
/// Strategy-based ComWrappers implementation for Linux COM-like interop
/// Uses the modern .NET ComWrappers architecture with source generators
/// </summary>
public class LinuxStrategyBasedComWrappers : StrategyBasedComWrappers
{
    // Singleton instance
    public static readonly LinuxStrategyBasedComWrappers Instance = new LinuxStrategyBasedComWrappers();
}
