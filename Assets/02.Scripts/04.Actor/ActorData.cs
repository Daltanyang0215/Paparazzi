using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorData
{
    public ActorData(Vector3 position, bool isFlip, Sprite idleSprite, Sprite hightLightSprite, ActorElement actorElement)
    {
        Position = position;
        IsFlip = isFlip;
        IdleSprite = idleSprite;
        HightLightSprite = hightLightSprite;
        ActorElement = actorElement;
    }

    [field:Header("ActorPos")]
    [field: SerializeField] public Vector3 Position { get; private set; }
    
    
    [field: Header("ActorRender")]
    [field: SerializeField] public bool IsFlip { get; private set; }
    [field: SerializeField] public Sprite IdleSprite { get; private set; }
    [field: SerializeField] public Sprite HightLightSprite { get; private set; }

    [field: Header("ActorElement")]
    [field: SerializeField] public ActorElement ActorElement { get; private set; }
}

[System.Serializable]
public class ActorElement
{
    [field: SerializeField] public ActorType ActorType { get; private set; }
    [field: SerializeField] public ActorColor ActorColor { get; private set; }
    [field: SerializeField] public ActorPart ActorPart { get; private set; }
}
public enum ActorType
{
    None,
    Actor,
    Car,
    Object
}
public enum ActorPart
{
    None = 0,
    Smoke,
    Guitar,
    Bag,
    Umbrella,
    Phone,
    rooftop,
    Track,
}
public enum ActorColor
{
    None,
    Blue,
    Orange,
    Green,
    Glay
}
