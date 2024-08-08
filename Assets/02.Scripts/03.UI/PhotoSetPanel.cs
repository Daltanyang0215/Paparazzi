using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoSetPanel : UICanvasBase
{
    [Header("PhotoList")]
    [SerializeField] private Transform _photoListParent;
    [SerializeField] private PhotoElement _photoElement;
    [SerializeField] private ToggleGroup _photoToggleGroup;
    private List<PhotoElement> _photoElements;

    [Header("RequesterList")]
    [SerializeField] private Transform _requesterListParent;
    [SerializeField] private RequesterElement _requesterElement;
    [SerializeField]private ToggleGroup _requesterToggleGroup;
    [SerializeField] private Toggle _allApplyToggle;
    private Dictionary<RequesterType, RequesterElement> _requesterList;

    [Header("Preview")]
    [SerializeField] private Image _photoPreview;
    [SerializeField] private Transform _checkPanel;

    private CaptureData _CurPreview;
    private bool _previewChaging;

    private void Start()
    {
        _photoElements = new List<PhotoElement>();
        _requesterList = new Dictionary<RequesterType, RequesterElement>();
    }

    public void SetPreview(CaptureData curPreview)
    {
        _previewChaging = true;
        _CurPreview = curPreview;
        _photoPreview.sprite = curPreview.CaptureSprite;
        _requesterList[curPreview.RequesterType].ChangeToggleValue(true);
        _previewChaging = false;
    }

    public void SetPreviewRequester(RequesterType requesterType)
    {
        if (_previewChaging) return;
        if (_allApplyToggle.isOn)
        {
            foreach (PhotoElement element in _photoElements)
            {
                element.CaptureData.SetRequesterType(requesterType);
                element.MarkerUpdate();
            }
        }
        else
        {
            _CurPreview.SetRequesterType(requesterType);
            _photoElements.Find(x => x.CaptureData == _CurPreview).MarkerUpdate();
        }
    }

    public override void ShowPanel()
    {
        _allApplyToggle.isOn = false;
        base.ShowPanel();
        SetRepuesterList();
        SetPhotoList();
    }

    public void SetPhotoList()
    {
        for (int i = _photoElements.Count - 1; i >= 0; i--)
        {
            Destroy(_photoElements[i].gameObject);
        }
        _photoElements.Clear();

        foreach (CaptureData capture in MainGameManager.Instance.Captures)
        {
            PhotoElement add = Instantiate(_photoElement, _photoListParent);
            add.Init(capture,_photoToggleGroup);
            _photoElements.Add(add);
        }
        _photoElements[0].toggle.isOn = true;
        SetPreview(MainGameManager.Instance.Captures[0]);
    }

    public void SetRepuesterList()
    {
        // 토글 생성
        foreach (RequesterType value in System.Enum.GetValues(typeof(RequesterType)))
        {
            if (value == RequesterType.None) continue;
            if (_requesterList.ContainsKey(value)) continue;
            RequesterElement add = Instantiate(_requesterElement, _requesterListParent);
            add.Init(value, _requesterToggleGroup);
            _requesterList.Add(value, add);
        }
        // 토글 활성화
        foreach (RequesterType value in System.Enum.GetValues(typeof(RequesterType)))
        {
            if (value == RequesterType.None || value == RequesterType.Police) continue;

            // 경찰 제외 첫 만남도 이벤트에 포함된 영역으로 이벤트가 없다면 만남도 없었다는 것
            Debug.Log($"{value} / {MainGameManager.Instance.Requester[value].EventDatas[0].EventStartDay} / {SaveSystem.Save.CurSave.DayCount}");
            _requesterList[value].gameObject.SetActive(MainGameManager.Instance.Requester[value].EventDatas[0].EventStartDay <= SaveSystem.Save.CurSave.DayCount);
        }
        if (_requesterList.ContainsKey(RequesterType.None)) return;
        RequesterElement remove = Instantiate(_requesterElement, _requesterListParent);
        remove.Init(RequesterType.None, _requesterToggleGroup);
        remove.ChangeToggleValue(true);
        _requesterList.Add(RequesterType.None, remove);
    }

    public void OnNextPage(bool checkPanel)
    {
        if (!checkPanel)
        {
            foreach (CaptureData capture in MainGameManager.Instance.Captures)
            {
                if (capture.IsSetRequester == false)
                {
                    _checkPanel.gameObject.SetActive(true);
                    return;
                }
            }
        }
        _checkPanel.gameObject.SetActive(false);
        HidePanel();
        DaySystem.Instance.ChangeState(false);
    }
}
