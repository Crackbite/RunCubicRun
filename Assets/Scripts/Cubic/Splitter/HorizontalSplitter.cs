using System.Collections;
using UnityEngine;

public class HorizontalSplitter : Splitter
{
    [SerializeField] private float _slideDistance = .6f;
    [SerializeField] private float _slideSpeed = .9f;

    public override void Split()
    {
        SplitOnePart(FirstPart);
        SplitOnePart(SecondPart);
        StartCoroutine(Slide(FirstPart, SecondPart));
    }

    public override void SplitOnePart(Transform part)
    {
        part.gameObject.SetActive(true);
    }

    private IEnumerator Slide(Transform topPart, Transform bottomPart)
    {
        const float Tolerance = 0.3f;
        float currentDistanceX = 0f;
        float initialPositionZ = bottomPart.position.z;

        while (currentDistanceX <= _slideDistance)
        {
            Vector3 nextPosition = bottomPart.position;
            nextPosition.x -= _slideDistance;

            topPart.position = Vector3.Lerp(topPart.position, nextPosition, _slideSpeed * Time.deltaTime);
            currentDistanceX = bottomPart.position.x - topPart.position.x;

            if (Mathf.Abs(bottomPart.position.z - initialPositionZ) > Tolerance ||
                Mathf.Abs(topPart.position.z - initialPositionZ) > Tolerance)
            {
                topPart.parent = null;
                bottomPart.parent = null;
                yield break;
            }

            yield return null;
        }

        topPart.parent = null;
        bottomPart.parent = null;
    }
}