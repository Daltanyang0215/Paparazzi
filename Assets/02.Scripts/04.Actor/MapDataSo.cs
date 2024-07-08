using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MapDataSo", menuName = "Papa/MapDataSo")]
public class MapDataSo : ScriptableObject
{
    /// <summary>
    /// �ش� �ʿ��� ������ ��ҵ��� ����Ʈ
    /// </summary>
    [field: SerializeField] public List<ActorData> Actordata { get; private set; }

    /// <summary>
    /// �ش� �ʿ��� ���� �� Ÿ�ϵ��� �����͸���Ʈ
    /// </summary>
    [field: SerializeField] public List<ActorElement> TargetElements { get; private set; }

    public static void CreateMapDataSO(string DataName, List<MapActor> mapActors)
    {
        MapDataSo saveData = CreateInstance<MapDataSo>();

        saveData.Actordata = new List<ActorData>();

        foreach (var actorData in mapActors)
        {
            saveData.Actordata.Add(new ActorData(actorData.transform.position,
                                                 actorData.ActorData.IsFlip,
                                                 actorData.ActorData.IdleSprite,
                                                 actorData.ActorData.HightLightSprite,
                                                 actorData.ActorData.ActorElement));
        }

        AssetDatabase.CreateAsset(saveData, $"Assets/00.GameData/04.MapData/{DataName}.asset");
        AssetDatabase.Refresh();
    }
}

