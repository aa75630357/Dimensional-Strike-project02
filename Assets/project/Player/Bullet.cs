using System;
using NUnit.Framework;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 200f;       //子彈速度
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
        transform.Translate(Vector3.forward * speed * Time.deltaTime);}}
        