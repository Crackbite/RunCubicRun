using UnityEngine;

[RequireComponent(typeof(CubicMovement))]
public class SwipeInput : MonoBehaviour
{
    private Vector3 _mousePreviousPosition;
    private CubicMovement _ñubicMovement;

    private void Start()
    {
        _ñubicMovement = GetComponent<CubicMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePreviousPosition = Input.mousePosition;
        }

        Vector3 mousePosition = Input.mousePosition;

        if (Input.GetMouseButtonUp(0) == false || mousePosition == _mousePreviousPosition
                                               || Mathf.Abs(mousePosition.x - _mousePreviousPosition.x)
                                               > Mathf.Abs(mousePosition.y - _mousePreviousPosition.y) == false)
        {
            return;
        }

        if (mousePosition.x > _mousePreviousPosition.x)
        {
            _ñubicMovement.MoveRight();
        }
        else
        {
            _ñubicMovement.MoveLeft();
        }
    }
}