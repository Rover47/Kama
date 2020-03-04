using UnityEngine;
using UnityEngine.UI;

public class ResizeOnMouseWheel : MonoBehaviour
{
    [SerializeField] float startSize = 1;
    [SerializeField] float minSize = 0.75f;
    [SerializeField] float maxSize = 1;

    [SerializeField] private float zoomRate = 5;

    public Vector3 difference;
    public Vector3 mousePos;
    public Touch[] _oldTouch; 
    float ZoomSpeedTouch = 0.1f;

    public RectTransform _thisRect;

    ScrollRect parentScrollRect;
    bool flagPivot = true;

    private void Start()
    {
        _thisRect = GetComponent<RectTransform>();
        parentScrollRect = transform.parent.parent.GetComponent<ScrollRect>();
    }

    public bool touching = false;
    Touch[] touches = new Touch[] { };

    private void LateUpdate()
    {
        if (touching && _oldTouch.Length > 1)
        {
            parentScrollRect.enabled = false;
            mousePos = (touches[0].position + touches[1].position) / 2;

            float newDistance = Vector2.Distance(touches[0].position, touches[1].position);
            float oldDistance = Vector2.Distance(_oldTouch[0].position, _oldTouch[1].position);
            float offset = newDistance - oldDistance;

            ChangeZoom(-offset / 100);
        }
        else
        {
            parentScrollRect.enabled = true;
            flagPivot = true;
        }
        _oldTouch = touches;
    }



    private void Update()
    {
        float scrollWheel = -Input.GetAxis("Mouse ScrollWheel");
        int touchCount = Input.touchCount;
            /*Touch[] */touches = Input.touches;

        touching = touchCount >= 2;

        if (touching && _oldTouch.Length < 2)
        {
            parentScrollRect.enabled = false;
            mousePos = (touches[0].position + touches[1].position) / 2;
            Vector2 localpoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_thisRect, mousePos, GetComponentInParent<Canvas>().worldCamera, out localpoint);

            Vector2 normalizedPoint = Rect.PointToNormalized(_thisRect.rect, localpoint);

            var tmpDelta = _thisRect.pivot - normalizedPoint;

            _thisRect.pivot = normalizedPoint;

            _thisRect.position = new Vector2(_thisRect.position.x - tmpDelta.x * _thisRect.sizeDelta.x * _thisRect.transform.localScale.x, 
                _thisRect.position.y - tmpDelta.y * _thisRect.sizeDelta.y * _thisRect.transform.localScale.y);
        }

        

        if (scrollWheel != 0)
        {
            ChangeZoom(scrollWheel);
        }     
    }

    private void ChangeZoom(float scrollWheel)
    {
        difference = Vector3.zero;
        float rate = 1 + 1 * Time.unscaledDeltaTime;
        if (scrollWheel > 0 && _thisRect.localScale.y > minSize)
        {
            SetZoom(Mathf.Clamp(_thisRect.localScale.y / rate, minSize, maxSize));
            difference = _thisRect.position - mousePos;
        }
        else if (scrollWheel < 0 && _thisRect.localScale.y < maxSize)
        {
            SetZoom(Mathf.Clamp(_thisRect.localScale.y * rate, minSize, maxSize));
            difference = _thisRect.position - mousePos;
        }
    }

    private void SetZoom(float targetSize)
    {
        _thisRect.localScale = new Vector3(targetSize, targetSize, 1);
    }
}