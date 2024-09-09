//CODE INSPIRED BY https://gist.github.com/mstevenson/7b85893e8caf5ca034e6
// Github Username: mstevenson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigurableJointRotations
{
    public static void SetTargetLocalRotation(this ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation)
    {
        if (joint.configuredInWorldSpace) return;
        InternalSetTargetRotation(joint, targetRotation, startRotation, Space.Self);
    }

    public static void SetTargetWorldRotation(this ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation)
    {
        if (!joint.configuredInWorldSpace) return;
        InternalSetTargetRotation(joint, targetRotation, startRotation, Space.World);
    }

    private static void InternalSetTargetRotation(ConfigurableJoint joint, Quaternion targetRot, Quaternion startRot, Space space)
    {
        Vector3 right = joint.axis;
        Vector3 forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
        Vector3 up = Vector3.Cross(forward, right).normalized;

        Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);
        Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);

        switch(space)
        {
            case Space.World:
                resultRotation *= startRot * Quaternion.Inverse(targetRot);
                break;
            case Space.Self:
                resultRotation *= Quaternion.Inverse(targetRot) * startRot;
                break;
        }

        resultRotation *= worldToJointSpace;
        joint.targetRotation = resultRotation;
    }
}
