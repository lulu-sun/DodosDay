using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IceRinkGameHelper : MonoBehaviour
{
    public bool OnIce { get; set; }

    public Vector2 SlideDirection { get; set; }

    public Vector3 PreviousFramePosition { get; set; }
    
    public static IceRinkGameHelper Instance { get; private set; }

    private Queue<bool> isSlidingHistory;
    private int queueSize = 10;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isSlidingHistory = new Queue<bool>();
        ResetIsSliding();
    }

    public void ResetIsSliding()
    {
        for (int i = 0; i < queueSize; i++)
        {
            isSlidingHistory.Enqueue(true);
        }
    }

    public bool IsSliding(Vector3 currentPosition)
    {
        isSlidingHistory.Enqueue(Mathf.Abs((currentPosition - PreviousFramePosition).magnitude) > 0);
        isSlidingHistory.Dequeue();
        bool isSliding = isSlidingHistory.Count(b => b) * 1.0 / isSlidingHistory.Count > 0;
        //Debug.Log($"isSliding: {isSliding}");
        //Debug.Log($"previousPosition == currentPosition: {currentPosition == PreviousFramePosition}");
        return isSliding;
    }
}
