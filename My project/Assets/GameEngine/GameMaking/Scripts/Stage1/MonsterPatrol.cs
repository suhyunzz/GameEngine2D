using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    public float moveSpeed = 5f;

    private int moveDirection = 1;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Rigidbody2D로 이동
        Vector2 newPos = rb.position + new Vector2(moveDirection * moveSpeed * Time.fixedDeltaTime, 0f);
        rb.MovePosition(newPos);

        // 스프라이트 뒤집기
        if (spriteRenderer != null)
            spriteRenderer.flipX = moveDirection == -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDirection *= -1;
        }
    }
}
