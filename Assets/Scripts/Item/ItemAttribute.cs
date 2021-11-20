using System;

[Flags]
public enum ItemAttribute
{
    none,
    isBook      = 1,     //本として読めるよ
    isImportant = 2 //だいじなもの
} 