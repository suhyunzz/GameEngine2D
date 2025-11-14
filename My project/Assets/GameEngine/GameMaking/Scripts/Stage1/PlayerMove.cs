using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("리스폰 설정")] // 새로 추가: 리스폰 위치 설정
    public Vector3 respawnPosition = new Vector3(0, 0, 0); // 시작 지점 좌표 (유니티 인스펙터에서 설정)

    [Header("달걀 설정")]
    public int currentEggs = 0;    // 현재 소유 달걀 수
    public int maxEggs = 1;        // 최대 보유 달걀 수
    public int minEggs = 0;        // 최소 보유 달걀 수
    
    // 보스 상호작용 관련 변수
    private BossController nearbyBoss = null; // 근처에 있는 보스 컨트롤러 참조

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;
    }

    void Update()
    {
        // 1. 이동 입력
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); // 대각선 이동 속도 보정

        // 2. 보스에게 달걀 전달 입력 (수정된 부분)
        if (Input.GetKeyDown(KeyCode.Space) && nearbyBoss != null)
        {
            GiveEggToBoss();
        }
    }

    void FixedUpdate()
    {
        // Rigidbody2D로 이동
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    // 달걀 전달 메서드 (새로 추가된 부분)
    void GiveEggToBoss()
    {
        if (currentEggs > 0)
        {
            // 달걀을 보스에게 전달 시도
            if (nearbyBoss.ReceiveEgg())
            {
                currentEggs--;
                Debug.Log("보스에게 달걀 전달 성공! 현재: " + currentEggs);
            }
        }
        else
        {
            Debug.Log("전달할 달걀이 없습니다.");
        }
    }

    void Respawn()
        {
            // 1. 플레이어 위치를 리스폰 지점으로 이동
            transform.position = respawnPosition;
            
            // 2. 달걀 수 초기화 (필요하다면)
            currentEggs = minEggs; // 최소값 0으로 초기화
            
            // 3. Rigidbody 속도 초기화 (충돌 후 관성 제거)
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }

            Debug.Log("몬스터와 충돌하여 시작 지점(" + respawnPosition + ")으로 리스폰되었습니다. 달걀: " + currentEggs);
        }
        
    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 일반 달걀 획득 로직 (기존 코드)
        if (collision.CompareTag("Stage1_Egg"))
        {
            if (currentEggs < maxEggs)
            {
                currentEggs += 1;
                Debug.Log("달걀 획득! 현재: " + currentEggs);
            }
            else
            {
                Debug.Log("달걀 최대 보유량 도달!");
            }
        }

        // 몬스터 충돌 로직 (기존 코드)
        if (collision.CompareTag("Stage1_Monster"))
        {
            if (currentEggs == maxEggs)
            {
                currentEggs -= 1;
                Debug.Log("달걀 감소! 현재: " + currentEggs);
                Respawn();
            }
            else
            {
                Debug.Log("달걀이 없어요!!");
                Respawn();
            }
            // 이 부분은 보스가 아닐 경우만 처리하거나, 따로 데미지 로직을 분리하는게 좋습니다.
        }

        // 보스 진입 시 nearbyBoss 설정 (새로 추가된 부분)
        if (collision.CompareTag("Stage1_Boss"))
        {
            nearbyBoss = collision.GetComponent<BossController>();
            if (nearbyBoss != null)
            {
                Debug.Log("보스 근처에 진입했습니다. Space 키로 달걀을 전달할 수 있습니다.");
            }
        }
    }

    // 보스 구역 이탈 시 nearbyBoss 해제 (새로 추가된 부분)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stage1_Boss"))
        {
            if (nearbyBoss != null)
            {
                nearbyBoss = null;
                Debug.Log("보스 구역에서 벗어났습니다.");
            }
        }
    }
}