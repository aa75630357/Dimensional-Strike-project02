using UnityEngine;

public class ViewController : MonoBehaviour
{
    public Transform cameraPivot; 
    public Camera mainCam; 

    [Header("縮放設定")]
    public float zoomspeed   = 5f;
    public float maxDistance = 15f;
    public float minDistance = 0f;
    private float xRotation  = 0f; // 紀錄抬頭/低頭的角度
    public float switch3D2D  = 1.0f; // 低於這個距離會切換成 3D

    [Header("狀態")]
    public bool is3DMode = true;
    public float currentDist = 0f; 
    private float targetYAngle = 0f;
    
    // 【設定】統一相機高度，避免切換時上下跳
    private float camHeight = 0.6f; 

    void Start()
    {
        currentDist = 0f;
        SetMod3D();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (is3DMode)   //控制3D視角和2D視角(瞄準)
        {
            // --- 這裡是補上的 3D 滑鼠轉動 ---
            float mouseX = Input.GetAxis("Mouse X") * 2f;
            float mouseY = Input.GetAxis("Mouse Y") * 2f;

            // 左右看：直接改變 targetYAngle，這樣切換 2D 時才不會亂掉
            targetYAngle += mouseX;

            // 上下看：控制抬頭低頭，並限制角度避免脖子斷掉 (看向正上或正下)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);

            // 讓相機上下看 (注意這裡只轉相機本身)
            mainCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    
        // 1. 滾輪控制
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0)
        {
            currentDist -= scroll * zoomspeed;
            currentDist = Mathf.Clamp(currentDist, minDistance, maxDistance);
        }

        // 2. 切換判斷
        // 距離小於 1 且還在 2D -> 切回 3D
        if (currentDist < switch3D2D && !is3DMode)
        {
            SetMod3D();
        }
        // 距離大於 1 且還在 3D -> 切去 2D
        if (currentDist > switch3D2D && is3DMode)
        {
            SetMod2D();
        }

        // 3. Q/E 旋轉 (只有 2D 有效)
        if (!is3DMode)
        {
            if (Input.GetKeyDown(KeyCode.E)) targetYAngle += 90f;
            if (Input.GetKeyDown(KeyCode.Q)) targetYAngle -= 90f;
            // 在 2D 模式下，要把上下看的角度歸零，不然切過去會低著頭
            xRotation = 0f;
        }
    }
    void LateUpdate()
    {
        if (Time.timeScale == 0f) return;
        // 4. 執行 Pivot 旋轉
        float smoothAngle = Mathf.LerpAngle(cameraPivot.eulerAngles.y, targetYAngle, Time.deltaTime * 5f);
        cameraPivot.rotation = Quaternion.Euler(0, smoothAngle, 0);
        
        // 5. 執行鏡頭移動與特效
        CameraAvoidBlock();
    }

    void SetMod3D()
    {
        is3DMode = true;
        mainCam.orthographic = false; // 開啟透視
        mainCam.nearClipPlane = 0.1f; // 恢復正常視距
    }

    void SetMod2D()
    {
        is3DMode = false;
        mainCam.orthographic = true; // 開啟平面模式
        
        // 校正旋轉角度 (避免歪歪的進 2D)
        targetYAngle = Mathf.Round(cameraPivot.eulerAngles.y / 90f) * 90f;
    }
    
    void CameraAvoidBlock()
    {
        if (is3DMode)
        {
            // --- 3D 模式 ---
            // 位置：回到頭部 (高度 camHeight)
            Vector3 targetPos = new Vector3(0, camHeight, 0);
            
            mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, targetPos, Time.deltaTime * 10f);
        }
        else
        {
            // --- 2D 模式 ---
            // 位置：往左拉遠 (-currentDist)
            // 【重要修正】這裡要把 2f 改成 camHeight (0.6f)，不然相機會飛太高
            Vector3 targetPos = new Vector3(-currentDist, camHeight, 0);
            
            mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, targetPos, Time.deltaTime * 10f);
            
            // 旋轉：鎖定看向右邊 (90度)
            mainCam.transform.localRotation = Quaternion.Euler(0, 90, 0);
            
            // 縮放：防止 Size 變成 0
            float safeSize = Mathf.Max(currentDist * 0.6f, 1.0f);
            mainCam.orthographicSize = safeSize;

            // 隱形牆壁：【重要修正】防止 nearClipPlane 變成負數
            // Mathf.Max 會取兩者較大值，保證最小是 0.1
            float clipPlane = currentDist - 2.0f;
            mainCam.nearClipPlane = Mathf.Max(clipPlane, 0.1f);
        }
    }
}
