class Atendimento extends Solicitacao {

    cosntructor() {
        this.disabled = null;
    }

    verificarDecimal(valor) {
        if (!/\./.test(valor)) {
            valor = valor + '00';
        }
        else if (/\.\d{1}$/.test(valor)) {
            valor += '0'
        }
        return valor;
    }

    limitaCaracte(texto, limit) {
        return texto.substring(0, limit) + '<a href="#" class="ver-mais" onclick="verMais(this)"> Ver mais... </a>';
    }

    criaLinhaContato(ele, id = null) {

        var obsLimitado = this.limitaCaracte(ele.Observacao, 8);/*.substring(0, 10) + '<a href="#" class="ver-mais" onclick="verMais(this)"> Ver mais... </a>';*/

        var Id = id === null ? ele.Id : id;
        var data = id === null ? moment(ele.Data).format('DD/MM/YYYY HH:mm') : ele.Data;

        $('#tblContato tbody')
            .append(`<tr id="${Id}">
                        <td>${data}</td> 
                        <td>${ele.Contatado}</td>
                        <td style="position:relative">
                            <div style="width:150px; overflow:hidden">
                                <span class="texto-observacao-limitado">${obsLimitado}</span>
                                <div class="none VerMais">
                                    <div class="txtObs">${ele.Observacao}
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td style="text-align:center">
                            <div class="${this.disabled ? 'none' : ''}">
                                <i id="excuir-contato" class="fa fa-trash" style="color:red; font-size:16px;"></i> &nbsp;
                                <i id="editar-contato" class="fa fa-edit" style="color:red; font-size:16px; cursor: pointer"></i>
                            </div>
                        </td>
                    </tr>`);

    }

    popularSolicitacao(dataFechamento, perfil) {

        //var mesSolicitacao = moment(this.DataInicioProcesso).month();

        //var mesAtual = moment().month();

        //var mesAnterior = moment().subtract(1, 'months').month();

        ////Considerar 1 dia a mais da data de fechamento, para realizar o bloqueio dos campos
        //dataFechamento = moment(dataFechamento, 'DD/MM/YYYY').add(1, 'day').format('L');

        //if ((this.DataProrrogada != '' || this.DataProrrogada != null) && moment(this.DataProrrogada).isAfter(moment())) {
        //    this.disabled = false;
        //}
        //else if (mesSolicitacao == mesAtual || mesSolicitacao == mesAnterior) {

        //    this.disabled =
        //        (moment().isAfter(dataFechamento) || moment().isSame(dataFechamento))
        //        && moment(this.DataInicioProcesso).isBefore(dataFechamento);
        //} else {
        //    this.disabled = true;
        //}

        $('#nome-participante').val(this.NomeParticipante);
        $('#modal-detalhe').find('#cpf-portabilidade').val(this.CPF);
        $('#valor-previdencia').val('').val(this.verificarDecimal(this.SaldoPrevidencia));
        $('#numero-processo').val('').val(this.CodIdentificadorProcesso);
        $('#numero-proposta').val('').val(this.CodIdentificadorProposta);
        $('#consultor-pgp').val('').val(this.ConsultorPGP);
        $('#consultor-matriz').val('').val(this.ConsultorMatriz);
        $('#matricula-consultor').val('').val(this.MatriculaConsultor);
        $('#data-solicitacao').val('').val(this.DataInicioProcesso != null ? moment(this.DataInicioProcesso).format('DD/MM/YYYY') : null);
        $('#data-final-portabilidade').val('').val(this.PrazoAtendimento != null ? moment(this.PrazoAtendimento).format('DD/MM/YYYY') : null);
        $('#data-ref').val('').val(this.DataRef != null ? moment(this.DataRef).format('DD/MM/YYYY') : null);
        $('#segmento').val('').val(this.Segmento);
        $('#lideranca').val('').val(this.Lideranca);
        $('#susep-cedente').val('').val(this.SUSEPCedente);
        $('#susep-cessionaria').val('').val(this.SUSEPCessionaria);
        $('#entidade').val('').val(this.NomeEntidade);
        $('#cnpj-cedente').val('').val(this.CIDTFDCNPJCedente);
        $('#cnpj-cessionaria').val('').val(this.CIDTFDCNPJCessionaria);
        $('#agencia-portablidade').val('').val(this.Agencia);
        $('#conta-portabilidade').val('').val(this.Conta);
        $('#valor-retido').val('').val(this.ValorRetido != null ? this.verificarDecimal(this.ValorRetido) : '').prop('disabled', this.disabled);
        $('#data-conclusao').val('').val(this.DataConclusao !== null ? moment(this.DataConclusao).format('DD/MM/YYYY HH:mm') : null);
        $('#observacao').val('').val(this.Observacao).prop('disabled', this.disabled);

        if (!this.disabled) {
            $('#valor-solicitado').val('').val(this.verificarDecimal(this.ValorPrevistoSaida)).prop('disabled', perfil != 'Master');
        }
        else {
            $('#valor-solicitado').val('').val(this.verificarDecimal(this.ValorPrevistoSaida));
        }

        if (perfil == 'Especialista')
            $('#especialista').val('').val(this.Especialista).prop('disabled', true);
        else
            $('#especialista').val('').val(this.Especialista).prop('disabled', this.disabled);


        //if (this.DataProrrogada != null && this.DataProrrogada != '') {
        //    $('#data-prorrogada').val('').val(moment(this.DataProrrogada).format('L')).prop('disabled', this.disabled);
        //} else {
        //    $('#data-prorrogada').val('').prop('disabled', this.disabled);
        //}

        $('#status').prop('disabled', this.disabled);
        $('#agencia').prop('disabled', this.disabled);
        $('#conta').prop('disabled', this.disabled);
        $('#motivo').prop('disabled', this.disabled);
        $('#btn-atualizar').prop('disabled', this.disabled);

    }

