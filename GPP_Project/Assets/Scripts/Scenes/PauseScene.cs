using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScene : Scene<TransitionData> {
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            this.gameObject.SetActive(false);
            Services.Scenes.CurrentScene.gameObject.SetActive(true);
        }
    }
}
