using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Viewcone))] 
public class ConeEditor : Editor {
        [MenuItem ("GameObject/Create Other/Cone")]
        private static void Create(){
                var gameObject = new GameObject("Viewcone");
                var s = gameObject.AddComponent<Viewcone>();
                var meshFilter = gameObject.GetComponent<MeshFilter>();
                meshFilter.mesh = new Mesh();
                s.Rebuild();
        }
        
        public override void OnInspectorGUI ()
        {
                var obj = target as Viewcone;

                if (obj == null)
                {
                        return;
                }
        
                DrawDefaultInspector();
                EditorGUILayout.BeginHorizontal ();
                
                // Rebuild mesh when user click the Rebuild button
                if (GUILayout.Button("Rebuild")){
                        obj.Rebuild();
                }
                EditorGUILayout.EndHorizontal ();
        }
}