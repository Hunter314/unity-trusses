# Unity-Trusses
This is an open source truss simulator written in C# that can be used with Unity. To use this truss simulator in Unity, you may import the contents of the assets folder and run any of the following scenes. If you would like to use it to simulate your own trusses, please read the information on the scripts below.
All the C# scripts are contained inside the "assets" folder. Here, I will include a brief discription of the scripts included and what they accomplish:
# Scripts
## BeamConfiguration.cs
This script includes the object definitions and events needed to physically simulate a given truss and visually display it through GameObjects. When attached as a component of an empty GameObject, on runtime it will use its public variables to generate a truss and then simulate it. These public variables indicate starting information about the joints and members of the truss, including the following: connections, elasticity, masses, whether the points are pinned, and starting positions. These public variables also include the speed of the simulation and the number of steps of simulation per second. You can modify these last two variables as needed for performace-- you may find that simulations with greater step sizes will become less and less accurate, and often instable. Generally, simulations with steps of greater than 0.05 seconds will generate poor results. If you are experiencing poor performance with this step size, it may be necessary to decrease the simulation speed.

### Visual Objects
This script also creates and updates visual objects of the truss. These visual objects are created as GameObjects in Unity. While GameObjects are used for the visualization, the physics is done entirely by the BeamConfiguration script. This is to allow for elements of the BeamConfiguration script such as the Member and Point classes to be reused with different visualization tools.

Red indicates high stress, blue indicates high strain. When a member of the truss reaches its set elastic limit (which can be set in the public variables), it will turn black and eventually break its connection. 
## GridConfiguration.cs
If you don't want to create trusses manually, the gridConfiguration script has a number of built in configurations of joints, members and pins. This include grids and rectangular prisms, with the option to add cross-sections, change anchor points or modify distances between rows. When a GridConfiguration component is added to an empty GameObject with a BeamConfiguration component, it will automatically set the public variables required to generate the truss according to specification. Note that the speed and number of steps for the simulation must still be set in BeamConfiguration. 
## CameraMove.cs
This script allows the camera to orbit around the origin point. The controls for the camera are listed inside this script, but for reference, are as follows:
A is rotate around origin left
D is rotate around origin right
E is rotate around origin up
C is rotate around origin down
W is move away from origin
S is move towards origin

# Scenes
## BuildingTruss.unity
A truss resembling a building, with a modifyable number of "stories" and modifyable shape. Is the shape of a rectangular prism and has cross-sections in all rectangular cells. A building with 14 x 14 floors and 10 "stories" is shown below:
![BuildingTruss](https://github.com/Hunter314/unity-trusses/blob/master/Truss.png?raw=true)
## Configuration2.unity
A simple kingspost truss, pinned at the edges.
## Configuration2.1.unity
A variation of a double pendulum in three dimensions. Helpful for learning to recognize stress/strain from the color of the members.

