using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovementAction : Action
{
    public float moveSpeed = 2f;
    private Character character;
    private Stack<Tile> path;
    private CharacterMessage message;

    protected void Start() {
        character = GetComponent<Character>();
    }

    public void GetSelectableTiles(bool canSelect)
    {
        if(!canSelect || IsExecuting() ) return;
        GetSelectableTiles();
    }
    public override void GetSelectableTiles()
    {
        map.ResetHighlight();
        selectableTiles = map.MovingBFS(GetCurrentTile(), character.currentMovementPoint);
        HighlightTiles(selectableTiles, Color.green);
        Execute();
    }


    private IEnumerator Move(Stack<Tile> path)
    {
        while(path.Count > 0){
            GetCurrentTile().player = null;
            Tile tile = path.Peek();
            Vector3 target = tile.ground.transform.position;
            target.y += character.GetComponent<Collider>().bounds.extents.y + tile.ground.GetComponent<Collider>().bounds.extents.y;
            if(Vector3.Distance(character.transform.position, target) >= 0.05f)
            {
                Vector3 heading = target-character.transform.position;
                heading.Normalize();
                Vector3 velocity = heading*moveSpeed;
                character.transform.forward = heading;
                character.transform.position += velocity * Time.deltaTime;
                yield return new WaitForSeconds(1);
            }else
            {   
                character.transform.position = target;
                GetCurrentTile().player = character.gameObject;
                path.Pop();
                yield return null;
            }
        }
    }

    protected override void SetUpExecution(Tile target){
        Stack<Tile> tempPath = map.AStarSearch(GetCurrentTile(), target);
        HighlightTiles(selectableTiles, Color.green);
        HighlightTiles(tempPath, Color.grey);
        if(Input.GetMouseButtonUp(0))
        {
            character.currentMovementPoint -= tempPath.Count();
            HighlightTiles(selectableTiles, Tile.NORMAL_COLOR);
            client.SendAction(new CharacterMessage(GetCurrentTile().x, target.x, GetCurrentTile().z, target.z, character.currentLifePoints));
            path = tempPath;
        }
    }

    public void SetUp(int x, int z)
    {
        path = map.AStarSearch(GetCurrentTile(), map.GetTile(x,z));
    }

    private void Update() {
        if(path == null) return;
        StartCoroutine(Move(path));
    }
    public bool IsExecuting() => path != null && path.Count > 0;
    protected override bool IsSelectedTileValid(Tile tile) => selectableTiles != null && tile.IsWalkable() && selectableTiles.Contains(tile);
}
