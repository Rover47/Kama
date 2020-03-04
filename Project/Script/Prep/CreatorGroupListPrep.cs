using System.Collections.Generic;
using TSS;
using UnityEngine;
using UnityEngine.UI;

public class CreatorGroupListPrep : MonoBehaviour
{
    [SerializeField] GameObject _listButtonPrep;

    public TSSCore _coreScheme;
    public List<Button> _listButton;

    public TSSItem _groupList;
    public TSSItem _buttonOpen;
    public Button _buttonClose;

    public int _firstElement;
    public CreatorMenu _creatorMenu;

    public void AddButtonToCore(int numberGroup)
    {
        _firstElement = NamePrep.GetFirstIndex(numberGroup);
        _groupList = transform.GetChild(0).GetComponent<TSSItem>();
        _coreScheme = transform.parent.gameObject.GetComponent<TSSCore>();

        for (int i = 0; i < _listButtonPrep.transform.childCount; i++)
        {
            if (i < NamePrep._countPrepOnScheme[numberGroup])
            {
                int x = i + 1;
                _listButton.Add(_listButtonPrep.transform.GetChild(i).GetComponent<Button>());
                _listButton[i].onClick.AddListener(delegate { SelectPrep(x); });
                if (x < _coreScheme.Count) 
                    _coreScheme[x].AddItem(_listButton[i].GetComponent<TSSItem>());

                SetNamePrep(_listButtonPrep.transform.GetChild(i).gameObject, numberGroup, i);
            }
            else
            {
                _listButtonPrep.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        _buttonOpen.OpenBranchImmediately();
    }

    public void SetNamePrep(GameObject button, int numScheme, int numPrep)
    {
        _creatorMenu = FindObjectOfType<CreatorMenu>();

        int currentIndex = _firstElement + numPrep;
        button.GetComponentInChildren<Text>().text = _creatorMenu._linkPrep[currentIndex].NamePrep;
    }


    public void SelectPrep(int id)
    {
        _coreScheme.SelectState(id.ToString());
    }

    public void openGroupList()
    {
        _groupList.Open();
    } 

    public void closeGroupList()
    {
        _groupList.Close();
    }
}
