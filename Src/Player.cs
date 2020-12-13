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
        private Label _ResourceLabel;
        private Label _LifeLabel;
        //Setup
        public override void _Ready()
        {
            _Camera = GetParent().GetNode<CameraBase.CameraBase>("CameraBase");
            _ResourceLabel = GetParent().GetNode<Label>("Interface/ResourceCounter/Label");
            _LifeLabel = GetParent().GetNode<Label>("Interface/LifeCounter/Label");
            Resource = 100;
            Lives = 5;
        }
        //Player interactions
        public override void _Process(float delta)
        {
            if(Input.IsActionJustPressed("action_command"))
            {
                if(!_Start)
                {
                    _Start = true;
                    GetTree().CallGroup("Units", "MoveTo");
                }
            }
            if(Input.IsActionJustPressed("alt_command"))
            {
                GetTree().CallGroup("Towers", "Upgrade");
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
                _LifeLabel.Text = _Lives.ToString();
            }
        }
        public int Resource
        {
            get{ return _Resource;}
            set
            {
                if(value >= 0 && value <= 1000)
                    _Resource = value;
                if(value < 0)
                    _Resource = 0;
                _ResourceLabel.Text = _Resource.ToString();
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
