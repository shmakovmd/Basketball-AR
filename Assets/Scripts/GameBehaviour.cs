using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using Task = System.Threading.Tasks.Task;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject ball;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private AudioClip goalSound;
    [SerializeField] private Animator goalLabel;
    [SerializeField] private Animator newRecordLabel;
    [SerializeField] private AudioClip[] catchSounds;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip newRecordSound;

    private bool _isGameSession;
    private bool _newRecord;

    private int _record;
    private int _score;

    public async void Goal()
    {
        try
        {
            scoreText.text = (++_score).ToString();
            audioSource.PlayOneShot(goalSound);
            goalLabel.Play("GoalLabelFadeInOut");

            if (_score > _record)
            {
                if (!_newRecord && _isGameSession)
                {
                    await Task.Delay(1000);
                    newRecordLabel.Play("NewRecordLabelFlash");
                    audioSource.PlayOneShot(newRecordSound);
                }

                recordText.text = (_record = _score).ToString();
                _newRecord = true;
            }

            _isGameSession = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void Lose()
    {
        if (!_isGameSession) return;
        audioSource.PlayOneShot(loseSound);
        _isGameSession = false;
        scoreText.text = (_score = 0).ToString();
        _newRecord = false;
    }

    public void PlayCatchSound()
    {
        audioSource.PlayOneShot(catchSounds[Random.Range(0, catchSounds.Length)]);
    }
}