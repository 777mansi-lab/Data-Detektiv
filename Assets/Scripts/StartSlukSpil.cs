using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSlukSpil : MonoBehaviour
{
   public AudioSource lyd;
   public void StartSpil()
    {
        lyd.Play();
        Invoke("LoadScene", lyd.clip.length); //Invoke kører flere kommandoer i rækkefølge, den her vil så køre LoadScene efter længden af lydklippet er spillet
    }
    public void LoadScene()
   {
    SceneManager.LoadScene("Game");
   }

    public void slutSpil() //Kommandoen der står i # er fra det tidligere projekt og blev fundet af chatGPT der. Den normale metode virker kun inde i Unity_Editor og den i # virker når spillet er færdig som .exe
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
