using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotoElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _photoImage;
    [SerializeField] private Image _marker;
    
    public CaptureData CaptureData {  get; private set; }
    public void Init(CaptureData data)
    {
        CaptureData = data;

        _photoImage.sprite = CaptureData.CaptureSprite;
    }

    public void MarkerUpdate()
    {
        //TODO ���߿� ��Ŀ ������ �޾ƿͼ� ���� �ؾߵ�
        //_marker.sprite = CaptureData.RequesterType;

        _marker.color = CaptureData.RequesterType == RequesterType.None ? Color.red : Color.green;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MainUIManager.Instance.PhotoSetPanel.SetPreview(CaptureData);
    }

}
