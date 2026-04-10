using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialManager:MonoBehaviour
{
    public TextMeshProUGUI textLift;
    private int currentStep = 1;
    [Header("UI步驟")]
    public GameObject[] stepUIs;
    [Header("步驟 1：WASD 變色 UI (把畫布上的字拖進來)")]
    public TextMeshProUGUI textW;
    public TextMeshProUGUI textA;
    public TextMeshProUGUI textS;
    public TextMeshProUGUI textD;
    public Color pressedColor = Color.cyan; // 預設給淺藍色
    private bool pressedW = false;
    private bool pressedA = false;
    private bool pressedS = false;
    private bool pressedD = false;
    [Header("第二步驟：攻擊3D怪物")]
    public GameObject Enemy3D;
    [Header("第三步驟：生成2D怪物滾輪切2D視角")]
    public GameObject Enemy2D;
    [Header("第五步驟：QE轉向AD換前後")]
    public TextMeshProUGUI textQ;
    public TextMeshProUGUI textE;
    public TextMeshProUGUI text2A;
    public TextMeshProUGUI text2D;
    private bool pressedQ = false;
    private bool pressedE = false;
    private bool pressed2A = false;
    private bool pressed2D = false;
    public ViewController viewCtrl;


    void Start()
    {   
        PlayerShooter.CanShoot = false;
        if (Enemy3D != null)Enemy3D.SetActive(false) ;
        if (Enemy2D != null)Enemy2D.SetActive(false) ;
        UpdateStepUI();
    }
    void NextStep()
    {
        currentStep += 1;
        UpdateStepUI();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None; // 解除鎖定
            Cursor.visible = true;
            SceneManager.LoadScene("StartMenu");
        }

        if (currentStep == 1)
        {
            if (Input.GetKeyDown(KeyCode.W)) { textW.color = pressedColor; pressedW = true; }
            if (Input.GetKeyDown(KeyCode.A)) { textA.color = pressedColor; pressedA = true; }
            if (Input.GetKeyDown(KeyCode.S)) { textS.color = pressedColor; pressedS = true; }
            if (Input.GetKeyDown(KeyCode.D)) { textD.color = pressedColor; pressedD = true; }
            if (Input.GetMouseButtonDown(0)) NextStep();
            if (pressedW && pressedA && pressedS && pressedD)
            {
                NextStep();
            }
        }
        else if (currentStep == 2)
        {
            if (Enemy3D == null)
            {
                NextStep();
                PlayerShooter.CanShoot = false;
            }
        }
        else if (currentStep == 3)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                NextStep();
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (viewCtrl != null)
                {
                    viewCtrl.currentDist = 3.5f;
                }
                NextStep();
            }
        }
        else if (currentStep == 4)
        {
            if (viewCtrl.currentDist >= 7f)
            {
                NextStep();
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (viewCtrl != null)
                {
                    viewCtrl.currentDist = 7f;
                }
                NextStep();
            }
        }else if(currentStep == 5)
        {
            if(Enemy2D == null)
            {
                NextStep();
            }
        }else if(currentStep == 6)
        {
            if (Input.GetKeyDown(KeyCode.Q)) { textQ.color = pressedColor; pressedQ = true; }
            if (Input.GetKeyDown(KeyCode.E)) { textE.color = pressedColor; pressedE = true; }
            if (Input.GetKeyDown(KeyCode.A)) { text2A.color = pressedColor; pressed2A = true; }
            if (Input.GetKeyDown(KeyCode.D)) { text2D.color = pressedColor; pressed2D = true; }
            if (Input.GetMouseButtonDown(0))
            {
                viewCtrl.currentDist = 0;
                NextStep();
            }
            if(pressed2A && pressed2D && pressedQ && pressedE)
            {
                viewCtrl.currentDist = 0;
                NextStep();
            }
        }else if(currentStep == 7)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextStep();
            }
        }else if(currentStep == 8)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextStep();
            }
        }else if(currentStep == 9)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextStep();
            }
        }else if(currentStep == 10)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.None; // 解除鎖定
                Cursor.visible = true;
                SceneManager.LoadScene("StartMenu");
            }
        }
    }
    void UpdateStepUI()
    {
        for (int i = 0; i < stepUIs.Length; i++)
        {
            if (stepUIs[i] != null)
            {
                stepUIs[i].SetActive(i == currentStep - 1);
            }
        }
        if (Enemy3D != null && currentStep == 2)
        {
            Enemy3D.SetActive(true);
            PlayerShooter.CanShoot = true;
            textLift.enabled = false;
        }
        if (Enemy2D != null && currentStep == 3)
        {
            Enemy2D.SetActive(true);
            PlayerShooter.CanShoot = false;
            textLift.enabled = true;
        }
        if (currentStep == 5)
        {
            PlayerShooter.CanShoot = true;
            textLift.enabled = false;
        }
        if (currentStep == 6)
        {
            PlayerShooter.CanShoot = false;
            textLift.enabled = true;
        }
    }
}
