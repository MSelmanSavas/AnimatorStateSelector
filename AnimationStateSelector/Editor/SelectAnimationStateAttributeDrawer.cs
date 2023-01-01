using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomPropertyDrawer(typeof(SelectAnimationStateAttribute))]
public class SelectAnimationStateAttributeDrawer : PropertyDrawer
{
    bool isInitialized = false;
    SelectAnimationStateAttribute _animationStateAttribute;
    AnimatorController _animatorController;
    Animator _animator;


    SerializedObject _targetObject;
    SerializedProperty _animatorProperty;

    List<string> _animatorStateNames = new List<string>();
    List<AnimatorState> _animatorStates = new List<AnimatorState>();

    Dictionary<System.Type, System.Func<Rect, SerializedProperty, GUIContent, bool>> _guiDrawersByTypes = new();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!isInitialized)
            Initialize();

        if (fieldInfo.DeclaringType.Equals(typeof(AnimationStateData)) || fieldInfo.DeclaringType.Equals(typeof(List<AnimationStateData>)))
            OnlyAnimationStateDataDrawer(position, property, label);
        else if (fieldInfo.FieldType.Equals(typeof(string)) || fieldInfo.FieldType.Equals(typeof(List<string>)))
            OnlyStringDrawer(position, property, label);
    }

    void Initialize()
    {
        _guiDrawersByTypes.Clear();

        _guiDrawersByTypes.Add(typeof(string), OnlyStringDrawer);
        _guiDrawersByTypes.Add(typeof(List<string>), OnlyStringDrawer);
        _guiDrawersByTypes.Add(typeof(AnimationStateData), OnlyAnimationStateDataDrawer);
        _guiDrawersByTypes.Add(typeof(List<AnimationStateData>), OnlyAnimationStateDataDrawer);

        isInitialized = true;
    }

    bool OnlyStringDrawer(Rect position, SerializedProperty property, GUIContent label)
    {
        _animationStateAttribute = attribute as SelectAnimationStateAttribute;

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
                if (!component.TryGetComponent<Animator>(out _animator))
                {
                    EditorGUI.PropertyField(position, property);
                    return false;
                }

            _animatorController = _animator.runtimeAnimatorController as AnimatorController;
        }
        else
        {
            _animator = _animatorProperty.objectReferenceValue as Animator;
            _animatorController = _animator.runtimeAnimatorController as AnimatorController;
        }

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
        _animationStateAttribute = attribute as SelectAnimationStateAttribute;

        if (_animationStateAttribute is null)
        {
            EditorGUI.PropertyField(position, property);
            return false;
        }

        _targetObject = property.serializedObject;

        SerializedProperty animationStateDataSP = _targetObject.FindProperty(property.propertyPath.Substring(0, property.propertyPath.LastIndexOf('.')));

        _animatorProperty = animationStateDataSP.FindPropertyRelative(nameof(AnimationStateData.SelectedAnimator));

        if (_animatorProperty is null || _animatorProperty.objectReferenceValue is null)
        {
            Component component = _targetObject.targetObject as Component;

            if (component != null)
                if (!component.TryGetComponent<Animator>(out _animator))
                {
                    EditorGUI.PropertyField(position, property);
                    return false;
                }

            _animatorController = _animator.runtimeAnimatorController as AnimatorController;
        }
        else
        {
            _animator = _animatorProperty.objectReferenceValue as Animator;
            _animatorController = _animator.runtimeAnimatorController as AnimatorController;
        }

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
            animationStateDataSP.FindPropertyRelative(nameof(AnimationStateData.Name)).stringValue = property.stringValue;
            animationStateDataSP.FindPropertyRelative(nameof(AnimationStateData.Hash)).intValue = Animator.StringToHash(property.stringValue);
            animationStateDataSP.FindPropertyRelative(nameof(AnimationStateData.Clip)).objectReferenceValue = animatorState.motion;
        }
    }
}
