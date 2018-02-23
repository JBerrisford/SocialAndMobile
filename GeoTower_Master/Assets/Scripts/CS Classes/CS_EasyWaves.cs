using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_EasyWaves
{
    public Dictionary<int, List<string>> Waves;
    public int wavesCurrent;
    public int wavesMax;

    public CS_EasyWaves()
    {
        Waves = new Dictionary<int, List<string>>
        {
            { 1, SetWave1() },
            { 2, SetWave2() },
            { 3, SetWave3() },
            { 4, SetWave4() },
            { 5, SetWave5() },
            { 6, SetWave6() },
            { 7, SetWave7() }
        };

        wavesCurrent = 1;
        wavesMax = 7;
    }

    List<string> SetWave1()
    {
        List<string> tempEnemies = new List<string>();

        for (int i = 0; i < 10; i++)
        {
            tempEnemies.Add("Prefabs/Enemies/Goblin");
        }

        return tempEnemies;
    }
    List<string> SetWave2()
    {
        List<string> tempEnemies = new List<string>();

        for (int i = 0; i < 15; i++)
        {
            tempEnemies.Add("Prefabs/Enemies/Wolf");
        }
        
        return tempEnemies;
    }
    List<string> SetWave3()
    {
        List<string> tempEnemies = new List<string>();

        bool flip = true;

        for (int i = 0; i < 9; i++)
        {
            if (flip)
            {
                tempEnemies.Add("Prefabs/Enemies/Goblin");
                tempEnemies.Add("Prefabs/Enemies/Wolf");
                flip = false;
            }
            else
            {
                tempEnemies.Add("Prefabs/Enemies/Wolf");
                tempEnemies.Add("Prefabs/Enemies/Goblin");
                flip = true;
            }
        }

        return tempEnemies;
    }
    List<string> SetWave4()
    {
        List<string> tempEnemies = new List<string>();
        bool flip = true;

        for (int i = 0; i < 9; i++)
        {
            if (flip)
            {
                tempEnemies.Add("Prefabs/Enemies/Goblin");
                tempEnemies.Add("Prefabs/Enemies/Orc");
                flip = false;
            }
            else
            {
                tempEnemies.Add("Prefabs/Enemies/Orc");
                tempEnemies.Add("Prefabs/Enemies/Goblin");
                flip = true;
            }
        }

        return tempEnemies;
    }
    List<string> SetWave5()
    {
        List<string> tempEnemies = new List<string>();

        bool flip = true;

        for (int i = 0; i < 12; i++)
        {
            if (flip)
            {
                tempEnemies.Add("Prefabs/Enemies/Orc");
                tempEnemies.Add("Prefabs/Enemies/Goblin");
                flip = false;
            }
            else
            {
                tempEnemies.Add("Prefabs/Enemies/Goblin");
                tempEnemies.Add("Prefabs/Enemies/Orc");
                flip = true;
            }
        }

        return tempEnemies;
    }
    List<string> SetWave6()
    {
        List<string> tempEnemies = new List<string>();

        for (int i = 0; i < 25; i++)
        {
            tempEnemies.Add("Prefabs/Enemies/Bat");
        }

        return tempEnemies;
    }
    List<string> SetWave7()
    {
        List<string> tempEnemies = new List<string>();

        bool flip = true;

        for (int i = 0; i < 16; i++)
        {
            if (flip)
            {
                tempEnemies.Add("Prefabs/Enemies/Bat");
                tempEnemies.Add("Prefabs/Enemies/Goblin");
                flip = false;
            }
            else
            {
                tempEnemies.Add("Prefabs/Enemies/Goblin");
                tempEnemies.Add("Prefabs/Enemies/Bat");
                flip = true;
            }

            if (i % 3 == 0)
            {
                tempEnemies.Add("Prefabs/Enemies/Orc");
            }
        }

        return tempEnemies;
    }

    public void ResetWaves()
    {
        wavesCurrent = 1;
    }
}
