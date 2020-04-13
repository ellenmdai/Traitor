using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://gamedev.stackexchange.com/questions/110958/what-is-the-proper-way-to-handle-data-between-scenes
// for tracking player deaths, 
public class GameStats
{
    private static int levelDeaths = 0, totalDeaths = 0, startTime = 0, endTime = 1;

    public static int LevelDeaths {
        get {
            return levelDeaths;
        }
        set {
            levelDeaths = value;
        }
    }

    public static int TotalDeaths {
        get {
            return totalDeaths;
        }
        set {
            totalDeaths = value;
        }
    }

    public static int StartTime {
        get {
            return startTime;
        }
        set {
            startTime = value;
        }
    }

    public static int EndTime
    {
        get
        {
            return endTime;
        }
        set
        {
            endTime = value;
        }
    }
}
