using UnityEngine;
using System.Collections;

public class MakeObjects : MonoBehaviour{

	GameObject CreateEnemyFromSprite(Texture2D tex){
		GameObject go = CreateGOFromSprite (tex);
		go.tag = "enemy";
		return go;
	}

	GameObject CreateItemFromSprite(Texture2D tex){
		 GameObject go = CreateGOFromSprite (tex);
		go.tag = "Item";
		return go;
	}

	GameObject CreateStaticFromSprite(Texture2D tex){
		GameObject go = CreateGOFromSprite (tex);
		go.tag = "Static";
		return go;
	}

	GameObject CreateGOFromSprite(Texture2D tex ){
		GameObject newObj = Instantiate(gameObject) as GameObject;
		
		SpriteRenderer renderer =  newObj.GetComponent<SpriteRenderer>();
		Sprite sprite = new Sprite();
		sprite = Sprite.Create(tex,new Rect(0, 0, tex.width, tex.height),new Vector2(tex.width/2,tex.height/2));
		renderer.sprite = sprite;
		newObj.AddComponent<PolygonCollider2D> ();
		return newObj;
	}
}
