using BradescoPGP.Common.Logging;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using BradescoPGP.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using BradescoPGP.Web.Models;
using System.Globalization;

namespace BradescoPGP.Web.Services
{
    public class ObterPSDCAPIC
    {
        public void CarregarPSDC()
        {
            var url = "https://www4.net.bradesco.com.br/psdc/presHome.jsf";
            var user = "f596509";
            var password = "morango6";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.KeepAlive = true;
            request.Accept = @"*/*";

            NetworkCredential credential = new NetworkCredential(user, password);
            CredentialCache credentialCache = new CredentialCache();
            credentialCache.Add(new Uri(url), "Basic", credential);
            request.Credentials = credentialCache;
            request.PreAuthenticate = true;
            request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;

            HttpWebResponse response = null;
            try
            {
                //Primeiro GET
                response = (HttpWebResponse)request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                string htmlResponse = reader.ReadToEnd();


                var doc = new HtmlDocument();

                doc.LoadHtml(htmlResponse);

                var jsf_viewid = doc.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

                var jsf_tree_64 = doc.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                var jsf_state_64 = doc.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                var input_hidden_command_button_suport_ = "/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf";

                var cookies = new CookieContainer();

                request.CookieContainer = cookies;

                //url = "https://www4.net.bradesco.com.br/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf";

                request = (HttpWebRequest)WebRequest.Create(url);

                //Faz o POST com os dados de pesquisa
                var dadosResponse = "";
                var postParameters = new Dictionary<string, string>();
                postParameters.Add("jsf_viewid", jsf_viewid);
                postParameters.Add("jsf_tree_64", jsf_tree_64);
                postParameters.Add("jsf_state_64", jsf_state_64);
                postParameters.Add("consultaIdentificacaoPessoa:rotulo_cpf_cnpj", "07155034304");
                postParameters.Add("consultaIdentificacaoPessoa:rotulo_club", "");
                postParameters.Add("consultaIdentificacaoPessoa:rotulo_agencia", "");
                postParameters.Add("consultaIdentificacaoPessoa:rotulo_conta", "");
                postParameters.Add("consultaIdentificacaoPessoa:digitoConta", "");
                postParameters.Add("consultaIdentificacaoPessoa:rotulo_nome_razao_social:", "");
                postParameters.Add("consultaIdentificacaoPessoa:bt_pesq_nomeRazaoSocial", "Pesquisar");
                postParameters.Add("_input_hidden_command_button_suport_", input_hidden_command_button_suport_);
                postParameters.Add("consultaIdentificacaoPessoa_SUBMIT", "1");
                postParameters.Add("consultaIdentificacaoPessoa:_link_hidden_", "");

                //Enviar apenas quando os dados ja foram preenchidos, e remover o campo consultaIdentificacaoPessoa:bt_pesq_nomeRazaoSocial
                //postParameters.Add("consultaIdentificacaoPessoa:botao_popup_investimento", "Enviar");

                foreach (string key in postParameters.Keys)
                {
                    dadosResponse += HttpUtility.UrlEncode(key) + "="
                          + HttpUtility.UrlEncode(postParameters[key]) + "&";
                }

                request.Method = "POST";
                request.KeepAlive = true;
                request.Accept = @"*/*";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Credentials = credentialCache;
                request.PreAuthenticate = true;
                request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
                dadosResponse = "linkDummyForm:_link_hidden_=_id56&action=#{cadastro_consultaIdentificacaoPessoaBean.iniciarPagina}&stateSafeRollerMenu=1&stateSafeTabMenu=1&jsf_viewid=" + jsf_viewid + "&jsf_state_64=" + jsf_state_64 + "&jsf_tree_64" + jsf_tree_64;

                var byteRequest = Encoding.ASCII.GetBytes(dadosResponse);
                request.ContentLength = byteRequest.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(byteRequest, 0, byteRequest.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                reader = new StreamReader(response.GetResponseStream());

                string htmlResponsePost = reader.ReadToEnd();

                //Envia outro post passando todas as informações do cliente.

                doc.LoadHtml(htmlResponsePost);


                var club = doc.DocumentNode
                    .SelectSingleNode("//input[@name='consultaIdentificacaoPessoa:rotulo_club']").Attributes["value"].Value;

                var nomeRazaoSocial = doc.DocumentNode
                    .SelectSingleNode("//input[@name='consultaIdentificacaoPessoa:rotulo_nome_razao_social']").Attributes["value"].Value;

                if (!string.IsNullOrEmpty(club) && !string.IsNullOrEmpty(nomeRazaoSocial))
                {
                    request = (HttpWebRequest)WebRequest.Create(url);

                    //Faz o POST com os dados de pesquisa
                    dadosResponse = "";
                    postParameters = new Dictionary<string, string>();
                    postParameters.Clear();
                    postParameters.Add("jsf_viewid", jsf_viewid);
                    postParameters.Add("jsf_tree_64", jsf_tree_64);
                    postParameters.Add("jsf_state_64", jsf_state_64);
                    postParameters.Add("consultaIdentificacaoPessoa:rotulo_cpf_cnpj", "07155034304");
                    postParameters.Add("consultaIdentificacaoPessoa:rotulo_club", "");
                    postParameters.Add("consultaIdentificacaoPessoa:rotulo_agencia", "");
                    postParameters.Add("consultaIdentificacaoPessoa:rotulo_conta", "");
                    postParameters.Add("consultaIdentificacaoPessoa:digitoConta", "");
                    postParameters.Add("consultaIdentificacaoPessoa:rotulo_nome_razao_social:", "");
                    postParameters.Add("consultaIdentificacaoPessoa:botao_popup_investimento", "Enviar");
                    postParameters.Add("_input_hidden_command_button_suport_", input_hidden_command_button_suport_);
                    postParameters.Add("consultaIdentificacaoPessoa_SUBMIT", "1");
                    postParameters.Add("consultaIdentificacaoPessoa:_link_hidden_", "");
                    //postParameters.Add("consultaIdentificacaoPessoa:bt_pesq_nomeRazaoSocial", "Pesquisar");

                    foreach (string key in postParameters.Keys)
                    {
                        dadosResponse += HttpUtility.UrlEncode(key) + "="
                              + HttpUtility.UrlEncode(postParameters[key]) + "&";
                    }
                    byteRequest = Encoding.ASCII.GetBytes(dadosResponse);

                    request.Method = "POST";
                    request.KeepAlive = true;
                    request.Accept = @"*/*";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Credentials = credentialCache;
                    request.PreAuthenticate = true;
                    request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;

                    request.ContentLength = byteRequest.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(byteRequest, 0, byteRequest.Length);
                    }

                    response = (HttpWebResponse)request.GetResponse();

                    reader = new StreamReader(response.GetResponseStream());

                    htmlResponsePost = reader.ReadToEnd();
                }


            }
            catch (Exception e)
            {
                Log.Error("Erro na requisição", e);
            }

            //using (var webClient = new CookieAwareWebClient())
            //{
            //    var cc = new CredentialCache();

            //    cc.Add(new Uri(url), "Kerberos", new NetworkCredential("f596509", "morango4", "corp.bradesco.com.br"));

            //    webClient.Credentials = cc;

            //    var htmlResponse = webClient.DownloadString(url);
            //}
        }

