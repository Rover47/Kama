using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public RectTransform _transformContent;
    RectTransform _thisRect;
    ScrollRect _scrollMap;

    public ResizeOnMouseWheel _OnMouse;

    void Start()
    {
        _scrollMap = _transformContent.gameObject.transform.parent.parent.GetComponent<ScrollRect>();
        _thisRect = GetComponent<RectTransform>();
        _OnMouse = _transformContent.GetComponent<ResizeOnMouseWheel>();
    }

    void Update()
    {
        float scl = _transformContent.GetComponent<RectTransform>().localScale.x;
        scl = 1 / scl;


        if (_scrollMap)
        {
            Vector2 perentWH = _transformContent.GetComponent<RectTransform>().rect.size;
            transform.localPosition = new Vector3((-_transformContent.localPosition.x) * scl - perentWH.x / 2,
                (-_transformContent.localPosition.y) * scl - perentWH.y / 2,
                0);
        }
        transform.localScale = new Vector3(scl, scl, scl);

        transform.GetChild(0).GetComponent<RectTransform>().localPosition = _transformContent.localPosition;
        transform.GetChild(0).GetComponent<RectTransform>().localScale = _transformContent.localScale;
    }
}
