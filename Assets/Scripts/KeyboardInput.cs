using UnityEngine;

[RequireComponent(typeof(CubicMovement))]
public class KeyboardInput : MonoBehaviour
{
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
    }
}