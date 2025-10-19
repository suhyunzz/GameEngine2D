using UnityEngine;

public class Monster_Move : MonoBehaviour
{
    [Header("이동 설정")]
    public float speed = 1.0f;         // 이동 속도
    public float moveDistance = 5.0f;  // 총 이동할 거리 (5.0f만큼 왼쪽으로 이동 후 정지)

    private SpriteRenderer sr;      
    private Animator anim;                 
    private float totalDistanceMoved = 0f; // 총 이동한 거리 추적
    private bool isMoving = true;          // 현재 이동 중인지 여부

    void Start()
    {
        sr = GetComponent<SpriteRenderer>(); 
        anim = GetComponent<Animator>();     // 애니메이터 컴포넌트 가져오기 (Idle 상태 유지를 위해 별도 제어는 하지 않습니다.)

        // 몬스터가 5.0f만큼 왼쪽으로 이동합니다.
        Debug.Log("몬스터 단순 이동 시작! 5.0f만큼 왼쪽으로 이동 후 멈춥니다.");

        // NOTE: 애니메이션 제어 로직을 제거했습니다. 
        // Animator의 Entry State가 Idle 애니메이션으로 설정되어 있다면, 이동하는 동안 Idle 상태가 유지됩니다.
    }

    void Update()
    {
        if (isMoving)
        {
            // 한 프레임 동안 움직일 거리 계산
            float moveAmount = speed * Time.deltaTime;
            
            // 1. 실제 좌측 이동
            transform.Translate(Vector3.left * moveAmount);
            
            // 2. 이동 거리 누적
            totalDistanceMoved += moveAmount;

            // 3. 스프라이트 반전 (좌측 이동이므로 flipX = true)
            if (sr != null)
            {
                sr.flipX = true; 
            }

            // 4. 이동 거리 체크 및 정지
            if (totalDistanceMoved >= moveDistance)
            {
                isMoving = false;
                Debug.Log("몬스터가 목표 거리에 도달하여 정지했습니다.");
            }
        }
    }
}
