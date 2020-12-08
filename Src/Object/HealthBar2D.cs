using Godot;
using System;

namespace Unit
{
    public class HealthBar2D : TextureProgress
    {
        private const float DANGER = 0.3f;
        private const float WARNING = 0.75f;
        private const float FINE = 1.0f;

        private Resource _HealthBarGreen;
        private Resource _HealthBarYellow;
        private Resource _HealthBarRed;
        private TextureProgress _HealthBar;

        public override void _Ready()
        {
            _HealthBar = GetNode<TextureProgress>("HealthBar");
            _HealthBarGreen = GD.Load("res://TempRes/healthbar_green.png");
            _HealthBarYellow = GD.Load("res://TempRes/healthbar_yellow.png");
            _HealthBarRed = GD.Load("res://TempRes/healthbar_red.png");
        }
        public void UpdateBar(int CurrentHealth)
        {
            this.TextureProgress_ = _HealthBarGreen as Texture;
            if(CurrentHealth < MaxValue*WARNING)
                this.TextureProgress_ = _HealthBarYellow as Texture;
            if(CurrentHealth < MaxValue*DANGER)
                this.TextureProgress_ = _HealthBarRed as Texture;
            this.Value = CurrentHealth;
        }
    }
}
