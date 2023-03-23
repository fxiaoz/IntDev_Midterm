using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool goal1Reached;
    public bool goal2Reached;

    public TextMeshProUGUI myText1;
    public TextMeshProUGUI myText2;
    public int score1;
    public int score2;

    //public bool leaf1Collect;
    //public bool leaf2Collect;

    public GameObject area1Score;
    public GameObject area2Score;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        myText1.text = "Leaves Collected: 0";
        //myText2.text = "Area 2 Leaves: 0";

        //area2Score.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        myText1.text = "Leaves Collected: " + score1;
        myText2.text = "Leaves Collected: " + score2;

        if (score1 == 3)
        {
            goal1Reached = true;
        }

        if (goal1Reached == true)
        {
            SceneManager.LoadScene("Scene 2");
            //area1Score.SetActive(false);
            //area2Score.SetActive(true);
        }

        if (score2 == 7)
        {
            goal2Reached = true;
        }

        if (goal2Reached == true)
        {
            SceneManager.LoadScene("Win Screen");
        }

        if (player.transform.position.y <= -15)
        {
            SceneManager.LoadScene("Lose Screen");
        }
    }

    public void buttonClick()
    {
        SceneManager.LoadScene("Scene 1");
    }
}
