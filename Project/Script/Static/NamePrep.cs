using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NamePrep
{
    public static string[] _namePrep = new string[] { 
        "Рак почки",
        "Колоректальный рак",
        "Меланома",
        "Рак легкого",
        "РМЖ",
        "РПЖ",};

    public static int[] _countPrepOnScheme = new int[] { 6, 1, 5, 9, 9, 4 };

    public static string GetName(int i)
    {
        return _namePrep[i];
    }
    public static int GetFirstIndex(int j)
    {
        int count = 0;
        for (int i = 1; i <= j; i++)
        {
            count += NamePrep._countPrepOnScheme[i - 1];
        }
        return count;
    }

}
