using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBack : MonoBehaviour
{

    Vector2 currValue = Vector2.zero;
    float xVelocity = 0.0F;
    float yVelocity = 0.0F;
    public float smoothTime = 0.3F;
    public float scale = 1f;

    RectTransform _thisRect;

    private void Start()
    {
        _thisRect = GetComponent<RectTransform>();
    }
    public void ListenerMethod(Vector2 value)
    {
        currValue = -value * scale;
        //Debug.Log("ListenerMethod: " + value.x.ToString());
    }

    private void Update()
    {
        float newPositionX = Mathf.SmoothDamp(_thisRect.anchoredPosition.x, currValue.x, ref xVelocity, smoothTime);
        float newPositionY = Mathf.SmoothDamp(_thisRect.anchoredPosition.y, currValue.y, ref yVelocity, smoothTime);
        _thisRect.anchoredPosition = new Vector2(newPositionX, newPositionY);
    }
}
