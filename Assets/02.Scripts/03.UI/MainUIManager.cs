using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    private static MainUIManager _instance;
    public static MainUIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("MainUI").GetComponent<MainUIManager>();
            return _instance;
        }
    }

    [SerializeField] private bool _isPlaying;
    [Header("CameraAngle")]
    [SerializeField] private Transform _cameraAngle;
    [Header("CaptureCount")]
    [SerializeField] private TMP_Text _captureCountText;

    private void Start()
    {
        MainGameManager.Instance.CaptureAction += UpdateCaptureCountUI;
        UpdateCaptureCountUI();
    }

    private void Update()
    {
        if (!_isPlaying) return;
        _cameraAngle.position = Input.mousePosition;
    }

    private void UpdateCaptureCountUI()
    {
        _captureCountText.text = $"{MainGameManager.Instance.CurCaptureCount} / {MainGameManager.Instance.MaxCaptureCount}";
    }
}
