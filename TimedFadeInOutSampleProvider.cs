using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ringtone2iPhone
{
    /// <summary>
    /// Extension methods to add between SampleProviders easier
    /// </summary>
    public static class SampleExtensionMethods
    {
        /// <summary>
        /// Takes a specified amount of time from the source stream
        /// </summary>
        /// <param name="sampleProvider">Source sample provider</param>
        /// <param name="take">Duration to take</param>
        /// <returns>A sample provider that reads up to the specified amount of time</returns>
        public static ISampleProvider TakeWithFade(this ISampleProvider sampleProvider, TimeSpan take, TimeSpan fadeIn, TimeSpan fadeOut, TimeSpan skip)
        {
            var offset = new OffsetSampleProvider(sampleProvider) { SkipOver = skip, Take = take };
            if (fadeIn == TimeSpan.Zero && fadeOut == TimeSpan.Zero) return offset;
            var fade = new TimedFadeInOutSampleProvider(offset, fadeIn != TimeSpan.Zero);
            if (fadeIn != TimeSpan.Zero) fade.AddFadeIn(TimeSpan.Zero, fadeIn);
            if (fadeOut != TimeSpan.Zero) fade.AddFadeOut(take - fadeOut, fadeOut);
            return fade;
        }

        /// <summary>
        /// Takes a specified amount of time from the source stream
        /// </summary>
        /// <param name="sampleProvider">Source sample provider</param>
        /// <param name="take">Duration to take</param>
        /// <returns>A sample provider that reads up to the specified amount of time</returns>
        public static ISampleProvider TakeWithFade(this ISampleProvider sampleProvider, TimeSpan take, TimeSpan fadeIn, TimeSpan fadeOut)
        {
            return sampleProvider.TakeWithFade(take, fadeIn, fadeOut, TimeSpan.Zero);
        }
    }

    /// <summary>
    /// Sample Provider to allow fading in and out at specific position
    /// </summary>
    public class TimedFadeInOutSampleProvider : ISampleProvider
    {

        /// <summary>
        /// WaveFormat of this SampleProvider
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return source.WaveFormat; }
        }

        enum FadeState
        {
            Silence,
            FadingIn,
            FullVolume,
            FadingOut,
        }

        class FadeAction
        {
            public FadeState fadeState;
            public int fadeSampleCount;
        }

        private SortedDictionary<int, FadeAction> fadeActions;
        private readonly ISampleProvider source;
        private int processingPosition;
        private int fadeActionPosition;
        private int fadeSamplePosition;
        private int fadeSampleCount;
        private FadeState fadeState;

        /// <summary>
        /// Creates a new FadeInOutSampleProvider
        /// </summary>
        /// <param name="source">The source stream with the audio to be faded in or out</param>
        /// <param name="initiallySilent">If true, we start faded out</param>
        public TimedFadeInOutSampleProvider(ISampleProvider source, bool initiallySilent = false)
        {
            this.source = source;
            fadeState = initiallySilent ? FadeState.Silence : FadeState.FullVolume;
            fadeActions = new SortedDictionary<int, FadeAction>();
            processingPosition = 0;
            fadeActionPosition = 0;
        }

        /// <summary>
        /// Requests that a fade-in begins (will start on the next call to Read)
        /// </summary>
        /// <param name="fadeDurationInMilliseconds">Duration of fade in milliseconds</param>
        public void AddFadeIn(TimeSpan fadePosition, TimeSpan fadeDuration)
        {
            AddFade(fadePosition, fadeDuration, FadeState.FadingIn);
        }

        /// <summary>
        /// Requests that a fade-out begins (will start on the next call to Read)
        /// </summary>
        /// <param name="fadeDurationInMilliseconds">Duration of fade in milliseconds</param>
        public void AddFadeOut(TimeSpan fadePosition, TimeSpan fadeDuration)
        {
            AddFade(fadePosition, fadeDuration, FadeState.FadingOut);
        }

        /// <summary>
        /// Reads samples from this sample provider
        /// </summary>
        /// <param name="buffer">Buffer to read into</param>
        /// <param name="offset">Offset within buffer to write to</param>
        /// <param name="count">Number of samples desired</param>
        /// <returns>Number of samples read</returns>
        public int Read(float[] buffer, int offset, int count)
        {
            int sourceSamplesRead = source.Read(buffer, offset, count);

            int sample = 0;
            while (sample < sourceSamplesRead)
            {
                if (fadeActions.Keys.Count > 0 && fadeActionPosition == 0) fadeActionPosition = fadeActions.Keys.Min();
                if (fadeActionPosition == processingPosition)
                {
                    fadeSamplePosition = 0;
                    fadeSampleCount = fadeActions[fadeActionPosition].fadeSampleCount;
                    fadeState = fadeActions[fadeActionPosition].fadeState;
                    fadeActions.Remove(fadeActionPosition);
                    fadeActionPosition = 0;
                }
                float multiplier;
                switch (fadeState)
                {
                    case FadeState.FadingIn:
                        multiplier = (fadeSamplePosition / (float)fadeSampleCount);
                        for (int ch = 0; ch < source.WaveFormat.Channels; ch++)
                            buffer[offset + sample++] *= multiplier;
                        fadeSamplePosition++;
                        if (fadeSamplePosition > fadeSampleCount) fadeState = FadeState.FullVolume;
                        break;
                    case FadeState.FadingOut:
                        multiplier = 1.0f - (fadeSamplePosition / (float)fadeSampleCount);
                        for (int ch = 0; ch < source.WaveFormat.Channels; ch++)
                            buffer[offset + sample++] *= multiplier;
                        fadeSamplePosition++;
                        if (fadeSamplePosition > fadeSampleCount) fadeState = FadeState.Silence;
                        break;
                    case FadeState.Silence:
                        for (int ch = 0; ch < source.WaveFormat.Channels; ch++)
                            buffer[offset + sample++] = 0;
                        break;
                    default:
                        sample += source.WaveFormat.Channels;
                        break;
                }
                processingPosition++;
            }
            return sourceSamplesRead;
        }

        private void AddFade(TimeSpan fadePosition, TimeSpan fadeDuration, FadeState fadeState)
        {
            var fadeSamplePosition = (int)((fadePosition.TotalMilliseconds * source.WaveFormat.SampleRate) / 1000);
            var fadeSampleCount = (int)((fadeDuration.TotalMilliseconds * source.WaveFormat.SampleRate) / 1000);
            fadeActions.Add(fadeSamplePosition, new FadeAction { fadeState = fadeState, fadeSampleCount = fadeSampleCount });
        }
    }
}
