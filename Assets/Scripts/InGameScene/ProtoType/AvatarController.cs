using Photon.Pun;
using UnityEngine;

// MonoBehaviourPunCallbacks���p�����āAphotonView�v���p�e�B���g����悤�ɂ���
public class AvatarController : MonoBehaviourPunCallbacks
{
    private void Update()
    {
        // ���g�����������I�u�W�F�N�g�����Ɉړ��������s��
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