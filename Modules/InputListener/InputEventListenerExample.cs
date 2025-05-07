using UnityEngine;

public class InputEventListenerExample : MonoBehaviour, IInputEventListener
{
    private void OnEnable()
    {
        // ���U�� InputEventManager
        InputEventManager.Instance.RegisterListener(this);
    }

    private void OnDisable()
    {
        // �q InputEventManager �������U
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
