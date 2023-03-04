using UnityEngine;

[RequireComponent(typeof(CubicMovement))]
public class KeyboardInput : MonoBehaviour
{
    private CubicMovement _ñubicMovement;

    private void Start()
    {
        _ñubicMovement = GetComponent<CubicMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _ñubicMovement.MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _ñubicMovement.MoveRight();
        }
    }
}