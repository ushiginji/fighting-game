using UnityEngine;
using UnityEngine.UI;

public class CountUpTimer : MonoBehaviour
{
    public Text timerText;  // Unity��Inspector�ł��̃t�B�[���h��Text�R���|�[�l���g���A�T�C������
    public float startTime = 0f;  // �J�n���̎��Ԃ�0�ɐݒ�
    private float timeElapsed;

    void Start()
    {
        timeElapsed = startTime;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        UpdateTimerDisplay(timeElapsed);
    }

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
