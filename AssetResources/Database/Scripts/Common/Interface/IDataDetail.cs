using UnityEngine;

public interface IDataDetail
{
    string DetailName { get; }

    string DetailDescription { get; }

    Sprite GetSprite();
}
