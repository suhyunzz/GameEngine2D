using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject titleScreenPanel;  // 시작 화면 패널
    
    void Start()
    {
        // 게임 시작 시 시작 화면 표시
        ShowTitleScreen();
    }
    
    // 시작 화면 보이기
    void ShowTitleScreen()
    {
        titleScreenPanel.SetActive(true);
        Time.timeScale = 0f;  // 게임 일시정지
    }
    
    // 게임 시작 함수 (버튼에서 호출)
    public void StartGame()
    {
        titleScreenPanel.SetActive(false);  // 시작 화면 숨기기
        Time.timeScale = 1f;  // 게임 재개
    }
}