        #region Carregar APIC Anterior
        //public void CarregarAPIC()
        //{
        //    var url = "https://intranet8.net.bradesco.com.br/apic/presHome.jsf";
        //    var urlPost = "https://intranet8.net.bradesco.com.br/apic/situacaoAPIConsultar.jsf";

        //    var user = "f596509";
        //    var password = "morango6";

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.Method = "GET";
        //    request.KeepAlive = true;
        //    request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        //    request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        //    request.Host = "intranet8.net.bradesco.com.br";
        //    request.Headers.Add("Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
        //    request.Headers.Add("Upgrade-Insecure-Requests", "1");

        //    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
        //    //request.Headers.Add("Cookie", "dtSa=-");
        //    //request.Connection = "keep-alive";

        //    NetworkCredential credential = new NetworkCredential(user, password);
        //    CredentialCache credentialCache = new CredentialCache();
        //    credentialCache.Add(new Uri(url), "Basic", credential);
        //    request.Credentials = credentialCache;
        //    request.PreAuthenticate = true;
        //    request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;

        //    var container = new CookieContainer();
        //    request.CookieContainer = container;

        //    try
        //    {
        //        HttpWebResponse response = null;

        //        response = (HttpWebResponse)request.GetResponse();

        //        var stream = ReadFully(response.GetResponseStream());

