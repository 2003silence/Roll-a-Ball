using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text countText;   // 显示分数的文本
    public Text winText;     // 显示胜利信息的文本
    public Text timerText;   // 显示倒计时的文本
    public Text trashCountText; // 显示当前垃圾数量的文本

    private int count;       // 玩家分数
    public int trashCount = 6;  // 场上垃圾数量
    public int timeLimit = 60;  // 游戏总时间（秒）

    private bool isGameOver = false; // 标志游戏是否结束

    private void Start()
    {
        count = 0; // 初始化分数
        SetCountText();
        SetTrashCountText();
        winText.text = ""; // 初始化胜利文本
        StartCoroutine(CountdownTimer()); // 开始倒计时
    }

    // 已捡垃圾数量
    public void AddScore()
    {
        count++;
        SetCountText();
    }

    // 更新垃圾数量（增加）
    public void AddTrash()
    {
        trashCount++;
        SetTrashCountText();
    }

    // 更新垃圾数量（减少）
    public void RemoveTrash()
    {
        trashCount--;
        SetTrashCountText();
        // 检查是否达到过关条件
        if (trashCount <= 0)
        {
            WinGame();
        }
    }

    // 更新分数文本
    private void SetCountText()
    {
        countText.text = "已捡垃圾数量: " + count.ToString();
    }

    // 更新垃圾数量文本
    private void SetTrashCountText()
    {
        trashCountText.text = "场上垃圾数量: " + trashCount.ToString();
    }

    // 游戏胜利逻辑
    private void WinGame()
    {
        if (isGameOver) return; // 避免重复触发
        isGameOver = true;

        winText.text = "闯关成功!";
        StartCoroutine(LoadNextScene());
    }

    // 游戏失败逻辑
    private void LoseGame()
    {
        if (isGameOver) return; // 避免重复触发
        isGameOver = true;

        winText.text = "闯关失败!";
        StartCoroutine(LoadNextScene());
    }

    // 加载下一场景
    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2); // 等待2秒
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // 加载下一场景
    }

    // 倒计时逻辑
    private IEnumerator CountdownTimer()
    {
        int remainingTime = timeLimit; // 初始化剩余时间

        while (remainingTime > 0)
        {
            // 更新倒计时文本
            if (remainingTime > 60)
            {
                int minutes = remainingTime / 60;
                int seconds = remainingTime % 60;
                timerText.text = $"剩余时间: {minutes}分{seconds}秒";
            }
            else
            {
                timerText.text = $"剩余时间: {remainingTime}秒";
            }

            yield return new WaitForSeconds(1); // 每秒减少1
            remainingTime--;
        }

        // 倒计时结束，游戏失败
        LoseGame();
    }
}
