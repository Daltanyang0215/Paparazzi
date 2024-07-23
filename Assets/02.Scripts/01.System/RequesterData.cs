using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RequesterData", menuName = "Papa/RequesterData")]
public class RequesterData : ScriptableObject
{
    [field: SerializeField] public string RequsterName { get; private set; }
    [field: SerializeField] public Sprite RequsterMarker { get; private set; }
    [field: SerializeField] public Color RequsterColor { get; private set; }
    [field: SerializeField] public int RequestTrust { get; private set; }
    [field: SerializeField] public int RequestReward{ get; private set; }
    [field: SerializeField] public int RequestPenalty { get; private set; }
}
