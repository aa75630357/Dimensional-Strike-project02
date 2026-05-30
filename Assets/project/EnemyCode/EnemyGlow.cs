using System;
using UnityEngine;

public class EnemyGlow : MonoBehaviour
{
    [Header("判斷是否亮(打勾3D亮)(不打勾2D亮)")]
    public bool light3D = true;
    private ViewController viewCtrl; //是3D還是2D
    private Outline outline;        //邊框
    void Start()
    {
        outline = GetComponent<Outline>();
        viewCtrl = FindFirstObjectByType<ViewController>();
        if (viewCtrl == null)
        {
            Debug.LogError(gameObject.name + " 我找不到 ViewControlle");
        }
        if (outline == null)
        {
            Debug.LogError(gameObject.name + " 我身上沒有掛Outline");
        }
    }
    void Update()
    {
        if(outline == null || viewCtrl == null) return;
        //2D時候會是falue，3D的時候會是true
        //is3DMode也是同個道理
        outline.enabled = (light3D == viewCtrl.is3DMode);

    }
}