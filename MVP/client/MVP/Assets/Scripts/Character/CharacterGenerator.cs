using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject characterPrefab;
    private List<Character> characters;
    private List<Color> colors = new List<Color>{Color.magenta, Color.yellow};

    private Class[] assets;

    private void Start() {
        characters = new List<Character>();
        assets = Resources.LoadAll<Class>("Class");
    }

    private Character CreateCharacter(int x, int z, string className, string playerId,Color color)
    {
        Character c = Instantiate(characterPrefab, new Vector3(x, 0.75f, z), Quaternion.identity).GetComponent<Character>();
        c.SetClass(assets.First(c => c.className.Equals(className)));
        c.SetPlayer(playerId);
        c.GetComponent<Renderer>().material.color = color;
        return c;
    }

    public List<Character> GenerateCharacters(List<(int x, int z)> positions, List<string> classNameList, List<string> idList)
    {
        for (int i = 0; i < positions.Count; i++)
            characters.Add(CreateCharacter(positions[i].x, positions[i].z, classNameList[i], idList[i%2], colors[i%2]));

        return characters;
    }
}
