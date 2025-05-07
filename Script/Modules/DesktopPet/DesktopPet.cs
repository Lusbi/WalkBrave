using UnityEngine;

public class DesktopPet : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveInterval = 2f; // �C����ʪ����j�ɶ�
    private Vector3 targetPosition;
    private float moveTimer;

    void Start()
    {
        SetRandomTargetPosition();
        moveTimer = moveInterval;
    }

    void Update()
    {
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            SetRandomTargetPosition();
            moveTimer = moveInterval;
        }

        MoveTowardsTarget();
    }

    void SetRandomTargetPosition()
    {
        float x = Random.Range(-5f, 5f);
        targetPosition = new Vector3(x, transform.position.y, 0);
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetRandomTargetPosition();
        }
    }

    void OnMouseDown()
    {
        // ���I���d���ɡA����@�Ǥ��ʦ欰
        Debug.Log("�d���Q�I���F�I");
    }
}
