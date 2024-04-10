#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

    [CustomEditor(typeof(object), editorForChildClasses: true)]
    public class ButtonAttributeEditor : UnityEditor.Editor
    {
        private const int LabelFontSize = 24;
        private const float ButtonPadding = 100f;
        private const int ButtonFontSize = 16;
        private const int HeaderSpacing = 10;
        private const int ButtonSpacing = 10;

        private bool isMethodListOpen = true; // Added toggle for the method list

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (object)target;

            var methods = GetButtonMethods(script);

            if (!methods.Any())
            {
                return;
            }

            SetGUIStyles();

            EditorGUILayout.Space();

            // Main Box
            GUILayout.BeginVertical("box");
            DrawHeader(script);
            DrawToggleButton();

            if (isMethodListOpen)
            {
                DrawMethodList(methods, script);
            }

            GUILayout.EndVertical(); // End of main box

            ResetGUIStyles();
        }

        private IEnumerable<MethodInfo> GetButtonMethods(object script)
        {
            return script.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(method => method.GetCustomAttributes<ButtonAttribute>().Any());
        }

        private void SetGUIStyles()
        {
            GUI.color = Color.white;
            GUI.backgroundColor = Color.cyan;
            GUI.contentColor = Color.white;
        }

        private void ResetGUIStyles()
        {
            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;
            GUI.contentColor = Color.white;
        }

        private void DrawHeader(object script)
        {
            GUILayout.BeginVertical("box");

            string labelName = script.GetType().Name;
            string splitLabelWords = Regex.Replace(labelName, @"(?<!^)(?=[A-Z])", " ");
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = LabelFontSize,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label(splitLabelWords, labelStyle);

            GUILayout.EndVertical();
        }

        private void DrawToggleButton()
        {
            string toggleButtonText = isMethodListOpen ? "Close" : "Open";
            GUIStyle buttonStyle = new GUIStyle(EditorStyles.toolbarButton)
            {
                fontSize = ButtonFontSize,
                fontStyle = FontStyle.Bold
            };

            if (GUILayout.Button(toggleButtonText, buttonStyle))
            {
                isMethodListOpen = !isMethodListOpen;
            }
        }

        private void DrawMethodList(IEnumerable<MethodInfo> methods, object script)
        {
            GUILayout.Space(HeaderSpacing);
            foreach (var method in methods)
            {
                DrawButton(method, script);
                GUILayout.Space(ButtonSpacing); // Add space between buttons
            }
        }

        private void DrawButton(MethodInfo method, object script)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            string buttonName = method.Name;
            string splitButtonName = Regex.Replace(buttonName, @"(?<!^)(?=[A-Z])", " ");

            float buttonWidth = EditorGUIUtility.currentViewWidth - ButtonPadding;
            GUIStyle buttonStyle = new GUIStyle(EditorStyles.toolbarButton)
            {
                fontSize = ButtonFontSize,
                fontStyle = FontStyle.Bold,
                fixedWidth = buttonWidth
            };

            if (GUILayout.Button(splitButtonName, buttonStyle))
            {
                method.Invoke(script, null);
                EditorUtility.SetDirty(target);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

#endif
