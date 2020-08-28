using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BradescoPGP.Web.Services
{
    public class BuscaService
    {
        public ClienteViewModel Buscar(int? agencia, int? conta, string cpfCnpj, string matricula, string perfil)
        {
            var encarteiramentoDb = default(Encarteiramento);
            var cockpit = default(Cockpit);
            var vencimentos = default(List<Vencimento>);
            var pipelines = default(List<Pipeline>);
            var teds = default(List<TED>);
            var corretoraBra = default(Corretora);
            var corretoraAgo = default(Corretora);
            var dataAtual = DateTime.Now.Date;
            var minDateTed = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddMonths(-1) ;
            var maxDataTed = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year,dataAtual.Month));

            using (var context = new PGPEntities())
            {
                if (agencia.HasValue && conta.HasValue)
                {
                    encarteiramentoDb = context.Encarteiramento.FirstOrDefault(e => e.Agencia == agencia.ToString() && e.Conta == conta.ToString());

                    cockpit = context.Cockpit.FirstOrDefault(c => c.CodigoAgencia == agencia && c.Conta == conta);

                    vencimentos = context.Vencimento.Include(i => i.Status).Where(v => v.Cod_Agencia == agencia && v.Cod_Conta_Corrente == conta).ToList();

                    pipelines = context.Pipeline.Include(i => i.Status).Include(i => i.Origem).Include(i => i.Motivo).Where(p => p.Agencia == agencia && p.Conta == conta).ToList();

                    teds = context.TED
                    .Include(t => t.Motivo)
                    .Include(t => t.Status)
                    .OrderByDescending(t => t.Data)
                    .Where(t => 
                        t.Agencia == agencia.ToString() && 
                        t.Conta == conta.ToString() && 
                        !t.Status.Descricao.ToLower().Contains("aplicado") &&
                        !t.Status.Descricao.ToLower().Contains("Finalizado") &&
                        t.Data >= minDateTed && t.Data <= maxDataTed)
                        .ToList();
                }
                else if (!String.IsNullOrWhiteSpace(cpfCnpj))
                {
                    cockpit = context.Cockpit.FirstOrDefault(c => c.CPF == cpfCnpj);

                    if (cockpit != null)
                    {
                        var cpf = cockpit.CPF;

                        var posInicial = ObterCpfPesquisa(cpf);
                        var cpfPesquisaEnc = cpfCnpj.Substring(posInicial, cpf.Length-posInicial-2);
                        encarteiramentoDb = context.Encarteiramento.FirstOrDefault(e => e.CPF == cpfPesquisaEnc);

                        vencimentos = context.Vencimento.Include(i => i.Status).Where(v => v.Cod_Agencia == cockpit.CodigoAgencia && v.Cod_Conta_Corrente == cockpit.Conta).ToList();

                        pipelines = context.Pipeline
                            .Include(i => i.Status).Include(i => i.Origem).Include(i => i.Motivo)
                            .Where(p => p.Conta == cockpit.Conta && p.Agencia == cockpit.CodigoAgencia).ToList();
                    }
                }

                if (encarteiramentoDb != null)
                {
                    corretoraBra = context.Corretora.FirstOrDefault(c => c.Agencia == encarteiramentoDb.Agencia && c.Conta == encarteiramentoDb.Conta);

                    corretoraAgo = context.Corretora.FirstOrDefault(c => c.CPF == encarteiramentoDb.CPF);

                    var matriculaConsultor = encarteiramentoDb?.Matricula;

                    var aniversario = context.Aniversarios.FirstOrDefault(a => a.Agencia.ToString() == encarteiramentoDb.Agencia && a.Conta.ToString() == encarteiramentoDb.Conta)?.DataNascimento;

                    var consultor = context.Usuario.FirstOrDefault(c => c.Matricula == encarteiramentoDb.Matricula);

                    if (teds == null)
                    {
                            teds = context.TED
                           .Include(t => t.Motivo)
                           .Include(t => t.Status)
                           .OrderByDescending(t => t.Data)
                           .Where(t => 
                                t.Agencia == encarteiramentoDb.Agencia.ToString() && 
                                t.Conta == encarteiramentoDb.Conta.ToString() && 
                                !t.Status.Descricao.ToLower().Contains("aplicado") &&
                                !t.Status.Descricao.ToLower().Contains("Finalizado") &&
                                t.Data >= minDateTed && t.Data <= maxDataTed)
                           .ToList();
                        

                    }

                    var agendaService = new AgendaService();

                    var agenda = agendaService.ObterAgendaCompleta(matricula, perfil);

                    var Invest = context.TemInvestFacil.FirstOrDefault(c => c.Agencia.ToString() == encarteiramentoDb.Agencia && c.Conta.ToString() == encarteiramentoDb.Conta);

                    var temInvest = Invest != null;

                    var cluster = context.Clusterizacoes
                        .FirstOrDefault(c => c.AGENCIA.ToString() == encarteiramentoDb.Agencia.ToString() && 
                            c.CONTA.ToString() == encarteiramentoDb.Conta.ToString());

                    var viewModel = ClienteViewModel.Mapear(cockpit, consultor, cluster, 
                        vencimentos, pipelines, teds, agenda, aniversario, corretoraBra, corretoraAgo, encarteiramentoDb, temInvest);

                    return viewModel;
                }

                return new ClienteViewModel();
                
                //Função local para recuperar a posição inicial para busca no encarteiramento.
                int ObterCpfPesquisa (string cpf)
                {
                    int indice = 0;
                    for (int i = 0; i < cpf.Length; i++)
                    {
                        if(cpf[i] != cpf[i + 1])
                        {
                            indice = cpf[i] != '0' ? i: i + 1;
                            break;
                        }
                    }
                    return indice;
                }
            }
        }

        public List<ClienteViewModel> Buscar(string nome, string especialista)
        {
            using (PGPEntities _context = new PGPEntities())
            {
                var listaClienteViewModel = new List<ClienteViewModel>();
                var encarteiramentoDb = default(Encarteiramento);
                var cockpit = default(Cockpit);
                var vencimentos = default(List<Vencimento>);
                var pipelines = default(List<Pipeline>);
                var teds = default(List<TED>);

                var isolado = nome.PadLeft(nome.Length + 1).PadRight(nome.Length + 2);

                var noInicio = nome.PadRight(nome.Length + 1);

                var noFinal = nome.PadLeft(nome.Length + 1);

                var resultadoBusca = default(List<Cockpit>);

                if (!string.IsNullOrEmpty(especialista))
                {
                    resultadoBusca = _context.Cockpit
                        .Join(_context.Usuario, cock => cock.MatriculaConsultor, usu => usu.Matricula, (cok, usu) => new { cok, usu.Nome })
                        .Where(c =>
                            c.cok.NomeCliente.StartsWith(noInicio) ||
                            c.cok.NomeCliente.EndsWith(noFinal) ||
                            c.cok.NomeCliente.Contains(isolado) &&
                            c.Nome.ToUpper() == especialista.ToUpper()
                        ).Select(s => s.cok).ToList();

                }
                else
                {
                    resultadoBusca = _context.Cockpit
                        .Where(c => c.NomeCliente.StartsWith(noInicio) || c.NomeCliente.EndsWith(noFinal) || c.NomeCliente.Contains(isolado)).ToList();
                }

                if (resultadoBusca.Any())
                {
                    var clientesEncontrados = resultadoBusca.GroupBy(c => $"{c.CodigoAgencia}-{c.Conta}").Select(c => c.First()).ToList();

                    if (clientesEncontrados.Count > 1)
                    {
                        return clientesEncontrados.ConvertAll(c => ClienteViewModel.Mapear(c));
                    }
                    else
                    {
                        cockpit = clientesEncontrados.First();

                        encarteiramentoDb = _context.Encarteiramento.FirstOrDefault(e => e.Agencia == cockpit.CodigoAgencia.ToString() && e.Conta == cockpit.Conta.ToString());

                        vencimentos = _context.Vencimento.Where(v => v.Cod_Agencia == cockpit.CodigoAgencia && v.Cod_Conta_Corrente == cockpit.Conta).ToList();

                        pipelines = _context.Pipeline.Where(p => p.Conta == cockpit.CodigoAgencia && p.Conta == cockpit.CodigoAgencia).ToList();

                    }
                }

                if (encarteiramentoDb != null)
                {
                    var matriculaConsultor = encarteiramentoDb?.Matricula;

                    var consultor = _context.Usuario.FirstOrDefault(c => c.Matricula == matriculaConsultor);

                    teds = _context.TED
                        .Include(t => t.Motivo)
                        .Include(t => t.Status)
                        .OrderByDescending(t => t.Data)
                        .Where(t => t.Agencia == cockpit.CodigoAgencia.ToString() && t.Conta == cockpit.Conta.ToString() && !t.Status.Descricao.ToLower().Contains("aplicado"))
                        .ToList();

                    var Invest = _context.TemInvestFacil.FirstOrDefault(c => c.Agencia == cockpit.CodigoAgencia && c.Conta == cockpit.Conta);
                    var temInvest = Invest != null;
                    var clus = _context.Clusterizacoes.FirstOrDefault(c => 
                        c.AGENCIA.ToString() == cockpit.CodigoAgencia.ToString() && c.CONTA.ToString() == cockpit.Conta.ToString());

                    var viewModel = ClienteViewModel.Mapear(cockpit, 
                        consultor, clus, vencimentos, pipelines, teds, 
                        new AgendaViewModel(), null, new Corretora(), new Corretora(), 
                        encarteiramentoDb ,temInvest);

                    listaClienteViewModel.Add(viewModel);
                }


                return listaClienteViewModel;

            }
        }
    }
}