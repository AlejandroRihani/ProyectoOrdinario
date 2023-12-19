using ProyectoOrdinario.Enumeradores;
using ProyectoOrdinario.Interfaces;
using System;
using System.Collections.Generic;

namespace ProyectoOrdinario.Clases
{
    internal class JugadorPoker : IJugador
    {
        private List<ICarta> mano;
        private readonly IDealer dealer;

        public JugadorPoker(IDealer dealer)
        {
            mano = new List<ICarta>();
            this.dealer = dealer;
        }

        public void RealizarJugada()
        {
            Console.WriteLine($"El jugador en Poker realiza una jugada:");

            // Simulate player's moves
            // In Poker, players can exchange up to 4 cards
            for (int i = 0; i < 4; i++)
            {
                // Exchange a card with the dealer
                ICarta cartaDevolvida = dealer.RepartirCartas(1).FirstOrDefault();
                ICarta cartaIntercambiada = DevolverCarta(i);
                ObtenerCartas(new List<ICarta> { cartaDevolvida });

                Console.WriteLine($"El jugador ha intercambiado una carta. Nueva carta: {cartaDevolvida.Valor} de {cartaDevolvida.Figura}");
                MostrarEstadoMano();
            }

            Console.WriteLine("El jugador ha finalizado su jugada.");
        }

        public void ObtenerCartas(List<ICarta> cartas)
        {
            mano.AddRange(cartas);
        }

        public ICarta DevolverCarta(int indiceCarta)
        {
            if (indiceCarta >= 0 && indiceCarta < mano.Count)
            {
                ICarta carta = mano[indiceCarta];
                mano.RemoveAt(indiceCarta);
                return carta;
            }
            else
            {
                Console.WriteLine("Índice de carta inválido.");
                return null;
            }
        }

        public List<ICarta> DevolverTodasLasCartas()
        {
            List<ICarta> cartas = new List<ICarta>(mano);
            mano.Clear();
            return cartas;
        }

        public List<ICarta> MostrarCartas()
        {
            return new List<ICarta>(mano);
        }

        public ICarta MostrarCarta(int indiceCarta)
        {
            if (indiceCarta >= 0 && indiceCarta < mano.Count)
            {
                return mano[indiceCarta];
            }
            else
            {
                Console.WriteLine("Índice de carta inválido.");
                return null;
            }
        }

        private void MostrarEstadoMano()
        {
            Console.WriteLine("Estado de la mano del jugador:");
            foreach (var carta in mano)
            {
                Console.WriteLine($"   {carta.Valor} de {carta.Figura}");
            }
        }
    }
}