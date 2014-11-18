using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

public class CutSheet : MonoBehaviour
{   

	/// An sprite sheet from which to cut
	public Texture2D inputTex;

	/// The background color to ignore
	public Color backGroundColor = Color.white;
	
	// Use this for initialization
	void Start()
	{

		List<int[]> Islands = CutSpriteSheet(inputTex);

		// Render the sprite sheet with highlighting:
//		MeshRenderer Mr = GetComponent<MeshRenderer>();
//		Mr.material.mainTexture = DisplayTex;
//		DisplayTex.wrapMode = TextureWrapMode.Clamp;
//		DisplayTex.Apply(true);
		
		// Extract sprites as individual textures:
		Texture2D[] Sprites = GetSprites(inputTex, Islands);
//		Directory.CreateDirectory(Application.dataPath + "/sprites/");
//		for (int i = 0; i < Sprites.Length; i++)
//		{
//			byte[] bytes = Sprites[i].EncodeToPNG();
//			FileStream file = File.Open(Application.dataPath + "/sprites/sprite" + i + ".png", FileMode.Create);
//			BinaryWriter bw = new BinaryWriter(file);
//			bw.Write(bytes);
//			file.Close();
//		}
		print("done");
	}
	

	/// Extract sprites from sheet as individual textures
	Texture2D[] GetSprites(Texture2D input, List<int[]> Islands)
	{
		Texture2D[] output = new Texture2D[Islands.Count];
		
		int i = 0;
		foreach (int[] coords in Islands)
		{
			int width = coords[2] - coords[0];
			int height = coords[3] - coords[1];
			
			Texture2D sprite = new Texture2D(width, height);
			Color[] pix = input.GetPixels(coords[0], coords[1], width, height);
			
			sprite.SetPixels(pix);
			sprite.Apply(true);
			
			output[i++] = sprite;
		}
		
		return output;
	}
	

	

	/// Find contiguous islands of pixels in a sprite sheet.

	/// returns- A list of coordinates for each sprite, in the form:  [x_min, y_min, x_max, y_max]
	List<int[]> CutSpriteSheet(Texture2D Sprites)
	{
		// Get pixels, width and height from texture:
		Color[] pix = Sprites.GetPixels();
		int width = Sprites.width;
		int height = Sprites.height;
		
		// Create a new array which identifies which island each pixel belongs to.
		int[] Islands = new int[pix.Length];
		
		// Each pixel starts as it's own island, unless it's transparent, in which case we set it to -1
		for (int i = 0; i < Islands.Length; i++)
		{
			if (pix[i] == backGroundColor){
				pix[i][3]=0;
				Islands[i] = -1;
			}
			else{
				//print (pix[i]);
				Islands[i] = i;
			}
		}
		Sprites.SetPixels (pix);
		Sprites.Apply ();
		// For simplicity, we'll convert this to a 2d array
		int[,] Islands2d = new int[width, height];
		for (int i = 0; i < Islands.Length; i++)
		{
			int x = i % width;
			int y = (i - x) / width;
			Islands2d[x, y] = Islands[i];
		}
		
		// Now we spread each island
		bool Changed = true;
		while (Changed)
		{
			// If no changes are made this loop, we're done
			Changed = false;
			
			// For each pixel
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					// If this pixel is transparent, do nothing and continue to the next pixel
					if (Islands2d[x, y] == -1)
						continue;
					
					// For each pixel neighbouring that pixel
					for (int i = -1; i <= 1; i++)
					{
						// Check the neighbouring pixel is within bounds
						if ((x + i) < 0 || (x + i) >= width)
							continue;
						
						for (int j = -1; j <= 1; j++)
						{
							// Check the neighbouring pixel is within bounds
							if ((y + j) < 0 || (y + j) >= height)
								continue;
							
							// If this and the neighbouring pixel are not in the same island, join them
							if (Islands2d[x, y] > Islands2d[x + i, y + j] && Islands2d[x + i, y + j] != -1)
							{
								Islands2d[x, y] = Islands2d[x + i, y + j];
								
								Changed = true;
							}
						}
					}
				}
			}
		}
		
		// Now all connected islands of points should have the same number (ID)
		// We get the number of each island
		List<int> Island_IDs = new List<int>();
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (Islands2d[x, y] != -1 && !Island_IDs.Contains(Islands2d[x, y]))
					Island_IDs.Add(Islands2d[x, y]);
			}
		}
		
		List<int[]> output = new List<int[]>();
		
		// For each ID, get the upper and lower bounds of that island's coordinates
		for (int i = 0; i < Island_IDs.Count; i++)
		{
			int x_min = int.MaxValue;
			int x_max = int.MinValue;
			int y_min = int.MaxValue;
			int y_max = int.MinValue;
			
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					// If this point belongs to this island ID
					if (Islands2d[x, y] == Island_IDs[i])
					{  // Check if its coordinates extend the bounds of this island:
						x_min = x < x_min ? x : x_min;
						x_max = x > x_max ? x : x_max;
						y_min = y < y_min ? y : y_min;
						y_max = y > y_max ? y : y_max;
					}
				}
			}
			
			// What you do with this information now is up to you, for now I'll print it to the console:
			print("-----");
			print("Island ID: " + Island_IDs[i]);
			print("Top left corner:     " + x_min + ", " + y_min);
			print("Bottom right corner: " + x_max + ", " + y_max);
			
			// Also output as an array of coordinates
			output.Add(new int[] { x_min, y_min, x_max, y_max });
		}
		
		return output;
	}
}