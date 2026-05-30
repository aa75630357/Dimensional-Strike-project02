using UnityEngine;

public class plantSpawner : MonoBehaviour
{
    [Header("生成範圍設定")]
    public float mapSize = 100f;                // 地圖大小，X和Z軸各延伸這個距離的一半
    public float centerExcludeRadius = 10f;     // 中心排除半徑，玩家附近不生成

    [Header("草地設定（無碰撞體）")]
    public GameObject grassPrefab;              // 草地 Prefab
    public int grassCount = 50;                 // 草地生成數量
    public float grassSpawnY = 0f;

    [Header("樹木設定（有碰撞體）")]
    public GameObject treePrefab;               // 樹木 Prefab
    public int treeCount = 30;                  // 樹木生成數量'
    public float treeSpawnY = 0.3f;


    [Header("石頭設定（有碰撞體）")]
    public GameObject rockPrefab;               // 石頭 Prefab
    public int rockCount = 20;                  // 石頭生成數量
    public float rockSpawnY = 0f;

    [Header("防重疊設定")]
    public float overlapCheckRadius = 1f;       // Physics.CheckSphere 的檢測半徑
    public LayerMask overlapCheckLayer;         // 只檢測哪些 Layer 的碰撞體
    public int maxAttempts = 30;                // 每個物件最多嘗試幾次找座標

    void Start()
    {
        SpawnObjects(grassPrefab, grassCount,grassSpawnY, false); 
        SpawnObjects(treePrefab, treeCount,treeSpawnY, true);    
        SpawnObjects(rockPrefab, rockCount,rockSpawnY, true);
    }

    void SpawnObjects(GameObject prefab, int count,float SpawnY, bool checkOverlap)
    {
        if (prefab == null) return;

        // 【外層迴圈】你要生幾個物件，這個迴圈就跑幾次
        for (int i = 0; i < count; i++)
        {
            //生成點位
            Vector3 validPosition = Vector3.zero;
            //是否生成
            bool foundValidPos = false;

            // 【內層迴圈】每個物件，給它 maxAttempts 次機會找合適的位子
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // 步驟 1：在地圖範圍內，隨機射飛鏢找一個點
                float randomX = Random.Range(-mapSize / 2f, mapSize / 2f);
                float randomZ = Random.Range(-mapSize / 2f, mapSize / 2f);
                
                // 算出這個點在世界中的實際座標 (我們假設地面 Y 軸是 0)
                Vector3 tryPos = transform.position + new Vector3(randomX, SpawnY, randomZ);

                // 步驟 2：第一關審核 (遠離玩家)
                // 如果這個點離中心點太近，就 continue (放棄這次，重新找點)
                if (Vector3.Distance(transform.position, tryPos) <= centerExcludeRadius)
                {
                    continue; 
                }

                // 步驟 3：第二關審核 (防重疊)
                // 只有傳入 checkOverlap 為 true 的時候 (例如樹和石頭)，才做 CheckSphere 掃描
                if (checkOverlap)
                {
                    // 如果掃描半徑內，有撞到 overlapCheckLayer 裡的東西，就 continue (重新找點)
                    if (Physics.CheckSphere(tryPos, overlapCheckRadius, overlapCheckLayer))
                    {
                        continue; 
                    }
                }

                // 步驟 4：過關！這是一個好位子！
                validPosition = tryPos;
                foundValidPos = true;
                break; // 打破嘗試迴圈，不用再找了，準備生出來！
            }

            // 步驟 5：正式 Instantiate 把物件生出來
            if (foundValidPos)
            {
                // 給它一個隨機的旋轉角度，這樣每棵樹看起來面向不同，比較自然
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                
                // 生成物件，並把它設定為這台機器的子物件 (這樣 Hierarchy 比較乾淨)
                Instantiate(prefab, validPosition, randomRotation, transform);
            }
        }
    }
}
