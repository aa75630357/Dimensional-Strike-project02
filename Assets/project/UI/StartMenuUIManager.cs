using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenuUIManager:MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // 解除鎖定
        Cursor.visible = true;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("WW");
    }
    public void HowToPlayGame()
    {
        SceneManager.LoadScene("howToWW");    
    }
}
