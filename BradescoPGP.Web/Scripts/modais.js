var core = core || {}

core.modal = (function ($) {


    function adicionarEventoAgenda(data) {
        var box = $('#meus-eventos  div.eventos');
        var start = moment(data.start);
        if (start.format('DD/MM/YYYY') == moment().subtract(1, 'days').format('DD/MM/YYYY') ||
            start.format('DD/MM/YYYY') <= moment().format('DD/MM/YYYY')) {
            var bg = start.format('DD/MM/YYYY HH:mm:ss') < moment().format('DD/MM/YYYY HH:mm:ss') ? 'bg-red' : 'bg-blue';

            if (bg == 'bg-blue') {
                box.prepend(`<div class="external-event ${bg}">${start.format('DD/MM/YYYY HH:mm')} ${data.title}
                                <span data-id="${data.id}" class="excluir-evento pull-right"><i class="fa fa-close"></i></span>
                            </div>`);
            } else {
                box.append(`<div class="external-event ${bg}"> ${start.format('DD/MM/YYYY HH:mm')} ${data.title}
                                <span data-id="${data.id}" class="excluir-evento pull-right"><i class="fa fa-close"></i></span>
                            </div>`);
            }
        }
    }

    function modalNovoEvento(url, consultor, matricula) {
        var modalNovoEventoHtml = `<div class="modal fade " id="modal-novo-evento" style="display: block; padding-right: 17px;">
                  <div class="modal-dialog">
                    <div class="modal-content">
                      <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                          <span aria-hidden="true">×</span></button>
                        <h4 class="modal-title">Novo Evento</h4>
                      </div>
                      <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-9">
                                <div class="form-group">
                                    <label for="nome">Nome</label>
                                    <input type="text" readonly value="${consultor}" id="nome" class="form-control"/>
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <label for="matricula">Matricula</label>
                                    <input type="number" readonly value="${matricula}" id="matricula" class="form-control"/>
                                </div>
                            </div>
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="titulo">Titulo</label>
                                    <input type="text" id="titulo" class="form-control"/>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="data-inicial">Data Inicial</label>
                                    <input type="date" id="data-inicial" class="form-control"/>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="hora-inicial">Hora Inicial</label>
                                    <input type="time" id="hora-inicial" class="form-control"/>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="data-final">Data Final</label>
                                    <input type="date" id="data-final" class="form-control"/>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="hora-final">Hora Final</label>
                                    <input type="time" id="hora-final" class="form-control"/>
                                </div>
                            </div>
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="hora">Descrição</label>
                                    <textarea id="descricao" class="form-control" rows="6"></textarea>
                                </div>
                            </div>
                      </div>
                        </div>
                        
                      <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Cancelar</button>
                        <button type="submit" id="save-calendar-changes" class="btn btn-danger">Criar</button>
                      </div>
                    </div>
                    <!-- /.modal-content -->
                  </div>
                  <!-- /.modal-dialog -->
                </div>`;
        $(modalNovoEventoHtml).appendTo('#modais');
        $('#modal-novo-evento').modal({ backdrop: 'static' });

        $('#save-calendar-changes').click(function () {
            var nome = $('.modal .modal-body #nome').val();
            var matriculaConsultor = $('.modal .modal-body #matricula').val();
            var titulo = $('.modal .modal-body #titulo').val();
            var dataInicial = $('.modal .modal-body #data-inicial').val();
            var horaInicial = $('.modal .modal-body #hora-inicial').val();
            var dataFinal = $('.modal .modal-body #data-final').val();
            var horaFinal = $('.modal .modal-body #hora-final').val();
            var descricao = $('.modal .modal-body #descricao').val();

            var dataHoraInicio = dataInicial + ' ' + horaInicial;
            var dataFim = dataFinal + ' ' + horaFinal;

            if (titulo == "") {
                core.notify.showNotify("O campo Titulo é obrigatorio", 'info', 'top-center');
                return;
            }
            if (dataInicial == "") {
                core.notify.showNotify("O campo Data Inicial é obrigatorio", 'info', 'top-center');
                return;
            }
            if (horaInicial == "") {
                core.notify.showNotify("O campo Hora Inicial é obrigatorio", 'info', 'top-center');
                return;
            }
            if (dataFinal == "") {
                core.notify.showNotify("O campo Data Final é obrigatorio", 'info', 'top-center');
                return;
            }
            if (horaFinal == "") {
                core.notify.showNotify("O campo Data Final é obrigatorio", 'info', 'top-center');
                return;
            }
            if (dataHoraInicio > dataFim || dataHoraInicio == dataFim) {
                core.notify.showNotify("O range de data está no formato inválido, o campo Data Inicial não pode ser maior ou igual ao campo de Data Final ", 'info', 'top-center');
                return;
            }
            if (descricao == "") {
                descricao = "Sem descrição informada.";
            }

            var dados = { nome, matriculaConsultor, titulo, descricao, dataHoraInicio, dataFim };

            $.post(url, dados, function (data) {
                if (data.success) {
                    core.notify.showNotify("Evento criado com sucesso!", "success", "top-center");

                    if (window.location.pathname.toLowerCase().match(/agenda/g)) {
                        $('#calendar').fullCalendar('renderEvent', { start: moment(dataHoraInicio), end: moment(dataFim), title: titulo, description: descricao }, true)
                    }

                    adicionarEventoAgenda({ start: dataHoraInicio, end: dataFim, title: titulo, id: data.evento.Id });
                } else {
                    core.notify.showNotify("Desculpe não foi possivel criar um novo evento no momento", "error", "top-center");
                }
                $('#modal-novo-evento').modal('hide');
            });
        })

        $('#modal-novo-evento').on('hidden.bs.modal', function (e) {
            $(this).remove();
        });
    }

    function cria_links(url, max_links) {
        max_links = max_links || 6;
        var qtdLinks = $('.navbar-nav .custom-li .custom-icon-del').length;

        $('#save-novo-link-changes').click(function () {

            var Titulo = $("input#link-titulo").val().toUpperCase();
            var Url = $('input#link-url').val();

            if (Titulo == '' || Url == '') {
                core.notify.showNotify('Para criar um novo link você deve preencher todos os dados.', 'info', 'top-center')
                return;
            }

            var filePrefix = '';
            if (`${Url}`.includes("\\")) {
                filePrefix = 'file:///';
            }
            Url = filePrefix + Url

            $.post(url, { Titulo, Url, Exibir: true }, function (resp) {
                if (qtdLinks < max_links) {
                    $('#novo-link').parent().after(`<li class="custom-li">
                                    <a href="${resp.Url}" target="_blank" class="externalLink custom-link m-l-5">
                                        <span>${resp.Titulo.toUpperCase()}</span>
                                    </a>
                                    <span class="label custom-icon-del" id=${resp.Id} ><i data-toggle="modal" data-target="#modal-confirmar-excluir-link" class="fa fa-times fa-1x" ></i></span>
                        </li>`);
                }

                else {
                    $('#dropdown-links-ul').parent().removeClass('none');
                    $('#dropdown-links-ul').append(`<li class="custom-li">
                                    <a href="${resp.Url}" target="_blank" class="externalLink custom-link m-l-5">
                                        <span>${resp.Titulo.toUpperCase()}</span>
                                    </a>
                                    <span class="label custom-icon-del" id=${resp.Id} ><i data-toggle="modal" data-target="#modal-confirmar-excluir-link" class="fa fa-times fa-1x" ></i></span>
                        </li>`);
                }
            })
            $('#modal-novo-link').modal('hide');
        })
    }

    function newLink(url) {
        var width_window = window.innerWidth;

        if (width_window >= 992 && width_window < 1200) {
            cria_links(url, 5);
        }
        else if (width_window >= 768 && width_window < 991) {
            cria_links(url, 4);
        }
        else if (width_window < 768) {
            cria_links(url, 3);
        }
        else {
            cria_links(url);
        }
    }

    function modalNovolink(url) {
        var modalNovoLink = `<div class="modal fade " id="modal-novo-link" style="display: block; padding-right: 17px;">
                  <div class="modal-dialog">
                    <div class="modal-content">
                      <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                          <span aria-hidden="true">×</span></button>
                        <h4 class="modal-title">Cadastrar Novo Link</h4>
                      </div>
                      <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="nome">Titulo do link</label>
                                    <input type="text" id="link-titulo" class="form-control"/>
                                </div>
                            </div>
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="agencia">URL (Site)</label>
                                    <input type="text" id="link-url" class="form-control"/>
                                </div>
                            </div>
                       </div>
                     </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Cancelar</button>
                        <button type="submit" id="save-novo-link-changes" class="btn btn-danger">Criar</button>
                      </div>
                    </div>
                    <!-- /.modal-content -->
                  </div>
                  <!-- /.modal-dialog -->
                </div>`;

        $(modalNovoLink).appendTo('#modais')
        $('#modal-novo-link').modal({ backdrop: 'static' })

        newLink(url);

        $('#modal-novo-link').on('hidden.bs.modal', function (e) {
            $(this).remove()
        })
    }

    function modalCofirmarExclusaoLink(ele, url) {
        $('#modais').append(`<div class="modal fade " id="modal-confirmar-excluir-link" style="display: block; padding-right: 17px;">
                  <div class="modal-dialog">
                    <div class="modal-content">
                      <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                          <span aria-hidden="true">×</span></button>
                        <h4 class="modal-title">Excluir Link</h4>
                      </div>
                      <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <h5><strong>Tem certeza que deseja excluir este link?</strong></h5>
                            </div>
                       </div>
                     </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Cancelar</button>
                        <button type="submit" id="confirmar" class="btn btn-danger">Confirmar</button>
                      </div>
                    </div>
                    <!-- /.modal-content -->
                  </div>
                  <!-- /.modal-dialog -->
                </div>`);

        $('#modal-confirmar-excluir-link').modal({ backdrop: 'static' });

        $('#confirmar').click(function () {

            $('#modal-confirmar-excluir-link').modal('hide');


            //Deleta no banco
            $.post(url, { id: ele.prop('id') }, function (resp) {
                if (resp.status) {
                    ele.closest('li').remove();

                    var qtdLinks = $('.navbar-custom-menu > ul.navbar-nav > li.custom-li .custom-icon-del').length;
                    var linksDrop = $('#dropdown-links-ul li');

                    if (qtdLinks < 6 && linksDrop.length > 0) {
                        var link = linksDrop[0];
                        $('#novo-link').parent().after(link);
                    }


                    if ($('#dropdown-links-ul li').length == 0) {
                        $('#dropdown-links-ul').parent().addClass('none');
                    }

                    core.notify.showNotify('Link excluido com sucesso', 'success', 'top-center');
                }
            });
           

            
        });



        $('#modal-confirmar-excluir-link').on('hidden.bs.modal', function (e) {
            $(this).remove()
        });
    }

    function modalNovopipe(url, UrlUpdatePipe, urlObterPipe, urlRedirect) {

        var elem = document.querySelector('#OrigemId').cloneNode(true);
        $(elem).removeClass('none');
        var origens = elem.outerHTML;


        var elemMotivo = document.querySelector('#MotivoId').cloneNode(true);
        $(elemMotivo).removeClass('none');
        var motivo = elemMotivo.outerHTML;


        var elemStatus = document.querySelector('#Status').cloneNode(true);
        elemStatus.setAttribute('id', 'StatusId');
        $(elemStatus).removeClass('none');
        var options = $(elemStatus).children();
        $.each(options, function (i, v) {
            if (v.text == "Em Branco") {
                $(v).attr('selected', '');
            }
        })

        var status = elemStatus.outerHTML;
        var cliente = JSON.parse(sessionStorage.getItem('cliente'))

        $('#modais').append(
            `<div class="modal fade " id="modal-novo-pipe" style="display: block; padding-right: 17px;">
                  <div class="modal-dialog">
                    <div class="modal-content">
                      <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                          <span aria-hidden="true">×</span></button>
                        <h4 class="modal-title">Novo Pipeline</h4>
                      </div>
                      <div class="modal-body">
                        <div class="row">
                           <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="Especialista">Especialista</label>
                                    <input type="text" readonly value="${cliente.Consultor.Nome || ''}" id="Consultor" class="form-control"/>
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="Cliente">Cliente</label>
                                    <input type="text" readonly value="${cliente.NomeCliente || ''}" id="NomeCliente" class="form-control"/>
                                </div>
                             </div>
                             <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="Agencia">Agencia</label>
                                    <input type="text" readonly id="Agencia" value="${cliente.Agencia}" class="form-control"/>
                                </div>
                             </div>
                             <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="Conta">Conta</label>
                                    <input type="text" readonly id="Conta" value="${cliente.Conta}" class="form-control"/>
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="ValorDoPipe">Valor Do Pipe</label>
                                    <input type="text" id="ValorDoPipe" class="form-control"/>
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="Origem">Origem</label>
                                    ${origens}
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="DataPrevista">Data Prevista</label>
                                    <input type="date" id="DataPrevista" value="" class="form-control"/>
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="DataPrevista">Hora Prevista</label>
                                    <input type="time" id="HoraPrevista" value="08:00:00" class="form-control"/>
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="ValoresNoMercado"> Valores No Mercado</label> 
                                    <input type="text" id="ValoresNoMercado" class="form-control"/> 
                                </div>
                             </div>

                            <div class="col-lg-6">
                                <div class="form-group">
                                    <br/><input type="checkbox" id="BradescoPrincipalBanco"/> 
                                    <label for="BradescoPrincipalBanco"> Bradesco Principal Banco</label> 
                                </div><br/>
                             </div>
                            
                            <div class="col-lg-12" >

                                <div class="form-group">
                                    <label for="Status">Situação</label>
                                    ${status}
                                </div>
                             </div>
                            <div class="col-lg-6" id="group-valor-aplicado">
                                <div class="form-group">
                                    <label for="ValorAplicado">Valor Aplicado</label>
                                    <input type="text" id="ValorAplicado" value="" class="form-control" />
                                </div>
                             </div>
                            <div class="col-lg-6" id="group-valor-nao-aplicado">
                                <div class="form-group">
                                    <label for="Motivo">Motivo</label>
                                    ${motivo}
                                </div>
                             </div> 
                            <div id="group-data-prorrogada">
                                <div class="col-lg-6" >
                                    <div class="form-group">
                                        <label for="DataProrrogada">Data Prorrogada</label>
                                        <input type="date" id="DataProrrogada" value="" class="form-control" />
                                    </div>
                                 </div>
                                 <div class="col-lg-6" >
                                    <div class="form-group">
                                        <label for="DataProrrogada">Hora Prorrogada</label>
                                        <input type="time" id="HoraProrrogada" value="08:00:00" class="form-control" />
                                    </div>
                                 </div>
                            </div>
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="Observacoes">Comentário</label><br/>
                                    <textarea rows="5" class="form-control" id="Observacoes"></textarea>
                                </div>
                             </div>
                            
                         </div>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Cancelar</button>
                        <button type="submit" id="criar-pipeline" class="btn btn-danger">Criar</button>
                      </div>
                    </div>
                    <!-- /.modal-content -->
                  </div>
                  <!-- /.modal-dialog -->
                </div>`
        );


        $('#modal-novo-pipe input#Valor').inputmask("(.999){+|1},99", {
            positionCaretOnClick: "radixFocus",
            radixPoint: ",",
            _radixDance: true,
            numericInput: true,
            placeholder: "0",
            definitions: {
                "0": {
                    validator: "[0-9\uFF11-\uFF19]"
                }
            }
        });

        $('#modal-novo-pipe').modal({ backdrop: 'static' });

        //Mascara dos campos
        $('input#ValorDoPipe, input#ValorAplicado, input#ValoresNoMercado').inputmask("(.999){+|1},99", {
            positionCaretOnClick: "radixFocus",
            radixPoint: ",",
            _radixDance: true,
            numericInput: true,
            placeholder: "0",
            definitions: {
                "0": {
                    validator: "[0-9\uFF11-\uFF19]"
                }
            }
        });

        $('#StatusId').change(function () {
            VerificaSituacao($($(this).find('option:selected').get(0)), false);
        });

        $('#criar-pipeline').click(function () {

            var novo_pipe = {}

            var inputs = $('#modal-novo-pipe input:not(input#Cliente), #modal-novo-pipe select, #modal-novo-pipe textarea')

            inputs.each(function (i, v) {
                novo_pipe[v.id] = v.value
            })

            if (novo_pipe.StatusId == '') {
                core.notify.showNotify("O campo Status é obrigatório.", "info", "top-center");
                return;
            }

            if (novo_pipe.StatusId == '3' && novo_pipe.DataProrrogada == "") {
                core.notify.showNotify("O campo Data Prorogada é obrigatório.", "info", "top-center")
                return;
            }

            if (novo_pipe.StatusId == '1' && novo_pipe.ValorAplicado == "") {
                core.notify.showNotify("O campo Valor Aplicado é obrigatório.", "info", "top-center")
                return;
            }
            if (novo_pipe.StatusId == '2' && novo_pipe.Motivo == "") {
                core.notify.showNotify("O campo Motivo é obrigatório.", "info", "top-center")
                return;
            }
            if (novo_pipe.ValorDoPipe == "" || novo_pipe.ValorDoPipe == "0,00") {
                core.notify.showNotify("O campo Valor é obrigatório.", "info", "top-center")
                return;
            }
            if (novo_pipe.DataPrevista == "") {
                core.notify.showNotify("O campo Data Prevista é obrigatório.", "info", "top-center")
                return;
            }


            novo_pipe.MotivoId = novo_pipe.StatusId == '2' ? novo_pipe.MotivoId : "";
            novo_pipe.ValorDoPipe = `${novo_pipe.ValorDoPipe}`.replace(/\./g, '');
            novo_pipe.ValorAplicado = `${novo_pipe.ValorAplicado}`.replace(/\./g, '');
            novo_pipe.ValoresNoMercado = `${novo_pipe.ValoresNoMercado}`.replace(/\./g, '');
            novo_pipe.BradescoPrincipalBanco = document.getElementById("BradescoPrincipalBanco").checked;
            novo_pipe.MatriculaConsultor = cliente.Consultor.Matricula;

            novo_pipe.DataPrevista = moment(novo_pipe.DataPrevista + " " + novo_pipe.HoraPrevista).format('YYYY-MM-DD HH:mm:ss');

            if (novo_pipe.DataProrrogada != "" && novo_pipe.DataProrrogada != undefined && novo_pipe.DataProrrogada != null) {
                novo_pipe.DataProrrogada = moment(novo_pipe.DataProrrogada + " " + novo_pipe.HoraProrrogada).format('YYYY-MM-DD HH:mm:ss');
            }

            $.post(url, novo_pipe, function (resp) {
                var respjson = JSON.parse(resp);
                if (respjson.length > 0) {
                    core.home.recarregaPipelinesHome(resp, UrlUpdatePipe, urlObterPipe);
                    $('#modal-novo-pipe').modal('hide');
                    core.notify.showNotify("Pipeline criado com sucesso", "success", "top-center");
                }
                else {
                    $('#modal-novo-pipe').modal('hide');
                    core.notify.showNotify("Houve um erro inespeado ao criar este pipeline.", "error", "top-center");
                }
            });
        });

        $('#modal-novo-pipe').on('shown.bs.modal', function (e) {
            VerificaSituacao($('#StatusId option:selected'), true);
        });

        $('#modal-novo-pipe').on('hidden.bs.modal', function (e) {
            $(this).remove();
        });
    }

    function modalAtualizarpipe(data, UrlUpdatePipe, urlObterPipe, urlRedirect) {
        data = JSON.parse(data);
        var valor = data.ValorDoPipe != null ? data.ValorDoPipe.toString() : 0;
        var valorAplicado = data.ValorAplicado != null ? data.ValorAplicado.toString() : 0;
        var valorMercado = data.ValorMercado != null ? data.ValorMercado.toString() : 0;

        if (! /\.\d+$/.test(valor)) {
            valor += ',00'
        } else {
            valor = valor.replace('.', ',')
        }

        if (! /\.\d+$/.test(valorAplicado)) {
            valorAplicado += ',00'
        } else {
            valorAplicado = valorAplicado.replace('.', ',')
        }

        if (! /\.\d+$/.test(valorMercado)) {
            valorMercado += ',00'
        } else {
            valorMercado = valorMercado.replace('.', ',')
        }


        var elem = document.querySelector('#OrigemId').cloneNode(true);

        $(elem).removeClass('none');
        var options = $(elem).children();
        $.each(options, function (i, v) {
            if (v.text == data.Origem) {
                $(v).attr('selected', '');
            }
        })
        var origens = elem.outerHTML;


        var elemMotivo = document.querySelector('#MotivoId').cloneNode(true);

        elem.required = true;
        $(elemMotivo).removeClass('none');
        var options = $(elemMotivo).children();
        $.each(options, function (i, v) {
            if (v.text == data.Motivo) {
                $(v).attr('selected', '');
            }
        })
        var motivo = elemMotivo.outerHTML;


        var elemStatus = document.querySelector('#Status').cloneNode(true);
        elemStatus.setAttribute('id', 'StatusId');
        elemStatus.setAttribute('name', 'StatusId');
        $(elemStatus).removeClass('none');

        var options = $(elemStatus).children();
        $.each(options, function (i, v) {
            if (v.text == data.Situacao) {
                $(v).attr('selected', '');
            }
        })
        var status = elemStatus.outerHTML;

        var checkBradescoPB = data.BradescoPrincipalBanco ? 'checked' : '';

        var cliente = JSON.parse(sessionStorage.getItem('cliente'))

        var dataProrrogada = moment(data.DataProrrogada).format('YYYY-MM-DDTHH:mm:ss');
        var dataPrevista = moment(data.DataPrevista).format('YYYY-MM-DDTHH:mm:ss');

        $('#modais').append(

            `<div class="modal fade " id="modal-up-pipe" style="display: block; padding-right: 17px;">
                  <div class="modal-dialog">
                    <div class="modal-content">
                    <form action="${UrlUpdatePipe}" method="POST" id="update-pipeline">
                        <input type="hidden" value="${data.Id}" name="Id"/>
                        <input type="hidden" value="${urlRedirect}" name="urlRedirect"/>
                      <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                          <span aria-hidden="true">×</span></button>
                        <h4 class="modal-title">Atualizar Pipeline</h4>
                      </div>
                      <div class="modal-body">
                        <div class="row">
                           <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="Especialista">Especialista</label>
                                    <input type="text" readonly name="Consultor" value="${cliente != null ? cliente.Consultor.Nome || "" : data.Especialista || ""}" id="Consultor" class="form-control"/>
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="Cliente">Cliente</label>
                                    <input type="text" readonly name="NomeCliente" value="${cliente != null ? cliente.NomeCliente ||"" : data.Cliente || ""}" id="NomeCliente" class="form-control"/>
                                </div>
                             </div>
                             <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="Agencia">Agencia</label>
                                    <input type="text" readonly id="Agencia" name="Agencia" value="${cliente != null ? cliente.Agencia || "" : data.Agencia || ""}" class="form-control"/>
                                </div>
                             </div>
                             <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="Conta">Conta</label>
                                    <input type="text" readonly id="Conta" name="Conta" value="${cliente != null ? cliente.Conta || "" : data.Conta || ""}" class="form-control"/>
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="Valor">Valor</label>
                                    <input type="text" id="ValorDoPipe" name="ValorDoPipe" value="${valor}" class="form-control"/>
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="Origem">Origem</label>
                                    ${origens}
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="DataPrevista">Data Prevista</label>
                                    <input type="datetime-local" id="DataPrevista" name="DataPrevista" value="${dataPrevista}" class="form-control"/>
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="ValoresNoMercado"> Valores No Mercado</label>
                                    <input type="text" id="ValoresNoMercado" name="ValoresNoMercado" value="${valorMercado}" class="form-control"/> 
                                </div>
                            </div>
                            <div class="col-lg-6" >
                                <div class="form-group">
                                    <label for="Status">Situação</label>
                                    ${status}
                                </div>
                             </div>

                            <div class="col-lg-6" id="group-valor-aplicado">
                                <div class="form-group">
                                    <label for="ValorAplicado">Valor Aplicado</label>
                                    <input type="text" id="ValorAplicado" name="ValorAplicado" value="${valorAplicado}" class="form-control" />
                                </div>
                             </div>
                            
                            <div class="col-lg-6" id="group-valor-nao-aplicado">
                                <div class="form-group">
                                    <label for="Motivo">Motivo</label>
                                    ${motivo}
                                </div>
                             </div> 
                            <div class="col-lg-6" id="group-data-prorrogada">
                                <div class="form-group">
                                    <label for="DataProrrogada">Data Prorrogada</label>
                                    <input type="datetime-local" name="DataProrrogada" id="DataProrrogada" value="${dataProrrogada}" class="form-control" />
                                </div>
                             </div>
                            

                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="Comentario">Comentário</label><br/>
                                    <textarea rows="5" class="form-control" name="Observacoes" id="Observacoes">${data.Comentario != null ? data.Comentario : ''}</textarea>
                                </div>
                             </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <br/><input type="checkbox" ${checkBradescoPB} name="BradescoPrincipalBanco" value="true" id="BradescoPrincipalBanco"/> 
                                    <label for="BradescoPrincipalBanco"> Bradesco Principal Banco</label> <br/>
                                </div>
                             </div>
                         </div>
                      </div>
                        
                      <div class="modal-footer">
                        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Cancelar</button>
                        <button type="submit" class="btn btn-danger">Atualizar</button>
                      </div>
                 </form>
                    </div>
                    <!-- /.modal-content -->
                  </div>
                  <!-- /.modal-dialog -->
                </div>`
        );

        $('#modal-up-pipe').modal({ backdrop: 'static' });

        $('input#ValorDoPipe, input#ValorAplicado, input#ValoresNoMercado').inputmask("(.999){+|1},99", {
            
            radixPoint: ",",
            _radixDance: true,
            numericInput: true,
            placeholder: "0",
            definitions: {
                "0": {
                    validator: "[0-9\uFF11-\uFF19]"
                }
            }
        });

        $('#StatusId').change(function () {
            VerificaSituacao($($(this).find('option:selected').get(0)));
        });


        $('#update-pipeline').submit(function (e) {

            var inputs = $('#modal-up-pipe input, #modal-up-pipe select, #modal-up-pipe textarea');

            var update_pipe = { Id: data.Id };

            inputs.each(function (i, v) {
                update_pipe[v.id] = v.value;
            })

            if (update_pipe.StatusId == '3' && update_pipe.DataProrrogada == "") {
                e.preventDefault();
                core.notify.showNotify("O campo Data Prorogada é obrigatório.", "info", "top-center")
                return;
            }

            if (update_pipe.StatusId == '1' && update_pipe.ValorAplicado == "") {
                e.preventDefault();
                core.notify.showNotify("O campo Valor Aplicado é obrigatório.", "info", "top-center")
                return;
            }
            if (update_pipe.StatusId == '2' && update_pipe.Motivo == "") {
                e.preventDefault();
                core.notify.showNotify("O campo Motivo é obrigatório.", "info", "top-center")
                return;
            }
            if (update_pipe.ValorDoPipe == "" || update_pipe.ValorDoPipe == "0,00") {
                e.preventDefault();
                core.notify.showNotify("O campo Valor é obrigatório.", "info", "top-center")
                return;
            }
            if (update_pipe.DataPrevista == "") {
                e.preventDefault();
                core.notify.showNotify("O campo Data Prevista é obrigatório.", "info", "top-center")
                return;
            }
            

            update_pipe.MotivoId = update_pipe.StatusId == '2' ? update_pipe.MotivoId : "";
            update_pipe.ValorDoPipe = update_pipe.ValorDoPipe.replace(/\./g, '');
            update_pipe.ValorAplicado = update_pipe.ValorAplicado != null ? update_pipe.ValorAplicado.toString().replace(/\./g, '') : null;
            update_pipe.BradescoPrincipalBanco = document.querySelector('#BradescoPrincipalBanco').checked;
            update_pipe.ValoresNoMercado = update_pipe.ValoresNoMercado != null ? update_pipe.ValoresNoMercado.toString().replace(/\./g, '') : null;

            var valorPipeFloat = parseFloat(update_pipe.ValorDoPipe.replace(',', '.'));

            var valorAplicadoFloat = parseFloat(update_pipe.ValorAplicado.replace(',', '.'));

            if (valorAplicadoFloat > valorPipeFloat) {
                e.preventDefault();
                core.notify.showNotify("O valor aplicado não pode ser maior que o valor do pipeline", 'error', 'top-center');
                return;
            }


            if (update_pipe.DataDaConversao != null) {
                update_pipe.DataDaConversao = moment().format('YYYY-MM-DD HH:mm:ss');
            }

            if (update_pipe.ValorAplicado != '') {
                $('#ValorAplicado').inputmask('remove').val(update_pipe.ValorAplicado);
            }
            if (update_pipe.ValorDoPipe != '') {
                $('#ValorDoPipe').inputmask('remove').val(update_pipe.ValorDoPipe);
            }
            if (update_pipe.ValoresNoMercado != '') {
                $('#ValoresNoMercado').inputmask('remove').val(update_pipe.ValoresNoMercado);
            }

            $('#MotivoId').val(update_pipe.MotivoId);
            $('#BradescoPrincipalBanco').val(update_pipe.BradescoPrincipalBanco);


            //$.ajax({
            //    url: UrlUpdatePipe,
            //    data: update_pipe,
            //    type: 'POST',
            //    success: function (resp) {
            //        if (resp != null) {
            //            core.home.recarregaPipelinesHome(resp, UrlUpdatePipe, urlObterPipe);
            //            $('#modal-up-pipe').modal('hide');
            //            core.notify.showNotify("Pipeline atualizado com sucesso.", 'success', 'top-center');
            //        } else {
            //            core.notify.showNotify("Erro ao atualizar pipeline.", 'warning', 'top-center');
            //        }
            //    },
            //    error: function (e) {
            //        console.log(e);
            //    }
            //})

        })

        $('#modal-up-pipe').on('shown.bs.modal', function (e) {
            VerificaSituacao($('#StatusId option:selected'));
        })

        $('#modal-up-pipe').on('hidden.bs.modal', function (e) {
            $(this).remove();
        })
    }

    function modalAtualizarTED(ted, url, urlAplicacao, urlExcluirAplicacao, urlRedirect) {

        var totalValorTedAplicado = 0;

        var data = JSON.parse(ted);

        var situacoes = '<option disabled selected>Selecione um Status</option>';

        var motivos = '<option disabled selected>Selecione um Motivo</option>';

        var outrasInst = '';

        var produtos = '';

        var aplicacoes = '';

        for (var i = 0; i < data.Situacoes.length; i++) {

            var selected = data.Situacoes[i].Id == data.StatusId ? "selected" : "";

            situacoes += `<option ${selected} value="${data.Situacoes[i].Id}">${data.Situacoes[i].Descricao}</option>`
        }

        for (var i = 0; i < data.TedsMotivos.length; i++) {

            var selected = data.TedsMotivos[i].Id == data.MotivoId ? "selected" : "";

            motivos += `<option ${selected} value="${data.TedsMotivos[i].Id}">${data.TedsMotivos[i].Descricao}</option>`
        }

        for (var i = 0; i < data.TedsMotivosOutrasInst.length; i++) {
            var selected = data.TedsMotivosOutrasInst[i].Id == data.OutrasInstId ? "selected" : "";

            outrasInst += `<option ${selected} value="${data.TedsMotivosOutrasInst[i].Id}">${data.TedsMotivosOutrasInst[i].Descricao}</option>`
        }

        for (var i = 0; i < data.Produtos.length; i++) {
            produtos += `<option value="${data.Produtos[i].Id}">${data.Produtos[i].Descricao}</option>`
        }


        for (var i = 0; i < data.Aplicacoes.length; i++) {
            aplicacoes += `<tr id="${data.Aplicacoes[i].IdAplicacao}">
                            <td class="border-right">${data.Aplicacoes[i].Produto}</td >
                            <td class="border-right">${data.Aplicacoes[i].Valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                            <td class="text-center"><a href="#" class="btn btn-danger excluir-aplicacao"><i class="fa fa-trash-o"></i></a></td>
				        </tr >`;
        }

        var produtosAplicados = [];

        if (data.Aplicacoes.length > 0) {
            $.each(data.Aplicacoes, function (e, ele) {
                totalValorTedAplicado += ele.Valor;
            });
            //totalValorTedAplicado = data.Aplicacoes.reduce((f, s) => (f.Valor != undefined ? f.Valor : f ) + s.Valor);

            produtosAplicados = data.Aplicacoes.map(a => a.ProdutoId);
        }

        //Contatos

        var modalAtualizarTED = $('<div>', { class: "modal fade", style: "display: block; padding-right: 17px;" }).append(
            `<div class="modal-dialog modal-lg" style="width:90%">
                    <div class="modal-content">
                     <form method="post" action="${url}" id="form-ted">
                     <input type="hidden" value="${urlRedirect}" name="RedirectUrl"/>
                      <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                          <span aria-hidden="true">×</span></button>
                        <h4 class="modal-title">Atualizar TED</h4>
                      </div>
                      <div class="modal-body">
                        <input name="Id" value="${data.Id}" type="hidden" />
                        <div class="row relative">
		                    <div class="col-lg-3">
			                    <label>Data</label><br>
			                    <p>${moment(data.Data).format('DD/MM/YYYY')}</p>
		                    </div>
		                    <div class="col-lg-3">
			                    <label>Agencia</label><br>
			                    <p>${data.Agencia}</p>	
		                    </div>
		                    <div class="col-lg-3">
			                    <label>Conta</label><br>
			                    <p>${data.Conta}</p>	
		                    </div>
		                    <div class="col-lg-3">
			                    <label>Valor</label><br>
			                    <p>${data.Valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</label>	
		                    </div>
		                    <div class="col-lg-5">
			                    <label>Especialista</label><br>
                                <p>${data.NomeConsultor}</p>
		                    </div>
		                    <div class="col-lg-5">
			                    <label>Status</label><br>
			                    <select name="StatusId" required id="statusTED" style="width:60%" onchange="core.modal.statusChange(this)" class="form-control">
                                     ${situacoes}
                                </select>
		                    </div>
	                    </div>
	                    <hr>  
                        <div class="row">
		                    <div class="col-lg-4">
			                    <table class="table table-striped ">
				                    <thead>
					                    <tr>
						                    <th>Contatos</th>
						                    <th class="tbl-alg-center">Sim</th>
						                    <th class="tbl-alg-center">Não</th>
					                    </tr>
				                    </thead>
				                    <tbody>
					                    <tr>
						                    <td class="alg-left">Contatou Cliente</td>
						                    <td class="tbl-alg-center"><input type="radio" ${data.ContatouCliente || data.ContatouCliente == null ? "checked" : ""} id = "contatou-cliente-sim" name = "ContatouCliente" value = "true" ></td >
						                    <td class="tbl-alg-center"><input type="radio" ${data.ContatouCliente != null && !data.ContatouCliente ? "checked" : ""} id="contatou-cliente-nao" name="ContatouCliente" value="false"></td>
					                    </tr>
					                    <tr>
						                    <td class="alg-left">Contatou Gerente</td>
						                    <td class="tbl-alg-center"><input type="radio" ${data.ContatouGerente || data.ContatouGerente == null ? "checked" : ""} id="contatou-gerente-sim" name="ContatouGerente" value="true"></td>
						                    <td class="tbl-alg-center"><input type="radio" ${data.ContatouGerente != null && !data.ContatouGerente ? "checked" : ""} id="contatou-gerente-nao" name="ContatouGerente"  value="false"></td>
					                    </tr>
					                    <tr>
						                    <td class="alg-left">Gerente Solicitou Não Atuação</td>
						                    <td class="tbl-alg-center"><input type="radio" ${data.GerenteSolicitouNaoAtuacao || data.GerenteSolicitouNaoAtuacao == null ? "checked" : ""} id="gerente-solicitou-sim" name="GerenteSolicitouNaoAtuacao" value="true"></td>
						                    <td class="tbl-alg-center"><input type="radio" ${data.GerenteSolicitouNaoAtuacao != null && !data.GerenteSolicitouNaoAtuacao ? "checked" : ""} id="gerente-solicitou-nao" name="GerenteSolicitouNaoAtuacao" value="false"></td>
					                    </tr> 
					                    <tr>
						                    <td class="alg-left">Especialista Atuou</td>
						                    <td class="tbl-alg-center"><input type="radio" ${data.EspecialistaAtuou == null || data.EspecialistaAtuou ? "checked" : ""} id="especialista-atuou-sim" name="EspecialistaAtuou" value="true">
						                    </td>
						                    <td class="tbl-alg-center"><input type="radio" ${data.EspecialistaAtuou != null && !data.EspecialistaAtuou ? "checked" : ""} id="especialista-atuou-nao" name="EspecialistaAtuou"  value="false">
						                    </td>
					                    </tr>
					                    
					                    <tr>
						                    <td class="alg-left">Cliente Localizado</td>
						                    <td class="tbl-alg-center"><input type="radio" ${data.ClienteLocalizado == null || data.ClienteLocalizado ? "checked" : ""} id="cliente-localizado-sim" name="ClienteLocalizado" value="true">
						                    </td>
						                    <td class="tbl-alg-center"><input type="radio" ${data.ClienteLocalizado != null && !data.ClienteLocalizado ? "checked" : ""} id="cliente-localizado-nao" name="ClienteLocalizado" value="false">
						                    </td>
					                    </tr>
					                    <tr>
						                    <td class="alg-left">Cliente Aceita Consultoria</td>
						                    <td class="tbl-alg-center"><input type="radio" ${data.ClienteAceitaConsultoria == null || data.ClienteAceitaConsultoria ? "checked" : ""} id="cliente-aceita-sim" name="ClienteAceitaConsultoria" value="true"></td>
                                            <td class="tbl-alg-center"><input type="radio" ${data.ClienteAceitaConsultoria != null && !data.ClienteAceitaConsultoria ? "checked" : ""} id="cliente-aceita-nao" name="ClienteAceitaConsultoria" value="false"></td>
					                    </tr>
				                    </tbody>
			                    </table>
		                    </div>
		                    <div class="col-lg-5">
				                <label class="d-block" style="display:block">Total Aplicado: <span id="valor-total-aplicado">${totalValorTedAplicado.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</span></label>
				                <select class="form-control input-total" style="width:50%; display:inline" id="produto">
				                <option value="" disabled selected>Selecione o Produto</option>
					                ${produtos}
				                </select>
				                <input type="text" style="width:30%; display:inline" name="valor-total" id="valor-aplicado" class="form-control input-total numeric">
				                <button type="button" id="adicionar-produto" class="btn btn-success btn-add" style="display:inline">Adcionar</button>
			                    <div class="scroll-aplicacao">
                                    <table class="table table-striped" id="tabela-produtos" style="margin-top:5px">
				                        <thead>
					                        <tr class="tr-total">
						                        <th class="th-produto">Produto</th>
						                        <th class="th-produto">Valor</th>
                                                <th>Excluir</th>
					                        </tr>
				                        </thead>
				                        <tbody>
					                        ${aplicacoes}
				                        </tbody>
			                        </table>
                                </div>
		                    </div>
		                    <div class="col-lg-3">
			                    <label>Não Aplicado: <span id="valor-total-nao-aplicado">${(data.Valor - totalValorTedAplicado).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</span></label><br>

			                    <label>Motivo Não Aplicação</label>
			                    <select id="motivos-teds" required name="MotivoTedId" class="form-control mg-bottom">
				                    ${motivos}
			                    </select>   
                                <div id="motivo-outra-inst" class="none">
			                        <label>Motivo Outras Instiuições</label>
			                        <select class="form-control mg-bottom" name="OutrasInstId">
                                        <option value="" selected>Selecione o submotivo</option>
				                        ${outrasInst}
			                        </select>
                                </div>
			                    <div> 
				                    <label>Observações</label>
				                    <textarea class="form-control height-15" rows="8" name="Observacao">${data.Observacao != "null" && data.Observacao != null ? data.Observacao : ""}</textarea>
			                    </div>
		                    </div>
	                    </div>
                    </div>
                    <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                    <button type="submit" id="update-ted" class="btn btn-danger">Atualizar</button>
                    </div>
                </form>
            </div><!-- /.modal-content -->
        </div>`);

        //<tr>
        //    <td class="alg-left">Gerente de Investimentos Atuou</td>
        //    <td class="tbl-alg-center"><input type="radio" ${data.GerenteInvestimentoAtuou == null || data.GerenteInvestimentoAtuou ? "checked" : ""} id="gerente-atuou-sim" name="GerenteInvestimentoAtuou" value="true"></td>
        //    <td class="tbl-alg-center"><input type="radio" ${data.GerenteInvestimentoAtuou != null && !data.GerenteInvestimentoAtuou ? "checked" : ""} id="gerente-atuou-nao" name="GerenteInvestimentoAtuou" value="false"></td>
        //</tr>

        modalAtualizarTED.appendTo("#modais");

        modalAtualizarTED.modal({ backdrop: 'static' });

        modalAtualizarTED.on("hidden.bs.modal", function () {
            $(this).data('bs.modal', null);

            modalAtualizarTED.remove();
        });

        $('#produto option').each(function (ind, ele) {
            var valueOption = parseFloat($(ele).attr('value'));

            if (!isNaN(valueOption) && produtosAplicados.includes(valueOption)) {
                $(this).attr('disabled', true);
            }
        });


        $('#adicionar-produto').click(function () {

            var IdProduto = $('#produto').val();

            var ValorAplicado = $('#valor-aplicado').val().replace(/\./g, '');

            var valorAplicado = parseFloat(ValorAplicado.replace(',', '.'));

            var valorTed = data.Valor;

            if (isNaN(valorAplicado) || valorAplicado == 0) {
                core.notify.showNotify('O valor aplicado não pode ser 0 ou estar vazio', 'info', 'top-center');
                return;
            }

            if ((totalValorTedAplicado + valorAplicado) > valorTed) {
                core.notify.showNotify('O valor desta aplicação ultrapassou o valor total da ted', 'info', 'top-center');
                return;
            }

            $.post(urlAplicacao, { IdTed: data.Id, IdProduto, ValorAplicado }, function (resp) {
                resp = JSON.parse(resp);
                if (resp.status) {
                    totalValorTedAplicado += valorAplicado;
                    $('#tabela-produtos > tbody').append(
                        `<tr id="${resp.Id}">
                            <td class="border-right">${data.Produtos.filter(p => p.Id == IdProduto)[0].Descricao}</td >
                            <td class="border-right">${valorAplicado.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</td>
                            <td class="text-center"><a href="#" class="btn btn-danger excluir-aplicacao"><i class="fa fa-trash-o"></i></a></td>
				        </tr >`);

                    core.notify.showNotify(resp.mensagem, 'success', 'top-center');

                    $('#produto option')[0].selected = true;

                    $('#produto option').each(function (ind, ele) {
                        var optionValor = ele.value;
                        if (optionValor == IdProduto) {
                            $(this).attr('disabled', true);
                        }

                    });

                    $('#valor-aplicado').val('');

                    //AtualizaValores
                    $('#valor-total-aplicado').text(totalValorTedAplicado.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }));
                    $('#valor-total-nao-aplicado').text((data.Valor - totalValorTedAplicado).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }));
                }
                else {
                    core.notify.showNotify(resp.mensagem, 'error', 'top-center');
                }
            });


        });

        $(document).on('change', '#motivos-teds', function () {
            var motivo = $(this).val();

            if (motivo == "8") {
                $('#motivo-outra-inst').removeClass('none');
            } else {
                $('#motivo-outra-inst').addClass('none');
            }
        });

        $(document).on('click', '.excluir-aplicacao', function () {
            var linha = $(this).closest('tr');
            var id = linha.attr('id');
            var produtoExcluido = linha.children().first().text();

            $.get(urlExcluirAplicacao, { id }, function (resp) {
                resp = JSON.parse(resp);

                if (resp.status) {
                    core.notify.showNotify(resp.mensagem, 'success', 'top-center');
                    totalValorTedAplicado -= resp.valorExcluido;

                    linha.remove();

                    $('#produto option').each(function (ind, ele) {
                        var nomeProduto = $(ele).text();

                        if (nomeProduto == produtoExcluido) {
                            $(this).removeAttr('disabled');
                        }
                    });

                    //AtualizaValores
                    $('#valor-total-aplicado').text(totalValorTedAplicado.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }));
                    $('#valor-total-nao-aplicado').text((data.Valor - totalValorTedAplicado).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }));
                }
                else {
                    core.notify.showNotify(resp.mensagem, 'error', 'top-center');
                }
            });
        });

        modalAtualizarTED.on("shown.bs.modal", function () {
            //Mascara dos campos
            $('.numeric').inputmask("(.999){+|1},99", {
                //positionCaretOnClick: "radixFocus",
                radixPoint: ",",
                _radixDance: true,
                numericInput: true,
                placeholder: "0",
                noshift: true,
                definitions: {
                    "0": {
                        validator: "[0-9\uFF11-\uFF19]"
                    }
                }
            });

            $('#motivos-teds').trigger('change');

            $('.scroll-aplicacao').slimScroll({ height: '250px' });
        });

        $("#update-ted").click(function () {

            String.prototype.splice = function (idx, rem, str) {
                return this.slice(0, idx) + str + this.slice(idx + Math.abs(rem));
            };
        });

        $(modalAtualizarTED.find('form').get(0)).submit(function (e) {

            String.prototype.splice = function (idx, rem, str) {
                return this.slice(0, idx) + str + this.slice(idx + Math.abs(rem));
            };

            var status = $(this).find('#statusTED').val();
            var motivo = $(this).find('#motivos-teds').val();

            if (status == "" || status == null) {
                e.preventDefault();
                core.notify.showNotify('O campo Status é obrigatório', 'info', 'top-center');
                return;
            }

            if (totalValorTedAplicado == 0 && status == "16" && (motivo == "" || motivo == null)) {
                e.preventDefault();
                core.notify.showNotify('O campo Motivo é obrigatório', 'info', 'top-center');
                return;
            }

            var totalChecks = $('input[type="radio"]').length / 2;

            var inputsChecados = $('input:checked').length;

            if (totalChecks > inputsChecados) {
                e.preventDefault();
                core.notify.showNotify('É obrigatório responder todos os items da sessão contatos', 'info', 'top-center   ');
                return;
            }
        });
    }

    function statusChange(el) {

        $("#ValorAplicado-container").hide();

        $("#motivoTED-container").hide();


        var option = $($(el).find("option:selected").get(0));

        if (option.text().toLowerCase() == "aplicado") {

            $("#ValorAplicado-container").show();

        } else if (option.text().toLowerCase() == "não aplicado") {

            $("#motivoTED-container").show();
        }
    }

    function tratarDados(form) {

        String.prototype.splice = function (idx, rem, str) {
            return this.slice(0, idx) + str + this.slice(idx + Math.abs(rem));
        };

        var inputValorAplicado = $(form).find('input[name="ValorAplicado"]')[0];

        if ($(inputValorAplicado).is(":visible")) {

            var valorAplicado = $(inputValorAplicado).val() ? $(inputValorAplicado).val().replace(/[^0-9]/g, '') : '';

            if (valorAplicado != '') {
                $(inputValorAplicado).val(valorAplicado.splice(valorAplicado.length - 2, 0, ','));
            }
        }
        else {

            $(inputValorAplicado).removeAttr('required');
        }
    }

    function modalCockpit(agencia, conta, cpf) {
        var modalCockpit = `
            <div class="modal fade mymodal" id="modal-cockpit" role="dialog">
                <div class="modal-dialog modal-lg">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"> <i class='fa fa-times'></i> </button>
                            <button class="close modalMinimize-cockpit"> <i class='fa fa-minus toggle-icon'></i> </button>
                            <h4 class="modal-title">Cockpit</h4>
                        </div>
                        <div class="modal-body">
                            <iframe src="#" frameborder="0" scrolling="auto" seamless="seamless" height="500" width="1000" id="frame-cockpit"></iframe>
                        </div>
                    </div>
                </div>
            </div>`;

        $('#box-modais-open > .box-body').html('');
        $(modalCockpit).appendTo('#modais');
        $(modalCockpit).appendTo('#modais .modal-cockpit');


        $("#frame-cockpit").attr("src", "http://10.244.168.119/agenciadigital/FicHaDeCliente/FichaDeClienteDetalhado?cpf=" + cpf + "&agencia=" + agencia + "&conta=" + conta)

        $("#modal-cockpit").modal({ backdrop: false, keyboard: false });

        var $content, $modal, $apnData, $modalCon; $content = $(".min");
        $(".modalMinimize-cockpit").on("click", function () {
            $modalCon = $(this).closest(".mymodal").attr("id");
            $apnData = $(this).closest(".mymodal");
            $modal = "#" + $modalCon;
            $(".modal-backdrop").addClass("display-none");
            $($modal).toggleClass("min");
            if ($($modal).hasClass("min")) {
                if ($('#cockpit-container').length == 0) {
                    $('#box-modais-open > .box-body').append('<div class="minmaxCon" id="cockpit-container"></div>');
                }

                $('#modal-cockpit').modal('hide');
                //$("#cockpit-container").append($apnData);
                $(this).find("i").toggleClass('fa-minus').toggleClass('fa-expand');
                $('.min .modal-header').addClass('event-style bg-red');
                $('#box-modais-open').show();
                $('#frame-cockpit').hide();
            }
            else {
                //$("#modais").append($apnData);
                $('#modal-cockpit').modal('show');
                $(this).find("i").toggleClass('fa-expand').toggleClass('fa-minus');
                $('.modal-header').removeClass('event-style bg-red');
                $('#frame-cockpit').show();
            };
        });

        $("button[data-dismiss='modal']").click(function () {
            $(this).closest(".mymodal").removeClass("min");
            $(".container").removeClass($apnData);
            $(this).next('.modalMinimize').find("i").removeClass('fa fa-expand').addClass('fa fa-minus');
        });


        $('#modal-cockpit').on('shown.bs.modal', function (e) {
            $modalCon = $(this).closest(".mymodal").attr("id");
            $apnData = $(this).closest(".mymodal");
            $modal = "#" + $modalCon;
            $(".modal-backdrop").addClass("display-none");
            $($modal).toggleClass("min");
            if ($($modal).hasClass("min")) {
                if ($('#cockpit-container').length == 0) {
                    $('#box-modais-open > .box-body').append('<div class="minmaxCon" id="cockpit-container"></div>')
                }
                $('#modal-cockpit .modal-header > button> i.toggle-icon').toggleClass('fa-minus').toggleClass('fa-expand');
                $("#cockpit-container").append($apnData);
                $('.min .modal-header').addClass('event-style bg-red')
                $('#box-modais-open').show();
                $('#frame-cockpit').hide()
            }
        })

        $('#modal-cockpit').on('hidden.bs.modal', function (e) {
            $(this).remove()
            $('#cockpit-container').remove()

            if ($('#box-modais-open .box-body').length == 0) {
                $('#box-modais-open').hide();
            }
        });
    }

    function modalSinv(agencia, conta, cpf, url) {
        var modalsinv = `
            <div class="modal fade mymodal" id="modal-sinv" role="dialog">
                <div class="modal-dialog modal-lg">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"> <i class='fa fa-times'></i> </button>
                            <h4 class="modal-title">SINV</h4>
                        </div>

                        <div class="modal-body">
                            <div id="popUp-SINV">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
        $('#box-modais-open > .box-body').html('');
        $('#modais').append(modalsinv);

        var postSinvUrl = url;

        $("#popUp-SINV").html("");

        $('<iframe name="target-iframe"/>').appendTo('#popUp-SINV')
            .attr({ 'height': '800', 'width': '1000', 'frameborder': '0' })
            .attr({ 'id': 'target-iframe' });

        $("#popUp-SINV").append("<form id='form-target-iframe' action='" + postSinvUrl + "' target='target-iframe' method='post'> <input type='text' name='txtAgencia' value='" + agencia + "' /> <input type='text' name='txtContaCorrente' value='" + conta + "' /> <input type='text' name='txtCNPJCPF' value='" + cpf + "' /> <input type='text' name='txtReferencia' value='1' /></form>");

        $("#target-iframe").attr("src", postSinvUrl);

        $("#form-target-iframe").submit();

        $("#form-target-iframe").remove();

        $("#modal-sinv").modal({ backdrop: false, keyboard: false });

        $('#modal-sinv').on('hidden.bs.modal', function (e) {
            $(this).remove();
        });
    }

    function modalSinvTransacional(agencia, conta, url) {
        var modalsinvTransacoinal = `
            <div class="modal fade mymodal" id="modal-sinv-transacional" role="dialog">
                <div class="modal-dialog modal-lg">
                    <!-- Modal content-->
                    <div class="modal-content" style="width:1100px">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"> <i class='fa fa-times'></i> </button>
                            <h4 class="modal-title">SINV - Transacional</h4>
                        </div>
                        <div class="modal-body">
                            <div id="popUp-SINV-Transacional">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
        $('#box-modais-open > .box-body').html('');
        $('#modais').append(modalsinvTransacoinal);

        var postSinvUrl = url;

        $("#popUp-SINV-Transacional").html("");

        $('<iframe name="target-iframe-sinv-transacional" src="https://intranet8.net.bradesco.com.br/cinv/servlet/ServletControladoraSubMenu?MenuOpcao=2" />').appendTo('#popUp-SINV-Transacional')
            .attr({ 'height': '800', 'width': '1100', 'frameborder': '0' })
            .attr({ 'id': 'target-iframe-sinv-transacional' });

        $("#popUp-SINV-Transacional").append("<form id='form-target-iframe' action='" + postSinvUrl + "' target='target-iframe-sinv-transacional' method='post'> <input type='text' name='agencia' value='" + agencia + "' /> <input type='text' name='conta' value='" + conta + "' /> <input type='text' name='isCheckedAll' value='on' /></form>");

        $("#modal-sinv-transacional").modal({ backdrop: false, keyboard: false });

        setTimeout(function () {
            $("#target-iframe-sinv-transacional").attr("src", postSinvUrl);

            $("#form-target-iframe").submit();

            $("#form-target-iframe").remove();

        }, 1000);


        $('#modal-sinv-transacional').on('hidden.bs.modal', function (e) {
            $(this).remove();
        });
    }

    function modalSinvEnviarTermos(url) {
        //<button class="close modalMinimize-sinv"> <i class='fa fa-minus toggle-icon'></i> </button>
        var modalsinv = `
            <div class="modal fade mymodal" id="modal-sinv" role="dialog">
                <div class="modal-dialog modal-lg">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"> <i class='fa fa-times'></i> </button>
                            <h4 class="modal-title">SINV - Enviar Termos</h4>
                        </div>

                        <div class="modal-body">
                            <div id="popUp-SINV">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
        $('#box-modais-open > .box-body').html('');
        $('#modais').append(modalsinv);

        $("#popUp-SINV").html("");

        $('<iframe name="target-iframe"/>').appendTo('#popUp-SINV')
            .attr({ 'height': '800', 'width': '1000', 'frameborder': '0' })
            .attr({ 'id': 'target-iframe' });

        $("#target-iframe").attr("src", url);

        $("#modal-sinv").modal({ backdrop: false, keyboard: false });

        $('#modal-sinv').on('hidden.bs.modal', function (e) {
            $(this).remove();
        });
    }

    function modalCockpitColmeia(url) {

        var modalCockpit = `
            <div class="modal fade mymodal" id="modal-cockpit-colmeia" role="dialog">
                <div class="modal-dialog modal-lg">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"> <i class="fa fa-times"></i> </button>
                            <button class="close modalMinimize-cockpit-colmeia"> <i class="fa fa-minus toggle-icon"></i> </button>
                            <h4 class="modal-title">Colmeia - Cockpit</h4>
                        </div>
                        <div class="modal-body">
                            <iframe src="http://10.244.168.119/agenciadigital/Home/Index" frameborder="0" scrolling="auto" seamless="seamless" height="500" width="1000" id="frame-cockpit"></iframe>
                        </div>
                    </div>
                </div>
            </div>`;

        $('#modais').append('<div class="modal-cockpit-colmeia-container"></div>')
        $(modalCockpit).appendTo('#modais .modal-cockpit-colmeia-container')

        $("#modal-cockpit-colmeia").modal({ backdrop: false, keyboard: false });
        var $content, $modal, $apnData, $modalCon; $content = $(".min");

        $(".modalMinimize-cockpit-colmeia").on("click", function () {
            $modalCon = $(this).closest(".mymodal").attr("id");
            $apnData = $(this).closest(".mymodal");
            $modal = "#" + $modalCon;
            $(".modal-backdrop").addClass("display-none");
            $($modal).toggleClass("min");
            if ($($modal).hasClass("min")) {
                if ($('#colmeia-container').length == 0) {
                    $('#box-modais-open > .box-body').append('<div class="minmaxCon" id="colmeia-container"></div>')
                }
                $("#colmeia-container").append($apnData);
                $(this).find("i").toggleClass('fa-minus').toggleClass('fa-expand');
                $('.min .modal-header').addClass('event-style bg-red')
                $('#box-modais-open').show();
                $('#frame-cockpit').hide()
            }
            else {
                $("#modais .modal-cockpit-colmeia-container").append($apnData);
                $(this).find("i").toggleClass('fa-expand').toggleClass('fa-minus');
                $('.modal-header').removeClass('event-style bg-red')
                $('#frame-cockpit').show()
            };
        });

        $("button[data-dismiss='modal']").click(function () {
            $(this).closest(".mymodal").removeClass("min");
            $(".container").removeClass($apnData);
            $(this).next('.modalMinimize').find("i").removeClass('fa fa-expand').addClass('fa fa-minus');
        });

        $('#modal-cockpit-colmeia').on('hidden.bs.modal', function (e) {
            $(this).remove()
            $('#colmeia-container').remove()

            if ($('#box-modais-open .box-body').length == 0) {
                $('#box-modais-open').hide();
            }
        })
    }

    function VerificaSituacao(elem) {
        var valorSelecionado = `${elem.text()}`.toLowerCase();

        var elemHide = $('#group-valor-aplicado');
        var elemHideNaoAplicado = $('#group-valor-nao-aplicado');
        var elemHideDataProrrogada = $('#group-data-prorrogada');

        if (valorSelecionado == 'aplicado') {
            elemHide.show();
            elemHideNaoAplicado.hide();
            elemHideDataProrrogada.hide();
            $('#DataProrrogada').val('');
            $('#Motivo').val('');

        }
        else if (valorSelecionado == 'não aplicado') {
            elemHide.hide();
            elemHideNaoAplicado.show();
            elemHideDataProrrogada.hide();
            $('#ValorAplicado').val('');
            $('#DataProrrogada').val('');

        } else if (valorSelecionado == 'prorrogado') {
            elemHide.hide();
            elemHideNaoAplicado.hide();
            elemHideDataProrrogada.show();
            $('#ValorAplicado').val('');
            $('#Motivo').val('');

        } else {
            elemHide.hide();
            elemHideNaoAplicado.hide();
            elemHideDataProrrogada.hide();
            $('#ValorAplicado').val('');
            $('#Motivo').val('');
            $('#DataProrrogada').val('');

        }
    }

    function carregarSINV(agencia, conta, cpfCNPJ, url) {

        var cliente = cpfCNPJ.replace(/\D/g, '').length == 11 ? "F" : "J";
        var referencia = cpfCNPJ.replace(/\D/g, '').length == 0 ? "1" : "2";

        if (sinv != null)
            sinv.close();

        //Popups SINV COCKPIT
        var features = 'width=670, height=670, directories=no,location=no,menubar=no,scrollbars=yes,status=no,toolbar=no,resizable=yes';
        var aleatorio = Math.random();
        var sinv = window.open('', 'janela-sinv', features);

        var form = sinv.document.createElement('form');
        form.setAttribute('id', 'form-sinv');
        form.setAttribute('action', url);
        form.setAttribute('method', 'post');

        var txtAgencia = sinv.document.createElement('input');
        txtAgencia.setAttribute('name', 'txtAgencia');
        txtAgencia.setAttribute('type', 'text');
        txtAgencia.setAttribute('value', agencia);
        form.appendChild(txtAgencia);

        var txtConta = sinv.document.createElement('input');
        txtConta.setAttribute('name', 'txtContaCorrente');
        txtConta.setAttribute('type', 'text');
        txtConta.setAttribute('value', conta);
        form.appendChild(txtConta);


        var txtCPF = sinv.document.createElement('input');
        txtCPF.setAttribute('name', 'txtCGCCPF');
        txtCPF.setAttribute('type', 'hidden');
        txtCPF.setAttribute('value', cpfCNPJ.replace(/\D/g, ''));
        form.appendChild(txtCPF);


        var txtReferencia = sinv.document.createElement('input');
        txtReferencia.setAttribute('name', 'txtReferencia');
        txtReferencia.setAttribute('type', 'text');
        txtReferencia.setAttribute('value', referencia);
        form.appendChild(txtReferencia);


        var txtCliente = sinv.document.createElement('input');
        txtCliente.setAttribute('name', 'txtCliente');
        txtCliente.setAttribute('type', 'text');
        txtCliente.setAttribute('value', cliente);
        form.appendChild(txtCliente);

        sinv.document.body.appendChild(form);
        sinv.document.forms[0].style.display = 'none';
        sinv.document.forms[0].submit();


        //$("#popUp-SINV").html("")
        //$('<iframe name="target-iframe"/>').appendTo('#popUp-SINV').attr({ 'height': '600', 'width': '780', 'frameborder': '0' }).attr({ 'id': 'target-iframe' });


        //var form = "<form id='form-target-iframe' action='" + postSinvUrl + "' target='target-iframe' method='post'> <input type='text' name='txtAgencia' value='" + agencia + "' /> <input type='text' name='txtContaCorrente' value='" + conta + "' /> <input type='hidden' name='txtCGCCPF' value='" + cpfCNPJ.replace(/\D/g, '') + "' /> <input type='text' name='txtReferencia' value='" + referencia + "' /><input type='text' name='txtCliente' value='" + cliente + "'/></form>";

        //$("#popUp-SINV").append(form);

        //$("#target-iframe").attr("src", postSinvUrl);

        //$("#form-target-iframe").submit();

        //$("#form-target-iframe").remove();
    }

    function modalContatoAplicResg(aplicResg, urlAplic, filtros) {

        var data = JSON.parse(aplicResg);

        data = data.aplic;
        //Contatos

        var modalContatoAplicResg = $('<div>', { class: "modal fade", style: "display: block; padding-right: 17px;" }).append(
            `<div class="modal-dialog modal-lg" role="document">
                <div class="modal-content scroll-modal" style="border-radius:10px">
              <form action="${urlAplic}" method="post">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                    <h3 class="modal-title">Aplicação/Resgate Contato</h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                               <input type="hidden" value="${filtros.Agencia}" name="Agencia" />
                               <input type="hidden" value="${filtros.Conta}" name="Conta" />
                               <input type="hidden" value="${filtros.Especialista}" name="Especialista" />
                               <input type="hidden" value="${filtros.De}" name="De" />
                               <input type="hidden" value="${filtros.Ate}" name="Ate" />
                               <input type="hidden" value="${data.Id}" name="IdAplicResgate" />
                                <table class="table table-striped">
                                    <thead>
                                        <tr style="font-size:18px">
                                            <th>Contatos</th>
                                            <th class="tbl-alg-center">Sim</th>
                                            <th class="tbl-alg-center">Não</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr style="font-size:17px">
                                            <td class="alg-left">Contatou Cliente</td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.ContatouCliente || data.ContatouCliente == null ? "checked" : ""} id="contatou-cliente-sim" name="ContatouCliente" value="true"></td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.ContatouCliente != null && !data.ContatouCliente ? "checked" : ""} id="contatou-cliente-nao" name="ContatouCliente" value="false"></td>
                                        </tr>
                                        <tr style="font-size:17px">
                                            <td class="alg-left">Realocou</td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.Realocou || data.Realocou == null ? "checked" : ""} id="realocou-sim" name="Realocou" value="true"></td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.Realocou != null && !data.Realocou ? "checked" : ""} id="realocou-nao" name="Realocou" value="false"></td>
                                        </tr>
                                        <tr style="font-size:17px">
                                            <td class="alg-left">Pagamentos / Uso do Recurso</td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.PagamentosUsoDoRecurso || data.PagamentosUsoDoRecurso == null ? "checked" : ""} id="pagamentos-uso-sim" name="PagamentosUsoDoRecurso" value="true"></td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.PagamentosUsoDoRecurso != null && !data.PagamentosUsoDoRecurso ? "checked" : ""} id="pagamentos-uso-nao" name="PagamentosUsoDoRecurso" value="false"></td>
                                        </tr>
                                        <tr style="font-size:17px">
                                            <td class="alg-left">Aplicou em Outro Banco</td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.AplicouEmOutroBanco || data.AplicouEmOutroBanco == null ? "checked" : ""}  id="aplicou-sim" name="AplicouEmOutroBanco" value="true"></td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.AplicouEmOutroBanco != null && !data.AplicouEmOutroBanco ? "checked" : ""} id="aplicou-nao" name="AplicouEmOutroBanco" value="false"></td>
                                        </tr>
                                        <tr style="font-size:17px">
                                            <td class="alg-left">Problemas de Relacionamento</td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.ProblemasDeRelacionamento || data.ProblemasDeRelacionamento == null ? "checked" : ""}  id="problemas-relacionamento-sim" name="ProblemasDeRelacionamento" value="true"></td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.ProblemasDeRelacionamento != null && !data.ProblemasDeRelacionamento ? "checked" : ""} id="problemas-relacionamento-nao" name="ProblemasDeRelacionamento" value="false"></td>
                                        </tr>
                                        <tr style="font-size:17px">
                                            <td class="alg-left">Vai Analisar a Oferta</td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.VaiAnalisarOferta || data.VaiAnalisarOferta == null ? "checked" : ""}  id="analisar-oferta-sim" name="VaiAnalisarOferta" value="true"></td>
                                            <td class="tbl-alg-center"><input type="radio" class="radiobuttonsize" ${data.VaiAnalisarOferta != null && !data.VaiAnalisarOferta ? "checked" : ""} id="analisar-oferta-nao" name="VaiAnalisarOferta" value="false"></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                   </div>
                    <div class="modal-footer">
                            <div class="col-lg-4 pull-right">
                                <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                                <button type="submit" class="btn btn-gradient" id="btn-atualizar">
                                    <i class="fa fa-refresh icone-atualizar"></i>
                                    Atualizar
                                </button>
                            </div>
                    </div>
               </form>
            </div>
           </div>`);

        modalContatoAplicResg.appendTo("#modais");

        modalContatoAplicResg.modal({ backdrop: 'static' });

        modalContatoAplicResg.on("hidden.bs.modal", function () {
            $(this).data('bs.modal', null);

            modalContatoAplicResg.remove();
        });

    }



    return {
        modalNovoEvento,
        modalNovolink,
        modalNovopipe,
        modalAtualizarpipe,
        modalCofirmarExclusaoLink,
        modalAtualizarTED,
        modalCockpit,
        newLink,
        modalSinv,
        modalCockpitColmeia,
        carregarSINV,
        tratarDados,
        statusChange,
        modalSinvEnviarTermos,
        modalSinvTransacional,
        modalContatoAplicResg
    }

})($)


