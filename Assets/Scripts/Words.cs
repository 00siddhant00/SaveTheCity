using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;


public class Words : MonoBehaviour
{
    public TMP_Text wordDisplay;
    private string word;
    [SerializeField] private string[] story;
    private string[] storyIndex;
    private string altword;
    private int storyLine;
    private int currentLetterIndex;
    private List<bool> result = new List<bool>();
    public GameObject Bomb;
    public ParticleSystem Fx;

    void Start()
    {
        Fx = Fx.GetComponent<ParticleSystem>();
        Time.timeScale = 1;
        var currentStory = Random.Range(0, story.Length);
        storyIndex = story[currentStory].Split('|');
        result.Clear();
        storyLine = 0;
        word = storyIndex[storyLine];
        altword = word;
        currentLetterIndex = 0;
        wordDisplay.text = word;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Initialize()
    {
        
    }

    void Update()
    {
        Bomb.transform.position -= new Vector3(0, 1 * Time.deltaTime, 0);
        if (Bomb.transform.position.y < -2.5)
        {
            Bomb.transform.position = new Vector3(0, 4, 0);
            //Time.timeScale = 0;
            Bomb.SetActive(false);
            Fx.Play();
        }
        if(Time.timeScale != 0)
        {
            Type();
        }

        //Type();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestartGame();
        }
    }

    void Type()
    {
        if (Input.inputString.Length > 0)
        {
            string input = Input.inputString[0].ToString();
            if (input == word[currentLetterIndex].ToString())
            {
                Debug.Log("Correct!");
                result.Add(true);
                altword = ReplaceLetter(word);
                currentLetterIndex++;
                Bomb.transform.position += new Vector3(0, 0.3f, 0);
                if (currentLetterIndex >= word.Length - 1)
                {
                    result.Clear();
                    Debug.Log("You have typed the word correctly!");
                    storyLine++;
                    word = storyIndex[storyLine];
                    currentLetterIndex = 0;
                }
            }
            else
            {
                Debug.Log("Incorrect.");
                result.Add(false);
                altword = ReplaceLetter(word);
                currentLetterIndex++;
                Bomb.transform.position -= new Vector3(0, 0.1f, 0);
                if (currentLetterIndex >= word.Length - 1)
                {
                    result.Clear();
                    Debug.Log("everythings wrong");
                    storyLine++;
                    word = storyIndex[storyLine];
                    currentLetterIndex = 0;
                }
            }
            wordDisplay.text = altword;
        }
    }

    private string ReplaceLetter(string input)
    {
        char[] inputArray = input.ToCharArray();
        string[] text = ConvertToStringArray(inputArray);
        for (int i = 0; i < result.Count; i++)
        {
            switch (result[i])
            {
                case true:
                    text[i] = "<color=green>" + word[i] + "</color>";
                break;

                case false:
                    text[i] = "<color=red>" + word[i] + "</color>";
                break;
            }
        }
        return new string(JoinStringArray(text));
    }

    private string[] ConvertToStringArray(char[] input)
    {
        return input.Select(c => c.ToString()).ToArray();
    }

    private string JoinStringArray(string[] inputArray)
    {
        return string.Join("", inputArray);
    }
}
