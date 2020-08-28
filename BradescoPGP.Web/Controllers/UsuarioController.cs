using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using BradescoPGP.Common;
using BradescoPGP.Common.Logging;
using BradescoPGP.Repositorio;
using BradescoPGP.Web.Models;

namespace BradescoPGP.Web.Controllers
{
    [Authorize]
    public class UsuarioController : AbstractController
    {
        private readonly PGPEntities _context;

        public UsuarioController(DbContext context) : base(context)
        {
            _context = context as PGPEntities;
        }
        public ActionResult Index()
        {
            if (!User.IsInRole(NivelAcesso.Master.ToString()))
            {
                return View("Error",
                    new ErrorViewModel
                    {
                        Endereco = Url.Action("Index", "Usuario", null, Request.Url.Scheme),
                        Mensagem = "Você não tem permissão para acessar esta página!",
                        Status = "401 - Não autorizado"
                    });
            }


            ViewBag.Titulo = "Perfil";

            return View(_context.Usuario.ToList());
        }

        public ActionResult Perfil(string matricula)
        {
            ViewBag.Titulo = "Perfil";

            var claimsIdentity = ((ClaimsIdentity)User.Identity);

            var matriculaUsuarioLogado = claimsIdentity.FindFirst("matricula").Value;

            var usuario = default(Usuario);

            var usuarioAdmin = User.IsInRole(NivelAcesso.Master.ToString());

            var usuarios = _context.Usuario.Select(u => new UsuarioViewModel
            {
                UsuarioId = u.UsuarioId,
                Matricula = u.Matricula,
                Equipe = u.Equipe,
                Perfil = u.Perfil,
                NomeUsuario = u.NomeUsuario,
                Nome = u.Nome,
            }).ToList();

            if (string.IsNullOrWhiteSpace(matricula))
            {
                usuario = _context.Usuario.FirstOrDefault(u => u.Matricula.ToLower() == matriculaUsuarioLogado.ToLower());

                if (usuarioAdmin)
                {
                    ViewBag.usuarios = usuarios;

                    ViewBag.equipes = usuarios.Select(u => u.Equipe).Distinct();
                }
            }
            else if (usuarioAdmin)
            {
                usuario = _context.Usuario.FirstOrDefault(u => u.Matricula == matricula);

                ViewBag.equipes = usuarios.Select(u => u.Equipe).Distinct();

                ViewBag.usuarios = usuarios;
            }


            if (usuario != null)
            {
                var viewModel = new UsuarioViewModel
                {
                    UsuarioId = usuario.UsuarioId,
                    Matricula = usuario.Matricula,
                    Equipe = usuario.Equipe,
                    MatriculaSupervisor = usuario.MatriculaSupervisor,
                    //TipoUsuario = usuario.TipoUsuario,
                    Perfil = usuario.Perfil,
                    NomeUsuario = usuario.NomeUsuario,
                    Nome = usuario.Nome,
                    NomeSupervisor = usuario.NomeSupervisor,
                    ReceberNotificacaoEvento = usuario.NotificacaoEvento,
                    ReceberNotificacaoPipeline = usuario.NotificacaoPipeline

                };

                return View(viewModel);
            }



            //var linksCapInv = GetLinksCapInvest();
            //ViewBag.LinksCap = linksCapInv.FirstOrDefault(l => l.Titulo == "CapLiq").Url;
            //ViewBag.LinksInvest = linksCapInv.FirstOrDefault(l => l.Titulo == "Invest").Url;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Detalhes(string matricula)
        {
            if (!User.IsInRole(NivelAcesso.Master.ToString()))
            {
                return View("Error",
                    new ErrorViewModel
                    {
                        Endereco = Url.Action("Detalhes", "Usuario", null, Request.Url.Scheme),
                        Mensagem = "Você não tem permissão para acessar esta página!",
                        Status = "401 - Não autorizado"
                    });
            }

            if (matricula == null)
            {
                ViewBag.error = "Desculpe, não foi possível executar esta ação!";

                return View("Perfil");
            }
            var usuario = _context.Usuario.FirstOrDefault(u => u.Matricula == matricula);

            if (usuario == null)
            {
                ViewBag.error = "Desculpe, não foi possível executar esta ação!";

                Log.Error($"Usuário não encontrado, matrícula: {matricula}!");

                return View("Perfil");
            }

            var viewModel = new UsuarioViewModel
            {
                UsuarioId = usuario.UsuarioId,
                Matricula = usuario.Matricula,
                Equipe = usuario.Equipe,
                MatriculaSupervisor = usuario.MatriculaSupervisor,
                ReceberNotificacaoEvento = usuario.NotificacaoEvento,
                ReceberNotificacaoPipeline = usuario.NotificacaoPipeline,
                Perfil = usuario.Perfil,
                NomeUsuario = usuario.NomeUsuario,
                Nome = usuario.Nome,
                NomeSupervisor = usuario.NomeSupervisor
            };

            //var linksCapInv = GetLinksCapInvest();
            //ViewBag.LinksCap = linksCapInv.FirstOrDefault(l => l.Titulo == "CapLiq")?.Url;
            //ViewBag.LinksInvest = linksCapInv.FirstOrDefault(l => l.Titulo == "Invest")?.Url;
            ViewBag.Titulo = "Detalhe";

            return View("Detalhes", viewModel);
        }

        public ActionResult Novo()
        {
            if (!User.IsInRole(NivelAcesso.Master.ToString()))
            {
                return View("Error",
                    new ErrorViewModel
                    {
                        Endereco = Url.Action("Novo", "Usuario", null, Request.Url.Scheme),
                        Mensagem = "Você não tem permissão para acessar esta página!",
                        Status = "401 - Não autorizado"
                    });
            }

            var dadosUsuarios = _context.Usuario.ToList();

            var supervisores = dadosUsuarios
                .Where(u => u.NomeSupervisor.ToLower() != "bradesco" && (u.PerfilId == 1 || u.PerfilId == 2))
                .Distinct()
                .OrderBy(u => u.Nome)
                .ToDictionary(k => k.Nome, v => v.Matricula);

            //var supervisores = dadosUsuariosSupervisores;


            ViewData["Perfil"] = new SelectList(_context.Perfil.ToList().OrderBy(p => p.Descricao), "PerfilId", "Descricao");

            ViewBag.Equipes = dadosUsuarios.Select(u => u.Equipe).Distinct().ToList().ConvertAll(e => new SelectListItem { Text = e.ToUpper(), Value = e });
            ViewData["Supervisores"] = new SelectList(supervisores.Keys.ToList());
            ViewData["Matriculas"] = supervisores;

            //var linksCapInv = GetLinksCapInvest();
            //ViewBag.LinksCap = linksCapInv.FirstOrDefault(l => l.Titulo == "CapLiq")?.Url;
            //ViewBag.LinksInvest = linksCapInv.FirstOrDefault(l => l.Titulo == "Invest")?.Url;
            ViewBag.Titulo = "Perfil - Novo usuario";
            return View();
        }

        // POST: Usuario/Novo
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Novo([Bind(Include = "Nome,Matricula,NomeSupervisor,MatriculaSupervisor,Equipe,NomeUsuario,PerfilId,TipoUsuario")] UsuarioViewModel model)
        {
            if (!User.IsInRole(NivelAcesso.Master.ToString()))
            {
                return View("Error",
                    new ErrorViewModel
                    {
                        Endereco = null,
                        Mensagem = "Você não tem permissão para acessar esta página!",
                        Status = "401 - Não autorizado"
                    });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = new Usuario
                    {
                        NomeUsuario = model.NomeUsuario,
                        Equipe = model.Equipe,
                        Matricula = model.Matricula,
                        MatriculaSupervisor = model.MatriculaSupervisor,
                        Nome = model.Nome,
                        NomeSupervisor = model.NomeSupervisor,
                        PerfilId = model.PerfilId,
                        NotificacaoEvento = true,
                        NotificacaoPipeline = true

                        //,TipoUsuario = model.TipoUsuario
                    };

                    _context.Usuario.Add(usuario);

                    _context.SaveChanges();

                    return RedirectToAction("Perfil");
                }
                catch (Exception ex)
                {
                    ViewBag.error = "Desculpe, não foi possível executar esta ação!";

                    Log.Error($"Erro ao cadastrar novo usuário, matrícula: {model.Matricula}!", ex);

                    ViewData["Perfil"] = new SelectList(_context.Perfil.ToList(), "PerfilId", "Descricao");

                    return View(model);
                }
            }

            ViewData["Perfil"] = new SelectList(_context.Perfil.ToList(), "PerfilId", "Descricao");
            return View(model);
        }

