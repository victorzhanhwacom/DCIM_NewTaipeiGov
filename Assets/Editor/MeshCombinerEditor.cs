using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace VzDev.EditorUtils
{
    public class MeshCombinerEditor : EditorWindow
    {
        [MenuItem("VzDev Tools/BIM Pipelines/Force Mesh Combiner")]
        public static void ShowWindow()
        {
            GetWindow<MeshCombinerEditor>("Mesh Combiner");
        }

        private void OnGUI()
        {
            Rect contentRect = EditorGUILayout.BeginVertical();

            GUILayout.Label("Force Combine Selected Meshes By Material", EditorStyles.boldLabel);
            GUILayout.Label("BIM/Revit 最佳化版本：支援多選、修正鏡像反轉與多材質缺塊", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (GUILayout.Button("Combine Selected Parent's Children", GUILayout.Height(30)))
            {
                CombineSelected();
            }

            EditorGUILayout.EndVertical();

            if (Event.current.type == EventType.Repaint)
            {
                float fixedWidth = 480f; 
                float paddingHeight = 25f;
                float targetHeight = contentRect.height + paddingHeight;

                Vector2 calculatedSize = new Vector2(fixedWidth, targetHeight);
                
                if (this.minSize != calculatedSize)
                {
                    this.minSize = calculatedSize;
                    this.maxSize = calculatedSize;
                }
            }
        }

        private void CombineSelected()
        {
            GameObject[] selectedGroups = Selection.gameObjects;
            
            if (selectedGroups == null || selectedGroups.Length == 0)
            {
                EditorUtility.DisplayDialog("Error", "Please select at least one parent GameObject in Hierarchy.", "OK");
                return;
            }

            int processedCount = 0;

            foreach (GameObject selectedGroup in selectedGroups)
            {
                // Key: Material, Value: List of (MeshFilter, SubMeshIndex) to support Multi-Material objects
                Dictionary<Material, List<(MeshFilter filter, int subMeshIndex)>> materialToMeshMap = 
                    new Dictionary<Material, List<(MeshFilter, int)>>();
                
                MeshRenderer[] renderers = selectedGroup.GetComponentsInChildren<MeshRenderer>();
                if (renderers.Length == 0) continue;

                // Step 1: Deep scan all renderers and their materials (including multi-materials)
                foreach (var renderer in renderers)
                {
                    MeshFilter filter = renderer.GetComponent<MeshFilter>();
                    if (filter == null || filter.sharedMesh == null) continue;

                    Material[] sharedMaterials = renderer.sharedMaterials;
                    int subMeshCount = filter.sharedMesh.subMeshCount;

                    // Map each submesh to its corresponding material
                    for (int i = 0; i < subMeshCount; i++)
                    {
                        if (i >= sharedMaterials.Length) continue; // Safety check
                        
                        Material mat = sharedMaterials[i];
                        if (mat == null) continue;

                        if (!materialToMeshMap.ContainsKey(mat))
                        {
                            materialToMeshMap[mat] = new List<(MeshFilter, int)>();
                        }

                        materialToMeshMap[mat].Add((filter, i));
                    }
                }

                if (materialToMeshMap.Count == 0) continue;

                // Step 2: Combine process
                GameObject combinedRoot = new GameObject($"{selectedGroup.name}_CombinedRoot");
                combinedRoot.transform.position = selectedGroup.transform.position;
                combinedRoot.transform.rotation = selectedGroup.transform.rotation;
                combinedRoot.transform.localScale = selectedGroup.transform.localScale;

                foreach (var pair in materialToMeshMap)
                {
                    Material mat = pair.Key;
                    var meshDataList = pair.Value;

                    List<CombineInstance> combineInstances = new List<CombineInstance>();
                    List<Mesh> temporaryMirroredMeshes = new List<Mesh>(); // To track cloned meshes for clean up

                    foreach (var data in meshDataList)
                    {
                        CombineInstance combine = new CombineInstance();
                        
                        // Calculate final matrix relative to our new root
                        Matrix4x4 relativeMatrix = combinedRoot.transform.worldToLocalMatrix * data.filter.transform.localToWorldMatrix;
                        combine.transform = relativeMatrix;
                        combine.subMeshIndex = data.subMeshIndex;

                        // FIX: Revit Mirror/Negative Scale Detection
                        // If the matrix determinant is negative, the winding order will be reversed (invisible mesh)
                        if (relativeMatrix.determinant < 0)
                        {
                            // Clone the mesh topology to flip triangles safely without affecting original FBX asset
                            Mesh invertedMesh = Instantiate(data.filter.sharedMesh);
                            InvertMeshTriangles(invertedMesh, data.subMeshIndex);
                            temporaryMirroredMeshes.Add(invertedMesh);
                            
                            combine.mesh = invertedMesh;
                        }
                        else
                        {
                            combine.mesh = data.filter.sharedMesh;
                        }

                        combineInstances.Add(combine);

                        // Safely disable original renderer
                        var r = data.filter.GetComponent<MeshRenderer>();
                        //if (r != null) r.enabled = false;
                    }

                    // Create new merged GameObject
                    GameObject combinedObj = new GameObject($"Combined_{mat.name}");
                    combinedObj.transform.SetParent(combinedRoot.transform, false);

                    MeshFilter newFilter = combinedObj.AddComponent<MeshFilter>();
                    MeshRenderer newRenderer = combinedObj.AddComponent<MeshRenderer>();

                    Mesh newMesh = new Mesh();
                    newMesh.indexFormat = IndexFormat.UInt32;
                    
                    // Combine into a single submesh (true) since they all share the exact same material
                    newMesh.CombineMeshes(combineInstances.ToArray(), true, true);

                    newFilter.sharedMesh = newMesh;
                    newRenderer.sharedMaterial = mat;

                    GameObjectUtility.SetStaticEditorFlags(combinedObj, StaticEditorFlags.ContributeGI | StaticEditorFlags.BatchingStatic);

                    // Clean up temporary instantiated mirrored meshes from memory immediately
                    foreach (var tmpMesh in temporaryMirroredMeshes)
                    {
                        DestroyImmediate(tmpMesh);
                    }
                }

                Undo.RegisterCreatedObjectUndo(combinedRoot, "Combine Meshes Revit Optimized");
                processedCount++;
            }

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Success", $"Successfully processed {processedCount} Revit model roots.", "OK");
        }

        /// <summary>
        /// Inverts the winding order of triangles for a specific submesh to fix flipped normals.
        /// </summary>
        private void InvertMeshTriangles(Mesh mesh, int subMeshIndex)
        {
            int[] triangles = mesh.GetTriangles(subMeshIndex);
            
            // Reverse winding order (swap vertex 0 and 1 for every triangle triplet)
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i];
                triangles[i] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
            
            mesh.SetTriangles(triangles, subMeshIndex);
            
            // Recalculate bounds and normals to apply changes properly
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }
}