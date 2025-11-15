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
    [SerializeField] private GameObject startInfoPanel;
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private Animator basketballSetupAnimator;
    [SerializeField] private AudioClip fallSound;
    [SerializeField] private Transform gameAreaTransform;
    [SerializeField] private Transform arCameraTransform;
    [SerializeField] private FloatingHoopBehaviour floatingHoopBehaviour;
    [SerializeField] private bool isFloatingHoop;
    [SerializeField] private bool isNetworkGame;

    private bool _isGameSession;
    private bool _isNewRecord;

    private Connection _networkConnection;

    private int _record;
    private int _score;

    public bool IsNetworkGame => isNetworkGame;

    private void Awake()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        gameAreaTransform.parent = null;
        InitializeGame();
#endif
    }

    private void Start()
    {
        recordText.text = (_record = saveSystem.gameData.record).ToString();
        // _networkConnection = GetComponent<Connection>();
    }

    public void InitializeGame()
    {
        if (!isFloatingHoop) audioSource.PlayOneShot(fallSound);

        basketballSetupAnimator.Play("FallB");
        ball.SetActive(true);
        ball.GetComponentInChildren<Animator>().Play("BallFadeIn");
        startInfoPanel.SetActive(false);
        gameAreaTransform.LookAt(new Vector3(arCameraTransform.position.x,
            gameAreaTransform.position.y,
            arCameraTransform.position.z));
    }

    public async void Goal()
    {
        try
        {
            // if (isNetworkGame && !ball.GetComponent<NetworkObject>().IsOwner) return;

            scoreText.text = (++_score).ToString();
            audioSource.PlayOneShot(goalSound, 0.5F);
            goalLabel.Play("GoalLabelFadeInOut");

            if (isFloatingHoop) floatingHoopBehaviour.StartNextMoving(_score);

            if (_score > _record)
            {
                if (!_isNewRecord && _isGameSession)
                {
                    await Task.Delay(1000);
                    newRecordLabel.Play("NewRecordLabelFlash");
                    audioSource.PlayOneShot(newRecordSound);
                }

                recordText.text = (saveSystem.gameData.record = _record = _score).ToString();
                _isNewRecord = true;
                saveSystem.Save();
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
        // if (isNetworkGame && !ball.GetComponent<NetworkObject>().IsOwner) return;
        if (!_isGameSession) return;
        audioSource.PlayOneShot(loseSound, 0.5F);
        _isGameSession = false;
        scoreText.text = (_score = 0).ToString();
        _isNewRecord = false;
    }

    public void PlayCatchSound()
    {
        // if (isNetworkGame && !ball.GetComponent<NetworkObject>().IsOwner) return;

        audioSource.PlayOneShot(catchSounds[Random.Range(0, catchSounds.Length)]);
    }
}