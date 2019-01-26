﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text scoreText;

    public int score;

    public static GameController Instance;

    private void Start()
    {
        Instance = this;
    }

    public void AddScore(int pointsToAdd)
    {
        score += pointsToAdd;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
