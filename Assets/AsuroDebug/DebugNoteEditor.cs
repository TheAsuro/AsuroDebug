using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugNote))]
public class DebugNoteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DebugNote note = target as DebugNote;

        note.noteText = EditorGUILayout.TextArea(note.noteText);
    }
}
