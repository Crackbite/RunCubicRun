using System.Collections.Generic;
using UnityEngine;

public class FreeSidewayChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _ignoreLayerMask = 1 << 7;

    private readonly List<Collider> _trapColliders = new();

    public float Check(Transform movingObject, float maxSideDistance, Vector3 direction)
    {
        const int MaxAmount = 5;

        float distance = maxSideDistance;
        distance += transform.localScale.z / 2f;

        var area = new Vector3(distance, 0, distance);
        var colliders = new Collider[MaxAmount];
        int amount = Physics.OverlapBoxNonAlloc(
            movingObject.position,
            area,
            colliders,
            Quaternion.identity,
            _ignoreLayerMask);

        for (int i = 0; i < amount; i++)
        {
            Collider currentCollider = colliders[i];

            if (direction.z > 0)
            {
                if (currentCollider.transform.position.z > movingObject.position.z)
                {
                    _trapColliders.Add(currentCollider);
                }
            }
            else if (direction.z < 0)
            {
                if (currentCollider.transform.position.z < movingObject.position.z)
                {
                    _trapColliders.Add(currentCollider);
                }
            }
        }

        return _trapColliders.Count > 0 ? GetMaxSideDistance(movingObject, maxSideDistance) : maxSideDistance;
    }

    private float GetMaxSideDistance(Transform movingObject, float maxSideDistance)
    {
        Vector3 startPosition = movingObject.position;

        foreach (Collider currentCollider in _trapColliders)
        {
            float distance = Vector3.Distance(startPosition, currentCollider.ClosestPoint(startPosition));

            if (maxSideDistance > distance)
            {
                maxSideDistance = distance;
            }
        }

        return maxSideDistance - (transform.localScale.z / 2f);
    }
}