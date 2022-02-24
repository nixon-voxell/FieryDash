using UnityEngine;

[System.Serializable]
public struct PlatformConfig
{
  public float Seed => _seed;
  private float _seed;

  public float NoiseInput => _noiseInput;
  private float _noiseInput;
  [SerializeField, Range(0.01f, 1.0f)] private float _increment;
  [SerializeField] private float _minLength, _maxLength;

  public PlatformConfig(float increment, float minLength, float maxLength)
  {
    this._seed = 0.0f;
    this._noiseInput = 0.0f;
    this._increment = increment;
    this._minLength = minLength;
    this._maxLength = maxLength;
  }

  public void ReSeed() => _seed = Random.Range(0.0f, 1000.0f);

  public float GetRandomLength()
  {
    float noise = Mathf.PerlinNoise(_noiseInput, _seed);
    _noiseInput += _increment;
    return Mathf.Lerp(_minLength, _maxLength, noise);
  }
}
