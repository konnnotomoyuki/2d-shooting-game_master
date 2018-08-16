using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 大型の敵のスクリプト
public class EnemyType3 : MonoBehaviour
{
    // ヒットポイント
    public int hp = 100;

    // スコアのポイント
    public int point = 1000;

    private bool blnEnemyDestroy = false;

    // Spaceshipコンポーネント
    Spaceship spaceship;

    SpriteRenderer sprite;

    // Use this for initialization
    IEnumerator Start ()
    {
        // Spaceshipコンポーネントを取得
        spaceship = GetComponent<Spaceship>();

        sprite = GetComponent<SpriteRenderer>();

        // ローカル座標のY軸のマイナス方向に移動する
        Move(transform.up * -1);

        // canShotがfalseの場合、ここでコルーチンを終了させる
        if (spaceship.canShot == false)
        {
            yield break;
        }

        while (true)
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
    }

    private void Update()
    {
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

        if (hp > 0)
        {
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

            spaceship.GetAnimator().SetTrigger("Damage");
        }
        // ヒットポイントが0以下であれば
        else if (hp <= 0)
        {
            if (blnEnemyDestroy == false)
            {
                // スコアコンポーネントを取得してポイントを追加
                FindObjectOfType<Score>().AddPoint(point);

                blnEnemyDestroy = true;

                // 爆発
                spaceship.Explosion();

                Invoke("EnemyDestroy", 2);
            }
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