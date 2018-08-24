using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    // ヒットポイント
    public int hp;

    // スコアのポイント
    public int point;

    // 敵の種類
    public int intEnemyType;

    // 速度
    public Vector2 speed;

    // ターゲットとなるオブジェクト
    Vector2 PlayerPosition;

    // 現在位置
    Vector2 Position;

    // 大型エネミーの点滅表示の際使用
    SpriteRenderer sprite;

    // ラジアン変数
    private float rad;

    private bool blnEnemyDestroy = false;

    // Spaceshipコンポーネント
    Spaceship spaceship;

    IEnumerator Start()
    {
        // Spaceshipコンポーネントを取得
        spaceship = GetComponent<Spaceship>();

        // ローカル座標のY軸のマイナス方向に移動する
        if (intEnemyType == 1 || intEnemyType == 3)
        {
            Move(transform.up * -1);
        }

        // 大型エネミーの場合のみSpriteRendererコンポーネントを取得
        if (intEnemyType == 3)
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        // canShotがfalseの場合、ここでコルーチンを終了させる
        if (spaceship.canShot == false)
        {
            yield break;
        }

        while (true)
        {
            if (intEnemyType == 1 || intEnemyType == 3)
            {
                // 子要素を全て取得する
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform shotPosition = transform.GetChild(i);

                    // ShotPositionの位置/角度で弾を撃つ
                    spaceship.Shot(shotPosition);
                }

                // shotDelay秒待つ
                yield return new WaitForSeconds(spaceship.shotDelay);
            }
            else if (intEnemyType == 2)
            {
                Transform shotPosition = transform.GetChild(1);

                // ShotPositionの位置/角度で弾を撃つ
                spaceship.Shot(shotPosition);

                // shotDelay秒待つ
                yield return new WaitForSeconds(spaceship.shotDelay);
            }
        }
    }

    private void Update()
    {
        // 自機を追尾するType2エネミーの場合のみ起動
        if (intEnemyType == 2)
        {
            // Playerのゲームオブジェクトがあるか否かで行動を変える
            if (FindObjectOfType<Player>() != null) {
                PlayerPosition = FindObjectOfType<Player>().transform.position;
                Position = transform.position;

                rad = Mathf.Atan2(
                    PlayerPosition.y - transform.position.y,
                    PlayerPosition.x - transform.position.x);

                Position.x += speed.x * Mathf.Cos(rad);
                Position.y += speed.y * Mathf.Sin(rad);
                transform.position = Position;
            }
        }

        // 大型であるType3エネミーの場合のみ起動
        if (blnEnemyDestroy == true)
        {
            StartCoroutine(Blink());
        }
    }

    // 機体の移動
    public void Move(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * spaceship.speed;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName(c.gameObject.layer);

        // レイヤー名がBullet (Player)以外の時は何も行わない
        if (layerName != "Bullet (Player)") return;

        // PlayerBulletのTransformを取得
        Transform playerBulletTransform = c.transform.parent;

        // Bulletコンポーネントを取得
        Bullet bullet = playerBulletTransform.GetComponent<Bullet>();

        // ヒットポイントを減らす
        hp = hp - bullet.power;

        // 弾の削除
        Destroy(c.gameObject);

        // ヒットポイントが0以下であれば
        if (hp <= 0)
        {
            // スコアコンポーネントを取得してポイントを追加
            FindObjectOfType<Score>().AddPoint(point);

            if (intEnemyType == 1 || intEnemyType == 2)
            {
                // エネミーの削除
                Destroy(gameObject);

                // サイズの大きいエネミーの場合、レイヤーを変更する
                if (intEnemyType == 2)
                {
                    gameObject.layer = 0;
                }

                // 爆発
                spaceship.Explosion();
            }
            else if (intEnemyType == 3)
            {
                blnEnemyDestroy = true;

                // 爆発
                spaceship.Explosion();

                Invoke("EnemyDestroy", 2);
            }
        }
        else
        {
            spaceship.GetAnimator().SetTrigger("Damage");
        }
    }

    // 点滅コルーチン
    IEnumerator Blink()
    {
        while (true)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(1.0f);
        }
    }

    void EnemyDestroy()
    {
        // エネミーの削除
        Destroy(gameObject);
    }
}