using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CaptureOnlyMovements.Types;

public class ConfigPrefixPreset : IEquatable<ConfigPrefixPreset>
{
    public ConfigPrefixPreset() { }

    public ConfigPrefixPreset(ConfigPrefixPreset a)
    {
        OutputFileNamePrefix = a.OutputFileNamePrefix;
        ProcessExecutebleFullName = a.ProcessExecutebleFullName;
        WaitForProcessActive = a.WaitForProcessActive;
    }

    public string OutputFileNamePrefix { get; set; } = string.Empty;
    public string ProcessExecutebleFullName { get; set; } = string.Empty;
    public bool WaitForProcessActive { get; set; }
    public bool IsNew { get; internal set; }

    public bool Equals(ConfigPrefixPreset? other)
    {
        var x = this;
        var y = other;
        if (x is ConfigPrefixPreset a && y is ConfigPrefixPreset b)
        {
            return
                a.OutputFileNamePrefix == b.OutputFileNamePrefix &&
                a.ProcessExecutebleFullName == b.ProcessExecutebleFullName &&
                a.WaitForProcessActive == b.WaitForProcessActive;
        }

        return false;
    }

    public int GetHashCode([DisallowNull] ConfigPrefixPreset obj)
    {
        if (obj is not ConfigPrefixPreset a)
            return 0;

        // Combineer de relevante velden in een enkele hash.
        return HashCode.Combine(
            a.OutputFileNamePrefix,
            a.ProcessExecutebleFullName,
            a.WaitForProcessActive
        );
    }
}

