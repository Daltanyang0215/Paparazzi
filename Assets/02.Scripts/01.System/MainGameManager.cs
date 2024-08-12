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
    public Dictionary<RequesterType, ActorElement> Targets { get; private set; }

    public Action CameraChangeAction;
    public Action CaptureAction;
    public Action DayEndAction;



    private void Start()
    {
        _camera = Camera.main;
        CaptureAction += CheckDailyFinish;

        Requester = new Dictionary<RequesterType, RequesterData>();
        Targets = new Dictionary<RequesterType, ActorElement>();
        foreach (RequesterType type in Enum.GetValues(typeof(RequesterType)))
        {
            if (type == RequesterType.None) continue;
            Requester.Add(type, _requesterDatas[(int)type - 1]);
        }
        DaySystem.Instance.states[DayState.Start].OnStateEnter();
    }

    public void StartGameToIndex(int index)
    {
        SaveSystem.Save.SetCurSaveIndex(index);
        DaySystem.Instance.ChangeState(DayState.Door, true);
    }

    public void ChangeState(bool isFade = false) => DaySystem.Instance.ChangeState(isFade);

    public void RandomSelectMapData()
    {
        CurMapData = _mapDataSos[UnityEngine.Random.Range(0, _mapDataSos.Count)];
        Targets[RequesterType.Police] = CurMapData.TargetElement;

        foreach (RequesterType type in Enum.GetValues(typeof(RequesterType)))
        {
            if (type == RequesterType.None || type == RequesterType.Police) continue;
            Targets[type] = Requester[type].GetEventData()?.EventTarget;
        }
    }

    public void DailyStart()
    {
        CurCaptureCount = 0;
        Captures.Clear();
        MapManager.Instance.ActorInit(CurMapData);
    }

    public void CameraCapture(Vector2 mousePos)
    {
        if (DaySystem.Instance.Current != DayState.Capture) return;
        if (CurCaptureCount >= MaxCaptureCount) return;
        CurCaptureCount++;

        RenderTexture render = new RenderTexture(Screen.width, Screen.height, 24);
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);

        _camera.targetTexture = render;
        _camera.Render();

        RenderTexture.active = render;
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        _camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(render);

        Sprite sprite = Sprite.Create(texture, new Rect(Mathf.RoundToInt(mousePos.x) - 360,
                                                        Mathf.RoundToInt(mousePos.y) - 240,
                                                        720,
                                                        480), Vector2.zero);

        List<MapActor> mapActors = new List<MapActor>();

        Vector2 posLB = Camera.main.ScreenToWorldPoint(mousePos - rect.size * .5f);
        Vector2 posRT = Camera.main.ScreenToWorldPoint(mousePos + rect.size * .5f);

        foreach (MapActor mapActor in MapManager.Instance.Actors)
        {
            if (mapActor.transform.position.x > posLB.x &&
                mapActor.transform.position.x < posRT.x &&
                mapActor.transform.position.y > posLB.y &&
                mapActor.transform.position.y < posRT.y)
            {
                mapActors.Add(mapActor);
            }
        }

        Captures.Add(new CaptureData(sprite, mapActors));

        CaptureAction?.Invoke();
    }

    private void CheckDailyFinish()
    {
        if (CurCaptureCount != MaxCaptureCount) return;
        //Debug.Log("Finish");
        DayEndAction?.Invoke();
    }

    public void GameExit()
    {
        Application.Quit();
    }

    [ContextMenu("DeleteSaveData")]
    private void DeleteData()
    {
        SaveSystem.DeleteData();
    }
}

[Serializable] // TODO 디버깅 용 , 나중에 시리얼라이즈 지워야 됨
public class CaptureData
{
    public CaptureData(Sprite captureSprite, List<MapActor> captureActor)
    {
        CaptureSprite = captureSprite;
        CaptureActors = captureActor;
    }

    // TODO 디버깅 용 , 나중에 시리얼라이즈 지워야 됨
    [field: SerializeField] public Sprite CaptureSprite { get; private set; }
    [field: SerializeField] public List<MapActor> CaptureActors { get; private set; }
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
    Secret,
    Agency,
    Rebel
}
