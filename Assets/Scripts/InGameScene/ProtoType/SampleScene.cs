using Photon.Pun;
using Photon.Realtime;
using System.Runtime.Serialization;
using UnityEngine;

// https://zenn.dev/o8que/books/bdcb9af27bdd7d/viewer/c04ad5
// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class SampleScene : MonoBehaviourPunCallbacks
{
    [SerializeField] private ObjectManager _objectManager;
    private void Start()
    {
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Player";

        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // ランダムなルームに参加する
        PhotonNetwork.JoinRandomRoom();
    }

    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ルームの参加人数を4人に設定する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        Debug.Log("Join Room");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players)
        {
            //_objectManager.AddPlayer(p);
            p.GetComponent<AvatarController>().Init(_objectManager);
        }
        var position = new Vector3(Random.Range(-3f, 3f), 3.0f, Random.Range(-3f, 3f));
        GameObject player = PhotonNetwork.Instantiate("ProtoType/Avatar", position, Quaternion.identity);
        Debug.Log(player);
        player.GetComponent<AvatarController>().Init(_objectManager);
    }

    [PunRPC]
    private void AddPlayer(GameObject player)
    {
        Debug.Log(player);
        //_objectManager.AddPlayer(player);
        player.GetComponent<AvatarController>().Init(_objectManager);
    }
}
