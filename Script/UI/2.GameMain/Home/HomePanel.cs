using PrimeTween;
using UnityEngine;

public class HomePanel : PanelBase
{
    [SerializeField] private RectTransform m_petParent;
    [SerializeField] private RectTransform m_petTransform;
    [SerializeField] private float m_moveDuration = 1.0f;
    [SerializeField] private Vector2 m_limitPosX;

    private Vector3 m_targetPosition;

    public override void ActiveOn()
    {
        uiGameMainView.desktopPetController.SetParent(m_petParent);
        uiGameMainView.desktopPetController.ChangeState(PetState.Walking);
    }

    public void MovePetTo(Vector3 targetPosition)
    {
        // Clamp the target position within the specified X-axis limits
        float clampedX = Mathf.Clamp(targetPosition.x, m_limitPosX.x, m_limitPosX.y);
        float currentX = m_petParent.anchoredPosition.x;

        // Ensure the movement is at least 100 units
        if (Mathf.Abs(clampedX - currentX) < 100)
        {
            clampedX = currentX + (clampedX > currentX ? 100 : -100);
            clampedX = Mathf.Clamp(clampedX, m_limitPosX.x, m_limitPosX.y);
        }

        m_targetPosition = new Vector3(clampedX, targetPosition.y, targetPosition.z);

        // Tween the pet's position
        float start = currentX;
        float end = clampedX;
        Tween.UIAnchoredPositionX(m_petParent, start, end, m_moveDuration, Ease.InOutQuad);
    }

    public void SetMoveDuration(float duration)
    {
        m_moveDuration = duration;
    }

    [SerializeField] private float m_moveInterval = 2.0f; // Time interval for movement
    private float m_timeAccumulator = 0.0f;

    public override void Tick(float deltaTime)
    {
        m_timeAccumulator += deltaTime;

        if (m_timeAccumulator >= m_moveInterval)
        {
            m_timeAccumulator = 0.0f;

            // Trigger movement logic
            Vector3 randomTargetPosition = new Vector3(
                Random.Range(m_limitPosX.x, m_limitPosX.y),
                m_petParent.anchoredPosition.y,
                0
            );
            MovePetTo(randomTargetPosition);
        }
    }
}
