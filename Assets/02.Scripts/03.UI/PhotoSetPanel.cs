using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoSetPanel : MonoBehaviour
{
    [Header("Page")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CalculatePanel _nextPage;

    [Header("PhotoList")]
    [SerializeField] private Transform _photoListParent;
    [SerializeField] private PhotoElement _photoElement;
    private List<PhotoElement> _photoElements;

    [Header("RequesterList")]
    [SerializeField] private Transform _requesterListParent;
    [SerializeField] private RequesterElement _requesterElement;
    private Dictionary<RequesterType, RequesterElement> _requesterList;

    [Header("Preview")]
    [SerializeField] private Image _photoPreview;
    [SerializeField] private Transform _checkPanel;

    private CaptureData _CurPreview;
    private ToggleGroup _toggleGroup;
    private bool _previewChaging;

    private void Start()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
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
        _CurPreview.SetRequesterType(requesterType);
        _photoElements.Find(x => x.CaptureData == _CurPreview).MarkerUpdate();
    }

    public void ShowPanel()
    {
        _canvas.enabled = true;
        SetRepuesterList();
        SetPhotoList();
    }

    public void SetPhotoList()
    {
        for (int i = _photoElements.Count-1; i >= 0; i--)
        {
            Destroy(_photoElements[i].gameObject);
        }

        foreach (CaptureData capture in MainGameManager.Instance.Captures)
        {
            PhotoElement add = Instantiate(_photoElement, _photoListParent);
            add.Init(capture);
            _photoElements.Add(add);
        }
        SetPreview(MainGameManager.Instance.Captures[0]);
    }

    public void SetRepuesterList()
    {

        foreach (RequesterType value in System.Enum.GetValues(typeof(RequesterType)))
        {
            if (value == RequesterType.None) continue;
            if (_requesterList.ContainsKey(value)) return;
            RequesterElement add = Instantiate(_requesterElement, _requesterListParent);
            add.Init(value, _toggleGroup);
            _requesterList.Add(value, add);
        }
        if (_requesterList.ContainsKey(RequesterType.None)) return;
        RequesterElement remove = Instantiate(_requesterElement, _requesterListParent);
        remove.Init(RequesterType.None, _toggleGroup);
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
        _canvas.enabled = false;
        _nextPage.ShowPanel();
    }
}
