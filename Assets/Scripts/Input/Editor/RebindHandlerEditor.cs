using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Input.Editor
{
    [CustomEditor(typeof(RebindHandler))]
    public class RebindHandlerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            RebindHandler self = (RebindHandler)target;
            GUILayout.Space(30);
            if (GUILayout.Button("Trigger Rebind"))
            {
                Debug.Log("Rebinding");
                self.TriggerRebind();
            }
            if (GUILayout.Button("Trigger Remove Override"))
            {
                Debug.Log("Unbinding");
                self.ClearIndividualBinding();
            }
            if (GUILayout.Button("Remove All Overrides"))
            {
                Debug.Log("Clearing Overrides");
                self.ResetAllBindingOverrides();
            }
        }
    }
}