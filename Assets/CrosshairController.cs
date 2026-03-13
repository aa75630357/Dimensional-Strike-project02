using UnityEngine;

public class CrosshairController:MonoBehaviour
{
    [Header("綁定")]
    public ViewController viewCtrl;
    private RectTransform rectTransform;
    void Start()
    {   // 專業定義：獲取 UI 專用的 Transform 組件。
        // 白話文：去抓取這個準星 UI 自己身上的座標控制器，我們等下要強制移動它。
        rectTransform = GetComponent<RectTransform>();
        // 專業定義：隱藏作業系統的滑鼠游標。
        // 白話文：把 Windows 系統那個白色的滑鼠箭頭變不見，這樣畫面上才不會有「準星+游標」兩個東西打架。
        Cursor.visible = false;
    }
    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (viewCtrl == null) return;
        if (viewCtrl.is3DMode)
        {
            // 【3D 模式】：鎖定模式
            // 專業定義：將 UI 的錨點座標歸零。
            // 白話文：強制把紅點準星吸回螢幕的正中心！
            rectTransform.anchoredPosition = Vector2.zero;

            // 專業定義：鎖定游標在視窗中心。
            // 白話文：把隱形的滑鼠游標「銬」在遊戲視窗正中間。這是 3D FPS 遊戲必寫的語法，不然你轉身太大力滑鼠會飛出去！
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            // 【2D 模式】：自由瞄準模式
            // 專業定義：讀取 Input.mousePosition 像素座標並賦值給 UI 座標。
            // 白話文：抓取你實體滑鼠現在停在螢幕上的哪裡，然後把紅點準星直接「瞬移」過去貼著它！
            rectTransform.position = Input.mousePosition;

            // 專業定義：解除游標鎖定。
            // 白話文：解開滑鼠的手銬，讓它可以自由在畫面上亂滑，這樣你才能在 2D 畫面裡瞄準四面八方的怪物。
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
