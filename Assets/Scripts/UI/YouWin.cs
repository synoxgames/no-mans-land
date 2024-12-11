using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWin : MonoBehaviour
{

   private float currentTime = 0f;
   private float targetTime = 3f;


     // Update is called once per frame
   void Update () {
      if(Input.anyKey && currentTime > targetTime) {
         // Go back to main menu
         SceneManager.LoadScene("MainMenu");      
      }
      currentTime += Time.deltaTime;
   }


   // Display game over message
   void OnGUI() {
      
        // Show player score in white on the top left of the screen
        GUI.color = Color.white;   
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.fontSize = 40;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(0,Screen.height/ 4f,Screen.width,70), "You Win");

        string message;
      
        //The lost message will be shown in red
        message = "To be continued";
        // Show lost/won message
        GUI.Label(new Rect(0,Screen.height/ 4f + 80f,Screen.width,70), message);

        // Last line will be shown in white
        GUI.color = Color.white;
        GUI.Label(new Rect(0,Screen.height/ 4f + 240f,Screen.width,70), "Press any key to return to Main Menu");
        GUI.Label(new Rect(0,Screen.height/ 4f + 290f,Screen.width,70), "for now as will polish more later");
   }
}
