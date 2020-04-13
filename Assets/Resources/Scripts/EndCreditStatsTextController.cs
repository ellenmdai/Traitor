using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndCreditStatsTextController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BlackFade.instance.isFadeOutComplete())
        {
            int secondsElapsed = Epoch.SecondsElapsed(GameStats.EndTime, GameStats.StartTime);
            int minutesElapsed = secondsElapsed / 60;
            secondsElapsed = secondsElapsed - (minutesElapsed * 60);
            string minutesString = minutesElapsed.ToString();
            string secondsString = secondsElapsed.ToString();
            if(secondsElapsed < 10)
            {
                secondsString = "0" + secondsString;
            }
            string totalDeaths = GameStats.TotalDeaths.ToString();

            if (GameStats.StartTime == 0)
            {
                //well then they didn't start on level 0, or something went wrong
                //don't really know how to deal with this tbh
                //for now just gonna show 0:00 time
                minutesString = "0";
                secondsString = "00";

            }

            gameObject.GetComponent<Text>().text = "You finished in " + minutesString + ":" + secondsString +
                                            '\n' + "With "+ totalDeaths +" Deaths";

            
        }
        
    }
}
