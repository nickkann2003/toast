using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSetter : MonoBehaviour
{
    public Vector3 targetRotationEuler = new Vector3(0, 90, -90);

    void setRotate(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        if(otherRigidbody != null)
        {
            // Get the transform of the GameObject.
            Transform objectTransform = other.transform;
            //otherRigidbody.angularVelocity = Vector3.zero;
            
            // Convert Euler angles to a Quaternion for rotation.
            Quaternion targetRotation = Quaternion.Euler(targetRotationEuler);

            if(objectTransform.rotation != targetRotation)
            {
                // Calculate the quaternion delta.
                Quaternion deltaRotation = targetRotation * Quaternion.Inverse(objectTransform.rotation);

                // Convert the delta rotation to an angle-axis representation.
                float angle;
                Vector3 axis;
                deltaRotation.ToAngleAxis(out angle, out axis);

                // Ensure the angle is in the range [-180, 180] degrees.
                if (angle > 180f)
                {
                    angle -= 360f;
                }

                // Calculate the angular velocity as the axis of rotation scaled by the angle.
                Vector3 angularVelocity = axis * (angle * Mathf.Deg2Rad);
                angularVelocity = angularVelocity.normalized * 3;

                if(otherRigidbody.angularVelocity != angularVelocity)
                {
                    otherRigidbody.angularVelocity += (angularVelocity - otherRigidbody.angularVelocity)/20f;
                }

                //otherRigidbody.angularVelocity = angularVelocity;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        setRotate(other);
    }

    private void OnTriggerStay(Collider other)
    {
        setRotate(other);
    }
}
