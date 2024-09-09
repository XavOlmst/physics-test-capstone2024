using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdoll : MonoBehaviour
{
    [Tooltip("Value above 8")]
    [SerializeField] private int _solverIterations = 15;
    [Tooltip("Value above 8")]
    [SerializeField] private int _solverVelocityIterations = 12;
    [Tooltip("Value above 20")] 
    [SerializeField] private float _maxAngularVelocity = 30;

    [Tooltip("1st is animation, 2nd ragdoll")]
    [SerializeField] private List<Transform> _hips;

    [SerializeField] private List<Transform> _animatedTransforms;
    [SerializeField] private List<ConfigurableJoint> _configurableJoints;
    private List<Rigidbody> _rigidbodies = new();
    private List<Quaternion> _initialJointRotations = new();

    private void AddRigidbody(Rigidbody rb)
    {
        if (_rigidbodies.Contains(rb)) return;

        _rigidbodies.Add(rb);
    }

    private void Start()
    {
        foreach (ConfigurableJoint joint in _configurableJoints)
        {
            _initialJointRotations.Add(joint.transform.rotation);

            Rigidbody rb = joint.GetComponent<Rigidbody>();
            AddRigidbody(rb);

            rb = joint.connectedBody;
            AddRigidbody(rb);
        }

        foreach(Rigidbody rb in _rigidbodies)
        {
            rb.solverIterations = _solverIterations;
            rb.solverVelocityIterations = _solverVelocityIterations;
            rb.maxAngularVelocity = _maxAngularVelocity;
        }
    }

    private void FixedUpdate()
    {
        _hips[1].position = _hips[0].position; //fucking hacks

        for (int jointIndex = 0; jointIndex < _configurableJoints.Count; jointIndex++)
        {
            ConfigurableJoint joint = _configurableJoints[jointIndex];
            Transform animationTransform = _animatedTransforms[jointIndex];

            ConfigurableJointRotations.SetTargetLocalRotation(joint, animationTransform.localRotation, joint.transform.localRotation);
        }
    }

}
