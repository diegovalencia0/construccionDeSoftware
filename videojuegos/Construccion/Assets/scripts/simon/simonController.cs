// Creating the main functions for simon game works
// Diego Valencia Moreno
// 2024-05-03

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimonController : MonoBehaviour
{
    [SerializeField] List<SimonButton> buttons;
    [SerializeField] List<int> sequence;
    [SerializeField] float delay;
    [SerializeField] int level;
    [SerializeField] bool playerTurn = false;
    [SerializeField] int maxTurnsCompleted = 0;
    [SerializeField] int counter = 0;
    [SerializeField] int numButtons;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform buttonParent;
    public AudioClip gameOverSound;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI maxTurnsText;

    void Start()
    {
        PrepareButtons();
    }

    void PrepareButtons()
    {
        for (int i = 0; i < numButtons; i++)
        {
            int index = i;
            GameObject newButton = Instantiate(buttonPrefab, buttonParent);
            newButton.GetComponent<Image>().color = Color.HSVToRGB((float)index / numButtons, 1, 1);
            newButton.GetComponent<SimonButton>().Init(index);
            buttons.Add(newButton.GetComponent<SimonButton>());
            buttons[i].gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonPressed(index));
        }
        AddToSequence();
    }

    public void ButtonPressed(int index)
    {
        if (playerTurn)
        {
            if (index == sequence[counter])
            {
                buttons[index].Highlight();
                counter++;
                if (counter == sequence.Count)
                {
                    playerTurn = false;
                    level++;
                    counter = 0;
                    if (level > maxTurnsCompleted)
                    {
                        maxTurnsCompleted = level;
                        UpdateMaxTurnsText();
                    }
                    StartCoroutine(WaitAndAddNewSequence());
                }
            }
            else
            {
                Debug.Log("Game Over!");
                PlayGameOverSound();
                StartCoroutine(RestartGameAfterDelay(6f));
            }
        }
    }

    void AddToSequence()
    {
        sequence.Add(Random.Range(0, buttons.Count));
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        UpdateLevelText();
        yield return new WaitForSeconds(delay);

        foreach (int index in sequence)
        {
            buttons[index].Highlight();
            yield return new WaitForSeconds(delay);
            yield return new WaitForSeconds(1f);
        }

        playerTurn = true;
    }

    IEnumerator WaitAndAddNewSequence()
    {
        yield return new WaitForSeconds(2);
        AddToSequence();
    }

    void PlayGameOverSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameOverSound;
        audioSource.Play();
        
    }

    IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestartGame();
    }

    void RestartGame()
    {
        playerTurn = false;
        counter = 0;
        sequence.Clear();
        level = 0;
        AddToSequence();
    }

    void UpdateLevelText()
    {
        levelText.text = "Level: " + (level+1).ToString();
    }

    void UpdateMaxTurnsText()
    {
        maxTurnsText.text = "Highest score: " + (maxTurnsCompleted).ToString();
    }
}
