using ProyectoOrdinario.Enumeradores;
using ProyectoOrdinario.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoOrdinario.Clases
{
    public class JuegoPoker : IJuego
    {
        private List<IJugador> jugadores;
        private IDealer dealer;
        private IDeckDeCartas deck;
        private bool juegoTerminado;

        public JuegoPoker()
        {
            this.jugadores = new List<IJugador>();
            this.dealer = new Dealer();
            this.deck = new DeckDeCartas();
            this.juegoTerminado = false;
        }

        public IDealer Dealer => dealer;

        public bool JuegoTerminado => juegoTerminado;

        public void AgregarJugador(IJugador jugador)
        {
            jugadores.Add(jugador);
        }

        public void IniciarJuego()
        {
            Console.WriteLine("Iniciando juego de Poker...");
            // Barajear el mazo antes de cada juego
            deck.BarajearDeck();
        }

        public void JugarRonda()
        {
            Console.WriteLine("Iniciando nueva ronda...");

            // Repartir cinco cartas a cada jugador
            foreach (var jugador in jugadores)
            {
                jugador.ObtenerCartas(dealer.RepartirCartas(5));
            }

            // Mostrar las cartas iniciales
            MostrarEstadoJuego();

            // Permitir que cada jugador realice un intercambio de cartas
            foreach (var jugador in jugadores)
            {
                Console.WriteLine($"Turno del jugador número {jugadores.IndexOf(jugador)}");
                jugador.RealizarJugada();
            }

            // Mostrar el estado final del juego
            MostrarEstadoJuego();

            // Determinar al ganador y finalizar la ronda
            MostrarGanador();
            juegoTerminado = true;
        }

        public void MostrarGanador()
        {
            Console.WriteLine("Determinando al ganador...");

            IJugador ganador = null;
            int maxHandRank = 0;

            foreach (var jugador in jugadores)
            {
                int handRank = EvaluarMano(jugador.MostrarCartas());

                if (handRank > maxHandRank)
                {
                    maxHandRank = handRank;
                    ganador = jugador;
                }
            }

            if (ganador != null)
            {
                Console.WriteLine($"Ganador: {jugadores.IndexOf(ganador)} Mano: ");
                MostrarManoJugador(ganador);
            }
            else
            {
                Console.WriteLine("Es un empate.");
            }
        }

        private int EvaluarMano(List<ICarta> mano)
        {
            // Ordenar las cartas por valor
            mano = mano.OrderBy(carta => (int)carta.Valor).ToList();

            if (EsEscaleraReal(mano)) return 9;
            if (EsEscaleraColor(mano)) return 8;
            if (EsPoker(mano)) return 7;
            if (EsFullHouse(mano)) return 6;
            if (EsColor(mano)) return 5;
            if (EsEscalera(mano)) return 4;
            if (EsTrio(mano)) return 3;
            if (EsDoblePar(mano)) return 2;
            if (EsPar(mano)) return 1;

            return (int)mano.Last().Valor; // Valor de la carta más alta
        }

        private string TraducirRankMano(int rank)
        {
            switch (rank)
            {
                case 9:
                    return "Escalera Real";
                case 8:
                    return "Escalera de Color";
                case 7:
                    return "Poker";
                case 6:
                    return "Full House";
                case 5:
                    return "Color";
                case 4:
                    return "Escalera";
                case 3:
                    return "Trío";
                case 2:
                    return "Doble Par";
                case 1:
                    return "Par";
                default:
                    return "Carta Alta";
            }
        }

        private bool EsEscaleraReal(List<ICarta> mano)
        {
            if (mano.All(carta => carta.Valor >= ValoresCartasEnum.Diez) && EsEscaleraColor(mano))
            {
                return true;
            }
            return false;
        }

        private bool EsEscaleraColor(List<ICarta> mano)
        {
            return EsColor(mano) && EsEscalera(mano);
        }

        private bool EsPoker(List<ICarta> mano)
        {
            var grupos = mano.GroupBy(carta => carta.Valor);

            foreach (var grupo in grupos)
            {
                if (grupo.Count() == 4)
                {
                    return true;
                }
            }
            return false;
        }

        private bool EsFullHouse(List<ICarta> mano)
        {
            var grupos = mano.GroupBy(carta => carta.Valor);

            bool tieneTrio = false;
            bool tienePar = false;

            foreach (var grupo in grupos)
            {
                if (grupo.Count() == 3)
                {
                    tieneTrio = true;
                }
                else if (grupo.Count() == 2)
                {
                    tienePar = true;
                }
            }

            return tieneTrio && tienePar;
        }

        private bool EsColor(List<ICarta> mano)
        {
            return mano.All(carta => carta.Figura == mano.First().Figura);
        }

        private bool EsEscalera(List<ICarta> mano)
        {
            for (int i = 0; i < mano.Count - 1; i++)
            {
                if ((int)mano[i + 1].Valor - (int)mano[i].Valor != 1)
                {
                    return false;
                }
            }
            return true;
        }

        private bool EsTrio(List<ICarta> mano)
        {
            var grupos = mano.GroupBy(carta => carta.Valor);

            foreach (var grupo in grupos)
            {
                if (grupo.Count() == 3)
                {
                    return true;
                }
            }
            return false;
        }

        private bool EsDoblePar(List<ICarta> mano)
        {
            var grupos = mano.GroupBy(carta => carta.Valor);

            int cantidadPares = 0;

            foreach (var grupo in grupos)
            {
                if (grupo.Count() == 2)
                {
                    cantidadPares++;
                }
            }

            return cantidadPares == 2;
        }

        private bool EsPar(List<ICarta> mano)
        {
            var grupos = mano.GroupBy(carta => carta.Valor);

            foreach (var grupo in grupos)
            {
                if (grupo.Count() == 2)
                {
                    return true;
                }
            }
            return false;
        }

        private void MostrarEstadoJuego()
        {
            Console.WriteLine("\nEstado actual del juego:");

            // Mostrar las cartas de cada jugador
            foreach (var jugador in jugadores)
            {
                Console.WriteLine($"Cartas de {jugador.GetType().Name}:");
                foreach (var carta in jugador.MostrarCartas())
                {
                    Console.WriteLine($"   {carta.Valor} de {carta.Figura}");
                }
                Console.WriteLine();
            }
        }
        private void MostrarManoJugador(IJugador jugador)
        {
            foreach(var carta in jugador.MostrarCartas())
            {
                Console.WriteLine($"{carta.Valor} de {carta.Figura}");
            }        
        }
    }
}