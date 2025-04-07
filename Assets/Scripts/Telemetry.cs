using System;
using System.IO;
using UnityEngine;

public class Telemetry : MonoBehaviour
{
    ChronoTimer globalChrono = new ChronoTimer();
    ChronoTimer itemDrawerChrono = new ChronoTimer();
    int itemDrawerOpenCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        globalChrono.StartChrono();
        itemDrawerChrono.StartChrono();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData()
    {
        globalChrono.StopChrono();
        string outData = "";

        GameManager gameManager = GameObject.FindFirstObjectByType<GameManager>();
        if(gameManager)
        {
            outData += ("level;" + GameManager.levelIndex + "\n");
            outData += ("ballThrow;" + gameManager.scoreManager.throwsNbText.text + "\n");
            outData += ("totalPin;" + gameManager.scoreManager.totalPinsNbText.text + "\n");
            outData += ("fallenPin;" + gameManager.scoreManager.fallenPinsNbText.text + "\n");
        }
        outData += ("time;" + globalChrono.elapsedTime + "\n");
        outData += ("timeToOpenItemDrawer;" + itemDrawerChrono.elapsedTime + "\n");
        outData += ("ItemDrawerOpenCount;" + Mathf.Ceil(itemDrawerOpenCount / 2f) + "\n");

        GameObject[] ressources = GameObject.FindGameObjectsWithTag("Ressource");

        outData += ("ObjectPlaced;" + ressources.Length + "\n");
        foreach (GameObject ressource in ressources)
        {
            outData += ("ressource;" + ressource.name + "\n");
            outData += (ressource.name + "_grabCount;" + ressource.GetComponent<Ressource>().grabCount + "\n");
            outData += (ressource.name + "_transform_position_xyz;" + ressource.transform.position.x + 
                ";" + ressource.transform.position.y + ";" + ressource.transform.position.z + "\n");
            outData += (ressource.name + "_transform_rotation_xyz;" + ressource.transform.rotation.x + 
                ";" + ressource.transform.rotation.y + ";" + ressource.transform.rotation.z + "\n");
            outData += (ressource.name + "_transform_scale_xyz;" + ressource.transform.localScale.x + 
                ";" + ressource.transform.localScale.y + ";" + ressource.transform.localScale.z + "\n");

        }
        string time = System.DateTime.Now.ToString();
        time = time.Replace('/', '_');
        time = time.Replace(' ', '_');
        time = time.Replace(':', '_');
        string file = "/data_" + time + ".txt";
        string dir = System.IO.Directory.GetParent(Application.dataPath).FullName + "/telemetry";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(dir + '/' + file, outData);
    }

    public void SetItemDrawerOpen()
    {
        itemDrawerOpenCount++;
        if (itemDrawerChrono.isRunning)
        {
            itemDrawerChrono.StopChrono();
        }
    }
}
