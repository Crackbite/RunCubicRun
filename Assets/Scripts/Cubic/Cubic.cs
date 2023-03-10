using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubic : MonoBehaviour
{
    [SerializeField] private bool _canDestroy;

    public bool CanDestroy => _canDestroy;
}
