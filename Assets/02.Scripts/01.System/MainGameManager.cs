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
    [field: SerializeField] public MainGameState MainState { get; private set; }
    public FSMStateBase FSMState { get; private set; }
    [field: SerializeField] public int DayCount { get; private set; } = 0;
    [field: SerializeField] public bool IsCameraMove { get; set; }
    public Action CameraChangeAction;

    [field: SerializeField] public List<CaptureData> Captures { get; private set; } = new List<CaptureData>();
    private Camera _camera;

    [field: SerializeField] public byte MaxCaptureCount { get; private set; } = 5;
    public byte CurCaptureCount { get; private set; }

    [SerializeField] private List<MapDataSo> _mapDataSos;
    // TODO ���߿� ���ϸ� �����ʹ� ����Ʈ�� �ϰ� �ϴٸ� �޾Ƽ� ����ҰŶ� ����� �ӽ� ����
    public MapDataSo CurMapData { get; private set; }
    public ActorElement Target { get; private set; }

    public Action CaptureAction;
    public Action DayStartAction;
    public Action DayEndAction;

    public Dictionary<RequesterType, int> RequesterPoints { get; private set; }

    public int HasCoin = 50;

    private void Start()
    {
        _camera = Camera.main;
        CaptureAction += CheckDailyFinish;

        RequesterPoints = new Dictionary<RequesterType, int>();
        foreach (RequesterType type in Enum.GetValues(typeof(RequesterType)))
        {
            if (type == RequesterType.None) continue;
            RequesterPoints.Add(type, 50);
        }
        // �κ� ȭ�� ������ �Ѿ
        ChangeNextState();
    }

    public void ChangeNextState()
    {
        MainState = MainState == MainGameState.Ending ? MainGameState.Door : MainState + 1;
        Debug.Log($"Change State : {MainState}");

        switch (MainState)
        {
            case MainGameState.Start:
                break;
            case MainGameState.Intro:
                // ���̺� ������ �ٷ� �ѱ�
                ChangeNextState();
                break;
            case MainGameState.Door:
                DayCount++;
                CurMapData = _mapDataSos[UnityEngine.Random.Range(0, _mapDataSos.Count)]; 
                MainUIManager.Instance.FadeEffect(false);
                MainUIManager.Instance.DoorPanel.ShowPanel(true);
                Target = CurMapData.TargetElement;

                break;
            case MainGameState.News:
                MainUIManager.Instance.NewsPanel.SnowPanel(true);
                MainUIManager.Instance.MemoPanel.SetMemoList(Target);
                break;
            case MainGameState.Capture:
                MainUIManager.Instance.DoorPanel.ShowPanel(false);
                MainUIManager.Instance.NewsPanel.SnowPanel(false);
                MainUIManager.Instance.FadeEffect(false);
                DailyStart();
                break;
            case MainGameState.Requester:
                MainUIManager.Instance.PhotoSetPanel.ShowPanel();
                break;
            case MainGameState.Calculate:
                MainUIManager.Instance.CalculatePanel.ShowPanel(true);
                break;
            case MainGameState.Ending:
                MainUIManager.Instance.CalculatePanel.ShowPanel(false);
                ChangeNextState();
                break;
        }
    }


    public void DailyStart()
    {
        CurCaptureCount = 0;
        Captures.Clear();
        MapManager.Instance.ActorInit(CurMapData);
        DayStartAction?.Invoke();
    }

    public void CameraCapture(ActorElement element)
    {
        if (MainState != MainGameState.Capture) return;
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

[Serializable] // TODO ����� �� , ���߿� �ø�������� ������ ��
public class CaptureData
{
    public CaptureData(Sprite captureSprite, ActorElement captureElement)
    {
        CaptureSprite = captureSprite;
        CaptureElement = captureElement;
    }

    // TODO ����� �� , ���߿� �ø�������� ������ ��
    [field: SerializeField] public Sprite CaptureSprite { get; private set; }
    [field: SerializeField] public ActorElement CaptureElement { get; private set; }
    [field: SerializeField] public RequesterType RequesterType { get; private set; }

    // �߼����� ���� �ߴ��� Ȯ���ϱ� ���� �뵵
    public bool IsSetRequester { get; private set; } = false;

    public void SetRequesterType(int index) => SetRequesterType((RequesterType)index);
    public void SetRequesterType(RequesterType requesterType)
    {
        IsSetRequester = true;
        RequesterType = requesterType;
    }
}

//  ���߿� �߰� �ؾ� ��
public enum RequesterType
{
    None,
    Police,
}

public enum MainGameState
{
    Start,
    Intro,
    Door,
    News,
    Capture,
    Requester,
    Calculate,
    Ending,
}