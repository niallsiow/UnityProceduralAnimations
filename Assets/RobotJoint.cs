using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotJoint : MonoBehaviour
{

    public Vector3 Axis;

    public Vector3 StartOffset;

    public float angle;

    private void Awake()
    {
        StartOffset = transform.localPosition;
    }

    private void Update()
    {
        angle = transform.localEulerAngles.x;
    }

    public void UpdateJointAngle(float inputAngle)
    {
        transform.Rotate(Axis, inputAngle);
    }
}
