﻿/*
 * Nome: pazienti_cls.cs
 * Progetto: Ospedale
 * Gruppo: 3
 * Partecipanti del gruppo: Condello Alessandro, Bergantin Andrea, Gavinelli Riccardo
 * Ultima modifica: 04/05/2020 
 */
namespace ospedale
{
    // Eredito delle variabili dalla classe persona
    public class paziente_cls : persona_cls
    {
        // Variabili globali del paziente
        public char sesso;
        public int n_stanza;
        public int n_letto;
        public string data_nascita;
        // Variabili private
        private string data_ricovero;
        public string Data_Ricovero
        {
            get { return data_ricovero; }
            set { data_ricovero = value; }

        }
        private string tessera_sanitaria;
        public string Tessera_Sanitaria
        {
            get { return tessera_sanitaria; }
            set { tessera_sanitaria = value; }

        }
        private string malattia;
        public string Malattia
        {
            get { return malattia; }
            set { malattia = value; }

        }
        private string dimessione_decesso;
        public string Dimessione_Decesso
        {
            get { return dimessione_decesso; }
            set { dimessione_decesso = value; }

        }

        // Costruttore
        public paziente_cls(string nome, string cognome, char sesso, string data_nascita, string data_ricovero, int n_stanza, int n_letto, string malattia, string dim_dec, string tessera_sanitaria)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.sesso = sesso;
            this.data_nascita = data_nascita;
            this.n_letto = n_letto;
            this.n_stanza = n_stanza;
            Data_Ricovero = data_ricovero;
            Tessera_Sanitaria = tessera_sanitaria;
            Malattia = malattia;
            Dimessione_Decesso = dim_dec;
        }

    }
}
