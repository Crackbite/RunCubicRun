using UnityEngine;

public class Crusher : Bonus
{
    [SerializeField] private Cubic _cubic;

    private void Start()
    {
        if (_cubic == null)
        {
            _cubic = FindObjectOfType<Cubic>();
        }
    }

    public override void Apply()
    {
        _cubic.CanDestroy = true;
    }

    public override void Cancel()
    {
        _cubic.CanDestroy = false;
    }
}