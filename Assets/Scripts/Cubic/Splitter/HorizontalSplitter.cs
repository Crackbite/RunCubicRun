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
        Vector3 nextPosition;

        while (currentDistanceX <= _slideDistance)
        {
            nextPosition = bottomPart.transform.position;
            nextPosition.x = bottomPart.transform.position.x - _slideDistance;
            upperPart.position = Vector3.Lerp(upperPart.position, nextPosition, _slideSpeed * Time.deltaTime);
            currentDistanceX = bottomPart.transform.position.x - upperPart.position.x;
            yield return null;
        }
    }
}