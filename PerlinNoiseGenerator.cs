using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator {
    private float _amplitude = 1.0f;
    private float _frequency = 100.0f;
    private int _octaves = 8;

    public void SetAmplitude(float amplitude)
    {
        _amplitude = amplitude;
    }

    public void SetFrequency(float frequency)
    {
        _frequency = frequency;
    }

    public void SetOctaves(int octaves)
    {
        _octaves = octaves;
    }

    public float GetPerlinNoiseValueAt(float x, float y)
    {
        float returnValue = 0f;

        float gain = 1.0f;
        for (int i = 0; i < _octaves; i++)
        {
            returnValue += Mathf.PerlinNoise(x * gain / _frequency, y * gain / _frequency) * _amplitude / gain;
            gain *= 2.0f;
        }

        return returnValue;
    }
}
