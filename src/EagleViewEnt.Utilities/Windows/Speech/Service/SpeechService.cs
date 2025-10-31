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

using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using System.Speech.Synthesis;
using System.Text;

using AKSoftware.Localization.MultiLanguages;
using MgbUtilties.Core.Extensions.Logger;

using Serilog;
using MgbUtilities.Windows.Speech.Extensions;

namespace MgbUtilities.Windows.Speech.Service;

#pragma warning disable IDE0330 // Use 'System.Threading.Lock'
[SupportedOSPlatform("windows")]
public class SpeechService : ISpeechService, IDisposable
{

    readonly ILanguageContainerService _language;
    readonly object _lock = new object();
    readonly SpeechSynthesizer _speech = new();
    bool _mute;
    ReadOnlyCollection<InstalledVoice> _installedVoices;
    string[] _lastResourceKeys = [];

    public SpeechService( ILanguageContainerService language )
    {
        _language = language;
        _installedVoices = _speech.GetInstalledVoices();
        if(_installedVoices.Count > 1)
            _speech.SelectVoice(_installedVoices[1].VoiceInfo.Name);
    }

    /// <summary>
    ///  Gets or sets a value indicating whether the audio is muted.
    /// </summary>
    public bool Mute
    {
        get => _mute;
        set
        {
            lock(_lock) {
                if(_mute == value)
                    return;
                CancelAsync(); // This is safe, as CancelAsync also locks on _lock (reentrant)
                _mute = value;
            }
        }
    }

    /// <summary>
    ///  Cancels all ongoing and pending asynchronous speech synthesis operations.
    /// </summary>
    /// <remarks>
    ///  This method should be called when you need to stop all speech synthesis tasks that are currently in progress or
    ///  queued. It is thread-safe and can be called from any thread.
    /// </remarks>
    public void CancelAsync()
    {
        lock(_lock)
            if(_speech.State != SynthesizerState.Ready)
                _speech.SpeakAsyncCancelAll();
    }

    public void Dispose()
    {
        _speech.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///  Retrieves a culture-specific text representation based on the provided language code.
    /// </summary>
    /// <remarks>
    ///  This method processes a collection of resource keys to generate a text string, which is then formatted using the
    ///  specified language code. The method is thread-safe due to the use of a lock.
    /// </remarks>
    /// <param name="lang">The language code used to format the text. This parameter cannot be null or empty.</param>
    /// <returns>
    ///  A string containing the culture-specific text formatted according to the specified language code. Returns an empty
    ///  string if no resource keys are available.
    /// </returns>
    string GetCultureText( string lang )
    {
        lock(_lock) {

            // Check for null and empty array
            if(_lastResourceKeys.Length == 0)
                return string.Empty;

            StringBuilder result = new StringBuilder();
            foreach(string key in _lastResourceKeys) {
                if(string.IsNullOrWhiteSpace(key))
                    continue;
                result.Append(_language[key]);
            }
            return result.ToString().ToTextSsml(lang);
        }
    }

    /// <summary>
    ///  Asynchronously replays the last spoken text in the specified language.
    /// </summary>
    /// <remarks>
    ///  This method replays the last spoken text using the specified language. If no language is specified,  it
    ///  defaults to English ("en-US"). The replay is performed asynchronously.
    /// </remarks>
    /// <param name="lang">The language code for the text to be replayed. Defaults to "en-US".</param>
    public void ReplayAsync( string lang = "en-US" ) => SpeakAsync(GetCultureText(lang), lang, true);

    /// <summary>
    ///  Initiates asynchronous speech synthesis for the specified text.
    /// </summary>
    /// <remarks>
    ///  If the specified language is not available, the method will use the first available voice. The method does
    ///  nothing if the text is null or empty, or if the service is muted.
    /// </remarks>
    /// <param name="text">
    ///  The text to be spoken. If <paramref name="isSsml" /> is <see langword="true" />, this should be a valid SSML
    ///  string.
    /// </param>
    /// <param name="lang">The language code for the voice to be used. Defaults to "en-US".</param>
    /// <param name="isSsml">
    ///  Indicates whether the <paramref name="text" /> is in SSML format. <see langword="true" /> if the text is SSML;
    ///  otherwise, <see langword="false" />.
    /// </param>
    public void SpeakAsync
        ( string text, string lang = "en-US", bool isSsml = false )
    {
        if(_mute || string.IsNullOrEmpty(text))
            return;

        lock(_lock) {
            CancelAsync();

            InstalledVoice? voice = _installedVoices
                .FirstOrDefault(v => v.VoiceInfo.Culture.Name.Equals(lang, StringComparison.OrdinalIgnoreCase)) ??
                _installedVoices.FirstOrDefault();

            if(voice != null)
                _speech.SelectVoice(voice.VoiceInfo.Name);

            try {
                Log.Logger
                .WithCallingContext<SpeechService>()
                .Debug("SpeakAsync: {Text} {Language} {Voice}", text, lang, voice?.VoiceInfo.Name);

                if(isSsml)
                    _speech.SpeakSsmlAsync(text);
                else
                    _speech.SpeakAsync(text);
            } catch(Exception ex) {
                Log.Logger.WithCallingContext<SpeechService>().Error(ex, "Speech failed. {Text}", text);
            }
        }
    }

    /// <summary>
    ///  Asynchronously speaks the text associated with the specified resource keys in the given language.
    /// </summary>
    /// <remarks>
    ///  The method locks the resource keys to ensure thread safety and initiates an asynchronous operation to speak the
    ///  text. The text is retrieved based on the provided language and resource keys.
    /// </remarks>
    /// <param name="lang">The language code representing the language in which the text should be spoken.</param>
    /// <param name="resourceKeys">An array of resource keys identifying the text to be spoken.</param>
    public void SpeakResourceAsync( string lang, params string[] resourceKeys )
    {
        lock(_lock)
            _lastResourceKeys = resourceKeys?.ToArray() ?? [];
        SpeakAsync(GetCultureText(lang), lang, true);
    }

}

