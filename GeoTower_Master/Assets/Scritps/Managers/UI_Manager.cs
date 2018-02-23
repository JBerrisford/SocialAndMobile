using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Manager : Singleton_Base<UI_Manager>
{
    public GameObject mainMenu;
    public GameObject levelSelect;

    public Text healthText;
    public Text moneyText;
    public Text waveText;

    public GameObject UI;
    public ErrorWarning errorMsg;

    public GameObject interactionPanel;
    public GameObject towerPanel;
    public GameObject detailsPanel;
    public GameObject wavePanel;
    public GameObject waveDetails;
    public Dictionary<string, Button> upgradeButtons;

    public GameObject gameOver;
    public Text gameOverText;

    public Button startWaveBtn;
    public Button pathBtn;

	public override void Init ()
	{
		base.Init ();
        errorMsg.Init();

        upgradeButtons = new Dictionary<string, Button>();
        Button[] tempBTN = interactionPanel.GetComponentsInChildren<Button>();

        foreach (Button temp in tempBTN)
            upgradeButtons.Add(temp.gameObject.name, temp);

        UpdateTowerUI(null);
    }

    public void ToggleOff()
    {
        interactionPanel.SetActive(false);
        towerPanel.SetActive(false);
        detailsPanel.SetActive(false);
        wavePanel.SetActive(false);
        gameOver.SetActive(false);
        waveDetails.SetActive(false);
    }

    public void GameOver(bool playerWon)
    {
        ToggleOff();
        
        if(playerWon)
        {
            gameOverText.text = "You Win!";
        }
        else
        {
            gameOverText.text = "You ran out of Lives!";
        }

        gameOver.SetActive(true);
    }

    public void ToggleWaveDetails()
    {
        if (waveDetails.activeInHierarchy)
        {
            waveDetails.SetActive(false);
        }
        else
        {
            waveDetails.SetActive(true);
        }
    }

    public void ReturnToMain()
    {
        GameManager.Instance.NewInGameState(CS_Enum.IN_GAME_STATE.NOT_ACTIVE);
        GameManager.Instance.NewGameState(CS_Enum.GAME_STATE.MAIN_MENU);
    }

    public void UpgradeTower(string buttonKey)
    {
        Player_Manager.Instance.UpgradeTower(buttonKey);
    }

    public void UpdateTowerUI(Tower_Base tb)
    {
        if(tb == null)
        {
            foreach (KeyValuePair <string, Button> temp in upgradeButtons)
            {
                temp.Value.gameObject.SetActive(false);
            }
        }
        else
        {
            for(int i = 0; i < tb.Upgrades.Count; i++)
            {
                if (i == 0)
                {
                    upgradeButtons["DestroyTower"].gameObject.SetActive(true);
                }
                else
                {
                    upgradeButtons["Upgrade " + i].gameObject.SetActive(true);
                    upgradeButtons["Upgrade " + i].transform.Find("Text").GetComponent<Text>().text = tb.GetUpgradeName(i - 1);
                }
            }
        }
    }

    public void ErrorMsg(string msg)
    {
        errorMsg.StartError(msg);
    }

	public void TowerSelect(string towerSelect)
	{
		GameObject go = (GameObject)Instantiate (Resources.Load("Prefabs/Towers/" + towerSelect), new Vector3(100,0,0), Quaternion.identity);
		Tower_Base tb = go.GetComponent<Tower_Base> ();
        tb.Init();

        if ((Player_Manager.Instance.Money + tb.Cost) >= 0)
        {
            Player_Manager.Instance.SetTowerToBuild(tb);
        }
        else
        {
            Destroy(go);
            ErrorMsg(CS_Strings.cantAffordThat);
        }
	}

    public void ToggleLevelSelectOn()
    {
        levelSelect.SetActive(true);
    }

    public void ToggleLevelSelectOff()
    {
        levelSelect.SetActive(true);
    }

    public void SetSceneToLoad(string sceneName)
    {
        GameManager.Instance.SetSceneToLoad(sceneName);
    }

    public void StartWave()
    {
        GameManager.Instance.NewInGameState(CS_Enum.IN_GAME_STATE.START_WAVE);
        startWaveBtn.interactable = false;
        pathBtn.interactable = false;
    }

    public override void BeginNewState()
    {
        switch(GameManager.Instance.GameState)
        {
            case CS_Enum.GAME_STATE.MAIN_MENU:
                mainMenu.SetActive(true);
                UI.SetActive(false);
                break;
            case CS_Enum.GAME_STATE.NEW:
                mainMenu.SetActive(false);
                break;
            case CS_Enum.GAME_STATE.LOAD:
                mainMenu.SetActive(false);
                UI.SetActive(true);
                break;
            case CS_Enum.GAME_STATE.SAVE:
               
                break;
            case CS_Enum.GAME_STATE.START_GAME:
                
                break;
            case CS_Enum.GAME_STATE.SETTINGS:
                
                break;
            case CS_Enum.GAME_STATE.CREDITS:
                
                break;
            case CS_Enum.GAME_STATE.GAME_OVER:
                
                break;
            case CS_Enum.GAME_STATE.IN_GAME:
                break;
            default:
                UI.SetActive(false);
                break;
        }
    }

    public override void InGameStateChange()
    {
        switch (GameManager.Instance.InGameState)
        {
            case CS_Enum.IN_GAME_STATE.NOT_ACTIVE:
                UI.SetActive(false);
                //Close all UI
                break;
            case CS_Enum.IN_GAME_STATE.STANDBY:
                ToggleOff();
                UI.SetActive(true);
                interactionPanel.SetActive(true);
                towerPanel.SetActive(true);
                detailsPanel.SetActive(true);
                wavePanel.SetActive(true);
                //Open Main UI
                break;
            case CS_Enum.IN_GAME_STATE.REST_PHASE:
                //Open wave preview UI
                break;
            case CS_Enum.IN_GAME_STATE.START_WAVE:
                //Close wave preview UI
                break;
            case CS_Enum.IN_GAME_STATE.END_WAVE:
                startWaveBtn.interactable = true;
                pathBtn.interactable = true;
                //Reset wave preview UI
                break;
            case CS_Enum.IN_GAME_STATE.PAUSED:
                ToggleOff();
                //Hide all UI
                break;
        }
    }

    public void NewGameState(string stateName)
    {
        CS_Enum.GAME_STATE newState = CS_Enum.GAME_STATE.MAIN_MENU;

        switch (stateName)
        {
            case "NEW":
                newState = CS_Enum.GAME_STATE.NEW;
                break;
            case "LOAD":
                newState = CS_Enum.GAME_STATE.LOAD;
                break;
            case "SETTINGS":
                newState = CS_Enum.GAME_STATE.SETTINGS;
                break;
            case "CREDITS":
                newState = CS_Enum.GAME_STATE.CREDITS;
                break;
            case "EXIT":
                newState = CS_Enum.GAME_STATE.EXIT;
                break;
        }

        GameManager.Instance.NewGameState(newState);
    }

    private void OnGUI() // <--- DEBUG
    {
        healthText.text = Player_Manager.Instance.Health.ToString();
        moneyText.text = Player_Manager.Instance.Money.ToString();
        waveText.text = Wave_Manager.Instance.GetWaveCurrentMax;
    }
}
