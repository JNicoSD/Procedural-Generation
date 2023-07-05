using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float GenerateNoise(int x, int y, float octaves, float scale, float frequency, float persistence, float lacunarity, float amplitude)
    {
        float _frequency = frequency;
        float _amplitude = amplitude;
        float noiseValue = 0f;

        for(int o = 0; o < octaves; o++)
        {
            float xCoord = (float)x / scale * _frequency;
            float yCoord = (float)y / scale * _frequency;

            float sample = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 0.5f;
            noiseValue += sample * _amplitude;

            _amplitude *= persistence;
            _frequency *= lacunarity;
        }
        return noiseValue;
    }
}
