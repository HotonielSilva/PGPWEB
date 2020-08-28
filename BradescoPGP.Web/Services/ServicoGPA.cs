//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using BradescoPGP.Repositorio.GPA;
//using MySql.Data.MySqlClient;

//namespace BradescoPGP.Repositorio.ServicosGPA
//{
//    public class ServicoGPA
//    {
//        private List<Segmento> Segmentos = new List<Segmento>();
//        private List<SelectListItem> Especialistas = new List<SelectListItem>();
       
//        public BoletadorCentral Obter(int codigoBoletadorCentral)
//        {
//            using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["GPA"].ConnectionString))
//            {
//                conn.Open();

//                var cmd = new MySqlCommand("pBoletadorCentral", conn);
//                cmd.CommandType = System.Data.CommandType.StoredProcedure;
//                cmd.Parameters.AddWithValue("@operacao", "OBTE");
//                cmd.Parameters.AddWithValue("@p_cBoletadorCentral", codigoBoletadorCentral);
//                cmd.Parameters.AddWithValue("@p_cBoletadorPresencial", 0);
//                cmd.Parameters.AddWithValue("@p_cMaquinaPresencial","");
//                cmd.Parameters.AddWithValue("@p_dtEvento",default(DateTime));
//                cmd.Parameters.AddWithValue("@p_rHorario","");
//                cmd.Parameters.AddWithValue("@p_cSegmento",0);
//                cmd.Parameters.AddWithValue("@p_cAg",0);
//                cmd.Parameters.AddWithValue("@p_rGerente","");
//                cmd.Parameters.AddWithValue("@p_cEvento",0);
//                cmd.Parameters.AddWithValue("@p_cSubEvento",0);
//                cmd.Parameters.AddWithValue("@p_cConsultor","");
//                cmd.Parameters.AddWithValue("@p_vQtde",0);
//                cmd.Parameters.AddWithValue("@p_cPepAgPub",0);
//                cmd.Parameters.AddWithValue("@p_rNome","");
//                cmd.Parameters.AddWithValue("@p_rEndereco","");
//                cmd.Parameters.AddWithValue("@p_cCta",0);
//                cmd.Parameters.AddWithValue("@p_rDig","");
//                cmd.Parameters.AddWithValue("@p_rCargo","");
//                cmd.Parameters.AddWithValue("@p_rOrgao","");
//                cmd.Parameters.AddWithValue("@p_rGrpEconomico","");
//                cmd.Parameters.AddWithValue("@p_cPipeline",0);
//                cmd.Parameters.AddWithValue("@p_vPipeline",0);
//                cmd.Parameters.AddWithValue("@p_dtPipeline", default(DateTime));
//                cmd.Parameters.AddWithValue("@p_cNegocioRealizado",0);
//                cmd.Parameters.AddWithValue("@p_cProduto",0);
//                cmd.Parameters.AddWithValue("@p_vValor",0);
//                cmd.Parameters.AddWithValue("@p_vTaxa",0);
//                cmd.Parameters.AddWithValue("@p_cConcorrente",0);
//                cmd.Parameters.AddWithValue("@p_cNegocioPerdido",0);
//                cmd.Parameters.AddWithValue("@p_rDescritivo","");
//                cmd.Parameters.AddWithValue("@p_cForaProgramacao",0);
//                cmd.Parameters.AddWithValue("@p_cVisitaCancelada",0);
//                cmd.Parameters.AddWithValue("@p_rAutorizante","");
//                cmd.Parameters.AddWithValue("@p_dtUltAtualizacao", default(DateTime));
//                cmd.Parameters.AddWithValue("@p_dtImport", default(DateTime));
//                cmd.Parameters.AddWithValue("@p_cTipoCanal",0);
//                cmd.Parameters.AddWithValue("@p_cTipoContato",0);
//                cmd.Parameters.AddWithValue("@p_cOrigemContato",0);
//                cmd.Parameters.AddWithValue("@p_cFila",0);
//                cmd.Parameters.AddWithValue("@p_cModulo",0);
//                cmd.Parameters.AddWithValue("@p_cIndicouCorretora",0);
//                cmd.Parameters.AddWithValue("@p_cUsuar","");
//                cmd.Parameters.AddWithValue("@p_cPrfilAcsso",0);
                
//                using (var reader = cmd.ExecuteReader())
//                {
//                    var boletadorCentral = default(BoletadorCentral);

//                    if (reader.Read())
//                    {
//                        var teste = (int)reader["cBoletadorCentral"];
//                    }

//                    boletadorCentral = BoletadorCentral.Mapear(reader);
//                    return boletadorCentral;
//                }
                
//            }
//        }

//        public List<Segmento> SegmentosCombo()
//        {
//            using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["GPA"].ConnectionString))
//            {
//                conn.Open();

//                var cmd = new MySqlCommand("pSegmentos", conn);
//                cmd.CommandType = System.Data.CommandType.StoredProcedure;
//                cmd.Parameters.AddWithValue("OPERACAO", "COMB");
//                cmd.Parameters.AddWithValue("p_cSegmento", default(int));
//                cmd.Parameters.AddWithValue("p_rSegmento", "");
//                cmd.Parameters.AddWithValue("p_cAtivo", default(decimal));

//                using (var reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        var segmento = new Segmento();
//                        segmento.rSegmento = reader["rSegmento"].ToString();
//                        segmento.cSegmento = reader["cSegmento"].ToString();
//                        Segmentos.Add(segmento);
//                    }

//                    return Segmentos;
//                }
//            }
//        }

//        public List<SelectListItem> EspecialistasFormatoGPA()
//        {
//            using (var conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["GPA"].ConnectionString))
//            {
//                conn.Open();

//                var cmd = new MySqlCommand("pUsuar", conn);
//                cmd.CommandType = System.Data.CommandType.StoredProcedure;
//                cmd.Parameters.AddWithValue("OPERACAO", "COMB");
//                cmd.Parameters.AddWithValue("p_cFuncBdsco", "");
//                cmd.Parameters.AddWithValue("p_iFuncBdsco", "");
//                cmd.Parameters.AddWithValue("p_rEmailUsuar", "");
//                cmd.Parameters.AddWithValue("p_nFoneUsuar", default(decimal));
//                cmd.Parameters.AddWithValue("p_cPrfilAcsso", default(decimal));
//                cmd.Parameters.AddWithValue("p_cEstAtivo", default(decimal));
//                cmd.Parameters.AddWithValue("p_cUsuarUltAtulz", "");
//                cmd.Parameters.AddWithValue("p_dHoraUltAtulz", default(DateTime));
//                cmd.Parameters.AddWithValue("p_dHoraGerPade", default(DateTime));
//                cmd.Parameters.AddWithValue("p_dHoraSincPade", default(DateTime));
//                cmd.Parameters.AddWithValue("p_DadosPade", default(byte[]));
//                cmd.Parameters.AddWithValue("p_cFuncBdscoBack", "");

//                using (var reader = cmd.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        Especialistas.Add(new SelectListItem {
//                            Text = reader["iFuncBdsco"].ToString(),
//                            Value = reader["cFuncBdsco"].ToString()
//                        });
//                    }
//                    Especialistas.Insert(0, new SelectListItem { Text = ":: Selecione ::", Value = "", Selected = true });

//                    return Especialistas ;
//                }
//            }
	        
//        }
//    }
//}