        public ActionResult ExportarExcel()
        {
            var excel = default(byte[]);

            var usuarios = _context.Usuario.ToList();

            excel = GerarExcel(usuarios.ConvertAll(u => UsuariosExportExcel.Mapear(u)), "Hierarquias");

            var nomeArquivo = "Hierarquias.xlsx";

            return File(excel, System.Net.Mime.MediaTypeNames.Application.Octet, nomeArquivo);
        }

        public ActionResult Editar(string matricula)
        {
            if (!User.IsInRole(NivelAcesso.Master.ToString()))
            {
                return View("Error",
                    new ErrorViewModel
                    {
                        Endereco = Url.Action("Editar", "Usuario", null, Request.Url.Scheme),
                        Mensagem = "Você não tem permissão para acessar esta página!",
                        Status = "401 - Não autorizado"
                    });
            }

            if (matricula == null)
            {
                ViewBag.error = "Desculpe, não foi possível carregar os dados!";

                return View();
            }

            var dadosUsuarios = _context.Usuario.ToList();

            var supervisoresMatricula = dadosUsuarios.Where(s => s.NomeSupervisor != "Bradesco" && (s.PerfilId == 1 || s.PerfilId == 2))
                .Distinct()
                .OrderBy(u => u.Nome)
                .ToDictionary(k => k.Nome, v => v.Matricula);

            var usuario = dadosUsuarios.FirstOrDefault(u => u.Matricula == matricula);

            if (usuario == null)
            {
                ViewBag.error = "Desculpe, não foi possível executar esta ação!";

                Log.Error($"Usuário não encontrado, matrícula: {matricula}!");

                return View("Perfil");
            }

            var viewModel = new UsuarioViewModel
            {
                UsuarioId = usuario.UsuarioId,
                Matricula = usuario.Matricula,
                Equipe = usuario.Equipe,
                MatriculaSupervisor = usuario.MatriculaSupervisor,
                //TipoUsuario = usuario.TipoUsuario,
                Perfil = usuario.Perfil,
                PerfilId = usuario.PerfilId,
                NomeUsuario = usuario.NomeUsuario,
                Nome = usuario.Nome,
                NomeSupervisor = usuario.NomeSupervisor
            };

            ViewData["Perfis"] = _context.Perfil.OrderBy(p => p.Descricao).ToList();
            ViewData["Equipes"] = dadosUsuarios.Select(s => s.Equipe).Distinct().ToList();
            ViewData["Supervisores"] = supervisoresMatricula.Keys.ToList(); /*dadosUsuarios.Select(s => s.NomeSupervisor).Distinct().Where(s => s != "Bradesco").ToList();*/
            ViewData["Matriculas"] = supervisoresMatricula;

            ViewBag.Titulo = "Perfil - Editar Usuario";

            //var linksCapInv = GetLinksCapInvest();
            //ViewBag.LinksCap = linksCapInv.FirstOrDefault(l => l.Titulo == "CapLiq")?.Url;
            //ViewBag.LinksInvest = linksCapInv.FirstOrDefault(l => l.Titulo == "Invest")?.Url;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Editar([Bind(Include = "UsuarioId,Matricula,NomeUsuario,Nome,NomeSupervisor,MatriculaSupervisor,Equipe,TipoAcesso,PerfilId")] Usuario usuario)
        {
            if (!User.IsInRole(NivelAcesso.Master.ToString()))
            {
                return View("Error",
                    new ErrorViewModel
                    {
                        Endereco = null,
                        Mensagem = "Você não tem permissão para acessar esta página!",
                        Status = "401 - Não autorizado"
                    });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var local = _context.Usuario.Local.FirstOrDefault(u => u.UsuarioId == usuario.UsuarioId);

                    if (local != null)
                    {
                        _context.Entry(local).State = EntityState.Detached;
                    }

                    var entry = _context.Entry(usuario);

                    entry.State = EntityState.Modified;

                    entry.Property(u => u.Matricula).IsModified = false;

                    entry.Property(u => u.NomeUsuario).IsModified = false;

                    _context.SaveChanges();

                    if (MatriculaUsuario == usuario.Matricula)
                        return RedirectToAction("Logoff", "Conta");
                    else
                        return RedirectToAction("Perfil");
                }
                catch (DbEntityValidationException ex)
                {
                    var errosmessage = ObterValidationErros(ex);

                    Log.Error($"ValidationErros: {string.Join(Environment.NewLine, errosmessage)}");
                    
                    Log.Error($"Erro ao editar usuário, matrícula: {usuario.Matricula}!", ex);

                    return View("Error", new ErrorViewModel
                       {
                           Endereco = null,
                           Mensagem = "Houve um erro inesperado ao editar este usuario, contate o administrador.",
                           Status = "500 - Erro interno"
                       });
                }
                catch(Exception ex)
                {
                    Log.Error($"Erro ao editar usuário, matrícula: {usuario.Matricula}!", ex);

                    return View("Error", new ErrorViewModel
                       {
                           Endereco = null,
                           Mensagem = "Houve um erro inesperado ao editar este usuario, contate o administrador.",
                           Status = "500 - Erro interno"
                       });
                }
            }

            return View(usuario);
        }

