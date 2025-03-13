using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] Vector3 offset;
    Vector3 currentVelocity = Vector3.zero;

    [SerializeField] float minFOV;
    [SerializeField] float maxFOV;
    [SerializeField] float minZoomableSpeed;
    [SerializeField] float maxZoomableSpeed;

    float zoomVelocity = 0;
    [SerializeField] float zoomSmoothTime = 0.1f;
    [SerializeField] float lookAheadDistance;
    [SerializeField] float lookAheadSmooth;
    Vector3 lookAhead;
    Vector3 lookAheadVelocity = Vector3.zero;

    private Rigidbody2D rb;
    float playerSpeed;
    float normalizedSpeed;
    float denormalizedSpeed;

    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
        rb = targetTransform.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lookAhead = Vector3.SmoothDamp(
            lookAhead,
            rb.velocity.normalized * lookAheadDistance,
            ref lookAheadVelocity,
            lookAheadSmooth
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetTransform.position + offset + lookAhead,
            ref currentVelocity,
            smoothTime
        );


        //mainCamera.fieldOfView
        //rb.velocity.magnitude es  Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y)
        playerSpeed = rb.velocity.magnitude;


        if (playerSpeed < minZoomableSpeed)
        {
            normalizedSpeed = 0;
        }
        else if (playerSpeed > maxZoomableSpeed)
        {
            normalizedSpeed = 1;
        }
        else
            normalizedSpeed = (playerSpeed - minZoomableSpeed) / (maxZoomableSpeed - minZoomableSpeed);


        denormalizedSpeed = (maxFOV - minFOV) * normalizedSpeed + minFOV;


        mainCamera.fieldOfView = Mathf.SmoothDamp(
            mainCamera.fieldOfView,
            denormalizedSpeed,
            ref zoomVelocity,
            zoomSmoothTime
        );

        //print(playerSpeed + " -> " + normalizedSpeed + " -> " + denormalizedSpeed + " -> " + mainCamera.fieldOfView);
    }
}
