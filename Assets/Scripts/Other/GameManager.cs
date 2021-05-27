using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class GameManager
{
    public static bool PlayerControls = true;

    public static GameObject PlayerCharacter;

    public static Player scriptPlayer;
    public static PlayerMovement scriptMovement;
    public static PlayerActions scriptActions;
    public static Cursor scriptCursor;
    public static CanvasManager scriptCanvas;
    public static Hud scriptHud;

    public static Animator BossHud;
    public static Animator Overlay;

    public static Arena arena;

    public static AudioSource musicSource;
    public static float VolumeSfx = 0.5f;
    public static float VolumeBgm = 0.25f;

    public static float currentTime;
    public static float bestTime;
    public static bool timerPaused;

    public static void ChangeOverlay(int state)
    {
        Overlay.SetInteger("state",state);

    }

    public static void AnimationOverlay(string animation)
    {
        Overlay.Play(animation);
    }

    public static void ReloadScene()
    {
        GameManager.PlayerControls = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}
