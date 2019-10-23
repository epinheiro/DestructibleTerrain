using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PolygonScenarioController : MonoBehaviour
{
    Color clrDestruction = new Color(0,0,0,0);

    ///// Terrain manipulation
    /////////////////////////////
    Texture2D TerrainTexture;

    Texture2D SetTerrainTextureToDefault(){
        Texture2D originalTexture = Resources.Load<Texture2D>("spr_terrain");

        // Clone the texture to not alter the original
        TerrainTexture = (Texture2D) Instantiate(originalTexture); 
        
        return TerrainTexture;
    }

    void DestroyQuadrangle(Texture2D tex, int x0, int y0, int width, int height){
        for (int i=x0; i< width; i++)
            for (int j=y0; j< height; j++)
                tex.SetPixel(i, j, clrDestruction);
        
        tex.Apply();

        UpdatePolygonCollider2D();
    }

    void DestroyCircle(Texture2D tex, int x, int y, int radius){
        DrawCircle(tex, clrDestruction, x, y, radius);

        tex.Apply();
        
        UpdatePolygonCollider2D();
    }

    // https://stackoverflow.com/a/56616769 //
    Texture2D DrawCircle(Texture2D tex, Color color, int x, int y, int radius = 3){
        float rSquared = radius * radius;

        for (int u = x - radius; u < x + radius + 1; u++)
            for (int v = y - radius; v < y + radius + 1; v++)
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                    tex.SetPixel(u, v, color);

        return tex;
    }

    ///// World Space manipulation 
    /////////////////////////////
    Vect2Int TransformGlobalToTexture(Texture2D tex, float x, float y){
        SpriteRenderer sRenderer = this.GetComponent<SpriteRenderer>();
        Rect sRect = sRenderer.sprite.rect;
        Vector2 sSize = sRenderer.size;
        
        // TODO - only works when scenario has its center on (0,0)
        float collisionX = x - this.transform.position.x;
        float collisionY = y - this.transform.position.y;

        int textureX = (int) ((collisionX/(2*sSize.x)+0.5F)*tex.width);
        int textureY = (int) ((collisionY/(2*sSize.y)+0.5F)*tex.height);

        //Debug.Log("DEBUG: Original->Collision->Texture");
        //Debug.Log("(" + x.ToString("F2") + "," + y.ToString("F2") +")->(" + collisionX.ToString("F2") + "," + collisionY.ToString("F2") + ")->(" + textureX + "," + textureY + ")");

        return new Vect2Int(textureX, textureY);
    }

    /*  
        The sprite vertices attribute always return the vertices centered in 0,0.
        So this function get one vertex and uses to calculate the texture size, 
        as the bottom left corner of the texture was the 0,0 position.
    */
    Vector2 GetNormalizeSpriteSize(GameObject renderingGo){
        Vector2 vertex = renderingGo.GetComponent<SpriteRenderer>().sprite.vertices[0];

        return new Vector2( Mathf.Abs(vertex.x)*4, Mathf.Abs(vertex.y)*4);
    }

    ///// Game Components 
    /////////////////////////////
    
    void SetInitialGameComponents(){
        SpriteRenderer sr = this.gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(
                                TerrainTexture, 
                                new Rect(0f, 0f, TerrainTexture.width, TerrainTexture.height),
                                new Vector2(0.5f, 0.5f), 100f
                            );

        this.gameObject.AddComponent<PolygonCollider2D>();
    }

    void UpdatePolygonCollider2D(){
        Destroy(this.gameObject.GetComponent<PolygonCollider2D>());
        this.gameObject.AddComponent<PolygonCollider2D>();
    }

    ///// Mono Behavior 
    /////////////////////////////

    void Awake(){
        SetTerrainTextureToDefault();
        SetInitialGameComponents();

        

        Time.timeScale = 0.5F;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other){
        GameObject otherGo = other.gameObject;
        if (otherGo.tag == "Bullet"){

            float cx = otherGo.transform.position.x;
            float cy = otherGo.transform.position.y;

            Vect2Int transformedPoint = TransformGlobalToTexture(TerrainTexture, cx, cy);

            DestroyCircle(TerrainTexture, transformedPoint.x, transformedPoint.y, 50);

            Destroy(other.gameObject);
        }
    }
}
