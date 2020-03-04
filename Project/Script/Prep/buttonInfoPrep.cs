using System.Collections;
using System.Collections.Generic;
using TSS;
using UnityEngine;
using UnityEngine.UI;

public class buttonInfoPrep : MonoBehaviour
{
    public int _currPrep = 0;
    public TSSCore _coreInfo;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(pressButtonInfo);
    }

    public void pressButtonInfo()
    {
        _coreInfo.SelectState(_currPrep.ToString());
    }

    public void SetInfo(int i, TSSCore core)
    {
        _currPrep = i;
        _coreInfo = core;
    }
}
