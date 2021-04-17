using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelStats
{

    private static int items, switches, checkpoints;
    private static bool levelOne, levelTwo, levelThree;

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
    
}
