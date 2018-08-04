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

    void Start()
    {
        // Canvas内のゲームオブジェクトを検索し取得する
        ShootingGame = GameObject.Find("Shooting Game");
        PressX = GameObject.Find("Press X");
        Effect = GameObject.Find("Effect");
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

    public bool IsPlaying()
    {
        // ゲーム中かどうかはタイトルの表示/非表示で判断する
        return ShootingGame.activeSelf == false;
    }
}

//using UnityEngine;

//public class Manager : MonoBehaviour
//{
//    // Playerプレハブ
//    public GameObject player;

//    // タイトル
//    private GameObject Canvas;

//    void Start()
//    {
//        // Titleゲームオブジェクトを検索し取得する
//        Canvas = GameObject.Find("Canvas");
//    }

//    void Update()
//    {
//        // ゲーム中ではなく、Xキーが押されたらtrueを返す。
//        if (IsPlaying() == false && Input.GetKeyDown(KeyCode.X))
//        {
//            GameStart();
//        }
//    }

//    void GameStart()
//    {
//        // ゲームスタート時に、タイトルを非表示にしてプレイヤーを作成する
//        Canvas.SetActive(false);
//        Instantiate(player, player.transform.position, player.transform.rotation);
//    }

//    public void GameOver()
//    {
//        // ハイスコアの保存
//        FindObjectOfType<Score>().Save();

//        // ゲームオーバー時に、タイトルを表示する
//        Canvas.SetActive(true);
//    }

//    public bool IsPlaying()
//    {
//        // ゲーム中かどうかはタイトルの表示/非表示で判断する
//        return Canvas.activeSelf == false;
//    }
//}