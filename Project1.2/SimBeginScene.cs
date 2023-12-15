using Godot;
using System;
using System.Runtime.Intrinsics.X86;

public partial class SimBeginScene : Node3D
{
MeshInstance3D anchor;
MeshInstance3D ball;
SpringModel spring;
Label keLabel;
Label peLabel;
Label TotLabel;
Vector3 bl;
Vector3 v0;

PendSim pend;

double xA, yA, zA; // coords of anchor
double xB, yB, zB; // coords of bob
double xC, yC, zC;
float length0; // natural length of pendulum
float length; // length of pendulum
double angle; // pendulum angle
double time;
double hz;
double [] Pendangle;
double PE;
double KE;
double Tot;


Vector3 endA; // end point of anchor
Vector3 endB; // end point for pendulum bob


// Called when the node enters the scene tree for the first time.
public override void _Ready()
{
GD.Print("Hello MEE 381 in Godot!");
xA = 0.0; yA = 1.2; zA = 0.0;
xB = -0.7; yB = -0.2; zB = 0.5;
xC = 0.0; yC = 0.0; zC = 0.9;
anchor = GetNode<MeshInstance3D>("Anchor");
ball = GetNode<MeshInstance3D>("Ball1");
spring = GetNode<SpringModel>("SpringModel");
endA = new Vector3((float)xA, (float)yA, (float)zA);
bl = new Vector3((float)xB, (float)yB, (float)zB);
v0 = new Vector3((float)xC, (float)yC, (float)zC);
anchor.Position = endA;
endB = endA + bl;

keLabel = GetNode<Label>("VBoxContainer/keLabel");
peLabel = GetNode<Label>("VBoxContainer/peLabel");
TotLabel = GetNode<Label>("VBoxContainer/Totlabel");

pend = new PendSim();



length0 = length = 0.9f;
spring.GenMesh(0.05f, 0.015f, length, 6.0f, 62);


// endB.X = endA.X + length*Mathf.Sin(angleF);
// endB.Y = endA.Y - length*Mathf.Cos(angleF);
// endB.Z = endA.Z;
PlacePendulum(endB);
// PlacePendulum((float)angle);

time = 0.0;
}


// Called every frame. 'delta' is the elapsed time since the previous frame.
public override void _Process(double delta)
{
float angleF = 1.0f*(float)Math.Sin(2.0 * time);
float angleA = (float)(0.4*time);
length = length0 + 0.3f * (float)Math.Cos(4.0 * time);

// float angleA = 0.0f;
// float angleF = (float)pend.Angle;

PE = 1.4 * 9.81 * (length-length0);

keLabel.Text = PE.ToString("0.00");
peLabel.Text = angleF.ToString("0.00");


float hz = length*Mathf.Sin(angleF);
endB.X = endA.X + hz*Mathf.Cos(angleA);
endB.Y = endA.Y - length*Mathf.Cos(angleF);
endB.Z = endA.Z + hz*Mathf.Sin(angleA);
PlacePendulum(endB);
time += delta;



}






public override void _PhysicsProcess(double delta)
{
base._PhysicsProcess(delta);

pend.StepRK2(time, delta);
}

// public override void _PhysicsProcess(double delta)
// {

// }
private void PlacePendulum(Vector3 endBB)
{

spring.PlaceEndPoints(endA, endB);
ball.Position = endBB;

}

public double [] angleF
{
	get
	{
		return Pendangle;
	}

}
}

