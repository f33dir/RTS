using Godot;
using System;
using CameraBase;
using Unit;

namespace Player
{
    //Collision mask of game objects like building, units etc.
    public enum CollisionMask
    {
        Nothing,
        Environment,
        Units,
    }
    public enum MouseModifier // пока пусть будет енум на всякий, 
    {                         //шифт лучше вынести в отдельный триггер для ситуаций по типу shift+"модификатор"+ПКМ(ЛКМ)
        None = 0,
        Attack,               // "A" modifier
    }
    public class Player : Spatial
    {
        private int _Resource;
        private int _Lives;
        private bool _Start = false;
        private CameraBase.CameraBase _Camera;
        //Setup
        public override void _Ready()
        {
            _Camera = GetParent().GetNode<CameraBase.CameraBase>("CameraBase");
            _Resource = 100;
            _Lives = 5;
        }
        //Player interactions
        public override void _Process(float delta)
        {
            if(Input.IsActionJustPressed("action_command"))
            {
                _Start = true;
                GetTree().CallGroup("Units", "MoveTo");
            }
            if(_Lives <= 0) 
                GetTree().Quit(); // грубо, но для теста сойдет
        }
        public int Lives
        {
            get { return _Lives;}
            set
            {
                _Lives = value;
            }
        }
        //возможно юзается где-то еще
        public CameraBase.CameraBase GetCamera()
        {
            return _Camera;
        }
        public bool Start
        {
            get{ return _Start; }
        }
        public CameraBase.CameraBase Camera
        {
            get{ return _Camera; }
        }
    }
}
