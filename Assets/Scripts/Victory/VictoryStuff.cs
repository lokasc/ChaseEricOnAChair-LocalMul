using UnityEngine;

public class VictoryStuff : MonoBehaviour
{
    public Transform modelSlot;
    public Material goldMaterial;
    public GameObject spinStuff;
    public float spinStuffSpeed;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SpawnWinningModel();
    }

    void Update()
    {
        if (spinStuff != null)
            spinStuff.transform.Rotate(0f, spinStuffSpeed * Time.deltaTime, 0f);

        if (modelSlot != null)
            modelSlot.Rotate(0f, spinStuffSpeed * Time.deltaTime, 0f);
    }

    private void SpawnWinningModel()
    {
        var pm = gameManager.winningPlayer;
        var obj = Instantiate(pm.characterModel, modelSlot.position, modelSlot.rotation, modelSlot);

        var renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
            r.material = goldMaterial;
    }
}
