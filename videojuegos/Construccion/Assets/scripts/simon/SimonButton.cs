// Creating the button functions for simon game
// Diego Valencia Moreno
// 2024-05-03

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonButton : MonoBehaviour
{
    [SerializeField] float delay;
    Color originalColor;
    AudioSource audio;

    public void Init(int index)
    {
        originalColor = GetComponent<Image>().color;
        audio = GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>($"Audio/{index + 1}");
    }

    public void Highlight()
    {
        audio.Play();
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(delay);
        GetComponent<Image>().color = originalColor;
    }
}
