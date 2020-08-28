using BradescoPGP.Common.Logging;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace BradescoPGP.Web.Models
{
    public class ClienteViewModel
    {
        public ClienteViewModel()
        {
            TEDs = new List<TEDViewModel>();
        }

        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public int Agencia { get; set; }
        public CorretoraViewModel Corretora { get; set; }
        public int Conta { get; set; }
        public string CPFCNPJ { get; set; }
        public DateTime? Aniversario { get; set; }
        public PSDCModeloRetorno PSDC { get; set; }
        public ApicModeloRetorno APIC { get; set; }
        public Consultor Consultor { get; set; }
        public AgendaViewModel Agenda { get; set; }
        public string TemInvestfacil { get; set; }
        public virtual List<TEDViewModel> TEDs { get; set; }
        public virtual List<VencimentoViewModel> Vencimentos { get; set; }
        public virtual List<PipelineViewModel> Pipelines { get; set; }


        public static ClienteViewModel Mapear(Encarteiramento encarteiramento)
        {
            var codigoUsuario = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("nomeUsuario").Value;

            var senhaApicPSDC = !string.IsNullOrEmpty(HttpContext.Current.Session["password"].ToString()) ? HttpContext.Current.Session["password"].ToString() : null;

            var cockpit = default(Cockpit);

            var apic = default(ApicModeloRetorno);

            var psdc = default(PSDCModeloRetorno);

            using (var db = new PGPEntities())
            {
                cockpit = db.Cockpit.FirstOrDefault(x => x.CodigoAgencia.ToString() == encarteiramento.Agencia && x.Conta.ToString() == encarteiramento.Conta);
            }

            try
            {
                apic = ObterPSDCAPIC.CarregarAPIC(cockpit.CPF, codigoUsuario, senhaApicPSDC);

                psdc = ObterPSDCAPIC.CarregarPSDC_OPERACAO_CREDITO(cockpit.CPF, codigoUsuario, senhaApicPSDC);
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao carregar informações do apic psdc", ex);
            }


            return new ClienteViewModel
            {
                //Corretora
                Agencia = int.Parse(encarteiramento.Agencia),
                Consultor = new Consultor { Nome = encarteiramento.CONSULTOR },
                Conta = int.Parse(encarteiramento.Conta),
                NomeCliente = String.Empty, //TODO: Preencher com Nome do Cliente do Encarteiramento
                CPFCNPJ = encarteiramento.CPF,
                PSDC = psdc,
                APIC = apic
            };
        }

        public static ClienteViewModel Mapear(Cockpit cockpit)
        {
            return new ClienteViewModel
            {
                Agencia = cockpit.CodigoAgencia,
                Conta = cockpit.Conta,
                NomeCliente = cockpit.NomeCliente,
                CPFCNPJ = cockpit.CPF
            };
        }

        public static ClienteViewModel Mapear(
            Cockpit cockpit, 
            Usuario consultor, 
            Clusterizacoes clusterizacao, 
            List<Vencimento> vencimentos, 
            List<Pipeline> pipelines, 
            List<TED> teds, 
            AgendaViewModel agenda, 
            DateTime? Aniversario, 
            Corretora Bra, 
            Corretora Ago, 
            Encarteiramento encarteiramento,
            bool temInvestFacil)
        {
            var codigoUsuario = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("nomeUsuario").Value;

            var senhaApicPSDC = HttpContext.Current?.Session["password"]?.ToString();

            var apic = default(ApicModeloRetorno);

            var psdc = default(PSDCModeloRetorno);

            cockpit = cockpit ?? new Cockpit();

            consultor = consultor ?? new Usuario();

            clusterizacao = clusterizacao ?? new Clusterizacoes();

            vencimentos = vencimentos ?? new List<Vencimento>();

            pipelines = pipelines ?? new List<Pipeline>();
            

            teds = teds ?? new List<TED>();

            try
            {
                psdc = ObterPSDCAPIC.CarregarPSDC_OPERACAO_CREDITO(cockpit.CPF, codigoUsuario, senhaApicPSDC);

                apic = ObterPSDCAPIC.CarregarAPIC(cockpit.CPF, codigoUsuario, senhaApicPSDC);
            }
            catch (Exception ex)
            {
                Log.Information("Erro ao carregar informações do apic psdc", ex);
            }
            
            return new ClienteViewModel
            {
                Agencia = int.TryParse(encarteiramento.Agencia, out int agencia) ? agencia : 0,
                Conta = int.TryParse(encarteiramento.Conta, out int conta) ? conta : 0,
                Consultor = new Consultor { Nome = consultor.Nome, Matricula = consultor.Matricula },
                NomeCliente = cockpit.NomeCliente,
                CPFCNPJ = cockpit.CPF ?? encarteiramento.CPF,
                PSDC = psdc,
                APIC = apic,
                Pipelines = pipelines.ConvertAll(p => PipelineViewModel.Mapear(p)),
                Vencimentos = vencimentos.ConvertAll(v => VencimentoViewModel.Mapear(v, consultor.Nome)),
                TEDs = teds.ConvertAll(t => TEDViewModel.Mapear(t)),
                Agenda = agenda,
                Corretora = CorretoraViewModel.Mapear(Bra, Ago),
                Aniversario = Aniversario ?? null,
                TemInvestfacil = temInvestFacil ? "SIM" : "NÃO"
            };
        }
    }

    public class PSDCModeloRetorno
    {
        public DateTime PSDC_DataAtualizacao { get; set; }
        public string PSDC_SituacaoCadastral { get; set; }
    }

    public class ApicModeloRetorno
    {
        public string APIC_Perfil { get; set; }
        public DateTime APIC_DataPerfil { get; set; }
    }

    public class OptionsParaTela
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public static OptionsParaTela Mapear(Motivo motivos)
        {
            return new OptionsParaTela
            {
                Id = motivos.Id,
                Descricao = motivos.Descricao
            };
        }

        public static OptionsParaTela Mapear(Status status)
        {
            return new OptionsParaTela
            {
                Id = status.Id,
                Descricao = status.Descricao
            };
        }

        public static OptionsParaTela Mapear(TedsMotivos motivosTeds)
        {
            return new OptionsParaTela
            {
                Id = motivosTeds.Id,
                Descricao = motivosTeds.Motivo
            };
        }

        public static OptionsParaTela Mapear(TedsMotivoOutrasInst outrasInst)
        {
            return new OptionsParaTela
            {
                Id = outrasInst.Id,
                Descricao = outrasInst.Motivo
            };
        }

        internal static OptionsParaTela Mapear(TedsProdutos produtos)
        {
            return new OptionsParaTela
            {
                Id = produtos.Id,
                Descricao = produtos.Produto
            };
        }
    }

    public class TEDViewModel
    {
        public int Id { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCnpj { get; set; }
        public string MatriculaConsultor { get; set; }
        public string NomeConsultor { get; set; }
        public string MatriculaSupervisor { get; set; }
        public string NomeSupervisor { get; set; }
        public string Area { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public decimal? ValorAplicado { get; set; }
        public int? MotivoId { get; set; }
        public string Motivo { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public bool Notificado { get; set; }
        public List<OptionsParaTela> Motivos { get; set; } //Motivos antigos que tem que continuar na aplicação
        public List<OptionsParaTela> Situacoes { get; set; }
        public List<OptionsParaTela> TedsMotivos { get; set; } //Nova implementação de motivos
        public List<OptionsParaTela> TedsMotivosOutrasInst { get; set; } //Nova implementação de motivos
        public List<OptionsParaTela> Produtos { get; set; } //Nova implementação de motivos
        public string Equipe { get; set; }
        public int? OutrasInstId { get; set; }
        public bool? ContatouCliente { get; set; }
        public bool? ContatouGerente { get; set; }
        public bool? GerenteSolicitouNaoAtuacao { get; set; }
        public bool? GerenteInvestimentoAtuou { get; set; }
        public bool? EspecialistaAtuou { get; set; }
        public bool? ClienteLocalizado { get; set; }
        public bool? ClienteAceitaConsultoria { get; set; }
        public List<TedsAplicacoesViewModel> Aplicacoes { get; set; }
        public string Observacao { get; set; }
        public int? MotivoTedId { get; set; }
        public string MotivoTed { get; private set; }

        public static TEDViewModel Mapear(TED entity, string equipe = null)
        {
            return new TEDViewModel
            {
                Id = entity.Id,
                Motivo = entity.Motivo?.Descricao,
                MotivoTed = entity.TedsMotivos?.Motivo,
                Agencia = entity.Agencia,
                Area = entity.Area,
                Conta = entity.Conta,
                CpfCnpj = entity.CpfCnpj,
                Data = entity.Data,
                MatriculaConsultor = entity.MatriculaConsultor,
                MatriculaSupervisor = entity.MatriculaSupervisor,
                MotivoId = entity.MotivoTedId,
                OutrasInstId = entity.OutrasInstId,
                NomeCliente = entity.NomeCliente,
                NomeSupervisor = entity.NomeSupervisor,
                Status = entity.Status.Descricao,
                StatusId = entity.Status.Id,
                Valor = entity.Valor,
                ValorAplicado = entity.TedsAplicacoes.Sum(s => s.ValorAplicado),
                NomeConsultor = entity.NomeConsultor,
                Notificado = entity.Notificado,
                Equipe = equipe ?? string.Empty,
                ContatouCliente = entity.TedsContatos?.ContatouCliente ?? null,
                ContatouGerente = entity.TedsContatos?.ContatouGerente ?? null,
                GerenteSolicitouNaoAtuacao = entity.TedsContatos?.GerenteSolicitouNaoAtuacao ?? null,
                GerenteInvestimentoAtuou = entity.TedsContatos?.GerenteInvestimentoAtuou ?? null,
                EspecialistaAtuou = entity.TedsContatos?.EspecialistaAtuou ?? null,
                ClienteLocalizado = entity.TedsContatos?.ClienteLocalizado ?? null,
                ClienteAceitaConsultoria = entity.TedsContatos?.ClienteAceitaConsultoria ?? null,
                Observacao = entity.Observacao,
                Aplicacoes = entity.TedsAplicacoes.ToList().ConvertAll(a => TedsAplicacoesViewModel.Mapear(a))
            };
        }
    }

    public class TedsAplicacoesViewModel
    {
        public int IdTed { get; set; }
        public int ProdutoId { get; set; }
        public string Produto { get; set; }
        public decimal Valor { get; set; }
        public int IdAplicacao { get; set; }

        public static TedsAplicacoesViewModel Mapear(TedsAplicacoes ted)
        {
            return new TedsAplicacoesViewModel
            {
                Valor = ted.ValorAplicado,
                Produto = ted.TedsProdutos.Produto,
                IdTed = ted.TED.Id,
                ProdutoId = ted.TedsProdutos.Id,
                IdAplicacao = ted.Id
            };
        }
    }

    public class PipelineViewModel
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        [StringLength(50)]
        public string Especialista { get; set; }
        public int Agencia { get; set; }
        public int Conta { get; set; }
        public bool BradescoPrincipalBanco { get; set; }
        public decimal? ValorMercado { get; set; }
        public DateTime? DataProrrogada { get; set; }
        public decimal ValorDoPipe { get; set; }
        public decimal? ValorAplicado { get; set; }
        public DateTime DataPrevista { get; set; }
        public string Comentario { get; set; }
        public int OrigemId { get; set; }
        public int StatusId { get; set; }
        public int MotivoId { get; set; }
        public string Motivo { get; set; }
        public string Origem { get; set; }
        public string Situacao { get; set; }
        public string Matricula { get; set; }
        public string Equipe { get; set; }

        public static PipelineViewModel Mapear(Pipeline pipeline)
        {
            var pipeViewModel = new PipelineViewModel();

            pipeViewModel.Id = pipeline.Id;
            pipeViewModel.Origem = pipeline.Origem != null ? pipeline.Origem.Descricao : null;
            pipeViewModel.ValorDoPipe = pipeline.ValorDoPipe;
            pipeViewModel.ValorAplicado = pipeline.ValorAplicado ?? null;
            pipeViewModel.DataPrevista = pipeline.DataPrevista;
            pipeViewModel.Comentario = pipeline.Observacoes ?? null;
            pipeViewModel.Cliente = pipeline.NomeCliente;
            pipeViewModel.Especialista = pipeline.Consultor;
            pipeViewModel.BradescoPrincipalBanco = pipeline.BradescoPrincipalBanco;
            pipeViewModel.ValorMercado = pipeline.ValoresNoMercado ?? null;
            pipeViewModel.Agencia = pipeline.Agencia;
            pipeViewModel.Conta = pipeline.Conta;
            pipeViewModel.Motivo = pipeline.Motivo?.Descricao ?? null;
            pipeViewModel.DataProrrogada = pipeline.DataProrrogada ?? null;
            //pipeViewModel.DataDaConversao = pipeline.DataDaConversao ?? null;
            pipeViewModel.Situacao = pipeline.Status?.Descricao ?? null;
            pipeViewModel.Matricula = pipeline.MatriculaConsultor;


            return pipeViewModel;
        }

    }

    public class VencimentoViewModel
    {
        public int Id { get; set; }
        public string Especialista { get; set; }
        public string Produto { get; set; }
        public decimal SaldoAtual { get; set; }
        public int Agencia { get; set; }
        public int Conta { get; set; }
        public DateTime DataVencimento { get; set; }
        public double PercentualIndexador { get; set; }
        public string Cliente { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string Matriucla { get; set; }
        public string Equipe { get; set; }

        public static VencimentoViewModel MapearExcel(Vencimento vencimento, string consultor, string matricula, string equipe)
        {
            var venc = new VencimentoViewModel();

            venc.Id = vencimento.Id;
            venc.Especialista = consultor;
            venc.Produto = vencimento.Nome_produto_sistema_origem;
            venc.SaldoAtual = vencimento.SALDO_ATUAL;
            venc.Agencia = vencimento.Cod_Agencia;
            venc.Conta = vencimento.Cod_Conta_Corrente;
            venc.DataVencimento = vencimento.Dt_Vecto_Contratado;
            venc.PercentualIndexador = vencimento.Perc_Indexador;
            venc.Status = vencimento.Status?.Descricao;
            venc.StatusId = vencimento.Status?.Id;
            venc.Cliente = vencimento.Nm_Cliente_Contraparte;
            venc.Matriucla = matricula;
            venc.Equipe = equipe;


            return venc;
        }

        public static VencimentoViewModel Mapear(Vencimento vencimento, string consultor, string matricula = null)
        {
            var venc = new VencimentoViewModel();

            venc.Id = vencimento.Id;
            venc.Especialista = consultor;
            venc.Produto = vencimento.Nome_produto_sistema_origem;
            venc.SaldoAtual = vencimento.SALDO_ATUAL;
            venc.Agencia = vencimento.Cod_Agencia;
            venc.Conta = vencimento.Cod_Conta_Corrente;
            venc.DataVencimento = vencimento.Dt_Vecto_Contratado;
            venc.PercentualIndexador = vencimento.Perc_Indexador;
            venc.Status = vencimento.Status?.Descricao;
            venc.StatusId = vencimento.Status?.Id;
            venc.Cliente = vencimento.Nm_Cliente_Contraparte;
            venc.Matriucla = matricula ?? string.Empty;

            return venc;
        }
    }
}