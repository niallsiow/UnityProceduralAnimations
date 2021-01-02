using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Manager : MonoBehaviour
{
    public RobotJoint[] joints;

    public float samplingDistance;
    public float learningRate;

    public Transform Target;

    private void Update()
    {
        Vector3 ikTarget = Target.position;
        InverseKinematics(ikTarget);
        Debug.Log("End Effector Position: " + ForwardKinematics() + ", Distance From Target = " + DistanceFromTarget(ikTarget));
    }

    public Vector3 ForwardKinematics()
    {
        Vector3 prevPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;

        for(int i = 1; i < joints.Length; i++)
        {
            // Rotates around a new axis
            rotation *= Quaternion.AngleAxis(joints[i - 1].angle, joints[i - 1].Axis);
            Vector3 nextPoint = prevPoint + rotation * joints[i].StartOffset;

            prevPoint = nextPoint;
        }

        return prevPoint;
    }


    
    public float DistanceFromTarget(Vector3 target)
    {
        Vector3 point = ForwardKinematics();
        return Vector3.Distance(point, target);
    }

    public float PartialGradient(Vector3 target, int i)
    {
        // Saves the angle, it will be restored later
        float angle = joints[i].angle;

        float f_x = DistanceFromTarget(target);

        joints[i].angle += samplingDistance;
        float f_x_plus_d = DistanceFromTarget(target);

        float gradient = (f_x_plus_d - f_x) / samplingDistance;

        joints[i].angle = angle;

        return gradient;
    }

    public void InverseKinematics(Vector3 target)
    {
        for(int i = 0; i < joints.Length; i++)
        {
            float gradient = PartialGradient(target, i);
            float angle = learningRate * gradient;

            joints[i].UpdateJointAngle(angle);
        }
    }
}