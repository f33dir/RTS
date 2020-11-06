using Godot;
using System;

namespace Unit
{
    public class HealthBar2D : TextureProgress
    {
        private const float DANGER = 0.3f;
        private const float WARNING = 0.5f;
        private const float FINE = 1.0f;

        private Resource _HealthBarGreen;
        private Resource _HealthBarYellow;
        private Resource _HealthBarRed;

        public override void _Ready()
        {
            this.Hide();
            
            _HealthBarGreen = GD.Load("res://TempRes/healthbar_green.png");
            _HealthBarYellow = GD.Load("res://TempRes/healthbar_yellow.png");
            _HealthBarRed = GD.Load("res://TempRes/healthbar_red.png");

            if(GetParent() != null && GetParent().Get("_HP") != null)
            {
                this.MaxValue = (int)GetParent().Get("_HP");
            }
        }
        public void UpdateBar(int CurrentHealth, int FullHealth)
        {
            this.TextureProgress_ = _HealthBarGreen as Texture;
            if(CurrentHealth < MaxValue*WARNING)
                this.TextureProgress_ = _HealthBarYellow as Texture;
            if(CurrentHealth < MaxValue*DANGER)
                this.TextureProgress_ = _HealthBarRed as Texture;
            if(CurrentHealth < this.MaxValue)
                Show();
            this.Value = CurrentHealth;
        }
    }
}
