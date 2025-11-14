using UnityEngine;

public class HomeZoneController : MonoBehaviour
{
    [Header("리스폰 위치 설정")]
    // 플레이어가 실제로 돌아갈 리스폰 좌표
    public Vector3 respawnPoint = new Vector3(0f, 0f, 0f); 

    // 플레이어가 이 영역 안에 있는지 확인하는 플래그
    private bool playerIsInside = false;

    // 플레이어 오브젝트의 transform을 저장
    private Transform playerTransform; 

    // Start는 한 번만 호출됩니다.
    void Start()
    {
        // 씬에서 "Player" 태그를 가진 오브젝트를 찾습니다.
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다. 태그를 확인하세요!");
        }
    }

    // 매 프레임 업데이트
    void Update()
    {
        // 1. 플레이어가 영역 안에 있고 (playerIsInside == true)
        // 2. Space 키가 눌렸다면
        if (playerIsInside && Input.GetKeyDown(KeyCode.Space))
        {
            WarpPlayerToRespawn();
        }
    }

    // 플레이어를 리스폰 지점으로 이동시키는 메서드
    void WarpPlayerToRespawn()
    {
        if (playerTransform != null)
        {
            // 플레이어의 위치를 리스폰 지점으로 변경
            playerTransform.position = respawnPoint;
            
            // Rigidbody2D가 있다면 속도도 초기화 (선택 사항)
            Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }

            Debug.Log("Home Zone에서 Space 키 입력! 리스폰 위치(" + respawnPoint + ")로 워프합니다.");
        }
    }

    // 플레이어가 콜라이더 영역에 들어왔을 때
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 태그 (일반적으로 "Player" 태그 사용)를 확인
        if (other.CompareTag("Player"))
        {
            playerIsInside = true;
            Debug.Log("플레이어가 Home Zone에 진입했습니다. Space 키를 누르면 리스폰합니다.");
        }
    }

    // 플레이어가 콜라이더 영역에서 나갔을 때
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInside = false;
            Debug.Log("플레이어가 Home Zone에서 이탈했습니다.");
        }
    }
}