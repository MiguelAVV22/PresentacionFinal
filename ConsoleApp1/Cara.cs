using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    // La clase Cara representa una cara de un objeto tridimensional.
    public class Cara
    {
        // Lista de vértices que forman la cara.
        public List<Vertice> Vertices { get; set; }

        // Propiedades de color RGB
        public float Red { get; set; } = 0.65f;
        public float Green { get; set; } = 0.0f;
        public float Blue { get; set; } = 0.0f;

        private Matrix4 MatrizTransformacion = Matrix4.Identity;

        // Constructor de la clase Cara.
        public Cara()
        {
            // Inicializa la lista de vértices.
            Vertices = new List<Vertice>();
        }

        // Constructor con color personalizado
        public Cara(float red, float green, float blue)
        {
            Vertices = new List<Vertice>();
            Red = red;
            Green = green;
            Blue = blue;
        }

        public void Trasladar(float x, float y, float z)
        {
            MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateTranslation(x, y, z));
        }

        public void Escalar(float n)
        {
            MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateScale(n));
        }
        public void Rotar(string eje, float angulo)
        {
            float radians = MathHelper.DegreesToRadians(angulo);
            if (eje == "x")
                MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateRotationX(radians));
            else if (eje == "y")
                MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateRotationY(radians));
            else if (eje == "z")
                MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateRotationZ(radians));
        }

        public void Reflejar(string eje)
        {
            if (eje == "x")
                MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateScale(-1.0f, 1.0f, 1.0f));
            else if (eje == "y")
                MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateScale(1.0f, -1.0f, 1.0f));
            else if (eje == "z")
                MatrizTransformacion = Matrix4.Mult(MatrizTransformacion, Matrix4.CreateScale(1.0f, 1.0f, -1.0f));
        }
        // Método para dibujar la cara.
        public void Dibujar()
        {
        
            GL.Begin(PrimitiveType.Polygon);

           
            SetColor();

          
            foreach (Vertice vertex in Vertices)
            {
                Vector4 Trasformado = Vector4.Transform(new Vector4((float)vertex.X, (float)vertex.Y, (float)vertex.Z, 1), MatrizTransformacion);
                GL.Vertex4(Trasformado);
            }

         
            GL.End();

        }
        private void SetColor()
        {
            GL.Color3(Red, Green, Blue);
        }
        public void LoadVertices(Vertice[] vertices)
        {
            foreach (Vertice vertex in vertices)
            {
                Vertices.Add(vertex);
            }
        }

    }
}