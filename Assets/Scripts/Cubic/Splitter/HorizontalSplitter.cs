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

    private IEnumerator Slide(Transform upperPart, Transform bottomPart)
    {
        float currentDistanceX = 0f;

        while (currentDistanceX <= _slideDistance)
        {
            Vector3 nextPosition = bottomPart.position;
            nextPosition.x -= _slideDistance;

            upperPart.position = Vector3.Lerp(upperPart.position, nextPosition, _slideSpeed * Time.deltaTime);
            currentDistanceX = bottomPart.position.x - upperPart.position.x;

            yield return null;
        }
    }
}