using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IceRinkGameHelper : MonoBehaviour
{
    public bool IsSlippery { get; set; }

    public Vector2 SlideDirection { get; set; }

    public Vector3 PreviousFramePosition { get; set; }
    
    public static IceRinkGameHelper Instance { get; private set; }

    private Queue<bool> isSlidingWindow;
    private int queueSize = 75;
    private double threshold = 0.05;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        isSlidingWindow = new Queue<bool>();
        ResetIsSliding();
    }

    public void ResetIsSliding()
    {
        for (int i = 0; i < queueSize; i++)
        {
            isSlidingWindow.Enqueue(true);
        }
    }

    public bool IsSliding(Vector3 currentPosition)
    {
        isSlidingWindow.Enqueue(Mathf.Abs((currentPosition - PreviousFramePosition).magnitude) > 0);
        isSlidingWindow.Dequeue();
        return isSlidingWindow.Count(b => b) * 1.0 / isSlidingWindow.Count >= threshold;
    }
}
