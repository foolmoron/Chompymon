using System;
using UnityEngine;
using System.Collections;
using System.IO;
using Crosstales.FB;
using UnityEngine.SceneManagement;

public class DemonManager : Manager<DemonManager> {

    public Demon CurrentDemon;

    public string MonScene = "Mon";

    public void NewDemon() {
        StartGameWithDemon(Demon.CreateDemon());
    }

    public void LoadDemon() {
        try {
            var file = FileBrowser.OpenSingleFile("Summon Chompymon", Application.persistentDataPath, "chompymon");
            var demon = Demon.Load(new FileInfo(file));
            if (demon == null) {
                Debug.LogError("Invalid Chompymon!!");
            } else {
                StartGameWithDemon(demon);
            }
        } catch (Exception e) {
            Debug.LogError("Invalid file selected!!");
        }
    }

    public void StartGameWithDemon(Demon d) {
        CurrentDemon = d;
        SceneManager.LoadScene(MonScene);
    }

    void Update() {
        Debug.Log(CurrentDemon?.Name);
    }
}
