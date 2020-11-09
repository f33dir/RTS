using Godot;
using System;

namespace Unit
{
    public class HealthBar : Spatial
    {
        private Viewport _Viewport;
        private HealthBar2D _HealthBar2D;
        private Sprite3D _HealthBar3D;
        
        public override void _Ready()
        {
            _Viewport = GetNode<Viewport>("Viewport");
            _HealthBar2D = GetNode<HealthBar2D>("Viewport/HealthBar");
            _HealthBar3D = GetNode<Sprite3D>("HP");
            _HealthBar3D.Texture = _Viewport.GetTexture();
        }
        public void Update(int CurrentHealth, int FullHealth)
        {
            _HealthBar2D.UpdateBar(CurrentHealth, FullHealth);
        }
    }
}