        //        stream = RequestPSDC.Decompress(stream);

        //        var html = Encoding.UTF8.GetString(stream);

        //        var doc = new HtmlDocument();

        //        doc.LoadHtml(html);

        //        var jsf_viewid = doc.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

        //        var jsf_tree_64 = doc.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

        //        var jsf_state_64 = doc.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;


        //        #region AbrePaginaInicial
        //        //Post para abrir a tela de pesquisa

        //        var dadosResponse = "";

        //        var postParameters = new Dictionary<string, string>();

        //        //Cria os parametros para envio no post
        //        postParameters.Add("jsf_viewid", jsf_viewid);
        //        postParameters.Add("jsf_tree_64", jsf_tree_64);
        //        postParameters.Add("jsf_state_64", jsf_state_64);
        //        postParameters.Add("stateSafeRollerMenu", "2");
        //        postParameters.Add("stateSafeTabMenu", "1");
        //        postParameters.Add("action", "#{situacaoAPIBean.carregarPagina}");
        //        postParameters.Add("linkDummyForm:_link_hidden_", "_id62");

        //        foreach (string key in postParameters.Keys)
        //        {
        //            dadosResponse += HttpUtility.UrlEncode(key) + "="
        //                  + HttpUtility.UrlEncode(postParameters[key]) + "&";
        //        }
        //        var parametros = Encoding.ASCII.GetBytes(dadosResponse);


        //        //Configura a requisição
        //        request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Method = "POST";
        //        request.KeepAlive = true;
        //        request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        //        request.Host = "intranet8.net.bradesco.com.br";
        //        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        //        request.Headers.Add("Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
        //        request.Headers.Add("Upgrade-Insecure-Requests", "1");
        //        request.Headers.Add("Origin", "https://intranet8.net.bradesco.com.br");
        //        request.Headers.Add("Cache-Control", "max-age=0");
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.Referer = "https://intranet8.net.bradesco.com.br/apic/resultadoAnalisePerfil.jsf";
        //        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
        //        //request.Headers.Add("Cookie", "JSESSIONID_apic=0000U7Q9wqaR4xwhnH0pj45alJi:1cg2k3da5; rxVisitor=1566333562300CJJH8P0QH07R6VH5H731A5NHCPR35PUD; dtLatC=3; dtPC=26$333581085_381h-vDOMCJOIPNOEEDNOFOIJNGBPDMDFGFODG; dtSa=true%7CC%7C-1%7CConsultar%20Situa%C3%A7%C3%A3o%20API%20do%20Cliente%7C-%7C1566333586842%7C333580802_81%7Chttps%3A%2F%2Fintranet8.net.bradesco.com.br%2Fapic%2FpresHome.jsf%7CBanco%20Bradesco%20S%2FA%7C1566333582930%7C; rxvt=1566335386881|1566333562309; dtCookie=26$2CB4BECBCD744DC38CBF37E0D4433805|423af5f65d46494c|1|50adf5e8e7298694|1");

        //        credential = new NetworkCredential(user, password);
        //        credentialCache = new CredentialCache();
        //        credentialCache.Add(new Uri(url), "Basic", credential);
        //        request.Credentials = credentialCache;
        //        request.PreAuthenticate = true;
        //        request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
        //        request.ContentLength = parametros.Length;

        //        container = new CookieContainer();
        //        request.CookieContainer = container;
        //        //Escreve os dados na requisição
        //        using (var reqStream = request.GetRequestStream())
        //        {
        //            reqStream.Write(parametros, 0, parametros.Length);
        //        }

        //        response = (HttpWebResponse)request.GetResponse();

        //        stream = ReadFully(response.GetResponseStream());

        //        stream = RequestPSDC.Decompress(stream);

        //        html = Encoding.UTF8.GetString(stream);
        //        #endregion


        //        #region Executa Post para pesquisa
        //        //Inserir dados do cliente para Envio no sistema APIC

        //         dadosResponse = "";

        //        postParameters = new Dictionary<string, string>();

