using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculatePanel : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    [SerializeField] private Slider _requesterSlider;
    [SerializeField] private TMP_Text _requesterPoint;
    [SerializeField] private TMP_Text _requesterAddPoint;
    [SerializeField] private TMP_Text _hasCost;
    [SerializeField] private TMP_Text _requesterCost;
    [SerializeField] private TMP_Text _subCost;
    [SerializeField] private TMP_Text _totalCost;

    [Header("Test Data")]
    [SerializeField] private int _requestSliderValue;
    [SerializeField] private int _addCoinValue;
    [SerializeField] private int _subCoinValue;

    public void ShowPanel(bool show)
    {
        _canvas.enabled = show;
        if (!show) return;

        int addPoint = 0;
        int addCoin = 0;

        ActorElement target = MainGameManager.Instance.Target;

        foreach (CaptureData capture in MainGameManager.Instance.Captures)
        {
            // 폐기는 점수 제외
            if (capture.RequesterType == RequesterType.None) continue;



            if ((target.ActorType == ActorType.None || target.ActorType == capture.CaptureElement.ActorType) &&
                (target.ActorColor == ActorColor.None || target.ActorColor == capture.CaptureElement.ActorColor) &&
                (target.ActorPart == ActorPart.None || target.ActorPart == capture.CaptureElement.ActorPart))
            {
                addPoint += _requestSliderValue;
                addCoin += _addCoinValue;
            }
            else
            {
                addPoint -= _requestSliderValue;
            }
        }

        MainGameManager.Instance.RequesterPoints[RequesterType.Police] += addPoint;

        _requesterSlider.value = MainGameManager.Instance.RequesterPoints[RequesterType.Police] / 100f;
        _requesterPoint.text = MainGameManager.Instance.RequesterPoints[RequesterType.Police].ToString();
        _requesterAddPoint.text = addPoint.ToString();

        _hasCost.text = MainGameManager.Instance.HasCoin.ToString();
        _requesterCost.text = addCoin.ToString();
        _subCost.text = _subCoinValue.ToString();

        MainGameManager.Instance.HasCoin += addCoin + _subCoinValue;
        _totalCost.text = MainGameManager.Instance.HasCoin.ToString();

    }
}
