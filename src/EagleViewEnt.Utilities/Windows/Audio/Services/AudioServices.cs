//-----------------------------------------------------------------------
// <copyright 
//	   Author="Brian Dick"
//     Company="Eagle View Enterprises LLC"
//     Copyright="(c) Eagle View Enterprises LLC. All rights reserved."
//     Email="support@eagleviewent.com"
//     Website="www.eagleviewent.com"
// >
//	   <Disclaimer>
//			THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// 			TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// 			THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// 			CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// 			DEALINGS IN THE SOFTWARE.
// 		</Disclaimer>
// </copyright>
//-----------------------------------------------------------------------

using System.Runtime.Versioning;

using NAudio.CoreAudioApi;

namespace MgbUtilities.Windows.Audio.Services;

/// <summary>
///  Provides services for managing audio playback on Windows platforms, including volume control and mute
///  functionality.
/// </summary>
/// <remarks>
///  This class interacts with the default audio playback device to perform operations such as increasing or decreasing
///  the volume, muting or unmuting the audio, and setting the volume to a specific level. It is designed to work on
///  Windows operating systems and requires the appropriate platform support.
/// </remarks>
[SupportedOSPlatform("windows")]
public class AudioServices
{

    readonly MMDevice _defaultDevice;

    float _volumeStep = 1.5f;

    public AudioServices()
    {

        // Initialize the default audio playback device
        MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
        _defaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
    }

    /// <summary>
    ///  Gets or sets the incremental step value for adjusting the volume. <remarks>This method adjusts the volume by a
    ///  fixed amount defined by <c>VolumeStep</c>.   Ensuring that the resulting volume does not fall above or below
    ///  allowed values.</remarks>
    /// </summary>
    public float VolumeStep
    {
        get => _volumeStep;
        set
        {
            if((_volumeStep == value) || (_volumeStep > 50.0f) || (_volumeStep < 0.1f))
                return;
            _volumeStep = value;
        }
    }

    /// <summary>
    ///  Decreases the current volume by a predefined step.
    /// </summary>
    public void DecreaseVolume() => SetVolume(GetVolume() - VolumeStep);

    /// <summary>
    ///  Gets the current master volume level as a percentage.
    /// </summary>
    /// <returns>
    ///  A <see cref="float" /> representing the master volume level, where 0 indicates muted and 100 indicates maximum
    ///  volume.
    /// </returns>
    public float GetVolume() => _defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100;

    /// <summary>
    ///  Increases the current volume by a predefined step.
    /// </summary>
    /// <remarks>
    ///  This method adjusts the volume by adding a fixed increment, defined by <c>VolumeStep</c>, to the current volume
    ///  level. Ensure that the resulting volume does not exceed the maximum allowable limit.
    /// </remarks>
    public void IncreaseVolume() => SetVolume(GetVolume() + VolumeStep);

    /// <summary>
    ///  Determines whether the default audio device is currently muted.
    /// </summary>
    /// <returns><see langword="true" /> if the default audio device is muted; otherwise, <see langword="false" />.</returns>
    public bool IsMuted() => _defaultDevice.AudioEndpointVolume.Mute;

    /// <summary>
    ///  Mutes or unmutes the audio output of the default device.
    /// </summary>
    /// <param name="mute">
    ///  A boolean value indicating whether to mute the audio.  <see langword="true" /> to mute the audio; <see
    ///  langword="false" /> to unmute.
    /// </param>
    public void Mute( bool mute ) => _defaultDevice.AudioEndpointVolume.Mute = mute;

    /// <summary>
    ///  Sets the audio volume level for the default audio device.
    /// </summary>
    /// <remarks>
    ///  If the audio device is currently muted, calling this method will unmute it. The volume is set as a scalar value
    ///  between 0.0 and 1.0, corresponding to the percentage provided.
    /// </remarks>
    /// <param name="volumeLevel">
    ///  The desired volume level as a percentage. Valid values range from 0.0 to 100.0. Values below 0.0 will be set to
    ///  0.0, and values above 100.0 will be set to 100.0.
    /// </param>
    public void SetVolume( float volumeLevel )
    {
        if(volumeLevel < 0.0f)
            volumeLevel = 0.0f;
        if(volumeLevel > 100.0f)
            volumeLevel = 100.0f;
        if(IsMuted())
            Mute(false);

        // Set the volume as a scalar value (0.0 to 1.0)
        _defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = volumeLevel / 100.0f;
    }

}