using UnityEngine;

public class EnemySpawner:MonoBehaviour
{
    [Header("目標玩家位址")]
    public Transform player;
    [Header("怪物種類藍圖(陣列)")]
    public GameObject[] EnemyPrefab;
    [Header("生成設定")]
    public float SpawnSpeed = 5f;   //幾秒出生
    public float timer = 0f;        //倒數
    [Header("生成範圍")]
    public float minRadius = 13f;   //最近生成怪物距離
    public float maxRadius = 15f;   //最遠生成怪物距離
    public float minY = 0f;         //最低
    public float maxY = 4f;         //最高

    void Update()                   //生成怪物
    {
        timer += Time.deltaTime;
        if (timer >= SpawnSpeed && player != null)//生成怪物計時還有防呆
        {
            SpawnEnemy();
            timer = 0;
        }
    }
    void SpawnEnemy()               //如何生成怪物
    {
        //生成哪類怪物
        int randomIndex = Random.Range(0,EnemyPrefab.Length);
        GameObject ThisEnemy = EnemyPrefab[randomIndex];
        //生成座標為以玩家為中心的一圈(甜甜圈)
        //弧度
        float angle = Random.Range(0f, Mathf.PI * 2);
        //距離
        float radius = Random.Range(minRadius, maxRadius);    
        //XZ座標也就是定位
        float spawnX = player.position.x + Mathf.Cos(angle) * radius;
        float spawnZ = player.position.z + Mathf.Sin(angle) * radius;
        //高度
        float spawnY = player.position.y + Random.Range(minY, maxY);
        // 組合出最終的生成點
        Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);
        // 3. 把怪物生出來！
        Instantiate(ThisEnemy, spawnPos, Quaternion.identity);
    }
}
