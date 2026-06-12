using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema_de_controle_de_estacionamento.Models
{
    internal class Veículo
    {
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }

        public DateTime? DataHoraEntrada { get; set; }
        public DateTime? DataHoraSaida { get; set; }

        public bool Estacionado { get; set; }
        public bool ValorPago { get; set; }

    }
}
