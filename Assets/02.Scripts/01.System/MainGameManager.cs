using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    private static MainGameManager _instance;
    public static MainGameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("GameManager").GetComponent<MainGameManager>();
            return _instance;
        }
    }

    [field: Header("MainState")]
    [field: SerializeField] public bool IsCameraMove { get; set; }
    public Action CameraChangeAction;

    [field: SerializeField] public List<CaptureData> Captures { get; private set; } = new List<CaptureData>();
    private Camera _camera;

    [field: SerializeField] public byte MaxCaptureCount { get; private set; } = 5;
    public byte CurCaptureCount { get; private set; }

    [SerializeField] private MapDataSo _mapDataSo;
    public ActorElement Target { get; private set; }

    public Action CaptureAction;
    public Action DayStartAction;
    public Action DayEndAction;

    public Dictionary<RequesterType,int> RequesterPoints { get; private set; }

    public int HasCoin = 50;

    private void Start()
    {
        _camera = Camera.main;
        CaptureAction += CheckDailyFinish;
        DailyStart();

        RequesterPoints = new Dictionary<RequesterType, int>();
        foreach (RequesterType type in Enum.GetValues(typeof(RequesterType)))
        {
            if (type == RequesterType.None) continue;
            RequesterPoints.Add(type, 50);
        }
    }

    public void DailyStart()
    {
        CurCaptureCount = 0;
        Captures.Clear();

        MapManager.Instance.ActorInit(_mapDataSo);
        Target = _mapDataSo.TargetElements[UnityEngine.Random.Range(0, _mapDataSo.TargetElements.Count)];
        MainUIManager.Instance.MemoPanel.SetMemoList(Target);
        DayStartAction?.Invoke();
    }

    public void CameraCapture(ActorElement element)
    {
        if (CurCaptureCount >= MaxCaptureCount) return;
        CurCaptureCount++;

        RenderTexture render = new RenderTexture(512, 512, 24);
        Rect rect = new Rect(0, 0, 512, 512);
        Texture2D texture = new Texture2D(512, 512, TextureFormat.RGBA32, false);

        _camera.targetTexture = render;
        _camera.Render();

        RenderTexture.active = render;
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        _camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(render);

        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);
        Captures.Add(new CaptureData(sprite, element));

        CaptureAction?.Invoke();
    }

    private void CheckDailyFinish()
    {
        if (CurCaptureCount != MaxCaptureCount) return;
        Debug.Log("Finish");
        DayEndAction?.Invoke();
    }

}

[Serializable] // TODO 디버깅 용 , 나중에 시리얼라이즈 지워야 됨
public class CaptureData
{
    public CaptureData(Sprite captureSprite, ActorElement captureElement)
    {
        CaptureSprite = captureSprite;
        CaptureElement = captureElement;
    }

    // TODO 디버깅 용 , 나중에 시리얼라이즈 지워야 됨
    [field: SerializeField] public Sprite CaptureSprite { get; private set; }
    [field: SerializeField] public ActorElement CaptureElement { get; private set; }
    [field: SerializeField] public RequesterType RequesterType { get; private set; }

    // 발송지를 선택 했는지 확인하기 위한 용도
    public bool IsSetRequester { get; private set; } = false;

    public void SetRequesterType(int index) => SetRequesterType((RequesterType)index);
    public void SetRequesterType(RequesterType requesterType)
    {
        IsSetRequester = true;
        RequesterType = requesterType;
    }
}

//  나중에 추가 해야 됨
public enum RequesterType
{
    None,
    Police,
}