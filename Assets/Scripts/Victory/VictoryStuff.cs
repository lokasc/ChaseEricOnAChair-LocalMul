using UnityEngine;

public class VictoryStuff : MonoBehaviour
{
    public Transform modelSlot;
    public Material goldMaterial;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SpawnWinningModel();
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
