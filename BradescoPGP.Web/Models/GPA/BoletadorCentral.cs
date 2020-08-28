//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BradescoPGP.Repositorio.GPA
//{
//    public class BoletadorCentral
//    {
//            public int cBoletadorCentral    {get; set;}
//            public decimal? cBoletadorPresencial {get; set;}
//            public string cMaquinaPresencial   {get; set;}
//            public DateTime? dtEvento {get; set;}
//            public string rHorario {get; set;}
//            public decimal? cSegmento {get; set;}
//            public decimal? cAg {get; set;}
//            public string rGerente {get; set;}
//            public decimal? cEvento {get; set;}
//            public decimal? cSubEvento {get; set;}
//            public string cConsultor {get; set;}
//            public decimal? vQtde {get; set;}
//            public decimal? cPepAgPub {get; set;}
//            public string rNome {get; set;}
//            public string rEndereco{get; set;}
//            public decimal? cCta{get; set;}
//            public string rDig{get; set;}
//            public string rCargo{get; set;}
//            public string rOrgao{get; set;}
//            public string rGrpEconomico{get; set;}
//            public decimal? cPipeline{get; set;}
//            public decimal? vPipeline{get; set;}
//            public DateTime? dtPipeline{get; set;}
//            public decimal? cNegocioRealizado{get; set;}
//            public decimal? cProduto{get; set;}
//            public decimal? vValor{get; set;}
//            public decimal? vTaxa{get; set;}
//            public decimal? cConcorrente{get; set;}
//            public decimal? cNegocioPerdido{get; set;}
//            public string rDescritivo{get; set;}
//            public decimal? cForaProgramacao{get; set;}
//            public decimal? cVisitaCancelada{get; set;}
//            public string rAutorizante{get; set;}
//            public DateTime? dtUltAtualizacao{get; set;}
//            public decimal? cStatus{get; set;}
//            public decimal? cFase{get; set;}
//            public DateTime? dtImport{get; set;}
//            public decimal? cTipoCanal{get; set;}
//            public decimal? cTipoContato{get; set;}
//            public decimal? cOrigemContato{get; set;}
//            public decimal? cFila{get; set;}
//            public decimal? cModulo{get; set;}
//            public decimal? cIndicouCorretora{get; set;}
//            public string cUsuar{get; set;}
//            public string rFone{get; set;}
//            public string rEmail { get; set; }

//        public static BoletadorCentral Mapear(MySqlDataReader reader)
//        {
//            DateTime dtImportResult;
//            DateTime dtUltAtuaResult;

//            var dtEvento = default(DateTime);
//            var resultConversao = DateTime.TryParse(reader["dtEvento"].ToString(), out dtEvento);

//            DateTime dtpiperesult;
//            var dtPipe = DateTime.TryParse(reader["dtPipeline"].ToString(), out dtpiperesult);

