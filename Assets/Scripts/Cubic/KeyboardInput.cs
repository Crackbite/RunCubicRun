using UnityEngine;

[RequireComponent(typeof(CubicMovement))]
public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private PressSpeedReducer _pressSpeedReducer;
    
    private CubicMovement _cubicMovement;

    private void Start()
    {
        _cubicMovement = GetComponent<CubicMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _cubicMovement.MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _cubicMovement.MoveRight();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _cubicMovement.MoveForward();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _pressSpeedReducer.ReduceSpeed();
        }
    }
}