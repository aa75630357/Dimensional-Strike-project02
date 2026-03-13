Dimensional Strike - Project 02 (次元打擊)
這是一款 2D/3D 混合視角的射擊遊戲專案，專注於實現流暢的射擊手感、動態的敵人 AI 系統以及精確的物理碰撞。

📂 專案核心目錄導覽
為了方便開發與後續維護，本專案採用模組化結構管理：

Assets/Scripts/ ⬅️ 【核心邏輯】面試官請點此檢視代碼

PlayerControl/: 處理玩家移動、瞄準及射擊觸發邏輯。

WeaponSystem/: 彈道計算、彈藥管理及槍械參數設定。

EnemyAI/: 敵人的追蹤演算法與行為決策系統。

UI/: 使用 TextMesh Pro 實作的 HUD 介面（血量、彈藥顯示）。

Assets/Scenes/: 包含主要的戰鬥關卡與測試環境。

Assets/Prefabs/: 所有的遊戲物件預製體（包含子彈特效、敵人原型）。

Assets/TextMesh Pro/: 專案使用的排版資源，確保 UI 在不同解析度下的清晰度。

🛠 關鍵技術實現
在這個射擊專案中，我重點開發了以下技術模組：

彈道與射擊系統 (Bullet & Shooting)

採用 Raycast (射線偵測) 或 Prefab-based Projectiles 實作，確保射擊判定的精準度。

實作了子彈的發射冷卻時間（Cooldown）與彈匣限制邏輯。

相機追蹤與震動 (Camera Shake)

透過代碼控制相機在開火時產生的輕微震動，增強射擊的「打擊感」。

UI 動態回饋

整合 TextMesh Pro，即時顯示玩家的彈藥殘量與分數變動。
