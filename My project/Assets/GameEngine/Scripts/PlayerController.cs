using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5.0f;
    
    [Header("점프 설정")]
    public float jumpForce = 10.0f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private bool isGrounded = false;
    private int score = 0;  // 점수 추가
    private Vector3 startPosition;
void Start()
{
    rb = GetComponent<Rigidbody2D>();
    sr = GetComponent<SpriteRenderer>();
    anim = GetComponent<Animator>();
    
    // 게임 시작 시 위치를 저장 - 새로 추가!
    startPosition = transform.position;
    Debug.Log("시작 위치 저장: " + startPosition);

}
    
    void Update()
    {
        // 좌우 이동
        float moveX = 0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        //달리기 모드
        float currentMoveSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(moveX) > 0)
        {
            currentMoveSpeed = moveSpeed * 2f;
        }

        rb.velocity = new Vector2(moveX * currentMoveSpeed, rb.velocity.y);

        if (moveX > 0)
            sr.flipX = false;  // 오른쪽 바라보기
        else if (moveX < 0)
            sr.flipX = true;   // 왼쪽 바라보기

        anim.SetFloat("Speed", Mathf.Abs(moveX)); // moveX가 0이면 Idle, 0보다 크면 달리기

        // 점프 (지난 시간에 배운 내용)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Debug.Log("점프!");
            anim.SetBool("isJumping", true);
        }
        Debug.Log("현재 이동 속도" + currentMoveSpeed);
    }
    

    
    // 바닥 충돌 감지 (Collision)
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥 충돌 감지 (기존 코드)
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("바닥에 착지!");
            anim.SetBool("isJumping", false);
        }

        // 장애물 충돌 감지 - 새로 추가!
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("⚠️ 장애물 충돌! 시작 지점으로 돌아갑니다.");

            // 시작 위치로 순간이동
            transform.position = startPosition;

            // 속도 초기화 (안 하면 계속 날아감)
            rb.velocity = new Vector2(0, 0);
        }
    }
	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
		    Debug.Log("바닥에서 떨어짐");
            isGrounded = false;
            anim.SetBool("isJumping", true);
		}
	}

    // 아이템 수집 감지 (Trigger)
void OnTriggerEnter2D(Collider2D other)
    {
        // 코인 수집 (기존 코드)
        if (other.CompareTag("Coin"))
        {
            score += 1;
            Debug.Log("💰 코인 획득! 현재 점수: " + score);
            Destroy(other.gameObject);
        }
        
        // // 별 수집 (기존 코드)
        // if (other.CompareTag("Star"))
        // {
        //     score += 5;
        //     Debug.Log("⭐ 별 획득! +5점! 현재 점수: " + score);
        //     Destroy(other.gameObject);
        // }
        
        // 골 도달 - 새로 추가!
        if (other.CompareTag("Goal"))
        {
            Debug.Log("🎉🎉🎉 게임 클리어! 🎉🎉🎉");
            Debug.Log("최종 점수: " + score + "점");
            
            // 캐릭터 조작 비활성화
            enabled = false;
        }
    }
}