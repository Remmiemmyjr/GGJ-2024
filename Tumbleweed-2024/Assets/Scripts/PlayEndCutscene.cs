using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayEndCutscene : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer outroCutscene;
    [SerializeField]
    private GameObject cutsceneBorder;

    private void OnTriggerEnter(Collider other)
    {
        // Ignore objects other than the player
        if (other.tag != "Player")
            return;

        outroCutscene.Play();
        cutsceneBorder.SetActive(true);
        StartCoroutine(OutroCutsceneSequence());
    }

    private IEnumerator OutroCutsceneSequence()
    {
        yield return new WaitForSeconds((float)outroCutscene.length);
        outroCutscene.gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
