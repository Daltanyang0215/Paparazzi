using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        //MainGameManager.Instance.CameraChangeAction += CameraAngleEnable;
        MainGameManager.Instance.DayEndAction += ShowNextDayButton;
    }

    private void Update()
    {
        if (!canvas.enabled) return;

        Vector2 mousePos = Input.mousePosition;
        mousePos.x = Mathf.Clamp(mousePos.x, 360, Screen.width - 360);
        mousePos.y = Mathf.Clamp(mousePos.y, 240, Screen.height - 240);
        _cameraAngle.position = mousePos;




        if (Input.GetMouseButtonDown(1))
        {
            CameraAngleEnable(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            CameraAngleEnable(false);
        }
        else if (Input.GetMouseButton(1) && Input.GetMouseButtonDown(0))
        {
            // 대상을 체크하는 방법을 추가해줘야 하나?

            //Vector3 cameraPos = Camera.main.ScreenToWorldPoint(mousePos);
            //cameraPos.z = -5;
            //Camera.main.transform.position = cameraPos;
            MainGameManager.Instance.CameraCapture(mousePos);
        }

    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        _nextDay.gameObject.SetActive(false);

        _captureCountText.text = $"{MainGameManager.Instance.MaxCaptureCount} /  {MainGameManager.Instance.MaxCaptureCount}";
        CameraAngleEnable(false);
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

    private void CameraAngleEnable(bool isEnable)
    {
        _cameraAngle.gameObject.SetActive(isEnable);
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
