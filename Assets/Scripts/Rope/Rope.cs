using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Rope : MonoBehaviour
{
    private List<RopeNode> nodes = new List<RopeNode>();
    [SerializeField] List<RopeNode> keyNodes;
    [SerializeField] GameObject RopeNodePrefab;
    private float RopeNodeSize;
    private bool isBurning;
    private EventInstance fire;
    

    void Start()
    {
        fire = AudioManager.instance.CreateEventInstance(FModEvents.instance.fire);

        RopeNodeSize = keyNodes[0].transform.localScale.x;

        for(int i = 0; i < keyNodes.Count - 1; i++)
        {
            Transform curNodeTransform = keyNodes[i].transform;
            if(keyNodes.Count != 1)
            {
                Transform nextNodeTransform = keyNodes[i + 1].transform;
                Vector3 displacement = nextNodeTransform.position - curNodeTransform.position;
                float distance = displacement.magnitude;

                GameObject prevNode = keyNodes[i].gameObject;

                int numRopeNodes = (int)(distance / RopeNodeSize) + 1;

                for (int j = 1; j < numRopeNodes; j++)
                {
                    GameObject tempRopeNode = Instantiate(RopeNodePrefab, 
                                                            Vector3.Lerp(curNodeTransform.position, nextNodeTransform.position, (float)j / (numRopeNodes)), 
                                                            Quaternion.identity);
                    prevNode.GetComponent<RopeNode>().nodes.Add(tempRopeNode.GetComponent<RopeNode>());
                    tempRopeNode.GetComponent<RopeNode>().nodes.Add(prevNode.GetComponent<RopeNode>());
                    prevNode = tempRopeNode;
                    nodes.Add(tempRopeNode.GetComponent<RopeNode>());
                }
                prevNode.GetComponent<RopeNode>().nodes.Add(nextNodeTransform.gameObject.GetComponent<RopeNode>());
                nextNodeTransform.gameObject.GetComponent<RopeNode>().nodes.Add(prevNode.GetComponent<RopeNode>());
            }
        }

        isBurning = false;


    }

    // Update is called once per frame
    void Update()
    {
        isBurning = false;
        foreach (var node in nodes)
        {
            if (node.GetNodeState() == RopeNode.State.Burning)
            {
                isBurning = true;
                break;
            }
        }
        PLAYBACK_STATE firePlayBackState;
        fire.getPlaybackState(out firePlayBackState);
        if (isBurning)
        {
            
            if (firePlayBackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                fire.start();
            }
        }
        else if(firePlayBackState.Equals(PLAYBACK_STATE.PLAYING))
        {
            fire.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
