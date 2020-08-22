// FmpVlcBackend
// By Declan Hoare 2020
// Public Domain

using FRESHMusicPlayer.Backends;
using LibVLCSharp.Shared;
using System;
using System.Composition;

[Export(typeof(IAudioBackend))]
class FmpVlcBackend : IAudioBackend
{
	LibVLC vlc;
	MediaPlayer player;

	public event EventHandler<EventArgs> OnPlaybackStopped;

	public float Volume
	{
		get => player.Volume / 100f;
		set => player.Volume = (int)(value * 100);
	}
	public TimeSpan CurrentTime
	{
		get => TimeSpan.FromMilliseconds(player.Time);
		set => player.Time = (long)value.TotalMilliseconds;
	} 

	public TimeSpan TotalTime => TimeSpan.FromMilliseconds(player.Media.Duration);

	public FmpVlcBackend()
	{
		Core.Initialize();
		vlc = new LibVLC();
		player = new MediaPlayer(vlc);
		player.EndReached += OnPlaybackStopped;
	}
	
	public void LoadSong(string file)
	{
		player.Media = new Media(vlc, file, file.Contains("://") ? FromType.FromLocation : FromType.FromPath);
	}
	
	public void Play()
	{
		player.Play();
	}
	
	public void Pause()
	{
		player.SetPause(true);
	}
	
	public void Dispose()
	{
		player.Dispose();
		vlc.Dispose();
	}
}

