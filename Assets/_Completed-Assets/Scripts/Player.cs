using UnityEngine;
using System.Threading;
using System.Collections;

public class Player : MonoBehaviour
{
    // Spaceshipコンポーネント
    Spaceship spaceship;

    // Renderコンポーネント
    Renderer renderer;

    public int intPlayerNum = 3;
    
    bool blnStopMove = false;
    bool blnStopShot = false;

    IEnumerator Start()
    {
        // Spaceshipコンポーネントを取得
        spaceship = GetComponent<Spaceship>();

        renderer = GetComponent<Renderer>();

        while (true)
        {
            if (blnStopShot == false)
            {
                // 弾をプレイヤーと同じ位置/角度で作成
                spaceship.Shot(transform);

                // ショット音を鳴らす
                GetComponent<AudioSource>().Play();

                // ショットの発射間隔
                yield return new WaitForSeconds(spaceship.shotDelay);
            }
            else if (blnStopShot == true)
            {
                int i = 0;
                blnStopShot = false;

                while (i != 5000000)
                {
                    i++;
                }
            }
        }
    }

    void Update()
    {
        // 右・左
        float x = Input.GetAxisRaw("Horizontal");

        // 上・下
        float y = Input.GetAxisRaw("Vertical");

        // 移動する向きを求める
        Vector2 direction = new Vector2(x, y).normalized;

        // 移動の制限
        Move(direction);
    }

    // 機体の移動
    void Move(Vector2 direction)
    {
        if (blnStopMove == true)
        {
            blnStopMove = false;
            int i = 0;

            while (i != 5000000)
            {
                i++;
            }
        }

        // 画面左下のワールド座標をビューポートから取得
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        // 画面右上のワールド座標をビューポートから取得
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // プレイヤーの座標を取得
        Vector2 pos = transform.position;

        // 移動量を加える
        pos += direction * spaceship.speed * Time.deltaTime;

        // プレイヤーの位置が画面内に収まるように制限をかける
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        // 制限をかけた値をプレイヤーの位置とする
        transform.position = pos;
    }

    // ぶつかった瞬間に呼び出される
    void OnTriggerEnter2D(Collider2D c)
    {
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName(c.gameObject.layer);

        // レイヤー名がBullet (Enemy)の時は弾を削除
        if (layerName == "Bullet (Enemy)")
        {
            // 弾の削除
            Destroy(c.gameObject);
        }

        // レイヤー名がBullet (Enemy)またはEnemyの場合は爆発
        if (layerName == "Bullet (Enemy)" || layerName == "Enemy")
        {
            // 残機を減らす
            intPlayerNum--;

            // 爆発する
            spaceship.Explosion();

            if (intPlayerNum == 0)
            {
                // プレイヤーを削除
                Destroy(gameObject);

                // Managerコンポーネントをシーン内から探して取得し、GameOverメソッドを呼び出す
                FindObjectOfType<Manager>().GameOver();
            }
            else if(intPlayerNum != 0)
            {
                blnStopMove = true;
                blnStopShot = true;
                Damage();
            }
        }
    }

    IEnumerator Damage()
    {
        //レイヤーをPlayerDamageに変更
        gameObject.layer = LayerMask.NameToLayer("PlayerRest");
        //while文を10回ループ
        int count = 10;
        while (count > 0)
        {
            //透明にする
            renderer.material.color = new Color(1, 1, 1, 0);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            //元に戻す
            renderer.material.color = new Color(1, 1, 1, 1);
            //0.05秒待つ
            yield return new WaitForSeconds(0.05f);
            count--;
        }
        //レイヤーをPlayerに戻す
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}