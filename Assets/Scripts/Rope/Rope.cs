using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private List<RopeNode> nodes;
    [SerializeField] List<RopeNode> keyNodes;
    [SerializeField] GameObject RopeNodePrefab;
    private float RopeNodeSize;
    // Start is called before the first frame update
    void Start()
    {
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

                int numRopeNodes = (int)(distance / RopeNodeSize);

                for (int j = 1; j < numRopeNodes; j++)
                {
                    GameObject tempRopeNode = Instantiate(RopeNodePrefab, 
                                                            Vector3.Lerp(curNodeTransform.position, nextNodeTransform.position, (float)j / (numRopeNodes)), 
                                                            Quaternion.identity);
                    prevNode.GetComponent<RopeNode>().nodes.Add(tempRopeNode.GetComponent<RopeNode>());
                    tempRopeNode.GetComponent<RopeNode>().nodes.Add(prevNode.GetComponent<RopeNode>());
                    prevNode = tempRopeNode;
                }
                prevNode.GetComponent<RopeNode>().nodes.Add(nextNodeTransform.gameObject.GetComponent<RopeNode>());
                nextNodeTransform.gameObject.GetComponent<RopeNode>().nodes.Add(prevNode.GetComponent<RopeNode>());
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
