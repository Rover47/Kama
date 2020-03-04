//using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using TSS;
using UnityEngine;

public class ControllerStateCore : MonoBehaviour
{
    [SerializeField] TSSCore _firstCore;
    [SerializeField] List<TSSCore> _listSecondCore;
    [SerializeField] List<TSSItem> _listGroupPrep;
    public List<GameObject> _listForSiblink;

    [SerializeField] TSSProfile[] _listProfile;
    [SerializeField] string _prevState;
    [SerializeField] TSSItem _blur;
    [SerializeField] TSSItem _Info;


    public void LightWave()
    {
        foreach (var item in _listForSiblink) { item.transform.parent.GetChild(1).GetComponent<TSSItem>().Open(); }
    }

    #region scheme
    IEnumerator coroutineOpenScheme(int first)
    {
        foreach (var item in _listSecondCore) { if (item.gameObject.activeSelf) item.SelectDefaultState(); }
        foreach (var item in _listForSiblink) { item.GetComponent<TSSItem>().Close(); }
        yield return new WaitForSeconds(0.55f);
        _listForSiblink[first - 1].GetComponent<TSSItem>().Open();
        _listForSiblink[first - 1].transform.parent.SetAsLastSibling();
    }

    public void openScheme(int first)
    {
        CloseAllPrep();
        StopAllCoroutines();
        StartCoroutine(coroutineOpenScheme(first));
        _blur.Close();
        _Info.Close();
    }

    public void chooseScheme(int first)
    {
        _firstCore.SelectState(first.ToString());
    }
    #endregion

    #region Prep
    public void selectPrep(int first, int second)
    {
        _firstCore.SelectState(first.ToString());
        for (int i = 0; i < _listSecondCore.Count; i++)
        {
            if (first == i + 1)
            {
                _listSecondCore[i].SelectState(second.ToString());
                if (i < _listGroupPrep.Count)
                {
                    StartCoroutine(coroutineOpenListGroup(_listGroupPrep[i]));
                } 
            }
            else
            {
                _listSecondCore[i].SelectDefaultState();
                if (i < _listGroupPrep.Count) { _listGroupPrep[i].Close(); } 
            }
        }
    }

    public void closePrep()
    {
        foreach (var item in _listSecondCore)
        {
            item.SelectState("0");
        }
    }
    #endregion


    public void CloseAllPrep()
    {
        closePrep();
        foreach (var item in _listGroupPrep)
        {
            item.CloseBranch();
        }
    }

    IEnumerator coroutineOpenListGroup(TSSItem prepForOpen)
    {
        yield return new WaitForSeconds(1f);
        prepForOpen.Open();
    }

    public void openInfo()
    {
        _prevState = _firstCore.currentState.name;
        foreach (var item in _listForSiblink) { item.GetComponent<TSSItem>().Close(); }
        CloseAllPrep();
        _blur.Open();
        _Info.Open();
    }

    public void closeInfo()
    {
        _firstCore.SelectState(_prevState);
        _blur.Close();
        _Info.Close();
    }

    [ExecuteInEditMode]
    [ContextMenu("LoadListGroupPrep")]
    public void LoadListGroupPrep()
    {
        ControllerLoadMap _thisListPrep = this.GetComponent<ControllerLoadMap>();
        _thisListPrep._listPrep.Clear();
        _listGroupPrep.Clear();
        GameObject[] currArray = GameObject.FindGameObjectsWithTag("GroupListPrep");

        _listProfile = Resources.LoadAll<TSSProfile>(@"TSSProfile\ListPrep");

        int i = 0;
        foreach (GameObject item in currArray)
        {
            _listGroupPrep.Add(item.transform.GetChild(0).GetComponent<TSSItem>());
            _listGroupPrep[i].profile = _listProfile[i];
            TSSProfile.ProfileRevert(_listGroupPrep[i], _listProfile[i]);
            _thisListPrep._listPrep.Add(item.transform.GetComponent<CreatorGroupListPrep>());
            i++;
        }

    }
}
