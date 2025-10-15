using System.Globalization;
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

    public new void PositionContentAtPlaneAnchor(HitTestResult hitTestResult)
    {
        var arCamera2dPosition = new Vector2(arCameraTransform.position.x, arCameraTransform.position.z);
        var planeIndicator2dPosition =
            new Vector2(planeIndicatorTransform.position.x, planeIndicatorTransform.position.z);
        var distance = Vector2.Distance(arCamera2dPosition, planeIndicator2dPosition);

        debugText.text = distance.ToString(CultureInfo.CurrentCulture);

        if (isPinned || distance is >= 4 or <= 3) return;

        base.PositionContentAtPlaneAnchor(hitTestResult);
        gameAreaTransform.LookAt(new Vector3(arCameraTransform.position.x,
            gameAreaTransform.position.y,
            arCameraTransform.position.z));
        isPinned = true;
    }
}