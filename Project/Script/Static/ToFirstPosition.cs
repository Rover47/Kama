using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToFirstPosition : MonoBehaviour
{

    public Vector2 _positionForContent;
    RectTransform _thisRect;

    void Start()
    {
        _thisRect = GetComponent<RectTransform>();
    }

    public void ResetPosition()
    {
        _thisRect.localScale = Vector3.one;
        _thisRect.pivot = new Vector2(0.5f, 0.5f);
        _thisRect.localPosition = _positionForContent;
    }
}
