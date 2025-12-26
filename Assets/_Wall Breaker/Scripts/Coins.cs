using UnityEngine;

public class Coins : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger working");

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Coin collected!");
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }
}
