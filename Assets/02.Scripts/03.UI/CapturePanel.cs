using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CapturePanel : UICanvasBase
{
    [Header("CameraAngle")]
    [SerializeField] private Transform _cameraAngle;
    [Header("CaptureCount")]
    [SerializeField] private TMP_Text _captureCountText;
    [Header("NextDay")]
    [SerializeField] private Button _nextDay;
    [Header("Effect")]
    [SerializeField] private Image _effectImage;
    [SerializeField] private AnimationCurve _effectColorCurve;
    [SerializeField] private float _fadeTime = 0.5f;


    protected override void Awake()
    {
        base.Awake();
        _effectImage.enabled = false;
        _nextDay.gameObject.SetActive(false);
        MainGameManager.Instance.CaptureAction += UpdateCaptureCountUI;
        MainGameManager.Instance.CameraChangeAction += CameraAngleEnable;
        MainGameManager.Instance.DayEndAction += ShowNextDayButton;
    }

    private void Update()
    {
        if(!canvas.enabled) return;
        _cameraAngle.position = Input.mousePosition;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        _nextDay.gameObject.SetActive(false);

        _captureCountText.text = $"{MainGameManager.Instance.MaxCaptureCount} /  {MainGameManager.Instance.MaxCaptureCount}";
        CameraAngleEnable();
    }

    public override void HidePanel()
    {
        base.HidePanel();
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
    private void ShowNextDayButton()
    {
        _nextDay.gameObject.SetActive(true);
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
}
