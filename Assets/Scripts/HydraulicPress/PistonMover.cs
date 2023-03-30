using System;
using UnityEngine;

[RequireComponent(typeof(PressSpeedHandler), typeof(PistonPresser))]
public class PistonMover : MonoBehaviour
{
    [SerializeField] private PressStand _pressStand;
    [SerializeField] private float _crushedCubicSizeY = .1f;

    private bool _isCubicReached;
    private PistonPresser _pistonPresser;
    private PressSpeedHandler _pressSpeedHandler;
    private Cubic _cubic;

    public event Action WorkCompleted;

    public bool IsWorking { get; private set; }

    private void Awake()
    {
        _pistonPresser = GetComponent<PistonPresser>();
        _pressSpeedHandler = GetComponent<PressSpeedHandler>();
    }

    private void OnEnable()
    {
        _pistonPresser.CubicReached += PistonPresserOnCubicReached;
    }

    private void Update()
    {
        if (IsWorking == false)
        {
            return;
        }

        float speed = _isCubicReached ? _pressSpeedHandler.CubicPressSpeed : _pressSpeedHandler.GetCurrentSpeed();

        Vector3 currentPosition = transform.position;
        Vector3 newPosition = currentPosition;

        float pressStandTopYPosition = _pressStand.Bounds.max.y + _crushedCubicSizeY;
        newPosition.y = pressStandTopYPosition;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(currentPosition, newPosition, step);

        if (Mathf.Approximately(newPosition.y, transform.position.y))
        {
            if (_cubic != null)
            {
                CrushCubic();
            }

            IsWorking = false;
            WorkCompleted?.Invoke();
        }
    }

    private void OnDisable()
    {
        _pistonPresser.CubicReached -= PistonPresserOnCubicReached;
    }

    public void Init()
    {
        _pressSpeedHandler.Init();
    }

    public void TurnOff()
    {
        IsWorking = false;
    }

    public void TurnOn()
    {
        IsWorking = true;
    }

    private void PistonPresserOnCubicReached(Cubic cubic)
    {
        _cubic = cubic;
        _isCubicReached = true;
    }

    private void CrushCubic()
    {
        float positionY = _cubic.transform.position.y + _cubic.Bounds.extents.y + _crushedCubicSizeY;
        _cubic.transform.localScale = new Vector3(_cubic.transform.localScale.x, _crushedCubicSizeY, _cubic.transform.localScale.z);
        _cubic.transform.position = new Vector3(_cubic.transform.position.x, positionY, _cubic.transform.position.z);
    }
}