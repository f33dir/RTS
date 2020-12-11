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
    private int wave = 3;
    public override void _Ready()
    {
        _Bug = ResourceLoader.Load<PackedScene>("res://Scenes/Units/Bug.tscn");
        _ATAT = ResourceLoader.Load<PackedScene>("res://Scenes/Units/AT-AT.tscn");
        _Tank = ResourceLoader.Load<PackedScene>("res://Scenes/Units/Tank.tscn");
        _LocalTree = GetParent().GetParent().GetParent();
        _spawntimer = GetNode<Timer>("SpawnTimer");
        _spawntimer.WaitTime = 0.5f;
        SpawnWave();
    }
    public void SpawnWave()
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
    }
    private Unit.DynamicUnit MakeEnemy(PackedScene unitScene)
    {
        Unit.DynamicUnit enemy = (Unit.DynamicUnit)unitScene.Instance();   
        return enemy;
    }
    private void SetupEnemy(int wave,Unit.DynamicUnit enemy){
        float speed = (1.5f+0.05f*wave);
        float size = (1 + 0.01f*wave);
        int damage = (wave/10)+1;
        int hp  = wave*5;
        enemy.HP = hp;
        enemy.Damage = damage;
        enemy.MoveSpeed = speed;
        enemy.Scale = new Vector3(size,size,size);
    }
    public void _on_SpawnTimer_timeout()
    {
        if(_SpawnQueue.Count>0)
        {
            var enemy = _SpawnQueue.Dequeue();
            _LocalTree.AddChild(enemy);
            SetupEnemy(wave,enemy);
            _spawntimer.Start();
        }
    }
}
