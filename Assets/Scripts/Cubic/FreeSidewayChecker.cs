using System.Collections.Generic;
using UnityEngine;

public class FreeSidewayChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _ignoreLayerMask = 1 << 7;

    private readonly List<Collider> _trapColliders = new();

    public float Check(Transform movingObject, float maxSideDistance, Vector3 direction)
    {
        float distance = maxSideDistance;
        int maxAmount = 1;

        distance += transform.localScale.z / 2f;
        Vector3 area = new Vector3(distance, 0, distance);
        Collider[] colliders = new Collider[maxAmount];
        int amount = Physics.OverlapBoxNonAlloc(
            movingObject.position,
            area,
            colliders,
            Quaternion.identity,
            _ignoreLayerMask);

        for (int i = 0; i < amount; i++)
        {
            if (direction.z > 0)
            {
                if (colliders[i].transform.position.z > movingObject.position.z)
                {
                    _trapColliders.Add(colliders[i]);
                }
            }
            else if (direction.z < 0)
            {
                if (colliders[i].transform.position.z < movingObject.position.z)
                {
                    _trapColliders.Add(colliders[i]);
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