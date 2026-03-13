using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("數值設定")]
    public float speed = 3f; // 怪物移動速度

    private Transform player; 
    private ViewController viewCtrl; // 用來知道現在是 2D 還是 3D
    private Camera mainCam;

    // 控制顯示與碰撞的組件
    private MeshRenderer meshRenderer;
    private Collider col;

    void Start()
    {
        // 抓取場景上的玩家與相機資料
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            viewCtrl = playerObj.GetComponent<ViewController>();
        }
        mainCam = Camera.main;

        // 抓取怪物的身體外觀與碰撞體
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        if (player == null || viewCtrl == null) return;
        // --- 1. 移動邏輯 (純粹往玩家靠近，不管身體面向哪裡) ---
        Vector3 dirToPlayer = player.position - transform.position;
        dirToPlayer.y = 0; // 鎖定 Y 軸，避免怪物飛起來

        if (dirToPlayer.magnitude > 0.1f)
        {
            // 往玩家的方向推動座標
            transform.position += dirToPlayer.normalized * speed * Time.deltaTime;
        }

        // --- 2. 顯示與旋轉邏輯 (視角魔法在這裡！) ---
        if (viewCtrl.is3DMode)
        {
            // 【3D 模式】：全部顯示，且怪物直接面向玩家
            meshRenderer.enabled = true;
            col.enabled = true;

            if (dirToPlayer != Vector3.zero)
            {
                transform.forward = dirToPlayer.normalized; // 看著玩家
            }
        }
        else
        {
            // 【2D 模式】：計算+-45度角，並面向鏡頭

            // 取得相機畫面的「左右橫軸」
            Vector3 viewAxis = mainCam.transform.right;
            viewAxis.y = 0;

            // 數學魔法：計算怪物方向與畫面橫軸的夾角
            // Mathf.Abs 取絕對值，代表不管在玩家左邊還是右邊 (東或西) 都能算
            float dot = Mathf.Abs(Vector3.Dot(dirToPlayer.normalized, viewAxis.normalized));

            // cos(45度) 約等於 0.707。如果數值大於 0.707，代表怪物在 +-45度 的走道內！
            bool isVisible = dot >= 0.5f;

            // 根據結果決定要不要讓玩家看到、打到牠
            meshRenderer.enabled = isVisible;

            // 讓怪物面向鏡頭 (跟相機看著相反的方向)，這樣怪物看起來就是完美的平面！
            transform.forward = mainCam.transform.right;
        }
    }
      void OnCollisionEnter(Collision collision)
    {
        // 注意這裡要用 collision.gameObject.CompareTag
       if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); // 怪物碰到玩家，怪物自殺
            if (UIManager.instance != null) UIManager.instance.TakeDamage(10f);
        }
    }
}