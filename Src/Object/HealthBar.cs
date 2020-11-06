using Godot;
using System;

namespace Unit
{
    public class HealthBar : Sprite3D
    {
        private Viewport _Viewport;
        private HealthBar2D _HealthBar2D;
        
        public override void _Ready()
        {
            _Viewport = GetNode<Viewport>("Viewport");
            _HealthBar2D = GetNode<HealthBar2D>("Viewport/HealthBar");
            Texture = _Viewport.GetTexture();
        }
        public void Update(int CurrentHealth, int FullHealth)
        {
            _HealthBar2D.UpdateBar(CurrentHealth, FullHealth);
        }
    }
}