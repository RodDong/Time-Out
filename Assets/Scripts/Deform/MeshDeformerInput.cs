using Unity.Burst.CompilerServices;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {
	
	public float force = 10f;
	public float forceOffset = 0.1f;
	GameObject m_player;
	Rigidbody playerRb;
	RaycastHit hit;

	private bool isPressed = false;

    private void Start()
    {
		m_player = GameObject.FindGameObjectWithTag("Player");
		if(m_player == null) { Debug.LogWarning("Player is not present in scene"); }
		playerRb = m_player.GetComponent<Rigidbody>();
	}

    void Update () {
		isPressed = Physics.Raycast(playerRb.position, Vector3.down, out hit);
        if (isPressed) {
			HandleInput();
		}
	}

	void HandleInput () {

		MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
		if (deformer) {
			Vector3 point = hit.point;
			point += hit.normal * forceOffset;
			deformer.AddDeformingForce(point, force);
		}
	}

	public bool GetIsPressed()
	{
		return isPressed;
	}
}