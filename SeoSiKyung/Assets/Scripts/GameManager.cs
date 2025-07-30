using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int health;
    public PlayerMove player;

    public void HealthDown(int h)
    {
        if (health > h)
        {
            health -= h;
        }
        else
        {
            // player.die();
        }
    }
}
