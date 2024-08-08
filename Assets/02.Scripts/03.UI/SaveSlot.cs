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
        transform.GetChild(1).GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            SaveSystem.Save.SaveDatas[slotindex] = null;
            SaveSystem.SaveData();
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);

        });

        if (SaveSystem.Save.SaveDatas[index] == null || SaveSystem.Save.SaveDatas[index].DayCount <= 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            _dayText.text = $"{SaveSystem.Save.SaveDatas[index].DayCount}일차";
            _lastTime.text = $"마지막 플레이 : {SaveSystem.Save.SaveDatas[index].LastTimeToString}";
        }

    }

}
