﻿using System;
using UnityEngine;
using System.Collections.Generic;
using Geodesy.Controllers;
using Geodesy.Controllers.Workers;

namespace Geodesy.Views
{
	public enum RenderingMode
	{
		Texture,
		Depth,
		Terrain
	}

	/// <summary>
	/// Define a rectangular quad mesh projected on a datum.
	/// </summary>
	public class Patch
	{
		public const int SubdivisionsWithoutTerrain = 8;
		public const int SubdivisionsWithTerrain = 32;
		public const int TextureSize = 256;

		/// <summary>
		/// The depth at which the terrain will be displayed.
		/// </summary>
		public const int TerrainDisplayedDepth = 6;

		public const int MaxAltitude = 9000;

		private GameObject gameObject;

		private MeshFilter meshFilter;

		public int i { get; private set; }

		public int j { get; private set; }

		public int Depth { get; private set; }

		public RenderTexture Texture { get; private set; }

		private RenderingMode mode;

		public RenderingMode Mode
		{
			get { return mode; }
			set
			{
				if (mode != value)
				{
					mode = value;
					UpdateRenderingMode ();
				}
			}
		}

		public DateTime InvisibleSince { get; private set; }

		private Material textureMaterial;

		private Material pseudocolorMaterial;

		private Material terrainMaterial;

		private MeshRenderer renderer;

		private bool visible;

		public bool Visible
		{
			get { return visible; }
			set
			{
				gameObject.SetActive (value);
				visible = value;
				if (!value)
				{
					InvisibleSince = DateTime.Now;
				}
			}
		}

		public Patch (Globe globe, Transform root, int i, int j, int depth, Material material, Material pseudoColor, Material terrain)
		{
			pseudocolorMaterial = pseudoColor;
			terrainMaterial = terrain;

			this.i = i;
			this.j = j;
			this.Depth = depth;

			MeshObject meshObject = MeshBuilder.Instance.RequestPatchMesh (i, j, depth);
			Mesh mesh = meshObject.Mesh;
			mesh.RecalculateBounds ();

			CreateGameObject (mesh, material, meshObject.Position, root);
		}

		private void UpdateRenderingMode ()
		{
			switch (mode)
			{
				case RenderingMode.Texture:
					renderer.material = textureMaterial;
					break;
				case RenderingMode.Terrain:
					renderer.material = terrainMaterial;
					break;
				case RenderingMode.Depth:
					renderer.material = pseudocolorMaterial;
					renderer.material.color = Colors.MakeCheckered (Colors.GetDepthColor (Depth), 0.15f, i, j);
					break;
				default:
					throw new ArgumentException ("Unknown mode: " + mode);
			}
		}

		private void CreateGameObject (Mesh mesh, Material material, Vector3 position, Transform root)
		{
			gameObject = new GameObject (string.Format ("[{0}] {1}, {2}", Depth, i, j));
			gameObject.transform.parent = root;

			renderer = gameObject.AddComponent<MeshRenderer> ();
			renderer.material = material;
			textureMaterial = renderer.material;
			RenderTexture tex = new RenderTexture (TextureSize, TextureSize, 16);
			tex.name = gameObject.name;
			renderer.material.mainTexture = tex;
			renderer.material.SetTexture ("_EmissionMap", tex);
			Texture = tex;

			meshFilter = gameObject.AddComponent<MeshFilter> ();
			meshFilter.sharedMesh = mesh;

			gameObject.transform.position = position;
		}

		public void UpdateMesh (Mesh newMesh)
		{
			if (meshFilter == null)
			{
				throw new NullReferenceException ("Trying to update the mesh of on a null mesh filter.");
			}

			meshFilter.sharedMesh = newMesh;
		}

		public void Destroy ()
		{
			GameObject.Destroy (textureMaterial);
			GameObject.Destroy (gameObject);
		}
	}
}

