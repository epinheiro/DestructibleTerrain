using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    The Texture2D reads the texture from the bottom left corner, and the pixels go accordingly
    to the texture size. These pixel position isn't the position in the Unity World. 
 */

public class ShaderScenarioController : MonoBehaviour
{
    /////////////////////////////
    ///// Terrain Mask
    /////////////////////////////
    Texture2D TerrainMask;
    int terrainWidth;
    int terrainHeight;
    
    Texture2D GetTerrainMask(){
        TerrainMask = Resources.Load<Texture2D>("TerrainMask");
        terrainWidth = TerrainMask.width;
        terrainHeight = TerrainMask.height;

        return TerrainMask;
    }

    public bool isPositionFree(Vector3 position){
        return isPositionFree((int)position.x, (int)position.y);
    }

    public bool isPositionFree(int x, int y){
        Color pixelColor = GetPixelColorByUnityWorldSpace(x, y);

        if ( pixelColor == Color.black) return false;
        else return true;
    }

    public Color GetPixelColorByUnityWorldSpace(Vector3 position){
        return GetPixelColorByUnityWorldSpace(position.x, position.y);
    }

    public Color GetPixelColorByUnityWorldSpace(float x, float y){
        float normalizedX = x/spriteSize.x;
        float normalizedY = y/spriteSize.y;

        int pixelX = (int)(normalizedX*terrainWidth);
        int pixelY = (int)(normalizedY*terrainHeight);

        return GetPixelColor(pixelX, pixelY);
    }

    Color GetPixelColor(int x, int y){
        return TerrainMask.GetPixel(x, y);
    }

    /////////////////////////////
    ///// Renderer Sprite
    /////////////////////////////
    Vector2 spriteSize;

    /*  
        The sprite vertices attribute always return the vertices centered in 0,0.
        So this function get one vertex and uses to calculate the texture size, 
        as the bottom left corner of the texture was the 0,0 position.
    */
    Vector2 GetNormalizeAbsoluteVertexPosition(GameObject renderingGo){
        Vector2 vertex = renderingGo.GetComponent<SpriteRenderer>().sprite.vertices[0];

        return new Vector2( Mathf.Abs(vertex.x)*4, Mathf.Abs(vertex.y)*4);
    }




    void CreateWhiteForm(){
        TerrainMask.SetPixel(401,401, Color.white);
        TerrainMask.SetPixel(300,300, Color.white);
        TerrainMask.SetPixel(200,200, Color.white);
        TerrainMask.SetPixel(100,100, Color.white);
        
        TerrainMask.Apply();
    }


    /////////////////////////////
    ///// Monobehavior
    /////////////////////////////

    void Awake(){
        GetTerrainMask();
        spriteSize = GetNormalizeAbsoluteVertexPosition(this.transform.GetChild(0).gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
    }
}
