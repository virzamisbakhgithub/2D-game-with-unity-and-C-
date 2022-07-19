using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPortal : MonoBehaviour
{
    [SerializeField] private string namaLevel;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            SceneManager.LoadScene(namaLevel);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
