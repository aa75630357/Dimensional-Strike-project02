using UnityEngine;

public class EnemyGlow : MonoBehaviour
{
    [Header("判斷是否亮(打勾3D亮)(不打勾2D亮)")]
    public bool lightIn3D;

    [Header("怪物外框物件 (請把4個邊框都拉進來)")]
    public Renderer[] outlineRenderers; // 這裡改成陣列了！可以放無限多個

    [Header("發光設定")]
    public Color glowColor = Color.red;
    public float glowIntensity = 2f; // 可以手動調亮度的倍數
    
    private Material[] mats; // 用來存那4個邊框的材質球
    private ViewController viewCtrl; // 用來存你的視角控制器

    void Start()
    {
        // 1. 初始化材質球陣列，並確保它們都開啟發光功能
        mats = new Material[outlineRenderers.Length];
        for (int i = 0; i < outlineRenderers.Length; i++)
        {
            if (outlineRenderers[i] != null)
            {
                mats[i] = outlineRenderers[i].material;
                mats[i].EnableKeyword("_EMISSION");
            }
        }
        
        // 2. 自動在場景中找到你的 ViewController
        viewCtrl = FindObjectOfType<ViewController>();
    }

    void Update()
    {
        if (viewCtrl == null) return;

        // 3. 抓取現在的視角維度
        bool currentIs3D = viewCtrl.is3DMode;

        // 4. 判斷是否在對的維度
        bool shouldGlow = (currentIs3D && lightIn3D) || (!currentIs3D && !lightIn3D);

        // 5. 決定最終顏色
        Color finalColor = shouldGlow ? glowColor * glowIntensity : Color.black;

        // 6. 用迴圈一次把4個邊框的顏色全部改掉！
        for (int i = 0; i < mats.Length; i++)
        {
            if (mats[i] != null)
            {
                mats[i].SetColor("_EmissionColor", finalColor);
            }
        }
    }
}