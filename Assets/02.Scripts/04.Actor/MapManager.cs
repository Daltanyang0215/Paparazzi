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
    public List<MapActor> Actors { get; private set; }
    private Transform _actorParent;

    private void Start()
    {
        _actorParent = transform.Find("Actors");
        Actors = new List<MapActor>();
    }

    [ContextMenu("ActorInit")]
    public void ActorInit(MapDataSo mapdata)
    {
        foreach (MapActor actor in Actors)
        {
            Destroy(actor.gameObject);
        }
        Actors.Clear();

        for (int i = 0; i < mapdata.Actordata.Count; i++)
        {
            ActorData actorData = mapdata.Actordata[i];
            MapActor addActor = Instantiate(_mapActorPrefab, actorData.Position, Quaternion.identity, _actorParent);
            addActor.SetData(actorData, i);
            Actors.Add(addActor);
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
