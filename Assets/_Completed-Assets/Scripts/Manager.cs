using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Collections;

public class Manager : MonoBehaviour
{
    // Playerプレハブ
    public GameObject player;

    // キャンバス
    private GameObject Canvas;

    // タイトル
    private GameObject ShootingGame;
    private GameObject PressX;
    
    // ゲームクリア時演出
    private GameObject ClearGame;

    // ボス演出
    private GameObject BossComing;

    void Start()
    {
        // Canvas内のゲームオブジェクトを検索し取得する
        ShootingGame = GameObject.Find("Shooting Game");
        PressX = GameObject.Find("Press X");
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
        // または、ボスが出現していない場合、或いはボスが出現しており倒した状態でない場合のみタイトルを表示する
        if (FindObjectOfType<Boss>() == null
            || (FindObjectOfType<Boss>() != null && FindObjectOfType<Boss>().blnBossDestroy != true))
        {
            ShootingGame.SetActive(true);
            PressX.SetActive(true);
            return;
        }

        // ゲームオーバーの表示が出た後にボスを倒した場合、ゲームオーバーの表示を消してクリアの表示を出す
        if ((ShootingGame.activeSelf == true && PressX.activeSelf == true) &&
            FindObjectOfType<Boss>().blnBossDestroy == true)
        {
            ShootingGame.SetActive(false);
            PressX.SetActive(false);
        }
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

