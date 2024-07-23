using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotoElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _photoImage;
    [SerializeField] private Image _marker;

    public CaptureData CaptureData { get; private set; }
    public void Init(CaptureData data)
    {
        CaptureData = data;

        _photoImage.sprite = CaptureData.CaptureSprite;
    }

    public void MarkerUpdate()
    {
        //TODO 나중에 마커 데이터 받아와서 변경 해야됨
        _marker.sprite = CaptureData.IsSetRequester ? CaptureData.GetRequsterMarcker : null;
        _marker.color = CaptureData.IsSetRequester ? Color.white : Color.clear;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MainUIManager.Instance.PhotoSetPanel.SetPreview(CaptureData);
    }

}
