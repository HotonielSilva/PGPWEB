USE [BD_PGP_WEB_DESENV]
GO
/****** Object:  UserDefinedTableType [dbo].[IDList]    Script Date: 29/07/2020 17:28:31 ******/
CREATE TYPE [dbo].[IDList] AS TABLE(
	[ID] [int] NULL
)
GO
/****** Object:  Table [dbo].[SubMotivo]    Script Date: 29/07/2020 17:28:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubMotivo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](50) NOT NULL,
	[MotivoId] [int] NOT NULL,
	[EmUso] [bit] NULL,
 CONSTRAINT [PK_SubMotivo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Motivo]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Motivo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](50) NOT NULL,
	[Evento] [varchar](50) NOT NULL,
	[EmUso] [bit] NULL,
 CONSTRAINT [PK__MotivosP__3214EC07121B97D6] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_MotivosSemSubmotivos]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[vw_MotivosSemSubmotivos] as

select Id from Motivo where Id not in 
( select distinct m.Id from Motivo m
join SubMotivo s on m.Id = s.MotivoId )
 



GO
/****** Object:  Table [dbo].[CaminhoDinheiro]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaminhoDinheiro](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MesDataBase] [varchar](10) NOT NULL,
	[Agencia] [varchar](10) NOT NULL,
	[Segmento] [varchar](50) NOT NULL,
	[Produto] [varchar](100) NOT NULL,
	[VL_APLIC] [decimal](18, 2) NOT NULL,
	[VL_RESG] [decimal](18, 2) NOT NULL,
	[PERC_DINHEIRO_NOVO] [decimal](18, 2) NOT NULL,
	[PERC_CDB] [decimal](18, 2) NOT NULL,
	[PERC_ISENTOS] [decimal](18, 2) NOT NULL,
	[PERC_COMPROMISSADAS] [decimal](18, 2) NOT NULL,
	[PERC_LF] [decimal](18, 2) NOT NULL,
	[PERC_FUNDOS] [decimal](18, 2) NOT NULL,
	[PERC_CORRET] [decimal](18, 2) NOT NULL,
	[PERC_PREVI] [decimal](18, 2) NOT NULL,
	[PERC_MSM_TIT_ENV_TOTAL] [decimal](18, 2) NOT NULL,
	[PERC_DIF_TIT_ENV_TOTAL] [decimal](18, 2) NOT NULL,
	[PERC_OUTROS] [decimal](18, 2) NOT NULL,
	[MatriculaConsultor] [varchar](7) NOT NULL,
	[Consultor] [varchar](100) NOT NULL,
	[MatriculaCordenador] [char](7) NOT NULL,
	[Cordenador] [varchar](100) NOT NULL,
 CONSTRAINT [PK__CaminhoD__3214EC072AA5D21A] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_CaminhoDinheiroAgrupado]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_CaminhoDinheiroAgrupado]
as

select 
	MesDataBase,
	MatriculaCordenador,
	Cordenador,
	MatriculaConsultor,
	Consultor, 
	Produto, 
	Sum(VL_APLIC) - Sum(VL_RESG) 'CAPLIQ' ,
	Sum(VL_APLIC) 'VL_APLIC', 
	SUM(PERC_DINHEIRO_NOVO)'PERC_NEWMONEY',
	Sum(VL_RESG) 'VL_RESG',
	SUM(PERC_CDB) 'PERC_CDB',
	SUM(PERC_ISENTOS) 'PERC_ISENTOS',
	SUM(PERC_COMPROMISSADAS) 'PERC_CPMSS',
	SUM(PERC_LF) 'PERC_LF',
	SUM(PERC_FUNDOS) 'PERC_FDOS',
	SUM(PERC_CORRET) 'PERC_CORRET',
	SUM(PERC_PREVI) 'PERC_PREVI',
	SUM(PERC_MSM_TIT_ENV_TOTAL) 'PERC_MSM_TIT_ENV_TOTAL',
	SUM(PERC_DIF_TIT_ENV_TOTAL) 'PERC_DIF_TIT_ENV_TOTAL',
	SUM(PERC_OUTROS) 'PERC_OUTROS'
from CaminhoDinheiro
group by MesDataBase, MatriculaCordenador, Cordenador, MatriculaConsultor, Consultor, Produto 
GO
/****** Object:  Table [dbo].[Clusterizacoes]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clusterizacoes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CONTA] [int] NOT NULL,
	[AGENCIA] [int] NOT NULL,
	[CPF_CNPJ] [varchar](50) NULL,
	[PERFIL_API] [varchar](50) NULL,
	[MES_VCTO_API] [varchar](50) NULL,
	[NIVEL_DESENQ_FX_RISCO] [varchar](50) NULL,
	[SALDO_TOTAL_M3] [decimal](18, 2) NOT NULL,
	[SALDO_TOTAL] [decimal](18, 2) NOT NULL,
	[SALDO_CORRETORA_BRA] [decimal](18, 2) NOT NULL,
	[SALDO_CORRETORA_AGORA] [decimal](18, 2) NOT NULL,
	[SALDO_CORRETORA] [decimal](18, 2) NOT NULL,
	[SALDO_PREVIDENCIA] [decimal](18, 2) NOT NULL,
	[SALDO_POUPANCA] [decimal](18, 2) NOT NULL,
	[SALDO_INVESTS] [decimal](18, 2) NOT NULL,
	[SALDO_DAV_20K] [decimal](18, 2) NOT NULL,
	[SALDO_COMPROMISSADAS] [decimal](18, 2) NOT NULL,
	[SALDO_ISENTOS] [decimal](18, 2) NOT NULL,
	[SALDO_LF] [decimal](18, 2) NOT NULL,
	[SALDO_CDB] [decimal](18, 2) NOT NULL,
	[SALDO_FUNDOS] [decimal](18, 2) NOT NULL,
	[COD_GER_RELC] [varchar](10) NULL,
	[GER_RELC] [varchar](100) NULL,
	[Situacao] [varchar](50) NULL,
	[Segmento] [varchar](50) NULL,
	[DiretoriaRegional] [varchar](50) NULL,
	[CodDiretoriaRegional] [varchar](50) NULL,
	[AREA] [nvarchar](50) NULL,
	[ACAO] [nvarchar](100) NULL,
	[ACAO_PRINCIPAL] [nvarchar](100) NULL,
 CONSTRAINT [PK_dbo.Clusterizacoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Encarteiramento]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Encarteiramento](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TIP_CLIENTE] [varchar](50) NULL,
	[CPF] [varchar](14) NULL,
	[DATA] [datetime2](7) NULL,
	[Agencia] [varchar](10) NOT NULL,
	[Conta] [varchar](10) NOT NULL,
	[AG_PRINC] [varchar](50) NULL,
	[CONTA_PRINC] [varchar](50) NULL,
	[CONSULTOR] [varchar](100) NULL,
	[Matricula] [varchar](30) NOT NULL,
	[EQUIPE_RESPONSAVEL] [varchar](70) NULL,
	[EQUIPE_MESA] [varchar](70) NULL,
	[DIR_REG_AG_PRINC] [varchar](100) NULL,
	[GER_REG_AG_PRINC] [varchar](100) NULL,
	[COD_REG] [int] NULL,
	[COD_DIR] [int] NULL,
	[AREA] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.Encarteiramento] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vwClusterTopTier]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwClusterTopTier]
AS
SELECT        TOP (100) PERCENT dbo.Encarteiramento.CONSULTOR, dbo.Clusterizacoes.AGENCIA, dbo.Clusterizacoes.CONTA, dbo.Clusterizacoes.CPF_CNPJ, dbo.Clusterizacoes.GER_RELC, dbo.Clusterizacoes.ACAO, 
                         dbo.Clusterizacoes.NIVEL_DESENQ_FX_RISCO, dbo.Clusterizacoes.PERFIL_API, dbo.Clusterizacoes.SALDO_TOTAL, dbo.Clusterizacoes.SALDO_PREVIDENCIA, dbo.Clusterizacoes.ACAO_PRINCIPAL
FROM            dbo.Clusterizacoes INNER JOIN
                         dbo.Encarteiramento ON dbo.Clusterizacoes.AGENCIA = dbo.Encarteiramento.Agencia AND dbo.Clusterizacoes.CONTA = dbo.Encarteiramento.Conta
WHERE        (dbo.Clusterizacoes.AREA LIKE 'TOP TIER%') AND (dbo.Encarteiramento.AREA LIKE 'TOP TIER%')
ORDER BY dbo.Clusterizacoes.SALDO_TOTAL DESC
GO
/****** Object:  Table [dbo].[Cockpit]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cockpit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CodFuncionalGerente] [int] NOT NULL,
	[NomeGerente] [varchar](100) NOT NULL,
	[CPF] [varchar](14) NULL,
	[NomeCliente] [varchar](100) NOT NULL,
	[CodigoAgencia] [int] NOT NULL,
	[NomeAgencia] [varchar](100) NULL,
	[Conta] [int] NOT NULL,
	[DataEncarteiramento] [date] NULL,
	[DataContato] [datetime] NOT NULL,
	[DataRetorno] [datetime] NULL,
	[Observacao] [varchar](8000) NULL,
	[ContatoTeveExito] [bit] NOT NULL,
	[DataHoraEdicaoContato] [datetime] NULL,
	[MeioContato] [varchar](100) NOT NULL,
	[ClienteNaoLocalizado] [bit] NOT NULL,
	[TipoTransacao] [varchar](100) NULL,
	[Finalizado] [bit] NULL,
	[GerenteRegistrouContato] [int] NULL,
	[MatriculaConsultor] [varchar](50) NULL,
 CONSTRAINT [PK_Cockpit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_NomeCliente]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vw_NomeCliente] as 
	select distinct c.CodigoAgencia as Agencia, c.Conta, c.NomeCliente 
		from Cockpit c join Encarteiramento e on
		e.Agencia = c.CodigoAgencia and e.Conta = c.Conta

GO
/****** Object:  Table [dbo].[Aniversarios]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aniversarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CPF] [varchar](14) NOT NULL,
	[Agencia] [int] NOT NULL,
	[Conta] [int] NOT NULL,
	[DataNascimento] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AplicacaoResgate]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AplicacaoResgate](
	[Id] [int] NOT NULL,
	[agencia] [int] NOT NULL,
	[conta] [int] NOT NULL,
	[data] [date] NOT NULL,
	[hora] [time](7) NOT NULL,
	[operacao] [varchar](45) NULL,
	[perif] [varchar](45) NULL,
	[produto] [varchar](45) NULL,
	[terminal] [varchar](45) NULL,
	[valor] [decimal](20, 2) NOT NULL,
	[gerente] [varchar](50) NULL,
	[advisor] [varchar](45) NULL,
	[segmento] [varchar](45) NULL,
	[enviado] [bit] NULL,
	[MatriculaConsultor] [varchar](10) NOT NULL,
	[Notificado] [bit] NOT NULL,
 CONSTRAINT [PK_TbAplicacaoResgate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AplicResgateContatos]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AplicResgateContatos](
	[IdAplicResgate] [int] NOT NULL,
	[ContatouCliente] [bit] NULL,
	[Realocou] [bit] NULL,
	[PagamentosUsoDoRecurso] [bit] NULL,
	[AplicouEmOutroBanco] [bit] NULL,
	[ProblemasDeRelacionamento] [bit] NULL,
	[VaiAnalisarOferta] [bit] NULL,
 CONSTRAINT [PK_AplicResgateContatos] PRIMARY KEY CLUSTERED 
(
	[IdAplicResgate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CaptacaoLiquida]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaptacaoLiquida](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Agencia] [varchar](10) NOT NULL,
	[Conta] [varchar](20) NOT NULL,
	[Ag_Conta] [varchar](50) NOT NULL,
	[CodAgencia] [varchar](100) NULL,
	[MesDataBase] [varchar](30) NOT NULL,
	[Diretoria] [varchar](100) NOT NULL,
	[TipoPessoa] [char](2) NOT NULL,
	[GerenciaRegional] [varchar](100) NOT NULL,
	[MatriculaCordenador] [char](7) NOT NULL,
	[CordenadorPGP] [varchar](100) NOT NULL,
	[MatriculaConsultor] [char](7) NOT NULL,
	[Consultor] [varchar](100) NOT NULL,
	[DataBase] [datetime] NOT NULL,
	[Produto] [varchar](100) NOT NULL,
	[ValorAplicacao] [decimal](18, 2) NOT NULL,
	[ValorResgate] [decimal](18, 2) NOT NULL,
	[ValorNET] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_CaptacaoLiquida] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CaptacaoLiquidaResumo]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaptacaoLiquidaResumo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MatriculaConsultor] [varchar](30) NOT NULL,
	[ProdutoMacro] [varchar](100) NOT NULL,
	[VL_NET] [decimal](18, 5) NOT NULL,
	[MatriculaCordenador] [varchar](30) NOT NULL,
 CONSTRAINT [PK__Captacao__3214EC07EB835122] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Configuracoes]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configuracoes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Chave] [varchar](100) NOT NULL,
	[Valor] [varchar](250) NOT NULL,
	[Descricao] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Corretora]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Corretora](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Agencia] [varchar](20) NULL,
	[Conta] [varchar](20) NULL,
	[CPF] [varchar](14) NULL,
	[Status] [varchar](15) NOT NULL,
	[Nome] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Evento]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Evento](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](50) NOT NULL,
	[Nome] [varchar](70) NOT NULL,
	[Descricao] [varchar](100) NOT NULL,
	[DataHoraInicio] [datetime] NOT NULL,
	[DataFim] [datetime] NOT NULL,
	[MatriculaConsultor] [varchar](10) NOT NULL,
	[Finalizado] [bit] NOT NULL,
	[Notificado] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Evento] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fundos]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fundos](
	[Nome] [varchar](100) NOT NULL,
	[Cnpj] [varchar](20) NOT NULL,
 CONSTRAINT [PK_Fundos] PRIMARY KEY CLUSTERED 
(
	[Cnpj] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Investfacil]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Investfacil](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AGENCIA] [varchar](10) NOT NULL,
	[Vlr_Evento] [decimal](18, 2) NOT NULL,
	[CONTA] [varchar](13) NOT NULL,
	[SEGMENTO_CLIENTE] [varchar](150) NULL,
	[NUM_CONTRATO] [varchar](100) NULL,
	[MES_DT_BASE] [varchar](100) NULL,
	[DT_EMISSAO] [datetime] NULL,
	[PRAZO_PERMAN] [int] NULL,
	[FX_PERMANENCIA] [varchar](100) NULL,
	[FX_VOLUME] [varchar](100) NULL,
	[SEGM_AGRUPADO] [varchar](100) NULL,
	[SEGMENTO_MACRO] [varchar](100) NULL,
	[MatriculaConsultor] [varchar](30) NULL,
 CONSTRAINT [PK_dbo.Investfacil] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvestFacilResumo]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvestFacilResumo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Matricula] [varchar](10) NOT NULL,
	[Vlr_Evento] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Matricula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Links]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Links](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](50) NULL,
	[Url] [varchar](200) NULL,
	[Exibir] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Thread] [varchar](255) NOT NULL,
	[Level] [varchar](50) NOT NULL,
	[Logger] [varchar](255) NOT NULL,
	[Message] [varchar](4000) NOT NULL,
	[Exception] [varchar](2000) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notificacoes]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notificacoes](
	[id] [int] NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[valor] [bit] NOT NULL,
	[IdUsuario] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Origem]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Origem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](50) NOT NULL,
	[Evento] [varchar](50) NOT NULL,
 CONSTRAINT [PK__OrigemPi__3214EC07ACB02FD8] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Perfil]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Perfil](
	[PerfilId] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PerfilId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pipeline]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pipeline](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NomeCliente] [varchar](50) NULL,
	[Agencia] [int] NOT NULL,
	[Consultor] [varchar](50) NOT NULL,
	[Conta] [int] NOT NULL,
	[BradescoPrincipalBanco] [bit] NOT NULL,
	[ValoresNoMercado] [decimal](18, 2) NULL,
	[ValorDoPipe] [decimal](18, 2) NOT NULL,
	[OrigemId] [int] NULL,
	[DataPrevista] [datetime] NOT NULL,
	[DataDaConversao] [datetime] NULL,
	[DataProrrogada] [datetime] NULL,
	[MotivoId] [int] NULL,
	[StatusId] [int] NOT NULL,
	[Observacoes] [varchar](1000) NULL,
	[ValorAplicado] [decimal](18, 2) NULL,
	[MatriculaConsultor] [varchar](10) NOT NULL,
	[Notificado] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.PipelineArquivos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Produtividade]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Produtividade](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Parametro] [varchar](50) NOT NULL,
	[ValorPercerntualMinino] [decimal](18, 2) NOT NULL,
	[De] [datetime] NOT NULL,
 CONSTRAINT [PK_Produtividade] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Qualitativo]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Qualitativo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NomeConsultor] [varchar](100) NULL,
	[OBJETIVOTOTAL] [int] NOT NULL,
	[DENTRODACARTEIRA] [int] NOT NULL,
	[FORADACARTEIRA] [int] NOT NULL,
	[TOTALCONTATOS] [int] NOT NULL,
	[PORCENTAGEMATINGIMENTO] [nvarchar](max) NULL,
	[GIRODECARTEIRAOBJETIVO] [int] NOT NULL,
	[GIRODECARTEIRAOREALIZADO] [int] NOT NULL,
	[PORCENTAGEMATINGIMENTOGIRO] [nvarchar](max) NULL,
	[REVISAOFINANCEIRAOBJETIVO] [nvarchar](max) NULL,
	[REVISAOFINANCEIRAREALIZADO] [int] NOT NULL,
	[PORCENTAGEMATINGIMENTOREVISAO] [varchar](50) NULL,
	[CADASTROAPIOBJETIVO] [nvarchar](max) NULL,
	[CADASTROAPIREALIZADO] [float] NOT NULL,
	[PORCENTAGEMATINGIMENTOCADASTROAPI] [varchar](50) NULL,
	[MatriculaConsultor] [varchar](10) NULL,
 CONSTRAINT [PK_dbo.Qualitativo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Solicitacao]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Solicitacao](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Segmento] [varchar](50) NULL,
	[Lideranca] [varchar](50) NULL,
	[ConsultorMatriz] [varchar](50) NULL,
	[ConsultorPGP] [varchar](50) NULL,
	[NomeParticipante] [varchar](50) NOT NULL,
	[CPF] [varchar](11) NOT NULL,
	[SaldoPrevidencia] [decimal](18, 2) NOT NULL,
	[ValorPrevistoSaida] [decimal](18, 2) NOT NULL,
	[NomeEntidade] [varchar](60) NOT NULL,
	[DataInicioProcesso] [datetime] NOT NULL,
	[PrazoAtendimento] [datetime] NULL,
	[DataRef] [datetime] NULL,
	[CodIdentificadorProcesso] [varchar](50) NOT NULL,
	[CodIdentificadorProposta] [varchar](50) NOT NULL,
	[SUSEPCedente] [varchar](50) NULL,
	[SUSEPCessionaria] [varchar](50) NULL,
	[CIDTFDCNPJCedente] [varchar](50) NULL,
	[CIDTFDCNPJCessionaria] [varchar](50) NULL,
	[StatusId] [int] NOT NULL,
	[MotivoId] [int] NULL,
	[SubMotivoId] [int] NULL,
	[MatriculaConsultor] [varchar](10) NOT NULL,
	[ValorRetido] [decimal](18, 2) NULL,
	[Observacao] [varchar](1000) NULL,
	[Agencia] [int] NULL,
	[Conta] [int] NULL,
	[DataConclusao] [datetime] NULL,
	[DataInclusao] [datetime] NULL,
	[SubStatusId] [int] NULL,
	[PrazoFinal] [datetime] NULL,
	[ContatoAgencia] [varchar](50) NULL,
	[DescricaoTipoSolicitacao] [varchar](100) NULL,
	[CodigoIdentificadorAgenciaBRA] [int] NULL,
 CONSTRAINT [PK_Solicitacao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](50) NOT NULL,
	[Evento] [varchar](50) NOT NULL,
 CONSTRAINT [PK__Status__3214EC07C92C459F] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubStatus]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descricao] [varchar](50) NOT NULL,
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK_SubStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TED]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TED](
	[Id] [int] NOT NULL,
	[Data] [datetime] NOT NULL,
	[Agencia] [varchar](10) NOT NULL,
	[Conta] [varchar](10) NOT NULL,
	[NomeCliente] [varchar](100) NULL,
	[MatriculaConsultor] [varchar](30) NULL,
	[NomeConsultor] [varchar](50) NULL,
	[MatriculaSupervisor] [varchar](10) NULL,
	[NomeSupervisor] [varchar](50) NULL,
	[Area] [varchar](20) NOT NULL,
	[Valor] [decimal](18, 2) NOT NULL,
	[ValorAplicado] [decimal](18, 2) NULL,
	[MotivoId] [int] NULL,
	[StatusId] [int] NOT NULL,
	[Notificado] [bit] NOT NULL,
	[MotivoTedId] [int] NULL,
	[OutrasInstId] [int] NULL,
	[Observacao] [varchar](1000) NULL,
	[CpfCnpj] [nvarchar](20) NULL,
 CONSTRAINT [PK_TED] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TEDFaixaEquipe]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TEDFaixaEquipe](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Equipe] [varchar](50) NOT NULL,
	[De] [decimal](18, 2) NOT NULL,
	[Ate] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TedsAplicacoes]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TedsAplicacoes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTed] [int] NOT NULL,
	[IdProduto] [int] NOT NULL,
	[ValorAplicado] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_TedsAplicacoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_TedsAplicacoes] UNIQUE NONCLUSTERED 
(
	[IdProduto] ASC,
	[IdTed] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TedsContatos]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TedsContatos](
	[IdTed] [int] NOT NULL,
	[ContatouCliente] [bit] NULL,
	[ContatouGerente] [bit] NULL,
	[GerenteSolicitouNaoAtuacao] [bit] NULL,
	[GerenteInvestimentoAtuou] [bit] NULL,
	[EspecialistaAtuou] [bit] NULL,
	[ClienteLocalizado] [bit] NULL,
	[ClienteAceitaConsultoria] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTed] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TedsMotivoOutrasInst]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TedsMotivoOutrasInst](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Motivo] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TedsMotivoOutrasInst] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TedsMotivos]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TedsMotivos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Motivo] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TedsMotivos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TedsProdutos]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TedsProdutos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Produto] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TbTedsProdutos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemInvestFacil]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemInvestFacil](
	[id] [int] NOT NULL,
	[Agencia] [int] NOT NULL,
	[Conta] [int] NOT NULL,
	[DataInicio] [datetime] NULL,
	[DataFim] [datetime] NULL,
	[Status] [varchar](255) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[Nome] [varchar](50) NULL,
	[Matricula] [varchar](10) NOT NULL,
	[NomeSupervisor] [varchar](50) NULL,
	[MatriculaSupervisor] [varchar](10) NOT NULL,
	[Equipe] [varchar](100) NULL,
	[PerfilId] [int] NOT NULL,
	[NomeUsuario] [varchar](30) NOT NULL,
	[UsuarioId] [int] IDENTITY(1,1) NOT NULL,
	[NotificacaoEvento] [bit] NOT NULL,
	[NotificacaoPipeline] [bit] NOT NULL,
 CONSTRAINT [PK__Usuario__2B3DE7B851485A74] PRIMARY KEY CLUSTERED 
(
	[UsuarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vencimento]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vencimento](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Cod_Agencia] [int] NOT NULL,
	[Cod_Conta_Corrente] [int] NOT NULL,
	[Dt_Vecto_Contratado] [datetime] NOT NULL,
	[Nm_Cliente_Contraparte] [varchar](100) NOT NULL,
	[Perc_Indexador] [float] NOT NULL,
	[Nome_produto_sistema_origem] [varchar](100) NOT NULL,
	[SALDO_ATUAL] [decimal](18, 2) NOT NULL,
	[StatusId] [int] NULL,
 CONSTRAINT [PK_dbo.VencimentosCompleto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WindowsServiceConfig]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WindowsServiceConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Tarefa] [varchar](30) NOT NULL,
	[CaminhoOrigem] [varchar](200) NULL,
	[UltimaExecucao] [datetime] NOT NULL,
	[IntervaloExecucao] [varchar](20) NOT NULL,
	[DataUltimaModificacao] [datetime] NULL,
	[PadraoPesquisa] [varchar](50) NULL,
	[Acrescentar] [bit] NULL,
	[UltimoArquivo] [varchar](150) NOT NULL,
	[EmExecucao] [bit] NOT NULL,
	[NomeArquivo] [varchar](100) NULL,
	[TeveFalha] [bit] NOT NULL,
 CONSTRAINT [PK__WindowsS__3214EC07DF7D32AC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AplicacaoResgate] ADD  DEFAULT ((0)) FOR [Notificado]
GO
ALTER TABLE [dbo].[CaptacaoLiquida] ADD  CONSTRAINT [DF__CaptacaoL__Valor__5614BF03]  DEFAULT ((0)) FOR [ValorAplicacao]
GO
ALTER TABLE [dbo].[CaptacaoLiquida] ADD  CONSTRAINT [DF__CaptacaoL__Valor__5708E33C]  DEFAULT ((0)) FOR [ValorResgate]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_TOTAL]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_CORRETORA_BRA]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_CORRETORA_AGORA]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_CORRETORA]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_PREVIDENCIA]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_POUPANCA]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_INVESTS]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_DAV_20K]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_COMPROMISSADAS]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_ISENTOS]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_LF]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_CDB]
GO
ALTER TABLE [dbo].[Clusterizacoes] ADD  DEFAULT ((0)) FOR [SALDO_FUNDOS]
GO
ALTER TABLE [dbo].[Evento] ADD  CONSTRAINT [DF_Evento_Notificado]  DEFAULT ((0)) FOR [Notificado]
GO
ALTER TABLE [dbo].[Links] ADD  CONSTRAINT [DF_Links_Exibir]  DEFAULT ((1)) FOR [Exibir]
GO
ALTER TABLE [dbo].[Motivo] ADD  DEFAULT ((1)) FOR [EmUso]
GO
ALTER TABLE [dbo].[Notificacoes] ADD  DEFAULT ((0)) FOR [valor]
GO
ALTER TABLE [dbo].[Pipeline] ADD  CONSTRAINT [DF_Pipeline_Notificado]  DEFAULT ((0)) FOR [Notificado]
GO
ALTER TABLE [dbo].[Solicitacao] ADD  DEFAULT (getdate()) FOR [DataInclusao]
GO
ALTER TABLE [dbo].[SubMotivo] ADD  DEFAULT ((1)) FOR [EmUso]
GO
ALTER TABLE [dbo].[TEDFaixaEquipe] ADD  DEFAULT ((0)) FOR [De]
GO
ALTER TABLE [dbo].[TEDFaixaEquipe] ADD  DEFAULT ((0)) FOR [Ate]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF__Usuario__NomeUsu__59FA5E80]  DEFAULT ('username') FOR [NomeUsuario]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF__Usuario__Notific__2FCF1A8A]  DEFAULT ((1)) FOR [NotificacaoEvento]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF__Usuario__Notific__30C33EC3]  DEFAULT ((1)) FOR [NotificacaoPipeline]
GO
ALTER TABLE [dbo].[WindowsServiceConfig] ADD  CONSTRAINT [DF__WindowsSe__Acres__0A688BB1]  DEFAULT ((0)) FOR [Acrescentar]
GO
ALTER TABLE [dbo].[WindowsServiceConfig] ADD  DEFAULT ('') FOR [UltimoArquivo]
GO
ALTER TABLE [dbo].[WindowsServiceConfig] ADD  DEFAULT ((0)) FOR [EmExecucao]
GO
ALTER TABLE [dbo].[WindowsServiceConfig] ADD  DEFAULT ((0)) FOR [TeveFalha]
GO
ALTER TABLE [dbo].[AplicResgateContatos]  WITH CHECK ADD  CONSTRAINT [FK_AplicResgateContatos_IdAplicacaoResgate] FOREIGN KEY([IdAplicResgate])
REFERENCES [dbo].[AplicacaoResgate] ([Id])
GO
ALTER TABLE [dbo].[AplicResgateContatos] CHECK CONSTRAINT [FK_AplicResgateContatos_IdAplicacaoResgate]
GO
ALTER TABLE [dbo].[Notificacoes]  WITH CHECK ADD  CONSTRAINT [FK__Notificac__IdUsu__2EDAF651] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([UsuarioId])
GO
ALTER TABLE [dbo].[Notificacoes] CHECK CONSTRAINT [FK__Notificac__IdUsu__2EDAF651]
GO
ALTER TABLE [dbo].[Pipeline]  WITH CHECK ADD  CONSTRAINT [FK_Pipeline_Motivo] FOREIGN KEY([MotivoId])
REFERENCES [dbo].[Motivo] ([Id])
GO
ALTER TABLE [dbo].[Pipeline] CHECK CONSTRAINT [FK_Pipeline_Motivo]
GO
ALTER TABLE [dbo].[Pipeline]  WITH CHECK ADD  CONSTRAINT [FK_Pipeline_Origem] FOREIGN KEY([OrigemId])
REFERENCES [dbo].[Origem] ([Id])
GO
ALTER TABLE [dbo].[Pipeline] CHECK CONSTRAINT [FK_Pipeline_Origem]
GO
ALTER TABLE [dbo].[Pipeline]  WITH CHECK ADD  CONSTRAINT [FK_Pipeline_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[Pipeline] CHECK CONSTRAINT [FK_Pipeline_Status]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH NOCHECK ADD  CONSTRAINT [FK_Solicitacao_Motivo] FOREIGN KEY([MotivoId])
REFERENCES [dbo].[Motivo] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Motivo]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH NOCHECK ADD  CONSTRAINT [FK_Solicitacao_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_Status]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH NOCHECK ADD  CONSTRAINT [FK_Solicitacao_SubMotivo] FOREIGN KEY([SubMotivoId])
REFERENCES [dbo].[SubMotivo] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_SubMotivo]
GO
ALTER TABLE [dbo].[Solicitacao]  WITH NOCHECK ADD  CONSTRAINT [FK_Solicitacao_SubStatus] FOREIGN KEY([SubStatusId])
REFERENCES [dbo].[SubStatus] ([Id])
GO
ALTER TABLE [dbo].[Solicitacao] CHECK CONSTRAINT [FK_Solicitacao_SubStatus]
GO
ALTER TABLE [dbo].[SubMotivo]  WITH CHECK ADD  CONSTRAINT [FK_SubMotivo_Motivo] FOREIGN KEY([MotivoId])
REFERENCES [dbo].[Motivo] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubMotivo] CHECK CONSTRAINT [FK_SubMotivo_Motivo]
GO
ALTER TABLE [dbo].[SubStatus]  WITH CHECK ADD  CONSTRAINT [FK_SubStatus_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[SubStatus] CHECK CONSTRAINT [FK_SubStatus_Status]
GO
ALTER TABLE [dbo].[TED]  WITH NOCHECK ADD FOREIGN KEY([MotivoTedId])
REFERENCES [dbo].[TedsMotivos] ([Id])
GO
ALTER TABLE [dbo].[TED]  WITH NOCHECK ADD FOREIGN KEY([OutrasInstId])
REFERENCES [dbo].[TedsMotivoOutrasInst] ([Id])
GO
ALTER TABLE [dbo].[TED]  WITH NOCHECK ADD  CONSTRAINT [FK_TED_MotivosTeds] FOREIGN KEY([MotivoId])
REFERENCES [dbo].[Motivo] ([Id])
GO
ALTER TABLE [dbo].[TED] CHECK CONSTRAINT [FK_TED_MotivosTeds]
GO
ALTER TABLE [dbo].[TED]  WITH NOCHECK ADD  CONSTRAINT [FK_TED_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[TED] CHECK CONSTRAINT [FK_TED_Status]
GO
ALTER TABLE [dbo].[TedsAplicacoes]  WITH CHECK ADD  CONSTRAINT [FK_TbTedsAplicacoes_TbTedsProdutos] FOREIGN KEY([IdProduto])
REFERENCES [dbo].[TedsProdutos] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[TedsAplicacoes] CHECK CONSTRAINT [FK_TbTedsAplicacoes_TbTedsProdutos]
GO
ALTER TABLE [dbo].[TedsAplicacoes]  WITH CHECK ADD  CONSTRAINT [FK_TedsAplicacoes_TED] FOREIGN KEY([IdTed])
REFERENCES [dbo].[TED] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TedsAplicacoes] CHECK CONSTRAINT [FK_TedsAplicacoes_TED]
GO
ALTER TABLE [dbo].[TedsContatos]  WITH CHECK ADD  CONSTRAINT [FK_TedsContatos_TbTED] FOREIGN KEY([IdTed])
REFERENCES [dbo].[TED] ([Id])
GO
ALTER TABLE [dbo].[TedsContatos] CHECK CONSTRAINT [FK_TedsContatos_TbTED]
GO
ALTER TABLE [dbo].[Usuario]  WITH NOCHECK ADD  CONSTRAINT [FK__Usuario__PerfilI__619B8048] FOREIGN KEY([PerfilId])
REFERENCES [dbo].[Perfil] ([PerfilId])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK__Usuario__PerfilI__619B8048]
GO
ALTER TABLE [dbo].[Vencimento]  WITH NOCHECK ADD  CONSTRAINT [FK_Vencimento_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[Vencimento] CHECK CONSTRAINT [FK_Vencimento_Status]
GO
/****** Object:  StoredProcedure [dbo].[sp_CapLiqCanDin]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[sp_CapLiqCanDin]
as
begin
   --Agrupamento por agencia, conta e produto para caminho do dinehro 
   if(OBJECT_ID(N'dbo.CamDinResumo', N'U') is null)
	begin
		select agencia, Conta, produto, MatriculaConsultor, 
		Sum(VL_DINHEIRO_NOVO)'VL_DINHEIRO_NOVO', 
		Sum(VL_RESG_CDB)'VL_RESG_CDB', 
		Sum(VL_RESG_ISENTOS)'VL_RESG_ISENTOS', 
		Sum(VL_RESG_COMPROMISSADAS)'VL_RESG_COMPROMISSADAS', 
		Sum(VL_RESG_LF)'VL_RESG_LF', 
		Sum(VL_RESG_FUNDOS)'VL_RESG_FUNDOS', 
		Sum(VL_RESG_CORRET)'VL_RESG_CORRET', 
		Sum(VL_RESG_PREVI)'VL_RESG_PREVI'
		into CamDinResumo
		from CaminhoDinheiro 
		group by Agencia , Conta, Produto, MatriculaConsultor
	end
	

	 if(OBJECT_ID(N'dbo.CapLiqResumo', N'U') is null)
	begin
		--Agrupamento por agencia, conta e produto para captação liquida
		select agencia, Conta, produto, MatriculaConsultor,  
		Sum(ValorAplicacao)'TotalAplicacao', 
		Sum(valorResgate)'TotalResgate', 
		(Sum(ValorAplicacao) - Sum(valorResgate))'CapLiq'
		into CapLiqResumo
		from CaptacaoLiquida 
		group by Agencia , Conta, Produto, MatriculaConsultor
	end

	select ca.*, u.MatriculaSupervisor, u.Nome 'Especialista' ,cam.VL_DINHEIRO_NOVO, cam.VL_RESG_CDB, cam.VL_RESG_ISENTOS,
	cam.VL_RESG_COMPROMISSADAS, cam.VL_RESG_LF, cam.VL_RESG_FUNDOS, cam.VL_RESG_CORRET,
	cam.VL_RESG_PREVI from CapLiqResumo ca
	inner join CamDinResumo cam
	on ca.Agencia = cam.Agencia and ca.Conta = cam.Conta and ca.Produto = cam.Produto
	inner join Usuario u on ca.MatriculaConsultor = u.Matricula
	

	drop table CapLiqResumo

	drop table CamDinResumo
end
GO
/****** Object:  StoredProcedure [dbo].[sp_CapLiqResumo]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[sp_CapLiqResumo] 
  @mesDataBase varchar(10) = null
as
begin

	delete from CaptacaoLiquidaResumo

	if(@mesDataBase) is null
		set @mesDataBase = FORMAT(getdate(), 'MMM yyyy');

	insert into CaptacaoLiquidaResumo ([MatriculaCordenador], [MatriculaConsultor], [ProdutoMacro], [VL_NET]) 
	select MatriculaCordenador, MatriculaConsultor, Produto 'ProdutoMacro', sum(valorNET)'VL_NET' from CaptacaoLiquida
	where MesDataBase = @mesDataBase
	group by MatriculaConsultor, Produto, MatriculaCordenador
	order by MatriculaConsultor;

end

GO
/****** Object:  StoredProcedure [dbo].[sp_EXPURGO_AplicacaoResgate]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[sp_EXPURGO_AplicacaoResgate]
(                                         
        @IDS IDLIST READONLY      
)                                         
AS

SET NOCOUNT ON;

BEGIN TRY
        BEGIN TRANSACTION

        DECLARE @Results TABLE(id INTEGER)

        DELETE 
        FROM AplicacaoResgate 
        WHERE Id IN (SELECT ID FROM @IDS)        

        COMMIT TRANSACTION
END TRY
BEGIN CATCH
        PRINT ERROR_MESSAGE();

        ROLLBACK TRANSACTION
        THROW; -- Rethrow exception
END CATCH

GO
/****** Object:  StoredProcedure [dbo].[sp_EXPURGO_LOGS]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[sp_EXPURGO_LOGS]
(                                         
        @IDS IDLIST READONLY      
)                                         
AS

SET NOCOUNT ON;

BEGIN TRY
        BEGIN TRANSACTION

        DECLARE @Results TABLE(id INTEGER)

        DELETE 
        FROM [Log] 
        WHERE Id IN (SELECT ID FROM @IDS)        

        COMMIT TRANSACTION
END TRY
BEGIN CATCH
        PRINT ERROR_MESSAGE();

        ROLLBACK TRANSACTION
        THROW; -- Rethrow exception
END CATCH


GO
/****** Object:  StoredProcedure [dbo].[sp_EXPURGO_Pipeline]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[sp_EXPURGO_Pipeline]
(                                         
        @IDS IDLIST READONLY      
)                                         
AS

SET NOCOUNT ON;

BEGIN TRY
        BEGIN TRANSACTION

        DECLARE @Results TABLE(id INTEGER)

        DELETE 
        FROM Pipeline 
        WHERE Id IN (SELECT ID FROM @IDS)        

        COMMIT TRANSACTION
END TRY
BEGIN CATCH
        PRINT ERROR_MESSAGE();

        ROLLBACK TRANSACTION
        THROW; -- Rethrow exception
END CATCH

GO
/****** Object:  StoredProcedure [dbo].[sp_EXPURGO_TED]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_EXPURGO_TED]
(                                         
        @IDS IDLIST READONLY      
)                                         
AS

SET NOCOUNT ON;

BEGIN TRY
        BEGIN TRANSACTION

        DECLARE @Results TABLE(id INTEGER)

        DELETE 
        FROM TED 
        WHERE Id IN (SELECT ID FROM @IDS)        

        COMMIT TRANSACTION
END TRY
BEGIN CATCH
        PRINT ERROR_MESSAGE();

        ROLLBACK TRANSACTION
        THROW; -- Rethrow exception
END CATCH
GO
/****** Object:  StoredProcedure [dbo].[sp_EXPURGO_Vencimentos]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[sp_EXPURGO_Vencimentos]
(                                         
        @IDS IDLIST READONLY      
)                                         
AS

SET NOCOUNT ON;

BEGIN TRY
        BEGIN TRANSACTION

        DECLARE @Results TABLE(id INTEGER)

        DELETE 
        FROM Vencimento 
        WHERE Id IN (SELECT ID FROM @IDS)        

        COMMIT TRANSACTION
END TRY
BEGIN CATCH
        PRINT ERROR_MESSAGE();

        ROLLBACK TRANSACTION
        THROW; -- Rethrow exception
END CATCH

GO
/****** Object:  StoredProcedure [dbo].[sp_ObterCapLiq]    Script Date: 29/07/2020 17:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_ObterCapLiq] 
@matriculaConsultor int null,
@matriculaCord int null,   
@mesDataBase varchar(10) = null
as

begin
if(@mesDataBase) is null
		set @mesDataBase = FORMAT(getdate(), 'MMM yyyy');

if(@matriculaConsultor is not null and @matriculaConsultor <> '')
	  SELECT Diretoria, GerenciaRegional, Produto,MatriculaConsultor, Consultor, sum(valorNET)'ValorNET'
	  FROM [BD_PGP_WEB_DESENV].[dbo].[CaptacaoLiquida]
	  where MatriculaConsultor = @matriculaConsultor and  MesDataBase = @mesDataBase 
	  group by Diretoria, GerenciaRegional, Produto, MatriculaConsultor,Consultor

else if(@matriculaCord is not null and @matriculaCord <> '')
      SELECT Diretoria, GerenciaRegional, Produto,MatriculaConsultor, Consultor, MatriculaCordenador , CordenadorPGP, sum(valorNET)'ValorNET'
	  FROM [BD_PGP_WEB_DESENV].[dbo].[CaptacaoLiquida]
	  where MatriculaCordenador = @matriculaCord and  MesDataBase = @mesDataBase 
	  group by Diretoria, GerenciaRegional, Produto, MatriculaConsultor, MatriculaCordenador, Consultor, CordenadorPGP

ELSE
      SELECT Diretoria, GerenciaRegional, Produto,MatriculaConsultor, Consultor, MatriculaCordenador , CordenadorPGP, sum(valorNET)'ValorNET'
	  FROM [BD_PGP_WEB_DESENV].[dbo].[CaptacaoLiquida]
	  where MesDataBase = @mesDataBase
	  group by Diretoria, GerenciaRegional, Produto, MatriculaConsultor, MatriculaCordenador, Consultor, CordenadorPGP

end
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Clusterizacoes"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 267
               Right = 282
            End
            DisplayFlags = 280
            TopColumn = 19
         End
         Begin Table = "Encarteiramento"
            Begin Extent = 
               Top = 5
               Left = 480
               Bottom = 307
               Right = 690
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwClusterTopTier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwClusterTopTier'
GO
