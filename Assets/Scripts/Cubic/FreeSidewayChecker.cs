using System.Collections.Generic;
using UnityEngine;

public class FreeSidewayChecker : MonoBehaviour
{
    [SerializeField] private int _ignoreLayerMask = 7;

    private List<Collider> _trapColliders = new List<Collider>();

    public float Check(Transform movingObject, float maxSideDistance, Vector3 direction)
    {
        Collider[] allColliders;
        Vector3 area;
        float distance = maxSideDistance;

        distance += transform.localScale.z / 2;
        area = new Vector3(distance, 0, distance);

        allColliders = Physics.OverlapBox(movingObject.position, area, Quaternion.identity, _ignoreLayerMask);

        foreach (var collider in allColliders)
        {
            if (direction.z > 0)
            {
                if (collider.transform.position.z > movingObject.position.z)
                    _trapColliders.Add(collider);
            }
            else if (direction.z < 0)
            {
                if (collider.transform.position.z < movingObject.position.z)
                    _trapColliders.Add(collider);
            }
        }


        if (_trapColliders.Count > 0)
            return GetMaxSideDistance(movingObject, maxSideDistance);

        return maxSideDistance;
    }

    private float GetMaxSideDistance(Transform movingObject, float maxSideDistance)
    {
        float distance;
        Vector3 startPosition = movingObject.position;

        foreach (var collider in _trapColliders)
        {
            distance = Vector3.Distance(startPosition, collider.ClosestPoint(startPosition));

            if (maxSideDistance > distance)
                maxSideDistance = distance;
        }

        return maxSideDistance - transform.localScale.z / 2;
    }
}
