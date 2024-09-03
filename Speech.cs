using System.Configuration;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

class Speech
{
    // ENV Speech keys
    public static string? speechKey = ConfigurationManager.AppSettings.Get("SPEECH_SERVICE_KEY");
    public static string? speechRegion = ConfigurationManager.AppSettings.Get(
        "SPEECH_SERVICE_REGION"
    );

    // Speech Recognition Result function
    async static Task<string> FromMic(SpeechConfig speechConfig)
    {
        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        Console.WriteLine("Speak into your microphone.");
        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");

        return speechRecognitionResult.Text;
    }

    public static async Task<string> HandleSpeech()
    {
        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
        return await FromMic(speechConfig);
    }
}
