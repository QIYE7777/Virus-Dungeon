using UnityEngine;

[CreateAssetMenu]
public class CardConfig : ScriptableObject
{
    public string id;

    public int price;
    public CardTier tier;
    public CardCategory category;
    public string title;
    public string desc;
    public Sprite sp;

    public CardParam param;
    public CardParam goldenParam;
}

[System.Serializable]
public class CardParam
{
    public int p1;
    public int p2;
}

