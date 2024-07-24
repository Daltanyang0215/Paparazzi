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
    [field: SerializeField] public bool IsCameraMove { get; set; }
    private Camera _camera;

    [field: Header("RequesterDatas")]
    [field: SerializeField] public Sprite NoneMarker { get; private set; }
    [SerializeField] private List<RequesterData> _requesterDatas;
    public Dictionary<RequesterType, RequesterData> Requester { get; private set; }
    public Dictionary<RequesterType, int> RequesterPoints { get; private set; }
    [field: Header("QuestDatas")]
    [SerializeField] private List<QuestDataSO> _qeustDatas;
    public QuestDataSO CurQuest { get; private set; }
    public Dictionary<RequesterType, int> LastQuestID { get; private set; }

    [field: Header("IngameDatas")]
    [field: SerializeField] public List<CaptureData> Captures { get; private set; } = new List<CaptureData>();
    [field: SerializeField] public byte MaxCaptureCount { get; private set; } = 5;
    public byte CurCaptureCount { get; private set; }

    [SerializeField] private List<MapDataSo> _mapDataSos;
    // TODO 나중에 데일리 데이터는 리스트로 하고 하다를 받아서 사용할거라 현재는 임시 방편
    public MapDataSo CurMapData { get; private set; }
    public ActorElement Target { get; private set; }

    public Action CameraChangeAction;
    public Action CaptureAction;
    public Action DayEndAction;


    public int HasCoin = 50;

    private void Start()
    {
        _camera = Camera.main;
        CaptureAction += CheckDailyFinish;

        Requester = new Dictionary<RequesterType, RequesterData>();
        RequesterPoints = new Dictionary<RequesterType, int>();
        foreach (RequesterType type in Enum.GetValues(typeof(RequesterType)))
        {
            if (type == RequesterType.None) continue;
            Requester.Add(type, _requesterDatas[(int)type - 1]);
            RequesterPoints.Add(type, 50);
        }

        // 로비 화면 없으니 넘어가, 나중에 수정해야 됨
        DaySystem.Instance.ChangeState(DayState.Door, true);
    }

    public void ChangeState(bool isFade = false) => DaySystem.Instance.ChangeState(isFade);

    public void RandomSelectMapData()
    {
        CurMapData = _mapDataSos[UnityEngine.Random.Range(0, _mapDataSos.Count)];
        Target = CurMapData.TargetElement;
        CurQuest = null;
        foreach (QuestDataSO quest in _qeustDatas)
        {
            if (quest.CheckCanStartQuest())
            {
                CurQuest = quest;
                break;
            }
        }
    }

    public void DailyStart()
    {
        CurCaptureCount = 0;
        Captures.Clear();
        MapManager.Instance.ActorInit(CurMapData);
    }

    public void CameraCapture(ActorElement element)
    {
        if (DaySystem.Instance.Current != DayState.Capture) return;
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
        //Debug.Log("Finish");
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

    public Sprite GetRequsterMarcker => RequesterType == RequesterType.None ? MainGameManager.Instance.NoneMarker :
        MainGameManager.Instance.Requester[RequesterType].RequsterMarker;

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
