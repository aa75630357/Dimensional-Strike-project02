using UnityEngine;

public class PlayerMovement:MonoBehaviour
{
    [Header("移動速度")]
    public float Speed = 5f;    //角色移動速度
    [Header("系統綁定()")]
    public ViewController viewCtrl;
    public Camera mainCam;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (mainCam == null) mainCam = Camera.main;
    }
    void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        if (viewCtrl == null || mainCam == null) return;
        // 讀取鍵盤 WASD 或方向鍵的輸入 (-1 到 1)
        float h = Input.GetAxisRaw("Horizontal"); // A/D 左/右
        float v = Input.GetAxisRaw("Vertical");   // W/S 前/後

        Vector3 moveDir = Vector3.zero;
        if (viewCtrl.is3DMode)
        {
            // 【3D 模式】：根據攝影機的面相來決定移動方向
            Vector3 camForward = mainCam.transform.forward;
            Vector3 camRight = mainCam.transform.right;// 鎖定 Y 軸，避免你看著天空按 W，人就跟著飛上天
            camForward.y = 0; 
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // 組合出最終的移動方向
            moveDir = camForward * v + camRight * h;
        }
        else
        {
            // 【2D 模式】：只能左右走 (螢幕的左右)
            // 這裡我們直接無視變數 'v' (W/S鍵)，鎖死深度移動！
            Vector3 camRight = mainCam.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            // 只有水平輸入(h)會產生方向
            moveDir = camRight * h; 
        }

        // 正規化，避免玩家同時按 W 和 D (斜角) 的時候走得比較快
        if (moveDir.magnitude > 1f) moveDir.Normalize();

        // 算出目標速度
        Vector3 targetVelocity = moveDir * Speed;
        
        // 【關鍵】：保留玩家原本在 Y 軸的速度 (為了地心引力掉落，或是未來的跳躍)
        targetVelocity.y = rb.linearVelocity.y;
        
        // 直接把速度覆蓋給 Rigidbody，這是最乾淨且不會被牆壁卡住的寫法！
        rb.linearVelocity = targetVelocity;
    }
}