
using System;

[Serializable]
public class AudioSettings
{
    // Range: 0..100
    public int PreviewVolumePercent { get; set; } = 50;
    public int VolumePercent { get; set; } = 100;
    public int BackgroundMusicVolumePercent { get; set; } = 70;

    public int SceneChangeSoundVolumePercent { get; set; } = 100;

    public EPitchDetectionAlgorithm pitchDetectionAlgorithm = EPitchDetectionAlgorithm.Dywa;
}
