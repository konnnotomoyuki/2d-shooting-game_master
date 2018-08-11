using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDummy : MonoBehaviour
{
    // ヒットポイント
    public int hp = 10000;

    // ラジアン変数
    private float rad;

    // 速度
    public Vector2 speed = new Vector2(0.02f, 0.02f);

    public GameObject FirstTarget;     

    public GameObject Boss;

    // 現在位置
    public Vector2 position;

    private GameObject Canvas;

    // Spaceshipコンポーネント
    Spaceship spaceship;

    void Start()
    {
        // Spaceshipコンポーネントを取得
        spaceship = GetComponent<Spaceship>();

        position = transform.position;

        rad = Mathf.Atan2(FirstTarget.transform.position.y - transform.position.y,
                          FirstTarget.transform.position.x - transform.position.x);

        Canvas = GameObject.Find("Canvas");

        Canvas.transform.Find("Effect").gameObject.SetActive(true);

        FindObjectOfType<Manager>().BossAppear();
    }

    private void Update()
    {
        if (transform.position.y > FirstTarget.transform.position.y)
        {
            XYMove();
        }
        else if (transform.position.y < FirstTarget.transform.position.y)
        {
            Instantiate(Boss);
            Canvas.transform.Find("Effect").gameObject.SetActive(false);
            FindObjectOfType<Manager>().AppearStop();
            Destroy(this.gameObject);
        }
    }

    private void XYMove()
    {
        position = transform.position;

        position.x += speed.x * Mathf.Cos(rad);
        position.y += speed.y * Mathf.Sin(rad);

        transform.position = position;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName(c.gameObject.layer);

        // レイヤー名がBullet (Player)以外の時は何も行わない
        if (layerName != "Bullet (Player)") return;
    }
}