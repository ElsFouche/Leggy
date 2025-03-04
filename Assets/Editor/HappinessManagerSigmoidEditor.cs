using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(SigmoidFunction))]
public class HappinessManagerSigmoidEditor : Editor
{
    public VisualTreeAsset visualTree;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        visualTree.CloneTree(root);

        var curveField = new CurveField("Sigmoid Curve");
        //curveField.value = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(0.5f, 0.5f, 1, 1),
         //   new Keyframe(1, 1, 0, 0));

        curveField.RegisterValueChangedCallback(evt =>
        {
            var component = target as SigmoidFunction;
            component.sigmoidCurve = evt.newValue;
            EditorUtility.SetDirty(component);
        });

        root.Add(curveField);

        return root;
    }
}
