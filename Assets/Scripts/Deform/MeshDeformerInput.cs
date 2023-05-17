using Unity.Burst.CompilerServices;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {
	
	public float force = 10f;
	public float forceOffset = 0.1f;
	GameObject m_player;
	Rigidbody playerRb;
	RaycastHit hit;

    private void Start()
    {
		m_player = GameObject.FindGameObjectWithTag("Player");
		playerRb = m_player.GetComponent<Rigidbody>();
	}

    void Update () {

        if (Physics.Raycast(playerRb.position, Vector3.down, out hit)) {
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
}