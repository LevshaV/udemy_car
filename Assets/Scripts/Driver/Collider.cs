using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : Capsule
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.Booster)
        {
            // moveSpeed = GetActualSpeed(moveSpeedFast);
            moveSpeedModifier = accelerationValue;
            changeSpeedStartTime = Time.time;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == Tags.Slower)
        {
            // moveSpeed = GetActualSpeed(moveSpeedLow);
            moveSpeedModifier = decelerationValue;
            changeSpeedStartTime = Time.time;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        OnCollisionEnter2D(other);
    }

    // float GetActualSpeed(float currentBase)
    // {
    //     return Input.GetKey(KeyCode.LeftShift) ? currentBase * n2oAccelerationValue : currentBase;
    // }
}
