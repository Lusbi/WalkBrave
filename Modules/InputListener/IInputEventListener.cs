using UnityEngine;

public interface IInputEventListener
{
    void OnKeyDown(KeyCode keyCode); // ��L���U�ƥ�
    void OnKeyUp(KeyCode keyCode);   // ��L����ƥ�
    void OnMouseClick(int button, Vector3 position); // �ƹ��I���ƥ�
}
