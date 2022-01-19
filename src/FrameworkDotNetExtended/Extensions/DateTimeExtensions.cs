using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkDotNetExtended.Extensions
{
    /// <summary>
    /// Enum que indica qual o formato da data é requerido.
    /// </summary>
    public enum FormatoData
    {
        DDMMYY,
        DDMMYYYY,
        DDMMYYHoraMinSeg,
        DDMMYYYYHoraMinSeg,
        DDMMYYHoraMin,
        DDMMYYYYHoraMin,
        YYMMDDHoraMinSeg,
        YYYYMMDDHoraMinSeg,
        YYMMDDHoraMin,
        YYYYMMDDHoraMin,
        YYYYMMDD,
        YYMMDD,
        DiaSemanaDDMesYYYY,
        DiaSemanaDDMesYY
    }

    public static class DateTimeExtensions
    {


        /// <summary>
        /// Método que retorna uma string com a data formatada no formato desejado.
        /// </summary>   
        /// <example>
        ///     Ex: data = 13/03/2007
        ///         formatoData = FormatoData.DiaSemanaDDMesYYYY
        ///         retorno = terça-feira, 13 de março de 2007
        /// </example>
        /// <param name="data"></param>
        /// <param name="formatoData"></param>
        /// <returns></returns>                
        public static string FormatarData(this DateTime data, FormatoData formatoData)
        {
            string dataRetornada = string.Empty;
            string mascara = string.Empty;

            switch (formatoData)
            {
                case FormatoData.DDMMYY:
                    mascara = "dd/MM/yy";
                    break;
                case FormatoData.DDMMYYYY:
                    mascara = "dd/MM/yyyy";
                    break;
                case FormatoData.DDMMYYHoraMinSeg:
                    mascara = "dd/MM/yy HH:mm:ss";
                    break;
                case FormatoData.DDMMYYYYHoraMinSeg:
                    mascara = "dd/MM/yyyy HH:mm:ss";
                    break;
                case FormatoData.DDMMYYHoraMin:
                    mascara = "dd/MM/yy HH:mm";
                    break;
                case FormatoData.DDMMYYYYHoraMin:
                    mascara = "dd/MM/yyyy HH:mm";
                    break;
                case FormatoData.YYMMDDHoraMinSeg:
                    mascara = "yy/MM/dd HH:mm:ss";
                    break;
                case FormatoData.YYYYMMDDHoraMinSeg:
                    mascara = "yyyy/MM/dd HH:mm:ss";
                    break;
                case FormatoData.YYMMDDHoraMin:
                    mascara = "yy/MM/dd HH:mm";
                    break;
                case FormatoData.YYYYMMDDHoraMin:
                    mascara = "yyyy/MM/dd HH:mm";
                    break;
                case FormatoData.YYYYMMDD:
                    mascara = "yyyy/MM/dd";
                    break;
                case FormatoData.YYMMDD:
                    mascara = "yy/MM/dd";
                    break;
                case FormatoData.DiaSemanaDDMesYYYY:
                    mascara = "dddd, dd {0} MMMM {0} yyyy";
                    break;
                case FormatoData.DiaSemanaDDMesYY:
                    mascara = "dddd, dd {0} MMMM {0} yy";
                    break;
                default:
                    break;
            }

            if (formatoData == FormatoData.DiaSemanaDDMesYY || formatoData == FormatoData.DiaSemanaDDMesYYYY)
            {
                dataRetornada = data.ToString(mascara).Replace("{0}", "de");
            }
            else
            {
                dataRetornada = data.ToString(mascara);
            }

            return dataRetornada;
        }
    }
}
