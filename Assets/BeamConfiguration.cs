using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamConfiguration : MonoBehaviour
{
    // Start is called before the first frame update
    
    public List<Vector3> Joints;
    public List<int> LoadIndices;
    public List<int> LoadValues;
    public List<int> PinIndices;
    public List<int> RollingJointIndices;
    public List<int> MembersFirst;
    public List<int> MembersSecond;
    private List<int[]> members;
    private List<Member> memberObjs;
    private List<Joint> jointObjs;
    private List<GameObject> memberMarkers;
    private List<GameObject> jointMarkers;
    public GameObject VisualJoint;
    public GameObject VisualMember;
    public GameObject VisualPin;
    public GameObject VisualRolling;
    private int framesPerUpdate;
    private int framesSoFar;
    private int counter;
    public float dt;
    public float speed;
    public bool breaking;
    public bool isVelocity;
    public bool isStatic;
    public bool isEditable;
    void Start()
    {
        //Called once these variables are initialized

        if (Joints != null)
        {
            CreateMembersList();
            CreateSystem();
            CreateVisualSystemObjs();

        }


    }
    public void StartNow()
    {
        CreateMembersList();
        CreateSystem();
        CreateVisualSystemObjs();


    }
    // Update is called once per frame
    void Update()
    {
        if (memberObjs == null)
        {
            StartNow();
        }
        for (int i = 0; i < speed / (24 * dt); i++)
        {
            UpdateMemberForces();
            UpdateJointMovement();

        }
        framesSoFar++;
        if (framesSoFar == 4)
        {
            framesSoFar = 0;
            UpdateVisualSystemObjs();
        }
        
        
        
        
    }
    private void CreateMembersList()
    {
        members = new List<int[]>();
        for (int i = 0; i < MembersFirst.Count; i++)
        {
            members.Add(new int[2]);
            members[i][0] = MembersFirst[i];
            members[i][1] = MembersSecond[i];
        }
    }
    private void UpdateJointMovement()
    {
        for (int i = 0; i < jointObjs.Count; i++)
        {
            jointObjs[i].updatePosition(dt, isVelocity);
        }
    }
    private void UpdateMemberForces()
    {
        for (int i = 0; i < memberObjs.Count; i++)
        {
            memberObjs[i].solveForces();
        }
    }
    private void CreateSystem()
    {
        jointObjs = new List<Joint>();
        memberObjs = new List<Member>();
        
        for (int i = 0; i < Joints.Count; i++)
        {
            jointObjs.Add(new Joint(Joints[i]));
            


        }
        for (int i = 0; i < PinIndices.Count; i++)
        {
            jointObjs[PinIndices[i]].pin();
        }
        for (int i = 0; i < members.Count; i++)
        {
            memberObjs.Add(new Member(jointObjs[members[i][0]], jointObjs[members[i][1]]));



        }

    }
    private void TotalSystemEquilibrium()
    {
        //Choose a center point wherever the fixed joint is
        for (int i = 0; i < Joints.Count; i++)
        {

        }


    }
    
    private void CreateVisualSystemObjs()
    {
        
        jointMarkers = new List<GameObject>();
        memberMarkers = new List<GameObject>();
        for (int i = 0; i < Joints.Count; i++)
        {
            
            jointMarkers.Add(Instantiate(VisualJoint, jointObjs[i].getPosition(), Quaternion.identity, this.GetComponentInParent<Transform>()));
            //Debug.Log("Instantiated a joint marker.");
        }
        for (int i = 0; i < members.Count; i++)
        {
            memberMarkers.Add(Instantiate(VisualMember, memberObjs[i].getPosition(), Quaternion.LookRotation(memberObjs[i].getDirectionVector()) * Quaternion.Euler(90,0,0), this.GetComponentInParent<Transform>()));
            memberMarkers[i].transform.localScale = new Vector3(1,0.5f*memberObjs[i].getLength(),1);
            //Debug.Log("Stress: " + memberObjs[i].getStress());
            //Debug.Log("Change: " + memberObjs[i].changeInLength());
            
            float percentOfMax = (0.5f + (0.45f / memberObjs[i].getMaxStress()) * (memberObjs[i].getMaxStress() - memberObjs[i].getStress()));
            //Debug.Log("Percent of max: " + percentOfMax);
            Color newColor = new Color(0f, 0.5f, 0.5f);
            var memberRenderer = memberMarkers[i].GetComponent<Renderer>();
            memberRenderer.material.color = newColor;
        }

    }
    private void UpdateVisualSystemObjs()
    {
        if (jointMarkers == null)
        {
            CreateVisualSystemObjs();
        }
        for (int i = 0; i < Joints.Count; i++)
        {

            jointMarkers[i].transform.position = jointObjs[i].getPosition();
            
        }
        for (int i = 0; i < members.Count; i++)
        {
            if (memberObjs[i].isDestroyed())
            {
                GameObject.Destroy(memberMarkers[i]);
            }
            else
            {
                memberMarkers[i].transform.localScale = new Vector3(1, 0.5f * memberObjs[i].getLength(), 1);
                memberMarkers[i].transform.position = memberObjs[i].getPosition();
                memberMarkers[i].transform.rotation = Quaternion.LookRotation(memberObjs[i].getDirectionVector()) * Quaternion.Euler(90, 0, 0);
                Color newColor;
                if (memberObjs[i].changeInLength() > 0)
                {
                    if (memberObjs[i].getStress() > memberObjs[i].getMaxStress())
                    {
                        newColor = new Color(0f, 0f, 0f);
                        memberObjs[i].breakMember();
                    }
                    else
                    {
                        float percentOfMax = memberObjs[i].getStress() / memberObjs[i].getMaxStress();
                        newColor = new Color(percentOfMax, 1f - percentOfMax, 0f);
                        
                    }


                }
                else
                {
                    if (-memberObjs[i].getStress() > memberObjs[i].getMaxStress())
                    {
                        newColor = new Color(0f, 0f, 0f);
                        memberObjs[i].breakMember();
                    }
                    else
                    {
                        //float percentOfMax = (0.5f + (0.45f / memberObjs[i].getMaxStress()) * (memberObjs[i].getMaxStress() - memberObjs[i].getStress()));
                        float percentOfMax = memberObjs[i].getStress() / memberObjs[i].getMaxStress();

                        newColor = new Color(0f, 1f - percentOfMax, percentOfMax);

                    }

                }
                var memberRenderer = memberMarkers[i].GetComponent<Renderer>();
                memberRenderer.material.color = newColor;
            }
            
        }
    }
    /*private void MethodOfJoints()
    {

        for (int i = 0; i < memberObjs.Count; i++)
        {




        }

    }*/
}
public class KNForce
{
    float x;
    float y;
    float z;
    Vector3 normalVector;
    Joint forceJoint;
    public Vector3 getPos()
    {
        return forceJoint.getPosition();
    }
    public Vector3 getVector()
    {
        return new Vector3(x, y, z);
    }
    public Vector3 getNormal()
    {
        return normalVector;
    }
    public float getMagnitude()
    {
        return getVector().magnitude;
    }
    public static KNForce weight(float weight)
    {
        return new KNForce(new Vector3(0, 0 - weight, 0));

    }
    public static KNForce windForce()
    {
        //Magnitude of force is dependent on global wind vector3.
        //
        return new KNForce();
    }
    public static KNForce normalUp(float normal)
    {
        return new KNForce(new Vector3(0, normal, 0));

    }

