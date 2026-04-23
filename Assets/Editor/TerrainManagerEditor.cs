using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(8f);
        EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);

        var manager = (TerrainManager)target;

        if (GUILayout.Button("Diffuse 머티리얼 적용"))
            ApplyDiffuseMaterial(manager);

        if (GUILayout.Button("지형 텍스처 페인팅"))
        {
            manager.PaintTerrain();
            EditorUtility.SetDirty(manager.GetComponent<Terrain>());
            AssetDatabase.SaveAssets();
            Debug.Log("[TerrainManager] Terrain painted.");
        }
    }

    private static void ApplyDiffuseMaterial(TerrainManager manager)
    {
        var shader = Shader.Find("Nature/Terrain/Diffuse");
        if (shader == null)
        {
            Debug.LogError("[TerrainManager] Shader 'Nature/Terrain/Diffuse' not found.");
            return;
        }

        const string matPath = "Assets/Resources/Terrain/TerrainDiffuse.mat";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        if (mat == null)
        {
            mat = new Material(shader) { name = "TerrainDiffuse" };
            AssetDatabase.CreateAsset(mat, matPath);
        }

        manager.ApplyDiffuseMaterial(mat);

        var terrain = manager.GetComponent<Terrain>();
        foreach (var layer in terrain.terrainData.terrainLayers)
        {
            if (layer != null) EditorUtility.SetDirty(layer);
        }

        EditorUtility.SetDirty(terrain);
        AssetDatabase.SaveAssets();
        Debug.Log("[TerrainManager] Diffuse material applied.");
    }
}
