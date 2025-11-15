using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloatingHoopBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    private Vector3 _aPosition;
    private Vector3 _bPosition;
    private bool _isLoop;

    private Vector3 _nextPosition;
    private float _sinTime;
    private Vector3 _startPosition;

    private void Start()
    {
        _nextPosition = _startPosition = transform.localPosition;
    }

    private void Update()
    {
        switch (_isLoop)
        {
            case true:
                if (ComparePositions(transform.localPosition, _aPosition))
                {
                    _sinTime = 0;
                    _nextPosition = _bPosition;
                }

                if (ComparePositions(transform.localPosition, _bPosition))
                {
                    _sinTime = 0;
                    _nextPosition = _aPosition;
                }

                break;
            case false when ComparePositions(transform.localPosition, _nextPosition):
                return;
        }

        const float moveSpeed = 0.25F;
        _sinTime += Time.deltaTime * moveSpeed;
        _sinTime = Mathf.Clamp(_sinTime, 0, (float)Math.PI);
        var t = 0.5F * Mathf.Sin(_sinTime - (float)Math.PI / 2F) + 0.5F;
        transform.localPosition =
            Vector3.Slerp(transform.localPosition, _nextPosition, t);
    }

    private static bool ComparePositions(Vector3 a, Vector3 b)
    {
        const float tolerance = 0.001F;
        return Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance && Math.Abs(a.z - b.z) < tolerance;
    }

    private void MoveToRandomPosition()
    {
        Debug.Log("MoveToRandomPosition");
        // TODO: make random delta position more far
        _isLoop = false;
        debugText.text = _nextPosition.ToString();
        _sinTime = 0;
        _nextPosition = new Vector3(Random.Range(_startPosition.x - 0.5F, _startPosition.x + 0.5F),
            Random.Range(_startPosition.y - 0.24F, _startPosition.y + 0.24F), transform.localPosition.z);
    }

    private void MoveOneAxis()
    {
        Debug.Log("MoveOneAxis");
        _isLoop = true;
        _sinTime = 0;
        var randPos = Random.Range(_startPosition.x - 0.5F, _startPosition.x + 0.5F);
        _aPosition = new Vector3(randPos, transform.localPosition.y, transform.localPosition.z);
        _bPosition = new Vector3(randPos + Random.Range(-0.35F, 0.35F),
            Random.Range(_startPosition.x - 0.5F, _startPosition.x + 0.5F),
            transform.localPosition.z);
        _nextPosition = _aPosition;
    }

    private void MoveTwoAxes()
    {
        Debug.Log("MoveOneAxis");
    }

    private void MoveWithGates()
    {
        Debug.Log("MoveWithGates");
    }

    private void MoveWithOtherObstacles()
    {
        Debug.Log("MoveWithOtherObstacles");
    }

    public void StartNextMoving(int score)
    {
        MoveToRandomPosition();
/*
        switch (score)
        {
            case > 2 and < 8:
                MoveToRandomPosition();
                break;
            case > 7 and < 13:
                MoveOneAxis();
                break;
            case > 12 and < 18:
                MoveTwoAxes();
                break;
            case > 17 and < 26:
                MoveWithGates();
                break;
            case > 25:
                MoveWithOtherObstacles();
                break;
        }
*/
    }
}