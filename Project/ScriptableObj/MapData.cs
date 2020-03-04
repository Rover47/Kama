using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MapData", menuName = "Map Data", order = 51)]
public class MapData : ScriptableObject
{
    [SerializeField]
    private string nameFolder;

    public string NameFolder
    {
        get
        {
            return nameFolder;
        }
    }

}
