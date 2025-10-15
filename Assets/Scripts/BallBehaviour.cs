using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private Transform ballSpawner;
    [SerializeField] private float xImp;
    [SerializeField] private float yImp;
    [SerializeField] private float zImp;
    [SerializeField] private GameObject[] goalTriggers;
    [SerializeField] private GameBehaviour game;

    [SerializeField] private AudioClip throwSound;
    [SerializeField] private AudioClip[] fastGoalHoopSounds;
    [SerializeField] private AudioClip shieldSound;
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private AudioClip[] ringSounds;

    private BallAnimationEventHandler _animationEventHandler;
    private Animator _animator;
    private AudioSource _audioSource;
    private float _firstGoalTime;
    private int _goalTriggersCount;
    private bool _isGoal;
    private Rigidbody _rigidbody;

    private bool IsThrown { get; set; }
    public bool IsTooClose { get; set; }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _animationEventHandler = GetComponentInChildren<BallAnimationEventHandler>();
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Floor":
                if (_animationEventHandler.IsDestructiveFadingOut) return;
                _audioSource.PlayOneShot(bounceSound, other.relativeVelocity.magnitude);
                _animator.Play("BallFadeOutSlow");
                if (!_isGoal) game.Lose();
                break;
            case "Basketball Shield":
                _audioSource.PlayOneShot(shieldSound);
                break;
            case "Basketball Ring":
                _audioSource.PlayOneShot(ringSounds[Random.Range(0, ringSounds.Length)],
                    Mathf.Clamp(other.relativeVelocity.magnitude / 3F, 0, 1));
                break;
            case "Basketball Column":
                _audioSource.PlayOneShot(bounceSound, other.relativeVelocity.magnitude);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Goal Trigger":
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                _goalTriggersCount++;
                if (_goalTriggersCount == goalTriggers.Length)
                {
                    game.Goal();
                    _isGoal = true;
                }

                switch (other.gameObject.name)
                {
                    case "Goal Trigger 1":
                        _firstGoalTime = Time.time;
                        break;
                    case "Goal Trigger 4":
                    {
                        if (Time.time - _firstGoalTime < 0.24F)
                            _audioSource.PlayOneShot(
                                fastGoalHoopSounds[Random.Range(0, fastGoalHoopSounds.Length - 1)]);
                        break;
                    }
                }

                break;
            case "Side Limiter":
                if (_animationEventHandler.IsDestructiveFadingOut) return;
                _animator.Play("BallFadeOutFast");
                if (!_isGoal) game.Lose();
                break;
        }
    }

    public void RestorePosition()
    {
        Debug.Log("RestorePosition");
        transform.parent = ballSpawner.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        _rigidbody.isKinematic = true;
        if (!IsTooClose)
        {
            _animator.Play("BallFadeIn");
            game.PlayCatchSound();
        }

        _goalTriggersCount = 0;
        _isGoal = false;
        IsThrown = false;
        foreach (var goal in goalTriggers) goal.GetComponent<BoxCollider>().enabled = true;
    }

    public void Throw(Vector2 touchStartPosition, Vector2 touchEndPosition, float timeDifference)
    {
        Debug.Log(
            $"Throwing Ball {touchStartPosition}, {touchEndPosition}, {timeDifference} {Screen.width} {Screen.height}");

        const float maxHoldingTime = 0.4F;

        if (IsThrown || IsTooClose || _animationEventHandler.IsFadingIn || timeDifference > maxHoldingTime) return;

        IsThrown = true;
        _audioSource.PlayOneShot(throwSound, 0.8F);

        var xImpulse = Mathf.Clamp((touchEndPosition.x - touchStartPosition.x) / Screen.width, 2, -2);
        var yImpulse = Mathf.Clamp((touchEndPosition.y - touchStartPosition.y) / (Screen.height / 5F), 0, 1);
        var zImpulse = maxHoldingTime / timeDifference / 3;

        transform.parent = null;
        _rigidbody.isKinematic = false;
        // _rigidbody.AddRelativeForce(xImpulse, yImpulse, zImpulse, ForceMode.Impulse);
        _rigidbody.AddRelativeForce(xImp, yImp, zImp, ForceMode.Impulse);
        _rigidbody.AddRelativeTorque(zImp, 0, 0, ForceMode.Impulse);
        // _rigidbody.AddRelativeTorque(zImpulse, 0, xImpulse, ForceMode.Impulse);
    }
}