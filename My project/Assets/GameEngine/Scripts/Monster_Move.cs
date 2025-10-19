using UnityEngine;

public class Monster_Move : MonoBehaviour
{
    public float moveSpeed = 5.0f; 
    private Rigidbody rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    // FixedUpdate()는 물리 연산 주기에 맞춰 호출되므로 Rigidbody 제어에 적합합니다.
    void FixedUpdate() 
    {
        // 1. 왼쪽 방향 벡터 (월드 좌표계의 X축 음수 방향)
        Vector3 leftMovement = Vector3.left * moveSpeed; 

        // 2. Rigidbody의 속도(velocity)를 설정하여 이동시킵니다.
        // Y축 속도는 현재 Rigidbody의 속도를 유지하여 중력 등이 계속 적용되도록 합니다.
        rb.velocity = new Vector3(leftMovement.x, rb.velocity.y, rb.velocity.z);

        // 오브젝트가 벽에 부딪히면 물리 엔진이 속도를 0으로 만들어 멈추게 합니다.
    }
}