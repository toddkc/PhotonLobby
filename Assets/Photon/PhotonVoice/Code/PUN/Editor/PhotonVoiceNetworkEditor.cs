namespace Photon.Voice.PUN.Editor
{
    using Unity.Editor;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(PhotonVoiceNetwork))]
    public class PhotonVoiceNetworkEditor : VoiceConnectionEditor
    {
        private SerializedProperty autoConnectAndJoinSp;
        private SerializedProperty autoLeaveAndDisconnectSp;
        private SerializedProperty usePunAppSettingsSp;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.autoConnectAndJoinSp = this.serializedObject.FindProperty("AutoConnectAndJoin");
            this.autoLeaveAndDisconnectSp = this.serializedObject.FindProperty("AutoLeaveAndDisconnect");
            this.usePunAppSettingsSp = this.serializedObject.FindProperty("usePunAppSettings");
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(this.autoConnectAndJoinSp, new GUIContent("Auto Connect And Join", "Auto connect voice client and join a voice room when PUN client is joined to a PUN room"));
            EditorGUILayout.PropertyField(this.autoLeaveAndDisconnectSp, new GUIContent("Auto Leave And Disconnect", "Auto disconnect voice client when PUN client is not joined to a PUN room"));
            if (EditorGUI.EndChangeCheck())
            {
                this.serializedObject.ApplyModifiedProperties();
            }
            base.OnInspectorGUI();
        }

        protected override void DisplayAppSettings()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(this.usePunAppSettingsSp, new GUIContent("Use PUN's App Settings", "Use App Settings From PUN's PhotonServerSettings"));
            if (GUILayout.Button("PhotonServerSettings", EditorStyles.miniButton, GUILayout.Width(120)))
            {
                Selection.objects = new Object[] { Pun.PhotonNetwork.PhotonServerSettings };
                EditorGUIUtility.PingObject(Pun.PhotonNetwork.PhotonServerSettings);
            }
            EditorGUILayout.EndHorizontal();
            if (!this.usePunAppSettingsSp.boolValue)
            {
                base.DisplayAppSettings();
            }
        }
    }
}