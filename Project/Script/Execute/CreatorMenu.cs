using System.Collections.Generic;
using TSS;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CreatorMenu : MonoBehaviour
{
    [SerializeField] ControllerStateCore _controllerState;

    [Space]
    [SerializeField] GameObject _groupButton;
    [SerializeField] Button.ButtonClickedEvent[] _actions;

    [Space]
    [SerializeField] GameObject _coreInfoGo;
    [SerializeField] TSSCore _coreInfo;
    [SerializeField] GameObject _groupButtonPrep;
    [SerializeField]
    public PrepData[] _linkPrep;

    public List<_sortStructure> _listButtonPrep;

    [System.Serializable]
    public struct _sortStructure
    {
        public int _sotrIndexPrep;
        public GameObject _sortElement;

        public _sortStructure(int nn, GameObject gg)
        {
            _sotrIndexPrep = nn;
            _sortElement = gg;
        }
    }

    [ContextMenu("ReplaceSiblinrList")]
    public void ReplaceSiblinrList()
    {
        _controllerState._listForSiblink.Clear();
        foreach (Transform item in _groupButton.transform)
        {
            _controllerState._listForSiblink.Add(item.transform.GetChild(0).gameObject);
        }
    }

    [ContextMenu("dublicateMenu")]
    public void dublicateMenu()
    {
        DeleteOldDublicate(_groupButton);
        for (int i = 1; i < 6; i++)
        {
            GameObject currentbutton = Instantiate(_groupButton.transform.GetChild(0).gameObject, _groupButton.transform);
            RectTransform currentRect = currentbutton.GetComponent<RectTransform>();
            currentRect.anchoredPosition = new Vector2(currentRect.anchoredPosition.x, currentRect.anchoredPosition.y + (i * -250));

            Button btn = currentbutton.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick = _actions[i];
        }
        ReplaceSiblinrList();
    }

    #region delete methods
    private void DeleteOldDublicate(GameObject group)
    {
        for (int i = group.transform.childCount - 1; i > 0; i--)
        {
            DestroyImmediate(group.transform.GetChild(i).gameObject);
        }
    }
    private void DeleteOldInfo(GameObject group)
    {
        if (group.transform.childCount > 2)
            for (int i = group.transform.childCount - 1; i > 1; i--)
            {
                DestroyImmediate(group.transform.GetChild(i).gameObject);
            }
    }
    #endregion

    static List<_sortStructure> BubbleSort(List<_sortStructure> array)
    {
        var len = array.Count;
        for (var i = 1; i < len; i++)
        {
            for (var j = 0; j < len - i; j++)
            {
                if (array[j]._sotrIndexPrep > array[j + 1]._sotrIndexPrep)
                {
                    var temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }

        Debug.Log("Star Delete");
        for (var i = len-2; i >= 0 ; i--)
        {
            if (array[i]._sotrIndexPrep == array[i + 1]._sotrIndexPrep)
                array.RemoveAt(i + 1);
        }
        len = array.Count;

        for (var i = len-1 ; i >= 0; i--)
        {
            array[i]._sortElement.transform.SetAsFirstSibling();
        }
        return array;
    }

    [ContextMenu("loapPrepData")]
    public void loadPrepData()
    {
        DeleteOldInfo(_coreInfo.transform.gameObject);
        DeleteOldDublicate(_groupButtonPrep.transform.gameObject);
        for (int i = _coreInfo.Count - 1; i > -1; i--)
        {
            _coreInfo[i].Remove();
        }
        _linkPrep = Resources.LoadAll<PrepData>(@"ScriptableObj\Prep");
        checkInfoPrep(_coreInfo.transform.GetChild(1).gameObject, _groupButtonPrep.transform.GetChild(0).gameObject, 1);

        _listButtonPrep.Clear();
        _listButtonPrep.Add(new _sortStructure(_linkPrep[0].IndexPrep, _groupButtonPrep.transform.GetChild(0).gameObject));

        for (int i = 2; i < _linkPrep.Length + 1; i++)
        {
            GameObject currentbutton = Instantiate(_coreInfo.transform.GetChild(1).gameObject, _coreInfo.transform);
            GameObject currentInfoButton = Instantiate(_groupButtonPrep.transform.GetChild(0).gameObject, _groupButtonPrep.transform); //path for button
            checkInfoPrep(currentbutton, currentInfoButton, i);

            _listButtonPrep.Add(new _sortStructure(_linkPrep[i - 1].IndexPrep, currentInfoButton.gameObject));
        }

        _listButtonPrep = BubbleSort(_listButtonPrep);
        _coreInfo[0].SetAsDefault();
    }

    public void checkItemPrep(GameObject prep, int i)
    {
        buttonItemPrep itemPrep = prep.GetComponent<buttonItemPrep>();
        if (itemPrep == null)
        {
            itemPrep = prep.AddComponent<buttonItemPrep>();
        }
        itemPrep._controllerState = GetComponent<ControllerStateCore>();
        itemPrep._mainCore = _linkPrep[i].maps[0].mainCore;
        itemPrep._secondCore = _linkPrep[i].maps[0].secondCore;
        itemPrep.GetComponentInChildren<Text>().text = NamePrep._namePrep[_linkPrep[i].maps[0].mainCore - 1];//_linkPrep[i].FormaPrep;

        if (_linkPrep[i].maps.Count > 1)
        {
            //Debug.Log("double" + itemPrep.gameObject);
            CheckSecondButton(prep, i);
        }
    }

    public void CheckSecondButton(GameObject prep, int i)
    {
        prep.transform.parent.GetChild(1).gameObject.SetActive(true);
        buttonItemPrep itemPrep = prep.transform.parent.GetChild(1).GetComponent<buttonItemPrep>();
        if (itemPrep == null)
        {
            itemPrep = prep.AddComponent<buttonItemPrep>();
        }
        itemPrep._controllerState = GetComponent<ControllerStateCore>();
        itemPrep._mainCore = _linkPrep[i].maps[1].mainCore;
        itemPrep._secondCore = _linkPrep[i].maps[1].secondCore;
        itemPrep.GetComponentInChildren<Text>().text = NamePrep._namePrep[_linkPrep[i].maps[1].mainCore - 1];
    }

    public void checkInfoPrep(GameObject infoRightPrep, GameObject groupSlideLeft, int i)
    {
        Text lameLeftGroup = groupSlideLeft.GetComponentInChildren<Text>();
        lameLeftGroup.text = _linkPrep[i - 1].NamePrep;

        buttonInfoPrep infoPrep = groupSlideLeft.GetComponent<buttonInfoPrep>();
        if (infoPrep == null)
        {
            infoPrep = groupSlideLeft.AddComponent<buttonInfoPrep>();          
        }
        infoPrep.SetInfo(i, _coreInfo);
        _coreInfo.AddState(infoRightPrep.GetComponent<TSSItem>(), (i).ToString());
        _coreInfo[i - 1].AddItem(groupSlideLeft.GetComponent<TSSItem>());
        checkItemPrep(infoRightPrep.transform.GetChild(1).transform.GetChild(0).gameObject, i - 1); 

        setNames(infoRightPrep.transform.GetChild(0).gameObject, i - 1);
    }

    public void setNames(GameObject parentInfo, int i)
    {
        Text[] _allText = parentInfo.GetComponentsInChildren<Text>();
        _allText[2].text = _linkPrep[i].NamePrep;
        _allText[3].text = _linkPrep[i].FormaPrep;
    }


}
