using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web.Mvc;
using BradescoPGP.Common;
using BradescoPGP.Common.Logging;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using BradescoPGP.Web.Services;
using Newtonsoft.Json;
using OfficeOpenXml;
using static System.Net.Mime.MediaTypeNames;

namespace BradescoPGP.Web.Controllers
{
    [Authorize]
    public abstract class AbstractController : Controller
    {
        private readonly PGPEntities _context;
        public string EquipeUsuario
        {
            get => User.Identity.IsAuthenticated ? ((ClaimsIdentity)User.Identity).FindFirst("equipe").Value : null;
        }
        public string MatriculaUsuario
        {
            get => User.Identity.IsAuthenticated ? ((ClaimsIdentity)User.Identity).FindFirst("matricula").Value : null;
        }
        public string Cargo
        {
            get => User.Identity.IsAuthenticated ? ((ClaimsIdentity)User.Identity).FindFirst("cargo").Value : null;
        }

        public AbstractController(DbContext context)
        {
            _context = context as PGPEntities;
        }

        public ActionResult ObterInvstFacil()
        {
            decimal somacap = 0;
            decimal somaCapSemInvest = 0;
            decimal soma = 0;

            if (User.IsInRole(NivelAcesso.Especialista.ToString()))
            {
                //Investfácil
                var resultado = _context.InvestFacilResumo.Where(i => i.Matricula == MatriculaUsuario).ToList();
                soma = resultado.Sum(s => s.Vlr_Evento);

                //Captação líquida
                var listaCaptacaoes = _context.CaptacaoLiquidaResumo.Where(c => c.MatriculaConsultor == MatriculaUsuario).ToList();

                somacap = listaCaptacaoes.Sum(s => s.VL_NET);
                somaCapSemInvest = listaCaptacaoes.Where(c => !c.ProdutoMacro.ToLower().StartsWith("invest")).Sum(s => s.VL_NET);
            }
            else if (User.IsInRole(NivelAcesso.Master.ToString()))
            {
                //Investfácil
                var resultado = _context.InvestFacilResumo.ToList();
                soma = resultado.Sum(s => s.Vlr_Evento);

                //Captação líquida
                var listaCaptacaoes = _context.CaptacaoLiquidaResumo.ToList();

                somacap = listaCaptacaoes.Sum(s => s.VL_NET);
                somaCapSemInvest = listaCaptacaoes.Where(c => !c.ProdutoMacro.ToLower().StartsWith("invest")).Sum(s => s.VL_NET);
            }
            else
            {
                var invest = _context.InvestFacilResumo.Where(x => x.MatriculaCordenador == MatriculaUsuario).ToList();
                soma = invest.Sum(s => s.Vlr_Evento);

                var captacoes = _context.CaptacaoLiquidaResumo.Where(s => s.MatriculaCordenador == MatriculaUsuario)
                    .ToList();

                somacap = captacoes.Sum(s => s.VL_NET);
                somaCapSemInvest = captacoes.Where(c => c.ProdutoMacro.ToLower().StartsWith("invest")).Sum(s => s.VL_NET);
                somaCapSemInvest = captacoes.Where(c => !c.ProdutoMacro.ToLower().StartsWith("invest")).Sum(s => s.VL_NET);
            }

            return Json(new
            {
                SaldoInvestfacil = soma,
                SaldoCaptacaoLiquida = somacap,
                SaldoCaptacaoLiquidaSemInvest = somaCapSemInvest
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult NovoLink(Links link)
        {
            if (link == null)
            {
                return Json(new { error = "O link não pode ser nulo" }, JsonRequestBehavior.AllowGet);
            }

            using (PGPEntities db = new PGPEntities())
            {
                db.Links.Add(link);
                db.SaveChanges();
            }

            return Json(link, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportarCaptacaoLiquida(bool mesAtual)
        {
            #region Versao Antiga
            //var capLiqService = new CaptacaoLiquidaService();

            //var capLiq = capLiqService.GerarCaptacaoLiquida();

            //if(Cargo == NivelAcesso.Especialista.ToString())
            //{
            //    capLiq = capLiq.Where(s => s.Key == MatriculaUsuario).ToDictionary(k => k.Key, v => v.Value);
            //}
            //else if(Cargo == NivelAcesso.Gestor.ToString())
            //{
            //    capLiq = capLiq.Where(s => s.Value.MatriculaSupervisor == MatriculaUsuario).ToDictionary(k => k.Key, v => v.Value);
            //}

            //return File(GerarExcel(capLiq, "CaptacaoLiquida"), Application.Octet, "Captação Líquida.xlsx");

            #endregion

            var capLiqRepo = new CaptacaoLiquidaRepository();

            var excelService = new CaptacaoLiquidaExcelService();

            var capLiq = default(List<IGrouping<string, CaptacaoLiquida>>);

            var caminhoDinheiroAgrupado = default(List<vw_CaminhoDinheiroAgrupado>);

            var camDinAnalitico = default(List<CaminhoDinheiro>);

            var mesDataBase = mesAtual ? DateTime.Now.ToString("MMM yyyy") : DateTime.Now.AddMonths(-1).ToString("MMM yyyy");

            var anoMes = mesAtual ? DateTime.Now.ToString("yyyyMM") : DateTime.Now.AddMonths(-1).ToString("yyyyMM");

            var query = _context.CaptacaoLiquida.Where(s => s.MesDataBase == mesDataBase);

            if (User.IsInRole(NivelAcesso.Especialista.ToString()))
            {
                capLiq = query
                    .Where(s => s.MatriculaConsultor == MatriculaUsuario)
                    .GroupBy(s => s.Diretoria)
                    .ToList();

                caminhoDinheiroAgrupado = capLiqRepo.ObterCaminhoDinheiroAgrupado(NivelAcesso.Especialista, MatriculaUsuario, anoMes);

                camDinAnalitico = capLiqRepo.ObterCaminhoDinheiroAnalitico(NivelAcesso.Especialista, MatriculaUsuario, anoMes);

            }
            else if (User.IsInRole(NivelAcesso.Gestor.ToString()))
            {
                capLiq = capLiq = query
                    .Where(s => s.MatriculaConsultor == MatriculaUsuario)
                    .GroupBy(s => s.Diretoria)
                    .ToList();

                caminhoDinheiroAgrupado = capLiqRepo.ObterCaminhoDinheiroAgrupado(NivelAcesso.Gestor, MatriculaUsuario, anoMes);
            }
            else
            {
                capLiq = capLiqRepo.ObterCapLiq(mesDataBase:mesDataBase)
                    .GroupBy(s => s.CordenadorPGP).ToList();

                caminhoDinheiroAgrupado = capLiqRepo.ObterCaminhoDinheiroAgrupado(NivelAcesso.Master, MatriculaUsuario, anoMes);
            }

            var camDin = caminhoDinheiroAgrupado.ConvertAll(s => CaminhoDinheiroModel.Mapear(s));

            var excel = excelService.GerarExcelCaptacaoLiquida(ref capLiq, ref camDin, ref camDinAnalitico, $"Captação Liquida - Mês Database {mesDataBase}", Cargo);

            return File(excel, Application.Octet, $"Captação Liquida - Mês Database {mesDataBase}.xlsx");

        }

        public ActionResult ExportarInvestFacil()
        {
            var listaInvestfacil = new List<Investfacil>();

            var nomeArquivoPlanilha = _context.WindowsServiceConfig.FirstOrDefault(c => c.Tarefa == Comando.ImportarInvestFacil.ToString()).UltimoArquivo;

            var excel = default(byte[]);

            if (Cargo == NivelAcesso.Especialista.ToString())
            {
                listaInvestfacil = _context.Investfacil.Where(i => i.MatriculaConsultor == MatriculaUsuario).ToList();

            }
            else if (Cargo == NivelAcesso.Gestor.ToString())
            {
                listaInvestfacil = _context.Investfacil.Where(i => i.MatriculaCordenador == MatriculaUsuario).ToList();
            }
            else
            {
                listaInvestfacil = _context.Investfacil.ToList();
            }

            var nome = nomeArquivoPlanilha.Substring(0, nomeArquivoPlanilha.Length - 4);

            var result = listaInvestfacil.ConvertAll(i => {
                return new InvestFacilExcel
                {
                    AGENCIA = i.AGENCIA,
                    CONTA = i.CONTA,
                    DT_EMISSAO = i.DT_EMISSAO,
                    FX_PERMANENCIA = i.FX_PERMANENCIA,
                    FX_VOLUME = i.FX_VOLUME,
                    MES_DT_BASE = i.MES_DT_BASE,
                    NUM_CONTRATO = i.NUM_CONTRATO,
                    PRAZO_PERMAN = i.PRAZO_PERMAN,
                    SEGMENTO_CLIENTE = i.SEGMENTO_CLIENTE,
                    SEGMENTO_MACRO = i.SEGMENTO_MACRO,
                    SEGM_AGRUPADO = i.SEGM_AGRUPADO,
                    Vlr_Evento = i.Vlr_Evento,
                    Especialista = i.NomeConsultor,
                    Equipe = i.Plataforma
                };
            });

            excel = GerarExcel(result, nome);

            return File(excel, Application.Octet, nome + ".xlsx");

        }

        [HttpPost]
        public JsonResult ExcluirLink(int id)
        {
            using (PGPEntities db = new PGPEntities())
            {
                var link = db.Links.FirstOrDefault(l => l.Id == id);

                db.Links.Remove(link);

                db.SaveChanges();
            }

            return Json(new { status = true });
        }

        public ActionResult Download(String file)
        {
            var fileName = file.Split(new string[] { "file:///" }, StringSplitOptions.RemoveEmptyEntries)[0];

            fileName = Path.Combine(Server.MapPath("~/Download"), new FileInfo(fileName).Name);

            if (!System.IO.File.Exists(fileName))
            {
                Log.InformationFormat("Download: O Arquivo {0} não foi encontrado!", fileName);

                return View("error", new ErrorViewModel { Mensagem = "O arquivo não foi encontrado ou está sendo usado por outro processo", Status = "404", Endereco = string.Empty });
            }

            return File(fileName, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(fileName));
        }

        public JsonResult ObterLinks()
        {
            var viewModalLista = new List<LinkViewModel>();
            PGPEntities db = new PGPEntities();

            var links = db.Links.Where(l => l.Exibir).ToList();
            links.ForEach(l =>
            {
                var link = new LinkViewModel
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    Url = l.Url
                };
                viewModalLista.Add(link);
            });

            db.Dispose();

            return Json(viewModalLista, JsonRequestBehavior.AllowGet);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                var dataAtual = DateTime.Now.Date;
                var minDate = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddMonths(-1);
                var minDateVencimento = DateTime.Now.AddDays(-90);
                var maxDateVencimento = DateTime.Now.AddDays(90);
                var minDatePipeline = dataAtual.AddDays(-30) /* new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)*/;
                var maxDatePipelineTed = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                var AplicacaoResgateCount = 0;

                var statusInicialTed = _context.Status
                    .FirstOrDefault(s => s.Evento == Eventos.TEDNovo.ToString() &&
                    s.Descricao.ToUpper().StartsWith("Não Tratado"))?.Id ?? 0;


                //Obter numero de pipes, teds, vencimentos em brancos
                if (User.IsInRole(NivelAcesso.Especialista.ToString()))
                {
                    //TEDs
                    ViewBag.TedsNovas = _context.TED.Include(t => t.Status)
                        .Join(_context.Encarteiramento,
                            ted => new { agencia = ted.Agencia, conta = ted.Conta },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ted, enc) => new { ted })
                        .Where(t =>
                        t.ted.MatriculaConsultor == MatriculaUsuario &&
                        t.ted.Area.ToUpper().Contains("PGP") &&
                        //t.Status.Evento == Eventos.TED.ToString() &&
                        t.ted.StatusId == statusInicialTed &&
                        t.ted.Data >= minDate && t.ted.Data <= maxDatePipelineTed).Count();


                    //Vencimentos
                    ViewBag.VencimentosCount = _context.Vencimento.Include(i => i.Status).Join(_context.Encarteiramento,
                        ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                        enc => new { agencia = enc.Agencia, conta = enc.Conta },
                        (ven, enc) => new { ven, enc.Matricula, enc.CONSULTOR })
                        .Where(res => res.Matricula == MatriculaUsuario &&
                            res.ven.Dt_Vecto_Contratado >= minDateVencimento &&
                            res.ven.Dt_Vecto_Contratado <= maxDateVencimento &&
                            res.ven.Status.Descricao.ToLower() == "em branco").Count();

                    //Pipelines
                    ViewBag.PipelinesCount = _context.Pipeline.Where(p => p.MatriculaConsultor == MatriculaUsuario &&
                    p.Status.Descricao.ToLower() == "em branco" &&
                        (p.DataProrrogada.HasValue ? p.DataProrrogada >= minDatePipeline : p.DataPrevista >= minDatePipeline)).Count();

                    //AplicacaoResgate
                    AplicacaoResgateCount = _context.AplicacaoResgate.Where(a => a.MatriculaConsultor == MatriculaUsuario &&
                    !a.Notificado).Count();
                }
                else if (User.IsInRole(NivelAcesso.Gestor.ToString()))
                {
                    //TEDs
                    ViewBag.TedsNovas = _context.TED.Include(t => t.Status)
                    .Join(_context.Encarteiramento,
                        ted => new { agencia = ted.Agencia, conta = ted.Conta },
                        enc => new { agencia = enc.Agencia, conta = enc.Conta },
                        (ted, enc) => new { ted, enc.Matricula })
                    .Join(_context.Usuario.Include(u => u.Perfil),
                        t => t.Matricula,
                        u => u.Matricula,
                    (t, u) => new { Usuario = u, TED = t })
                    .Where(t => t.Usuario.Equipe == EquipeUsuario &&
                        t.TED.ted.Status.Evento == Eventos.TED.ToString() &&
                        t.TED.ted.Area.ToUpper().Contains("PGP") &&
                        t.TED.ted.StatusId == statusInicialTed &&
                        t.TED.ted.Data >= minDate && t.TED.ted.Data <= maxDatePipelineTed).Count();


                    //Vencimentos
                    ViewBag.VencimentosCount = _context.Vencimento.Join(_context.Encarteiramento,
                        ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                        enc => new { agencia = enc.Agencia, conta = enc.Conta },
                        (ven, enc) => new { ven, enc.Matricula })
                        .Join(_context.Usuario,
                        res => res.Matricula,
                        usu => usu.Matricula,
                        (res, usu) => new { res, usu.MatriculaSupervisor, usu.Nome }
                        )
                        .Where(result => result.MatriculaSupervisor == MatriculaUsuario &&
                        result.res.ven.Dt_Vecto_Contratado >= minDateVencimento &&
                        result.res.ven.Dt_Vecto_Contratado <= maxDateVencimento &&
                        result.res.ven.Status.Descricao.ToLower() == "em branco").Count();

                    //Pipelines
                    ViewBag.PipelinesCount = _context.Pipeline.Join(_context.Usuario,
                        pipe => pipe.MatriculaConsultor,
                        usu => usu.Matricula,
                        (pipe, usu) => new { pipe, usu }
                        ).Where(result => result.usu.MatriculaSupervisor == MatriculaUsuario &&
                        result.pipe.Status.Descricao.ToLower() == "em branco" &&
                        (result.pipe.DataProrrogada.HasValue ?
                            result.pipe.DataProrrogada >= minDatePipeline : result.pipe.DataPrevista >= minDatePipeline))
                        .ToList()
                        .Select(r => r.pipe).Count();

                }
                else
                {
                    //TEDs
                    ViewBag.TedsNovas = _context.TED.Include(t => t.Status)
                       .Join(_context.Encarteiramento,
                            ted => new { agencia = ted.Agencia, conta = ted.Conta },
                            enc => new { agencia = enc.Agencia, conta = enc.Conta },
                            (ted, enc) => new { ted, enc.Matricula })
                            .Join(_context.Usuario,
                                t => t.Matricula,
                                usu => usu.Matricula,
                                (t, usu) => new { t }
                            )
                            .Where(j => j.t.ted.Area.ToUpper().Contains("PGP") &&
                                j.t.ted.StatusId == statusInicialTed &&
                                j.t.ted.Data >= minDate && j.t.ted.Data <= maxDatePipelineTed).Count();


                    //Vencimentos
                    ViewBag.VencimentosCount = _context.Vencimento.Include(i => i.Status).Join(_context.Encarteiramento,
                    ven => new { agencia = ven.Cod_Agencia.ToString(), conta = ven.Cod_Conta_Corrente.ToString() },
                    enc => new { agencia = enc.Agencia, conta = enc.Conta },
                    (ven, enc) => new { ven, enc.CONSULTOR }).Where(v => v.ven.Status.Descricao.ToLower() == "em branco").Count();

                    //Pipelines
                    ViewBag.PipelinesCount = _context.Pipeline
                        .Where(p => p.Status.Descricao.ToLower() == "em branco" &&
                        (p.DataProrrogada.HasValue ? p.DataProrrogada >= minDatePipeline : p.DataPrevista >= minDatePipeline)).Count();
                }

                ViewBag.AplicacaoResgateCount = AplicacaoResgateCount;

                //Obtem os links para os arquivos de captacao-liquida e saldo u=investfacil
                var linksCapInv = GetLinksCapInvest();
                ViewBag.LinksCap = linksCapInv?.FirstOrDefault(l => l.Titulo == "CapLiq")?.Url;
                ViewBag.LinksInvest = linksCapInv?.FirstOrDefault(l => l.Titulo == "Invest")?.Url;

                //Links
                var linksExternos = _context.Links.Where(l => !l.Exibir);
                ViewBag.CockpitColmeia = linksExternos?.FirstOrDefault(l => l.Titulo == "Cockpit Colmeia")?.Url;
                ViewBag.CockpitPost = linksExternos?.FirstOrDefault(l => l.Titulo == "Cockpit")?.Url;
                ViewBag.SINVUrl = linksExternos?.FirstOrDefault(l => l.Titulo == "SINV")?.Url;
                ViewBag.PSDCUrl = linksExternos?.FirstOrDefault(l => l.Titulo == "PSDC")?.Url;

                //ComboBox
                ViewBag.Equipes = SelectListItemGenerator.Equipes();

                //Opcao para recerber notificacao
                ViewBag.NotificacaoEvento = _context.Usuario.FirstOrDefault(u => u.Matricula == MatriculaUsuario)?.NotificacaoEvento;
                ViewBag.NotificacaoPipeline = _context.Usuario.FirstOrDefault(u => u.Matricula == MatriculaUsuario)?.NotificacaoPipeline;

                //Portabilidade
                ViewBag.Status = SelectListItemGenerator.Status(EventosStatusMotivosOrigens.Portabilidade.ToString());

                ViewBag.Motivos = SelectListItemGenerator.Motivos(EventosStatusMotivosOrigens.Portabilidade.ToString());

                ViewBag.MotivosSemSubmotivos = JsonConvert.SerializeObject(_context.vw_MotivosSemSubmotivos.Select(s => s.Id).ToList()); //"[]";

                ViewBag.SubStatus = _context.SubStatus.ToList().ConvertAll(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Descricao });

                ((List<SelectListItem>)ViewBag.SubStatus).Insert(0, new SelectListItem { Value = string.Empty, Text = string.Empty });

                ViewBag.Especialistas = SelectListItemGenerator.Especialistas();
            }

            base.OnActionExecuting(filterContext);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new CustomJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected byte[] GerarExcel<T>(List<T> dados, string nomePlanilha)
        {
            //if (!dados.Any())
            //return new byte[] { };

            var file = new ExcelPackage();

            var wk = file.Workbook;

            var properites = typeof(T).GetProperties().Where(p => !p.Name.Contains("Id")).ToArray();

            var sheet = wk.Worksheets.Add(nomePlanilha);

            int linha = 1;

            var contatos = default(PropertyInfo[]);

            var type = typeof(T);

            int col = 1;

            var produtos = new List<string>();

            //Cabechalhos
            if (type != typeof(UsuariosExportExcel))
            {
                linha = 2;

                var colunasExcluidas = new string[] { "MatriculaSupervisor", "Area", "Notificado", "Motivos", "Situacoes" };
                var colunasTeds = new string[]
                   {
                        "Agencia","Conta","NomeCliente","MatriculaConsultor","NomeConsultor","NomeSupervisor","Data","Valor","ValorAplicado","Motivo",
                        "Status","Equipe"
                   };

                if (type == typeof(TEDViewModel))
                {
                    properites = properites.Where(v => colunasTeds.Contains(v.Name)).ToArray();
                }
                else
                {
                    properites = properites.Where(v => !colunasExcluidas.Contains(v.Name)).ToArray();
                }

                //Mesclagem primeira Linha
                sheet.Cells[1, 1].Style.Font.Bold = true;
                sheet.Cells[1, 1].Style.Font.Size = 16;
                sheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[1, 1, 1, properites.Length + 1].Merge = true;
                sheet.Cells[1, 1, 1, properites.Length].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                sheet.Cells[1, 1, 1, properites.Length].AutoFitColumns();

                if (type == typeof(CockpitExportExcel))
                {
                    properites = properites.Where(p => p.Name != "MatriculaConsultor").ToArray();

                    foreach (var p in properites)
                    {
                        sheet.Cells[linha, col].Value = p.Name;
                        sheet.Cells[linha, col].AutoFitColumns();
                        col++;
                    }
                }
                else
                {
                    properites = properites.Where(p => !p.Name.Contains("RecebeNotificacao")).ToArray();

                    for (int idxProp = 0; idxProp < properites.Length; idxProp++)
                    {
                        if (type == typeof(TEDViewModel) && col == 1)
                        {

                            sheet.Cells[linha, col].Value = "Ag-Conta";
                            idxProp -= 1;
                        }
                        else
                        {
                            sheet.Cells[linha, col].Value = properites[idxProp].Name;
                        }

                        sheet.Cells[linha, col].AutoFitColumns();
                        col++;
                    }

                    //Cabeçalho adicional para ted
                    if (type == typeof(TEDViewModel))
                    {
                        contatos = typeof(TedsContatos).GetProperties().Where(p => !p.Name.Contains("Id") && !p.Name.Contains("TED")).ToArray();

                        //Mesclagem primeira Linha Contatos
                        sheet.Cells[1, col].Style.Font.Bold = true;
                        sheet.Cells[1, col].Value = "Contatos";
                        sheet.Cells[1, col].Style.Font.Size = 16;
                        sheet.Cells[1, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheet.Cells[1, col, 1, col + contatos.Length - 1].Merge = true;
                        sheet.Cells[1, col, 1, col + contatos.Length - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[1, col, 1, col + contatos.Length - 1].AutoFitColumns();

                        foreach (var contato in contatos)
                        {
                            sheet.Cells[linha, col].Value = contato.Name;
                            sheet.Cells[linha, col].AutoFitColumns();
                            col++;
                        }

                        produtos = _context.TedsProdutos.Select(s => s.Produto).ToList();

                        //Mesclagem primeira Linha Contatos

                        sheet.Cells[1, col].Style.Font.Bold = true;
                        sheet.Cells[1, col].Value = "Aplicações";
                        sheet.Cells[1, col].Style.Font.Size = 16;
                        sheet.Cells[1, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheet.Cells[1, col, 1, col + produtos.Count - 1].Merge = true;
                        sheet.Cells[1, col, 1, col + produtos.Count - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        sheet.Cells[1, col, 1, col + produtos.Count - 1].AutoFitColumns();

                        foreach (var produto in produtos)
                        {
                            sheet.Cells[linha, col].Value = produto;
                            sheet.Cells[linha, col].AutoFitColumns();
                            col++;
                        }
                    }
                }

                sheet.Cells[linha, 1, linha, properites.Length].Style.Font.Bold = true;

                if (type == typeof(TEDViewModel))
                {
                    sheet.Cells[linha, 1, linha, properites.Length + contatos.Length + produtos.Count].AutoFilter = true;
                }
                else
                {
                    sheet.Cells[linha, 1, linha, properites.Length].AutoFilter = true;
                }

                sheet.View.FreezePanes(3, 1);
            }
            else
            {
                var colunasExcluidas = new string[] { "NotificacaoEvento", "NotificacaoPipeline", "Notificado" };

                properites = properites.Where(p => !colunasExcluidas.Contains(p.Name)).ToArray();

                foreach (var p in properites)
                {
                    sheet.Cells[linha, col].Value = p.Name;
                    sheet.Cells[linha, col].AutoFitColumns();
                    col++;
                }
                sheet.Cells[linha, 1, linha, properites.Length].Style.Font.Bold = true;
                sheet.Cells[linha, 1, linha, properites.Length].AutoFilter = true;

                sheet.View.FreezePanes(2, 1);


            }

            //Dados
            foreach (var dado in dados)
            {
                linha++;

                if (type == typeof(PipelineViewModel))
                {
                    sheet.Cells[1, 1].Value = "Pipelines";

                    var p = dado as PipelineViewModel;

                    sheet.Cells[linha, 1].Value = p.Cliente;
                    sheet.Cells[linha, 2].Value = p.Especialista;
                    sheet.Cells[linha, 3].Value = p.Agencia;
                    sheet.Cells[linha, 4].Value = p.Conta;
                    sheet.Cells[linha, 5].Value = p.BradescoPrincipalBanco ? "Sim" : "Não";
                    sheet.Cells[linha, 6].Value = p.ValorMercado;
                    sheet.Cells[linha, 7].Value = p.DataProrrogada?.ToShortDateString();
                    sheet.Cells[linha, 8].Value = p.ValorDoPipe;
                    sheet.Cells[linha, 9].Value = p.ValorAplicado;
                    sheet.Cells[linha, 10].Value = p.DataPrevista.ToShortDateString();
                    sheet.Cells[linha, 11].Value = p.Comentario;
                    sheet.Cells[linha, 12].Value = p.Motivo;
                    sheet.Cells[linha, 13].Value = p.Origem;
                    sheet.Cells[linha, 14].Value = p.Situacao;
                    sheet.Cells[linha, 15].Value = int.Parse(p.Matricula);
                    sheet.Cells[linha, 16].Value = p.Equipe;



                    sheet.Cells[linha, 1, linha, properites.Length].AutoFitColumns();
                }
                else if (type == typeof(VencimentoViewModel))
                {
                    sheet.Cells[1, 1].Value = "Vencimentos";

                    var v = dado as VencimentoViewModel;

                    sheet.Cells[linha, 1].Value = v.Especialista;
                    sheet.Cells[linha, 2].Value = v.Produto;
                    sheet.Cells[linha, 3].Value = v.SaldoAtual;
                    sheet.Cells[linha, 4].Value = v.Agencia;
                    sheet.Cells[linha, 5].Value = v.Conta;
                    sheet.Cells[linha, 6].Value = v.DataVencimento.ToShortDateString();
                    sheet.Cells[linha, 7].Value = v.PercentualIndexador;
                    sheet.Cells[linha, 8].Value = v.Cliente;
                    sheet.Cells[linha, 9].Value = v.Status;
                    sheet.Cells[linha, 10].Value = int.Parse(v.Matriucla);
                    sheet.Cells[linha, 11].Value = v.Equipe;

                    sheet.Cells[linha, 1, linha, properites.Length].AutoFitColumns();
                }
                else if (type == typeof(CarteiraClienteExportExcel))
                {
                    sheet.Cells[1, 1].Value = "Clusterização";

                    var v = dado as CarteiraClienteExportExcel;

                    sheet.Cells[linha, 1].Value = v.Especialista;
                    sheet.Cells[linha, 3].Value = v.Agencia;
                    sheet.Cells[linha, 2].Value = v.Conta;
                    sheet.Cells[linha, 4].Value = v.CPF;
                    sheet.Cells[linha, 5].Value = v.PerfilApi;
                    sheet.Cells[linha, 6].Value = v.MES_VCTO_API;
                    sheet.Cells[linha, 7].Value = v.NIVEL_DESENQ_FX_RISCO;
                    sheet.Cells[linha, 8].Value = v.NomeCliente;
                    sheet.Cells[linha, 9].Value = v.NomeGerente;
                    sheet.Cells[linha, 10].Value = v.UltimoContato?.ToShortDateString();
                    sheet.Cells[linha, 11].Value = v.UltimaTentativa.ToShortDateString();
                    sheet.Cells[linha, 12].Value = v.DiasCorridosÚltimoContato;
                    sheet.Cells[linha, 13].Value = v.Situacao;
                    sheet.Cells[linha, 14].Value = int.Parse(v.Matricula);
                    sheet.Cells[linha, 15].Value = v.Equipe;
                    sheet.Cells[linha, 16].Value = v.SALDO_TOTAL_M3;
                    sheet.Cells[linha, 17].Value = v.SALDO_TOTAL;
                    sheet.Cells[linha, 18].Value = v.SALDO_CORRETORA_BRA;
                    sheet.Cells[linha, 19].Value = v.SALDO_CORRETORA_AGORA;
                    sheet.Cells[linha, 20].Value = v.SALDO_CORRETORA;
                    sheet.Cells[linha, 21].Value = v.SALDO_PREVIDENCIA;
                    sheet.Cells[linha, 22].Value = v.SALDO_POUPANCA;
                    sheet.Cells[linha, 23].Value = v.SALDO_INVESTS;
                    sheet.Cells[linha, 24].Value = v.SALDO_DAV_20K;
                    sheet.Cells[linha, 25].Value = v.SALDO_COMPROMISSADAS;
                    sheet.Cells[linha, 26].Value = v.SALDO_ISENTOS;
                    sheet.Cells[linha, 27].Value = v.SALDO_LF;
                    sheet.Cells[linha, 28].Value = v.SALDO_CDB;
                    sheet.Cells[linha, 29].Value = v.SALDO_FUNDOS;

                }
                else if (type == typeof(TEDViewModel))
                {
                    sheet.Cells[1, 1].Value = "TEDs";
                    var v = dado as TEDViewModel;
                    sheet.Cells[linha, 1].Value = $"{int.Parse(v.Agencia)}-{int.Parse(v.Conta)}";
                    sheet.Cells[linha, 2].Value = int.Parse(v.Agencia);
                    sheet.Cells[linha, 3].Value = int.Parse(v.Conta);
                    sheet.Cells[linha, 4].Value = v.NomeCliente;
                    sheet.Cells[linha, 5].Value = int.Parse(v.MatriculaConsultor);
                    sheet.Cells[linha, 6].Value = v.NomeConsultor;
                    sheet.Cells[linha, 7].Value = v.NomeSupervisor;
                    sheet.Cells[linha, 8].Value = v.Data.ToShortDateString();
                    sheet.Cells[linha, 9].Value = v.Valor;
                    sheet.Cells[linha, 10].Value = v.ValorAplicado;
                    sheet.Cells[linha, 11].Value = v.Motivo;
                    sheet.Cells[linha, 12].Value = v.Status;
                    sheet.Cells[linha, 13].Value = v.Equipe;

                    //contatos
                    sheet.Cells[linha, 14].Value = v.ContatouCliente.HasValue && v.ContatouCliente.Value ? "Sim" : "Não";
                    sheet.Cells[linha, 15].Value = v.ContatouGerente.HasValue && v.ContatouGerente.Value ? "Sim" : "Não";
                    sheet.Cells[linha, 16].Value = v.GerenteSolicitouNaoAtuacao.HasValue && v.GerenteSolicitouNaoAtuacao.Value ? "Sim" : "Não";
                    sheet.Cells[linha, 17].Value = v.GerenteInvestimentoAtuou.HasValue && v.GerenteInvestimentoAtuou.Value ? "Sim" : "Não";
                    sheet.Cells[linha, 18].Value = v.EspecialistaAtuou.HasValue && v.EspecialistaAtuou.Value ? "Sim" : "Não";
                    sheet.Cells[linha, 19].Value = v.ClienteLocalizado.HasValue && v.ClienteLocalizado.Value ? "Sim" : "Não";
                    sheet.Cells[linha, 20].Value = v.ClienteAceitaConsultoria.HasValue && v.ClienteAceitaConsultoria.Value ? "Sim" : "Não";

                    //Aplicacoes
                    var coluna = 21;
                    foreach (var prod in produtos)
                    {
                        sheet.Cells[linha, coluna].Value = v.Aplicacoes.Where(a => a.Produto == prod).Sum(s => s.Valor);
                        coluna++;
                    }
                }
                else if (type == typeof(CockpitExportExcel))
                {
                    sheet.Cells[1, 1].Value = "Cockpit";

                    var cockpit = dado as CockpitExportExcel;

                    sheet.Cells[linha, 1].Value = cockpit.Equipe;
                    sheet.Cells[linha, 2].Value = cockpit.Especialista;
                    sheet.Cells[linha, 3].Value = cockpit.CodFuncionalGerente;
                    sheet.Cells[linha, 4].Value = cockpit.NomeGerente;
                    sheet.Cells[linha, 5].Value = cockpit.CPF;
                    sheet.Cells[linha, 6].Value = cockpit.NomeCliente;
                    sheet.Cells[linha, 7].Value = cockpit.CodigoAgencia;
                    sheet.Cells[linha, 8].Value = cockpit.NomeAgencia;
                    sheet.Cells[linha, 9].Value = cockpit.Conta;
                    sheet.Cells[linha, 10].Value = cockpit.DataEncarteiramento.HasValue ? cockpit.DataEncarteiramento.Value.ToShortDateString() : null;
                    sheet.Cells[linha, 11].Value = cockpit.DataContato.ToShortDateString();
                    sheet.Cells[linha, 12].Value = cockpit.DataRetorno.HasValue ? cockpit.DataRetorno.Value.ToShortDateString() : null;
                    sheet.Cells[linha, 13].Value = cockpit.Observacao;
                    sheet.Cells[linha, 14].Value = cockpit.ContatoTeveExito;
                    sheet.Cells[linha, 15].Value = cockpit.DataHoraEdicaoContato.HasValue ? cockpit.DataHoraEdicaoContato.Value.ToShortDateString() : null;
                    sheet.Cells[linha, 16].Value = cockpit.MeioContato;
                    sheet.Cells[linha, 17].Value = cockpit.ClienteNaoLocalizado;
                    sheet.Cells[linha, 18].Value = cockpit.TipoTransacao;
                    sheet.Cells[linha, 19].Value = cockpit.Finalizado;
                    sheet.Cells[linha, 20].Value = cockpit.GerenteRegistrouContato;
                    //sheet.Cells[linha, 21].Value = cockpit.MatriculaConsultor;

                }
                else if (type == typeof(InvestFacilExcel))
                {
                    sheet.Cells[1, 1].Value = "Saldo Investfacil";

                    var investfacil = dado as InvestFacilExcel;

                    sheet.Cells[linha, 1].Value = investfacil.SEGMENTO_CLIENTE;
                    sheet.Cells[linha, 2].Value = int.Parse(investfacil.AGENCIA);
                    sheet.Cells[linha, 3].Value = int.Parse(investfacil.CONTA);
                    sheet.Cells[linha, 4].Value = long.Parse(investfacil.NUM_CONTRATO);
                    sheet.Cells[linha, 4].Style.Numberformat.Format = "0";
                    sheet.Cells[linha, 5].Value = investfacil.MES_DT_BASE;
                    sheet.Cells[linha, 6].Value = investfacil.DT_EMISSAO.Value.ToShortDateString();
                    sheet.Cells[linha, 7].Value = investfacil.PRAZO_PERMAN;
                    sheet.Cells[linha, 8].Value = investfacil.FX_PERMANENCIA;
                    sheet.Cells[linha, 9].Value = investfacil.FX_VOLUME;
                    sheet.Cells[linha, 10].Value = investfacil.Vlr_Evento;
                    sheet.Cells[linha, 10].Style.Numberformat.Format = "R$ #,##0.00";
                    sheet.Cells[linha, 11].Value = investfacil.SEGM_AGRUPADO;
                    sheet.Cells[linha, 12].Value = investfacil.SEGMENTO_MACRO;
                    sheet.Cells[linha, 13].Value = investfacil.Especialista;
                    sheet.Cells[linha, 14].Value = investfacil.Equipe;
                }
                else if (type == typeof(UsuariosExportExcel))
                {
                    //Seta cabeçãlho exclusivo

                    var usuario = dado as UsuariosExportExcel;

                    sheet.Cells[linha, 1].Value = usuario.Usuario;
                    sheet.Cells[linha, 2].Value = int.Parse(usuario.Matricula);
                    sheet.Cells[linha, 3].Value = usuario.Supervisor;
                    sheet.Cells[linha, 4].Value = int.Parse(usuario.MatriculaGestor);
                    sheet.Cells[linha, 5].Value = usuario.Equipe;
                    sheet.Cells[linha, 6].Value = usuario.TipoDeAcesso;
                    sheet.Cells[linha, 7].Value = usuario.Username;
                }
                else if (type == typeof(AplicacaoResgateViewModel))
                {
                    sheet.Cells[1, 1].Value = "Aplicação e Resgate";
                    var aplicacaoes = dado as AplicacaoResgateViewModel;

                    sheet.Cells[linha, 1].Value = aplicacaoes.agencia;
                    sheet.Cells[linha, 2].Value = aplicacaoes.conta;
                    sheet.Cells[linha, 3].Value = aplicacaoes.data.ToShortDateString();
                    var hora = aplicacaoes.hora.Hours < 10 ? $"0{aplicacaoes.hora.Hours}" : $"{aplicacaoes.hora.Hours}";
                    var minutos = aplicacaoes.hora.Minutes < 10 ? $"0{aplicacaoes.hora.Minutes}" : $"{aplicacaoes.hora.Minutes}";
                    sheet.Cells[linha, 4].Value = $"{hora }:{minutos}";
                    sheet.Cells[linha, 5].Value = aplicacaoes.operacao;
                    sheet.Cells[linha, 6].Value = aplicacaoes.perif;
                    sheet.Cells[linha, 7].Value = aplicacaoes.produto;
                    sheet.Cells[linha, 8].Value = aplicacaoes.terminal;
                    sheet.Cells[linha, 9].Value = aplicacaoes.valor;
                    sheet.Cells[linha, 10].Value = aplicacaoes.gerente;
                    sheet.Cells[linha, 11].Value = aplicacaoes.advisor;
                    sheet.Cells[linha, 12].Value = aplicacaoes.segmento;
                    sheet.Cells[linha, 13].Value = aplicacaoes.enviado.HasValue && aplicacaoes.enviado.Value ? "Sim" : "Não";
                    sheet.Cells[linha, 14].Value = aplicacaoes.Especialista;
                    sheet.Cells[linha, 15].Value = int.Parse(aplicacaoes.Matricula);
                }

            }

            //Formatações finais
            for (int coluna = 1; coluna < sheet.Dimension.Columns; coluna++)
            {
                sheet.Column(coluna).AutoFit();
            }

            sheet.Cells[2, 1, sheet.Dimension.Rows, sheet.Dimension.Columns].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            return file.GetAsByteArray();
        }

        protected List<Links> GetLinksCapInvest()
        {
            return _context.Links.Where(l => !l.Exibir).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();

            base.Dispose(disposing);
        }
    }
}