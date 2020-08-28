var core = core || {}

core.notify = (function ($) {
    /**
* Formata um numero e retorna sua string em formato currency pt-br
* @param {Float32Array} numero
* @returns {string}
*/
    function format_number_pt_br(numero) {
        return numero.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
    }

    //default notify conf
    $.notify.defaults({
        showDuration: 100,
        style: 'bootstrap',
        gap: 5
    })

    //Configuração Toastr
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "2000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    /**
    * Dispara o pop-up notify.
    * @param {string} text
    * @param {string} type
    * @param {string} position
    * @param {string} selector
    * @returns {void}
    */
    function showNotify(text, type, position) {
        ///Criar um notificação na tela tanto globalmente quanto em um determinado elemento se informado seu seletor.
        toastr.options.positionClass = `toast-${position}`

        toastr[type](text)

    }

    /**
    * Dispar popup de alerta de Aplicação e Resgate.
    * @param {string} agencia
    * @param {string} conta
    * @param {string} valor
    * @param {string} data
    */
    function alertAplicacaoResgate(agencia, conta, valor, data, operacao) {

        var titulo;

        switch (operacao.toLowerCase()) {
            case "apli":
                titulo = "Aplicação";
                break;
            case 'resg':
                titulo = "Resgate Parcial";
                break;
            case 'rest':
                titulo = "Resgate Total";
                break;
        }

        $.notify.addStyle('alertaAplicacaoResgate', {
            html: `<div>
                <button style="position:absolute; background-color:unset;border:0; font-weight:400;  right:1.5rem; top:7%; color:#fff" type="button" id="close-notify-pipe" area-label="Close">
                    <span style="font-size: 20px; font-weight: bold;" area-hidden="true">x</span>
                </button>
                <h3><i class="fa fa-info-circle"></i> <span>${titulo}</span></h3>
                <hr />
                <div>
                    <p>
                        <strong>Agencia: </strong><tt><span data-notify-text='agencia'></span> 
                        <span style="float:right; margin-right:40px">
                            <strong>Conta: </strong><tt><span data-notify-text='conta'></span></tt>
                        </span>
                    </p>
                    <p>
                        <span style="float:left"><strong>Valor:</strong><tt><span data-notify-text='valor'> </span></tt></span>
                        
                        <span style="float:right;">
                            <strong>Data: </strong><tt><span data-notify-text='data'> </span></tt>
                        </span>
                            <div style="clear:both"></div>
                    </p>
                    <p></p>
                </div>
            </div>`,
            classes: {
                aplic: {
                    'position': 'relative',
                    'background-color': '#0d7d0b',
                    'padding': '5px 2rem 10px 2rem',
                    'color': '#fff',
                    'margin': '2rem 0 15px 0',
                    'min-width': '350px',
                    'max-width': '500px',
                    'max-height': '290px'
                },
                resgate: {
                    'position': 'relative',
                    'background-color': '#e88f00',
                    'padding': '5px 2rem 10px 2rem',
                    'color': '#fff',
                    'margin': '2rem 0 15px 0',
                    'min-width': '350px',
                    'max-width': '500px',
                    'max-height': '290px'
                }
            }
        });

        var className = operacao.toUpperCase() == 'APLI' ? 'aplic' : 'resgate';
        $.notify(
            {
                agencia,
                conta,
                valor,
                data
            },
            {

                style: 'alertaAplicacaoResgate',
                position: 'bottom right',
                className: className,
                autoHide: true,
                clickToHide: false,
                showAnimation: 'show',
                hideAnimation: 'hide',
                hideDuration: 600,
                showDuration: 600,
            });

        $(document).on('click', '#close-notify-pipe', function () {
            //programmatically trigger propogating hide event
            $(this).trigger('notify-hide');
        });
    }

    /**
     * Dispar popup de alerta de um pipeline especificado.
     * @param {string} data
     * @param {string} valor
     * @param {String} cliente
     */
    function alertPipe(data, valor, cliente) {

        $.notify.addStyle('alertaPipeline', {
            html: `<div>
                <button style="position:absolute; background-color:unset;border:0; font-weight:400;  right:1.5rem; top:7%; color:#fff" type="button" id="close-notify-pipe" area-label="Close">
                    <span style="font-size: 20px; font-weight: bold;" area-hidden="true">x</span>
                </button>
                <h3><i class="fa fa-info-circle"></i> <span>Pipeline</span></h3>
                <small>Você tem um pipeline se aproximando.\nVeja os detalhes abaixo.</small>
                <hr />
                <p><strong>Data: </strong><tt><span data-notify-text='data'></span></tt></p>
                <p><strong>Valor: </strong><tt> <span data-notify-text='valor'></span></tt></p>
                <p><strong>Cliente: </strong><tt><span data-notify-text='cliente'> </span></tt></p>
            </div>`,
            classes: {
                default: {
                    'position': 'relative; background-color: #e88f00; padding: 5px 2rem 10px 2rem',
                    'color': '#fff',
                    'margin': '2rem 0 15px 0',
                    'min-width': '350px',
                    'max-width': '500px',
                    'max-height': '290px'
                }
            }
        });

        $.notify(
            {
                data,
                valor,
                cliente
            },
            {
                style: 'alertaPipeline',
                position: 'bottom right',
                className: 'default',
                autoHide: true,
                clickToHide: false,
                showAnimation: 'show',
                hideAnimation: 'hide',
                hideDuration: 600,
                showDuration: 600,
            });

        $(document).on('click', '#close-notify-pipe', function () {
            //programmatically trigger propogating hide event
            $(this).trigger('notify-hide');
        });
    }

    /**
     * Dispara o popup de alert para eventos próximos da data inicial.
     * @param {string} titulo
     * @param {string} dataInicio
     * @param {string} dataFim
     * @param {string} descricao
     */
    function alertEvento(titulo, dataInicio, dataFim, descricao) {

        $.notify.addStyle('alertaEventos', {
            html: `<div>
                <button style="position:absolute; background-color:unset;border:0; font-weight:400;  right:1.5rem; top:7%; color:#fff" type="button" id="close-notify" area-label="Close">
                    <span style="font-size: 20px; font-weight: bold;" area-hidden="true">x</span>
                </button>
                <h3><i class="fa fa-info-circle"></i> <span>Evento</span></h3>
                <small>Você tem um novo evento se aproximando.\nVeja os detalhes abaixo.</small>
                <hr />
                <p><strong>Titulo: </strong><tt><span data-notify-text='titulo'></span></tt> </p>
                <p><strong>Data Início: </strong><tt><span data-notify-text='dataInicio'></span></tt></p>
                <p><strong>Data Fim: </strong><tt> <span data-notify-text='dataFim'></span></tt></p>
                <p><strong>Descrição: </strong><tt><span data-notify-text='descricao'> </span></tt></p>
            </div>`,
            classes: {
                default: {
                    'position': 'relative; background-color: #e88f00; padding: 5px 2rem 10px 2rem',
                    'color': '#fff',
                    'margin': '2rem 0 15px 0',
                    'min-width': '350px',
                    'max-width': '500px',
                    'max-height': '290px'
                }
            }
        });

        //Dispara notificação
        $.notify(
            {
                titulo,
                dataInicio,
                dataFim,
                descricao
            },
            {
                style: 'alertaEventos',
                position: 'bottom right',
                className: 'default',
                autoHide: false,
                clickToHide: true,
                showAnimation: 'show',
                hideAnimation: 'hide',
                hideDuration: 600,
                showDuration: 600,
            });


        $(document).on('click', '#close-notify', function () {
            //programmatically trigger propogating hide event
            $(this).trigger('notify-hide');
        });
    }

    /**
        *  Inicializa o pop-up de alerta de nova TED, com estilo personalizado
        * @param {string} titulo
        * @param {string} agencia
        * @param {string} conta
        * @param {string} cliente
        * @param {Float32Array} valor
        * @param {FunctionStringCallback} verificar_callback
        * @returns {void}
        */
    function alertTeds(titulo, agencia, conta, cliente, valor, verificar_callback, url) {
        'use strics';

        //notify Style outlook
        $.notify.addStyle('outlook', {
            html: `<div style="position:relative">
                        <button style="position:absolute; background-color:unset;border:0; font-weight:400;  right:1.5rem; top:7%; color:#fff" type="button" id="close-notify" class="" area-label="Close">
                            <span style="font-size: 20px; font-weight: bold;" area-hidden="true">x</span>
                        </button>
                        <h3><i class="fa fa-info-circle"></i> <span data-notify-text='titulo'></span></h3>
                        <hr />
                        <p> <strong>Agência:</strong> <tt><span data-notify-text='agencia'></span></tt></p>
                        <p><strong> Conta: </strong> <span data-notify-text='conta'></span></p>
                        <p> <strong>Cliente: </strong><span data-notify-text='cliente'></span></p>
                        <h4><strong><span data-notify-text='valor'></span></strong></h4>
                        <hr />
                        <a class="btn bg-green btn-sm" href="${url}">Tratar</a>
                    </div>`,
            classes: {
                default: {
                    'background-color': 'rgba(211, 231, 238, 1) !important',
                    'padding': '2rem',
                    'margin': '2rem 0',
                    'min-width': '350px'
                },
                'bg-red': {
                    'background-color': '#a51807 !important',
                    'padding': '5px 2rem 10px 2rem',
                    'color': '#fff',
                    'margin': '2rem 0 15px 0',
                    'min-width': '350px',
                    'max-height': '290px'
                }
            }
        })


        //Gera o popup de alerta de novo ted.
        valor = format_number_pt_br(valor)

        $.notify(
            {
                titulo,
                agencia,
                conta,
                cliente,
                valor
            },
            {
                style: 'outlook',
                position: 'bottom right',
                className: 'bg-red',
                autoHide: true,
                clickToHide: false,
                showAnimation: 'show',
                hideAnimation: 'hide',
                hideDuration: 600,
                showDuration: 600,
            })

        //Fecha a notificação com o clique no x
        //Controle dos eventos cliques da notificação outlook
        $(document).on('click', '#verificar', function () {

            valor = parseFloat(valor.slice(2).replace('.', "").replace(',', '.'))
            var response = { titulo, agencia, conta, cliente, valor }
            verificar_callback(response)
            $(this).trigger('notify-hide')
        })
        $(document).on('click', '#close-notify', function () {
            //programmatically trigger propogating hide event
            $(this).trigger('notify-hide');
        });
    }

    function alertRetencao(especialista) {
        // Add the message to the page. 
        var host = location.host;

        var menus = ['Home', 'CarteiraCliente', 'TEDs', 'Vencimento', 'Pipeline', 'AplicacaoResgate','Portabilidade']

        var pathArray = location.pathname.substring(1).split('/');

        if (!menus.includes(pathArray[0]))
            host += pathArray[0];
        

        $.notify.addStyle('alertRetencao', {
            html: `
                        <div class="row" style="padding:5px;width:320px;" >
                            <iframe src="${host}/Audio/transmitter.mp3" style="display:none"></iframe>            
                            <div class="text-center" style="width:100%; border-bottom:1px solid #fff">
                                <h4>PARABÉNS PELA RETENÇÃO</h4>
                            </div>
                            <div class="p-l-10 text-center" style="width:100%; margin-top:10px">
                                <label>Especialista: </label>
                                <span data-notify-text='especialista' style="text-align:center;">
                            </div>
                        </div>
                    `,
            classes: {
                default: {
                    'background': 'linear-gradient(45deg, #0235a6 0%, #e20c58 100%)',
                    'border-radius': '5px',
                    'color': '#fff !important',
                    'width':'320px'   
                }
            }
        });

        $.notify(
            {
                especialista
            },
            {
                style: 'alertRetencao',
                className: 'default',
                position: 'bottom right',
                autoHide: true,
                clickToHide: true,
                showAnimation: 'show',
                autoHideDelay: 10000,
                hideAnimation: 'hide',

            });
    }


    function IniciarNotificacaoRetencao() {
        var chat = $.connection.notificacao;

        chat.client.dispararNotificacao = function (especialsita) {
            alertRetencao(especialsita);
        }
        
        $.connection.hub.start().done(function () {
            console.log('hub iniciado');

            $('#notificarRetencao').click(function () {
                var especialista = sessionStorage.getItem('retencao');

                chat.server.notificar(especialista);

                sessionStorage.removeItem('retencao');
            });
        });
    }

    function obterRetencao() {
        if (sessionStorage.getItem('retencao') != '' &&
            sessionStorage.getItem('retencao') != null &&
            sessionStorage.getItem('retencao') != undefined)
        {
            $('#notificarRetencao').trigger('click');
        }
    }

    return {
        showNotify,
        alertTeds,
        alertEvento,
        alertPipe,
        alertAplicacaoResgate,
        obterRetencao,
        IniciarNotificacaoRetencao
    }
})($)