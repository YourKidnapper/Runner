using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class PhysicalBodyPart : MonoBehaviour
{

    [SerializeField] private Transform target;
    private ConfigurableJoint joint;
    private Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        joint.targetRotation = Quaternion.Inverse(target.localRotation) * startRotation;
    }
}
