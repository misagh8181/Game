using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public GameObject[] images;
    private float _time;

    private void Start()
    {
        // Start the coroutine that will toggle the objects with a delay
        StartCoroutine(ToggleImagesWithDelay());
    }

    private IEnumerator ToggleImagesWithDelay()
    {
        // Ensure there are enough images in the list
        if (images.Length == 0)
        {
            yield break; // Exit if no images
        }

        // Activate the first image, wait for 15 seconds, then deactivate
        images[0].SetActive(true);
        yield return new WaitForSeconds(15f); // Wait for 15 seconds
        images[0].SetActive(false);

        // Activate the second image, wait for 12 seconds, then deactivate
        images[1].SetActive(true);
        yield return new WaitForSeconds(12f); // Wait for 12 seconds
        images[1].SetActive(false);

        // Activate the third image, wait for 8 seconds, then deactivate
        images[2].SetActive(true);
        yield return new WaitForSeconds(8f); // Wait for 8 seconds
        images[2].SetActive(false);

        // Activate the fourth image, wait for 6 seconds, then deactivate
        images[3].SetActive(true);
        yield return new WaitForSeconds(6f); // Wait for 6 seconds
        images[3].SetActive(false);

        // Activate the fifth image, wait for 6 seconds, then deactivate
        images[4].SetActive(true);
        yield return new WaitForSeconds(6f); // Wait for 6 seconds
        images[4].SetActive(false);

        // Activate the sixth image, wait for 15 seconds
        images[5].SetActive(true);
        yield return new WaitForSeconds(15f); // Wait for 15 seconds
        images[5].SetActive(false);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
    }
}