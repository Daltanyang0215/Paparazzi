using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LatterObject : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private RectTransform _rect;
    //[SerializeField] private TMP_Text _latterContents;
    private Vector2 _endPos;
    private Vector2 _grapOffset;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _endPos = _rect.anchoredPosition;
        _rect.anchoredPosition = _rect.anchoredPosition + Vector2.down * (Screen.height * .5f + _rect.anchoredPosition.y + _rect.sizeDelta.y);
    }

    //public void Init(string contents)
    //{
    //    _latterContents.text = contents;
    //    StartCoroutine(PaperAnimation());
    //}

    public void ShowAnimation()
    {
        StartCoroutine(PaperAnimation());
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        _grapOffset = (Vector2)transform.position - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + _grapOffset;
    }

    private IEnumerator PaperAnimation()
    {
        Vector2 startPos = _rect.anchoredPosition;
        float maxt = .5f;
        float t = 0;

        while (t < maxt)
        {
            t += Time.deltaTime;
            _rect.anchoredPosition = Vector2.Lerp(startPos, _endPos, 1 - (1 - t / maxt) * (1 - t / maxt));
            yield return null;
        }
        _rect.anchoredPosition = _endPos;
    }
}
