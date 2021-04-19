﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System;

public class GameManager : Manager<GameManager>
{
    public enum GameState
    {
        MENU,
        OVERWORLD,
        BATTLE
    }

    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject overworldManager;
    [SerializeField] private GameObject battleManager;
    //public GameObject overworldMusic;
    //public GameObject battleMusic;

    public Board[] boards;
    public Player[] playerChars;
    public Enemy[] enemyChars;

    private GameState state;

    public void Start()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        state = GameState.BATTLE;
        battleManager.SetActive(true);
        BattleManager.Instance.StartBattle(boards, playerChars, enemyChars);
    }

    public void MainMenu()
    {
        Debug.Log("Main Menu");
    }

    public void timeManip(float time)
    {
        StartCoroutine(timeManipCoroutine(time));
    }
    public IEnumerator timeManipCoroutine(float time)
    {
        Debug.Log("stop");
        Time.timeScale = 0.001f;

        yield return new WaitForSeconds(time * Time.timeScale);

        Time.timeScale = 1;
        Debug.Log("resume");
    }

    private void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            StartBattle();
        }
    }
}
