using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[Serializable]
public class PositionalMatrix
{
    public bool up_left;
    public bool up_mid;
    public bool up_right;
    public bool mid_left;
    public bool mid_right;
    public bool dw_left;
    public bool dw_mid;
    public bool dw_right;
    public PositionalMatrix(){}
    public bool[] Get8DMask()
    {
        bool[] mask = { false, true, false, true, true, false, true, false };

        if (up_mid)
        {
            if (mid_left) mask[0] = true;
            if (mid_right) mask[2] = true;
        }
        if (dw_mid)
        {
            if (mid_left) mask[5] = true;
            if (mid_right) mask[7] = true;
        }

        return mask;
    }
    public bool[] GetAsArray()
    {
        bool[] b = new bool[8];

        b[0] = up_left;
        b[1] = up_mid;
        b[2] = up_right;
        b[3] = mid_left;
        b[4] = mid_right;
        b[5] = dw_left;
        b[6] = dw_mid;
        b[7] = dw_right;

        return b;
    }
    public bool IsEqualTo(bool[] ask)
    {
        bool[] array = this.GetAsArray();
        bool[] mask = this.Get8DMask();
        for (int i = 0; i < 8; i++)
        {
            //^ means return true if equal
            if (mask[i] && (array[i] ^ ask[i]))
            {
                return false;
            }
        }
        return true;
    }
}


[CustomPropertyDrawer(typeof(PositionalMatrix))]
public class PositionalMatrixDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);
        contentPosition.width *= 0.33f;
        contentPosition.height *= 0.3f;
        EditorGUI.indentLevel = 0;

        Rect originalContentPos = contentPosition;

        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("up_left"), GUIContent.none);
        contentPosition.y += contentPosition.height;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("mid_left"), GUIContent.none);
        contentPosition.y += contentPosition.height;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("dw_left"), GUIContent.none);

        contentPosition.y = originalContentPos.y;
        contentPosition.x += contentPosition.width;

        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("up_mid"), GUIContent.none);
        contentPosition.y += contentPosition.height;
        EditorGUI.PrefixLabel(contentPosition, new GUIContent("X"));
        contentPosition.y += contentPosition.height;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("dw_mid"), GUIContent.none);

        contentPosition.y = originalContentPos.y;
        contentPosition.x += contentPosition.width;

        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("up_right"), GUIContent.none);
        contentPosition.y += contentPosition.height;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("mid_right"), GUIContent.none);
        contentPosition.y += contentPosition.height;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("dw_right"), GUIContent.none);



        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 100f;
    }
}