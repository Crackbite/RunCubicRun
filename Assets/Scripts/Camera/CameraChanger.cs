using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineBrain))]
public class CameraChanger : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private PistonMover _pistonMover;
    [SerializeField] private PressTopAnimator _pressTopAnimator;
    [SerializeField] private CinemachineVirtualCamera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera _cubicOnPressCamera;
    [SerializeField] private CinemachineVirtualCamera _pressCamera;

    private Coroutine _cameraBlendCoroutine;
    private CinemachineBrain _cinemachineBrain;

    public event Action CubicPressBlendFinished;

    private void Awake()
    {
        _cinemachineBrain = GetComponent<CinemachineBrain>();
        SwitchToMainCamera();
    }

    private void OnEnable()
    {
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
        _pistonMover.WorkCompleted += OnPistonMoverWorkCompleted;
        _pressTopAnimator.Completed += OnPressTopAnimationCompleted;
        _cinemachineBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
    }

    private void OnDisable()
    {
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
        _pistonMover.WorkCompleted -= OnPistonMoverWorkCompleted;
        _pressTopAnimator.Completed -= OnPressTopAnimationCompleted;
        _cinemachineBrain.m_CameraActivatedEvent.RemoveListener(OnCameraActivated);
    }

    private void OnCameraActivated(ICinemachineCamera newCamera, ICinemachineCamera previousCamera)
    {
        if (_cameraBlendCoroutine != null)
        {
            StopCoroutine(_cameraBlendCoroutine);
        }

        _cameraBlendCoroutine = StartCoroutine(WaitForBlendCompletion(newCamera));
    }

    private void OnCubicLeftPress()
    {
        SwitchToMainCamera();
    }

    private void OnCubicSteppedOnStand(PressStand pressStand)
    {
        SwitchToCubicOnPressCamera();
    }

    private void OnPistonMoverWorkCompleted()
    {
        SwitchToMainCamera();
    }

    private void OnPressTopAnimationCompleted()
    {
        SwitchToPressCamera(_pistonMover.transform);
    }

    private void SwitchToCubicOnPressCamera()
    {
        _cubicOnPressCamera.Priority = 1;
        _mainCamera.Priority = 0;
        _pressCamera.Priority = 0;
    }

    private void SwitchToMainCamera()
    {
        _mainCamera.Priority = 1;
        _pressCamera.Priority = 0;
        _cubicOnPressCamera.Priority = 0;
    }

    private void SwitchToPressCamera(Transform lookAt)
    {
        _pressCamera.Follow = lookAt;
        _pressCamera.LookAt = lookAt;

        _pressCamera.Priority = 1;
        _mainCamera.Priority = 0;
        _cubicOnPressCamera.Priority = 0;
    }

    private IEnumerator WaitForBlendCompletion(ICinemachineCamera newCamera)
    {
        while (_cinemachineBrain.IsBlending)
        {
            yield return null;
        }

        _cameraBlendCoroutine = null;

        if (ReferenceEquals(newCamera, _cubicOnPressCamera))
        {
            Transform highestBlock = _blockStack.Blocks[0].transform;
            SwitchToPressCamera(highestBlock);
        }
        else if (ReferenceEquals(newCamera, _pressCamera))
        {
            _cubic.SoundSystem.Play(SoundEvent.PressHit);
            CubicPressBlendFinished?.Invoke();
        }
    }
}