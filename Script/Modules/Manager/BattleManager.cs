using Mono.Cecil.Cil;
using UnityEngine;

public class BattleManager : GameCore.Singleton<BattleManager>, IInputEventListener
{
    private DesktopPetController _controllerPet;
    private EnemySpawn _enemySpawn;

    public BattleManager()
    {
        InputEventManager.Instance.RegisterListener(this);
        FindSceneObjects();
    }
    ~BattleManager()
    {
        InputEventManager.Instance.UnregisterListener(this);
    }

    private void FindSceneObjects()
    {
        _controllerPet = GameObject.FindAnyObjectByType<DesktopPetController>();
        _enemySpawn = GameObject.FindAnyObjectByType<EnemySpawn>();

        if (_controllerPet == null)
        {
            Debug.LogWarning("ControllerPet not found in the scene.");
        }

        if (_enemySpawn == null)
        {
            Debug.LogWarning("EnemySpawn not found in the scene.");
        }
    }

    public void OnKeyDown(KeyCode keyCode) => Hint(keyCode.ToString());

    public void OnKeyUp(KeyCode keyCode) => Hint(keyCode.ToString());

    public void OnMouseClick(int button, Vector3 position) => Hint();

    private void Hint(string key = null)
    {
        
    }
}
