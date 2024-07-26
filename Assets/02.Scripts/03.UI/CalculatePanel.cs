using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculatePanel : UICanvasBase
{
    [SerializeField] private Canvas _canvas;

    [Header("Trust")]
    [SerializeField] private Transform _trustParent;
    [SerializeField] private RequesterUIElement _requesterTrustPrefab;
    private Dictionary<RequesterType, RequesterUIElement> _truests;

    [Header("MoneyCalculate")]
    [SerializeField] private Transform _addMoneyParent;
    [SerializeField] private Transform _subMoneyParent;
    [SerializeField] private CalculateMoneyElement _totalMoneyElemeny;
    [SerializeField] private CalculateMoneyElement _moneyElemetPrefab;
    private List<CalculateMoneyElement> _addElements;
    private List<CalculateMoneyElement> _subElements;

    private List<int> _addVlaue;
    private List<int> _subVlaue;
    private int _totalVlaue;

    private void Start()
    {
        _truests = new Dictionary<RequesterType, RequesterUIElement>();
        foreach (RequesterType requester in System.Enum.GetValues(typeof(RequesterType)))
        {
            RequesterUIElement add = Instantiate(_requesterTrustPrefab, _trustParent);
            add.Init(requester);
            add.gameObject.SetActive(false);
            _truests[requester] = add;
        }

        _addElements = new List<CalculateMoneyElement>();
        for (int i = 0; i < 8; i++)
        {
            _addElements.Add(Instantiate(_moneyElemetPrefab, _addMoneyParent));
        }
        _subElements = new List<CalculateMoneyElement>();
        for (int i = 0; i < 4; i++)
        {
            _subElements.Add(Instantiate(_moneyElemetPrefab, _subMoneyParent));
        }
        _totalMoneyElemeny.Init("최종 소지금", 0, false);

        _addVlaue = new List<int>();
        _subVlaue = new List<int>();
    }

    public override void ShowPanel()
    {
        base.ShowPanel();

        foreach (CalculateMoneyElement element in _addElements)
        {
            element.gameObject.SetActive(false);
        }
        foreach (CalculateMoneyElement element in _subElements)
        {
            element.gameObject.SetActive(false);
        }
        _addVlaue.Clear();
        _subVlaue.Clear();

        // 고정 내용
        _addElements[0].Init("기존 소지금", MainGameManager.Instance.HasCoin, false);
        _addVlaue.Add(MainGameManager.Instance.HasCoin);
        _subElements[0].Init("집세", 20, false);
        _subVlaue.Add(20);

        Dictionary<RequesterType, int> requsterAddPoint = new Dictionary<RequesterType, int>();
        Dictionary<RequesterType, int> requsterAddCoin = new Dictionary<RequesterType, int>();

        foreach (RequesterType type in System.Enum.GetValues(typeof(RequesterType)))
        {
            requsterAddPoint.Add(type, 0);
            requsterAddCoin.Add(type, 0);
        }

        foreach (CaptureData capture in MainGameManager.Instance.Captures)
        {
            // 폐기는 점수 제외
            if (capture.RequesterType == RequesterType.None) continue;
            ActorElement target = MainGameManager.Instance.Targets[capture.RequesterType];

            if ((target.ActorType == ActorType.None || target.ActorType == capture.CaptureElement.ActorType) &&
                (target.ActorColor == ActorColor.None || target.ActorColor == capture.CaptureElement.ActorColor) &&
                (target.ActorPart == ActorPart.None || target.ActorPart == capture.CaptureElement.ActorPart))
            {
                requsterAddPoint[capture.RequesterType] += MainGameManager.Instance.Requester[capture.RequesterType].RequestDaltaTrust;
                requsterAddCoin[capture.RequesterType] += MainGameManager.Instance.Requester[capture.RequesterType].RequestReward;
            }
            else
            {
                requsterAddPoint[capture.RequesterType] -= MainGameManager.Instance.Requester[capture.RequesterType].RequestDaltaTrust;
            }
        }

        foreach (RequesterType type in System.Enum.GetValues(typeof(RequesterType)))
        {
            if (type == RequesterType.None) continue;

            if (ReferenceEquals(MainGameManager.Instance.Targets[type], null))
            {
                _truests[type].gameObject.SetActive(false);
                continue;
            }
            _truests[type].gameObject.SetActive(true);
            _truests[type].AddValue(requsterAddPoint[type]);
        }

        foreach (RequesterType type in System.Enum.GetValues(typeof(RequesterType)))
        {
            if (type == RequesterType.None) continue;

            if (ReferenceEquals(MainGameManager.Instance.Targets[type], null)) continue;
            if (requsterAddCoin[type] > 0)
            {
                // 비화성회된 오브젝트 탐색
                foreach (CalculateMoneyElement element in _addElements)
                {
                    if (element.gameObject.activeSelf) continue;
                    element.Init(type.ToString() + " Reward", requsterAddCoin[type], false);
                    _addVlaue.Add(requsterAddCoin[type]);
                    break;
                }
            }
            //else
            //{
            //    // 비화성회된 오브젝트 탐색
            //    foreach (CalculateMoneyElement element in _subElements)
            //    {
            //        if (element.gameObject.activeSelf) continue;
            //        element.Init(type.ToString() + " panulty", requsterAddCoin[type], false);
            //    }
            //}
        }
        
        UpdateMoneyCalculate();
    }

    public override void HidePanel()
    {
        MainGameManager.Instance.HasCoin = _totalVlaue;
        base.HidePanel();
    }

    public void UpdateMoneyCalculate()
    {
        int addmoney = 0;
        int submoney = 0;
        for (int i = 0; i < _addElements.Count; i++)
        {
            CalculateMoneyElement element = _addElements[i];
            // 토글 할 수 있는 항목이며 선택을 하지 않았다면 스킵
            if (!element.gameObject.activeSelf || element.Toggle.enabled && element.Toggle.isOn == false) continue;
            Debug.Log(i);
            addmoney += _addVlaue[i];
        }

        for (int i = 0; i < _subElements.Count; i++)
        {
            CalculateMoneyElement element = _subElements[i];
            // 토글 할 수 있는 항목이며 선택을 하지 않았다면 스킵
            if (!element.gameObject.activeSelf || element.Toggle.enabled && element.Toggle.isOn == false) continue;
            submoney += _subVlaue[i];
        }

        _totalVlaue = addmoney - submoney;
        _totalMoneyElemeny.UpdateMoney(_totalVlaue);
    }
}
