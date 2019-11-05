using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PostProcessFromMaterial : MonoBehaviour {
	public static PostProcessFromMaterial _instance;
	public static PostProcessFromMaterial Instance
	{
		get {
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<PostProcessFromMaterial>();
				if (_instance == null)
				{
					GameObject container = new GameObject("PostProcessFromMaterial");
					_instance = container.AddComponent<PostProcessFromMaterial>();
				}
			}     
			return _instance;
		}
	}
	
	public Material material;
	public bool active = false;

	public DepthTextureMode depthTextureMode;
	private Camera cam;
	// Use this for initialization
	private void Awake()
	{
		cam = GetComponent<Camera>();
	}

	private void OnEnable()
	{
		cam.depthTextureMode = DepthTextureMode.Depth | DepthTextureMode.MotionVectors | DepthTextureMode.DepthNormals;
	}


	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (material == null || !active)
			return;
		material.SetMatrix("_InverseViewMatrix", Camera.main.worldToCameraMatrix.inverse);
		Graphics.Blit(source, material);
	}
}
