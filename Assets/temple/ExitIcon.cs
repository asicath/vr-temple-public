using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitIcon : Activatable {

    public override void onActivate()
    {
        var light = GetComponentInChildren<Light>();
        light.color = Color.green;
        SceneManager.LoadSceneAsync("lobby");
    }

    public override void onEnter()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public override void onExit()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
