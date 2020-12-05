using UnityEngine;
using System.Collections;

[System.Serializable]
public class Bullet
{
    public float damage;
    public float shotSpeed;
    public float surviveTime;
    public Enemy tempEnemy;

    public Vector2 _direction;
    protected float _elapsedTime;
    
    public virtual void Direction(Vector2 _direction) { }
    public virtual Vector2 Move() { return _direction; }
    public virtual Vector2 Throw(Vector2 _direction) { return _direction; }
    public void ResetElapsedTime() { _elapsedTime = 0; }
    public void LoadElapsedTime() { _elapsedTime += Time.deltaTime; }
    public bool CheckElapsedTime()
    {
        if (_elapsedTime > surviveTime)
            return true;
        else
            return false;
    }
}

public class B_Pistol : Bullet
{
    public override void Direction(Vector2 _direction)
    {
        this._direction = _direction;
    }

    public override Vector2 Move()
    {
        LoadElapsedTime();
        return _direction * shotSpeed * Time.deltaTime;
    }
}

public class B_SMG : Bullet
{
    public override void Direction(Vector2 _direction)
    {
        this._direction = _direction;
    }

    public override Vector2 Move()
    {
        LoadElapsedTime();
        return _direction * shotSpeed * Time.deltaTime;
    }
}

public class B_Sniper : Bullet
{
    public override void Direction(Vector2 _direction)
    {
        this._direction = _direction;
    }

    public override Vector2 Move()
    {
        LoadElapsedTime();
        return _direction * shotSpeed * Time.deltaTime;
    }
}

public class B_AR : Bullet
{
    public override void Direction(Vector2 _direction)
    {
        this._direction = _direction;
    }

    public override Vector2 Move()
    {
        LoadElapsedTime();
        return _direction * shotSpeed * Time.deltaTime;
    }
}

public class B_SG : Bullet
{
    public override void Direction(Vector2 _direction)
    {
        this._direction = _direction;
    }

    public override Vector2 Move()
    {
        LoadElapsedTime();
        return _direction * shotSpeed * Time.deltaTime;
    }
}

public class B_Grenade : Bullet
{
    public override Vector2 Throw(Vector2 _direction)
    {
        this._direction = Vector2.up;
        this._direction.x = _direction.x;
        return this._direction;
    }
}

public class B_Ailan : Bullet
{
    public override void Direction(Vector2 _direction)
    {
        this._direction = _direction;
    }
    public override Vector2 Move()
    {
        LoadElapsedTime();
        return _direction * shotSpeed * Time.deltaTime;
    }
}

public class B_Robot : Bullet
{
    public override Vector2 Throw(Vector2 _direction)
    {
        this._direction = Vector2.up;
        this._direction.x = _direction.x;
        return this._direction;
    }
}
