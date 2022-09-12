using System;
using System.Collections.Generic;

namespace FrameworkDotNetExtended.Entities
{
    public class AnoMes
    {
        public int Ano { get; set; }
        public int Mes { get; set; }

        public string Id
        {
            get
            {
                return this.Ano + "/" + this.Mes;
            }
        }
        public int Numeric
        {
            get
            {
                return int.Parse(this.Mes.ToString().PadLeft(4, '0') + this.Mes.ToString().PadLeft(4, '0'));
            }
        }

        public String Descricao
        {
            get
            {
                string descricaoMes = "";
                switch (this.Mes)
                {
                    case 1:
                        descricaoMes = "Janeiro";
                        break;
                    case 2:
                        descricaoMes = "Fevereiro";
                        break;
                    case 3:
                        descricaoMes = "Março";
                        break;
                    case 4:
                        descricaoMes = "Abril";
                        break;
                    case 5:
                        descricaoMes = "Maio";
                        break;
                    case 6:
                        descricaoMes = "Junho";
                        break;
                    case 7:
                        descricaoMes = "Julho";
                        break;
                    case 8:
                        descricaoMes = "Agosto";
                        break;
                    case 9:
                        descricaoMes = "Setembro";
                        break;
                    case 10:
                        descricaoMes = "Outubro";
                        break;
                    case 11:
                        descricaoMes = "Novembro";
                        break;
                    case 12:
                        descricaoMes = "Dezembro";
                        break;
                }
                return descricaoMes + "/" + this.Ano;
            }
        }

        public AnoMes()
        {
            this.Ano = DateTime.Now.Year;
            this.Mes = DateTime.Now.Month;
        }

        public AnoMes(DateTime dateTime)
        {
            this.Ano = dateTime.Year;
            this.Mes = dateTime.Month;
        }

        public AnoMes(string id)
        {
            string[] numeros = id.Split('/');

            this.Ano = int.Parse(numeros[0]);
            this.Mes = int.Parse(numeros[1]);
        }

        public static IList<AnoMes> GenerateRange(DateTime reference, int months)
        {
            IList<AnoMes> retorno = new List<AnoMes>();

            DateTime startDate = new DateTime(reference.Year, reference.Month, 15, 0, 0, 0).AddDays(months * (-30));
            DateTime endDate = new DateTime(reference.Year, reference.Month, 15, 0, 0, 0).AddDays(months * (30));

            while (startDate < endDate)
            {
                retorno.Add(new AnoMes(startDate));
                startDate = startDate.AddDays(30);
            }

            return retorno;
        }

    }
}
