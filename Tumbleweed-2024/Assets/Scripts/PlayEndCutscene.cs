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
    [SerializeField]
    private GameObject renderImg;
    [SerializeField]
    private GameObject AUDIOMAIN;

    private void Start()
    {
        AUDIOMAIN.SetActive(true);
        cutsceneBorder.SetActive(false);
        outroCutscene.Stop();
        renderImg.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore objects other than the player
        if (other.tag != "Player")
            return;

        renderImg.SetActive(true);
        outroCutscene.Play();
        cutsceneBorder.SetActive(true);
        AUDIOMAIN.SetActive(false);
        StartCoroutine(OutroCutsceneSequence());
    }

    private IEnumerator OutroCutsceneSequence()
    {
        yield return new WaitForSeconds((float)outroCutscene.length);
        outroCutscene.gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
