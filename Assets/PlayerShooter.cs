using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public ViewController viewCtrl; 
    public Camera mainCam;
    
    [Header("槍設定")]
    public Transform gunPivot;  // 槍托(靠近身體的)
    public Transform gunPoint;  // 槍口
    public GameObject bullet;   // 子彈

    [Header("彈藥設定")]
    public float maxAmmo = 15f;     //最大容量
    public float currentAmmo = 15f; //當前子彈
    public float ammoRegenRate = 0.6f;//回子彈
    [Header("教學設定(開關子彈)")]
    public static bool CanShoot = true;

    void Start()
    {
        CanShoot = true;
    }
    void Update()
    {
        if (!CanShoot) return;
        if (Time.timeScale == 0f) return;
        if (currentAmmo < maxAmmo)
        {
            currentAmmo += ammoRegenRate * Time.deltaTime;
            if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
            // 隔空呼叫 UIManager 更新 UI！
            if (UIManager.instance != null) UIManager.instance.UpdateAmmo(currentAmmo / maxAmmo);
        }
        if (Input.GetMouseButtonDown(0) && currentAmmo >= 1f)
        {
            currentAmmo -= 2;
            if (UIManager.instance != null) UIManager.instance.UpdateAmmo(currentAmmo / maxAmmo);
            Shooter();
        }
    }
    void LateUpdate()
    {
        if (Time.timeScale == 0f) return;
        
        HeadleAiming();
    }

    void HeadleAiming()         
    {
        if (viewCtrl.is3DMode)
        {
            // --- 3D 模式 ---
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            // 【超級乾淨瞄準法】：抓取攝影機正前方 50 公尺處的一個「虛擬中心點」
            Vector3 centerTarget = mainCam.transform.position + mainCam.transform.forward * 50f;
            
            // 讓槍托直接轉過去「盯著」那個虛擬中心點
            gunPivot.LookAt(centerTarget);
        }
        else
        {
            // --- 2D 模式 ---
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 【終極精準瞄準法】建立一個跟攝影機平行的虛擬平面
            Plane plane = new Plane(-mainCam.transform.forward, gunPivot.position);
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            // 讓滑鼠射線打在這個平面上，算出世界座標 (解決 Q/E 轉向後滑鼠亂飄的問題)
            if (plane.Raycast(ray, out float distanceToPlane))
            {
                Vector3 mouseWorldPos = ray.GetPoint(distanceToPlane);
                
                // 防呆機制：滑鼠離身體超過 1 單位才去瞄準
                float distance = Vector3.Distance(mouseWorldPos, gunPivot.position);
                if (distance > 1.0f)
                {
                    gunPivot.LookAt(mouseWorldPos);
                }
                else
                {
                    // 離太近就看著前方
                    gunPivot.localRotation = Quaternion.identity;
                }
            }
        }
    }

    void Shooter()
    {   
        if (bullet != null && gunPoint != null)
        {
            GameObject newBullet = Instantiate(bullet, gunPoint.position, gunPoint.rotation);
            Bullet bulletScript = newBullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.isFiredIn3D = viewCtrl.is3DMode;
            }
        }
    }
}