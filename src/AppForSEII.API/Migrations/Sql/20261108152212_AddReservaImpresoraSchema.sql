IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    ALTER TABLE [Materiales] DROP CONSTRAINT [FK_Materiales_Piezas3D_Pieza3DId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    DROP INDEX [IX_Materiales_Pieza3DId] ON [Materiales];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Materiales]') AND [c].[name] = N'Pieza3DId');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Materiales] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Materiales] DROP COLUMN [Pieza3DId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    CREATE TABLE [Impresoras3D] (
        [Id] int NOT NULL IDENTITY,
        [Nombre] nvarchar(100) NOT NULL,
        [Modelo] nvarchar(100) NOT NULL,
        [Tipo] nvarchar(50) NOT NULL,
        [Descripcion] nvarchar(1000) NOT NULL,
        [PrecioKilovatioHora] decimal(18,2) NOT NULL,
        [PrecioReserva] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_Impresoras3D] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    CREATE TABLE [PiezasMateriales] (
        [PiezaId] int NOT NULL,
        [MaterialId] int NOT NULL,
        CONSTRAINT [PK_PiezasMateriales] PRIMARY KEY ([PiezaId], [MaterialId]),
        CONSTRAINT [FK_PiezasMateriales_Materiales_MaterialId] FOREIGN KEY ([MaterialId]) REFERENCES [Materiales] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PiezasMateriales_Piezas3D_PiezaId] FOREIGN KEY ([PiezaId]) REFERENCES [Piezas3D] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    CREATE TABLE [ReservasImpresora] (
        [Id] int NOT NULL IDENTITY,
        [FechaReserva] datetime2 NOT NULL,
        [NombreCliente] nvarchar(50) NOT NULL,
        [ApellidosCliente] nvarchar(50) NOT NULL,
        [DireccionFacturacion] nvarchar(200) NOT NULL,
        [PrecioTotal] decimal(18,2) NOT NULL,
        [MetodoPago] int NOT NULL,
        [ClienteId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_ReservasImpresora] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ReservasImpresora_AspNetUsers_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    CREATE TABLE [LineasReserva] (
        [Id] int NOT NULL IDENTITY,
        [TiempoReserva] int NOT NULL,
        [PrecioSubtotal] decimal(18,2) NOT NULL,
        [Impresora3DId] int NOT NULL,
        [ReservaImpresoraId] int NOT NULL,
        CONSTRAINT [PK_LineasReserva] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_LineasReserva_Impresoras3D_Impresora3DId] FOREIGN KEY ([Impresora3DId]) REFERENCES [Impresoras3D] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_LineasReserva_ReservasImpresora_ReservaImpresoraId] FOREIGN KEY ([ReservaImpresoraId]) REFERENCES [ReservasImpresora] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    CREATE INDEX [IX_LineasReserva_Impresora3DId] ON [LineasReserva] ([Impresora3DId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    CREATE INDEX [IX_LineasReserva_ReservaImpresoraId] ON [LineasReserva] ([ReservaImpresoraId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    CREATE INDEX [IX_PiezasMateriales_MaterialId] ON [PiezasMateriales] ([MaterialId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    CREATE INDEX [IX_ReservasImpresora_ClienteId] ON [ReservasImpresora] ([ClienteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261108152212_AddReservaImpresoraSchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261108152212_AddReservaImpresoraSchema', N'10.0.11');
END;

COMMIT;
GO

