using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorPanel : MonoBehaviour
{
    private Canvas _canvas;
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private DoorPaperObject _doorObject;
    [SerializeField] private Image _doorEffect;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    public void ShowPanel(bool show)
    {
        _canvas.enabled = show;
        MainUIManager.Instance.MemoPanel.gameObject.SetActive(!show);
        _doorObject.Init(this);
        StartCoroutine(DayCountAnimation());
    }

    public void CheckFinsh()
    {
        if (!_doorObject.IsAnimationFinish) return;
        MainGameManager.Instance.ChangeNextState();
    }

    private IEnumerator DayCountAnimation()
    {
        _dayText.text = MainGameManager.Instance.DayCount.ToString() + " ÀÏÂ÷";
        _doorEffect.enabled = false;

        float t = 0;
        float maxt = 1;
        Color textColor = _dayText.color;
        while (t < maxt)
        {
            textColor.a = Mathf.Lerp(0, 1, t / maxt);
            _dayText.color = textColor;
            t += Time.deltaTime;
            yield return null;
        }
        textColor.a = 1;
        _dayText.color = textColor;
        yield return new WaitForSeconds(0.1f);

        _doorEffect.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _doorEffect.enabled = false;
        yield return new WaitForSeconds(0.1f);
        _doorEffect.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _doorEffect.enabled = false;

        yield return new WaitForSeconds(0.2f);

        t = 0;
        while (t < maxt)
        {
            textColor.a = Mathf.Lerp(1, 0, t / maxt);
            _dayText.color = textColor;
            t += Time.deltaTime;
            yield return null;
        }
        textColor.a = 0;
        _dayText.color = textColor;
    }
}
