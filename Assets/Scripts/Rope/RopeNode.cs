using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeNode : Triggerable
{
    enum State
    {
        Burning,
        Burnt,
        Default
    }

    private State m_state;
    private bool isInteractable = false;
    private Material m_Material;
    [SerializeField] private float timer = 2.0f;
    [SerializeField] private GameObject fireAnimation;
    [SerializeField] private float igniteTime = 1.5f;
    public List<RopeNode> nodes = new List<RopeNode>();
    private TimeFreeze tf;

    private void OnTriggerEnter(Collider other)
    {
        isInteractable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInteractable = false;
    }

    public void ignite()
    {
        if(m_state == State.Default)
        {
            m_state = State.Burning;
        }
        
    }

    private void UpdateRopeNodeState()
    {
        if(m_state == State.Burning)
        {
            fireAnimation.SetActive(true);

            if (timer <= 0)
            {
                m_state = State.Burnt;
            } else if (timer <= igniteTime)
            {
                foreach(RopeNode node in nodes)
                {
                    node.ignite();
                }
            }
            timer -= Time.deltaTime;

        }
        else if(m_state == State.Burnt)
        {
            if (m_Material)
            {
                fireAnimation.SetActive(false);
                m_Material.color = Color.black;
            }
            GameEvents.current.TriggerEnter(id);
        }
        else if(m_state == State.Default)
        {
            if (isInteractable && Input.GetKeyDown(KeyCode.E))
            {
                ignite();
            }
        }
        else
        {
            Debug.LogError("Rope node in wrong state");
        }
    }

    void Start()
    {
        tf = GameObject.FindGameObjectWithTag("CopyPool").GetComponent<TimeFreeze>();
        m_state = State.Default;
        m_Material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (tf.freezed) {
            return;
        }
        UpdateRopeNodeState();
    }
}
