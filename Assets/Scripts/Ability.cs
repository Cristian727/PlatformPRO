using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] string abilityName;
    [SerializeField] protected float coolDown;

    public abstract void Trigger();
}
