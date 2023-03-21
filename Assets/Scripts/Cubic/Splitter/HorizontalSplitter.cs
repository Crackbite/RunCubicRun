using System.Collections;
using UnityEngine;

public class HorizontalSplitter : Splitter
{
    [SerializeField] private float _slideDistance;
    [SerializeField] private float _slideSpeed;

    public override void Split()
    {
        SplitOnePart(FirstPart);
        SplitOnePart(SecondPart);
        StartCoroutine(Slide(FirstPart, SecondPart));
    }

    public void SplitOnePart(Transform part)
    {
        part.gameObject.SetActive(true);
    }

    private IEnumerator Slide(Transform upperPart, Transform bottomPart)
    {
        float currentDistanceX = 0;

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