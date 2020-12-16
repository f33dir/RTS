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
            MaxValue = 10;
        }
        public double MaxValue
        {
            get{ return _HealthBar2D.MaxValue;}
            set
            {
                // if(value > _HealthBar2D.MaxValue)
                    _HealthBar2D.MaxValue = value;
            }
        }
        public void UpdateBar(int CurrentHealth)
        {
            _HealthBar2D.UpdateBar(CurrentHealth);
        }
    }
}