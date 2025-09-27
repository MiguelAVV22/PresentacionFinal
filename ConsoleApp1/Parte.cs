using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ConsoleApp1
{
    internal class Parte
    {
        [JsonProperty(Order = 1)]
        public Vertice CentroDeMasa { get;  set; }
        [JsonProperty(Order = 2)]
        public Dictionary<string, Cara> caras;

        // Constructor sin parámetros para JSON
        public Parte()
        {
            caras = new Dictionary<string, Cara>();
            CentroDeMasa = new Vertice(0, 0, 0);
        }

        public Parte(Vertice centroDeMasa)
        {
            caras = new Dictionary<string, Cara>();
            CentroDeMasa = centroDeMasa;
        }
        
        public void AgregarCara(string nombre, Cara cara)
        {
            caras.Add(nombre, cara);
        }

        public void Trasladar(float x, float y, float z)
        {
            foreach (Cara Cara1 in caras.Values)
            {
                Cara1.Trasladar(x, y, z);
            }
        }
        public void Escalar(float n)
        {
            foreach (Cara cara in caras.Values)
            {
                cara.Escalar(n);
            }
        }
        public void Rotar(string eje, float angulo)
        {
            foreach (Cara cara in caras.Values)
            {
                cara.Rotar(eje, angulo);
            }
        }

        public void Reflejar(string eje)
        {
            foreach (Cara cara in caras.Values)
            {
                cara.Reflejar(eje);
            }
        }
        public void Dibujar()
        {
            GL.PushMatrix();
            GL.Translate((float)CentroDeMasa.X, (float)CentroDeMasa.Y, (float)CentroDeMasa.Z);
            foreach (Cara cara in caras.Values)
            {
                cara.Dibujar();
            }
            GL.PopMatrix();
            
        }

        
    }
}