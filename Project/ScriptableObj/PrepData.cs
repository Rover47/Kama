using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prep", menuName = "Prep Data", order = 52)]
public class PrepData : ScriptableObject
{
    [SerializeField]
    private string namePrep = "prep";
    [SerializeField]
    private string formaPrep = "forma";
    [SerializeField]
    private int indexPrep = -1;
    
    [SerializeField]
    public List<Link> maps = new List<Link>() { new Link() };

    public string NamePrep { get { return namePrep; } }

    public string FormaPrep { get { return formaPrep; } }

    public int IndexPrep { get { return indexPrep; } }

    [System.Serializable]
    public class Link
    {
        public int mainCore;
        public int secondCore;

        public Link()
        {
            mainCore = -1;
            secondCore = -1;
        }
    }

}
