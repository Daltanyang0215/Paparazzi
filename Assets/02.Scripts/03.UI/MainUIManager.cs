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
                _instance = GameObject.Find("MainUI").GetComponent<MainUIManager>();
            return _instance;
        }
    }

    [SerializeField] private bool _isPlaying;
    [Header("CameraAngle")]
    [SerializeField] private Transform _cameraAngle;
    [Header("CaptureCount")]
    [SerializeField] private TMP_Text _captureCountText;
    [Header("CaptureEffect")]
    [SerializeField] private Image _effectImage;
    [SerializeField] private AnimationCurve _effectColorCurve;
    [SerializeField] private float _fadeTime = 0.5f;
    [field: Header("TargetMemo")]
    [field :SerializeField] public MemoPanel MemoPanel { get; private set; }
    [Header("NextDay")]
    [SerializeField] private Button _nextDay;
    [field: SerializeField] public PhotoSetPanel PhotoSetPanel { get; private set; }
    [field: SerializeField] public CalculatePanel CalculatePanel { get; private set; }

    private void Start()
    {
        MainGameManager.Instance.CaptureAction += UpdateCaptureCountUI;
        MainGameManager.Instance.CameraChangeAction += CameraAngleEnable;
        MainGameManager.Instance.DayStartAction += CloseNextDayButton;
        MainGameManager.Instance.DayStartAction += CameraAngleEnable;
        MainGameManager.Instance.DayEndAction += ShowNextDayButton;
        _effectImage.enabled = false;
        _nextDay.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_isPlaying) return;
        _cameraAngle.position = Input.mousePosition;
    }

    private void UpdateCaptureCountUI()
    {
        _captureCountText.text = $"{MainGameManager.Instance.MaxCaptureCount - MainGameManager.Instance.CurCaptureCount} / {MainGameManager.Instance.MaxCaptureCount}";
        StopAllCoroutines();
        StartCoroutine(CpatureEffect());
    }

    private void CameraAngleEnable()
    {
        _cameraAngle.gameObject.SetActive(MainGameManager.Instance.IsCameraMove);
    }

    private IEnumerator CpatureEffect()
    {
        _effectImage.enabled = true;
        float t = 0;
        while (t < _fadeTime)
        {
            t += Time.deltaTime;
            _effectImage.color = new Color(1, 1, 1, _effectColorCurve.Evaluate(t / _fadeTime));
            yield return null;
        }
        _effectImage.enabled = false;
    }

    private void CloseNextDayButton()
    {
        _nextDay.gameObject.SetActive(false);
    }
    private void ShowNextDayButton()
    {
        _nextDay.gameObject.SetActive(true);
    }
}
