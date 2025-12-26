using UnityEngine;

public class GameComplete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Completed!");
        }

    }

}
