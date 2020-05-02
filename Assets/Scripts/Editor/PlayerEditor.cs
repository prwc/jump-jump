using UnityEditor;
using UnityEngine;

public static class PlayerEditor
{
    [MenuItem("Player/Clear High Score")]
    private static void ClearHighScore()
    {
        PlayerPrefs.DeleteKey(Player.HighScorePlayerPref);
    }
}
