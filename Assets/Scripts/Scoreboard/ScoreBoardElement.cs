using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardElement : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color currentEntryColor;
    [SerializeField] private TextMeshProUGUI tmpRank;
    [SerializeField] private TextMeshProUGUI tmpName;
    [SerializeField] private TextMeshProUGUI tmpTime;
    [SerializeField] private TextMeshProUGUI tmpWave;
    [SerializeField] private TextMeshProUGUI tmpScore;
    private TimeSpan time;

    public void SetStats(Scoreboard.ScoreBoardValues values)
    {
        string timeString =  values.time;
        tmpTime.text = timeString;
        tmpWave.text = values.waves.ToString();
        tmpScore.text = values.score.ToString();
        tmpName.text = values.name;
    }

    public void SetStats(string time, int wave, int score)
    {
        tmpTime.text = time;
        tmpWave.text = wave.ToString();
        tmpScore.text = score.ToString();
    }

    public Scoreboard.ScoreBoardValues GetScoreBoardValues()
    {
        Scoreboard.ScoreBoardValues values = new Scoreboard.ScoreBoardValues(GetName(),GetScore(),GetWaves(),GetTime());
        return values;
    }

    public void SetAsActive()
    {
        image.color = currentEntryColor;
    }

    public void SetRank(int rank)
    {
        string newRank = "";
        switch(rank)
        {
            case 0:     newRank = "1st";    break;
            case 1:     newRank = "2nd";    break;
            case 2:     newRank = "3rd";    break;
            default:    newRank = (rank+1).ToString() + "th"; break;
        }
        tmpRank.text = newRank;
    }

    public int GetRank()
    {
        int rank = 0;
        string onlyNumber = tmpRank.text.Substring(0,1);
        int.TryParse(onlyNumber, out rank);
        return rank;
    }

    public int GetWaves()
    {
        int waves = 0;
        int.TryParse(tmpWave.text, out waves);
        return waves;
    }

    public string GetTime()
    {
        return tmpTime.text;
    }

    public int GetScore()
    {
        int score = 0;
        int.TryParse(tmpScore.text, out score);
        return score;        
    }

    public void SetScore(int value)
    {
        tmpScore.text = value.ToString();
    }

    public void SetName(string name) 
    {
        tmpName.text = name;
    }

    public string GetName()
    {
        return tmpName.text;
    }
}
