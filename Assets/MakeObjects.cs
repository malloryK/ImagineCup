using UnityEngine;
using System.Collections;

public class MakeObjects : MonoBehaviour{

	private static MakeObjects _instance;
	public static MakeObjects Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = (MakeObjects)GameObject.FindObjectOfType(typeof(MakeObjects));
				if (!_instance)
				{
					GameObject container = new GameObject();
					_instance = container.AddComponent(typeof(MakeObjects)) as MakeObjects;
				}
			}
			
			return _instance;
		}
	}
	
	public GameObject MakeGOaEnemy(GameObject go){
		go.AddComponent<EnemyBehaviour>();
		go.tag = "Enemy";
		return go;
	}
	
	public GameObject MakeGOaItem(GameObject go){
		go.AddComponent<ItemBehaviour>();

		go.tag = "Item";
		return go;
	}

	public GameObject MakeGOaStatic(GameObject go){
		go.AddComponent<ItemBehaviour>();
		go.tag = "Static";
		return go;
	}

	public GameObject CreateGOFromTexture2D(Texture2D tex ){
		GameObject newObj = Instantiate(gameObject) as GameObject;

		SpriteRenderer renderer = new SpriteRenderer ();
		renderer = newObj.AddComponent("SpriteRenderer") as SpriteRenderer;
		Sprite sprite = new Sprite();
		sprite = Sprite.Create(tex,new Rect(0, 0, tex.width, tex.height),new Vector2(tex.width/2,tex.height/2));
		renderer.sprite = sprite;
		newObj.AddComponent<PolygonCollider2D> ();
		return newObj;
	}
}
