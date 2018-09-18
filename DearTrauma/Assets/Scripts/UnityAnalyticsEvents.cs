using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class UnityAnalyticsEvents : MonoBehaviour {

    public void HelpNeeded(int count)
    {
        AnalyticsEvent.Custom("help_needed", new Dictionary<string, object>
        {
            { "help_count", count },
            { "time_elapsed", Time.timeSinceLevelLoad }
        });
    }

    public void QuitGame()
    {
        AnalyticsEvent.Custom("quit_game", new Dictionary<string, object>
        {
            { "time_elapsed", Time.timeSinceLevelLoad }
        });
    }

    public void FellIntoHole(int holeNumber)
    {
        AnalyticsEvent.Custom("fell_into_hole", new Dictionary<string, object>
        {
            { "hole_number", holeNumber },
            { "time_elapsed", Time.timeSinceLevelLoad }
        });
    }

    public void StartLevel(int levelNumber)
    {
        AnalyticsEvent.Custom("level_start", new Dictionary<string, object>
        {
            { "level_index", levelNumber },
            { "time_elapsed", Time.timeSinceLevelLoad }
        });
    }

    public void EndLevel(int levelNumber)
    {
        AnalyticsEvent.Custom("level_complete", new Dictionary<string, object>
        {
            { "level_index", levelNumber },
            { "time_elapsed", Time.timeSinceLevelLoad }
        });
    }

    public void EnemyKilledPlayer(int enemyNumber)
    {
        AnalyticsEvent.Custom("enemy_killed_player", new Dictionary<string, object>
        {
            { "enemy_index", enemyNumber },
            { "time_elapsed", Time.timeSinceLevelLoad }
        });
    }

    public void PlayerKilledEnemy(int enemyNumber)
    {
        AnalyticsEvent.Custom("player_killed_enemy", new Dictionary<string, object>
        {
            { "enemy_index", enemyNumber },
            { "time_elapsed", Time.timeSinceLevelLoad }
        });
    }
}
