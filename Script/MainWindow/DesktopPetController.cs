using UnityEngine;
using UnityEngine.UI;

public class DesktopPetController : MonoBehaviour
{
    [Header("���ʳ]�w")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveInterval = 2f; // �C����ʪ����j�ɶ�
    [SerializeField] private float idleTime = 3f; // ���m�ɶ�
    [SerializeField] private float movementRange = 5f; // ���ʽd��

    [Header("�Ϲ��]�w")]
    [SerializeField] private Image petImage; // �d���Ϲ�
    [SerializeField] private Sprite idleSprite; // ���m����ܪ��Ϥ�
    [SerializeField] private Sprite walkingSprite; // �樫����ܪ��Ϥ�
    [SerializeField] private float spriteFlipInterval = 0.3f; // �Ϲ��������j�ɶ� (�Ω�����ʵe�ĪG)

    [Header("�椬�]�w")]
    [SerializeField] private GameObject interactionPanel;

    private enum PetState { Idle, Walking, Battle }
    private PetState currentState = PetState.Idle;
    private Vector3 targetPosition;
    private float stateTimer;
    private float spriteFlipTimer;
    private bool facingRight = true;

    // �Ω�O�ФW�@�Ӫ��A���ɶ��A�H�K��{���W�h���欰
    private float lastStateChangeTime;

    private void Awake()
    {
        // �T�O�� Image �ե�
        if (petImage == null)
            petImage = GetComponent<Image>();

        if (petImage == null)
        {
            Debug.LogError("DesktopPetController �ݭn�@�� Image �ե�I");
            enabled = false;
            return;
        }

        if (interactionPanel != null)
            interactionPanel.SetActive(false);
    }

    private void Start()
    {
        // �]�m��l���A
        ChangeState(PetState.Idle);
        lastStateChangeTime = Time.time;
        spriteFlipTimer = spriteFlipInterval;

        // �]�m��l�Ϲ�
        if (idleSprite != null)
            petImage.sprite = idleSprite;
    }

    private void Update()
    {
        // ��s�p�ɾ�
        stateTimer -= Time.deltaTime;
        spriteFlipTimer -= Time.deltaTime;

        // �ھڷ�e���A����������欰
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

        // �q�W�@�Ӫ��A�h�X
        switch (currentState)
        {
            case PetState.Idle:
                // ���m���A�h�X�޿�
                break;
            case PetState.Walking:
                // �樫���A�h�X�޿�
                break;
            case PetState.Battle:
                // �԰��e���h�X�޿�
                break;
        }

        // ������s���A
        currentState = newState;

        // �]�w�s���A���p�ɾ�
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
            // ���m�ɶ������A�}�l����
            ChangeState(PetState.Walking);
        }
    }

    private void UpdateWalkingState()
    {
        // ���ʴ¦V�ؼЦ�m
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // �¦V�]�w�]�ھڲ��ʤ�V½��Ϲ��^
        if (targetPosition.x < transform.position.x && facingRight)
        {
            FlipImage(false);
        }
        else if (targetPosition.x > transform.position.x && !facingRight)
        {
            FlipImage(true);
        }

        // ����²�檺�樫�ʵe�ĪG�]�p�G���ѤF�h�Ӧ樫�Ϲ��^
        if (walkingSprite != null && spriteFlipTimer <= 0)
        {
            spriteFlipTimer = spriteFlipInterval;
            // �o�̥i�H�������P���樫�Ϲ��A�p�G���h�Ӫ���
        }

        // �ˬd�O�_�w�g��F�ؼЦ�m�β��ʮɶ�����
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f || stateTimer <= 0)
        {
            // ��^���m���A
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
        // �u�b������V�H���ͦ��ؼЦ�m
        float x = Random.Range(transform.position.x - movementRange, transform.position.x + movementRange);
        targetPosition = new Vector3(x, transform.position.y, transform.position.z);

        // �T�O���|���ʨ�ù����~
        RectTransform canvasRect = petImage.canvas.GetComponent<RectTransform>();
        if (canvasRect != null)
        {
            float halfWidth = petImage.rectTransform.rect.width * 0.5f;
            float minX = halfWidth;
            float maxX = canvasRect.rect.width - halfWidth;
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        }
    }

    // �I���d���ɪ�����
    public void OnImageClick()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(!interactionPanel.activeSelf);
        }

        // �i�H�b�o�̲K�[��h���ʮĪG�A�p���ġB�Ϲ�������
        Debug.Log("�d���Q�I���F�I");
    }

    public void SetParent(RectTransform rectTransform)
    {
        var _rect = GetComponent<RectTransform>();
        _rect.SetParent(rectTransform);
        _rect.anchoredPosition = Vector2.zero;
    }
}
