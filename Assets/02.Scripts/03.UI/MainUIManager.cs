using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    private static MainUIManager _instance;
    public static MainUIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("GameManager").GetComponent<MainUIManager>();
            return _instance;
        }
    }

    [SerializeField] private bool _isPlaying;
    [Header("CameraAngle")]
    [SerializeField] private Transform _cameraAngle;
    [Header("CaptureCount")]
    [SerializeField] private TMP_Text _captureCountText;
    [Header("CaptureEffect")]
    [Header("NextDay")]
    [SerializeField] private Button _nextDay;

    [field: Header("StatePanel")]
    [field: SerializeField] public DoorPanel DoorPanel { get; private set; }
    [field: SerializeField] public NewsPanel NewsPanel { get; private set; }
    [field: SerializeField] public CapturePanel CapturePanel { get; private set; }
    [field: SerializeField] public PhotoSetPanel PhotoSetPanel { get; private set; }
    [field: SerializeField] public CalculatePanel CalculatePanel { get; private set; }

    [field: Header("SystemPanel")]
    [field: SerializeField] public MemoPanel MemoPanel { get; private set; }
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeTime = 0.5f;
    private bool IsFading = false;

    private void Update()
    {
        if (!_isPlaying) return;
        _cameraAngle.position = Input.mousePosition;
    }
    public void FadeEffect(Action fadeAfterAction = null)
    {
        if (IsFading) return;
        StartCoroutine(FadeInOutEffect(fadeAfterAction));
    }

    private IEnumerator FadeInOutEffect(Action fadeAfterAction)
    {
        IsFading = true;
        float t = 0;
        while (t < _fadeTime)
        {
            t += Time.deltaTime;
            _fadeImage.color = new Color(0, 0, 0, (t / _fadeTime));
            yield return null;
        }
        _fadeImage.color = Color.black;
        fadeAfterAction?.Invoke();

        t = 0;
        while (t < _fadeTime)
        {
            t += Time.deltaTime;
            _fadeImage.color = new Color(0, 0, 0, 1 - (t / _fadeTime));
            yield return null;
        }
        _fadeImage.color = Color.clear;
        IsFading = false;
    }
}
