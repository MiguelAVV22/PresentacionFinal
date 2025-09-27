
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    internal class Escenario
    {
        [JsonProperty(Order = 1)]
        public Dictionary<string, Objeto> objetos;
        private float originX, originY, originZ;

        // Constructor sin parámetros para JSON
        public Escenario()
        {
            originX = 0;
            originY = 0;
            originZ = 0;
            objetos = new Dictionary<string, Objeto>();
        }

        public Escenario(float originX, float originY, float originZ)
        {
            this.originX = originX;
            this.originY = originY;
            this.originZ = originZ;
            objetos = new Dictionary<string, Objeto>();
        }


        public void AgregarObjeto(string nombre, Objeto objeto)
        {
            objetos.Add(nombre, objeto);
        }

        public Objeto get(string nombreobjeto)
        {
            if (objetos.ContainsKey(nombreobjeto))
            {
                return objetos[nombreobjeto];
            }
            else
            {
                throw new Exception($"La parte {nombreobjeto} no existe en este objeto.");
            }
        }

        public void Trasladar(float x, float y, float z)
        {
            foreach (Objeto objeto in objetos.Values)
            {
                objeto.Trasladar(x, y, z);
            }
        }
        public void Escalar(float n)
        {
            foreach (Objeto objeto in objetos.Values)
            {
                objeto.Escalar(n);
            }
        }
        public void Rotar(string eje, float angulo)
        {
            foreach (Objeto objeto in objetos.Values)
            {
                objeto.Rotar(eje, angulo);
            }
        }

        public void Reflejar(string eje)
        {
            foreach (Objeto objeto in objetos.Values)
            {
                objeto.Reflejar(eje);
            }
        }
        public void Dibujar()
        {
            GL.PushMatrix();
            GL.Translate(originX, originY, originZ);
            foreach (Objeto objeto in objetos.Values)
            {
                objeto.Dibujar();
            }
            GL.PopMatrix();
        }
    }
}