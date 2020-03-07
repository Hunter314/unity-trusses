using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BeamConfiguration))]
public class GridConfiguration : MonoBehaviour
{
    public int length;
    public int width;
    public float height;
    public float spacing;
    public Vector3 lowerLeft;
    public enum pinStyle { pinCorners, pinPerimeter, noPins };

    public pinStyle thisStyle;
    // Start is called before the first frame update
    void Start()
    {
        ConfigParameters newConfigPrms = new ConfigParameters();
        createJointGrid(newConfigPrms);
        applyParamsToConfig(newConfigPrms);
        
        
        /*
         Here are the public variables this script must address:
    public List<Vector3> Joints;
    public List<int> LoadIndices; //not necessary
    public List<int> LoadValues; // not necessary
    public List<int> PinIndices;
    public List<int> RollingJointIndices; //not necessary
    public List<int> MembersFirst;
    public List<int> MembersSecond;
    The following can still be modified directly in unity (not dependent on grid configuration):
    public GameObject VisualJoint;
    public GameObject VisualMember;
    public GameObject VisualPin;
    public GameObject VisualRolling;
    public float dt;
    public float speed;
    
         
         */
    }

    void createJointGrid(ConfigParameters thisConfig)
    {
        thisConfig.initialize();
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                thisConfig.Joints.Add(new Vector3(i * spacing, height, j * spacing));
                //0,0, 0,width (last in row )length,0 (first in last row) length,width (last in last row)
            }

        }
        for (int i = 0; i < length * width; i++)
        {
            //i % length -
            //i / length == width: 
            if (((i) / width != length - 1))
            {
                thisConfig.MembersFirst.Add(i);
                thisConfig.MembersSecond.Add(i + width);

            }

            if (i % width != (width - 1))
            {
                thisConfig.MembersFirst.Add(i);
                thisConfig.MembersSecond.Add(i + 1);

            }


        }
        for (int i = 0; i < length * width; i++)
        {
            //i % length -
            //i / length == width: 
            switch (thisStyle)
            {
                case pinStyle.noPins:
                    break;
                case pinStyle.pinPerimeter:
                    if (i % width == (width - 1) || i % width == 0 || ((i) / width == length - 1) || ((i) / width == 0))
                    {
                        thisConfig.PinIndices.Add(i);
                    }
                    break;
                case pinStyle.pinCorners:
                    if (i % width == (width - 1) && ((i) / width == length - 1) || i % width == 0 && ((i) / width == 0))
                    {
                        thisConfig.PinIndices.Add(i);
                    }
                    break;
            }
            
        }


    }
    void applyParamsToConfig(ConfigParameters theseParams)
    {

        BeamConfiguration thisConfig = GetComponent<BeamConfiguration>();
        thisConfig.MembersFirst = theseParams.MembersFirst;
        thisConfig.MembersSecond = theseParams.MembersSecond;
        thisConfig.Joints = theseParams.Joints;
        thisConfig.PinIndices = theseParams.PinIndices;
        

    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
public class ConfigParameters
{
    public List<Vector3> Joints;
    public List<int> MembersFirst;
    public List<int> MembersSecond;
    public List<int> PinIndices;
    public void initialize()
    {

        MembersFirst = new List<int>();
        MembersSecond = new List<int>();
        Joints = new List<Vector3>();
        PinIndices = new List<int>();


    }
    public void extrude(Vector3 extrusion)
    {
        for (int i = 0; i < Joints.Count; i++)
        {
            Joints.Add(Joints[i] + extrusion);

        }


    }
    void radialLineExtrusion(Vector3 extrusion, Vector3 origin)
    {
        for (int i = 0; i < Joints.Count; i++)
        {
            Joints.Add(Joints[i] + extrusion);
        }

    }
}

