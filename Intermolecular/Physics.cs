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

            public Vector2 Displacement = Vector2.Zero;
            public Vector2 Force = Vector2.Zero;
            
            public float Rotation = 0.0f;
            public float w = 0.0f;

            public float Radius = 50.0f;
            public float Mass = 1.0f;

            public Texture2DWrapper Texture;

            public PhysicalObject(Texture2DWrapper _texture, 
                                  Vector2 _position = default(Vector2), 
                                  float _rotation = 0.0f, 
                                  float _radius = 50.0f)
            {
                Texture = _texture;
                Position = _position;
                Rotation = _rotation;
                Radius = _radius;
            }

            public void Update(float dt)
            {
                // force decomposition
                Vector2 m = Vector2.Divide(Displacement, Displacement.Length());
                Vector2 Fa = Vector2.Dot(Force, m) * m;
                Vector2 Fb = Force - Fa;

                // translation update
                this.Velocity += Fa/Mass * dt;
                this.Position += this.Velocity * dt;

                // rotation update
                float Torque = Fb.Length() * Displacement.Length();
                float I = MathHelper.PiOver2 * this.Mass * this.Radius;
                this.w += Torque / I * dt;
                this.Rotation += this.w * dt;
            }

            public void Draw()
            {
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
