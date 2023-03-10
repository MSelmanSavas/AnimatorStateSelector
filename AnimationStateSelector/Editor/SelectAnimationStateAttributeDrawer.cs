using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomPropertyDrawer(typeof(SelectAnimatorStateAttribute))]
public class SelectAnimationStateAttributeDrawer : PropertyDrawer
{
    SelectAnimatorStateAttribute _animationStateAttribute;
    AnimatorController _animatorController;
    Animator _animator;

    SerializedObject _targetObject;
    SerializedProperty _animatorProperty;

    List<string> _animatorStateNames = new List<string>();
    List<AnimatorState> _animatorStates = new List<AnimatorState>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (fieldInfo.DeclaringType.Equals(typeof(AnimatorStateData)) || fieldInfo.DeclaringType.Equals(typeof(List<AnimatorStateData>)))
            OnlyAnimationStateDataDrawer(position, property, label);
        else if (fieldInfo.FieldType.Equals(typeof(string)) || fieldInfo.FieldType.Equals(typeof(List<string>)))
            OnlyStringDrawer(position, property, label);
    }

    bool OnlyStringDrawer(Rect position, SerializedProperty property, GUIContent label)
    {
        _animationStateAttribute = attribute as SelectAnimatorStateAttribute;

        if (_animationStateAttribute is null)
        {
            EditorGUI.PropertyField(position, property);
            return false;
        }

        _targetObject = property.serializedObject;
        _animatorProperty = _targetObject.FindProperty(_animationStateAttribute.AnimatorPropertyName);

        if (_animatorProperty is null || _animatorProperty.objectReferenceValue is null)
        {
            Component component = _targetObject.targetObject as Component;

            if (component != null)
            {
                if (!component.TryGetComponent<Animator>(out _animator))
                {
                    EditorGUI.PropertyField(position, property, label);
                    return false;
                }
            }
        }
        else
        {
            _animator = _animatorProperty.objectReferenceValue as Animator;
        }

        if (_animator.runtimeAnimatorController == null)
        {
            EditorGUI.PropertyField(position, property, label);
            return false;
        }

        _animatorController = _animator.runtimeAnimatorController as AnimatorController;

        _animatorStateNames.Clear();
        _animatorStates.Clear();

        foreach (var layer in _animatorController.layers)
        {
            foreach (var childAnimatorState in layer.stateMachine.states)
            {
                AnimatorState animState = childAnimatorState.state;
                _animatorStates.Add(animState);
                _animatorStateNames.Add(animState.name);
            }
        }

        int indexOfCurrentSelection = _animatorStateNames.IndexOf(property.stringValue);

        if (indexOfCurrentSelection == -1)
        {
            property.stringValue = _animatorStateNames[0];
            _targetObject.ApplyModifiedProperties();
        }

        int selectedIndex = EditorGUI.Popup(position, label.text, indexOfCurrentSelection, _animatorStateNames.ToArray());

        if (indexOfCurrentSelection != selectedIndex)
        {
            property.stringValue = _animatorStateNames[selectedIndex];
            _targetObject.ApplyModifiedProperties();
        }

        return true;
    }

    bool OnlyAnimationStateDataDrawer(Rect position, SerializedProperty property, GUIContent label)
    {
        _animationStateAttribute = attribute as SelectAnimatorStateAttribute;

        if (_animationStateAttribute is null)
        {
            EditorGUI.PropertyField(position, property);
            return false;
        }

        _targetObject = property.serializedObject;

        SerializedProperty animationStateDataSP = _targetObject.FindProperty(property.propertyPath.Substring(0, property.propertyPath.LastIndexOf('.')));

        _animatorProperty = animationStateDataSP.FindPropertyRelative(nameof(AnimatorStateData.SelectedAnimator));

        if (_animatorProperty is null || _animatorProperty.objectReferenceValue is null)
        {
            Component component = _targetObject.targetObject as Component;

            if (component != null)
            {
                if (!component.TryGetComponent<Animator>(out _animator))
                {
                    EditorGUI.PropertyField(position, property, label);
                    return false;
                }
            }
        }
        else
        {
            _animator = _animatorProperty.objectReferenceValue as Animator;
        }

        if (_animator.runtimeAnimatorController == null)
        {
            EditorGUI.PropertyField(position, property, label);
            return false;
        }

        _animatorController = _animator.runtimeAnimatorController as AnimatorController;
        
        _animatorStateNames.Clear();
        _animatorStates.Clear();

        foreach (var layer in _animatorController.layers)
        {
            foreach (var childAnimatorState in layer.stateMachine.states)
            {
                AnimatorState animState = childAnimatorState.state;
                _animatorStates.Add(animState);
                _animatorStateNames.Add(animState.name);
            }
        }

        int indexOfCurrentSelection = _animatorStateNames.IndexOf(property.stringValue);

        if (indexOfCurrentSelection == -1)
        {
            property.stringValue = _animatorStateNames[0];
            SetAnimationStateDatas(_animatorStates[0]);
            _targetObject.ApplyModifiedProperties();
        }

        int selectedIndex = EditorGUI.Popup(position, label.text, indexOfCurrentSelection, _animatorStateNames.ToArray());

        if (indexOfCurrentSelection != selectedIndex)
        {
            property.stringValue = _animatorStateNames[selectedIndex];
            SetAnimationStateDatas(_animatorStates[selectedIndex]);
            _targetObject.ApplyModifiedProperties();
        }

        return true;

        void SetAnimationStateDatas(UnityEditor.Animations.AnimatorState animatorState)
        {
            animationStateDataSP.FindPropertyRelative(nameof(AnimatorStateData.Name)).stringValue = property.stringValue;
            animationStateDataSP.FindPropertyRelative(nameof(AnimatorStateData.Hash)).intValue = Animator.StringToHash(property.stringValue);
            animationStateDataSP.FindPropertyRelative(nameof(AnimatorStateData.Clip)).objectReferenceValue = animatorState.motion;
        }
    }
}
