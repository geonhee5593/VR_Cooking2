using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public float stageTime = 60f;
    public int goalOrders = 3;
    public int currentStage = 1;
    public int maxStage = 6;

    private float currentTime;
    private int completedOrders = 0;
    private bool stageRunning = true;

    public OrderManager orderManager;
    public UIManager uiManager;

    void Start()
    {
        currentStage = PlayerPrefs.GetInt("Stage", 1); 
        uiManager.UpdateStageText(currentStage);

        currentTime = stageTime;
        orderManager.SetStage(currentStage);
        orderManager.SpawnNewOrder();

    }

    void Update()
    {
        if (!stageRunning) return;

        currentTime -= Time.deltaTime;
        uiManager.UpdateTimer(currentTime);

        if (currentTime <= 0f)
        {
            EndStage(false);
        }
    }

    public void OnOrderCompleted()
    {
        completedOrders++;

        if (completedOrders >= goalOrders)
        {
            EndStage(true);
        }
        else
        {
            orderManager.SpawnNewOrder();
        }
    }
    public void OnClickNextStage()
    {
        int nextStage = currentStage + 1;
        PlayerPrefs.SetInt("Stage", nextStage);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void EndStage(bool success)
    {
        stageRunning = false;
        uiManager.ShowResult(success);
        Time.timeScale = 0f;

        if (success)
        {
            Debug.Log(" 스테이지 클리어!");

            if (currentStage < maxStage)
            {
                //currentStage++;
                //Invoke("ReloadScene", 2f); // 2초 후 다음 스테이지 시작
            }
            else
            {
                Debug.Log(" 모든 스테이지 완료!");
            }
        }
        else
        {
            Debug.Log(" 스테이지 실패");
        }

        Time.timeScale = 0f;
    }

    void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
