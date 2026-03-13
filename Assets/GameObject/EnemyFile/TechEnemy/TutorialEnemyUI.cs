using UnityEngine;

public class TutorialEnemyUI : MonoBehaviour
{
    [Header("這塊看板是在 3D 模式顯示嗎？(打勾=3D顯示, 不勾=2D顯示)")]
    public bool showIn3D;

    private ViewController viewCtrl;
    private Canvas myCanvas;

    void Start()
    {
        // 自動抓取大管家和自己身上的畫布
        viewCtrl = FindObjectOfType<ViewController>();
        myCanvas = GetComponent<Canvas>(); 
    }

    // 💡 老師小提醒：UI 轉向通常寫在 LateUpdate，確保攝影機移動完後才轉，畫面才不會抖動
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
        
        // 2. 判斷維度，決定要不要讓玩家看到字
        if (viewCtrl != null && myCanvas != null)
        {
            bool currentIs3D = viewCtrl.is3DMode;
            
            // 如果「現在是3D且設定3D顯示」或「現在是2D且設定2D顯示」，就開啟 Canvas
            myCanvas.enabled = (currentIs3D && showIn3D) || (!currentIs3D && !showIn3D);
        }
    }
}