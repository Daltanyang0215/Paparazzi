using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorPanel : UICanvasBase
{
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private DoorPaperObject _doorObject;
    [SerializeField] private Image _doorEffect;
    [SerializeField] private DoorPaperObject _subObject;

    protected override void Awake()
    {
        base.Awake();
        _doorObject.Init(this);
        _subObject.Init(this);
        //TODO 디버그용, 나중에 지워야 됨
        canvas.enabled = true;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        MainUIManager.Instance.MemoPanel.ShowPanel();
        _dayText.enabled = false;

        StartCoroutine(DayCountAnimation());
        StartCoroutine(KnockAnimation());
    }

    public void CheckFinsh()
    {
        if (!_subObject.IsAnimationFinish) // 서브오브젝트까지 애니메이션이 완료되었는지 확인
        {
            _subObject.ShowAnimation();
            return;
        }
        DaySystem.Instance.ChangeState(false);
    }

    private IEnumerator DayCountAnimation()
    {
        yield return new WaitForSeconds(1);
        _dayText.text = DaySystem.Instance.DayCount.ToString() + " 일차";

        float t = 0;
        float maxt = 1;
        Color textColor = _dayText.color;
        _dayText.enabled = true;
        while (t < maxt)
        {
            textColor.a = Mathf.Lerp(0, 1, t / maxt);
            _dayText.color = textColor;
            t += Time.deltaTime;
            yield return null;
        }
        textColor.a = 1;
        _dayText.color = textColor;

        yield return new WaitForSeconds(0.5f);

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
    private IEnumerator KnockAnimation()
    {
        _doorEffect.enabled = false;
        yield return new WaitForSeconds(2);

        _doorEffect.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _doorEffect.enabled = false;
        yield return new WaitForSeconds(0.1f);
        _doorEffect.enabled = true;
        yield return new WaitForSeconds(0.1f);
        _doorEffect.enabled = false;
        yield return new WaitForSeconds(0.1f);


        _doorObject.ShowAnimation();
    }
}
