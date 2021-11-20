using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackPlayer : MonoBehaviour
{
    [System.Serializable]
    public class MinMax
    {
        public float Min;
        public float Max;

        public MinMax(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString() => $"({Min}, {Max})";
    }

    public MinMax maxYVals;
    public MinMax maxXVals;
    public Transform target;
    public Camera mainView;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 point = mainView.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - mainView.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            destination = new Vector3(
                        Mathf.Clamp(destination.x, maxXVals.Min, maxXVals.Max),
                        Mathf.Clamp(destination.y, maxYVals.Min, maxYVals.Max),
                        destination.z);
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}
