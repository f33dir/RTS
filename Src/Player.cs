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
        private bool _Cmnd = false;
        private int _CurrentTowerCost =0;
        private int _Resource;
        private int _Lives;
        private bool _Start = false;
        private CameraBase.CameraBase _Camera;
        private Unit.Tower _SelectedTower;
        [Signal]
        public delegate void WaveStart();
        private Label _ResourceLabel;
        private Label _LifeLabel;
        private Interface _Interface;
        //Setup
        public override void _Ready()
        {
            _Camera = GetParent().GetNode<CameraBase.CameraBase>("CameraBase");
            _ResourceLabel = GetParent().GetNode<Label>("Interface/ResourceCounter/Label");
            _LifeLabel = GetParent().GetNode<Label>("Interface/LifeCounter/Label");
            _Interface = GetParent().GetNode<Interface>("Interface");
            Resource = 150;
            Lives = 50;
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
            if(Input.IsActionJustPressed("action_command"))
                {
                    BuilderManager.HideArrow();
                    if((_Cmnd)&&(_CurrentTowerCost<=_Resource))
                    {
                        {
                            if(BuilderManager.IsBuildable(ref p))
                            {
                                Resource -=_CurrentTowerCost;
                                BuilderManager.build(ref p);
                                _Cmnd = false;
                                _CurrentTowerCost = 0;
                            }
                        }   
                    }
                }
            if(Input.IsActionJustPressed("alt_command"))
            {
                var obj = Camera.RaycastFromMousePosition(GetViewport().GetMousePosition(),8);
                // if(obj.Contains("collider"))
                // {
                //     var tower = obj["collider"] as Unit.Tower;
                //     if(tower != null)
                //     {
                //         tower.Upgrade();
                //     }
                // }
                if(obj.Contains("collider"))
                {
                    if(_SelectedTower != null)
                        _SelectedTower.Deselect();
                    SelectedTower = obj["collider"] as Unit.Tower;
                    _Interface.UpdateStats();
                    SelectedTower.Select();
                }
                else if (_SelectedTower != null)
                {
                    _SelectedTower.Deselect();
                    _SelectedTower = null;
                    _Interface.UpdateStats();
                }
            }
            if(Input.IsActionJustPressed("UpgradeTower"))
            {
                if(_SelectedTower != null)
                {
                    if(_Resource >= _SelectedTower.Cost)
                    {
                        _SelectedTower.Upgrade();
                        _Interface.UpdateStats();
                    }
                }
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
                    _CurrentTowerCost = 100;
                    break;
                case 1:
                    a =ResourceLoader.Load<PackedScene>("res://Scenes/Towers/FreezingTower.tscn");
                    _CurrentTowerCost =150;
                    break;
                case 2:
                    _CurrentTowerCost = 250;
                    a =ResourceLoader.Load<PackedScene>("res://Scenes/Towers/MachineGunTower.tscn");
                    break;
            }
            _Cmnd =true;
            if(selector!=-1)
            {
                BuilderManager.ShowArrow();
            }
            var b = a.Instance() as Unit.BuildingUnit;
            var p = this;
            BuilderManager.SetBuilding(ref p,ref b);
        }
        public Unit.Tower SelectedTower
        {
            get { return _SelectedTower;}
            set
            {
                if(value != null)
                {
                    _SelectedTower = value;
                }
            }
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
