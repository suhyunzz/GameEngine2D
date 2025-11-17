using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Transform cam;              // 카메라의 Transform
    private Vector3 startPosition;      // 배경의 초기 위치
    
    [SerializeField] private float parallaxEffect;  // 패럴랙스 효과 강도
    
    void Start()
    {
        cam = Camera.main.transform;
        startPosition = transform.position;
    }

    void Update()
    {
        
    }

    void LateUpdate()
    {
        // 카메라 이동량에 비례하여 배경 이동
        float x = startPosition.x + cam.position.x * (1f-parallaxEffect);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}