    public KNForce(Vector3 inFVector)
    {
        x = inFVector.x;
        y = inFVector.y;
        z = inFVector.z;
        normalVector = Vector3.Normalize(inFVector);
        
        //For creating known vectors
    }
    public KNForce()
    {
        //For unknown vectors
    }

}
public class Joint
{

    private float mass;
    private bool pinned;
    private List<Member> connectedMembers;
    private List<bool> isFirstMemPoint;
    private List<KNForce> staticForces;
    private Vector3 position;
    private Vector3 velocity;
    //After an initial analysis of the entire truss as a rigid body, normal forces will be evaluated.
    public Joint(Vector3 inPosition, float massIn = 10f)
    {
        velocity = new Vector3(0, 0, 0);
        pinned = false;
        mass = massIn;
        position = inPosition;
        connectedMembers = new List<Member>();
        isFirstMemPoint = new List<bool>();
        staticForces = new List<KNForce>();
        staticForces.Add(KNForce.weight(mass * 9.81f));
    }
    public void removeMembers()
    {
        connectedMembers.Clear();
    }
    public void removeMember(Member inMember)
    {
        connectedMembers.Remove(inMember);
    }
    public void pin()
    {
        pinned = true;
    }
    public Vector3 getPosition()
    {
        return position;
    }
    public void addMember(Member inputMem, bool isFirst)
    {
        connectedMembers.Add(inputMem);
        isFirstMemPoint.Add(isFirst);
    }
    public void addForce(KNForce newForce)
    {
        staticForces.Add(newForce);
    }
    public void updatePosition(float changeT, bool velocityEnabled = true)
    {
        if (pinned)
        {
            return;
        }
        //First, assemble a list of forces
        Vector3 sumForce = new Vector3(0f, 0f, 0f);
        Vector3 distance;
        for (int i = 0; i < staticForces.Count; i++)
        {
            //Debug.Log("added a force.");
            sumForce = sumForce + staticForces[i].getVector();

        }
        for (int i = 0; i < connectedMembers.Count; i++)
        {
            sumForce += connectedMembers[i].getTerminalForce(isFirstMemPoint[i]);
            //Add a getvector onto getTerminalForce if you change forces back to KNForce object

        }
        //Let's do some KINEMATICS. And some F = ma too.
        //dx = 0.5 * a * t^2
        //dx = 0.5 * (F / m) * t^2
        //For all three dimensions
        velocity = velocity + (sumForce) * (changeT / mass);
        if (velocityEnabled)
        {
            distance = velocity * changeT + (0.5f / mass) * sumForce * Mathf.Pow(changeT, 2);

        }
        else
        {
            distance = (0.5f / mass) * sumForce * Mathf.Pow(changeT, 2);
        }
        //


        //Debug.Log("Distance travelled: " + distance);
        position = distance + position;
    }
}
public interface IMember
{
    bool isDestroyed();
    float getLength();
    Vector3 getTerminalForce(bool isFirst);
    Vector3 getTerminalForce(int index);

}
public class Member : IMember
{
    float kElasticity;
    float maxStress;
    Joint[] terminalJoints;
    Vector3[] terminalForces;
    bool isThisDestroyed = false;
    bool solvedForces = false;
    float initialLength;
    public Member(Joint jointA, Joint jointB, bool breaking = false, bool compressionOnly = false, float inKElasticity = 500f, float inMaxStress = 10000f)
    {
        maxStress = inMaxStress;
        kElasticity = inKElasticity;
        terminalJoints = new Joint[2];
        terminalJoints[0] = jointA;
        terminalJoints[1] = jointB;
        terminalForces = new Vector3[2];
        terminalForces[0] = new Vector3(0f, 0f, 0f);
        initialLength = getLength();

        jointA.addMember(this, true);
        jointB.addMember(this, false);
    }
    public bool isDestroyed()
    {
        return isThisDestroyed;
    }
    public float getMaxStress()
    {
        return maxStress;
    }
    public float getLength()
    {
        return getDirectionVector().magnitude;
    }
    public void breakMember()
    {
        terminalJoints[0].removeMember(this);
        terminalJoints[1].removeMember(this);
        isThisDestroyed = true;
    }
    public Vector3 getPosition()
    {
        //This is only for purposes regarding visual representation.
        //Takes the average position of two joints.
        //Debug.Log("Position is at " + (terminalJoints[0].getPosition() + terminalJoints[1].getPosition()) * 0.5f);
        return ((terminalJoints[0].getPosition() + terminalJoints[1].getPosition()) * 0.5f);
    }
    public Vector3 getDirectionVector()
    {
        return (terminalJoints[1].getPosition() - terminalJoints[0].getPosition());
    }
    public Vector3 getTerminalForce(bool isFirst)
    {

        if (isFirst)
        {
            return terminalForces[0];
        }
        else return terminalForces[1];
    }
    public Vector3 getTerminalForce(int index)
    {
        return terminalForces[index];
    }
    public float changeInLength()
    {
        //Negative in compression.
        //Positive in tension.
        return (getLength() - initialLength);
    }
    public void solveForces()
    {
        //Take the normal of the member's direction vector. Scale by dl * kElasticity, and this will be the force of the first point times two.
        Vector3 stretching = changeInLength() * kElasticity * getDirectionVector();
        terminalForces[0] = stretching * 0.5f;
        terminalForces[1] = -stretching * 0.5f;

        //Direction vector goes from FIRST POINT to SECOND POINT.
        //Change in length is negative in compression and positive in tension. In tension, the first point's force vector will be a scalar multiple of member direction vector.
        //Check compression of beam.


    }
    public float getStress()
    {
        return (changeInLength() * kElasticity * getDirectionVector()).magnitude;
        //The same thing that we labeled "stretching" while solving the forces.
        //Add an if statement for if the member is not solved yet
        //return terminalForces[0].getMagnitude() * 2f;


    }

}