using UnityEngine;
using System.Collections;

// プレイヤーを追いかける敵のスクリプト
public class EnemyType2 : MonoBehaviour
{
    // ヒットポイント
    public int hp = 1;

    // スコアのポイント
    public int point = 100;

    // 速度
    public Vector2 speed = new Vector2(0.01f, 0.01f);

    // ターゲットとなるオブジェクト
    Vector2 PlayerPosition;

    // 現在位置
    Vector2 Position;
    
    // ラジアン変数
    private float rad;

    // Spaceshipコンポーネント
    Spaceship spaceship;

    IEnumerator Start()
    {
        // Spaceshipコンポーネントを取得
        spaceship = GetComponent<Spaceship>();
        
        // canShotがfalseの場合、ここでコルーチンを終了させる
        if (spaceship.canShot == false)
        {
            yield break;
        }

        while (true)
        {            
            Transform shotPosition = transform.GetChild(1);

            // ShotPositionの位置/角度で弾を撃つ
            spaceship.Shot(shotPosition);

            // shotDelay秒待つ
            yield return new WaitForSeconds(spaceship.shotDelay);
        }
    }

    private void Update()
    {
        PlayerPosition = FindObjectOfType<Player>().transform.position;
        Position = transform.position;

        rad = Mathf.Atan2(
            PlayerPosition.y - transform.position.y,
            PlayerPosition.x - transform.position.x);

        Position.x += speed.x * Mathf.Cos(rad);
        Position.y += speed.y * Mathf.Sin(rad);
        transform.position = Position;

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

            // 爆発
            spaceship.Explosion();

            // エネミーの削除
            Destroy(gameObject);
        }
        else
        {
            spaceship.GetAnimator().SetTrigger("Damage");
        }
    }
}