        private List<string> ObterValidationErros(DbEntityValidationException e)
        {
            var errosList = new List<string>();

            var errosmessage = "";

            foreach (var eve in e.EntityValidationErrors)
            {
                errosmessage += $"Entidade do tipo \"{eve.Entry.Entity.GetType().Name}\" no estado \"{eve.Entry.State}\" tem os seguintes erros de validação:\n";

                foreach (var ve in eve.ValidationErrors)
                {
                    errosmessage += $"Property: \"{ve.PropertyName}\", Erro: \"{ve.ErrorMessage}\"";
                }

                errosList.Add(errosmessage);
            }
            return errosList;
        }

        public ActionResult Deletar(string matricula)
        {
            if (!User.IsInRole(NivelAcesso.Master.ToString()))
            {
                return View("Error",
                    new ErrorViewModel
                    {
                        Endereco = null,
                        Mensagem = "Você não tem permissão para executar esta ação!",
                        Status = "401 - Não autorizado"
                    });
            }

            if (matricula == null)
            {
                ViewBag.error = "Desculpe, não foi possível executar esta ação!";

                return RedirectToAction("Perfil");
            }

            var usuario = _context.Usuario.FirstOrDefault(u => u.Matricula == matricula);

            if (usuario == null)
            {
                ViewBag.error = "Desculpe, não foi possível executar esta ação!";

                Log.Error($"Usuário não encontrado, matrícula: {matricula}!");

                return RedirectToAction("Perfil");
            }

            try
            {
                _context.Usuario.Remove(usuario);

                _context.SaveChanges();

                return RedirectToAction("Perfil");
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao deletar o usuário, matrícula: {matricula}!", ex);

                ViewBag.error = "Desculpe, não foi possível executar esta ação!";

                return View("Perfil");
            }
        }

