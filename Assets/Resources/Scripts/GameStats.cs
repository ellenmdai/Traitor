using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://gamedev.stackexchange.com/questions/110958/what-is-the-proper-way-to-handle-data-between-scenes
// for tracking player deaths, 
public class GameStats
{
    private static int levelDeaths = 0, totalDeaths = 0;

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
}
