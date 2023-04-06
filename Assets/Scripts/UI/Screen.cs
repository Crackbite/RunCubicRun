using UnityEngine;

public abstract class Screen : MonoBehaviour
{
    public virtual void Enter()
    {
        gameObject.SetActive(true);
    }

    public virtual void Exit()
    {
        gameObject.SetActive(false);
    }
}