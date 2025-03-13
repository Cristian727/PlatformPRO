using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Stats : ScriptableObject
{
    [Header("Ground")]
    public float groundAcceleration;
    public float groundFriction;
    public float groundMaxSpeedX;
    [Header("Air")]
    public float airAcceleration;
    public float airFriction;
    public float airMaxSpeedX;
    public float maxFallSpeed;
    [Header("Jump")]
    public float jumpStrength;
    public float maxJumpPressTime;
    public int onAirJumps;
    [Header("Gravity")]
    public float upGravity;
    public float downGravity;
    public float yVelocityLowGravityThreshold;
    public float peakGravity;
}
