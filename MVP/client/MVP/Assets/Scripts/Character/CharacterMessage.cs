using System;

[Serializable]
public class CharacterMessage {

    public int destX;
      
    public int destZ;

    public int startX;
    public int startZ;
    
    public int lifePoints;


    public CharacterMessage()
    {

    }
    public CharacterMessage(int posX, int posZ, int lifePoints)
    {
        this.startX = posX;
        this.startZ = posZ;
        this.destX = posX;
        this.destZ = posZ;
        this.lifePoints = lifePoints;
    }

    public CharacterMessage(int startX, int destX, int startZ, int destZ, int lifePoints)
    {
        this.startX = startX;
        this.destX = destX;
        this.startZ = startZ;
        this.destZ = destZ;
        this.lifePoints = lifePoints;
    }

    public override string ToString() => "start :"+startX +","+startZ+" dest :"+destX+","+destZ+" lifepoint : "+lifePoints;
}