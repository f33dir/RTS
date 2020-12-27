using Godot;
using System;
using System.Collections.Generic;

public class Portal : Unit.BuildingUnit
{
    private Queue<Unit.DynamicUnit> _SpawnQueue = new Queue<Unit.DynamicUnit>();
    private PackedScene _Bug;
    private PackedScene _Tank;
    private PackedScene _ATAT;
    private Node _LocalTree;
    private int _wave = 0;
    private int _enemycount = 5;
    private Timer _spawntimer;
    private Timer _waveTimer;
    public bool AbleToSpawn =true; 
    private int wave = 3;
    public override void _Ready()
    {
        _Bug = ResourceLoader.Load<PackedScene>("res://Scenes/Units/Bug.tscn");
        _ATAT = ResourceLoader.Load<PackedScene>("res://Scenes/Units/AT-AT.tscn");
        _Tank = ResourceLoader.Load<PackedScene>("res://Scenes/Units/Tank.tscn");
        _LocalTree = GetParent().GetParent().GetParent();
        _spawntimer = GetNode<Timer>("SpawnTimer");
        _waveTimer = GetNode<Timer>("WaveTimer");
        _spawntimer.WaitTime = 0.8f;
        _waveTimer.WaitTime = 10f;
        GetNode("/root/Environment/Player").Connect("WaveStart",this,"HandleStartSignal");
    }
    public void SpawnWave()
    {
        if(AbleToSpawn)
        {
            ++wave;    
            var det = wave % 3;
            switch(det)
            {
                case 0:
                    for(int i = 0;i<_enemycount;i++)
                    {
                        _SpawnQueue.Enqueue(MakeEnemy(_Tank));
                    }
                    _spawntimer.Start();
                    break;
                case 1:
                    for(int i = 0;i<_enemycount;i++)
                    {
                        _SpawnQueue.Enqueue(MakeEnemy(_ATAT));
                    }
                    _spawntimer.Start();
                    break;
                case 2:
                for(int i = 0;i<_enemycount;i++)
                    {
                        _SpawnQueue.Enqueue(MakeEnemy(_Bug));
                    }
                    _spawntimer.Start();
                    _enemycount++;
                    break;
            }
            AbleToSpawn = false;
        }
    }
    private Unit.DynamicUnit MakeEnemy(PackedScene unitScene)
    {
        Unit.DynamicUnit enemy = (Unit.DynamicUnit)unitScene.Instance();   
        return enemy;
    }
    private void SetupEnemy(int wave,Unit.DynamicUnit enemy){
        wave*= 10;
        float speed = (1.5f+0.05f*wave);
        float size = (1 + 0.001f*wave);
        int damage = (wave/10)+1;
        int hp  = wave*5;
        enemy.HP = hp;
        enemy.Damage = damage;
        enemy.MoveSpeed = speed*4;
        // enemy.Scale = new Vector3(size,size,size);
        enemy.Cost = 5+ (wave / 2);
    }
    public void _on_SpawnTimer_timeout()
    {
        if(_SpawnQueue.Count>0)
        {
            var enemy = _SpawnQueue.Dequeue();
            _LocalTree.AddChild(enemy);
            enemy.Transform = this.Transform;
            SetupEnemy(wave,enemy);
            _spawntimer.Start();
            enemy.MoveTo();
        }
        else
        {
            AbleToSpawn = true;
            _waveTimer.Start();
        }
    }
    public void HandleStartSignal()
    {
        GD.Print("gotcha");
        SpawnWave();
    }
    public void _on_WaveTimer_timeout()
    {
        SpawnWave();
    }
}
