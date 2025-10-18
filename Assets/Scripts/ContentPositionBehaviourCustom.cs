using TMPro;
using UnityEngine;
using Vuforia;

public class ContentPositionBehaviourCustom : ContentPositioningBehaviour
{
    [SerializeField] private Transform arCameraTransform;
    [SerializeField] private Transform planeIndicatorTransform;
    [SerializeField] private Transform gameAreaTransform;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private bool isPinned;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fallSound;
    [SerializeField] private Animator basketballSetupAnimator;
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject startInfoPanel;

    public new void PositionContentAtPlaneAnchor(HitTestResult hitTestResult)
    {
        var arCamera2dPosition = new Vector2(arCameraTransform.position.x, arCameraTransform.position.z);
        var planeIndicator2dPosition =
            new Vector2(planeIndicatorTransform.position.x, planeIndicatorTransform.position.z);
        var distance = Vector2.Distance(arCamera2dPosition, planeIndicator2dPosition);

        if (isPinned || distance is >= 3 or <= 2.5F) return;

        base.PositionContentAtPlaneAnchor(hitTestResult);

        basketballSetupAnimator.Play("FallB");
        audioSource.PlayOneShot(fallSound);
        ball.SetActive(true);
        ball.GetComponentInChildren<Animator>().Play("BallFadeIn");
        startInfoPanel.SetActive(false);

        gameAreaTransform.LookAt(new Vector3(arCameraTransform.position.x,
            gameAreaTransform.position.y,
            arCameraTransform.position.z));
        isPinned = true;
    }
}