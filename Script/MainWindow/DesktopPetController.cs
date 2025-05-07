using UnityEngine;
using UnityEngine.UI;

public class DesktopPetController : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveInterval = 2f; // 每次行動的間隔時間
    [SerializeField] private float idleTime = 3f; // 閒置時間
    [SerializeField] private float movementRange = 5f; // 移動範圍

    [Header("圖像設定")]
    [SerializeField] private Image petImage; // 寵物圖像
    [SerializeField] private Sprite idleSprite; // 閒置時顯示的圖片
    [SerializeField] private Sprite walkingSprite; // 行走時顯示的圖片
    [SerializeField] private float spriteFlipInterval = 0.3f; // 圖像切換間隔時間 (用於模擬動畫效果)

    [Header("交互設定")]
    [SerializeField] private GameObject interactionPanel;

    private enum PetState { Idle, Walking, Battle }
    private PetState currentState = PetState.Idle;
    private Vector3 targetPosition;
    private float stateTimer;
    private float spriteFlipTimer;
    private bool facingRight = true;

    // 用於記憶上一個狀態的時間，以便實現不規則的行為
    private float lastStateChangeTime;

    private void Awake()
    {
        // 確保有 Image 組件
        if (petImage == null)
            petImage = GetComponent<Image>();

        if (petImage == null)
        {
            Debug.LogError("DesktopPetController 需要一個 Image 組件！");
            enabled = false;
            return;
        }

        if (interactionPanel != null)
            interactionPanel.SetActive(false);
    }

    private void Start()
    {
        // 設置初始狀態
        ChangeState(PetState.Idle);
        lastStateChangeTime = Time.time;
        spriteFlipTimer = spriteFlipInterval;

        // 設置初始圖像
        if (idleSprite != null)
            petImage.sprite = idleSprite;
    }

    private void Update()
    {
        // 更新計時器
        stateTimer -= Time.deltaTime;
        spriteFlipTimer -= Time.deltaTime;

        // 根據當前狀態執行對應的行為
        switch (currentState)
        {
            case PetState.Idle:
                UpdateIdleState();
                break;
            case PetState.Walking:
                UpdateWalkingState();
                break;
            case PetState.Battle:
                UpdateBattleState();
                break;
        }
    }
    private void ChangeState(PetState newState)
    {
        if (currentState == newState)
            return;

        // 從上一個狀態退出
        switch (currentState)
        {
            case PetState.Idle:
                // 閒置狀態退出邏輯
                break;
            case PetState.Walking:
                // 行走狀態退出邏輯
                break;
            case PetState.Battle:
                // 戰鬥畫面退出邏輯
                break;
        }

        // 切換到新狀態
        currentState = newState;

        // 設定新狀態的計時器
        switch (newState)
        {
            case PetState.Idle:
                stateTimer = idleTime;
                if (idleSprite != null)
                    petImage.sprite = idleSprite;
                break;
            case PetState.Walking:
                stateTimer = moveInterval;
                SetRandomTargetPosition();
                if (walkingSprite != null)
                    petImage.sprite = walkingSprite;
                break;
            case PetState.Battle:
                stateTimer = idleTime;
                break;
        }

        lastStateChangeTime = Time.time;
    }

    private void UpdateIdleState()
    {
        if (stateTimer <= 0)
        {
            // 閒置時間結束，開始移動
            ChangeState(PetState.Walking);
        }
    }

    private void UpdateWalkingState()
    {
        // 移動朝向目標位置
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 朝向設定（根據移動方向翻轉圖像）
        if (targetPosition.x < transform.position.x && facingRight)
        {
            FlipImage(false);
        }
        else if (targetPosition.x > transform.position.x && !facingRight)
        {
            FlipImage(true);
        }

        // 模擬簡單的行走動畫效果（如果提供了多個行走圖像）
        if (walkingSprite != null && spriteFlipTimer <= 0)
        {
            spriteFlipTimer = spriteFlipInterval;
            // 這裡可以切換不同的行走圖像，如果有多個的話
        }

        // 檢查是否已經到達目標位置或移動時間結束
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f || stateTimer <= 0)
        {
            // 返回閒置狀態
            ChangeState(PetState.Idle);
        }
    }

    private void UpdateBattleState()
    {

    }

    private void FlipImage(bool toRight)
    {
        facingRight = toRight;
        Vector3 scale = petImage.rectTransform.localScale;
        scale.x = toRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        petImage.rectTransform.localScale = scale;
    }

    private void SetRandomTargetPosition()
    {
        // 只在水平方向隨機生成目標位置
        float x = Random.Range(transform.position.x - movementRange, transform.position.x + movementRange);
        targetPosition = new Vector3(x, transform.position.y, transform.position.z);

        // 確保不會移動到螢幕之外
        RectTransform canvasRect = petImage.canvas.GetComponent<RectTransform>();
        if (canvasRect != null)
        {
            float halfWidth = petImage.rectTransform.rect.width * 0.5f;
            float minX = halfWidth;
            float maxX = canvasRect.rect.width - halfWidth;
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        }
    }

    // 點擊寵物時的互動
    public void OnImageClick()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(!interactionPanel.activeSelf);
        }

        // 可以在這裡添加更多互動效果，如音效、圖像切換等
        Debug.Log("寵物被點擊了！");
    }

    public void SetParent(RectTransform rectTransform)
    {
        var _rect = GetComponent<RectTransform>();
        _rect.SetParent(rectTransform);
        _rect.anchoredPosition = Vector2.zero;
    }
}
