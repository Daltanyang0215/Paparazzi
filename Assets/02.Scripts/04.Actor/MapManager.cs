using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region Singleton
    private static MapManager _instance;
    public static MapManager Instance => _instance ?? (_instance = GameObject.Find("MapManager").GetComponent<MapManager>());
    #endregion

    [SerializeField] private MapActor _mapActorPrefab;

    private Transform _actorParent;

    private void Start()
    {
        _actorParent = transform.Find("Actors");
    }

    //[Header("Test")]
    //[SerializeField] private MapDataSo _testMapData;
    [ContextMenu("ActorInit")]
    public void ActorInit(MapDataSo mapdata)
    {
        for (int i = _actorParent.childCount - 1; i >= 0; i--)
        {
            Destroy(_actorParent.GetChild(i).gameObject);
        }

        foreach (ActorData actorData in mapdata.Actordata)
        {
            Instantiate(_mapActorPrefab, actorData.Position, Quaternion.identity, _actorParent).SetData(actorData);
        }
    }
#if UNITY_EDITOR

    #region ExportActorData
    [Header("ExportActorData")]
    [SerializeField] private string _saveMapNamem;

    [ContextMenu("ExportMapData")]
    public void ExportMapData()
    {
        List<MapActor> saveActor = new List<MapActor>();

        _actorParent = transform.Find("Actors");
        foreach (MapActor actor in _actorParent.GetComponentsInChildren<MapActor>())
        {
            saveActor.Add(actor);
        }

        MapDataSo.CreateMapDataSO(_saveMapNamem, saveActor);
    }
    #endregion

#endif
}
