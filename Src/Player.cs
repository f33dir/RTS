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
        public BuildingManager.BuilderManager BuilderManager;
        private bool cmnd = false;
        private int _Resource;
        private int _Lives;
        private bool _Start = false;
        private CameraBase.CameraBase _Camera;
        [Signal]
        public delegate void WaveStart();
        private Label _ResourceLabel;
        private Label _LifeLabel;
        //Setup
        public override void _Ready()
        {
            _Camera = GetParent().GetNode<CameraBase.CameraBase>("CameraBase");
            _ResourceLabel = GetParent().GetNode<Label>("Interface/ResourceCounter/Label");
            _LifeLabel = GetParent().GetNode<Label>("Interface/LifeCounter/Label");
            _Resource = 100;
            _Lives = 5;
        }
        //Player interactions
        public override void _Process(float delta)
        {
            if(Input.IsActionJustPressed("StartWave"))
            {
                EmitSignal(nameof(WaveStart));
            }
            int selector = -1;
            if(Input.IsActionJustPressed("build_tower1"))
            {
                selector = 0;
            }
            if(Input.IsActionJustPressed("build_tower2"))
            {
                selector = 1;
            }
            if(Input.IsActionJustPressed("build_tower3"))
            {
                selector = 2;
            }
            if(selector!= -1)
                StartBuild(selector);
            var p = this;
            if(Input.IsActionJustPressed("action_command")&&(cmnd))
                if(BuilderManager.IsBuildable(ref p))
                {
                    BuilderManager.build(ref p);
                    cmnd = false;
                }
            if(Input.IsActionJustPressed("alt_command"))
            {
                GetTree().CallGroup("Towers", "Upgrade");
            }
            if(_Lives <= 0) 
                GetTree().Quit(); // грубо, но для теста сойдет
        }
        
        //gameplay functions
        private void StartBuild(int selector)
        {
            PackedScene a=ResourceLoader.Load<PackedScene>("res://Scenes/Towers/GenericTower.tscn");
            switch(selector)
            {
                case 0:
                    a =ResourceLoader.Load<PackedScene>("res://Scenes/Towers/GenericTower.tscn");
                    cmnd = true;
                    break;
                case 1:
                    a =ResourceLoader.Load<PackedScene>("res://Scenes/Towers/FreezingTower.tscn");
                    cmnd = true;
                    break;
                case 2:
                    cmnd =true;
                    a =ResourceLoader.Load<PackedScene>("res://Scenes/Towers/MachineGunTower.tscn");
                    break;
            }
            if(selector!=-1)
            {
                BuilderManager.ShowArrow();
            }
            var b = a.Instance() as Unit.BuildingUnit;
            var p = this;
            BuilderManager.SetBuilding(ref p,ref b);
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
