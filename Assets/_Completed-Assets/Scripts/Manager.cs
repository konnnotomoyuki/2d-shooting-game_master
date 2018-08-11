using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Collections;

public class Manager : MonoBehaviour
{
    // Playerプレハブ
    public GameObject player;

    // タイトル
    private GameObject ShootingGame;
    private GameObject PressX;
    private GameObject Effect;
    private GameObject Canvas;
    private GameObject ClearGame;
    private GameObject BossComing;

    void Start()
    {
        // Canvas内のゲームオブジェクトを検索し取得する
        ShootingGame = GameObject.Find("Shooting Game");
        PressX = GameObject.Find("Press X");
        Effect = GameObject.Find("Effect");
        Canvas = GameObject.Find("Canvas");

        ClearGame = Canvas.transform.Find("ClearGame").gameObject;
        BossComing = Canvas.transform.Find("Warning").gameObject;

    }

    void Update()
    {
        // ゲーム中ではなく、Xキーが押されたらtrueを返す。
        if (IsPlaying() == false && Input.GetKeyDown(KeyCode.X))
        {
            GameStart();
        } 
    }

    void GameStart()
    {
        // ゲームスタート時に、タイトルを非表示にしてプレイヤーを作成する
        ShootingGame.SetActive(false);
        PressX.SetActive(false);
        Instantiate(player, player.transform.position, player.transform.rotation);
    }

    public void GameOver()
    {
        // ハイスコアの保存
        FindObjectOfType<Score>().Save();

        // ゲームオーバー時に、タイトルを表示する
        ShootingGame.SetActive(true);
        PressX.SetActive(true);
    }

    public void BossAppear()
    {
        BossComing.SetActive(true);
    }

    public void AppearStop()
    {
        BossComing.SetActive(false);
    }

    public void BossDefeat()
    {
        ClearGame.SetActive(true);
    }

    public bool IsPlaying()
    {
        // ゲーム中かどうかはタイトルの表示/非表示で判断する
        return ShootingGame.activeSelf == false;
    }
}

