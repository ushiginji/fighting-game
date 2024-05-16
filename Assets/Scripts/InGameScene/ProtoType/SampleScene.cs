using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// https://zenn.dev/o8que/books/bdcb9af27bdd7d/viewer/c04ad5
// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class SampleScene : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        Debug.Log("Join Room");
        var position = new Vector3(Random.Range(-3f, 3f), 3.0f, Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate("ProtoType/Avatar", position, Quaternion.identity);
    }
}
