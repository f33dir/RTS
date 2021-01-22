using System;
using Godot;

class SoundEngine : Node{
    public AudioStreamPlayer music;
    public override void _Ready()
    {
        base._Ready();
        music = new AudioStreamPlayer();
        music.Bus = "music";
    }

    public void PlayMenuMusic(){
        var stream = ResourceLoader.Load<AudioStreamSample>("res://Resources/Sounds/rts cringe.wav");
        music.Stream = stream;
        music.Play();
    }
    public void PlayBattleMusic(){
        var stream = ResourceLoader.Load<AudioStreamSample>("res://Resources/Sounds/rts cringe 2.wav");
        music.Stream = stream;
        music.Play();
    }
}