﻿using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    // ヒットポイント
    public int hp = 100;

    // スコアのポイント
    public int point = 100;

    // 現在のターゲットオブジェクトの番号
    public int intCurrentNum = 0;

    // ラジアン変数
    private float rad;

    // 速度
    public Vector2 speed = new Vector2(1f, 1f);

    public GameObject FirstTarget;    

    // ターゲットオブジェクト
    public GameObject[] TargetObjects;

    // 現在位置
    public Vector2 position;

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

        rad = Mathf.Atan2(TargetObjects[intCurrentNum].transform.position.y - transform.position.y,
                          TargetObjects[intCurrentNum].transform.position.x - transform.position.x);

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
        BossMove();
    }

    private void BossMove()
    {
        if (intCurrentNum == 0)
        {
            if (transform.position.x > TargetObjects[intCurrentNum].transform.position.x)
            {
                XYMove();
            }
            else if (transform.position.x < TargetObjects[intCurrentNum].transform.position.x)
            {
                MoveChange();
            }
        }
        else if (intCurrentNum == 1)
        {
            if (transform.position.y > TargetObjects[intCurrentNum].transform.position.y)
            {
                XYMove();
            }
            else if (transform.position.y < TargetObjects[intCurrentNum].transform.position.y)
            {
                MoveChange();
            }
        }
        else if (intCurrentNum == 2)
        {
            if (transform.position.y < TargetObjects[intCurrentNum].transform.position.y)
            {
                XYMove();
            }
            else if (transform.position.y > TargetObjects[intCurrentNum].transform.position.y)
            {
                MoveChange();
            }
        }

    }

    private void XYMove()
    {
        position = transform.position;

        position.x += speed.x * Mathf.Cos(rad);
        position.y += speed.y * Mathf.Sin(rad);

        transform.position = position;
    }

    private void MoveChange()
    {
        intCurrentNum++;

        if (intCurrentNum == 3)
        {
            intCurrentNum = 0;
        }

        rad = Mathf.Atan2(TargetObjects[intCurrentNum].transform.position.y - transform.position.y,
                          TargetObjects[intCurrentNum].transform.position.x - transform.position.x);
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