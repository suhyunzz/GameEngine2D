using UnityEngine;

public class BossEggCollector : MonoBehaviour
{
    [Header("보스 달걀 저장")]
    public int totalEggs = 0;   // 보스가 받은 달걀 누적

    // 플레이어가 달걀 전달 시 호출
    public void ReceiveEggs(int eggCount)
    {
        totalEggs += eggCount;
        Debug.Log("보스가 달걀 " + eggCount + "개 받음! 총: " + totalEggs);
    }
}
