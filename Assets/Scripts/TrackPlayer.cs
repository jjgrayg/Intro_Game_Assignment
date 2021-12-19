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

    public MinMax YValueLimits;
    public MinMax XValueLimits;
    public Transform target;
    public Camera mainView;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (target)
        {
            Vector3 point = mainView.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - mainView.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            destination = new Vector3(
                        Mathf.Clamp(destination.x, XValueLimits.Min, XValueLimits.Max),
                        Mathf.Clamp(destination.y, YValueLimits.Min, YValueLimits.Max),
                        destination.z);
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}
