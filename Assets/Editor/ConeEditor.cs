using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (Viewcone))] 
public class ConeEditor : Editor {
        [MenuItem ("GameObject/Create Other/Cone")]
        static void Create(){
                GameObject gameObject = new GameObject("Viewcone");
                Viewcone s = gameObject.AddComponent<Viewcone>();
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                meshFilter.mesh = new Mesh();
                s.Rebuild();
        }
        
        public override void OnInspectorGUI ()
        {
                Viewcone obj;

                obj = target as Viewcone;

                if (obj == null)
                {
                        return;
                }
        
                base.DrawDefaultInspector();
                EditorGUILayout.BeginHorizontal ();
                
                // Rebuild mesh when user click the Rebuild button
                if (GUILayout.Button("Rebuild")){
                        obj.Rebuild();
                }
                EditorGUILayout.EndHorizontal ();
        }
}