        //        //Cria os parametros para envio no post
        //        postParameters.Add("jsf_viewid", jsf_viewid);
        //        postParameters.Add("jsf_tree_64", jsf_tree_64);
        //        postParameters.Add("jsf_state_64", jsf_state_64);
        //        postParameters.Add("radio", "1");
        //        postParameters.Add("situacaoAPIConsultarForm:cpfCnpj", "00000777048");
        //        postParameters.Add("situacaoAPIConsultarForm:avancar", "Avançar");
        //        postParameters.Add("_input_hidden_command_button_suport_", "/apic/situacaoAPIConsultar.jsf");
        //        postParameters.Add("situacaoAPIConsultarForm_SUBMIT", "1");

        //        foreach (string key in postParameters.Keys)
        //        {
        //            dadosResponse += HttpUtility.UrlEncode(key) + "="
        //                  + HttpUtility.UrlEncode(postParameters[key]) + "&";
        //        }
        //        parametros = Encoding.ASCII.GetBytes(dadosResponse);


        //        //Configura a requisição
        //        request = (HttpWebRequest)WebRequest.Create(urlPost);
        //        request.Method = "POST";
        //        request.KeepAlive = true;
        //        request.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        //        request.Host = "intranet8.net.bradesco.com.br";
        //        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        //        request.Headers.Add("Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
        //        request.Headers.Add("Upgrade-Insecure-Requests", "1");
        //        request.Headers.Add("Origin", "https://intranet8.net.bradesco.com.br");
        //        request.Headers.Add("Cache-Control", "max-age=0");
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.Referer = "https://intranet8.net.bradesco.com.br/apic/presHome.jsf";
        //        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";

        //        credential = new NetworkCredential(user, password);
        //        credentialCache = new CredentialCache();
        //        credentialCache.Add(new Uri(url), "Basic", credential);
        //        request.Credentials = credentialCache;
        //        request.PreAuthenticate = true;
        //        request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
        //        request.ContentLength = parametros.Length;

        //        //Escreve os parametros na requisição
        //        using (var reqStream = request.GetRequestStream())
        //        {
        //            reqStream.Write(parametros, 0, parametros.Length);
        //        }

        //        response = (HttpWebResponse)request.GetResponse();

        //        var readerByte = ReadFully(response.GetResponseStream());

        //        var desc = RequestPSDC.Decompress(readerByte);


        //        html = Encoding.UTF8.GetString(desc);

        //        #endregion

        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error("Erro na requisição", e);
        //    }

        //}
        #endregion

        private byte[] ReadFully(Stream input)
        {
            var retorno = default(byte[]);

            try
            {
                int bytesBuffer = 1024;

                byte[] buffer = new byte[bytesBuffer];

                using (MemoryStream ms = new MemoryStream())
                {
                    int readBytes;
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, readBytes);
                    }

                    retorno = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Erro na leitura do bytes", ex);
            }

            return retorno;
        }

        public static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode. // ... Then create a buffer and write into while reading from the GZIP stream.
            using (var stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                var buffer = new byte[size];
                using (var memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);

                        if (count > 0)
                            memory.Write(buffer, 0, count);

                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }


