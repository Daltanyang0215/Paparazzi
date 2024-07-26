using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculateMoneyElement : MonoBehaviour
{
    public Toggle Toggle { get; private set; }
    private Transform _toggleImage;

    private TMP_Text _title;
    private TMP_Text _moneyValue;


    private void Awake()
    {
        Toggle = GetComponent<Toggle>();
        Toggle.enabled = false;
        _toggleImage = Toggle.targetGraphic.transform;
        _toggleImage.gameObject.SetActive(false);
        _title = transform.GetChild(1).GetComponent<TMP_Text>();
        _moneyValue = transform.GetChild(2).GetComponent<TMP_Text>();

        Toggle.onValueChanged.AddListener(x => MainUIManager.Instance.CalculatePanel.UpdateMoneyCalculate());
        gameObject.SetActive(false);
    }

    public void Init(string title, int value, bool isToogle)
    {
        gameObject.SetActive(true);

        _title.text = title;
        _moneyValue.text = value.ToString();

        Toggle.enabled = isToogle;
        _toggleImage.gameObject.SetActive(isToogle);
    }
    public void UpdateMoney(int value)
    {
        _moneyValue.text = value.ToString();
    }
}
