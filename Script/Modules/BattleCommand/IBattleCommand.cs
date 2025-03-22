
using System.Collections;

public interface IBattleCommand
{
    /// <summary>
    /// 執行動作，等待回傳
    /// </summary>
    /// <returns></returns>
    IEnumerator Execute();
}
