using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private TMP_Text _lastTime;

    public void SetSlot(int index)
    {
        int slotindex = index;
        GetComponent<Button>().onClick.AddListener(() => MainGameManager.Instance.StartGameToIndex(slotindex));

        if (SaveSystem.Save.SaveDatas[index] == null || SaveSystem.Save.SaveDatas[index].DayCount <= 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            _dayText.text = $"{SaveSystem.Save.SaveDatas[index].DayCount}����";
            _lastTime.text = $"������ �÷��� : {SaveSystem.Save.SaveDatas[index].LastTimeToString}";
        }

    }

}
