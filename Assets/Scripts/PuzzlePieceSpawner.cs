using UnityEngine;

public class PuzzlePieceSpawner : MonoBehaviour
{
    public GameObject[] puzzlePieces; 
    public Transform[] spawnPoints; 

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SpawnPiece(0); 

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SpawnPiece(1); 

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SpawnPiece(2); 

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SpawnPiece(3); 

        if (Input.GetKeyDown(KeyCode.Alpha5))
            SpawnPiece(4); 

        if (Input.GetKeyDown(KeyCode.Alpha6))
            SpawnPiece(5); 
    }

    void SpawnPiece(int index)
    {
        if (index < puzzlePieces.Length && index < spawnPoints.Length)
        {
            Instantiate(puzzlePieces[index], spawnPoints[index].position, Quaternion.identity);
        }
    }
}
