using UnityEngine;

public class BallAnimationEventHandler : MonoBehaviour
{
    public bool IsDestructiveFadingOut { get; private set; }
    public bool IsDelayBeforeDestructiveFadingOut { get; private set; }
    public bool IsFadingIn { get; private set; }

    private void OnDestructiveFadeOutDelayBegin()
    {
        IsDelayBeforeDestructiveFadingOut = true;
    }

    private void OnDestructiveFadeOutBegin()
    {
        IsDestructiveFadingOut = true;
        IsDelayBeforeDestructiveFadingOut = false;
    }

    private void OnDestructiveFadeOutFinish()
    {
        GetComponentInParent<BallBehaviour>().RestorePosition();
        IsDestructiveFadingOut = false;
    }

    private void OnFadeInBegin()
    {
        IsFadingIn = true;
    }

    private void OnFadeInFinish()
    {
        IsFadingIn = false;
    }
}