using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class LevelPrototype : ScriptableObject
{
    public string id;
    public List<RoomPrototype> rooms;
}
