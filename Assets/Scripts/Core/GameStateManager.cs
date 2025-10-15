using UnityEngine;
using System.Collections.Generic;
using BasketLegend.Core;

namespace BasketLegend.Core
{
    public class GameStateManager : MonoBehaviour
    {
        private Dictionary<string, IGameState> gameStates;
        private IGameState currentState;

        private void Awake()
        {
            gameStates = new Dictionary<string, IGameState>();
            InitializeStates();
        }

        private void InitializeStates()
        {
            gameStates.Add("MainMenu", new MainMenuState());
            gameStates.Add("Settings", new SettingsState());
            gameStates.Add("LevelSelect", new LevelSelectState());
            gameStates.Add("Gameplay", new GameplayState());
        }

        public void ChangeState(string stateName)
        {
            if (gameStates.ContainsKey(stateName))
            {
                currentState?.Exit();
                currentState = gameStates[stateName];
                currentState.Enter();
            }
            else
            {
                Debug.LogWarning($"State '{stateName}' not found!");
            }
        }

        private void Update()
        {
            currentState?.Update();
        }

        public void StartGame()
        {
            ChangeState("MainMenu");
        }
    }
}