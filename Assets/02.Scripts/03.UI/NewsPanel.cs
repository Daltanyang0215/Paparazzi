using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewsPanel : UICanvasBase
{
    [SerializeField] private RectTransform _newsTransfrom;

    [SerializeField] private TMP_Text _newsText;

    [SerializeField] private GameObject _nextButton;

    [SerializeField] private Transform _otherPaparParent;

    [SerializeField] private LatterObject _testObject;

    public override void ShowPanel()
    {
        base.ShowPanel();
        StartCoroutine(NewsAnimation());
    }

    private IEnumerator NewsAnimation()
    {
        _newsText.text = MainGameManager.Instance.CurMapData.TargetNewComant;
        _nextButton.SetActive(false);

        float startPos = -1100;
        float endPos = -50;
        float maxt = .5f;
        float t = 0;
        while (t < maxt)
        {
            t += Time.deltaTime;
            _newsTransfrom.anchoredPosition = Vector3.up * Mathf.Lerp(startPos, endPos, 1 - (1 - t / maxt) * (1 - t / maxt));
            yield return null;
        }

        _testObject.ShowAnimation();

        yield return new WaitForSeconds(.75f);

        _newsTransfrom.anchoredPosition = Vector3.up * endPos;
        _nextButton.SetActive(true);
        MainUIManager.Instance.MemoPanel.ShowPanel();
    }
}
