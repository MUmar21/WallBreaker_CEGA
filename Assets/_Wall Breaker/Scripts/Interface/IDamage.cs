using UnityEngine;

public interface IDamage 
{
    void Damage();
}


public abstract class DamageCl : MonoBehaviour
{
    public virtual void Damage()
    {
        Debug.Log("Damaged!");  
    }
}
