using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTitlePanel : UICanvasBase
{
    [SerializeField] private Button _continueButton;

    [SerializeField] private Transform _slotParant;
    [SerializeField] private SaveSlot _slotPrefab;

    private void Start()
    {
        _continueButton.interactable = false;

        for (int i = 0; i < 3; i++)
        {
            Instantiate(_slotPrefab, _slotParant).SetSlot(i);
            if (SaveSystem.Save.SaveDatas[i] != null) _continueButton.interactable = true;
        }
        _slotParant.gameObject.SetActive(false);
    }

    public void SaveContinue()
    {
        MainGameManager.Instance.StartGameToIndex(SaveSystem.Save.LastSaveIndex);
    }
}
