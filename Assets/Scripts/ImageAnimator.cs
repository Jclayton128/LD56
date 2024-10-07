using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ThinkBuildPlay.ClimateCity
{

	public class ImageAnimation : MonoBehaviour
	{

		public Sprite[] sprites;
		public int FramesPerSprite = 6;
		public bool IsLooping = true;
		public bool ShouldDestroyOnEnd = false;

		private int index = 0;
		private Image image;
		private int frame = 0;

		void Awake()
		{
			image = GetComponent<Image>();
		}

		void Update()
		{
			if (!IsLooping && index == sprites.Length) return;
			frame++;
			if (frame < FramesPerSprite) return;
			image.sprite = sprites[index];
			frame = 0;
			index++;
			if (index >= sprites.Length)
			{
				if (IsLooping) index = 0;
				if (ShouldDestroyOnEnd) Destroy(gameObject);
			}
		}
	}


}
