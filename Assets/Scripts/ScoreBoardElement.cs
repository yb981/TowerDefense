using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpRank;
    [SerializeField] private TextMeshProUGUI tmpName;
    [SerializeField] private TextMeshProUGUI tmpTime;
    [SerializeField] private TextMeshProUGUI tmpWave;
    [SerializeField] private TextMeshProUGUI tmpScore;

    public void SetStats(int rank, string name, string time, int wave, int score)
    {
        tmpRank.text = rank.ToString();
        tmpRank.text = name;
        tmpRank.text = time;
        tmpRank.text = wave.ToString();
        tmpRank.text = score.ToString();
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
}
