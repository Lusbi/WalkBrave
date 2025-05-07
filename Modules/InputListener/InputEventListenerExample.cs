using UnityEngine;

public class InputEventListenerExample : MonoBehaviour, IInputEventListener
{
    private void OnEnable()
    {
        // 註冊到 InputEventManager
        InputEventManager.Instance.RegisterListener(this);
    }

    private void OnDisable()
    {
        // 從 InputEventManager 取消註冊
        InputEventManager.Instance.UnregisterListener(this);
    }

    public void OnKeyDown(KeyCode keyCode)
    {
        Debug.Log($"Key Down: {keyCode}");
    }

    public void OnKeyUp(KeyCode keyCode)
    {
        Debug.Log($"Key Up: {keyCode}");
    }

    public void OnMouseClick(int button, Vector3 position)
    {
        Debug.Log($"Mouse Button {button} Clicked at {position}");
    }
}
