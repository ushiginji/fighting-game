using Photon.Pun;
using UnityEngine;

// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class AvatarController : MonoBehaviourPunCallbacks
{
    private void Update()
    {
        // 自身が生成したオブジェクトだけに移動処理を行う
        if (photonView.IsMine)
        {
            var input = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow)) 
            {
                input = Vector3.forward;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                input = Vector3.back;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                input = Vector3.right;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                input = Vector3.left;
            }

            transform.Translate(6f * Time.deltaTime * input);
        }
    }
}