        [HttpPost]
        public ActionResult Pesquisa(string nome, string equipe)
        {
            var usuarios = default(List<Usuario>);

            if (!string.IsNullOrWhiteSpace(nome) && !string.IsNullOrWhiteSpace(equipe))
            {
                usuarios = _context.Usuario.Where(u => u.Nome.ToLower().Contains(nome) && u.Equipe.ToLower().Contains(equipe)).ToList();
            }
            else if (!string.IsNullOrWhiteSpace(nome))
            {
                usuarios = _context.Usuario.Where(u => u.Nome.ToLower().Contains(nome)).ToList();

            }
            else if (!string.IsNullOrWhiteSpace(equipe))
            {
                usuarios = _context.Usuario.Where(u => u.Equipe.ToLower().Contains(equipe)).ToList();
            }

            if (string.IsNullOrWhiteSpace(nome) && string.IsNullOrWhiteSpace(equipe))
            {
                usuarios = _context.Usuario.ToList();
            }


            ViewBag.usuarios = usuarios.Select(u => new UsuarioViewModel
            {
                UsuarioId = u.UsuarioId,
                Matricula = u.Matricula,
                Equipe = u.Equipe,
                //MatriculaSupervisor = u.MatriculaSupervisor,
                //TipoUsuario = usuario.TipoUsuario,
                Perfil = u.Perfil,
                NomeUsuario = u.NomeUsuario,
                Nome = u.Nome,
                //NomeSupervisor = u.NomeSupervisor
            }).ToList();

            ViewBag.equipes = _context.Usuario.Select(u => u.Equipe).Distinct().ToList();

            var claimsIdentity = ((ClaimsIdentity)User.Identity);

            var matriculaUsuarioLogado = claimsIdentity.FindFirst("matricula").Value;

            var usuario = _context.Usuario.FirstOrDefault(u => u.Matricula == matriculaUsuarioLogado);

            var viewModel = new UsuarioViewModel
            {
                UsuarioId = usuario.UsuarioId,
                Matricula = usuario.Matricula,
                Equipe = usuario.Equipe,
                MatriculaSupervisor = usuario.MatriculaSupervisor,
                //TipoUsuario = usuario.TipoUsuario,
                Perfil = usuario.Perfil,
                NomeUsuario = usuario.NomeUsuario,
                Nome = usuario.Nome,
                NomeSupervisor = usuario.NomeSupervisor
            };

            ViewData["filtro"] = new FiltroUsuario
            {
                //Nome = nome,
                Equipe = equipe
            };

            return View("Perfil", viewModel);

        }

        [HttpPost]
        public ActionResult ReceberNotificacao(bool valor, string notificacao)
        {
            var usuario = _context.Usuario.FirstOrDefault(u => u.Matricula == MatriculaUsuario);

            if (notificacao == Noticacoes.Evento.ToString()) usuario.NotificacaoEvento = valor;

            else if (notificacao == Noticacoes.Pipeline.ToString()) usuario.NotificacaoPipeline = valor;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { success = false, error = true, message = "Erro ao salvar alterações." });
            }

            return Json(new { success = true, error = false, message = "Cadatramento feito com sucesso" });
        }

    }
}
