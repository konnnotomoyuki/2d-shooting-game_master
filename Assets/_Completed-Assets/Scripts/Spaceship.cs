using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Spaceship : MonoBehaviour
{
    // 移動スピード
    public float speed;

    // 弾を撃つ間隔
    public float shotDelay;

    public int size;

    // 弾のPrefab
    public GameObject bullet;

    // 弾を撃つかどうか
    public bool canShot;

    // 爆発のPrefab
    public GameObject explosion;

    // 音を出さない爆発のPrefab
    public GameObject s_explosion;

    // アニメーターコンポーネント
    private Animator animator;

    void Start()
    {
        // アニメーターコンポーネントを取得
        animator = GetComponent<Animator>();
    }

    // 敵サイズの違いによる爆発の作成
    public void Explosion()
    {
        if (size < 2)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        else if(size >= 2)
        {
            if (size == 2)
            {
                InvokeRepeating("Explode", 0.1f, 1.0f);
            }
            else if(size == 3)
            {
                InvokeRepeating("Explode", 0.1f, 0.1f);
                InvokeRepeating("Explode", 0.1f, 1.0f);
                InvokeRepeating("Explode", 0.1f, 1.0f);
                InvokeRepeating("Explode", 0.1f, 1.0f);
            }
        }
    }

    public void Explode()
    {
        float x = Random.Range(-0.7f, 0.7f); float y = Random.Range(-0.7f, 0.7f);
        Instantiate(explosion, transform.position + new Vector3(x, y, 0.0f), transform.rotation);
        x = Random.Range(-0.7f, 0.7f); y = Random.Range(-0.7f, 0.7f);
        Instantiate(s_explosion, transform.position + new Vector3(x, y, 0.0f), transform.rotation);
    }

    // 弾の作成
    public void Shot(Transform origin)
    {
        Instantiate(bullet, origin.position, origin.rotation);
    }

    // アニメーターコンポーネントの取得
    public Animator GetAnimator()
    {
        return animator;
    }
}