using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequesterElement : MonoBehaviour
{
    private Toggle _toggle;
    private RequesterType _type;

    public void Init(RequesterType type, ToggleGroup group)
    {
        _type = type;
        _toggle = GetComponentInChildren<Toggle>();
        _toggle.group = group;
        GetComponentInChildren<TMPro.TMP_Text>().text = _type.ToString();

        transform.Find("Marker").GetComponent<Image>().sprite = type == RequesterType.None ? MainGameManager.Instance.NoneMarker : MainGameManager.Instance.Requester[type].RequsterMarker;
        
        _toggle.onValueChanged.RemoveAllListeners();
        _toggle.onValueChanged.AddListener((value) =>
        {
            if (!value) return;
            MainUIManager.Instance.PhotoSetPanel.SetPreviewRequester(_type);
        });
    }

    public void ChangeToggleValue(bool isOn) => _toggle.isOn = isOn;
}
