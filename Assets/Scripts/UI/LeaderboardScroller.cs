using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScroller : MonoBehaviour
{
    [SerializeField] private LeaderboardScreen _leaderboardScreen;
    [SerializeField] private ScrollRect _scrollView;
    [SerializeField] private RectTransform _content;

    private void OnEnable()
    {
        _leaderboardScreen.Showed += OnLeaderboardShowed;
    }

    private void OnDisable()
    {
        _leaderboardScreen.Showed -= OnLeaderboardShowed;
    }

    private void OnLeaderboardShowed()
    {
        if (IsPlayerNameVisible(_scrollView) == false)
        {
            Scroll();
        }
    }

    private void Scroll()
    {
        Transform targetTransform = _leaderboardScreen.Player.transform;
        Vector3 targetPosition = _content.InverseTransformPoint(targetTransform.position);
        _content.anchoredPosition = -targetPosition;
    }

    private bool IsPlayerNameVisible(ScrollRect scrollView)
    {
        const int cornersCount = 4;

        Vector3[] corners = new Vector3[cornersCount];
        RectTransform playerNameRect = _leaderboardScreen.Player.GetComponent<RectTransform>();
        playerNameRect.GetWorldCorners(corners);

        RectTransform scrollViewRect = scrollView.viewport;
        Vector3[] scrollCorners = new Vector3[cornersCount];
        scrollViewRect.GetWorldCorners(scrollCorners);

        for (int i = 0; i < cornersCount; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(scrollViewRect, corners[i]) == false)
            {
                return false;
            }
        }

        return true;
    }
}





