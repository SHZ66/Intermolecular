using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Intermolecular
{
    public static class Physics
    {
        public class PhysicalObject
        {
            public Vector2 Position = Vector2.Zero;
            public Vector2 Velocity = Vector2.Zero;

            public List<Vector2> Displacements = new List<Vector2>();
            public List<Vector2> Forces = new List<Vector2>();
            
            public float Rotation = 0.0f;
            public float w = 0.0f;

            public float Radius = 50.0f;
            public float Mass = 1.0f;

            public Texture2DWrapper Texture;

            public PhysicalObject(Texture2DWrapper _texture, 
                                  Vector2 _position = default(Vector2), 
                                  float _rotation = 1.0f, 
                                  float _radius = 50.0f)
            {
                Texture = _texture;
                Position = _position;
                Rotation = _rotation;
                Radius = _radius;
            }

            public virtual void Update(float dt)
            {
                // force decomposition
                for (int i = 0; i < Displacements.Count; i++)
                {
                    Vector2 Displacement = Displacements[i];
                    Vector2 Force        = Forces[i];
                    Vector2 m = Vector2.Divide(Displacement, Displacement.Length());
                    Vector2 Fa = Vector2.Dot(Force, m) * m;
                    Vector2 Fb = Force - Fa;

                    // translation  accel update
                    this.Velocity += Fa / Mass * dt;

                    // rotation accel update
                    float Torque = Fb.Length() * Displacement.Length();
                    float I = MathHelper.PiOver2 * this.Mass * this.Radius;
                    this.w += Torque / I * dt;
                }

                // translation update
                this.Position += this.Velocity * dt;

                // rotation update
                this.Rotation += this.w * dt;

                // clean up
                Displacements.Clear();
                Forces.Clear();
            }

            public virtual void Draw()
            {
                Texture.Draw(Position, Rotation);
            }
        }

        public class SpaceShip : PhysicalObject
        {
            public Vector2 Thrusts = Vector2.Zero;
            public Texture2DWrapper ThrustTexture;

            public Vector2 LeftThrust
            { get { return Vector2.Transform(Radius * Vector2.UnitY, Matrix.CreateRotationZ( MathHelper.PiOver4/1.5f + Rotation)); } }
            public Vector2 RightThrust
            { get { return Vector2.Transform(Radius * Vector2.UnitY, Matrix.CreateRotationZ(-MathHelper.PiOver4/1.5f + Rotation)); } }

            public SpaceShip(Texture2DWrapper _spaceshiptexture,
                             Texture2DWrapper _thrusttexture,
                             Vector2 _position = default(Vector2),
                             float _rotation = 1.0f,
                             float _radius = 50.0f):base(_spaceshiptexture, _position, _rotation, _radius)
            {
                ThrustTexture = _thrusttexture;
            }

            public override void Update(float dt)
            {
                Vector2 leftThrust  = LeftThrust;
                Vector2 rightThrust = RightThrust;
                base.Update(dt);
            }

            public override void Draw()
            {
                Vector2 leftThrust = LeftThrust;
                Vector2 rightThrust = RightThrust;
                ThrustTexture.Draw(Position + leftThrust, Rotation);
                ThrustTexture.Draw(Position + rightThrust, Rotation);
                Texture.Draw(Position, Rotation);                
            }
        }

        public static List<PhysicalObject> Objects = new List<PhysicalObject>();

        public static void Update(float dt)
        {
            foreach (PhysicalObject obj in Objects)
            {
                obj.Update(dt);
            }
            return;
        }

        public static void Draw()
        {
            foreach (PhysicalObject obj in Objects)
            {
                obj.Draw();
            }
            return;
        }
    }
}
