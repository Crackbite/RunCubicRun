using UnityEngine;

[RequireComponent(typeof(CubicMovement))]
public class KeyboardInput : MonoBehaviour
{
    private CubicMovement _�ubicMovement;

    private void Start()
    {
        _�ubicMovement = GetComponent<CubicMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _�ubicMovement.MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _�ubicMovement.MoveRight();
        }
    }
}