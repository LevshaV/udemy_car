using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Capsule : MonoBehaviour
{
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";

    private float _moveAmount;
    private Rigidbody2D rigidBody;

    [SerializeField] float steerSpeed = 100f;

    [SerializeField] float moveSpeedBase = 3.6f;
    [SerializeField] internal float decelerationValue = 0.5f, accelerationValue = 2f;
    [SerializeField] float moveSpeed, moveSpeedLow, moveSpeedFast;
    [SerializeField] internal float moveSpeedModifier;
    [SerializeField] float n2oAmount = 100, n2oDrainRate = 20, n2oRecoverRate = 5, n2oAccelerationBaseValue = 1.5f, n2oAccelerationValue;
    bool n2oDepleted, n2oActive;
    float n2oDepletedStartTime, n2oDepletedDuration = 5f;

    [SerializeField] internal float changeSpeedStartTime = 0f;
    [SerializeField] float accelerationDuration = 2f;
    [SerializeField] float decelerationDuration = 0.5f;

    public float MoveAmount
    {
        get
        {
            return _moveAmount;
        }
        private set
        {
            _moveAmount = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = moveSpeedBase;
        moveSpeedLow = moveSpeedBase * decelerationValue;
        moveSpeedFast = moveSpeedBase * accelerationValue;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var steerAmount = -(Input.GetAxis(HORIZONTAL_AXIS) * steerSpeed * Time.deltaTime);
        MoveAmount = Input.GetAxis(VERTICAL_AXIS) * moveSpeed * Time.deltaTime;

        // if (verticalSpeed >= 0)
        // {
        //     steerAmount = -steerAmount;
        // }

        transform.Translate(0, MoveAmount, 0);
        transform.Rotate(0, 0, steerAmount * (MoveAmount == 0 ? 0 : 1));

        CheckAcceleration();
        ResetPosition();
    }

    void CheckAcceleration()
    {
        // var accelerationEnded = moveSpeed >= moveSpeedFast && Time.time - changeSpeedStartTime >= accelerationDuration;
        // var decelerationEnded = moveSpeed < baseMoveSpeed && Time.time - changeSpeedStartTime >= decelerationDuration;
        // if (accelerationEnded || decelerationEnded)
        // {
        //     moveSpeed = baseMoveSpeed;
        // }

        // var n2oAccelerated = moveSpeed != baseMoveSpeed && moveSpeed != moveSpeedLow && moveSpeed != moveSpeedFast;
        // if (n2oAccelerated)
        // {
        //     n2oAmount -= n2oDrainRate * Time.deltaTime;
        //     if (!Input.GetKey(KeyCode.LeftShift) || n2oAmount <= 0)
        //         // moveSpeed = moveSpeedFast;
        //         moveSpeed /= n2oValue;
        // }

        // if (!n2oAccelerated)
        // {
        //     if (Input.GetKey(KeyCode.LeftShift) && n2oAmount > 0)
        //         moveSpeed *= n2oValue;
        //     else
        //         n2oAmount += n2oRecoverRate * Time.deltaTime;

        //     if (n2oAmount > 100)
        //         n2oAmount = 100;
        // }

        var n2oDrainedAmount = n2oDrainRate * Time.deltaTime;
        var n2oAccelerationAvailable = Input.GetKey(KeyCode.LeftShift) &&
                n2oAmount - n2oDrainedAmount > 0 &&
                !n2oDepleted;

        if (n2oAccelerationAvailable)
        {
            n2oAccelerationValue = n2oAccelerationBaseValue;
            n2oAmount -= n2oDrainRate * Time.deltaTime;
        }
        else
        {
            n2oAccelerationValue = 1f;
            n2oAmount += n2oRecoverRate * Time.deltaTime;
            if (n2oAmount > 100)
                n2oAmount = 100;
        }

        if (n2oAmount - n2oDrainedAmount <= 0 && !n2oDepleted)
        {
            n2oDepleted = true;
            n2oDepletedStartTime = Time.time;
        }

        if (n2oDepleted && Time.time - n2oDepletedStartTime >= n2oDepletedDuration)
            n2oDepleted = false;

        var accelerationEnded = moveSpeed >= moveSpeedFast && Time.time - changeSpeedStartTime >= accelerationDuration;
        var decelerationEnded = moveSpeed < moveSpeedBase && Time.time - changeSpeedStartTime >= decelerationDuration;
        if (accelerationEnded || decelerationEnded)
            moveSpeedModifier = 1f;

        moveSpeed = moveSpeedBase * (moveSpeedModifier * n2oAccelerationValue);
    }

    void ResetPosition()
    {
        if (Input.GetKey(KeyCode.R))
        {
            // transform.position = new Vector3(5.71f, -0.17f);
            SceneManager.LoadScene(0);
        }
    }
}
