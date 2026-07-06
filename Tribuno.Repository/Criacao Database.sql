CREATE DATABASE Tribuno;
GO

USE Tribuno;
GO

CREATE TABLE Usuario
(
    Id INT IDENTITY(1,1) NOT NULL,
    Nome NVARCHAR(60) NOT NULL,
    LoginUsuario NVARCHAR(20) NOT NULL,
    Senha NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NULL,
    Ativo BIT NOT NULL,
    DataCadastro DATETIME2 NOT NULL DEFAULT(GETDATE()),
    DataAlteracao DATETIME2 NULL,

    CONSTRAINT PK_Usuario
        PRIMARY KEY (Id),

    CONSTRAINT UQ_Usuario_Login
        UNIQUE(LoginUsuario)
);
GO

CREATE TABLE Operacao
(
    IdOperacao INT IDENTITY(1,1) NOT NULL,
    IdUsuario INT NOT NULL,

    NomeOperacao NVARCHAR(30) NOT NULL,
    Descricao NVARCHAR(100) NULL,

    DataCadastro DATETIME2 NOT NULL DEFAULT(GETDATE()),
    DataAlteracao DATETIME2 NULL,

    TipoOperacao INT NOT NULL,
    TipoCalculo INT NOT NULL,

    CONSTRAINT PK_Operacao
        PRIMARY KEY (IdOperacao),

    CONSTRAINT FK_Operacao_Usuario
        FOREIGN KEY (IdUsuario)
        REFERENCES Usuario(Id)
);
GO

CREATE TABLE OperacaoParcelas
(
    IdParcela INT IDENTITY(1,1) NOT NULL,

    IdOperacao INT NOT NULL,

    NumeroParcela INT NOT NULL,

    ValorParcela DECIMAL(18,2) NOT NULL,

    DataVencimento DATETIME2 NOT NULL,
    DataInclusao DATETIME2 NOT NULL DEFAULT(GETDATE()),
    DataAlteracao DATETIME2 NULL,

    StatusParcela INT NOT NULL,

    CONSTRAINT PK_OperacaoParcelas
        PRIMARY KEY (IdParcela),

    CONSTRAINT FK_OperacaoParcelas_Operacao
        FOREIGN KEY (IdOperacao)
        REFERENCES Operacao(IdOperacao)
        ON DELETE CASCADE
);
GO

CREATE INDEX IX_Operacao_IdUsuario
ON Operacao(IdUsuario);

CREATE INDEX IX_OperacaoParcelas_IdOperacao
ON OperacaoParcelas(IdOperacao);

CREATE INDEX IX_Usuario_Login
ON Usuario(LoginUsuario);
GO