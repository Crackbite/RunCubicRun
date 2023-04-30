using UnityEngine;

public class HorizontalSaw : Saw
{
    [SerializeField] private bool _canRandomizeSpeed;

    protected override void SetSpeed()
    {
        if (_canRandomizeSpeed)
        {
            base.SetSpeed();
        }
        else
        {
            return;
        }
    }
}
