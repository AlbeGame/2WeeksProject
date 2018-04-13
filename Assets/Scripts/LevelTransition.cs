using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelTransition : MonoBehaviour {

    public int SceneIndex = -1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (SceneIndex == -1)
            return;

        if (collision.collider.GetComponent<MainCharacter>() != null)
            SceneManager.LoadScene(SceneIndex);
    }
}
