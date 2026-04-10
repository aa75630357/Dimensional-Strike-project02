using System;
using NUnit.Framework;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;       //子彈速度
    public float lifeTime = 3f;     //子彈存在時間
    public string targetTag = "";   //2D3D子彈類型

    public bool isFiredIn3D;        //是否在3D還是2D來打到對應的怪物
    private Camera mainCam;         // 用來抓取攝影機方向，做2D深度掃描
    void Start()        //清除子彈
    {
        mainCam = Camera.main;
        Destroy(gameObject, lifeTime); //時間到自動銷回
    }
    void Update()       //持續往前飛
    {   //子彈往前方移動
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (!isFiredIn3D)
        {
            Check2DHit();
        }
    }
    void OnTriggerEnter(Collider other)//判斷子彈打到的是2D怪物還是3D

    {
        if (isFiredIn3D && other.CompareTag("Enemy3D"))
        {
            Destroy(other.gameObject);  //確認是否為3D打到是否為3D怪物
            Destroy(gameObject);
            if (UIManager.instance != null) UIManager.instance.AddScore(1);
        }
        else if (!other.isTrigger && !other.CompareTag("Player"))
        {
            Destroy(gameObject);        //確認打到東西是否為其他或玩家
        }
    }
    void Check2DHit()       //困難點2D如何打到怪物
    {
        if (mainCam == null) return;
        //這是讓子彈不考慮深度，也就是3D中的不考慮左右
        Vector3 boxHalfSize = new Vector3(0.5f, 0.5f, 25f);
        //讓這個箱子的「方向」永遠朝著螢幕深處
        Quaternion boxRotation = mainCam.transform.rotation;
        // 掃描！抓出子彈畫面上碰到的一切東西
        Collider[] hitColliders = Physics.OverlapBox(transform.position, boxHalfSize, boxRotation);

        foreach (Collider hit in hitColliders)
        {
            // 如果掃到了 2D 怪物，而且牠沒有隱形 (Collider 有開啟)
            if (hit.CompareTag("Enemy2D"))
            {
                //確認怪物是否有隱形來確認不會打到範圍外的怪物
                if (hit.GetComponent<MeshRenderer>().enabled == true)
                {
                    Destroy(hit.gameObject); // 殺死怪物
                    Destroy(gameObject);     // 子彈消失   
                    if (UIManager.instance != null) UIManager.instance.AddScore(1);          
                    return; // 打到一個就收工，避免一發穿透兩隻
                }
            }
        }
    }
}
