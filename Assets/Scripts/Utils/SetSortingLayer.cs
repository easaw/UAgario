using UnityEngine;
using System.Collections;

public class SetSortingLayer : MonoBehaviour {
	public enum RenderType
	{
		normalRenderer,
		trailRenderer,
		particleSystem,
		lineRenderer,
		textMesh
	}
	public RenderType renderType;
	public string layerName = "Default";
	public int order = 1;
	
	// Use this for initialization
	void OnEnable () 
	{
		setLayer();
	}
	
	public void setLayer()
	{
		switch(renderType)
		{
		case RenderType.normalRenderer:
			this.GetComponent<Renderer>().sortingLayerName = layerName;
			this.GetComponent<Renderer>().sortingOrder = order;
			break;
		case RenderType.textMesh:
			TextMesh mesh = this.GetComponent<TextMesh>();
			mesh.GetComponent<Renderer>().sortingLayerName = layerName;
			mesh.GetComponent<Renderer>().sortingOrder = order;
			break;
		case RenderType.trailRenderer:
			TrailRenderer trail = this.GetComponent<TrailRenderer>();
			trail.sortingLayerName = layerName;
			trail.GetComponent<Renderer>().sortingOrder = order;
			break;
		case RenderType.particleSystem:
			ParticleSystem part = this.GetComponent<ParticleSystem>();
			part.GetComponent<Renderer>().sortingLayerName = layerName;
			part.GetComponent<Renderer>().sortingOrder = order;
			break;
		case RenderType.lineRenderer:
			LineRenderer line = this.GetComponent<LineRenderer>();
			line.GetComponent<Renderer>().sortingLayerName = layerName;
			line.GetComponent<Renderer>().sortingOrder = order;
			break;
		default:
			break;
		}
	}
}