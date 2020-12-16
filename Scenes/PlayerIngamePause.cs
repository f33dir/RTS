using Godot;
using System;

public class PlayerIngamePause : Control
{
    private TextureButton CountinueButton;
    private TextureButton ExitButton;
    private Boolean Paused;
    public override void _Ready()
    {
        CountinueButton = GetNode<TextureButton>("ButtonBox/ButtonCountinueBox/ButtonCountinue");
        ExitButton = GetNode<TextureButton>("ButtonBox/ButtonExitBox/ButtonExit");
        CountinueButton.Connect("pressed",this,"_on_ButtonCountinue_button_down");
        ExitButton.Connect("pressed",this,"exitbutton");
    }
    public override void _PhysicsProcess(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel"))
        {
            Switch();
        }
    }
    public void Switch()
    {
        if(Paused)
        {
            GetTree().Paused = false;
            this.Visible = false;
            Paused = false;
        }
        else
        {
            GetTree().Paused = true;
            this.Visible = true;
            Paused = true;
        }
    }
    void _on_ButtonCountinue_button_down()
    {
        Switch();
    }
    void exitbutton()
    {
        GetTree().Quit();
    }
}
