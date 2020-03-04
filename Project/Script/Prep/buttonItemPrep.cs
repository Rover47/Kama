using UnityEngine;
using UnityEngine.UI;

public class buttonItemPrep : MonoBehaviour
{

    public int _mainCore;
    public int _secondCore;

    public ControllerStateCore _controllerState;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(pressButtonPrep);
    }

    public void pressButtonPrep()
    {
        _controllerState.selectPrep(_mainCore, _secondCore);
    }

}