//            var result = new BoletadorCentral();
//            result.cBoletadorCentral = (int)reader["cBoletadorCentral"];
//            result.cBoletadorPresencial = (decimal)reader["cBoletadorPresencial"];
//            result.cMaquinaPresencial = (string)reader["cMaquinaPresencial"];
//            if (resultConversao) result.dtEvento = dtEvento;
//            result.rHorario = (string)reader["rHorario"];
//            if(!string.IsNullOrEmpty(reader["cSegmento"].ToString())) result.cSegmento = (decimal)reader["cSegmento"];
//            if (!string.IsNullOrEmpty(reader["cAg"].ToString()))  result.cAg = (decimal)reader["cAg"];
//            result.rGerente = (string)reader["rGerente"];
//            if (!string.IsNullOrEmpty(reader["cEvento"].ToString())) result.cEvento = (decimal)reader["cEvento"];
//            if (!string.IsNullOrEmpty(reader["cSubEvento"].ToString())) result.cSubEvento = decimal.Parse(reader["cSubEvento"].ToString());
//            result.cConsultor = (string)reader["cConsultor"];
//            if (!string.IsNullOrEmpty(reader["vQtde"].ToString())) result.vQtde = (decimal)reader["vQtde"];
//            if (!string.IsNullOrEmpty(reader["cPepAgPub"].ToString())) result.cPepAgPub = (decimal)reader["cPepAgPub"];
//            if (!string.IsNullOrEmpty(reader["rNome"].ToString())) result.rNome = (string)reader["rNome"];
//            if (!string.IsNullOrEmpty(reader["rEndereco"].ToString())) result.rEndereco = (string)reader["rEndereco"];
//            if (!string.IsNullOrEmpty(reader["cCta"].ToString())) result.cCta = (decimal)reader["cCta"];
//            result.rDig = (string)reader["rDig"];
//            result.rCargo = (string)reader["rCargo"];
//            result.rOrgao = (string)reader["rOrgao"];
//            result.rGrpEconomico = (string)reader["rGrpEconomico"];
//            if (!string.IsNullOrEmpty(reader["cPipeline"].ToString())) result.cPipeline = (decimal)reader["cPipeline"];
//            if (!string.IsNullOrEmpty(reader["vPipeline"].ToString())) result.vPipeline = (decimal)reader["vPipeline"];
//            if(dtPipe) result.dtPipeline = dtpiperesult;
//            if (!string.IsNullOrEmpty(reader["cNegocioRealizado"].ToString())) result.cNegocioRealizado = (decimal)reader["cNegocioRealizado"];
//            if (!string.IsNullOrEmpty(reader["cProduto"].ToString())) result.cProduto = (decimal)reader["cProduto"];
//            if (!string.IsNullOrEmpty(reader["vValor"].ToString())) result.vValor = (decimal)reader["vValor"];
//            if (!string.IsNullOrEmpty(reader["vTaxa"].ToString())) result.vTaxa = (decimal)reader["vTaxa"];
//            if (!string.IsNullOrEmpty(reader["cConcorrente"].ToString())) result.cConcorrente = (decimal)reader["cConcorrente"];
//            if (!string.IsNullOrEmpty(reader["cNegocioPerdido"].ToString())) result.cNegocioPerdido = (decimal)reader["cNegocioPerdido"];
//            result.rDescritivo = (string)reader["rDescritivo"];
//            if (!string.IsNullOrEmpty(reader["cForaProgramacao"].ToString())) result.cForaProgramacao = (decimal)reader["cForaProgramacao"];
//            if (!string.IsNullOrEmpty(reader["cVisitaCancelada"].ToString())) result.cVisitaCancelada = (decimal)reader["cVisitaCancelada"];
//            result.rAutorizante = (string)reader["rAutorizante"];
//            if (DateTime.TryParse(reader["dtUltAtualizacao"].ToString(), out dtUltAtuaResult)) result.dtUltAtualizacao = dtUltAtuaResult;
//            //Campo não incluso na busca.
//            //if (!string.IsNullOrEmpty(reader["cStatus"].ToString())) result.cStatus = (decimal)reader["cStatus"];
//            //if (!string.IsNullOrEmpty(reader["cFase"].ToString())) result.cFase = (decimal)reader["cFase"];
//            if (DateTime.TryParse(reader["dtImport"].ToString(), out dtImportResult)) result.dtImport = dtImportResult;
//            if (!string.IsNullOrEmpty(reader["cTipoCanal"].ToString())) result.cTipoCanal = (decimal)reader["cTipoCanal"];
//            if (!string.IsNullOrEmpty(reader["cTipoContato"].ToString())) result.cTipoContato = (decimal)reader["cTipoContato"];
//            if (!string.IsNullOrEmpty(reader["cOrigemContato"].ToString())) result.cOrigemContato = (decimal)reader["cOrigemContato"];
//            if (!string.IsNullOrEmpty(reader["cFila"].ToString())) result.cFila = (decimal)reader["cFila"];
//            if (!string.IsNullOrEmpty(reader["cModulo"].ToString())) result.cModulo = (decimal)reader["cModulo"];
//            if (!string.IsNullOrEmpty(reader["cIndicouCorretora"].ToString())) result.cIndicouCorretora = (decimal)reader["cIndicouCorretora"];
//            //result.cUsuar = (string)reader["cUsuar"];
//            //result.rFone = (string)reader["rFone"];
//            //result.rEmail = (string)reader["rEmail"];

//            return result;
//        }
//    }
//}