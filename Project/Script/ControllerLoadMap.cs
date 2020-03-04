using System.Collections;
using System.Collections.Generic;
using TSS;
using UnityEngine;
using UnityEngine.UI;

public class ControllerLoadMap : MonoBehaviour
{
    public Text _textCounter;
    public GameObject _startBlackPanel;

    public LoaderMap[] _listLoader;
    public List<CreatorGroupListPrep> _listPrep;

    

    IEnumerator Start()
    {
        _startBlackPanel.SetActive(true);
        _textCounter.gameObject.SetActive(true);

        _listLoader = GetComponentsInChildren<LoaderMap>();

        for (int i = 0; i < _listLoader.Length; i++)
        {
            _textCounter.text = (i + 1).ToString();
            _listLoader[i].enabled = true;
            _listLoader[i].FunctionLoadContent();
            while (!_listLoader[i]._flagLoad)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        _textCounter.text = "ready";

        GameObject[] lw = GameObject.FindGameObjectsWithTag("LightWave");
        foreach (var item in lw)
        {
            item.GetComponent<TSSItem>().CloseBranchImmediately();
            item.GetComponent<TSSItem>().Open();
        }

        for (int i = 0; i < _listPrep.Count; i++)
        {
            _listPrep[i].AddButtonToCore(i);
        }

        this.GetComponent<TSSCore>().SelectState("1");
        _startBlackPanel.SetActive(false);
        _textCounter.gameObject.SetActive(false);
    }

    public void ClearListPrep()
    {
        _listPrep.Clear(); ;
    }

}
