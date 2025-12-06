using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneTransferTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Map1", LoadSceneMode.Additive);
        }
    }



    public void OnAddPlayer(PlayerInput playerInput)
    {
        DontDestroyOnLoad(playerInput.gameObject);
    }
}
