using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainManager : MonoBehaviour
{
    [SerializeField] private TerrainLayer _layerGround;
    [SerializeField] private TerrainLayer _layerGrass;
    [SerializeField] private TerrainLayer _layerDirt;

    [SerializeField] private float _slopeThresholdLow  = 0.35f;
    [SerializeField] private float _slopeThresholdHigh = 0.55f;
    [SerializeField] private float _heightThresholdLow  = 0.06f;
    [SerializeField] private float _heightThresholdHigh = 0.12f;

    public void PaintTerrain()
    {
        var terrain = GetComponent<Terrain>();
        var td = terrain.terrainData;
        td.terrainLayers = new[] { _layerGround, _layerGrass, _layerDirt };

        var res  = td.alphamapResolution;
        var maps = new float[res, res, 3];

        for (var y = 0; y < res; y++)
        {
            for (var x = 0; x < res; x++)
            {
                var nx     = (float)x / (res - 1);
                var ny     = (float)y / (res - 1);
                var height = td.GetInterpolatedHeight(nx, ny) / td.size.y;
                var slope  = td.GetSteepness(nx, ny) / 90f;

                float wGround, wGrass, wDirt;

                if (slope > _slopeThresholdLow)
                {
                    wDirt   = Mathf.InverseLerp(_slopeThresholdLow, _slopeThresholdHigh, slope);
                    wGrass  = 1f - wDirt;
                    wGround = 0f;
                }
                else if (height < _heightThresholdHigh)
                {
                    var t   = Mathf.InverseLerp(_heightThresholdLow, _heightThresholdHigh, height);
                    wGround = 1f - t;
                    wGrass  = t;
                    wDirt   = 0f;
                }
                else
                {
                    wGround = 0f;
                    wGrass  = 1f;
                    wDirt   = 0f;
                }

                maps[y, x, 0] = wGround;
                maps[y, x, 1] = wGrass;
                maps[y, x, 2] = wDirt;
            }
        }

        td.SetAlphamaps(0, 0, maps);
    }

    public void ApplyDiffuseMaterial(Material mat)
    {
        var terrain = GetComponent<Terrain>();
        terrain.materialTemplate = mat;

        foreach (var layer in terrain.terrainData.terrainLayers)
        {
            if (layer == null) continue;
            layer.smoothness = 0f;
            layer.metallic   = 0f;
            layer.specular   = Color.black;
        }
    }
}
