using System.Collections;
using UnityEngine;

public class ItemCommand : IBattleCommand
{
    public IEnumerator Execute()
    {
        yield return null;
    }
}
