using UnityEngine;

public class Monster_Move : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private Rigidbody2D rb; 
    private Animator anim;
    private SpriteRenderer sr; // 👈 SpriteRenderer 추가 (Flip을 위해)
    
    // 방향 전환을 위한 변수들
    private float direction = 1.0f; // 1.0f는 오른쪽, -1.0f는 왼쪽
    private float timeSinceDirectionChange = 0f;
    public float directionChangeInterval = 2.0f; // 👈 2초 간격 설정

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>(); 
        sr = GetComponent<SpriteRenderer>(); // 👈 SpriteRenderer 컴포넌트 가져오기
        
        // 초기 애니메이션 설정 (IDLE 유지)
        if (anim != null)
        {
            anim.SetFloat("Speed", 0f); 
        }
    }

    void FixedUpdate() 
    {
        // ----------------------------------------------------
        // 1. 2초마다 방향 전환 로직
        // ----------------------------------------------------
        
        // FixedUpdate는 Time.fixedDeltaTime을 사용합니다.
        timeSinceDirectionChange += Time.fixedDeltaTime; 
        
        if (timeSinceDirectionChange >= directionChangeInterval)
        {
            // 방향을 반전시킵니다. (1.0f -> -1.0f 또는 -1.0f -> 1.0f)
            direction *= -1; 
            
            // 타이머 초기화
            timeSinceDirectionChange = 0f;
        }

        // ----------------------------------------------------
        // 2. 이동 처리
        // ----------------------------------------------------

        // 현재 방향(direction)을 사용하여 이동 벡터를 생성
        Vector2 movement = new Vector2(direction * moveSpeed, rb.velocity.y);
        rb.velocity = movement;
        
        // ----------------------------------------------------
        // 3. 스프라이트 Flip 처리
        // ----------------------------------------------------
        if (sr != null)
        {
            if (direction > 0)
            {
                // 오른쪽으로 이동할 때 (원래 방향)
                sr.flipX = true; 
            }
            else if (direction < 0)
            {
                // 왼쪽으로 이동할 때 (뒤집기)
                sr.flipX = false; 
            }
        }
        
        // ----------------------------------------------------
        // 4. 애니메이션 강제 IDLE 유지 (요청 사항)
        // ----------------------------------------------------
        if (anim != null)
        {
            // 움직이고 있어도 IDLE 애니메이션을 강제로 유지
            anim.SetFloat("Speed", 0f); 
        }
    }
}