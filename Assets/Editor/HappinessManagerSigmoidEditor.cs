using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

/// Els: Modified 04/18/2025
/// This script has been dummied out due to changes in the sigmoid function script.
/// The sigmoid function script is now a non-monobehavior-derived script.
/// It provides construction access to creating new animation curves with no editor
/// access. 

// [CustomEditor(typeof(SigmoidFunction))]
/*
public class HappinessManagerSigmoidEditor : Editor
{
    public VisualTreeAsset visualTree;

    SigmoidFunction sigmoidFunction;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        sigmoidFunction = FindObjectOfType<SigmoidFunction>();

        visualTree.CloneTree(root);

        var curveField = new CurveField("Sigmoid Curve");
        curveField.value = new AnimationCurve(new Keyframe (0, 0, 0, 0),
            new Keyframe(sigmoidFunction.timeFrame, 1, 0, 0));

        curveField.RegisterValueChangedCallback(evt =>
        {
            var component = target as SigmoidFunction;
            component.sigmoidCurve = evt.newValue;
            EditorUtility.SetDirty(component);
        });

        root.Add(curveField);

        return root;
    }


    private void OnValidate()
    {
        //CreateInspectorGUI();
        //UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
    }
}
*/