    preencherSolicitacao(solicitacao) {
        this.Id = solicitacao.Id;
        this.Segmento = solicitacao.Segmento;
        this.Lideranca = solicitacao.Lideranca;
        this.ConsultorMatriz = solicitacao.ConsultorMatriz;
        this.ConsultorPGP = solicitacao.ConsultorPGP;
        this.NomeParticipante = solicitacao.NomeParticipante;
        this.CPF = solicitacao.CPF;
        this.SaldoPrevidencia = solicitacao.SaldoPrevidencia;
        this.ValorPrevistoSaida = solicitacao.ValorPrevistoSaida;
        this.NomeEntidade = solicitacao.NomeEntidade;
        this.DataInicioProcesso = solicitacao.DataInicioProcesso;
        this.PrazoAtendimento = solicitacao.PrazoAtendimento;
        this.DataRef = solicitacao.DataRef;
        this.CodIdentificadorProcesso = solicitacao.CodIdentificadorProcesso;
        this.CodIdentificadorProposta = solicitacao.CodIdentificadorProposta;
        this.SUSEPCedente = solicitacao.SUSEPCedente;
        this.SUSEPCessionaria = solicitacao.SUSEPCessionaria;
        this.CIDTFDCNPJCedente = solicitacao.CIDTFDCNPJCedente;
        this.CIDTFDCNPJCessionaria = solicitacao.CIDTFDCNPJCessionaria;
        this.StatusId = solicitacao.StatusId;
        this.SubStatusId = solicitacao.SubStatusId;
        this.MotivoId = solicitacao.MotivoId;
        this.SubMotivoId = solicitacao.SubMotivoId;
        this.MatriculaConsultor = solicitacao.MatriculaConsultor;
        this.ValorRetido = solicitacao.ValorRetido;
        this.Observacao = solicitacao.Observacao;
        this.Agencia = solicitacao.Agencia;
        this.Conta = solicitacao.Conta;
        this.DataConclusao = solicitacao.DataConclusao;
        this.Especialista = solicitacao.Especialista;
        this.Motivo = solicitacao.Motivo;
        this.Status = solicitacao.Status
        this.SubStatus = solicitacao.SubStatus;
        this.SubMotivo = solicitacao.SubMotivo;
    }

    validarStatus(ele, motivosSemSubmotivos, urlSubmotivo, urlSubtatus, isload = true) {
        let valor = ele.val();
        let texto = ele.text();
        let temSubStatus = false;
        let self = this;

        $.get(urlSubtatus, { id: valor }, function (resp) {
            resp = JSON.parse(resp);

            if (resp.length > 0) {
                temSubStatus = true;

                $("#sub-status").empty();

                var options = '';

                resp.forEach(function (ele) {
                    options += `<option value="${ele.Id}" ${ele.Id == self.SubStatusId ? 'selected': ''}>${ele.Descricao}</option>`;
                });

                $("#sub-status").html(options);
            }

            if (!this.disabled) {

                if (!temSubStatus) {
                    $("#sub-status").val('');
                }

                //Status
                switch (texto) {
                    case "Não Tratado": //"1016"
                        $('#data-conclusao').val('').attr('disabled', true);
                        $('#valor-retido').val('').attr('disabled', true);
                        $("#motivo").attr('disabled', true).val('');
                        $("#submotivo").attr('disabled', true).val('');
                        $("#sub-status").prop('disabled', !temSubStatus);
                        $('#valor-retido').attr('disabled', true).val('');

                        break;
                    case "Tratando": //"1017"
                        $('#data-conclusao').val('').attr('disabled', true);
                        $('#valor-retido').val('').attr('disabled', true);
                        $("#motivo").attr('disabled', true).val('');
                        $("#submotivo").attr('disabled', true).val('');
                        $("#sub-status").prop('disabled', !temSubStatus);
                        $('#valor-retido').attr('disabled', true).val('');
                        break;

                    case "Tratado": //"1018"
                        $('#data-conclusao').val('').attr('disabled', false);
                        $('#data-conclusao').removeAttr('disabled').val(moment().format('YYYY-MM-DD'));
                        $("#motivo").attr('disabled', true).val('');
                        $("#sub-status").prop('disabled', !temSubStatus);

                        self.ValidarSubStatus($("#sub-status").val(), motivosSemSubmotivos, urlSubmotivo, isload);

                        if (isload) self.validarMotivo(motivosSemSubmotivos, urlSubmotivo, isload);
                        break;

                    default:
                        $("#motivo").attr('disabled', true).val('');
                        $("#submotivo").attr('disabled', true).val('');
                        $('#valor-retido').attr('disabled', true).val('');
                        $('#data-conclusao').attr('disabled', true).val('');
                        $("#sub-status").prop('disabled', !temSubStatus).val('');
                        break;
                }
            }
            else {
                //Desabilita atualização caso passe não cumpra a regra do 5º dia util
                if (valor >= "3") {
                    self.validarMotivo(motivosSemSubmotivos, urlSubmotivo, isload);
                }
                $('#contato-agencia').prop('disabled', true);
                $('#data-conclusao').prop('disabled', true);
                $('#btn-atualizar').prop('disabled', true);
            }

        });
    }

