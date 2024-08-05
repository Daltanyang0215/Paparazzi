using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequesterUIElement : MonoBehaviour
{
    private TMP_Text _requesterName;
    private Slider _trustSlider;
    private TMP_Text _trustValue;
    private TMP_Text _trustDaltaValue;

    private RequesterType _requester;

    private void Awake()
    {
        _requesterName = transform.Find("Name").GetComponent<TMP_Text>();
        _trustSlider = transform.Find("Slider").GetComponent<Slider>();
        _trustValue = _trustSlider.transform.Find("Value").GetComponent<TMP_Text>();
        _trustDaltaValue = _trustSlider.transform.Find("AddValue").GetComponent<TMP_Text>();
    }

    public void Init(RequesterType requester)
    {
        _requester = requester;
        _requesterName.text = requester.ToString();
    }

    public void AddValue(int addPoint)
    {
        SaveSystem.Save.CurSave.RequesterPoints[_requester] += addPoint;

        _trustSlider.value = SaveSystem.Save.CurSave.RequesterPoints[_requester] * 0.01f;
        _trustValue.text = SaveSystem.Save.CurSave.RequesterPoints[_requester].ToString();
        _trustDaltaValue.text = (addPoint > 0 ? "+" : string.Empty) + addPoint.ToString();
    }


}
