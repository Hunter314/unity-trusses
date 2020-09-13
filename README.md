# Unity-Trusses
This is a truss simulator written in C# that can be used with Unity. To use this truss simulator in Unity, you may import the contents of the assets folder and run any of the following scenes. If you would like to use it to simulate your own trusses, please read the information on the scripts below.
All the C# scripts are contained inside the "assets" folder. Here, I will include a brief discription of the scripts included and what they accomplish:
# Scripts
## BeamConfiguration.cs
This script is responsible for simulating trusses. When attached as a component of an empty GameObject, on runtime it will use the specifications in its public variables to generate a truss and then simulate it. These public variables indicate starting information about the joints and members of the truss, including the following: connections, their elasticity, masses, whether the points are pinned, and starting positions. These public variables also include the speed of the simulation and the number of steps of simulation per second. You can modify these last two variables as needed for performace.
This script also creates and updates visual objects of the truss. This includes changing the color of the members depending on their stress or strain. Red indicates high stress, blue indicates high strain. When a member of the truss reaches its set elastic limit (which can be set in the public variables), it will turn black and eventually break its connection. 
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
A truss resembling a building, with a modifyable number of "stories" and modifyable shape. Is the shape of a rectangular prism and has cross-sections in all rectangular cells.
## Configuration2.unity
A simple kingspost truss, pinned at the edges.
## Configuration2.1.unity
A variation of a double pendulum in three dimensions. Helpful for developing intuition for recognizing stress/strain from the color of the members.