    ValidarSubStatus(subStatus, motivoSemSubmotivo, urlSubmotivo, isload) {
        //Adicionar configuracões para outros possiveis substatus que possam existir
        if (subStatus == "1") {
            $('#valor-retido').removeAttr('disabled').val(this.verificarDecimal(this.ValorPrevistoSaida));
            
            $('#motivo').children().each(function (idx, ele) {
                if ($(ele).text().startsWith("Reti")) {
                    this.disabled = false;
                    this.selected = true;
                }
            });
            $('#motivo').attr('disabled', true);
            $('#submotivo').attr('disabled', true).val('');
        }
        else {
            $('#valor-retido').attr('disabled', true).val('');
            $('#motivo').removeAttr('disabled');

            $('#motivo').children().each(function (idx, ele) {
                if ($(ele).text().startsWith("Reti")) {
                    this.disabled = true;
                    this.selected = false;
                }
            });

            this.validarMotivo(motivoSemSubmotivo, urlSubmotivo, isload)
        }
    }

    validarMotivo(motivosSemSubmotivos, urlSubmotivo, isload) {
        //Motivo
        let motivo = !isload ? $('#modal-detalhe').find('#motivo').val() : this.MotivoId;

        const self = this;

        if (motivo !== null) {

            var motivos = $('#modal-detalhe').find('#motivo option');

            motivos.each(function (i, v) {
                if ($(v).val() === motivo.toString()) {
                    v.selected = true;
                }
            });

            self.validarSubmotivo(motivosSemSubmotivos, urlSubmotivo);
        }
    }

    validarSubmotivo(motivosSemSubmotivos, urlSubmotivo) {
        const elemento = $('#submotivo');
        const self = this;
        var motivoId = $('#motivo').val();

        motivoId = parseInt(motivoId);

        if (motivosSemSubmotivos.includes(motivoId) || isNaN(motivoId)) {
            elemento.val('');
            elemento.attr('disabled', true);
            return;
        }

        elemento.removeAttr('disabled');

        var inativos = moment(this.DataInicioProcesso).year() < 2020;

        $.get(urlSubmotivo, { motivoId, inativos }, function (resp) {

            let submotivos = JSON.parse(resp);
            var submotivo = self.SubMotivoId;

            elemento.empty();

            submotivos.forEach(function (ele, ind) {
                elemento.append(`
                            <option value="${ele.Id}"${submotivo === ele.Id ? "selected" : ""} >${ele.Descricao}</option>
                        `);
            });
        });
    }

    clear() {
        this.Id = null;
        this.DescricaoTipoSolicitacao = null;
        this.CodigoIdentificadorProcesso = null;
        this.CodigoIdentificadorSolicitacao = null;
        this.NomeParticipante = null;
        this.ValorPrevistoSaida = null;
        this.NomeEntidade = null;
        this.DataInicioProcesso = null;
        this.CodigoIdentificadorProposta = null;
        this.SusepCedente = null;
        this.SusepCessionaria = null;
        this.CidtfdCnpjCdent = null;
        this.CidtfdCnpjCessionaria = null;
        this.CodigoIdentificadorSituacaoAN = null;
        this.DescricaoSituacaoAndamentoPro = null;
        this.CodigoIdentificadorAgenciaBRA = null;
        this.CdDiretoriaTecnica = null;
        this.NomeDiretoriaTecnica = null;
        this.CentidPrevdCompl = null;
        this.TipoSolicitacao = null;
        this.CPF = null;
        this.Consultor1 = null;
        this.Reserva = null;
        this.Lider4 = null;
        this.Lideranca = null;
        this.StatusId = null;
        this.SubStatusId = null;
        this.Contato = [];
        this.DataFinal = null;
        this.ValorRetido = null;
        this.ContatoAgencia = null;
        this.DataSeloVoz = null;
        this.Ramal = null;
        this.MotivoId = null;
        this.SubMotivoId = null;
        this.Observacao = null;
        this.DataConclusao = null;
        this.Especialista = null;
        this.Motivo = null;
        this.Status = null;
        this.SubStatus = null;
        this.SubMotivo = null;

        //Resete motivo Submotivo.
        $('#motivo').val('');
        $('#submotivo').val('').attr('disabled', true);
    }

    hasAtendmento() {
        return this.Id === null;
    }
}