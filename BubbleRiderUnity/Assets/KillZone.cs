using UnityEngine;
using UnityEngine.SceneManagement;
public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
    }
}
