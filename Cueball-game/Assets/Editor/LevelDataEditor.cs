#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    private ReorderableList ballSlotsList;
    private const float additionalHeight = 20f;

    private void OnEnable()
    {
        ballSlotsList = new ReorderableList(serializedObject, serializedObject.FindProperty("ballSlots"), true, true, true, true);

        ballSlotsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = ballSlotsList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), $"Ball Slot {index + 1}");

            var isLockedProp = element.FindPropertyRelative("isLocked");
            var ballHoldersProp = element.FindPropertyRelative("ballHolders");
            rect.y += EditorGUIUtility.singleLineHeight;

            // Display lock/unlock button
            if (GUI.Button(new Rect(rect.x, rect.y, 80, EditorGUIUtility.singleLineHeight), isLockedProp.boolValue ? "Unlock" : "Lock"))
            {
                isLockedProp.boolValue = !isLockedProp.boolValue;
            }

            rect.y += EditorGUIUtility.singleLineHeight + 2;

            // If the slot is unlocked, show the ball holders
            if (!isLockedProp.boolValue)
            {
                for (int i = 0; i < ballHoldersProp.arraySize; i++)
                {
                    var ballHolderProp = ballHoldersProp.GetArrayElementAtIndex(i);
                    var colorEnumProp = ballHolderProp.FindPropertyRelative("selectedColorEnum");
                    var colorProp = ballHolderProp.FindPropertyRelative("selectedColor");
                    EditorGUI.PrefixLabel(new Rect(rect.x + 90, rect.y, 80, EditorGUIUtility.singleLineHeight), new GUIContent($"Ball {i + 1}"));
                    BallHolder.BallColor selectedColorEnum = (BallHolder.BallColor)colorEnumProp.enumValueIndex;
                    Color selectedColor = GetColorForEnum(selectedColorEnum);
                    EditorGUI.ColorField(new Rect(rect.x + 170, rect.y, 40, EditorGUIUtility.singleLineHeight), GUIContent.none, selectedColor, false, false, false);
                    colorEnumProp.enumValueIndex = (int)(BallHolder.BallColor)EditorGUI.EnumPopup(new Rect(rect.x + 220, rect.y, 100, EditorGUIUtility.singleLineHeight), GUIContent.none, selectedColorEnum);
                    colorProp.colorValue = selectedColor;
                    rect.y += EditorGUIUtility.singleLineHeight + 2;
                }

                if (ballHoldersProp.arraySize < 4 && GUI.Button(new Rect(rect.x, rect.y, 80, EditorGUIUtility.singleLineHeight), "Add Ball"))
                {
                    ballHoldersProp.arraySize++;
                }

                if (ballHoldersProp.arraySize > 0 && GUI.Button(new Rect(rect.x + 90, rect.y, 80, EditorGUIUtility.singleLineHeight), "Remove Ball"))
                {
                    ballHoldersProp.arraySize--;
                }

                rect.y += EditorGUIUtility.singleLineHeight + additionalHeight;
            }
            else
            {
                rect.y += additionalHeight;
            }
        };

        ballSlotsList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Ball Slots");
        };

        ballSlotsList.elementHeightCallback = (index) =>
        {
            var element = ballSlotsList.serializedProperty.GetArrayElementAtIndex(index);

            var isLockedProp = element.FindPropertyRelative("isLocked");
            var ballHoldersProp = element.FindPropertyRelative("ballHolders");

            var elementHeight = EditorGUIUtility.singleLineHeight * (isLockedProp.boolValue ? 1 : ballHoldersProp.arraySize + 3) + additionalHeight;
            return elementHeight;
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ballSlotsList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private Color GetColorForEnum(BallHolder.BallColor colorEnum)
    {
        switch (colorEnum)
        {
            case BallHolder.BallColor.Cyan:
                return new Color(0, 1, 1);
            case BallHolder.BallColor.SkyBlue:
                return new Color(0.53f, 0.81f, 0.92f);
            case BallHolder.BallColor.Green:
                return new Color(0.15f, 0.95f, 0.55f);
            case BallHolder.BallColor.Black:
                return new Color(0.2f, 0.23f, 0.25f);
            case BallHolder.BallColor.LightPink:
                return new Color(1, 0.3f, 0.43f);
            case BallHolder.BallColor.Red:
                return new Color(0.64f, 0.08f, 0.24f);
            case BallHolder.BallColor.Orange:
                return new Color(0.83f, 0.44f, 0.12f);
            case BallHolder.BallColor.Yellow:
                return new Color(0.93f, 0.71f, 0.16f);
            case BallHolder.BallColor.Purple:
                return new Color(0.41f, 0.41f, 1f);
            default:
                return Color.white;
        }
    }
}
#endif

public class BallSlot
{
    public bool isLocked;
    public BallHolder[] ballHolders = new BallHolder[4];
}
