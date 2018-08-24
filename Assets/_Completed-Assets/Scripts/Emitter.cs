using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour
{
    // 現在のWave
    public int currentWave;

    // Waveプレハブを格納する
    public GameObject[] waves;

    // BossDummyオブジェクトを格納する
    public GameObject BossDummy;    

    // Managerコンポーネント
    private Manager manager;

    IEnumerator Start()
    {

        // Waveが存在しなければコルーチンを終了する
        if (waves.Length == 0)
        {
            yield break;
        }

        // Managerコンポーネントをシーン内から探して取得する
        manager = FindObjectOfType<Manager>();

        while (true)
        {
            // タイトル表示中は待機
            while (manager.IsPlaying() == false)
            {
                yield return new WaitForEndOfFrame();
            }

            GameObject g;

            // Waveを作成する
            g = (GameObject)Instantiate(waves[currentWave], transform.position, Quaternion.identity);

            // WaveをEmitterの子要素にする
            g.transform.parent = transform;

            // Waveの子要素のEnemyが全て削除されるまで待機する
            while (g.transform.childCount != 0)
            {
                yield return new WaitForEndOfFrame();
            }

            // Waveの削除
            Destroy(g);

            // 格納されているWaveを全て実行し、シーン上にプレイヤーが存在した場合BossDummyゲームオブジェクトを生成する
            if (++currentWave >= waves.Length)
            {
                while (FindObjectOfType<Player>() == null)
                {
                    yield return new WaitForEndOfFrame();
                }

                Instantiate(BossDummy);
                break;
            }
        }
    }
}