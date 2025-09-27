using OpenTK;
using System;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace ConsoleApp1
{
    public class Game : GameWindow
    {
        private Escenario Escenario1;
        private object objetoSeleccionado = null;
        private string transformacionActual = null;
        private string ejeTransformacion = "x"; // eje por defecto

        private float cameraX = 0.0f;
        private float cameraY = 0.0f;
        private float cameraZ = -15.0f;
        private float cameraRotationY = 0.0f;
        private float cameraRotationX = 0.0f;

        private float angulo = 0.0f;

        public Game() : base(800, 600)
        {
            string json = File.ReadAllText(@"C:\Proga-Grafica\PresentacionGrafica\ConsoleApp1\serializado.txt");
            Escenario1 = JsonConvert.DeserializeObject<Escenario>(json);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.Green);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            ProcesarEntrada();
        }

        private void ProcesarEntrada()
        {
            var state = Keyboard.GetState();

            // === Selección del objeto/parte ===
            if (state.IsKeyDown(Key.M)) objetoSeleccionado = Escenario1;                   // Todo el escenario
            if (state.IsKeyDown(Key.C)) objetoSeleccionado = Escenario1.get("CPU");        // CPU
            if (state.IsKeyDown(Key.D)) objetoSeleccionado = Escenario1.get("Teclado");    // Teclado
            if (state.IsKeyDown(Key.F)) objetoSeleccionado = Escenario1.get("Monitor");    // Monitor completo
            if (state.IsKeyDown(Key.G)) objetoSeleccionado = Escenario1.get("Monitor").get("Carcasa");
            if (state.IsKeyDown(Key.H)) objetoSeleccionado = Escenario1.get("Monitor").get("Pantalla");
            if (state.IsKeyDown(Key.J)) objetoSeleccionado = Escenario1.get("Monitor").get("Soporte");
            if (state.IsKeyDown(Key.K)) objetoSeleccionado = Escenario1.get("Monitor").get("Base");

            // === Selección de transformación ===
            if (state.IsKeyDown(Key.E)) transformacionActual = "Escalar";
            if (state.IsKeyDown(Key.R)) transformacionActual = "Rotar";
            if (state.IsKeyDown(Key.T)) transformacionActual = "Trasladar";
            if (state.IsKeyDown(Key.Q)) transformacionActual = "Reflejar";

            // === Selección de eje ===
            if (state.IsKeyDown(Key.U)) ejeTransformacion = "x";
            if (state.IsKeyDown(Key.I)) ejeTransformacion = "y";
            if (state.IsKeyDown(Key.O)) ejeTransformacion = "z";

            // === Aplicar transformación continuamente ===
            if (objetoSeleccionado != null && !string.IsNullOrEmpty(transformacionActual))
            {
                // Ajustamos la intensidad para que se note más por frame
                float deltaTraslacion = 0.1f;
                float deltaRotacion = 2.0f;
                float deltaEscala = 0.02f;

                if (state.IsKeyDown(Key.Plus) || state.IsKeyDown(Key.KeypadPlus))
                {
                    if (transformacionActual == "Reflejar")
                    {
                        // Para reflexión, aplicamos solo una vez por presión
                        AplicarTransformacion(objetoSeleccionado, transformacionActual, ejeTransformacion,
                            0, 0, 0, "+");
                    }
                    else
                    {
                        AplicarTransformacion(objetoSeleccionado, transformacionActual, ejeTransformacion,
                            deltaTraslacion, deltaRotacion, deltaEscala, "+");
                    }
                }

                if (state.IsKeyDown(Key.Minus) || state.IsKeyDown(Key.KeypadMinus))
                {
                    if (transformacionActual == "Reflejar")
                    {
                        // Para reflexión, aplicamos solo una vez por presión
                        AplicarTransformacion(objetoSeleccionado, transformacionActual, ejeTransformacion,
                            0, 0, 0, "-");
                    }
                    else
                    {
                        AplicarTransformacion(objetoSeleccionado, transformacionActual, ejeTransformacion,
                            -deltaTraslacion, -deltaRotacion, -deltaEscala, "-");
                    }
                }
            }

            // === Cámara ===
            
            if (state.IsKeyDown(Key.Number3)) cameraRotationY += 1.0f;
            if (state.IsKeyDown(Key.Number1)) cameraRotationY -= 1.0f;
            if (state.IsKeyDown(Key.Number5)) cameraRotationX -= 1.0f;
            if (state.IsKeyDown(Key.Number2)) cameraRotationX += 1.0f;
            if (state.IsKeyDown(Key.Number9)) cameraZ += 0.2f;
            if (state.IsKeyDown(Key.Number8)) cameraZ -= 0.2f;

            
        }

        private void AplicarTransformacion(object obj, string tipo, string eje,
                                           float deltaTraslacion, float deltaRotacion, float deltaEscala,
                                           string direccion)
        {
            string nombre = obj is Objeto ? "Objeto" : "Parte";

            if (obj is Objeto o)
            {
                if (tipo == "Escalar")
                    o.Escalar(1 + deltaEscala);
                else if (tipo == "Rotar")
                    o.Rotar(eje, deltaRotacion);
                else if (tipo == "Trasladar")
                    o.Trasladar(
                        eje == "x" ? deltaTraslacion : 0,
                        eje == "y" ? deltaTraslacion : 0,
                        eje == "z" ? deltaTraslacion : 0
                    );
                else if (tipo == "Reflejar")
                    o.Reflejar(eje);
            }
            else if (obj is Parte p)
            {
                if (tipo == "Escalar")
                    p.Escalar(1 + deltaEscala);
                else if (tipo == "Rotar")
                    p.Rotar(eje, deltaRotacion);
                else if (tipo == "Trasladar")
                    p.Trasladar(
                        eje == "x" ? deltaTraslacion : 0,
                        eje == "y" ? deltaTraslacion : 0,
                        eje == "z" ? deltaTraslacion : 0
                    );
                else if (tipo == "Reflejar")
                    p.Reflejar(eje);
            }

            // Mensaje en consola para feedback
            Console.WriteLine($"{tipo} → {nombre} en eje {eje.ToUpper()} ({direccion})");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            ConfigurarProyeccion();
            ConfigurarVista();

            DibujarEscenario();

            SwapBuffers();
        }

        private void ConfigurarProyeccion()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-1.0, 1.0, -1.0, 1.0, 1.0, 100.0);
        }

        private void ConfigurarVista()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(cameraX, cameraY, cameraZ);
            GL.Rotate(cameraRotationX, 1.0f, 0.0f, 0.0f);
            GL.Rotate(cameraRotationY, 0.0f, 1.0f, 0.0f);
        }

        private void DibujarEscenario()
        {
            Escenario1.Dibujar();

            angulo += 1f;
            if (angulo > 360f) angulo = 0.0f;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }
    }
}
