using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private bool[] _goalTriggersCollisions;

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
        _goalTriggersCollisions = new bool[4];
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Floor":
                _audioSource.PlayOneShot(bounceSound, other.relativeVelocity.magnitude / 2F);
                if (!IsThrown || _animationEventHandler.IsDestructiveFadingOut) return;
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Goal Trigger":
                switch (other.gameObject.name)
                {
                    case "Goal Trigger 1":
                        _firstGoalTime = Time.time;
                        _goalTriggersCollisions[0] = true;
                        break;
                    case "Goal Trigger 2":
                        if (_goalTriggersCollisions[0]) _goalTriggersCollisions[1] = true;
                        break;
                    case "Goal Trigger 3":
                        if (_goalTriggersCollisions[1]) _goalTriggersCollisions[2] = true;
                        break;
                    case "Goal Trigger 4":
                    {
                        if (_goalTriggersCollisions[2])
                        {
                            _goalTriggersCollisions[3] = true;
                            if (_goalTriggersCollisions.All(x => x))
                            {
                                game.Goal();
                                _isGoal = true;
                            }

                            _goalTriggersCollisions = new bool[4];
                        }

                        if (Time.time - _firstGoalTime < 0.24F)
                            _audioSource.PlayOneShot(
                                fastGoalHoopSounds[Random.Range(0, fastGoalHoopSounds.Length - 1)]);
                        break;
                    }
                }


                break;
            case "Side Limiter":
                if (!IsThrown || _animationEventHandler.IsDestructiveFadingOut) return;
                _animator.Play("BallFadeOutFast");
                if (!_isGoal) game.Lose();
                break;
        }
    }

    public void RestorePosition()
    {
        transform.parent = ballSpawner.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        _rigidbody.isKinematic = true;
        if (!IsTooClose)
        {
            _animator.Play("BallFadeIn");
            game.PlayCatchSound();
        }

        _goalTriggersCollisions = new bool[4];
        _isGoal = false;
        IsThrown = false;
        foreach (var goal in goalTriggers) goal.GetComponent<BoxCollider>().enabled = true;
    }

    public void Throw(Vector2 touchStartPosition, Vector2 touchEndPosition, float timeDifference)
    {
        const float maxHoldingTime = 0.4F;

        if (IsThrown || IsTooClose || _animationEventHandler.IsFadingIn || timeDifference > maxHoldingTime ||
            touchStartPosition.y > Screen.height / 1.5F || Time.timeScale == 0 ||
            touchEndPosition.y <= touchStartPosition.y) return;

        IsThrown = true;
        _audioSource.PlayOneShot(throwSound, 0.8F);

        var xImpulse = Mathf.Clamp((touchEndPosition.x - touchStartPosition.x) / Screen.width / 2F, -2, 2);

        var yImpulse = Mathf.Clamp(2.4F * (touchEndPosition.y - touchStartPosition.y) / Screen.height, 1.5F, 2);
        var zImpulse = Mathf.Clamp(maxHoldingTime / timeDifference / 4F, 0.5F, 2F);

        transform.parent = null;
        _rigidbody.isKinematic = false;
        _rigidbody.AddRelativeForce(xImpulse, yImpulse, zImpulse, ForceMode.Impulse);
        _rigidbody.AddRelativeTorque(zImpulse, 0, xImpulse, ForceMode.Impulse);
    }
}