using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public TrailController TrailController;
    public List<Bird> Birds;
    public List<Enemy> Enemies;
    private Bird _shotBird;
    public BoxCollider2D TapCollider;
    public Text status;

    private bool _isGameEnded = false;
    private bool _win = false;

    private void Update()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(_isGameEnded){
            status.gameObject.SetActive(true);
            ChangeStatusText();

            if(_win && SceneManager.GetActiveScene().name == "Level 2"){
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SceneManager.LoadScene(activeSceneName = "Level 1");
                }
            } else if(_win && SceneManager.GetActiveScene().name == "Level 1") {
                if (Input.GetKeyDown(KeyCode.Return))
                {   
                    SceneManager.LoadScene(activeSceneName = "Level 2");
                }
            } else{
                if (Input.GetKeyDown(KeyCode.Return))
                {   
                    SceneManager.LoadScene(activeSceneName);
                }
            }
            // if (Input.GetKeyDown(KeyCode.Return))
            // {

            // if (_win)
            //     SceneManager.LoadScene(activeSceneName == "Level 1" ? "Level 2" : "Level 1");
            // else
            //     SceneManager.LoadScene(activeSceneName);
            // }
        }
        
    }

    void Start()
    {   
        for(int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        for(int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }
        TapCollider.enabled = false;
        SlingShooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];
    }

    public void ChangeBird()
    {
        if(!TapCollider){
            return;
        }

        TapCollider.enabled = false;
        if (_isGameEnded)
        {
            return;
        }

        Birds.RemoveAt(0);

        if(Birds.Count > 0){
            SlingShooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }
        else{
            _isGameEnded = true;
        }
    }

     public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for(int i = 0; i < Enemies.Count; i++)
        {
            if(Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }

        if(Enemies.Count == 0)
        {
            _win = true;
            _isGameEnded = true;
        }
    }

    public void AssignTrail(Bird bird)
    {
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;
    }

    void OnMouseUp()
    {
        if(_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }

    private void ChangeStatusText()
    {       
        if (_win)
            status.text = $"{SceneManager.GetActiveScene().name} Completed!\nPress[Enter] to go to the next level";
        else
            status.text = "Try Again!\nPress [Enter] to restart this level";
    }
}