using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SceneEntry : ScriptableObject
{
    public SceneCombination sceneCombinationToEnter;
    public Vector3 positionOnEntry;
    public Vector2 lookDirectionOnEntry;
    public Transform optionalTransformToFollow;
}
