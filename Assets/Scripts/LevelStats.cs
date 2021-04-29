using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelStats
{
    private static float health, totalHealth;
    private static int items, switches, checkpoints, deaths;
    private static bool levelOne, levelTwo, levelThree, firstSpawn;

    public static float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public static float TotalHealth
    {
        get
        {
            return totalHealth;
        }
        set
        {
            totalHealth = value;
        }
    }

    public static int Items
    {
        get
        {
            return items;
        }
        set
        {
            items = value;
        }
    }
    public static int Switches
    {
        get
        {
            return switches;
        }
        set
        {
            switches = value;
        }
    }
    public static int Checkpoints
    {
        get
        {
            return checkpoints;
        }
        set
        {
            checkpoints = value;
        }
    }
    public static int Deaths
    {
        get
        {
            return deaths;
        }
        set
        {
            deaths = value;
        }
    }
    public static bool LevelOneCompleted
    {
        get
        {
            return levelOne;
        }
        set
        {
            levelOne = value;
        }
    }

    public static bool LevelTwoCompleted
    {
        get
        {
            return levelTwo;
        }
        set
        {
            levelTwo = value;
        }
    }

    public static bool LevelThreeCompleted
    {
        get
        {
            return levelThree;
        }
        set
        {
            levelThree = value;
        }
    }
    public static bool FirstSpawn
    {
        get
        {
            return firstSpawn;
        }
        set
        {
            firstSpawn = value;
        }
    }

}
