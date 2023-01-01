using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
public class SelectAnimatorStateAttribute : PropertyAttribute
{
    public string AnimatorPropertyName;

    public SelectAnimatorStateAttribute() { }

    public SelectAnimatorStateAttribute(string animatorPropertyName)
    {
        this.AnimatorPropertyName = animatorPropertyName;
    }
}
