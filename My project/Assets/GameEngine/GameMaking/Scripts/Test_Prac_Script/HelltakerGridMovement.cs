using UnityEngine;

public class HelltakerGridMovement : MonoBehaviour
{
    [Header("그리드 설정")]
    [Tooltip("그리드 한 칸의 크기 (Unity Unit 기준)")]
    public float gridSize = 1f;

    
    [Tooltip("이동 애니메이션 속도. 높을수록 빨리 이동합니다.")]
    public float moveSpeed = 8f; 
    
    [Header("충돌 감지")]
    [Tooltip("충돌 감지 레이어 (벽, 돌 등)")]
    public LayerMask blockingLayer;

    private bool isMoving = false; // 현재 이동 중 (입력 잠금)
    private Vector3 targetPosition;
    
    private void Start()
    {
        // 초기 위치를 그리드에 정확히 맞춥니다.
        // Helltaker는 그리드 기반이므로 이 과정이 중요합니다.
        targetPosition = GetGridPosition(transform.position);
        transform.position = targetPosition;
    }

    private void Update()
    {
        // 이동 중이 아닐 때만 입력을 받습니다.
        if (!isMoving)
        {
            HandleInput();
        }

        // 부드러운 이동 처리 (애니메이션)
        MoveSmoothly();
    }

    /// <summary>
    /// 입력 처리 및 이동 시도
    /// </summary>
    private void HandleInput()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector2 direction = Vector2.zero;

        // 1. 대각선 입력 방지 및 방향 결정
        if (Mathf.Abs(inputX) > Mathf.Abs(inputY))
        {
            // X축 입력이 더 크거나 같으면 X축으로 이동
            direction = new Vector2(inputX, 0f);
        }
        else if (Mathf.Abs(inputY) > Mathf.Abs(inputX))
        {
            // Y축 입력이 더 크면 Y축으로 이동
            direction = new Vector2(0f, inputY);
        }
        // *두 입력이 정확히 같으면 (대각선) 방향=zero로 남아 이동하지 않습니다.

        if (direction != Vector2.zero)
        {
            AttemptMove(direction);
        }
    }

    /// <summary>
    /// 실제 이동 및 충돌/상호작용 처리
    /// </summary>
    private void AttemptMove(Vector2 direction)
    {
        Vector3 start = transform.position;
        // 다음 칸의 그리드 위치 계산
        Vector3 end = start + new Vector3(direction.x * gridSize, direction.y * gridSize, 0);

        // 충돌 검사
        // 플레이어 크기가 크다면 BoxCast나 CircleCast를 사용하는 것이 더 정확합니다.
        RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);

        if (hit.transform == null)
        {
            // 1. 빈 공간으로 이동
            StartMove(end, 1);
        }
        else
        {
            // 2. 물체와 충돌 (벽 또는 밀 수 있는 오브젝트)
            // (이 부분에서 물체 밀기, 턴 소모 등 Helltaker 퍼즐 로직을 구현해야 합니다.)
            // 예: MovableObject movable = hit.transform.GetComponent<MovableObject>();
            
            // 임시: 움직일 수 없는 벽으로 간주하고 이동을 막고 턴을 소모하지 않습니다.
            Debug.Log("장애물에 막혔습니다.");
            
            // 만약 제자리 걸음으로 턴을 소모시키고 싶다면:
            // currentTurns -= 1; 
            // Debug.Log($"남은 턴: {currentTurns} (제자리 걸음)");
        }
    }

    /// <summary>
    /// 이동 애니메이션 시작 및 턴 소모
    /// </summary>
    private void StartMove(Vector3 destination, int turnsCost)
    {
        isMoving = true;
        targetPosition = destination;
        
    }

    /// <summary>
    /// 그리드 위치로 부드럽게 이동 (이동 중인 것처럼 보이게)
    /// </summary>
    private void MoveSmoothly()
    {
        if (isMoving)
        {
            // 목표 지점까지 프레임당 moveSpeed만큼 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 목표 지점에 도달했는지 확인
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                // 위치를 목표 지점에 정확히 맞춥니다.
                transform.position = targetPosition;
                isMoving = false; // 이동 완료 -> 다음 입력 허용
                
                // TODO: 턴이 끝났을 때의 추가 로직 (예: 발판 효과 적용, 적 움직임 턴 처리)
            }
        }
    }

    /// <summary>
    /// 월드 좌표를 가장 가까운 그리드 좌표로 반환
    /// </summary>
    private Vector3 GetGridPosition(Vector3 worldPos)
    {
        float x = Mathf.Round(worldPos.x / gridSize) * gridSize;
        float y = Mathf.Round(worldPos.y / gridSize) * gridSize;
        return new Vector3(x, y, worldPos.z);
    }
}