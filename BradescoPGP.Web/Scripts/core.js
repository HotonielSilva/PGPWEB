var core = core || {}

core.home = (function ($) {
        
    function mover_link_dropdown(elements) {
        $('#dropdown-links-ul').parent().removeClass('none');

        $.each(elements, function (i, v) {
            $('#dropdown-links-ul').append($(v).prop('outerHTML'));
        })
    }

    function getLinks(url, isMaster) {
        var exclusao = '';
        if (isMaster == 'True') {
            exclusao = `<i data-toggle="modal" data-target="#modal-confirmar-excluir-link" class="fa fa-times fa-1x"></i>`;
        }

        $.ajax({
            url: url,
            success: function (resp) {
                var qtdLink = 0;
                var maxLink = 6;
                $.each(resp, function (ind, val) {
                    if (qtdLink < maxLink) {
                        $('#li-novo-link').after(`<li class="custom-li">
                                        <a href="${val.Url}" target="_blank" class="externalLink custom-link m-l-5">
                                        <span>${val.Titulo.toUpperCase()}</span>
                                    </a> <span class="label custom-icon-del" id=${val.Id}>${exclusao}</span>
                                </li>`);
                    }
                    else {
                        $('#dropdown-links-ul').parent().removeClass('none');
                        $('#dropdown-links-ul').append(`<li class="custom-li">
                                        <a href="${val.Url}" target="_blank" class="externalLink custom-link m-l-5">
                                        <span>${val.Titulo.toUpperCase()}</span>
                                    </a> <span class="label custom-icon-del" id=${val.Id}>${exclusao}</span>
                                </li>`);
                    }
                    qtdLink++;
                })
            }
        })
    }

    function obterSenha() {
        $("#modal-psdc-apic-senha").modal({backdrop: 'static'});
        //return prompt("Digite sua senha para acesso aos sistemas APIC, PSDC, para consulta do perfil do cliente");
    }

    function verificaASPS(temASPS, url) {
        
        var input = $("#password-psdc-apic");

        if (temASPS == 'False') {
            obterSenha();

            $("#form-psdc-apic").submit(function (e) {
                e.preventDefault();
                var s = $("#password-psdc-apic").val();
                if (s == null || s == "") {
                    core.notify.showNotify("Digite sua senha para acesso ao PSDC e APIC", "info", "top-center");
                    return;
                }
                $("#modal-psdc-apic-senha").modal('hide');

                $.post(url + '?sapa=' + s);

            });

            $("#visualizar-senha").mousedown(function () {
                input.attr("type", "text");
            });

            $("#visualizar-senha").mouseup(function () {
                input.attr("type", "password");
            });
        }
    }

    function capLock(e) {
        kc = e.keyCode ? e.keyCode : e.which;
        sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);

        if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
            document.getElementById('divMayus').style.visibility = 'visible';
        else
            document.getElementById('divMayus').style.visibility = 'hidden';
    }

    function move_elements_on_resize() {
        $(window).resize(function () {
            var width_window = window.innerWidth;
            var elements = $('.navbar-custom-menu>.navbar-nav > li > a.custom-link:not(#novo-link)');

            if (width_window >= 992 && width_window < 1200) {
                if (elements.length > 5) {
                    var move = elements.parent().slice(0, 5);
                    mover_link_dropdown(move);
                    move.remove();
                }
            }
            else if (width_window >= 768 && width_window < 991) {
                if (elements.length > 4) {
                    var move = elements.parent().slice(0, 4);
                    mover_link_dropdown(move);
                    move.remove();
                }
            }
            else if (width_window < 768) {
                if (elements.length > 3) {
                    var move = elements.parent().slice(0, 3);
                    mover_link_dropdown(move);
                    move.remove();
                }
            }
        })
    }

    function getEvents() {
        $eventos = $('#agenda-home > .box-body');
        $eventos.html("");

        $.get("/Home/EventosJson", null, function (data) {
            $.each(data, function (i, v) {
                var bg_color = moment(v.DataHora).date() < moment().date() && moment(v.DataHora).month() < moment().month() ? 'bg-red' : 'bg-blue';

                if (bg_color == 'bg-red') {
                    $eventos.prepend(`<div class="event-style ${bg_color}">${moment(v.DataHora).format('HH:mm')} ${v.Titulo}</div>`);
                } else {
                    $eventos.append(`<div class="event-style ${bg_color}">${moment(v.DataHora).format('HH:mm')} ${v.Titulo}</div>`);
                }
            })
        })
    }

    function getPipe(idPipe, urlUpdatePipe, urlObterPipe, urlRedirect) {

        $.get(urlObterPipe, { idPipe }, function (data) {
            core.modal.modalAtualizarpipe(data, urlUpdatePipe, urlObterPipe, urlRedirect);
        })
         .fail(function (e) { console.log(e) });
    }

    function getTed(url, action, urlAplicacao, urlExcluirAplicacao, urlRedirect) {
        $.get(url, function (resp) {
            core.modal.modalAtualizarTED(resp, action, urlAplicacao, urlExcluirAplicacao, urlRedirect);
        });
    }

    function getAplicResg(IdAplicacaoResgate, urlObter, urlAtualizar, filtros) {
        $.get(urlObter, {IdAplicacaoResgate}, function (resp) {
            core.modal.modalContatoAplicResg(resp, urlAtualizar, filtros);
        });
    }

    function resetStatus(ele, url, evento) {
        var tr = $(ele).closest('tr');
        var id = tr.prop('id');
        var tds = tr.children();

        switch (evento) {
            case "TED":
                if (tds[6].textContent == "Em Branco") {
                    core.notify.showNotify('Está TED já esta em seu status inicial', 'info', 'top-center');
                    return;
                }
                break;
            case "Vencimento":
                if (tds[8].textContent == "Em Branco") {
                    core.notify.showNotify('Este vencimento já esta em seu status inicial', 'info', 'top-center');
                    return;
                }
                break;
            case "Pipeline":
                if (tds[5].textContent == "Em Branco") {
                    core.notify.showNotify('Este pipeline já esta em seu status inicial', 'info', 'top-center');
                    return;
                }
                break;
        }

        var modal = $('#modal-confirm');

        modal.find('#modal-confirm-text').text('Tem certeza que deseja reverter o status para esta evento?');
        modal.modal();

        $('#btn-delete-confirm').click(function () {
            modal.modal('hide');

            $.post(url, { id }, function (resp) {
                if (resp.status) {
                    switch (resp.evento) {
                        case "TED":
                            tds[6].textContent = "Em Branco";
                            tds[7].textContent = "";
                            tds[8].textContent = "";
                            break;
                        case "Pipeline":
                            tds[5].textContent = "Em Branco";
                            tds[10].textContent = "";
                            tds[11].textContent = "";
                            tds[12].textContent = "";
                            break;
                        case "Vencimento":
                            tds[8].textContent = "Em Branco";
                            break;
                    }
                }
            });
        });
    }
    
    function init(url) 
    {
        var pathUrlArr = window.location.pathname.split('/').slice(1);
        var currPage = pathUrlArr[pathUrlArr.length - 1];
        var headerPage = currPage ? currPage.charAt(0).toUpperCase() + currPage.slice(1) : "Home";

        var ch = $('section.content-header');
        var arr_li = $('section.content-header > ol');
        var ico = '<i class="fa fa-gear"></i>';

        switch (headerPage.toLocaleLowerCase()) {
            case 'configuracao':
                headerPage = `${ico} Configuração`;
                break;
            case 'aplicacaoresgate':
                headerPage = 'Aplicacao / Resgate';
                break;
            case 'gerencialindicadoresranking':
                headerPage = 'Gerencial > Ranking';
                break;
            case 'gerencialcliente':
                headerPage = 'Gerencial  >  Cliente';
                break;
            case 'gerencialespecialista':
                headerPage = 'Gerencial  >  Especialista';
                break;
            case 'gerencialmotivosubMotivo':
                headerPage = 'Gerencial  >  Motivo Submotivo';
                break;
            case 'gerencialindicadoresentidade':
                headerPage = 'Gerencial  >  Entidade';
                break;
            case 'operacional':
                headerPage = 'Atendimento';
                break;
            case 'parametromotivosubmotivo':
                headerPage = 'Parametro  >  MotivoSubMotivo';
                break;
            case 'parametrostatussubstatus':
                headerPage = 'Parametro  >  StatusSubStatus';
                break;
        }
        var titulo = headerPage.indexOf('>') != -1 ? headerPage.trim().split('>')[1].trim() : headerPage.trim();

        ch.prepend(`<h1>${titulo == 'RedirecionarHome' ? "Home" : titulo}</h1>`); // Adiciona o nome do cabeçalho da pagina

        if (headerPage.indexOf('>') != -1) {
            var crambs = headerPage.trim().split('>');

            for (var i = 0; i < crambs.length; i++) {

                if (crambs[i] != "Home" && crambs[i] != "RedirecionarHome") {
                    var active = i == crambs.length - 1 ? 'active' : '';
                    arr_li.append(`<li class="${active}"> ${crambs[i].trim()} </li>`);
                }
            }
        } else {
            if (headerPage != "Home" && headerPage != "RedirecionarHome") {
                arr_li.append(`<li class="active"> ${headerPage} </li>`);
            }
        }

        moment.locale('pt-br'); //pt-br
        

        //Obter Saldo InvestFacil
        $.get(url, function (resp) {

            var valorInvst = resp.SaldoInvestfacil != 0 ? resp.SaldoInvestfacil.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }) : 'R$ -'
            var valorCapLiq = resp.SaldoCaptacaoLiquida != 0 ? `C/Invest ${resp.SaldoCaptacaoLiquida.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}` : 'C/Invest R$ -'
            var valorCapLiqSemInvest = resp.SaldoCaptacaoLiquidaSemInvest != 0 ? `S/Invest ${resp.SaldoCaptacaoLiquidaSemInvest.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}` : 'C/Invest R$ -'

            $('#saldo-invest').text(valorInvst);
            $('#saldo-cap-liq').text(valorCapLiq);
            $('#saldo-cap-liq-sem-invest').text(valorCapLiqSemInvest);
        })
    }

    function efeitoSelecaoLinha(seletorTabela) {
        $(`${seletorTabela} > tbody`).on('click','tr', function () {

            if ($(this).hasClass('rowSelected')) {
                $(this).removeClass('rowSelected');
                return;
            }

            var trs = $(`${seletorTabela}  tr`);

            trs.each(function (i, v) {
                $(v).removeClass('rowSelected');
            });

            $(this).addClass('rowSelected');

        });
    }

    function linhaSelecionadaEfeito(ele, seletorTabela) {

        if ($(ele).hasClass('rowSelected')) {
            $(ele).removeClass('rowSelected');
            return;
        }

        var trs = $(`${seletorTabela} > tbody > tr`);

        trs.each(function (i, v) {
            $(v).removeClass('rowSelected');
        });

        $(ele).addClass('rowSelected');
    }

    function novaJanela(url, nomeJanela) {
        window.open(url, nomeJanela,
            'width=1300, height=800, directories=no,location=no,menubar=no,scrollbars=yes,status=no,toolbar=no,resizable=yes')
    }

    function PSDC(url, agencia, conta, cpf) {

        var features = 'width=1300, height=800, directories=no,location=no,menubar=no,scrollbars=yes,status=no,toolbar=no,resizable=yes';
        var aleatorio = Math.random() + Math.floor(Date.now()/1000);
        var novaJanela = window.open('', 'psdc', features);
        
        var form = novaJanela.document.createElement('form');
        form.setAttribute('id', 'form-psdc');
        form.setAttribute('method', 'post');
        form.setAttribute('action', url);

        var rotuloCpf = novaJanela.document.createElement('input');
        rotuloCpf.setAttribute('type', 'text');
        rotuloCpf.setAttribute('name', 'consultaIdentificacaoPessoa:rotulo_cpf_cnpj');
        rotuloCpf.setAttribute('value', '');
        form.appendChild(rotuloCpf);

        var rotuloAgencia = novaJanela.document.createElement('input');
        rotuloAgencia.setAttribute('type', 'text');
        rotuloAgencia.setAttribute('name', 'consultaIdentificacaoPessoa:rotulo_agencia');
        rotuloAgencia.setAttribute('value', agencia);
        form.appendChild(rotuloAgencia);

        var contaSemDig = conta.substr(0, conta.length - 1);
        var digConta = conta.substr(conta.length - 1);

        var rotuloConta = novaJanela.document.createElement('input');
        rotuloConta.setAttribute('type', 'text');
        rotuloConta.setAttribute('name', 'consultaIdentificacaoPessoa:rotulo_conta');
        rotuloConta.setAttribute('value', contaSemDig);
        form.appendChild(rotuloConta);

        var digitoConta = novaJanela.document.createElement('input');
        digitoConta.setAttribute('type', 'text');
        digitoConta.setAttribute('name', 'consultaIdentificacaoPessoa:digitoConta');
        digitoConta.setAttribute('value', digConta);
        form.appendChild(digitoConta);

        var submit = novaJanela.document.createElement('input');
        submit.setAttribute('type', 'text');
        submit.setAttribute('name', 'consultaIdentificacaoPessoa_SUBMIT');
        submit.setAttribute('value', 1);
        form.appendChild(submit);

        var popupInvestimento = novaJanela.document.createElement('input');
        popupInvestimento.setAttribute('type', 'text');
        popupInvestimento.setAttribute('name', 'consultaIdentificacaoPessoa:botao_popup_investimento');
        popupInvestimento.setAttribute('value', 'Enviar');
        form.appendChild(popupInvestimento);

        var buttonSuport = novaJanela.document.createElement('input');
        buttonSuport.setAttribute('type', 'text');
        buttonSuport.setAttribute('name', '_input_hidden_command_button_suport_');
        buttonSuport.setAttribute('value', '/psdc/cadastro.ConsultaIdentificacaoPessoa.jsf');
        form.appendChild(buttonSuport);

        novaJanela.document.body.appendChild(form);

        form.style.display = 'none';

        form.submit();
    }

    function redirecionarHome(ele, url) {
        $(ele).css({ 'background-color': '#f69595' })
        var agencia = $($(ele).children()[1]).text()
        var conta = $($(ele).children()[2]).text()
        window.location.href = url+'?agencia=' + agencia + '&conta=' + conta;
    }

    function recarregaPipelinesHome(data, urlAtualizarPipe, urlObterPipe) {
        $pipe = $('#pipe');
        $pipe.empty();
        data = JSON.parse(data);
        if (data && data.length > 0) {
            var qtdEmBranco = 0;
            var date = new Date();
            //var maxDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);
            $.each(data, (i, v) => {
                var statusValidos = ["Em Branco", "Prorrogado"];
                if (statusValidos.indexOf(v.Situacao) != -1) {
                    var valor = v.ValorDoPipe.toLocaleString('pr-BR', { currency: 'BRL', style: 'currency' });
                    var previstoProrrogado = v.DataProrrogada != null ?
                        `Prorrogado para: ${moment(v.DataProrrogada).format('DD/MM/YYYY')}` :
                        `previsto para: ${moment(v.DataPrevista).format('DD/MM/YYYY')}`

                    $pipe.append(`<li>
                            <small> ${v.Origem || "Sem origem"} &nbsp; | &nbsp; ${valor} &nbsp; | &nbsp; ${previstoProrrogado}&nbsp; | &nbsp; ${v.Situacao}</small> &nbsp;
                            <button class="btn-edit-pencil" id="${v.Id}" onclick="core.home.getPipe(this.id, '${urlAtualizarPipe}', '${urlObterPipe}')"><i class="fa fa-pencil"  style="color:red"></i></button><br /></li>`);
                    qtdEmBranco++
                }
            });

            if (qtdEmBranco == 0) {
                $pipe.append(`<li>Nenhuma informação localizada</li>`);
            }


        } else {
            $pipe.append(`<li>Nenhuma informação localizada</li>`);
        }
    }

    function recarregaVencimentosHome(data) {
        var $venc = $('#vencimento')

        if (data.error != undefined && data.error) {
            core.notify.showNotify("Erro ao atualizar o vencimento", "error", "top-center");
            return;
        }
        var querySelect = document.querySelector('#vencimento select');
        var selectlist = !Array.isArray(querySelect) ? querySelect.cloneNode(true) : querySelect.get(0).cloneNode(true);

        $venc = $('#vencimento').empty();

        if (data && data.length > 0) {
            var qtdLiCriada = 0;

            $.each(data, (i, v) => {
                var statusPermitidos = ['em branco', 'em negociação', 'contato sem sucesso'];

                if (statusPermitidos.indexOf(v.Status.toLowerCase()) != -1) {
                    var valor = v.SaldoAtual != null ? v.SaldoAtual.toLocaleString('pr-BR', { currency: 'BRL', style: 'currency' }) : '';

                    options = $(selectlist).children();
                    $.each(options, function (i, o) {
                        if ($(o).val() == v.StatusId) {
                            $(o).attr('selected', true);
                        }
                    });

                    var selectlistHtml = selectlist.outerHTML;

                    $venc.append(`<li data-id="${v.Id}">
                            <small> ${v.Produto} - ${moment(v.DataVencimento).format('DD/MM/YYYY')} - ${valor} - ${v.Status}</small> &nbsp;
                            <button onclick="$(this).next().toggleClass('none')" class="btn-edit-pencil"><i  class="fa fa-pencil" style="color:red"></i></button>
                                <span class="none">
                                    ${selectlistHtml}
                                    <button onclick="atualizarVencimento(this)" class="btn btn-danger btn-sm" id="alterar-status-venc-btn">Alterar</button>
                                </span>
                            <br /></li>`);
                    qtdLiCriada++;
                }
            });
            if (qtdLiCriada == 0) {
                $venc.append(`<li>Nenhuma informação localizada</li>`);
            }

        } else {
            $venc.append(`<li>Nenhuma informação localizada</li>`);
        }
    }

    function inicialPreLoader() {
        //preloader
        $('html,body').animate({ scrollTop: 0 }, 'fast')
        $('#preloader .inner').show();
        $('#preloader').show();
        $('body').delay(350).css({ 'overflow': 'hidden' });
    }

    function pararPreLoader() {
        //preloader
        $('html,body').animate({ scrollTop: 0 }, 'fast')
        $('#preloader .inner').fadeOut();
        $('#preloader').delay(350).fadeOut('slow');
        $('body').delay(350).css({ 'overflow': 'visible' });
    }

    function mostrarDadosPesquisaHome(data, urlRedirectHome) {

        if (Array.isArray(data)) {
            //inicialPreLoader();
            $("#box-home").fadeOut();
            $("#box-resultados").removeClass('none');

            //$("#aviso-usuarios-encontrados").text(`Foram encontrados ${data.length} clientes que contém '${$("#nome").val()}' no nome!`);

            var table = $("#resultados").DataTable();
            table.destroy();

            $("#resultados tbody").html('');

            data.forEach((e, i) => {
                $("#resultados tbody").append(`
                <tr id="${e.Id}" ondblclick="core.home.redirecionarHome(this, '${urlRedirectHome}')">
                    <td>${e.NomeCliente}</td>
                    <td>${e.Agencia}</td>
                    <td>${e.Conta}</td>
                    <td>${e.CPFCNPJ}</td>
                </tr>`);
            });

            $("#resultados").DataTable({
                language: {
                    "lengthMenu": "Mostrar _MENU_ resultados por página",
                    "zeroRecords": "Nenhum registro encontrado",
                    "info": "Foram encontrados _TOTAL_ resultados",
                    "infoFiltered": "(A pesquisa retornou _TOTAL_ resultados do total de _MAX_)",
                    "infoEmpty": "Nehum dado disponivel",
                    "search": "Pesquisar",
                    "oPaginate": {
                        "sNext": "Próximo",
                        "sPrevious": "Anterior",
                        "sFirst": "Primeiro",
                        "sLast": "Último"
                    },
                },
                lengthMenu:[25,50,100]
            });

            core.home.efeitoSelecaoLinha('#resultados')

            $('#nome').val('');

            pararPreLoader();
        }
        else {
            $("#box-resultados").fadeOut();
            $('#box-home').fadeIn('slow');
            $('#box-popup-sinv').show();

            $cadu = $('#cadu').empty();
            $niver = $('#niver').empty();
            $nome = $('#nome-cli').empty();
            $api = $('#api').empty();
            $venc = $('#vencimento').empty();
            $pipe = $('#pipe').empty();
            $ted = $('#ted').empty();
            $corretora = $('#corretora').empty();
            $consultor = $('#consultor').empty();

            $('input#agencia').val('');
            $('input#conta').val('');
            $('input#nome').val('');
            $('input#cpf-cnpj').val('');

            $('#small-agencia').text(data.Agencia);
            $('#small-conta').text(data.Conta);
            $('#small-cpf-cnpj').text(data.CPFCNPJ);

            if (data.Aniversario)
                $niver.text(moment(data.Aniversario).format('DD/MM'));
            else
                $niver.text(`ND`);

            if (data.Cadu)
                $cadu.text(`${moment(data.Cadu.Data).format('DD/MM/YYYY')} - ${data.Cadu.Status}`);
            else
                $cadu.text(`ND`);

            if (data.corretora && data.corretora.length > 0)
                $corretora.text(); //Falta os dados sobre a corretora
            else
                $corretora.text(`ND`);

            $nome.text(data.NomeCliente);

            data.API ? $api.html(`<small>${data.API.Data} <br>${data.API.Status}</small>`) : $api.text("ND");

            if (data.Vencimentos && data.Vencimentos.length > 0) {
                $.each(data.Vencimentos, (i, v) => {
                    var valor = v.SaldoAtual != null ? v.SaldoAtual.toLocaleString('pr-BR', { currency: 'BRL', style: 'currency' }): '';
                    $venc.append(`<li>
                            <small> ${v.Produto} - ${valor} </small> &nbsp;
                            <button class="btn-edit-pencil" data-search=""><i class="fa fa-pencil" style="color:red"></i></button><br /></li>`);
                });
            } else {
                $venc.append(`<li>Nenhuma informação localizada</li>`);
            }

            if (data.Pipelines && data.Pipelines.length > 0) {
                var qtdEmBranco = 0;
                var date = new Date();
                var maxDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                $.each(data.Pipelines, (i, v) => {
                    if (v.Situacao == "Em Branco" || (v.DataProrrogada != null && moment(v.DataProrrogada) <= maxDate)) {
                        var valor = v.ValorDoPipe.toLocaleString('pr-BR', { currency: 'BRL', style: 'currency' });
                        $pipe.append(`<li>
                            <small> ${v.Origem} &nbsp; | &nbsp; ${valor} &nbsp; | &nbsp; Previsto para: ${moment(v.DataPrevista).format('DD/MM/YYYY')} &nbsp; | &nbsp; ${v.Situacao}</small> &nbsp;
                            <button class="btn-edit-pencil" id="${v.Id}" onclick="core.home.getPipe(this.id, '@Url.Action("AtualizarPipeline","Home")', '@Url.Action("ObterPipe","Home")')"><i class="fa fa-pencil"  style="color:red"></i></button><br /></li>`);
                        qtdEmBranco++;
                    }
                });

                if (qtdEmBranco == 0) {
                    $pipe.append(`<li>Nenhuma informação localizada</li>`);
                }
            } else {
                $pipe.append(`<li>Nenhuma informação localizada</li>`);
            }

            if (data.TEDs && data.TEDs.length > 0) {
                $.each(data.TEDs, (i, v) => {
                    var valor = v.Valor.toLocaleString('pr-BR', { currency: 'BRL', style: 'currency' });
                    $ted.append(`<li>
                            <small>${valor} - ${moment(v.DataPrevista).format('DD/MM/YYYY')}</small> &nbsp;
                            <button  id="${v.Id}" class="btn-edit-pencil" onclick="core.home.getTed(this.id)"><i class="fa fa-pencil" style="color:red"></i></button><br /></li>`);
                });
            }
            else {
                $ted.append(`<li>Nenhuma informação localizada</li>`);
            }

            if (data.Consultor.Nome != "") {
                $consultor.text(`Carteira de: ${data.Consultor.Nome}`);
            } else {
                $consultor.text(`Cliente não possuí encarteiramento`);
            }
        }
    }

    function search_by_agencia_conta_cpfcpnj(requestUrl, agencia, conta, cpfCnpj, nome, urlRedirectHome, especialista) {
        $.ajax({
            url: requestUrl,
            type: 'GET',
            data: { agencia, conta, cpfCnpj, nome, especialista },
            success: function (data) {

                if (data.Agencia == null && data.Conta == null && data.Id == 0) {
                    core.notify.showNotify("Nehum registro encontrado.", 'info', 'top-center')
                    return false;
                }

                mostrarDadosPesquisaHome(data, urlRedirectHome);

                var obj = { Agencia: data.Agencia, Conta: data.Conta, Consultor: data.Consultor, CPFCNPJ: data.CPFCNPJ, NomeCliente: data.NomeCliente, Id: data.Id }
                sessionStorage.setItem('cliente', JSON.stringify(obj));
            },
            error: function (a, b, c) {
                console.error(a);
                console.error(b);
                console.error(c);
            }
        });
    }

    function removeLink() {
        $(document).on('click', '.custom-icon-del', function () {
            core.modal.modalCofirmarExclusaoLink();
            $('#confirmar').click(() => {
                $(this).closest('li').remove();
            })

            if ($('#dropdown-links-ul li').length == 0) {
                $('#dropdown-links-ul').parent().addClass('none');
            }
        })

    }

    return {
        init,
        removeLink,
        search_by_agencia_conta_cpfcpnj,
        getEvents,
        getPipe,
        getTed,
        getLinks,
        novaJanela,
        efeitoSelecaoLinha,
        redirecionarHome,
        mostrarDadosPesquisaHome,
        recarregaPipelinesHome,
        recarregaVencimentosHome,
        inicialPreLoader,
        pararPreLoader,
        PSDC,
        verificaASPS,
        capLock,
        resetStatus,
        linhaSelecionadaEfeito,
        getAplicResg
    }

})($)



