using System;

[Flags]
//周囲環境
public enum EnvironmentAttribute
{
    none,
    dark = 1,
    warm = 2,
    cold = 4
}