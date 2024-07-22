using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoorPaperObject : MonoBehaviour, IDragHandler
{
    private DoorPanel _doorCanvas;
    private RectTransform _rect;
    private bool _isCanDrag;
    public bool IsAnimationFinish { get; private set; }

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _isCanDrag = false;
        IsAnimationFinish = false;
    }

    public void Init(DoorPanel doorCanvas)
    {
        _doorCanvas = doorCanvas;
        _rect.gameObject.SetActive(false);
    }

    public void ShowAnimation()
    {
        _rect.gameObject.SetActive(true);
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        float startPos = 600;
        float endPos = 100;
        float maxt = .5f;
        float t = 0;
        while (t < maxt)
        {
            t += Time.deltaTime;
            _rect.anchoredPosition = Vector3.up * Mathf.Lerp(startPos, endPos, 1 - (1 - t / maxt) * (1 - t / maxt));
            yield return null;
        }
        _rect.anchoredPosition = Vector3.up * endPos;
        _isCanDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isCanDrag) return;
        if (eventData.delta.y < -3f)
        {
            _isCanDrag = false;
            Debug.Log("Next");
            StartCoroutine(DragAnimation());
        }

    }

    private IEnumerator DragAnimation()
    {
        float startPos = 100;
        float endPos = -1000;
        float maxt = .5f;
        float t = 0;
        while (t < maxt)
        {
            t += Time.deltaTime;
            _rect.anchoredPosition = Vector3.up * Mathf.Lerp(startPos, endPos, t / maxt);
            yield return null;
        }
        _isCanDrag = true;
        IsAnimationFinish = true;

        _rect.gameObject.SetActive(false);
        _doorCanvas.CheckFinsh();
    }


}
