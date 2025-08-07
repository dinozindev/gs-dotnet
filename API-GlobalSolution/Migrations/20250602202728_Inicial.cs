using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_GlobalSolution.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        const string bairro = "BAIRRO";
        const string tipoDesastre = "TIPO_DESASTRE";
        const string usuario = "USUARIO";
        const string endereco = "ENDERECO";
        const string sensor = "SENSOR";
        const string postagem = "POSTAGEM";
        const string alerta = "ALERTA";
        const string comentario = "COMENTARIO";
        const string number10 = "NUMBER(10)";
        const string oracleIdentity = "Oracle:Identity";
        const string incrementBy1 = "START WITH 1 INCREMENT BY 1";
        const string nvarchar255 = "NVARCHAR2(255)";
        const string nvarchar100 = "NVARCHAR2(100)";
        const string nvarchar50 = "NVARCHAR2(50)";
        const string timestamp7 = "TIMESTAMP(7)";
        
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: bairro,
                columns: table => new
                {
                    ID_BAIRRO = table.Column<int>(type: number10, nullable: false)
                        .Annotation(oracleIdentity, incrementBy1),
                    NOME_BAIRRO = table.Column<string>(type: nvarchar100, maxLength: 100, nullable: false),
                    ZONA_BAIRRO = table.Column<string>(type: nvarchar50, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BAIRRO", x => x.ID_BAIRRO);
                });

            migrationBuilder.Sql(
                "ALTER TABLE \"BAIRRO\" ADD CONSTRAINT CHK_ZONA_BAIRRO CHECK (\"ZONA_BAIRRO\" IN ('Zona Sul', 'Zona Norte', 'Zona Leste', 'Zona Oeste', 'Zona Central'))");
            
            migrationBuilder.CreateTable(
                name: tipoDesastre,
                columns: table => new
                {
                    ID_DESASTRE = table.Column<int>(type: number10, nullable: false)
                        .Annotation(oracleIdentity, incrementBy1),
                    NOME_DESASTRE = table.Column<string>(type: nvarchar100, maxLength: 100, nullable: false),
                    DESCRICAO_DESASTRE = table.Column<string>(type: nvarchar255, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPO_DESASTRE", x => x.ID_DESASTRE);
                });
                
            migrationBuilder.Sql(
                "ALTER TABLE \"TIPO_DESASTRE\" ADD CONSTRAINT CHK_NOME_DESASTRE CHECK (\"NOME_DESASTRE\" IN ('Enchente', 'Inundação', 'Deslizamento', 'Queimada', 'Incêndio Florestal', 'Tempestade', 'Vendaval'))"
            );
            
            migrationBuilder.CreateTable(
                name: usuario,
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: number10, nullable: false)
                        .Annotation(oracleIdentity, incrementBy1),
                    NOME_USUARIO = table.Column<string>(type: nvarchar100, maxLength: 100, nullable: false),
                    EMAIL_USUARIO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    SENHA_USUARIO = table.Column<string>(type: nvarchar50, maxLength: 50, nullable: false),
                    TELEFONE_USUARIO = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                    TIPO_USUARIO = table.Column<string>(type: nvarchar50, maxLength: 50, nullable: false, defaultValue: "Usuário")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID_USUARIO);
                });
            
                migrationBuilder.Sql(
                    "ALTER TABLE \"USUARIO\" ADD CONSTRAINT CHK_TIPO_USUARIO CHECK (\"TIPO_USUARIO\" IN ('Usuário', 'Funcionário'))"
                );

            migrationBuilder.CreateTable(
                name: endereco,
                columns: table => new
                {
                    ID_ENDERECO = table.Column<int>(type: number10, nullable: false)
                        .Annotation(oracleIdentity, incrementBy1),
                    LOGRADOURO_ENDERECO = table.Column<string>(type: nvarchar255, maxLength: 255, nullable: true),
                    NUMERO_ENDERECO = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: true),
                    COMPLEMENTO_ENDERECO = table.Column<string>(type: nvarchar255, maxLength: 255, nullable: true),
                    CEP_ENDERECO = table.Column<string>(type: "NVARCHAR2(9)", maxLength: 9, nullable: true),
                    BAIRRO_ID_BAIRRO = table.Column<int>(type: number10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENDERECO", x => x.ID_ENDERECO);
                    table.ForeignKey(
                        name: "ENDERECO_BAIRRO_FK",
                        column: x => x.BAIRRO_ID_BAIRRO,
                        principalTable: bairro,
                        principalColumn: "ID_BAIRRO",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateTable(
                name: sensor,
                columns: table => new
                {
                    ID_SENSOR = table.Column<int>(type: number10, nullable: false)
                        .Annotation(oracleIdentity, incrementBy1),
                    NOME_SENSOR = table.Column<string>(type: nvarchar100, maxLength: 100, nullable: false),
                    TIPO_SENSOR = table.Column<string>(type: nvarchar50, maxLength: 50, nullable: false),
                    BAIRRO_ID_BAIRRO = table.Column<int>(type: number10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SENSOR", x => x.ID_SENSOR);
                    table.ForeignKey(
                        name: "SENSOR_BAIRRO_FK",
                        column: x => x.BAIRRO_ID_BAIRRO,
                        principalTable: bairro,
                        principalColumn: "ID_BAIRRO");
                });

            migrationBuilder.CreateTable(
                name: "POSTAGEM",
                columns: table => new
                {
                    ID_POSTAGEM = table.Column<int>(type: number10, nullable: false)
                        .Annotation(oracleIdentity, incrementBy1),
                    TITULO_POSTAGEM = table.Column<string>(type: nvarchar100, maxLength: 100, nullable: false),
                    DESCRICAO_POSTAGEM = table.Column<string>(type: nvarchar255, maxLength: 255, nullable: false),
                    DATA_POSTAGEM = table.Column<DateTime>(type: timestamp7, nullable: false),
                    TIPO_POSTAGEM = table.Column<string>(type: nvarchar50, maxLength: 50, nullable: false, defaultValue: "Usuário"),
                    STATUS_POSTAGEM = table.Column<string>(type: nvarchar50, maxLength: 50, nullable: false, defaultValue: "Ativo"),
                    USUARIO_ID_USUARIO = table.Column<int>(type: number10, nullable: false),
                    ENDERECO_ID_ENDERECO = table.Column<int>(type: number10, nullable: false),
                    TIPO_DESASTRE_ID_DESASTRE = table.Column<int>(type: number10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POSTAGEM", x => x.ID_POSTAGEM);
                    table.ForeignKey(
                        name: "POSTAGEM_ENDERECO_FK",
                        column: x => x.ENDERECO_ID_ENDERECO,
                        principalTable: endereco,
                        principalColumn: "ID_ENDERECO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "POSTAGEM_TIPO_DESASTRE_FK",
                        column: x => x.TIPO_DESASTRE_ID_DESASTRE,
                        principalTable: tipoDesastre,
                        principalColumn: "ID_DESASTRE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "POSTAGEM_USUARIO_FK",
                        column: x => x.USUARIO_ID_USUARIO,
                        principalTable: usuario,
                        principalColumn: "ID_USUARIO");
                });
            
            migrationBuilder.Sql(
                    "ALTER TABLE \"POSTAGEM\" ADD CONSTRAINT CHK_STATUS_POSTAGEM CHECK (\"STATUS_POSTAGEM\" IN ('Ativo', 'Resolvido', 'Descartado'))"
                );

                migrationBuilder.Sql(
                    "ALTER TABLE \"POSTAGEM\" ADD CONSTRAINT CHK_TIPO_POSTAGEM CHECK (\"TIPO_POSTAGEM\" IN ('Usuário', 'Governo'))"
                );
                
            migrationBuilder.CreateTable(
                name: alerta,
                columns: table => new
                {
                    ID_ALERTA = table.Column<int>(type: number10, nullable: false)
                        .Annotation(oracleIdentity, incrementBy1),
                    NIVEL_RISCO = table.Column<string>(type: nvarchar50, maxLength: 50, nullable: false),
                    DATA_ALERTA = table.Column<DateTime>(type: timestamp7, nullable: false),
                    DESCRICAO_ALERTA = table.Column<string>(type: nvarchar255, maxLength: 255, nullable: true),
                    STATUS_ALERTA = table.Column<string>(type: nvarchar50, maxLength: 50, nullable: false, defaultValue: "Ativo"),
                    SENSOR_ID_SENSOR = table.Column<int>(type: number10, nullable: false),
                    TIPO_DESASTRE_ID_DESASTRE = table.Column<int>(type: number10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERTA", x => x.ID_ALERTA);
                    table.ForeignKey(
                        name: "ALERTA_SENSOR_FK",
                        column: x => x.SENSOR_ID_SENSOR,
                        principalTable: sensor,
                        principalColumn: "ID_SENSOR",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ALERTA_TIPO_DESASTRE_FK",
                        column: x => x.TIPO_DESASTRE_ID_DESASTRE,
                        principalTable: tipoDesastre,
                        principalColumn: "ID_DESASTRE",
                        onDelete: ReferentialAction.Cascade);
                });
            
             migrationBuilder.Sql(
                        "ALTER TABLE \"ALERTA\" ADD CONSTRAINT CHK_NIVEL_RISCO CHECK (\"NIVEL_RISCO\" IN ('Atenção', 'Alerta', 'Emergência'))"
                    );

                    migrationBuilder.Sql(
                        "ALTER TABLE \"ALERTA\" ADD CONSTRAINT CHK_STATUS_ALERTA CHECK (\"STATUS_ALERTA\" IN ('Ativo', 'Inativo', 'Resolvido'))"
                    );
                    
            migrationBuilder.CreateTable(
                name: comentario,
                columns: table => new
                {
                    ID_COMENTARIO = table.Column<int>(type: number10, nullable: false)
                        .Annotation(oracleIdentity, incrementBy1),
                    TEXTO_COMENTARIO = table.Column<string>(type: nvarchar255, maxLength: 255, nullable: false),
                    DATA_COMENTARIO = table.Column<DateTime>(type: timestamp7, nullable: false),
                    POSTAGEM_ID_POSTAGEM = table.Column<int>(type: number10, nullable: false),
                    USUARIO_ID_USUARIO = table.Column<int>(type: number10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMENTARIO", x => x.ID_COMENTARIO);
                    table.ForeignKey(
                        name: "COMENTARIO_POSTAGEM_FK",
                        column: x => x.POSTAGEM_ID_POSTAGEM,
                        principalTable: postagem,
                        principalColumn: "ID_POSTAGEM",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "COMENTARIO_USUARIO_FK",
                        column: x => x.USUARIO_ID_USUARIO,
                        principalTable: usuario,
                        principalColumn: "ID_USUARIO");
                });
            
            migrationBuilder.CreateTable(
                name: "CONFIRMA_POSTAGEM",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: number10, nullable: false),
                    ID_POSTAGEM = table.Column<int>(type: number10, nullable: false),
                    DATA_CONFIRMA = table.Column<DateTime>(type: timestamp7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONFIRMA_POSTAGEM", x => new { x.ID_USUARIO, x.ID_POSTAGEM });
                    table.ForeignKey(
                        name: "CONFIRMA_POSTAGEM_POSTAGEM_FK",
                        column: x => x.ID_POSTAGEM,
                        principalTable: postagem,
                        principalColumn: "ID_POSTAGEM",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "CONFIRMA_POSTAGEM_USUARIO_FK",
                        column: x => x.ID_USUARIO,
                        principalTable: usuario,
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALERTA_SENSOR_ID_SENSOR",
                table: alerta,
                column: "SENSOR_ID_SENSOR");

            migrationBuilder.CreateIndex(
                name: "IX_ALERTA_TIPO_DESASTRE_ID_DESASTRE",
                table: alerta,
                column: "TIPO_DESASTRE_ID_DESASTRE");

            migrationBuilder.CreateIndex(
                name: "IX_BAIRRO_NOME_BAIRRO",
                table: bairro,
                column: "NOME_BAIRRO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_COMENTARIO_POSTAGEM_ID_POSTAGEM",
                table: comentario,
                column: "POSTAGEM_ID_POSTAGEM");

            migrationBuilder.CreateIndex(
                name: "IX_COMENTARIO_USUARIO_ID_USUARIO",
                table: comentario,
                column: "USUARIO_ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_CONFIRMA_POSTAGEM_ID_POSTAGEM",
                table: "CONFIRMA_POSTAGEM",
                column: "ID_POSTAGEM");

            migrationBuilder.CreateIndex(
                name: "IX_ENDERECO_BAIRRO_ID_BAIRRO",
                table: endereco,
                column: "BAIRRO_ID_BAIRRO");

            migrationBuilder.CreateIndex(
                name: "IX_POSTAGEM_ENDERECO_ID_ENDERECO",
                table: postagem,
                column: "ENDERECO_ID_ENDERECO");

            migrationBuilder.CreateIndex(
                name: "IX_POSTAGEM_TIPO_DESASTRE_ID_DESASTRE",
                table: postagem,
                column: "TIPO_DESASTRE_ID_DESASTRE");

            migrationBuilder.CreateIndex(
                name: "IX_POSTAGEM_USUARIO_ID_USUARIO",
                table: postagem,
                column: "USUARIO_ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_SENSOR_BAIRRO_ID_BAIRRO",
                table: sensor,
                column: "BAIRRO_ID_BAIRRO");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_EMAIL_USUARIO",
                table: usuario,
                column: "EMAIL_USUARIO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_TELEFONE_USUARIO",
                table: usuario,
                column: "TELEFONE_USUARIO",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: alerta);

            migrationBuilder.DropTable(
                name: comentario);

            migrationBuilder.DropTable(
                name: "CONFIRMA_POSTAGEM");

            migrationBuilder.DropTable(
                name: sensor);

            migrationBuilder.DropTable(
                name: postagem);

            migrationBuilder.DropTable(
                name: endereco);

            migrationBuilder.DropTable(
                name: tipoDesastre);

            migrationBuilder.DropTable(
                name: usuario);

            migrationBuilder.DropTable(
                name: bairro);
        }
    }
}