        public static PSDCModeloRetorno CarregarPSDC_OPERACAO_CREDITO(string cpf, string codUsuario, string senha)
        {
            var url = "https://www4.net.bradesco.com.br/psdc/presHome.jsf";
            var user = codUsuario /*"f596509"*/;
            var password = senha /*"morango6"*/;
            var cpfCnpj = cpf /*"000.007.770-48"*/;

            using (var webClient = new CookieAwareWebClient(user, password))
            {
                webClient.Encoding = Encoding.GetEncoding("ISO-8859-1");

                #region Home GET 

                webClient.AddBaseHeaders();

                var htmlResponse = webClient.DownloadString(url);

                #endregion

                #region Menu Serviço ao Usuário -> Informação Cadastral -> Cadastro de cliente

                var htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                var jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                var jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                var jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

                webClient.AddBaseHeaders();

                var htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", jsf_viewid },
                    { "autoScroll", "0,0" },
                    { "actionListener", String.Empty },
                    { "linkDummyForm:_link_hidden_", "_id56" },
                    { "stateSafeRollerMenu", "1" },
                    { "action", "#{cadastro_consultaIdentificacaoPessoaBean.iniciarPagina}" },
                    { "stateSafeTabMenu", "1" }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);

                #endregion

                #region Pesquisa por CPF ou Agencia e Conta

                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

                url = "https://www4.net.bradesco.com.br/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf";

                webClient.AddBaseHeaders();

                htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", jsf_viewid },
                    { "consultaIdentificacaoPessoa:rotulo_cpf_cnpj", cpfCnpj },
                    { "consultaIdentificacaoPessoa:rotulo_club", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_agencia", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_conta", String.Empty },
                    { "consultaIdentificacaoPessoa:digitoConta", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_nome_razao_social", String.Empty },
                    { "consultaIdentificacaoPessoa:bt_pesq_nomeRazaoSocial", "Pesquisar" },
                    { "_input_hidden_command_button_suport_", "/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf" },
                    { "consultaIdentificacaoPessoa_SUBMIT", "1" },
                    { "autoScroll", "0,0" },
                    { "consultaIdentificacaoPessoa:_link_hidden_", String.Empty }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);

                #endregion

                #region Selecao de Primeiro Resultado da GRID

                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

                url = "https://www4.net.bradesco.com.br/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf";

                webClient.AddBaseHeaders();

                htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", jsf_viewid },
                    { "consultaIdentificacaoPessoa:rotulo_cpf_cnpj", cpfCnpj },
                    { "consultaIdentificacaoPessoa:rotulo_club", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_agencia", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_conta", String.Empty },
                    { "consultaIdentificacaoPessoa:digitoConta", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_nome_razao_social", String.Empty },
                    { "codigoLista", "0" }, //DINAMICO ????
                    { "_input_hidden_command_button_suport_", "/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf" },
                    { "consultaIdentificacaoPessoa_SUBMIT", "1" },
                    { "autoScroll", String.Empty },
                    { "consultaIdentificacaoPessoa:dataScroller", String.Empty },
                    {"consultaIdentificacaoPessoa:_link_hidden_", String.Empty }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);


                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

                webClient.AddBaseHeaders();

                htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", jsf_viewid },
                    { "consultaIdentificacaoPessoa:rotulo_cpf_cnpj", cpfCnpj },
                    { "consultaIdentificacaoPessoa:rotulo_club", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_agencia", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_conta", String.Empty },
                    { "consultaIdentificacaoPessoa:digitoConta", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_nome_razao_social", String.Empty },
                    { "codigoLista", "0" }, //DINAMICO ????
                    { "consultaIdentificacaoPessoa:botao_selecionar", "Selecionar" },
                    { "_input_hidden_command_button_suport_", "/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf" },
                    { "consultaIdentificacaoPessoa_SUBMIT", "1" },
                    { "autoScroll", "0,0" },
                    { "consultaIdentificacaoPessoa:_link_hidden_", String.Empty },
                    { "consultaIdentificacaoPessoa:dataScroller", String.Empty }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);


                //url = "https://www4.net.bradesco.com.br/psdc/cadastro.popupOperacaoCredito.jsf";

                //webClient.AddBaseHeaders();

                //htmlResponse = webClient.DownloadString(url);

                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                #endregion

                #region Selecao da Opcao "INVESTIMENTO" NO POPUP

                jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;


                //AJAX REQUEST

                url = "https://www4.net.bradesco.com.br/psdc/cadastro.popupOperacaoCredito.jsf?javax.portlet.faces.DirectLink=true";

                webClient.Headers[HttpRequestHeader.Accept] = "*/*";
                webClient.Headers[HttpRequestHeader.AcceptLanguage] = "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7";
                webClient.Headers[HttpRequestHeader.UserAgent] = CookieAwareWebClient.DefaultUserAgent;
                webClient.Headers[HttpRequestHeader.Referer] = "https://www4.net.bradesco.com.br/psdc/cadastro.popupOperacaoCredito.jsf";
                webClient.Encoding = Encoding.UTF8;

                htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "AJAXREQUEST", "_viewRoot" },
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", "/cadastro.popupOperacaoCredito.jsp" },
                    { "_input_hidden_command_button_suport_", "/psdc/cadastro.popupOperacaoCredito.jsf" },
                    { "popOperacaoCredito_SUBMIT", "1" },
                    { "autoScroll", String.Empty },
                    { "popOperacaoCredito:id20", "popOperacaoCredito:id20" },
                    { "", "" }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);
                #endregion

                #region Selecao da Opcao "Operação" no POP-UP

                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;


                url = "https://www4.net.bradesco.com.br/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf";

                webClient.AddBaseHeaders();

                webClient.Encoding = Encoding.GetEncoding("ISO-8859-1");

                htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", "/cadastro.ConsultaIdentificacaoPessoa.jsp" },
                    { "consultaIdentificacaoPessoa:rotulo_cpf_cnpj", cpfCnpj },
                    { "consultaIdentificacaoPessoa:rotulo_club", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_agencia", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_conta", String.Empty },
                    { "consultaIdentificacaoPessoa:digitoConta", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_nome_razao_social", String.Empty },
                    { "codigoLista", "0" }, //DINAMICO ????
                    { "consultaIdentificacaoPessoa:botao_popup_sim", "Enviar" },
                    { "_input_hidden_command_button_suport_", "/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf" },
                    { "consultaIdentificacaoPessoa_SUBMIT", "1" },
                    { "autoScroll", "0,0" },
                    { "consultaIdentificacaoPessoa:dataScroller", String.Empty },
                    { "consultaIdentificacaoPessoa:_link_hidden_", String.Empty }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);

                #endregion

                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                var dataAtualizacao = htmlDocument.DocumentNode.SelectSingleNode("//input[@id='popupIdentificacao:dataAtualizacao']").Attributes["value"].Value;
                var situacao = htmlDocument.DocumentNode.SelectSingleNode("//input[@id='popupIdentificacao:situacaoCadastral']").Attributes["value"].Value;

                return new PSDCModeloRetorno
                {
                    PSDC_DataAtualizacao = DateTime.Parse(dataAtualizacao),
                    PSDC_SituacaoCadastral = situacao
                };

            }
        }

        public static PSDCModeloRetorno CarregarPSDC_OPERACAO_INVESTIMENTO(string cpf, string codUsuario, string senha)
        {
            var url = "https://www4.net.bradesco.com.br/psdc/presHome.jsf";
            var user = codUsuario /*"f596509"*/;

            var password = senha /*"morango6"*/;

            var cpfCnpj = cpf; /*"000.007.770-48"*/

            using (var webClient = new CookieAwareWebClient(user, password))
            {
                webClient.Encoding = Encoding.GetEncoding("ISO-8859-1");

                #region Home GET 

                webClient.AddBaseHeaders();

                var htmlResponse = webClient.DownloadString(url);

                #endregion

                #region Menu Serviço ao Usuário -> Informação Cadastral -> Cadastro de cliente

                var htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                var jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                var jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                var jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

                webClient.AddBaseHeaders();

                var htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", jsf_viewid },
                    { "autoScroll", "0,0" },
                    { "actionListener", String.Empty },
                    { "linkDummyForm:_link_hidden_", "_id56" },
                    { "stateSafeRollerMenu", "1" },
                    { "action", "#{cadastro_consultaIdentificacaoPessoaBean.iniciarPagina}" },
                    { "stateSafeTabMenu", "1" }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);

                #endregion

                #region Pesquisa por CPF ou Agencia e Conta

                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

                url = "https://www4.net.bradesco.com.br/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf";

                webClient.AddBaseHeaders();

                htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", jsf_viewid },
                    { "consultaIdentificacaoPessoa:rotulo_cpf_cnpj", cpfCnpj },
                    { "consultaIdentificacaoPessoa:rotulo_club", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_agencia", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_conta", String.Empty },
                    { "consultaIdentificacaoPessoa:digitoConta", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_nome_razao_social", String.Empty },
                    { "consultaIdentificacaoPessoa:bt_pesq_nomeRazaoSocial", "Pesquisar" },
                    { "_input_hidden_command_button_suport_", "/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf" },
                    { "consultaIdentificacaoPessoa_SUBMIT", "1" },
                    { "autoScroll", "0,0" },
                    { "consultaIdentificacaoPessoa:_link_hidden_", String.Empty }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);

                #endregion

                #region Selecao de Primeiro Resultado da GRID

                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

                url = "https://www4.net.bradesco.com.br/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf";

                webClient.AddBaseHeaders();

                htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", jsf_viewid },
                    { "consultaIdentificacaoPessoa:rotulo_cpf_cnpj", cpfCnpj },
                    { "consultaIdentificacaoPessoa:rotulo_club", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_agencia", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_conta", String.Empty },
                    { "consultaIdentificacaoPessoa:digitoConta", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_nome_razao_social", String.Empty },
                    { "codigoLista", "0" }, //DINAMICO ????
                    { "_input_hidden_command_button_suport_", "/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf" },
                    { "consultaIdentificacaoPessoa_SUBMIT", "1" },
                    { "autoScroll", String.Empty },
                    { "consultaIdentificacaoPessoa:dataScroller", String.Empty },
                    {"consultaIdentificacaoPessoa:_link_hidden_", String.Empty }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);


                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

                webClient.AddBaseHeaders();

                htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", jsf_viewid },
                    { "consultaIdentificacaoPessoa:rotulo_cpf_cnpj", cpfCnpj },
                    { "consultaIdentificacaoPessoa:rotulo_club", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_agencia", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_conta", String.Empty },
                    { "consultaIdentificacaoPessoa:digitoConta", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_nome_razao_social", String.Empty },
                    { "codigoLista", "0" }, //DINAMICO ????
                    { "consultaIdentificacaoPessoa:botao_selecionar", "Selecionar" },
                    { "_input_hidden_command_button_suport_", "/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf" },
                    { "consultaIdentificacaoPessoa_SUBMIT", "1" },
                    { "autoScroll", "0,0" },
                    { "consultaIdentificacaoPessoa:dataScroller", String.Empty },
                    { "consultaIdentificacaoPessoa:_link_hidden_", String.Empty }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);


                url = "https://www4.net.bradesco.com.br/psdc/cadastro.popupOperacaoCredito.jsf";

                webClient.AddBaseHeaders();

                htmlResponse = webClient.DownloadString(url);

                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                #endregion

                #region Selecao da Opcao "INVESTIMENTO" NO POPUP

                jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;


                //AJAX REQUEST

                url = "https://www4.net.bradesco.com.br/psdc/cadastro.popupOperacaoCredito.jsf?javax.portlet.faces.DirectLink=true";

                webClient.Headers[HttpRequestHeader.Accept] = "*/*";
                webClient.Headers[HttpRequestHeader.AcceptLanguage] = "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7";
                webClient.Headers[HttpRequestHeader.UserAgent] = CookieAwareWebClient.DefaultUserAgent;
                webClient.Headers[HttpRequestHeader.Referer] = "https://www4.net.bradesco.com.br/psdc/cadastro.popupOperacaoCredito.jsf";
                webClient.Encoding = Encoding.UTF8;

                htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "AJAXREQUEST", "_viewRoot" },
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", "/cadastro.popupOperacaoCredito.jsp" },
                    { "_input_hidden_command_button_suport_", "/psdc/cadastro.popupOperacaoCredito.jsf" },
                    { "popOperacaoCredito_SUBMIT", "1" },
                    { "autoScroll", String.Empty },
                    { "popOperacaoCredito:id721", "popOperacaoCredito:id721" },
                    { "", "" }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);



                #endregion

                #region Selecao da Opcao "Operação" no POP-UP

                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;


                url = "https://www4.net.bradesco.com.br/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf";

                webClient.AddBaseHeaders();

                webClient.Encoding = Encoding.GetEncoding("ISO-8859-1");

                htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", "/cadastro.ConsultaIdentificacaoPessoa.jsp" },
                    { "consultaIdentificacaoPessoa:rotulo_cpf_cnpj", cpfCnpj },
                    { "consultaIdentificacaoPessoa:rotulo_club", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_agencia", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_conta", String.Empty },
                    { "consultaIdentificacaoPessoa:digitoConta", String.Empty },
                    { "consultaIdentificacaoPessoa:rotulo_nome_razao_social", String.Empty },
                    { "codigoLista", "0" }, //DINAMICO ????
                    { "consultaIdentificacaoPessoa:botao_popup_investimento", "Enviar" },
                    { "_input_hidden_command_button_suport_", "/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf" },
                    { "consultaIdentificacaoPessoa_SUBMIT", "1" },
                    { "autoScroll", "0,0" },
                    { "consultaIdentificacaoPessoa:dataScroller", String.Empty },
                    { "consultaIdentificacaoPessoa:_link_hidden_", String.Empty }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);

                #endregion

                htmlDocument = new HtmlDocument();
               
                htmlDocument.LoadHtml(htmlResponse);

                var dataAtualizacao = htmlDocument.DocumentNode.SelectSingleNode("//input[@id='popupIdentificacao:dataAtualizacao']").Attributes["value"].Value;
                var situacao = htmlDocument.DocumentNode.SelectSingleNode("//input[@id='popupIdentificacao:situacaoCadastral']").Attributes["value"].Value;

                return new PSDCModeloRetorno
                {
                    PSDC_DataAtualizacao = DateTime.Parse(dataAtualizacao),
                    PSDC_SituacaoCadastral = situacao
                };

            }
        }

        public static ApicModeloRetorno CarregarAPIC(string cpf, string codigoUsuario, string senha)
        {
            var url = "https://intranet8.net.bradesco.com.br/apic/presHome.jsf";

            var user = codigoUsuario /*"f596509"*/;

            var password = senha /*"morango6"*/;

            var cpfCnpj = cpf; //"000.007.770-48";

            using (var webClient = new CookieAwareWebClient(user, password))
            {
                webClient.Encoding = Encoding.GetEncoding("ISO-8859-1");

                #region Home GET 

                webClient.AddBaseHeaders();

                var htmlResponse = webClient.DownloadString(url);

                #endregion

                #region Menu -> Análise Perfil

                var htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                var jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                var jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                var jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

                webClient.AddBaseHeaders();

                var htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", jsf_viewid },
                    { "autoScroll", "0,0" },
                    { "actionListener", String.Empty },
                    { "stateSafeRollerMenu", "2" },
                    { "stateSafeTabMenu", "1" },
                    { "action", "#{situacaoAPIBean.carregarPagina}" },
                    { "linkDummyForm:_link_hidden_", "_id62" }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);

                #endregion

                #region Pesquisa por CPF ou Agencia e Conta

                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                jsf_tree_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_tree_64']").Attributes["value"].Value;

                jsf_state_64 = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_state_64']").Attributes["value"].Value;

                jsf_viewid = htmlDocument.DocumentNode.SelectSingleNode("//input[@name='jsf_viewid']").Attributes["value"].Value;

                url = "https://intranet8.net.bradesco.com.br/apic/situacaoAPIConsultar.jsf";

                webClient.AddBaseHeaders();

                htmlData = webClient.UploadValues(url, new System.Collections.Specialized.NameValueCollection
                {
                    { "jsf_tree_64", jsf_tree_64 },
                    { "jsf_state_64", jsf_state_64 },
                    { "jsf_viewid", jsf_viewid },
                    { "radio", "1" }, //PESQUISA POR CPF
                    { "situacaoAPIConsultarForm:cpfCnpj", cpfCnpj },
                    { "situacaoAPIConsultarForm:avancar", "Avançar" },
                    {"_input_hidden_command_button_suport_", "/apic/situacaoAPIConsultar.jsf" },
                    { "situacaoAPIConsultarForm_SUBMIT", "1" },
                    { "autoScroll", String.Empty },
                    { "situacaoAPIConsultarForm:_link_hidden_", String.Empty }
                });

                htmlResponse = Encoding.GetEncoding("ISO-8859-1").GetString(htmlData);

                #endregion

                htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlResponse);

                var resultados = htmlDocument.DocumentNode.SelectNodes("//td/span[@class='HtmlOutputTextBoldBradesco']");

                var data = default(DateTime);

                var perfil = HttpUtility.HtmlDecode(resultados[2].InnerHtml);

                resultados.First(a => DateTime.TryParseExact(a.InnerHtml,
                    "dd/MM/yyyy",
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.None,
                    out data));



                return new ApicModeloRetorno
                {
                    APIC_DataPerfil = data,
                    APIC_Perfil = perfil
                };
            }
        }

    }
}