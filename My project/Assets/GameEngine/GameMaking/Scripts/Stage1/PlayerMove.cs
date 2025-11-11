using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("달걀 설정")]
    public int currentEggs = 0;   // 현재 소유 달걀 수
    public int maxEggs = 1;       // 최대 보유 달걀 수
    public int minEggs = 0;       // 최소 보유 달걀 수

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 이동 입력
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize(); // 대각선 이동 속도 보정
    }

    void FixedUpdate()
    {
        // Rigidbody2D로 이동
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    // 달걀 충돌 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
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

        if (collision.CompareTag("Stage1_Monster"))
        {
            if (currentEggs == maxEggs)
            {
                currentEggs -= 1;
                Debug.Log("달걀 감소! 현재: " + currentEggs);
            }
            else
            {
                Debug.Log("달걀 최소 보유량 도달!");
            }
        }
    }
}
